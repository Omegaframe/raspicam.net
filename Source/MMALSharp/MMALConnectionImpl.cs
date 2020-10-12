using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using MMALSharp.Callbacks;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Extensions;
using MMALSharp.Native;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;
using static MMALSharp.MMALNativeExceptionHelper;

namespace MMALSharp
{
    /// <inheritdoc />
    public unsafe class MMALConnectionImpl : MMALObject, IConnection
    {
        /// <inheritdoc />
        public IConnectionCallbackHandler CallbackHandler { get; internal set; }

        /// <inheritdoc />
        public IBufferPool ConnectionPool { get; set; }

        /// <inheritdoc />
        public IDownstreamComponent DownstreamComponent { get; }

        /// <inheritdoc />
        public IComponent UpstreamComponent { get; }

        /// <inheritdoc />
        public IInputPort InputPort { get; }

        /// <inheritdoc />
        public IOutputPort OutputPort { get; }

        /// <inheritdoc />
        public string Name => Marshal.PtrToStringAnsi((IntPtr)(*Ptr).Name);

        /// <inheritdoc />
        public bool Enabled => (*Ptr).IsEnabled == 1;

        /// <inheritdoc />
        public uint Flags => (*Ptr).Flags;

        /// <inheritdoc />                        
        public long TimeSetup => (*Ptr).TimeSetup;

        /// <inheritdoc />
        public long TimeEnable => (*Ptr).TimeEnable;

        /// <inheritdoc />
        public long TimeDisable => (*Ptr).TimeDisable;


        MMALConnection.MMAL_CONNECTION_CALLBACK_T NativeCallback;

        /// <inheritdoc />
        public MMAL_CONNECTION_T* Ptr { get; }

        /// <inheritdoc />
        public override bool CheckState() => Ptr != null && (IntPtr)Ptr != IntPtr.Zero;

        /// <summary>
        /// Creates a new instance of <see cref="MMALConnectionImpl"/>.
        /// </summary>
        /// <param name="ptr">The native connection pointer.</param>
        /// <param name="output">The upstream component's output port.</param>
        /// <param name="input">The downstream component's input port.</param>
        /// <param name="inputComponent">The upstream component.</param>
        /// <param name="outputComponent">The downstream component.</param>
        /// <param name="useCallback">
        /// Configure the connection to intercept native callbacks. Note: will adversely impact performance. In addition, this will implicitly enable
        /// zero copy functionality on both the source and sink ports.
        /// </param>
        protected MMALConnectionImpl(MMAL_CONNECTION_T* ptr, IOutputPort output, IInputPort input, IDownstreamComponent inputComponent, IComponent outputComponent, bool useCallback)
        {
            Ptr = ptr;
            OutputPort = output;
            InputPort = input;
            DownstreamComponent = inputComponent;
            UpstreamComponent = outputComponent;

            if (useCallback)
            {
                CallbackHandler = new DefaultConnectionCallbackHandler(this);
                ConfigureConnectionCallback(output, input);
            }

            Enable();

            if (useCallback)
                OutputPort.SendAllBuffers(ConnectionPool);
        }

        /// <inheritdoc />
        public override string ToString() => $"Component connection - Upstream component: {UpstreamComponent.Name} on port {OutputPort.Name} Downstream component: {DownstreamComponent.Name} on port {InputPort.Name}";


        /// <inheritdoc />
        public override void Dispose()
        {
            MMALLog.Logger.LogDebug("Disposing connection.");
            OutputPort?.CloseConnection();
            InputPort?.CloseConnection();
            Destroy();
            base.Dispose();
        }

        /// <inheritdoc />
        public void Enable()
        {
            if (Enabled)
                return;

            MMALLog.Logger.LogDebug($"Enabling connection between {OutputPort.Name} and {InputPort.Name}");
            MMALCheck(MMALConnection.mmal_connection_enable(Ptr), "Unable to enable connection");
        }

        /// <inheritdoc />
        public void Disable()
        {
            if (!Enabled)
                return;

            MMALLog.Logger.LogDebug($"Disabling connection between {OutputPort.Name} and {InputPort.Name}");
            MMALCheck(MMALConnection.mmal_connection_disable(Ptr), "Unable to disable connection");
        }

