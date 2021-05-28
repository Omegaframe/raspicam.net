namespace Raspicam.Net.Native.Buffer
{
    public enum MmalBufferProperties
    {
        MmalBufferHeaderFlagEos = 1 << 0,
        MmalBufferHeaderFlagFrameStart = 1 << 1,
        MmalBufferHeaderFlagFrameEnd = 1 << 2,
        MmalBufferHeaderFlagFrame = MmalBufferHeaderFlagFrameStart | MmalBufferHeaderFlagFrameEnd,
        MmalBufferHeaderFlagKeyframe = 1 << 3,
        MmalBufferHeaderFlagDiscontinuity = 1 << 4,
        MmalBufferHeaderFlagConfig = 1 << 5,
        MmalBufferHeaderFlagEncrypted = 1 << 6,
        MmalBufferHeaderFlagCodecSideInfo = 1 << 7,
        MmalBufferHeaderFlagsSnapshot = 1 << 8,
        MmalBufferHeaderFlagCorrupted = 1 << 9,
        MmalBufferHeaderFlagTransmissionFailed = 1 << 10,
        MmalBufferHeaderFlagDecodeOnly = 1 << 11,
        MmalBufferHeaderFlagNal = 1 << 12,
        MmalBufferHeaderFlagUnknown = 9998,
        MmalBufferHeaderFlagCompleteFrame = 9999,
    }
}
