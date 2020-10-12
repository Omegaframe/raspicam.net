using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using MMALSharp.Callbacks;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Native;

namespace MMALSharp.Ports.Controls
{
    public unsafe class ControlPort : PortBase<IOutputCallbackHandler>, IControlPort
    {
        public override Resolution Resolution // todo: this does nothing? why?
        {
            get => new Resolution(0, 0);
            internal set { }
        }

        public ControlPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, PortType.Control, guid) { }

        public void Enable()
        {
            if (Enabled)
                return;

            CallbackHandler = new DefaultPortCallbackHandler(this, null);

            NativeCallback = new MMALPort.MMAL_PORT_BH_CB_T(NativeControlPortCallback);

            IntPtr ptrCallback = Marshal.GetFunctionPointerForDelegate(NativeCallback);

            MmalLog.Logger.LogDebug($"{Name}: Enabling control port.");

            EnablePort(ptrCallback);
        }
        public void Start() => Enable();

        /// <summary>
        /// This is the camera's control port callback function. The callback is used if
        /// MMALCameraConfig.SetChangeEventRequest is set to true.
        /// </summary>
        /// <param name="port">Native port struct pointer.</param>
        /// <param name="buffer">Native buffer header pointer.</param>
        internal void NativeControlPortCallback(MMAL_PORT_T* port, MMAL_BUFFER_HEADER_T* buffer)
        {
            if (buffer->Cmd == MMALEvents.MMAL_EVENT_PARAMETER_CHANGED)
            {
                var data = (MMAL_EVENT_PARAMETER_CHANGED_T*)buffer->Data;

                if (data->Hdr.Id == MMALParametersCamera.MMAL_PARAMETER_CAMERA_SETTINGS)
                {
                    var settings = (MMAL_PARAMETER_CAMERA_SETTINGS_T*)data;

                    MmalLog.Logger.LogDebug($"{Name}: Analog gain num {settings->AnalogGain.Num}");
                    MmalLog.Logger.LogDebug($"{Name}: Analog gain den {settings->AnalogGain.Den}");
                    MmalLog.Logger.LogDebug($"{Name}: Exposure {settings->Exposure}");
                    MmalLog.Logger.LogDebug($"{Name}: Focus position {settings->FocusPosition}");
                }
            }
            else if (buffer->Cmd == MMALEvents.MMAL_EVENT_ERROR)
            {
                MmalLog.Logger.LogInformation($"{Name}: Error buffer event returned. If using camera, check all connections, including the Sunny one on the camera board.");
            }
            else
            {
                MmalLog.Logger.LogInformation($"{Name}: Received unexpected camera control callback event");
            }

            if (MMALCameraConfig.Debug)
                MmalLog.Logger.LogDebug($"{Name}: In native control callback.");

            var bufferImpl = new MMALBufferImpl(buffer);

            if (!bufferImpl.CheckState())
            {
                MmalLog.Logger.LogWarning($"{Name}: Received null control buffer.");
                return;
            }

            if (MMALCameraConfig.Debug)
                bufferImpl.ParseEvents();

            bufferImpl.PrintProperties();

            CallbackHandler.Callback(bufferImpl);

            if (MMALCameraConfig.Debug)
                MmalLog.Logger.LogDebug($"{Name}: Releasing buffer.");

            bufferImpl.Release();
        }
    }
}
