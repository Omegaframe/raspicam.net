namespace MMALSharp.Native.Parameters
{
    public static class MmalParametersClock
    {
        public const int MmalParameterClockReference = MmalParametersCommon.MmalParameterGroupClock;
        public const int MmalParameterClockActive = MmalParametersCommon.MmalParameterGroupClock + 1;
        public const int MmalParameterClockScale = MmalParametersCommon.MmalParameterGroupClock + 2;
        public const int MmalParameterClockTime = MmalParametersCommon.MmalParameterGroupClock + 3;
        public const int MmalParameterClockUpdateThreshold = MmalParametersCommon.MmalParameterGroupClock + 4;
        public const int MmalParameterClockDiscontThreshold = MmalParametersCommon.MmalParameterGroupClock + 5;
        public const int MmalParameterClockRequestThreshold = MmalParametersCommon.MmalParameterGroupClock + 6;
        public const int MmalParameterClockEnableBufferInfo = MmalParametersCommon.MmalParameterGroupClock + 7;
        public const int MmalParameterClockFrameRate = MmalParametersCommon.MmalParameterGroupClock + 8;
        public const int MmalParameterClockLatency = MmalParametersCommon.MmalParameterGroupClock + 9;
    }
}