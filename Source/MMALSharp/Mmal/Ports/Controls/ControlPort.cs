using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using MMALSharp.Config;
using MMALSharp.Mmal.Callbacks;
using MMALSharp.Mmal.Components;
using MMALSharp.Native.Buffer;
using MMALSharp.Native.Events;
using MMALSharp.Native.Parameters;
using MMALSharp.Native.Port;
using MMALSharp.Utility;

namespace MMALSharp.Mmal.Ports.Controls
{
    unsafe class ControlPort : PortBase<IOutputCallbackHandler>, IControlPort
    {
        public override Resolution Resolution
        {
            get => new Resolution(0, 0);
            internal set { }
        }

        public ControlPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, PortType.Control, guid) { }
        
        public void Start() => Enable();

        void Enable()
        {
            if (Enabled)
                return;

            CallbackHandler = new DefaultPortCallbackHandler(this, null);

            NativeCallback = NativeControlPortCallback;

            var ptrCallback = Marshal.GetFunctionPointerForDelegate(NativeCallback);

            MmalLog.Logger.LogDebug($"{Name}: Enabling control port.");

            EnablePort(ptrCallback);
        }

        void NativeControlPortCallback(MmalPortType* port, MmalBufferHeader* buffer)
        {
            if (buffer->Cmd == MmalEvents.MmalEventParameterChanged)
            {
                var data = (MmalEventParameterChanged*)buffer->Data;

                if (data->Hdr.Id == MmalParametersCamera.MmalParameterCameraSettings)
                {
                    var settings = (MmalParameterCameraSettingsType*)data;

                    MmalLog.Logger.LogDebug($"{Name}: Analog gain num {settings->AnalogGain.Num}");
                    MmalLog.Logger.LogDebug($"{Name}: Analog gain den {settings->AnalogGain.Den}");
                    MmalLog.Logger.LogDebug($"{Name}: Exposure {settings->Exposure}");
                    MmalLog.Logger.LogDebug($"{Name}: Focus position {settings->FocusPosition}");
                }
            }
            else if (buffer->Cmd == MmalEvents.MmalEventError)
            {
                MmalLog.Logger.LogInformation($"{Name}: Error buffer event returned. If using camera, check all connections, including the Sunny one on the camera board.");
            }
            else
            {
                MmalLog.Logger.LogInformation($"{Name}: Received unexpected camera control callback event");
            }

            var bufferImpl = new MmalBuffer(buffer);

            if (!bufferImpl.CheckState())
            {
                MmalLog.Logger.LogWarning($"{Name}: Received null control buffer.");
                return;
            }

            CallbackHandler.Callback(bufferImpl);

            bufferImpl.Release();
        }
    }
}
