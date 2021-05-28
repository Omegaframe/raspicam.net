using System.Runtime.InteropServices;

namespace Raspicam.Net.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterCameraConfigType
    {
        public MmalParameterHeaderType Hdr;
        public int MaxStillsW;
        public int MaxStillsH;
        public int StillsYUV422;
        public int OneShotStills;
        public int MaxPreviewVideoW;
        public int MaxPreviewVideoH;
        public int NumPreviewVideoFrames;
        public int StillsCaptureCircularBufferHeight;
        public int FastPreviewResume;
        public MmalParameterCameraConfigTimestampModeType UseSTCTimestamp;

        public MmalParameterCameraConfigType(MmalParameterHeaderType hdr, int maxStillsW, int maxStillsH, int stillsYUV422,
            int oneShotStills, int maxPreviewVideoW, int maxPreviewVideoH, int numPreviewVideoFrames,
            int stillsCaptureCircularBufferHeight, int fastPreviewResume,
            MmalParameterCameraConfigTimestampModeType useSTCTimestamp)
        {
            Hdr = hdr;
            MaxStillsW = maxStillsW;
            MaxStillsH = maxStillsH;
            StillsYUV422 = stillsYUV422;
            OneShotStills = oneShotStills;
            MaxPreviewVideoW = maxPreviewVideoW;
            MaxPreviewVideoH = maxPreviewVideoH;
            NumPreviewVideoFrames = numPreviewVideoFrames;
            StillsCaptureCircularBufferHeight = stillsCaptureCircularBufferHeight;
            FastPreviewResume = fastPreviewResume;
            UseSTCTimestamp = useSTCTimestamp;
        }
    }
}