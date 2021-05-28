namespace Raspicam.Net.Native.Parameters
{
    public enum MmalParamFocusStatusType
    {
        MmalParamFocusStatusOff,
        MmalParamFocusStatusRequest,
        MmalParamFocusStatusReached,
        MmalParamFocusStatusUnableToReach,
        MmalParamFocusStatusLost,
        MmalParamFocusStatusCafMoving,
        MmalParamFocusStatusCafSuccess,
        MmalParamFocusStatusCafFailed,
        MmalParamFocusStatusManualMoving,
        MmalParamFocusStatusManualReached,
        MmalParamFocusStatusCafWatching,
        MmalParamFocusStatusCafSceneChanged,
        MmalParamFocusStatusMax = 0x7FFFFFFF
    }
}