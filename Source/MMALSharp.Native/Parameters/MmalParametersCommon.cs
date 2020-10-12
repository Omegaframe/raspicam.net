namespace MMALSharp.Native.Parameters
{
    public static class MmalParametersCommon
    {
        public const int MmalParameterGroupCommon = 0;
        public const int MmalParameterGroupCamera = 1 << 16;
        public const int MmalParameterGroupVideo = 2 << 16;
        public const int MmalParameterGroupAudio = 3 << 16;
        public const int MmalParameterGroupClock = 4 << 16;
        public const int MmalParameterGroupMiracast = 5 << 16;
        public const int MmalParameterUnused = 0;
        public const int MmalParameterSupportedEncodings = 1;
        public const int MmalParameterUri = 2;
        public const int MmalParameterChangeEventRequest = 3;
        public const int MmalParameterZeroCopy = 4;
        public const int MmalParameterBufferRequirements = 5;
        public const int MmalParameterStatistics = 6;
        public const int MmalParameterCoreStatistics = 7;
        public const int MmalParameterMemUsage = 8;
        public const int MmalParameterBufferFlagFilter = 9;
        public const int MmalParameterSeek = 10;
        public const int MmalParameterPowermonEnable = 11;
        public const int MmalParameterLogging = 12;
        public const int MmalParameterSystemTime = 13;
        public const int MmalParameterNoImagePadding = 14;
        public const int MmalParameterLockstepEnable = 15;

        public const int MmalParamSeekFlagPrecise = 0x01;
        public const int MmalParamSeekFlagForward = 0x02;
    }
}