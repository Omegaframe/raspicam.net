using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Native;

namespace MMALSharp.Ports.Outputs
{
    public unsafe class FileEncodeOutputPort : OutputPort
    {
        public FileEncodeOutputPort(IntPtr ptr, IComponent comp, Guid guid) : base(ptr, comp, guid) { }

        public FileEncodeOutputPort(IPort copyFrom) : base((IntPtr)copyFrom.Ptr, copyFrom.ComponentReference, copyFrom.Guid) { }

        internal override void NativeOutputPortCallback(MMAL_PORT_T* port, MMAL_BUFFER_HEADER_T* buffer)
        {
            if (MmalCameraConfig.Debug)
                MmalLog.Logger.LogDebug($"{Name}: In native {nameof(FileEncodeOutputPort)} callback");

            var bufferImpl = new MmalBuffer(buffer);
            bufferImpl.PrintProperties();
            bufferImpl.ParseEvents();

            ProcessBuffer(bufferImpl);
        }

        void ProcessBuffer(IBuffer bufferImpl)
        {
            var eos = bufferImpl.AssertProperty(MmalBufferProperties.MmalBufferHeaderFlagEos) ||
                      ComponentReference.ForceStopProcessing;

            if (bufferImpl.CheckState())
            {
                if (bufferImpl.Cmd > 0)
                {
                    if (bufferImpl.Cmd == MmalEvents.MmalEventFormatChanged)
                        Task.Run(() => { ProcessFormatChangedEvent(bufferImpl); });
                    else
                        ReleaseBuffer(bufferImpl, eos);
                }
                else
                {
                    if ((bufferImpl.Length > 0 && !eos && !Trigger.Task.IsCompleted) || (eos && !Trigger.Task.IsCompleted))
                        CallbackHandler.Callback(bufferImpl);
                    else
                        MmalLog.Logger.LogDebug($"{Name}: Buffer length empty.");

                    // Ensure we release the buffer before any signalling or we will cause a memory leak due to there still being a reference count on the buffer.                    
                    ReleaseBuffer(bufferImpl, eos);
                }
            }
            else
            {
                MmalLog.Logger.LogDebug($"{Name}: Invalid output buffer received");
            }

            // If this buffer signals the end of data stream, allow waiting thread to continue.
            if (eos)
            {
                MmalLog.Logger.LogDebug($"{Name}: End of stream. Signaling completion...");

                Task.Run(() => { Trigger.SetResult(true); });
            }
        }

        void ProcessFormatChangedEvent(IBuffer buffer)
        {
            MmalLog.Logger.LogInformation($"{Name}: Received MMAL_EVENT_FORMAT_CHANGED event");

            var ev = MmalEventFormat.GetEventFormat(buffer);

            MmalLog.Logger.LogInformation("-- Event format changed from -- ");
            LogFormat(new MmalEventFormat(Format), this);

            MmalLog.Logger.LogInformation("-- To -- ");
            LogFormat(ev, null);

            MmalLog.Logger.LogDebug($"{Name}: Finished processing MMAL_EVENT_FORMAT_CHANGED event");
        }

        static void LogFormat(MmalEventFormat format, IPort port)
        {
            var sb = new StringBuilder();

            if (port != null)
            {
                switch (port.PortType)
                {
                    case PortType.Input:
                        sb.AppendLine("Port Type: Input");
                        break;
                    case PortType.Output:
                        sb.AppendLine("Port Type: Output");
                        break;
                    case PortType.Control:
                        sb.AppendLine("Port Type: Control");
                        break;
                }
            }

            sb.AppendLine($"FourCC: {format.FourCC}");
            sb.AppendLine($"Width: {format.Width}");
            sb.AppendLine($"Height: {format.Height}");
            sb.AppendLine($"Crop: {format.CropX}, {format.CropY}, {format.CropWidth}, {format.CropHeight}");
            sb.AppendLine($"Pixel aspect ratio: {format.ParNum}, {format.ParDen}. Frame rate: {format.FramerateNum}, {format.FramerateDen}");

            if (port != null)
                sb.AppendLine($"Port info: Buffers num: {port.BufferNum}(opt {port.BufferNumRecommended}, min {port.BufferNumMin}). Size: {port.BufferSize} (opt {port.BufferSizeRecommended}, min {port.BufferSizeMin}). Alignment: {port.BufferAlignmentMin}");

            MmalLog.Logger.LogInformation(sb.ToString());
        }
    }
}
