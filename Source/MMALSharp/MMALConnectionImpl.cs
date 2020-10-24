using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using MMALSharp.Callbacks;
using MMALSharp.Components;
using MMALSharp.Extensions;
using MMALSharp.Native.Connection;
using MMALSharp.Native.Parameters;
using MMALSharp.Ports.Inputs;
using MMALSharp.Ports.Outputs;
using MMALSharp.Utility;
using static MMALSharp.MmalNativeExceptionHelper;

namespace MMALSharp
{
    unsafe class MmalConnectionImpl : MmalObject, IConnection
    {
        public IConnectionCallbackHandler CallbackHandler { get; internal set; }
        public IBufferPool ConnectionPool { get; set; }
        public IDownstreamComponent DownstreamComponent { get; }
        public IComponent UpstreamComponent { get; }
        public IInputPort InputPort { get; }
        public IOutputPort OutputPort { get; }
        public string Name => Marshal.PtrToStringAnsi((IntPtr)(*Ptr).Name);
        public bool Enabled => (*Ptr).IsEnabled == 1;
        public uint Flags => (*Ptr).Flags;        
        public long TimeSetup => (*Ptr).TimeSetup;
        public long TimeEnable => (*Ptr).TimeEnable;
        public long TimeDisable => (*Ptr).TimeDisable;
        public MmalConnectionType* Ptr { get; }

        MmalConnection.MmalConnectionCallbackT NativeCallback;

        public override bool CheckState() => Ptr != null && (IntPtr)Ptr != IntPtr.Zero;

        protected MmalConnectionImpl(MmalConnectionType* ptr, IOutputPort output, IInputPort input, IDownstreamComponent inputComponent, IComponent outputComponent, bool useCallback)
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
        
        public override string ToString() => $"Component connection - Upstream component: {UpstreamComponent.Name} on port {OutputPort.Name} Downstream component: {DownstreamComponent.Name} on port {InputPort.Name}";

        public override void Dispose()
        {
            MmalLog.Logger.LogDebug("Disposing connection.");
            OutputPort?.CloseConnection();
            InputPort?.CloseConnection();
            Destroy();
            base.Dispose();
        }

        public void Enable()
        {
            if (Enabled)
                return;

            MmalLog.Logger.LogDebug($"Enabling connection between {OutputPort.Name} and {InputPort.Name}");
            MmalCheck(MmalConnection.Enable(Ptr), "Unable to enable connection");
        }

        public void Disable()
        {
            if (!Enabled)
                return;

            MmalLog.Logger.LogDebug($"Disabling connection between {OutputPort.Name} and {InputPort.Name}");
            MmalCheck(MmalConnection.Disable(Ptr), "Unable to disable connection");
        }

        public void Destroy()
        {
            UpstreamComponent.CleanPortPools();
            DownstreamComponent.CleanPortPools();

            MmalCheck(MmalConnection.Destroy(Ptr), "Unable to destroy connection");
        }

        public void RegisterCallbackHandler(IConnectionCallbackHandler handler) => CallbackHandler = handler;

        internal static MmalConnectionImpl CreateConnection(IOutputPort output, IInputPort input, IDownstreamComponent inputComponent, bool useCallback)
        {
            var ptr = IntPtr.Zero;

            if (useCallback)
                MmalCheck(MmalConnection.Create(&ptr, output.Ptr, input.Ptr, MmalConnection.MmalConnectionFlagAllocationOnInput), "Unable to create connection");
            else
                MmalCheck(MmalConnection.Create(&ptr, output.Ptr, input.Ptr, MmalConnection.MmalConnectionFlagTunnelling | MmalConnection.MmalConnectionFlagAllocationOnInput), "Unable to create connection");

            return new MmalConnectionImpl((MmalConnectionType*)ptr, output, input, inputComponent, output.ComponentReference, useCallback);
        }

        protected virtual int NativeConnectionCallback(MmalConnectionType* connection)
        {
            if (MmalCameraConfig.Debug)
                MmalLog.Logger.LogDebug("Inside native connection callback");

            var queue = new MmalQueueImpl(connection->Queue);
            var bufferImpl = queue.GetBuffer();

            if (bufferImpl.CheckState())
            {
                if (MmalCameraConfig.Debug)
                    bufferImpl.PrintProperties();

                if (bufferImpl.Length > 0)
                    CallbackHandler.InputCallback(bufferImpl);

                InputPort.SendBuffer(bufferImpl);

                return (int)connection->Flags;
            }

            queue = new MmalQueueImpl(connection->Pool->Queue);
            bufferImpl = queue.GetBuffer();

            if (!bufferImpl.CheckState())
            {
                MmalLog.Logger.LogInformation("Buffer could not be obtained by connection callback");
                return (int)connection->Flags;
            }

            if (MmalCameraConfig.Debug)
                bufferImpl.PrintProperties();

            if (bufferImpl.Length > 0)
                CallbackHandler.OutputCallback(bufferImpl);

            OutputPort.SendBuffer(bufferImpl);

            return (int)connection->Flags;
        }

        void ConfigureConnectionCallback(IOutputPort output, IInputPort input)
        {
            output.SetParameter(MmalParametersCommon.MmalParameterZeroCopy, true);
            input.SetParameter(MmalParametersCommon.MmalParameterZeroCopy, true);

            NativeCallback = new MmalConnection.MmalConnectionCallbackT(NativeConnectionCallback);
            var ptrCallback = Marshal.GetFunctionPointerForDelegate(NativeCallback);

            Ptr->Callback = ptrCallback;

            ConnectionPool = new MmalPoolImpl(Ptr->Pool);
        }
    }
}