        /// <inheritdoc />
        public void Destroy()
        {
            UpstreamComponent.CleanPortPools();
            DownstreamComponent.CleanPortPools();

            MMALCheck(MMALConnection.mmal_connection_destroy(Ptr), "Unable to destroy connection");
        }

        /// <inheritdoc />
        public void RegisterCallbackHandler(IConnectionCallbackHandler handler) => CallbackHandler = handler;


        /// <summary>
        /// Facility to create a connection between two port objects.
        /// </summary>
        /// <param name="output">The output port of the connection.</param>
        /// <param name="input">The input port of the connection.</param>
        /// <param name="inputComponent">The managed instance of the component we are connecting to.</param>
        /// <param name="useCallback">When set to true, enable the connection callback delegate (adversely affects performance).</param>
        /// <returns>A new managed connection object.</returns>
        internal static MMALConnectionImpl CreateConnection(IOutputPort output, IInputPort input, IDownstreamComponent inputComponent, bool useCallback)
        {
            IntPtr ptr = IntPtr.Zero;

            if (useCallback)
                MMALCheck(MMALConnection.mmal_connection_create(&ptr, output.Ptr, input.Ptr, MMALConnection.MMAL_CONNECTION_FLAG_ALLOCATION_ON_INPUT), "Unable to create connection");
            else
                MMALCheck(MMALConnection.mmal_connection_create(&ptr, output.Ptr, input.Ptr, MMALConnection.MMAL_CONNECTION_FLAG_TUNNELLING | MMALConnection.MMAL_CONNECTION_FLAG_ALLOCATION_ON_INPUT), "Unable to create connection");

            return new MMALConnectionImpl((MMAL_CONNECTION_T*)ptr, output, input, inputComponent, output.ComponentReference, useCallback);
        }

        /// <summary>
        /// Represents the native callback method for a connection between two ports.
        /// </summary>
        /// <param name="connection">The native pointer to a MMAL_CONNECTION_T struct.</param>
        /// <returns>The value of all flags set against this connection.</returns>
        protected virtual int NativeConnectionCallback(MMAL_CONNECTION_T* connection)
        {
            if (MMALCameraConfig.Debug)
                MMALLog.Logger.LogDebug("Inside native connection callback");

            var queue = new MMALQueueImpl(connection->Queue);
            var bufferImpl = queue.GetBuffer();

            if (bufferImpl.CheckState())
            {
                if (MMALCameraConfig.Debug)
                    bufferImpl.PrintProperties();

                if (bufferImpl.Length > 0)
                    CallbackHandler.InputCallback(bufferImpl);

                InputPort.SendBuffer(bufferImpl);

                return (int)connection->Flags;
            }

            queue = new MMALQueueImpl(connection->Pool->Queue);
            bufferImpl = queue.GetBuffer();

            if (!bufferImpl.CheckState())
            {
                MMALLog.Logger.LogInformation("Buffer could not be obtained by connection callback");
                return (int)connection->Flags;
            }

            if (MMALCameraConfig.Debug)
                bufferImpl.PrintProperties();

            if (bufferImpl.Length > 0)
                CallbackHandler.OutputCallback(bufferImpl);

            OutputPort.SendBuffer(bufferImpl);

            return (int)connection->Flags;
        }

        void ConfigureConnectionCallback(IOutputPort output, IInputPort input)
        {
            output.SetParameter(MMALParametersCommon.MMAL_PARAMETER_ZERO_COPY, true);
            input.SetParameter(MMALParametersCommon.MMAL_PARAMETER_ZERO_COPY, true);

            NativeCallback = new MMALConnection.MMAL_CONNECTION_CALLBACK_T(NativeConnectionCallback);
            IntPtr ptrCallback = Marshal.GetFunctionPointerForDelegate(NativeCallback);

            Ptr->Callback = ptrCallback;

            ConnectionPool = new MMALPoolImpl(Ptr->Pool);
        }
    }
}
