using System;
using System.Runtime.InteropServices;

namespace MMALSharp.Native
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

    public enum MmalCoreStatsDir
    {
        MmalCoreStatsRx,
        MmalCoreStatsTx
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_HEADER_T
    {
        public int Id { get; set; }

        public int Size { get; set; }

        public MMAL_PARAMETER_HEADER_T(int id, int size)
        {
            this.Id = id;
            this.Size = size;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_BYTES_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        byte[] data;

        public byte[] Data => data;

        public MMAL_PARAMETER_BYTES_T(MMAL_PARAMETER_HEADER_T hdr, byte[] data)
        {
            this.Hdr = hdr;
            this.data = data;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_PARAMETER_CHANGE_EVENT_REQUEST_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        int changeId;
        int enable;

        public int ChangeId => changeId;
        public int Enable => enable;

        public MMAL_PARAMETER_CHANGE_EVENT_REQUEST_T(MMAL_PARAMETER_HEADER_T hdr, int changeId, int enable)
        {
            this.Hdr = hdr;
            this.changeId = changeId;
            this.enable = enable;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_BUFFER_REQUIREMENTS_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        int bufferNumMin, bufferSizeMin, bufferAlignmentMin, bufferNumRecommended, bufferSizeRecommended;

        public int BufferNumMin => this.bufferNumMin;

        public int BufferSizeMin => this.bufferSizeMin;

        public int BufferAlignmentMin => this.bufferAlignmentMin;

        public int BufferNumRecommended => this.bufferNumRecommended;

        public int BufferSizeRecommended => this.bufferSizeRecommended;

        public MMAL_PARAMETER_BUFFER_REQUIREMENTS_T(MMAL_PARAMETER_HEADER_T hdr, int bufferNumMin, int bufferSizeMin, int bufferAlignmentMin, int bufferNumRecommended, int bufferSizeRecommended)
        {
            this.Hdr = hdr;
            this.bufferNumMin = bufferNumMin;
            this.bufferSizeMin = bufferSizeMin;
            this.bufferAlignmentMin = bufferAlignmentMin;
            this.bufferNumRecommended = bufferNumRecommended;
            this.bufferSizeRecommended = bufferSizeRecommended;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_SEEK_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        long offset;
        uint flags;

        public long Offset => this.offset;

        public uint Flags => this.flags;

        public MMAL_PARAMETER_SEEK_T(MMAL_PARAMETER_HEADER_T hdr, long offset, uint flags)
        {
            this.Hdr = hdr;
            this.offset = offset;
            this.flags = flags;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_STATISTICS_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        uint bufferCount;
        uint frameCount;
        uint framesSkipped;
        uint framesDiscarded;
        uint eosSeen;
        uint maximumFrameBytes;
        uint totalBytes;
        uint corruptMacroblocks;

        public uint BufferCount => this.bufferCount;

        public uint FrameCount => this.frameCount;

        public uint FramesSkipped => this.framesSkipped;

        public uint FramesDiscarded => this.framesDiscarded;

        public uint EosSeen => this.eosSeen;

        public uint MaximumFrameBytes => this.maximumFrameBytes;

        public uint TotalBytes => this.totalBytes;

        public uint CorruptMacroBlocks => this.corruptMacroblocks;

        public MMAL_PARAMETER_STATISTICS_T(MMAL_PARAMETER_HEADER_T hdr, uint bufferCount, uint frameCount, uint framesSkipped,
                                           uint framesDiscarded, uint eosSeen, uint maximumFrameBytes, uint totalBytes,
                                           uint corruptMacroblocks)
        {
            this.Hdr = hdr;
            this.bufferCount = bufferCount;
            this.frameCount = frameCount;
            this.framesSkipped = framesSkipped;
            this.framesDiscarded = framesDiscarded;
            this.eosSeen = eosSeen;
            this.maximumFrameBytes = maximumFrameBytes;
            this.totalBytes = totalBytes;
            this.corruptMacroblocks = corruptMacroblocks;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CORE_STATISTICS_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        MmalCoreStatsDir dir;
        int reset;
        MMAL_CORE_STATISTICS_T stats;

        public MmalCoreStatsDir Dir => this.dir;

        public int Reset => this.reset;

        public MMAL_CORE_STATISTICS_T Stats => this.stats;

        public MMAL_PARAMETER_CORE_STATISTICS_T(MMAL_PARAMETER_HEADER_T hdr, MmalCoreStatsDir dir, int reset,
                                                MMAL_CORE_STATISTICS_T stats)
        {
            this.Hdr = hdr;
            this.dir = dir;
            this.reset = reset;
            this.stats = stats;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_MEM_USAGE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        int poolMemAllocSize;

        public int PoolMemAllocSize => this.poolMemAllocSize;

        public MMAL_PARAMETER_MEM_USAGE_T(MMAL_PARAMETER_HEADER_T hdr, int poolMemAllocSize)
        {
            this.Hdr = hdr;
            this.poolMemAllocSize = poolMemAllocSize;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_LOGGING_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        uint set;
        uint clear;

        public uint Set => this.set;

        public uint Clear => this.clear;

        public MMAL_PARAMETER_LOGGING_T(MMAL_PARAMETER_HEADER_T hdr, uint set, uint clear)
        {
            this.Hdr = hdr;
            this.set = set;
            this.clear = clear;
        }
    }

    public static class MmalParametersCamera
    {
        public const int MmalParameterThumbnailConfiguration = MmalParametersCommon.MmalParameterGroupCamera;
        public const int MmalParameterCaptureQuality = MmalParametersCommon.MmalParameterGroupCamera + 1;
        public const int MmalParameterRotation = MmalParametersCommon.MmalParameterGroupCamera + 2;
        public const int MmalParameterExifDisable = MmalParametersCommon.MmalParameterGroupCamera + 3;
        public const int MmalParameterExif = MmalParametersCommon.MmalParameterGroupCamera + 4;
        public const int MmalParameterAwbMode = MmalParametersCommon.MmalParameterGroupCamera + 5;
        public const int MmalParameterImageEffect = MmalParametersCommon.MmalParameterGroupCamera + 6;
        public const int MmalParameterColorEffect = MmalParametersCommon.MmalParameterGroupCamera + 7;
        public const int MmalParameterFlickerAvoid = MmalParametersCommon.MmalParameterGroupCamera + 8;
        public const int MmalParameterFlash = MmalParametersCommon.MmalParameterGroupCamera + 9;
        public const int MmalParameterRedeye = MmalParametersCommon.MmalParameterGroupCamera + 10;
        public const int MmalParameterFocus = MmalParametersCommon.MmalParameterGroupCamera + 11;
        public const int MmalParameterFocalLengths = MmalParametersCommon.MmalParameterGroupCamera + 12;
        public const int MmalParameterExposureComp = MmalParametersCommon.MmalParameterGroupCamera + 13;
        public const int MmalParameterZoom = MmalParametersCommon.MmalParameterGroupCamera + 14;
        public const int MmalParameterMirror = MmalParametersCommon.MmalParameterGroupCamera + 15;
        public const int MmalParameterCameraNum = MmalParametersCommon.MmalParameterGroupCamera + 16;
        public const int MmalParameterCapture = MmalParametersCommon.MmalParameterGroupCamera + 17;
        public const int MmalParameterExposureMode = MmalParametersCommon.MmalParameterGroupCamera + 18;
        public const int MmalParameterExpMeteringMode = MmalParametersCommon.MmalParameterGroupCamera + 19;
        public const int MmalParameterFocusStatus = MmalParametersCommon.MmalParameterGroupCamera + 20;
        public const int MmalParameterCameraConfig = MmalParametersCommon.MmalParameterGroupCamera + 21;
        public const int MmalParameterCaptureStatus = MmalParametersCommon.MmalParameterGroupCamera + 22;
        public const int MmalParameterFaceTrack = MmalParametersCommon.MmalParameterGroupCamera + 23;
        public const int MmalParameterDrawBoxFacesAndFocus = MmalParametersCommon.MmalParameterGroupCamera + 24;
        public const int MmalParameterJpegQFactor = MmalParametersCommon.MmalParameterGroupCamera + 25;
        public const int MmalParameterFrameRate = MmalParametersCommon.MmalParameterGroupCamera + 26;
        public const int MmalParameterUseStc = MmalParametersCommon.MmalParameterGroupCamera + 27;
        public const int MmalParameterCameraInfo = MmalParametersCommon.MmalParameterGroupCamera + 28;
        public const int MmalParameterVideoStabilisation = MmalParametersCommon.MmalParameterGroupCamera + 29;
        public const int MmalParameterFaceTrackResults = MmalParametersCommon.MmalParameterGroupCamera + 30;
        public const int MmalParameterEnableRawCapture = MmalParametersCommon.MmalParameterGroupCamera + 31;
        public const int MmalParameterDpfFile = MmalParametersCommon.MmalParameterGroupCamera + 32;
        public const int MmalParameterEnableDpfFile = MmalParametersCommon.MmalParameterGroupCamera + 33;
        public const int MmalParameterDpfFailIsFatal = MmalParametersCommon.MmalParameterGroupCamera + 34;
        public const int MmalParameterCaptureMode = MmalParametersCommon.MmalParameterGroupCamera + 35;
        public const int MmalParameterFocusRegions = MmalParametersCommon.MmalParameterGroupCamera + 36;
        public const int MmalParameterInputCrop = MmalParametersCommon.MmalParameterGroupCamera + 37;
        public const int MmalParameterSensorInformation = MmalParametersCommon.MmalParameterGroupCamera + 38;
        public const int MmalParameterFlashSelect = MmalParametersCommon.MmalParameterGroupCamera + 39;
        public const int MmalParameterFieldOfView = MmalParametersCommon.MmalParameterGroupCamera + 40;
        public const int MmalParameterHighDynamicRange = MmalParametersCommon.MmalParameterGroupCamera + 41;
        public const int MmalParameterDynamicRangeCompression = MmalParametersCommon.MmalParameterGroupCamera + 42;
        public const int MmalParameterAlgorithmControl = MmalParametersCommon.MmalParameterGroupCamera + 43;
        public const int MmalParameterSharpness = MmalParametersCommon.MmalParameterGroupCamera + 44;
        public const int MmalParameterContrast = MmalParametersCommon.MmalParameterGroupCamera + 45;
        public const int MmalParameterBrightness = MmalParametersCommon.MmalParameterGroupCamera + 46;
        public const int MmalParameterSaturation = MmalParametersCommon.MmalParameterGroupCamera + 47;
        public const int MmalParameterIso = MmalParametersCommon.MmalParameterGroupCamera + 48;
        public const int MmalParameterAntishake = MmalParametersCommon.MmalParameterGroupCamera + 49;
        public const int MmalParameterImageEffectParameters = MmalParametersCommon.MmalParameterGroupCamera + 50;
        public const int MmalParameterCameraBurstCapture = MmalParametersCommon.MmalParameterGroupCamera + 51;
        public const int MmalParameterCameraMinIso = MmalParametersCommon.MmalParameterGroupCamera + 52;
        public const int MmalParameterCameraUseCase = MmalParametersCommon.MmalParameterGroupCamera + 53;
        public const int MmalParameterCaptureStatsPass = MmalParametersCommon.MmalParameterGroupCamera + 54;
        public const int MmalParameterCameraCustomSensorConfig = MmalParametersCommon.MmalParameterGroupCamera + 55;
        public const int MmalParameterEnableRegisterFile = MmalParametersCommon.MmalParameterGroupCamera + 56;
        public const int MmalParameterRegisterFailIsFatal = MmalParametersCommon.MmalParameterGroupCamera + 57;
        public const int MmalParameterConfigfileRegisters = MmalParametersCommon.MmalParameterGroupCamera + 58;
        public const int MmalParameterConfigfileChunkRegisters = MmalParametersCommon.MmalParameterGroupCamera + 59;
        public const int MmalParameterJpegAttachLog = MmalParametersCommon.MmalParameterGroupCamera + 60;
        public const int MmalParameterZeroShutterLag = MmalParametersCommon.MmalParameterGroupCamera + 61;
        public const int MmalParameterFpsRange = MmalParametersCommon.MmalParameterGroupCamera + 62;
        public const int MmalParameterCaptureExposureComp = MmalParametersCommon.MmalParameterGroupCamera + 63;
        public const int MmalParameterSwSharpenDisable = MmalParametersCommon.MmalParameterGroupCamera + 64;
        public const int MmalParameterFlashRequired = MmalParametersCommon.MmalParameterGroupCamera + 65;
        public const int MmalParameterSwSaturationDisable = MmalParametersCommon.MmalParameterGroupCamera + 66;
        public const int MmalParameterShutterSpeed = MmalParametersCommon.MmalParameterGroupCamera + 67;
        public const int MmalParameterCustomAwbGains = MmalParametersCommon.MmalParameterGroupCamera + 68;
        public const int MmalParameterCameraSettings = MmalParametersCommon.MmalParameterGroupCamera + 69;
        public const int MmalParameterPrivacyIndicator = MmalParametersCommon.MmalParameterGroupCamera + 70;
        public const int MmalParameterVideoDenoise = MmalParametersCommon.MmalParameterGroupCamera + 71;
        public const int MmalParameterStillsDenoise = MmalParametersCommon.MmalParameterGroupCamera + 72;
        public const int MmalParameterAnnotate = MmalParametersCommon.MmalParameterGroupCamera + 73;
        public const int MmalParameterStereoscopicMode = MmalParametersCommon.MmalParameterGroupCamera + 74;
        public const int MmalParameterCameraInterface = MmalParametersCommon.MmalParameterGroupCamera + 75;
        public const int MmalParameterCameraClockingMode = MmalParametersCommon.MmalParameterGroupCamera + 76;
        public const int MmalParameterCameraRxConfig = MmalParametersCommon.MmalParameterGroupCamera + 77;
        public const int MmalParameterCameraRxTiming = MmalParametersCommon.MmalParameterGroupCamera + 78;
        public const int MmalParameterDpfConfig = MmalParametersCommon.MmalParameterGroupCamera + 79;
        public const int MmalParameterJpegRestartInterval = MmalParametersCommon.MmalParameterGroupCamera + 80;
        public const int MmalParameterCameraIspBlockOverride = MmalParametersCommon.MmalParameterGroupCamera + 81;
        public const int MmalParameterLensShadingOverride = MmalParametersCommon.MmalParameterGroupCamera + 82;
        public const int MmalParameterBlackLevel = MmalParametersCommon.MmalParameterGroupCamera + 83;
        public const int MmalParameterResizeParams = MmalParametersCommon.MmalParameterGroupCamera + 84;
        public const int MmalParameterCrop = MmalParametersCommon.MmalParameterGroupCamera + 85;
        public const int MmalParameterOutputShift = MmalParametersCommon.MmalParameterGroupCamera + 86;
        public const int MmalParameterCcmShift = MmalParametersCommon.MmalParameterGroupCamera + 87;
        public const int MmalParameterCustomCcm = MmalParametersCommon.MmalParameterGroupCamera + 88;
        public const int MmalParameterAnalogGain = MmalParametersCommon.MmalParameterGroupCamera + 89;
        public const int MmalParameterDigitalGain = MmalParametersCommon.MmalParameterGroupCamera + 90;

        public const int MmalMaxImageFxParameters = 6;

        public const int MmalParameterCameraInfoMaxCameras = 4;
        public const int MmalParameterCameraInfoMaxFlashes = 2;
        public const int MmalParameterCameraInfoMaxStrLen = 16;

        public const int MmalCameraAnnotateMaxTextLen = 32;
        public const int MmalCameraAnnotateMaxTextLenV2 = 128;
        public const int MmalCameraAnnotateMaxTextLenV3 = 128;
    }

    public enum MmalParamExposuremodeT
    {
        MmalParamExposuremodeOff,
        MmalParamExposuremodeAuto,
        MmalParamExposuremodeNight,
        MmalParamExposuremodeNightpreview,
        MmalParamExposuremodeBacklight,
        MmalParamExposuremodeSpotlight,
        MmalParamExposuremodeSports,
        MmalParamExposuremodeSnow,
        MmalParamExposuremodeBeach,
        MmalParamExposuremodeVerylong,
        MmalParamExposuremodeFixedfps,
        MmalParamExposuremodeAntishake,
        MmalParamExposuremodeFireworks,
        MmalParamExposuremodeMax = 0x7fffffff
    }

    public enum MmalParamExposuremeteringmodeT
    {
        MmalParamExposuremeteringmodeAverage,
        MmalParamExposuremeteringmodeSpot,
        MmalParamExposuremeteringmodeBacklit,
        MmalParamExposuremeteringmodeMatrix,
        MmalParamExposuremeteringmodeMax = 0x7fffffff
    }

    public enum MmalParamAwbmodeT
    {
        MmalParamAwbmodeOff,
        MmalParamAwbmodeAuto,
        MmalParamAwbmodeSunlight,
        MmalParamAwbmodeCloudy,
        MmalParamAwbmodeShade,
        MmalParamAwbmodeTungsten,
        MmalParamAwbmodeFluorescent,
        MmalParamAwbmodeIncandescent,
        MmalParamAwbmodeFlash,
        MmalParamAwbmodeHorizon,
        MmalParamAwbmodeGreyworld,
        MmalParamAwbmodeMax = 0x7fffffff
    }

    public enum MmalParamImagefxT
    {
        MmalParamImagefxNone,
        MmalParamImagefxNegative,
        MmalParamImagefxSolarize,
        MmalParamImagefxPosterize,
        MmalParamImagefxWhiteboard,
        MmalParamImagefxBlackboard,
        MmalParamImagefxSketch,
        MmalParamImagefxDenoise,
        MmalParamImagefxEmboss,
        MmalParamImagefxOilpaint,
        MmalParamImagefxHatch,
        MmalParamImagefxGpen,
        MmalParamImagefxPastel,
        MmalParamImagefxWatercolour,
        MmalParamImagefxFilm,
        MmalParamImagefxBlur,
        MmalParamImagefxSaturation,
        MmalParamImagefxColourswap,
        MmalParamImagefxWashedout,
        MmalParamImagefxPosterise,
        MmalParamImagefxColourpoint,
        MmalParamImagefxColourbalance,
        MmalParamImagefxCartoon,
        MmalParamImagefxDeinterlaceDouble,
        MmalParamImagefxDeinterlaceAdv,
        MmalParamImagefxDeinterlaceFast,
        MmalParamImagefxMax = 0x7fffffff
    }

    public enum MmalCameraStcModeT
    {
        MmalParamStcModeOff,
        MmalParamStcModeRaw,
        MmalParamStcModeCooked,
        MmalParamStcModeMax = 0x7fffffff
    }

    public enum MmalParamFlickeravoidT
    {
        MmalParamFlickeravoidOff,
        MmalParamFlickeravoidAuto,
        MmalParamFlickeravoid50Hz,
        MmalParamFlickeravoid60Hz,
        MmalParamFlickeravoidMax = 0x7FFFFFFF
    }

    public enum MmalParamFlashT
    {
        MmalParamFlashOff,
        MmalParamFlashAuto,
        MmalParamFlashOn,
        MmalParamFlashRedeye,
        MmalParamFlashFillin,
        MmalParamFlashTorch,
        MmalParamFlashMax = 0x7FFFFFFF
    }

    public enum MmalParamRedeyeT
    {
        MmalParamRedeyeOff,
        MmalParamRedeyeOn,
        MmalParamRedeyeSimple,
        MmalParamRedeyeMax = 0x7FFFFFFF
    }

    public enum MmalParamFocusT
    {
        MmalParamFocusAuto,
        MmalParamFocusAutoNear,
        MmalParamFocusAutoMacro,
        MmalParamFocusCaf,
        MmalParamFocusCafNear,
        MmalParamFocusFixedInfinity,
        MmalParamFocusFixedHyperfocal,
        MmalParamFocusFixedNear,
        MmalParamFocusFixedMacro,
        MmalParamFocusEdof,
        MmalParamFocusCafMacro,
        MmalParamFocusCafFast,
        MmalParamFocusCafNearFast,
        MmalParamFocusCafMacroFast,
        MmalParamFocusFixedCurrent,
        MmalParamFocusMax = 0x7FFFFFFF
    }

    public enum MmalParamCaptureStatusT
    {
        MmalParamCaptureStatusNotCapturing,
        MmalParamCaptureStatusCaptureStarted,
        MmalParamCaptureStatusCaptureEnded,
        MmalParamCaptureStatusMax = 0x7FFFFFFF
    }

    public enum MmalParamFocusStatusT
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

    public enum MmalParamFaceTrackModeT
    {
        MmalParamFaceDetectNone,
        MmalParamFaceDetectOn,
        MmalParamFaceDetectMax = 0x7FFFFFFF
    }

    public enum MmalParameterCameraConfigTimestampModeT
    {
        MmalParamTimestampModeZero,
        MmalParamTimestampModeRawStc,
        MmalParamTimestampModeResetStc,
        MmalParamTimestampModeMax = 0x7FFFFFFF
    }

    public enum MmalParameterCameraInfoFlashTypeT
    {
        MmalParameterCameraInfoFlashTypeXenon,
        MmalParameterCameraInfoFlashTypeLed,
        MmalParameterCameraInfoFlashTypeOther,
        MmalParameterCameraInfoFlashTypeMax = 0x7FFFFFFF
    }

    public enum MmalParameterCapturemodeModeT
    {
        MmalParamCapturemodeWaitForEnd,
        MmalParamCapturemodeWaitForEndAndHold,
        MmalParamCapturemodeResumeVfImmediately
    }

    public enum MmalParameterFocusRegionTypeT
    {
        MmalParameterFocusRegionTypeNormal,
        MmalParameterFocusRegionTypeFace,
        MmalParameterFocusRegionTypeMax
    }

    public enum MmalParameterDrcStrengthT
    {
        MmalParameterDrcStrengthOff,
        MmalParameterDrcStrengthLow,
        MmalParameterDrcStrengthMedium,
        MmalParameterDrcStrengthHigh,
        MmalParameterDrcStrengthMax = 0x7fffffff
    }

    public enum MmalParameterAlgorithmControlAlgorithmsT
    {
        MmalParameterAlgorithmControlAlgorithmsFacetracking,
        MmalParameterAlgorithmControlAlgorithmsRedeyeReduction,
        MmalParameterAlgorithmControlAlgorithmsVideoStabilisation,
        MmalParameterAlgorithmControlAlgorithmsWriteRaw,
        MmalParameterAlgorithmControlAlgorithmsVideoDenoise,
        MmalParameterAlgorithmControlAlgorithmsStillsDenoise,
        MmalParameterAlgorithmControlAlgorithmsTemporalDenoise,
        MmalParameterAlgorithmControlAlgorithmsAntishake,
        MmalParameterAlgorithmControlAlgorithmsImageEffects,
        MmalParameterAlgorithmControlAlgorithmsDynamicRangeCompression,
        MmalParameterAlgorithmControlAlgorithmsFaceRecognition,
        MmalParameterAlgorithmControlAlgorithmsFaceBeautification,
        MmalParameterAlgorithmControlAlgorithmsSceneDetection,
        MmalParameterAlgorithmControlAlgorithmsHighDynamicRange,
        MmalParameterAlgorithmControlAlgorithmsMax = 0x7fffffff
    }

    public enum MmalParamCameraUseCaseT
    {
        MmalParamCameraUseCaseUnknown,
        MmalParamCameraUseCaseStillsCapture,
        MmalParamCameraUseCaseVideoCapture,
        MmalParamCameraUseCaseMax = 0x7fffffff
    }

    public enum MmalParamPrivacyIndicatorT
    {
        MmalParameterPrivacyIndicatorOff,
        MmalParameterPrivacyIndicatorOn,
        MmalParameterPrivacyIndicatorForceOn,
        MmalParameterPrivacyIndicatorMax = 0x7fffffff
    }

    public enum MmalStereoscopicModeT
    {
        MmalStereoscopicModeNone,
        MmalStereoscopicModeSideBySide,
        MmalStereoscopicModeBottom,
        MmalStereoscopicModeMax = 0x7fffffff
    }

    public enum MmalCameraInterfaceT
    {
        MmalCameraInterfaceCsi2,
        MmalCameraInterfaceCcp2,
        MmalCameraInterfaceCpi,
        MmalCameraInterfaceMax = 0x7fffffff
    }

    public enum MmalCameraClockingModeT
    {
        MmalCameraClockingModeStrobe,
        MmalCameraClockingModeClock,
        MmalCameraClockingModeMax = 0x7fffffff
    }

    public enum MmalCameraRxConfigDecode
    {
        MmalCameraRxConfigDecodeNone,
        MmalCameraRxConfigDecodeDpcm8To10,
        MmalCameraRxConfigDecodeDpcm7To10,
        MmalCameraRxConfigDecodeDpcm6To10,
        MmalCameraRxConfigDecodeDpcm8To12,
        MmalCameraRxConfigDecodeDpcm7To12,
        MmalCameraRxConfigDecodeDpcm6To12,
        MmalCameraRxConfigDecodeDpcm10To14,
        MmalCameraRxConfigDecodeDpcm8To14,
        MmalCameraRxConfigDecodeDpcm12To16,
        MmalCameraRxConfigDecodeDpcm10To16,
        MmalCameraRxConfigDecodeDpcm8To16,
        MmalCameraRxConfigDecodeMax = 0x7fffffff
    }

    public enum MmalCameraRxConfigEncode
    {
        MmalCameraRxConfigEncodeNone,
        MmalCameraRxConfigEncodeDpcm10To8,
        MmalCameraRxConfigEncodeDpcm12To8,
        MmalCameraRxConfigEncodeDpcm14To8,
        MmalCameraRxConfigEncodeMax = 0x7fffffff
    }

    public enum MmalCameraRxConfigUnpack
    {
        MmalCameraRxConfigUnpackNone,
        MmalCameraRxConfigUnpack6,
        MmalCameraRxConfigUnpack7,
        MmalCameraRxConfigUnpack8,
        MmalCameraRxConfigUnpack10,
        MmalCameraRxConfigUnpack12,
        MmalCameraRxConfigUnpack14,
        MmalCameraRxConfigUnpack16,
        MmalCameraRxConfigUnpackMax = 0x7fffffff
    }

    public enum MmalCameraRxConfigPack
    {
        MmalCameraRxConfigPackNone,
        MmalCameraRxConfigPack8,
        MmalCameraRxConfigPack10,
        MmalCameraRxConfigPack12,
        MmalCameraRxConfigPack14,
        MmalCameraRxConfigPack16,
        MmalCameraRxConfigPackRaw10,
        MmalCameraRxConfigPackRaw12,
        MmalCameraRxConfigPackMax = 0x7fffffff
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_THUMBNAIL_CONFIG_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        int _enable;
        int _width;
        int _height;
        int _quality;

        public bool Enable => _enable == 1;

        public int Width => _width;

        public int Height => _height;

        public int Quality => _quality;

        public MMAL_PARAMETER_THUMBNAIL_CONFIG_T(MMAL_PARAMETER_HEADER_T hdr, bool enable, int width, int height, int quality)
        {
            this.Hdr = hdr;

            _enable = enable ? 1 : 0;
            _width = width;
            _height = height;
            _quality = quality;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MMAL_PARAMETER_EXIF_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        int keylen;
        int valueOffset;
        int valueLen;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        byte[] data;

        public int KeyLen => this.keylen;

        public int ValueOffset => this.valueOffset;

        public int ValueLen => this.valueLen;

        public byte[] Data => this.data;

        public MMAL_PARAMETER_EXIF_T(MMAL_PARAMETER_HEADER_T hdr, int keylen, int valueOffset, int valueLen, byte[] data)
        {
            this.Hdr = hdr;
            this.keylen = keylen;
            this.valueOffset = valueOffset;
            this.valueLen = valueLen;
            this.data = data;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MMAL_PARAMETER_EXIF_T_DUMMY
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        int keylen;
        int valueOffset;
        int valueLen;
        byte data;

        public int KeyLen => this.keylen;

        public int ValueOffset => this.valueOffset;

        public int ValueLen => this.valueLen;

        public byte Data => this.data;

        public MMAL_PARAMETER_EXIF_T_DUMMY(MMAL_PARAMETER_HEADER_T hdr, int keylen, int valueOffset, int valueLen, byte data)
        {
            this.Hdr = hdr;
            this.keylen = keylen;
            this.valueOffset = valueOffset;
            this.valueLen = valueLen;
            this.data = data;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_PARAMETER_EXPOSUREMODE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        MmalParamExposuremodeT value;

        public MmalParamExposuremodeT Value => this.value;

        public MMAL_PARAMETER_EXPOSUREMODE_T(MMAL_PARAMETER_HEADER_T hdr, MmalParamExposuremodeT value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_PARAMETER_EXPOSUREMETERINGMODE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        MmalParamExposuremeteringmodeT value;

        public MmalParamExposuremeteringmodeT Value => this.value;

        public MMAL_PARAMETER_EXPOSUREMETERINGMODE_T(MMAL_PARAMETER_HEADER_T hdr, MmalParamExposuremeteringmodeT value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_PARAMETER_AWBMODE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        MmalParamAwbmodeT value;

        public MmalParamAwbmodeT Value => this.value;

        public MMAL_PARAMETER_AWBMODE_T(MMAL_PARAMETER_HEADER_T hdr, MmalParamAwbmodeT value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_PARAMETER_IMAGEFX_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParamImagefxT value;

        public MmalParamImagefxT Value => value;

        public MMAL_PARAMETER_IMAGEFX_T(MMAL_PARAMETER_HEADER_T hdr, MmalParamImagefxT value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_IMAGEFX_PARAMETERS_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        MmalParamImagefxT effect;
        int numEffectParams;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MmalParametersCamera.MmalMaxImageFxParameters)]
        int[] effectParameter;

        public MmalParamImagefxT Effect => this.effect;

        public int NumEffectParams => this.numEffectParams;

        public int[] EffectParameter => this.effectParameter;

        public MMAL_PARAMETER_IMAGEFX_PARAMETERS_T(MMAL_PARAMETER_HEADER_T hdr, MmalParamImagefxT effect, int numEffectParams, int[] effectParameter)
        {
            this.Hdr = hdr;
            this.effect = effect;
            this.numEffectParams = numEffectParams;
            this.effectParameter = effectParameter;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_COLOURFX_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        int enable;
        int u;
        int v;

        public int Enable => this.enable;

        public int U => this.u;

        public int V => this.v;

        public MMAL_PARAMETER_COLOURFX_T(MMAL_PARAMETER_HEADER_T hdr, int enable, int u, int v)
        {
            this.Hdr = hdr;
            this.enable = enable;
            this.u = u;
            this.v = v;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAMERA_STC_MODE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalCameraStcModeT value;

        public MmalCameraStcModeT Value => this.value;

        public MMAL_PARAMETER_CAMERA_STC_MODE_T(MMAL_PARAMETER_HEADER_T hdr, MmalCameraStcModeT value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_FLICKERAVOID_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParamFlickeravoidT value;

        public MmalParamFlickeravoidT Value => value;

        public MMAL_PARAMETER_FLICKERAVOID_T(MMAL_PARAMETER_HEADER_T hdr, MmalParamFlickeravoidT value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_FLASH_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParamFlashT value;

        public MmalParamFlashT Value => value;

        public MMAL_PARAMETER_FLASH_T(MMAL_PARAMETER_HEADER_T hdr, MmalParamFlashT value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_REDEYE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParamRedeyeT value;

        public MmalParamRedeyeT Value => value;

        public MMAL_PARAMETER_REDEYE_T(MMAL_PARAMETER_HEADER_T hdr, MmalParamRedeyeT value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_FOCUS_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParamFocusT value;

        public MmalParamFocusT Value => value;

        public MMAL_PARAMETER_FOCUS_T(MMAL_PARAMETER_HEADER_T hdr, MmalParamFocusT value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAPTURE_STATUS_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParamCaptureStatusT value;

        public MmalParamCaptureStatusT Value => value;

        public MMAL_PARAMETER_CAPTURE_STATUS_T(MMAL_PARAMETER_HEADER_T hdr, MmalParamCaptureStatusT value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_FOCUS_STATUS_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParamFocusStatusT value;

        public MmalParamFocusStatusT Value => value;

        public MMAL_PARAMETER_FOCUS_STATUS_T(MMAL_PARAMETER_HEADER_T hdr, MmalParamFocusStatusT value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_FACE_TRACK_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParamFaceTrackModeT mode;
        uint maxRegions, frames, quality;

        public MmalParamFaceTrackModeT Value => mode;
        public uint MaxRegions => maxRegions;
        public uint Frames => frames;
        public uint Quality => quality;

        public MMAL_PARAMETER_FACE_TRACK_T(MMAL_PARAMETER_HEADER_T hdr, MmalParamFaceTrackModeT mode,
                                           uint maxRegions, uint frames, uint quality)
        {
            this.Hdr = hdr;
            this.mode = mode;
            this.maxRegions = maxRegions;
            this.frames = frames;
            this.quality = quality;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_FACE_TRACK_FACE_T
    {
        int faceId, score;
        MMAL_RECT_T faceRect;
        MMAL_RECT_T[] eyeRect;
        MMAL_RECT_T mouthRect;

        public int FaceId => faceId;
        public int Score => score;
        public MMAL_RECT_T FaceRect => faceRect;
        public MMAL_RECT_T[] EyeRect => eyeRect;
        public MMAL_RECT_T MouthRect => mouthRect;

        public MMAL_PARAMETER_FACE_TRACK_FACE_T(int faceId, int score, MMAL_RECT_T faceRect, MMAL_RECT_T[] eyeRect, MMAL_RECT_T mouthRect)
        {
            this.faceId = faceId;
            this.score = score;
            this.faceRect = faceRect;
            this.eyeRect = eyeRect;
            this.mouthRect = mouthRect;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_FACE_TRACK_RESULTS_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        uint numFaces, frameWidth, frameHeight;
        MMAL_PARAMETER_FACE_TRACK_FACE_T[] faces;

        public uint NumFaces => numFaces;
        public uint FrameWidth => frameWidth;
        public uint FrameHeight => frameHeight;
        public MMAL_PARAMETER_FACE_TRACK_FACE_T[] Faces => faces;

        public MMAL_PARAMETER_FACE_TRACK_RESULTS_T(MMAL_PARAMETER_HEADER_T hdr, uint numFaces, uint frameWidth, uint frameHeight,
                                                   MMAL_PARAMETER_FACE_TRACK_FACE_T[] faces)
        {
            this.Hdr = hdr;
            this.numFaces = numFaces;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.faces = faces;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_PARAMETER_CAMERA_CONFIG_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        int maxStillsW, maxStillsH, stillsYUV422, oneShotStills, maxPreviewVideoW, maxPreviewVideoH, numPreviewVideoFrames,
                    stillsCaptureCircularBufferHeight, fastPreviewResume;

        MmalParameterCameraConfigTimestampModeT useSTCTimestamp;

        public int MaxStillsW => maxStillsW;
        public int MaxStillsH => maxStillsH;
        public int StillsYUV422 => stillsYUV422;
        public int OneShotStills => oneShotStills;
        public int MaxPreviewVideoW => maxPreviewVideoW;
        public int MaxPreviewVideoH => maxPreviewVideoH;
        public int NumPreviewVideoFrames => numPreviewVideoFrames;
        public int StillsCaptureCircularBufferHeight => stillsCaptureCircularBufferHeight;
        public int FastPreviewResume => fastPreviewResume;
        public MmalParameterCameraConfigTimestampModeT UseSTCTimestamp => useSTCTimestamp;

        public MMAL_PARAMETER_CAMERA_CONFIG_T(MMAL_PARAMETER_HEADER_T hdr, int maxStillsW, int maxStillsH, int stillsYUV422,
                                              int oneShotStills, int maxPreviewVideoW, int maxPreviewVideoH, int numPreviewVideoFrames,
                                              int stillsCaptureCircularBufferHeight, int fastPreviewResume,
                                              MmalParameterCameraConfigTimestampModeT useSTCTimestamp)
        {
            this.Hdr = hdr;
            this.maxStillsW = maxStillsW;
            this.maxStillsH = maxStillsH;
            this.stillsYUV422 = stillsYUV422;
            this.oneShotStills = oneShotStills;
            this.maxPreviewVideoW = maxPreviewVideoW;
            this.maxPreviewVideoH = maxPreviewVideoH;
            this.numPreviewVideoFrames = numPreviewVideoFrames;
            this.stillsCaptureCircularBufferHeight = stillsCaptureCircularBufferHeight;
            this.fastPreviewResume = fastPreviewResume;
            this.useSTCTimestamp = useSTCTimestamp;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAMERA_INFO_CAMERA_T
    {
        int portId, maxWidth, maxHeight, lensPresent;

        public int PortId => portId;
        public int MaxWidth => maxWidth;
        public int MaxHeight => maxHeight;
        public int LensPresent => lensPresent;

        public MMAL_PARAMETER_CAMERA_INFO_CAMERA_T(int portId, int maxWidth, int maxHeight, int lensPresent)
        {
            this.portId = portId;
            this.maxWidth = maxWidth;
            this.maxHeight = maxHeight;
            this.lensPresent = lensPresent;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MMAL_PARAMETER_CAMERA_INFO_CAMERA_V2_T
    {
        int portId, maxWidth, maxHeight, lensPresent;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        string cameraName;

        public int PortId => portId;
        public int MaxWidth => maxWidth;
        public int MaxHeight => maxHeight;
        public int LensPresent => lensPresent;
        public string CameraName => cameraName;

        public MMAL_PARAMETER_CAMERA_INFO_CAMERA_V2_T(int portId, int maxWidth, int maxHeight, int lensPresent, string cameraName)
        {
            this.portId = portId;
            this.maxWidth = maxWidth;
            this.maxHeight = maxHeight;
            this.lensPresent = lensPresent;
            this.cameraName = cameraName;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAMERA_INFO_FLASH_T
    {
        MmalParameterCameraInfoFlashTypeT flashType;

        public MmalParameterCameraInfoFlashTypeT FlashType => flashType;

        public MMAL_PARAMETER_CAMERA_INFO_FLASH_T(MmalParameterCameraInfoFlashTypeT flashType)
        {
            this.flashType = flashType;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAMERA_INFO_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        int numCameras, numFlashes;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        MMAL_PARAMETER_CAMERA_INFO_CAMERA_T[] cameras;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        MMAL_PARAMETER_CAMERA_INFO_FLASH_T[] flashes;

        public int NumCameras => numCameras;
        public int NumFlashes => numFlashes;
        public MMAL_PARAMETER_CAMERA_INFO_CAMERA_T[] Cameras => cameras;
        public MMAL_PARAMETER_CAMERA_INFO_FLASH_T[] Flashes => flashes;

        public MMAL_PARAMETER_CAMERA_INFO_T(MMAL_PARAMETER_HEADER_T hdr, int numCameras, int numFlashes, MMAL_PARAMETER_CAMERA_INFO_CAMERA_T[] cameras,
                                            MMAL_PARAMETER_CAMERA_INFO_FLASH_T[] flashes)
        {
            this.Hdr = hdr;
            this.numCameras = numCameras;
            this.numFlashes = numFlashes;
            this.cameras = cameras;
            this.flashes = flashes;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAMERA_INFO_V2_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        int numCameras, numFlashes;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        MMAL_PARAMETER_CAMERA_INFO_CAMERA_V2_T[] cameras;
        [MarshalAs(UnmanagedType.ByValArray,SizeConst = 2)]
        MMAL_PARAMETER_CAMERA_INFO_FLASH_T[] flashes;

        public int NumCameras => numCameras;
        public int NumFlashes => numFlashes;
        public MMAL_PARAMETER_CAMERA_INFO_CAMERA_V2_T[] Cameras => cameras;
        public MMAL_PARAMETER_CAMERA_INFO_FLASH_T[] Flashes => flashes;

        public MMAL_PARAMETER_CAMERA_INFO_V2_T(MMAL_PARAMETER_HEADER_T hdr, int numCameras, int numFlashes, MMAL_PARAMETER_CAMERA_INFO_CAMERA_V2_T[] cameras,
                                            MMAL_PARAMETER_CAMERA_INFO_FLASH_T[] flashes)
        {
            this.Hdr = hdr;
            this.numCameras = numCameras;
            this.numFlashes = numFlashes;
            this.cameras = cameras;
            this.flashes = flashes;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAPTUREMODE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParameterCapturemodeModeT mode;

        public MmalParameterCapturemodeModeT Mode => mode;

        public MMAL_PARAMETER_CAPTUREMODE_T(MMAL_PARAMETER_HEADER_T hdr, MmalParameterCapturemodeModeT mode)
        {
            this.Hdr = hdr;
            this.mode = mode;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_FOCUS_REGION_T
    {
        MMAL_RECT_T rect;
        int weight, mask;
        MmalParameterFocusRegionTypeT type;

        public MMAL_RECT_T Rect => rect;
        public int Weight => weight;
        public int Mask => mask;
        public MmalParameterFocusRegionTypeT Type => type;

        public MMAL_PARAMETER_FOCUS_REGION_T(MMAL_RECT_T rect, int weight, int mask, MmalParameterFocusRegionTypeT type)
        {
            this.rect = rect;
            this.weight = weight;
            this.mask = mask;
            this.type = type;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_FOCUS_REGIONS_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        uint numRegions;
        int lockToFaces;
        MMAL_PARAMETER_FOCUS_REGION_T[] regions;

        public uint NumRegions => numRegions;
        public int LockToFaces => lockToFaces;
        public MMAL_PARAMETER_FOCUS_REGION_T[] Regions => regions;

        public MMAL_PARAMETER_FOCUS_REGIONS_T(MMAL_PARAMETER_HEADER_T hdr, uint numRegions, int lockToFaces, MMAL_PARAMETER_FOCUS_REGION_T[] regions)
        {
            this.Hdr = hdr;
            this.numRegions = numRegions;
            this.lockToFaces = lockToFaces;
            this.regions = regions;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_PARAMETER_INPUT_CROP_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MMAL_RECT_T rect;

        public MMAL_RECT_T Rect => rect;

        public MMAL_PARAMETER_INPUT_CROP_T(MMAL_PARAMETER_HEADER_T hdr, MMAL_RECT_T rect)
        {
            this.Hdr = hdr;
            this.rect = rect;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_SENSOR_INFORMATION_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MMAL_RATIONAL_T fNumber, focalLength;
        uint modelId, manufacturerId, revision;

        public MMAL_RATIONAL_T FNumber => fNumber;
        public MMAL_RATIONAL_T FocalLength => focalLength;
        public uint ModelId => modelId;
        public uint ManufacturerId => manufacturerId;
        public uint Revision => revision;

        public MMAL_PARAMETER_SENSOR_INFORMATION_T(MMAL_PARAMETER_HEADER_T hdr, MMAL_RATIONAL_T fNumber, MMAL_RATIONAL_T focalLength,
                                                   uint modelId, uint manufacturerId, uint revision)
        {
            this.Hdr = hdr;
            this.fNumber = fNumber;
            this.focalLength = focalLength;
            this.modelId = modelId;
            this.manufacturerId = manufacturerId;
            this.revision = revision;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_FLASH_SELECT_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParameterCameraInfoFlashTypeT flashType;

        public MmalParameterCameraInfoFlashTypeT FlashType => flashType;

        public MMAL_PARAMETER_FLASH_SELECT_T(MMAL_PARAMETER_HEADER_T hdr, MmalParameterCameraInfoFlashTypeT flashType)
        {
            this.Hdr = hdr;
            this.flashType = flashType;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_FIELD_OF_VIEW_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MMAL_RATIONAL_T fovH, fovV;

        public MMAL_RATIONAL_T FovH => fovH;
        public MMAL_RATIONAL_T FovV => fovV;

        public MMAL_PARAMETER_FIELD_OF_VIEW_T(MMAL_PARAMETER_HEADER_T hdr, MMAL_RATIONAL_T fovH, MMAL_RATIONAL_T fovV)
        {
            this.Hdr = hdr;
            this.fovH = fovH;
            this.fovV = fovV;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_PARAMETER_DRC_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParameterDrcStrengthT strength;

        public MmalParameterDrcStrengthT Strength => strength;

        public MMAL_PARAMETER_DRC_T(MMAL_PARAMETER_HEADER_T hdr, MmalParameterDrcStrengthT strength)
        {
            this.Hdr = hdr;
            this.strength = strength;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_ALGORITHM_CONTROL_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParameterAlgorithmControlAlgorithmsT algorithm;
        int enabled;

        public MmalParameterAlgorithmControlAlgorithmsT Algorithm => algorithm;
        public int Enabled => enabled;

        public MMAL_PARAMETER_ALGORITHM_CONTROL_T(MMAL_PARAMETER_HEADER_T hdr, MmalParameterAlgorithmControlAlgorithmsT algorithm,
                                                  int enabled)
        {
            this.Hdr = hdr;
            this.algorithm = algorithm;
            this.enabled = enabled;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAMERA_USE_CASE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParamCameraUseCaseT useCase;

        public MmalParamCameraUseCaseT UseCase => useCase;

        public MMAL_PARAMETER_CAMERA_USE_CASE_T(MMAL_PARAMETER_HEADER_T hdr, MmalParamCameraUseCaseT useCase)
        {
            this.Hdr = hdr;
            this.useCase = useCase;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_FPS_RANGE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MMAL_RATIONAL_T fpsLow, fpsHigh;

        public MMAL_RATIONAL_T FpsLow => fpsLow;
        public MMAL_RATIONAL_T FpsHigh => fpsHigh;

        public MMAL_PARAMETER_FPS_RANGE_T(MMAL_PARAMETER_HEADER_T hdr, MMAL_RATIONAL_T fpsLow, MMAL_RATIONAL_T fpsHigh)
        {
            this.Hdr = hdr;
            this.fpsLow = fpsLow;
            this.fpsHigh = fpsHigh;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_ZEROSHUTTERLAG_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        int zeroShutterLagMode, concurrentCapture;

        public int ZeroShutterLagMode => zeroShutterLagMode;
        public int ConcurrentCapture => concurrentCapture;

        public MMAL_PARAMETER_ZEROSHUTTERLAG_T(MMAL_PARAMETER_HEADER_T hdr, int zeroShutterLagMode, int concurrentCapture)
        {
            this.Hdr = hdr;
            this.zeroShutterLagMode = zeroShutterLagMode;
            this.concurrentCapture = concurrentCapture;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_PARAMETER_AWB_GAINS_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MMAL_RATIONAL_T rGain, bGain;

        public MMAL_RATIONAL_T RGain => rGain;
        public MMAL_RATIONAL_T BGain => bGain;

        public MMAL_PARAMETER_AWB_GAINS_T(MMAL_PARAMETER_HEADER_T hdr, MMAL_RATIONAL_T rGain, MMAL_RATIONAL_T bGain)
        {
            this.Hdr = hdr;
            this.rGain = rGain;
            this.bGain = bGain;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAMERA_SETTINGS_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        int exposure;
        MMAL_RATIONAL_T analogGain, digitalGain, awbRedGain, awbBlueGain;
        int focusPosition;

        public int Exposure => exposure;
        public MMAL_RATIONAL_T AnalogGain => analogGain;
        public MMAL_RATIONAL_T DigitalGain => digitalGain;
        public MMAL_RATIONAL_T AwbRedGain => awbRedGain;
        public MMAL_RATIONAL_T AwbBlueGain => awbBlueGain;
        public int FocusPosition => focusPosition;

        public MMAL_PARAMETER_CAMERA_SETTINGS_T(MMAL_PARAMETER_HEADER_T hdr, int exposure, MMAL_RATIONAL_T analogGain,
                                                MMAL_RATIONAL_T digitalGain, MMAL_RATIONAL_T awbRedGain, MMAL_RATIONAL_T awbBlueGain,
                                                int focusPosition)
        {
            this.Hdr = hdr;
            this.exposure = exposure;
            this.analogGain = analogGain;
            this.digitalGain = digitalGain;
            this.awbRedGain = awbRedGain;
            this.awbBlueGain = awbBlueGain;
            this.focusPosition = focusPosition;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_PRIVACY_INDICATOR_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParamPrivacyIndicatorT mode;

        public MmalParamPrivacyIndicatorT Mode => mode;

        public MMAL_PARAMETER_PRIVACY_INDICATOR_T(MMAL_PARAMETER_HEADER_T hdr, MmalParamPrivacyIndicatorT mode)
        {
            this.Hdr = hdr;
            this.mode = mode;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAMERA_ANNOTATE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        int enable;
        string text;
        int showShutter, showAnalogGain, showLens, showCaf, showMotion;

        public int Enable => enable;
        public string Text => text;
        public int ShowShutter => showShutter;
        public int ShowAnalogGain => showAnalogGain;
        public int ShowLens => showLens;
        public int ShowCaf => showCaf;
        public int ShowMotion => showMotion;

        public MMAL_PARAMETER_CAMERA_ANNOTATE_T(MMAL_PARAMETER_HEADER_T hdr, int enable, string text,
                                                int showShutter, int showAnalogGain, int showLens, int showCaf, int showMotion)
        {
            this.Hdr = hdr;
            this.enable = enable;
            this.text = text;
            this.showShutter = showShutter;
            this.showAnalogGain = showAnalogGain;
            this.showLens = showLens;
            this.showCaf = showCaf;
            this.showMotion = showMotion;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAMERA_ANNOTATE_V2_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        int enable;
        string text;
        int showShutter, showAnalogGain, showLens, showCaf, showMotion;

        public int Enable => enable;
        public string Text => text;
        public int ShowShutter => showShutter;
        public int ShowAnalogGain => showAnalogGain;
        public int ShowLens => showLens;
        public int ShowCaf => showCaf;
        public int ShowMotion => showMotion;

        public MMAL_PARAMETER_CAMERA_ANNOTATE_V2_T(MMAL_PARAMETER_HEADER_T hdr, int enable, string text,
                                                int showShutter, int showAnalogGain, int showLens, int showCaf, int showMotion)
        {
            this.Hdr = hdr;
            this.enable = enable;
            this.text = text;
            this.showShutter = showShutter;
            this.showAnalogGain = showAnalogGain;
            this.showLens = showLens;
            this.showCaf = showCaf;
            this.showMotion = showMotion;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        int _enable;

        int _showShutter, _showAnalogGain, _showLens, _showCaf, _showMotion, _showFrameNum,
            _enableTextBackground, _customBackgroundColor;

        byte _customBackgroundY, _customBackgroundU, _customBackgroundV, _dummy1;
        int _customTextColor;
        byte _customTextY, _customTextU, _customTextV, _textSize;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MmalParametersCamera.MmalCameraAnnotateMaxTextLenV3)]
        byte[] _text;

        public int Enable => _enable;
        public int ShowShutter => _showShutter;
        public int ShowAnalogGain => _showAnalogGain;
        public int ShowLens => _showLens;
        public int ShowCaf => _showCaf;
        public int ShowMotion => _showMotion;
        public int ShowFrameNum => _showFrameNum;
        public int EnableTextBackground => _enableTextBackground;
        public int CustomBackgroundColor => _customBackgroundColor;
        public byte CustomBackgroundY => _customBackgroundY;
        public byte CustomBackgroundU => _customBackgroundU;
        public byte CustomBackgroundV => _customBackgroundV;
        public byte Dummy1 => _dummy1;
        public int CustomTextColor => _customTextColor;
        public byte CustomTextY => _customTextY;
        public byte CustomTextU => _customTextU;
        public byte CustomTextV => _customTextV;
        public byte TextSize => _textSize;
        public byte[] Text => _text;

        public MMAL_PARAMETER_CAMERA_ANNOTATE_V3_T(MMAL_PARAMETER_HEADER_T hdr, int enable, int showShutter, int showAnalogGain, int showLens,
                                                   int showCaf, int showMotion, int showFrameNum, int enableTextBackground, int customBackgroundColor,
                                                   byte customBackgroundY, byte customBackgroundU, byte customBackgroundV, byte dummy1,
                                                   int customTextColor, byte customTextY, byte customTextU, byte customTextV, byte textSize,
                                                   byte[] text)
        {
            this.Hdr = hdr;

            _enable = enable;
            _text = text;
            _showShutter = showShutter;
            _showAnalogGain = showAnalogGain;
            _showLens = showLens;
            _showCaf = showCaf;
            _showMotion = showMotion;
            _showFrameNum = showFrameNum;
            _enableTextBackground = enableTextBackground;
            _customBackgroundColor = customBackgroundColor;
            _customBackgroundY = customBackgroundY;
            _customBackgroundU = customBackgroundU;
            _customBackgroundV = customBackgroundV;
            _dummy1 = dummy1;
            _customTextColor = customTextColor;
            _customTextY = customTextY;
            _customTextU = customTextU;
            _customTextV = customTextV;
            _textSize = textSize;
            _text = text;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAMERA_ANNOTATE_V4_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        int _enable;

        int _showShutter, _showAnalogGain, _showLens, _showCaf, _showMotion, _showFrameNum,
                    _enableTextBackground, _customBackgroundColor;

        byte _customBackgroundY, _customBackgroundU, _customBackgroundV, _dummy1;
        int _customTextColor;
        byte _customTextY, _customTextU, _customTextV, _textSize;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MmalParametersCamera.MmalCameraAnnotateMaxTextLenV3)]
        byte[] _text;

        int _justify, _xOffset, _yOffset;

        public int Enable => _enable;
        public int ShowShutter => _showShutter;
        public int ShowAnalogGain => _showAnalogGain;
        public int ShowLens => _showLens;
        public int ShowCaf => _showCaf;
        public int ShowMotion => _showMotion;
        public int ShowFrameNum => _showFrameNum;
        public int EnableTextBackground => _enableTextBackground;
        public int CustomBackgroundColor => _customBackgroundColor;
        public byte CustomBackgroundY => _customBackgroundY;
        public byte CustomBackgroundU => _customBackgroundU;
        public byte CustomBackgroundV => _customBackgroundV;
        public byte Dummy1 => _dummy1;
        public int CustomTextColor => _customTextColor;
        public byte CustomTextY => _customTextY;
        public byte CustomTextU => _customTextU;
        public byte CustomTextV => _customTextV;
        public byte TextSize => _textSize;
        public byte[] Text => _text;
        public int Justify => _justify;
        public int XOffset => _xOffset;
        public int YOffset => _yOffset;

        public MMAL_PARAMETER_CAMERA_ANNOTATE_V4_T(MMAL_PARAMETER_HEADER_T hdr, int enable, int showShutter, int showAnalogGain, int showLens,
                                                   int showCaf, int showMotion, int showFrameNum, int enableTextBackground, int customBackgroundColor,
                                                   byte customBackgroundY, byte customBackgroundU, byte customBackgroundV, byte dummy1,
                                                   int customTextColor, byte customTextY, byte customTextU, byte customTextV, byte textSize,
                                                   byte[] text, int justify, int xOffset, int yOffset)
        {
            this.Hdr = hdr;

            _enable = enable;
            _text = text;
            _showShutter = showShutter;
            _showAnalogGain = showAnalogGain;
            _showLens = showLens;
            _showCaf = showCaf;
            _showMotion = showMotion;
            _showFrameNum = showFrameNum;
            _enableTextBackground = enableTextBackground;
            _customBackgroundColor = customBackgroundColor;
            _customBackgroundY = customBackgroundY;
            _customBackgroundU = customBackgroundU;
            _customBackgroundV = customBackgroundV;
            _dummy1 = dummy1;
            _customTextColor = customTextColor;
            _customTextY = customTextY;
            _customTextU = customTextU;
            _customTextV = customTextV;
            _textSize = textSize;
            _text = text;
            _justify = justify;
            _xOffset = xOffset;
            _yOffset = yOffset;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_PARAMETER_STEREOSCOPIC_MODE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalStereoscopicModeT mode;
        int decimate, swapEyes;

        public MmalStereoscopicModeT Mode => mode;
        public int Decimate => decimate;
        public int SwapEyes => swapEyes;

        public MMAL_PARAMETER_STEREOSCOPIC_MODE_T(MMAL_PARAMETER_HEADER_T hdr, MmalStereoscopicModeT mode,
                                                  int decimate, int swapEyes)
        {
            this.Hdr = hdr;
            this.mode = mode;
            this.decimate = decimate;
            this.swapEyes = swapEyes;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAMERA_INTERFACE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalCameraInterfaceT mode;

        public MmalCameraInterfaceT Mode => mode;

        public MMAL_PARAMETER_CAMERA_INTERFACE_T(MMAL_PARAMETER_HEADER_T hdr, MmalCameraInterfaceT mode)
        {
            this.Hdr = hdr;
            this.mode = mode;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAMERA_CLOCKING_MODE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalCameraClockingModeT mode;

        public MmalCameraClockingModeT Mode => mode;

        public MMAL_PARAMETER_CAMERA_CLOCKING_MODE_T(MMAL_PARAMETER_HEADER_T hdr, MmalCameraClockingModeT mode)
        {
            this.Hdr = hdr;
            this.mode = mode;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAMERA_RX_CONFIG_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalCameraRxConfigDecode decode;
        MmalCameraRxConfigEncode encode;
        MmalCameraRxConfigUnpack unpack;
        MmalCameraRxConfigPack pack;
        uint dataLanes, encodeBlockLength, embeddedDataLines, imageId;

        public MmalCameraRxConfigDecode Decode => decode;
        public MmalCameraRxConfigEncode Encode => encode;
        public MmalCameraRxConfigUnpack Unpack => unpack;
        public MmalCameraRxConfigPack Pack => pack;
        public uint DataLanes => dataLanes;
        public uint EncodeBlockLength => encodeBlockLength;
        public uint EmbeddedDataLanes => embeddedDataLines;
        public uint ImageId => imageId;

        public MMAL_PARAMETER_CAMERA_RX_CONFIG_T(MMAL_PARAMETER_HEADER_T hdr, MmalCameraRxConfigDecode decode,
                                                 MmalCameraRxConfigEncode encode, MmalCameraRxConfigUnpack unpack,
                                                 MmalCameraRxConfigPack pack,
                                                 uint dataLanes, uint encodeBlockLength, uint embeddedDataLines, uint imageId)
        {
            this.Hdr = hdr;
            this.decode = decode;
            this.encode = encode;
            this.unpack = unpack;
            this.pack = pack;
            this.dataLanes = dataLanes;
            this.encodeBlockLength = encodeBlockLength;
            this.embeddedDataLines = embeddedDataLines;
            this.imageId = imageId;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CAMERA_RX_TIMING_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        uint timing1, timing2, timing3, timing4, timing5, term1, term2, cpiTiming1, cpiTiming2;

        public uint Timing1 => timing1;
        public uint Timing2 => timing2;
        public uint Timing3 => timing3;
        public uint Timing4 => timing4;
        public uint Timing5 => timing5;
        public uint Term1 => term1;
        public uint Term2 => term2;
        public uint CpiTiming1 => cpiTiming1;
        public uint CpiTiming2 => cpiTiming2;

        public MMAL_PARAMETER_CAMERA_RX_TIMING_T(MMAL_PARAMETER_HEADER_T hdr, uint timing1, uint timing2, uint timing3, uint timing4,
                                                 uint timing5, uint term1, uint term2, uint cpiTiming1, uint cpiTiming2)
        {
            this.Hdr = hdr;
            this.timing1 = timing1;
            this.timing2 = timing2;
            this.timing3 = timing3;
            this.timing4 = timing4;
            this.timing5 = timing5;
            this.term1 = term1;
            this.term2 = term2;
            this.cpiTiming1 = cpiTiming1;
            this.cpiTiming2 = cpiTiming2;
        }
    }

    public static class MmalParametersVideo
    {
        public const int MmalParameterDisplayregion = MmalParametersCommon.MmalParameterGroupVideo;
        public const int MmalParameterSupportedProfiles = MmalParametersCommon.MmalParameterGroupVideo + 1;
        public const int MmalParameterProfile = MmalParametersCommon.MmalParameterGroupVideo + 2;
        public const int MmalParameterIntraperiod = MmalParametersCommon.MmalParameterGroupVideo + 3;
        public const int MmalParameterRatecontrol = MmalParametersCommon.MmalParameterGroupVideo + 4;
        public const int MmalParameterNalunitformat = MmalParametersCommon.MmalParameterGroupVideo + 5;
        public const int MmalParameterMinimiseFragmentation = MmalParametersCommon.MmalParameterGroupVideo + 6;
        public const int MmalParameterMbRowsPerSlice = MmalParametersCommon.MmalParameterGroupVideo + 7;
        public const int MmalParameterVideoLevelExtension = MmalParametersCommon.MmalParameterGroupVideo + 8;
        public const int MmalParameterVideoEedeEnable = MmalParametersCommon.MmalParameterGroupVideo + 9;
        public const int MmalParameterVideoEedeLossrate = MmalParametersCommon.MmalParameterGroupVideo + 10;
        public const int MmalParameterVideoRequestIFrame = MmalParametersCommon.MmalParameterGroupVideo + 11;
        public const int MmalParameterVideoIntraRefresh = MmalParametersCommon.MmalParameterGroupVideo + 12;
        public const int MmalParameterVideoImmutableInput = MmalParametersCommon.MmalParameterGroupVideo + 13;
        public const int MmalParameterVideoBitRate = MmalParametersCommon.MmalParameterGroupVideo + 14;
        public const int MmalParameterVideoFrameRate = MmalParametersCommon.MmalParameterGroupVideo + 15;
        public const int MmalParameterVideoEncodeMinQuant = MmalParametersCommon.MmalParameterGroupVideo + 16;
        public const int MmalParameterVideoEncodeMaxQuant = MmalParametersCommon.MmalParameterGroupVideo + 17;
        public const int MmalParameterVideoEncodeRcModel = MmalParametersCommon.MmalParameterGroupVideo + 18;
        public const int MmalParameterExtraBuffers = MmalParametersCommon.MmalParameterGroupVideo + 19;
        public const int MmalParameterVideoAlignHoriz = MmalParametersCommon.MmalParameterGroupVideo + 20;
        public const int MmalParameterVideoAlignVert = MmalParametersCommon.MmalParameterGroupVideo + 21;
        public const int MmalParameterVideoDroppablePframes = MmalParametersCommon.MmalParameterGroupVideo + 22;
        public const int MmalParameterVideoEncodeInitialQuant = MmalParametersCommon.MmalParameterGroupVideo + 23;
        public const int MmalParameterVideoEncodeQpP = MmalParametersCommon.MmalParameterGroupVideo + 24;
        public const int MmalParameterVideoEncodeRcSliceDquant = MmalParametersCommon.MmalParameterGroupVideo + 25;
        public const int MmalParameterVideoEncodeFrameLimitBits = MmalParametersCommon.MmalParameterGroupVideo + 26;
        public const int MmalParameterVideoEncodePeakRate = MmalParametersCommon.MmalParameterGroupVideo + 27;
        public const int MmalParameterVideoEncodeH264DisableCabac = MmalParametersCommon.MmalParameterGroupVideo + 28;
        public const int MmalParameterVideoEncodeH264LowLatency = MmalParametersCommon.MmalParameterGroupVideo + 29;
        public const int MmalParameterVideoEncodeH264AuDelimiters = MmalParametersCommon.MmalParameterGroupVideo + 30;
        public const int MmalParameterVideoEncodeH264DeblockIdc = MmalParametersCommon.MmalParameterGroupVideo + 31;
        public const int MmalParameterVideoEncodeH264MbIntraMode = MmalParametersCommon.MmalParameterGroupVideo + 32;
        public const int MmalParameterVideoEncodeHeaderOnOpen = MmalParametersCommon.MmalParameterGroupVideo + 33;
        public const int MmalParameterVideoEncodePrecodeForQp = MmalParametersCommon.MmalParameterGroupVideo + 34;
        public const int MmalParameterVideoDrmInitInfo = MmalParametersCommon.MmalParameterGroupVideo + 35;
        public const int MmalParameterVideoTimestampFifo = MmalParametersCommon.MmalParameterGroupVideo + 36;
        public const int MmalParameterVideoDecodeErrorConcealment = MmalParametersCommon.MmalParameterGroupVideo + 37;
        public const int MmalParameterVideoDrmProtectBuffer = MmalParametersCommon.MmalParameterGroupVideo + 38;
        public const int MmalParameterVideoDecodeConfigVd3 = MmalParametersCommon.MmalParameterGroupVideo + 39;
        public const int MmalParameterVideoEncodeH264VclHrdParameters = MmalParametersCommon.MmalParameterGroupVideo + 40;
        public const int MmalParameterVideoEncodeH264LowDelayHrdFlag = MmalParametersCommon.MmalParameterGroupVideo + 41;
        public const int MmalParameterVideoEncodeInlineHeader = MmalParametersCommon.MmalParameterGroupVideo + 42;
        public const int MmalParameterVideoEncodeSeiEnable = MmalParametersCommon.MmalParameterGroupVideo + 43;
        public const int MmalParameterVideoEncodeInlineVectors = MmalParametersCommon.MmalParameterGroupVideo + 44;
        public const int MmalParameterVideoRenderStats = MmalParametersCommon.MmalParameterGroupVideo + 45;
        public const int MmalParameterVideoInterlaceType = MmalParametersCommon.MmalParameterGroupVideo + 46;
        public const int MmalParameterVideoInterpolateTimestamps = MmalParametersCommon.MmalParameterGroupVideo + 47;
        public const int MmalParameterVideoEncodeSpsTimings = MmalParametersCommon.MmalParameterGroupVideo + 48;
        public const int MmalParameterVideoMaxNumCallbacks = MmalParametersCommon.MmalParameterGroupVideo + 49;

        public enum MmalDisplaytransformT
        {
            MmalDisplayRot0,
            MmalDisplayMirrorRot0,
            MmalDisplayMirrorRot180,
            MmalDisplayRot180,
            MmalDisplayMirrorRot90,
            MmalDisplayRot270,
            MmalDisplayRot90,
            MmalDisplayMirrorRot270,
            MmalDisplayDummy = 0x7FFFFFFF
        }

        public enum MmalDisplaymodeT
        {
            MmalDisplayModeFill,
            MmalDisplayModeLetterbox,
            MmalDisplayModeDummy = 0x7FFFFFFF
        }

        public enum MmalDisplaysetT
        {
            MmalDisplaySetNone = 0,
            MmalDisplaySetNum = 1,
            MmalDisplaySetFullscreen = 2,
            MmalDisplaySetTransform = 4,
            MmalDisplaySetDestRect = 8,
            MmalDisplaySetSrcRect = 0x10,
            MmalDisplaySetMode = 0x20,
            MmalDisplaySetPixel = 0x40,
            MmalDisplaySetNoAspect = 0x80,
            MmalDisplaySetLayer = 0x100,
            MmalDisplaySetCopyProtect = 0x200,
            MmalDisplaySetAlpha = 0x400,
            MmalDisplaySetDummy = 0x7FFFFFFF
        }

        public enum MmalVideoProfileT
        {
            MmalVideoProfileH263Baseline,
            MmalVideoProfileH263H320Coding,
            MmalVideoProfileH263Backwardcompatible,
            MmalVideoProfileH263Iswv2,
            MmalVideoProfileH263Iswv3,
            MmalVideoProfileH263Highcompression,
            MmalVideoProfileH263Internet,
            MmalVideoProfileH263Interlace,
            MmalVideoProfileH263Highlatency,
            MmalVideoProfileMp4VSimple,
            MmalVideoProfileMp4VSimplescalable,
            MmalVideoProfileMp4VCore,
            MmalVideoProfileMp4VMain,
            MmalVideoProfileMp4VNbit,
            MmalVideoProfileMp4VScalabletexture,
            MmalVideoProfileMp4VSimpleface,
            MmalVideoProfileMp4VSimplefba,
            MmalVideoProfileMp4VBasicanimated,
            MmalVideoProfileMp4VHybrid,
            MmalVideoProfileMp4VAdvancedrealtime,
            MmalVideoProfileMp4VCorescalable,
            MmalVideoProfileMp4VAdvancedcoding,
            MmalVideoProfileMp4VAdvancedcore,
            MmalVideoProfileMp4VAdvancedscalable,
            MmalVideoProfileMp4VAdvancedsimple,
            MmalVideoProfileH264Baseline,
            MmalVideoProfileH264Main,
            MmalVideoProfileH264Extended,
            MmalVideoProfileH264High,
            MmalVideoProfileH264High10,
            MmalVideoProfileH264High422,
            MmalVideoProfileH264High444,
            MmalVideoProfileH264ConstrainedBaseline,
            MmalVideoProfileDummy = 0x7FFFFFFF
        }

        public enum MmalVideoLevelT
        {
            MmalVideoLevelH26310,
            MmalVideoLevelH26320,
            MmalVideoLevelH26330,
            MmalVideoLevelH26340,
            MmalVideoLevelH26345,
            MmalVideoLevelH26350,
            MmalVideoLevelH26360,
            MmalVideoLevelH26370,
            MmalVideoLevelMp4V0,
            MmalVideoLevelMp4V0B,
            MmalVideoLevelMp4V1,
            MmalVideoLevelMp4V2,
            MmalVideoLevelMp4V3,
            MmalVideoLevelMp4V4,
            MmalVideoLevelMp4V4A,
            MmalVideoLevelMp4V5,
            MmalVideoLevelMp4V6,
            MmalVideoLevelH2641,
            MmalVideoLevelH2641B,
            MmalVideoLevelH26411,
            MmalVideoLevelH26412,
            MmalVideoLevelH26413,
            MmalVideoLevelH2642,
            MmalVideoLevelH26421,
            MmalVideoLevelH26422,
            MmalVideoLevelH2643,
            MmalVideoLevelH26431,
            MmalVideoLevelH26432,
            MmalVideoLevelH2644,
            MmalVideoLevelH26441,
            MmalVideoLevelH26442,
            MmalVideoLevelH2645,
            MmalVideoLevelH26451,
            MmalVideoLevelDummy = 0x7FFFFFFF
        }

        public enum MmalVideoRatecontrolT
        {
            MmalVideoRatecontrolDefault,
            MmalVideoRatecontrolVariable,
            MmalVideoRatecontrolConstant,
            MmalVideoRatecontrolVariableSkipFrames,
            MmalVideoRatecontrolConstantSkipFrames,
            MmalVideoRatecontrolDummy = 0x7FFFFFFF
        }

        public enum MmalVideoIntraRefreshT
        {
            MmalVideoIntraRefreshDisabled = -1,
            MmalVideoIntraRefreshCyclic,
            MmalVideoIntraRefreshAdaptive,
            MmalVideoIntraRefreshBoth,
            MmalVideoIntraRefreshKhronosextensions = 0x6F000000,
            MmalVideoIntraRefreshVendorstartunused = 0x7F000000,
            MmalVideoIntraRefreshCyclicMrows = MmalVideoIntraRefreshVendorstartunused,
            MmalVideoIntraRefreshPseudoRand = MmalVideoIntraRefreshVendorstartunused + 1,
            MmalVideoIntraRefreshMax = MmalVideoIntraRefreshVendorstartunused + 2,
            MmalVideoIntraRefreshDummy = 0x7FFFFFFF
        }

        public enum MmalVideoEncodeT
        {
            MmalVideoEncoderRcModelT = 0,
            MmalVideoEncoderRcModelDefault = 0,
            MmalVideoEncoderRcModelJvt = MmalVideoEncoderRcModelDefault,
            MmalVideoEncoderRcModelVowifi = MmalVideoEncoderRcModelDefault + 1,
            MmalVideoEncoderRcModelCbr = MmalVideoEncoderRcModelDefault + 2,
            MmalVideoEncoderRcModelLast = MmalVideoEncoderRcModelDefault + 3,
            MmalVideoEncoderRcModelDummy = 0x7FFFFFFF
        }

        public enum MmalVideoEncodeH264MbIntraModesT
        {
            MmalVideoEncoderH264Mb4X4Intra,
            MmalVideoEncoderH264Mb8X8Intra,
            MmalVideoEncoderH264Mb16X16Intra,
            MmalVideoEncoderH264MbIntraDummy = 0x7FFFFFFF
        }

        public enum MmalVideoNalunitformatT
        {
            MmalVideoNalunitformatStartcodes,
            MmalVideoNalunitformatNalunitperbuffer,
            MmalVideoNalunitformatOnebyteinterleavelength,
            MmalVideoNalunitformatTwobyteinterleavelength,
            MmalVideoNalunitformatFourbyteinterleavelength,
            MmalVideoNalunitformatDummy = 0x7FFFFFFF
        }

        public enum MmalInterlaceTypeT
        {
            MmalInterlaceProgressive,
            MmalInterlaceFieldSingleUpperFirst,
            MmalInterlaceFieldSingleLowerFirst,
            MmalInterlaceFieldsInterleavedUpperFirst,
            MmalInterlaceFieldsInterleavedLowerFirst,
            MmalInterlaceMixed,
            MmalInterlaceKhronosExtensions = 0x6F000000,
            MmalInterlaceVendorStartUnused = 0x7F000000,
            MmalInterlaceMax = 0x7FFFFFFF
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_DISPLAYREGION_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        uint set, displayNum;
        int fullscreen;
        MmalParametersVideo.MmalDisplaytransformT transform;
        MMAL_RECT_T destRect, srcRect;
        int noAspect;
        MmalParametersVideo.MmalDisplaymodeT mode;
        int pixelX, pixelY;
        int layer, copyrightRequired;
        int alpha;

        public uint Set => set;
        public uint DisplayNum => displayNum;
        public int Fullscreen => fullscreen;
        public MmalParametersVideo.MmalDisplaytransformT Transform => transform;
        public MMAL_RECT_T DestRect => destRect;
        public MMAL_RECT_T SrcRect => srcRect;
        public int NoAspect => noAspect;
        public MmalParametersVideo.MmalDisplaymodeT Mode => mode;
        public int PixelX => pixelX;
        public int PixelY => pixelY;
        public int Layer => layer;
        public int CopyrightRequired => copyrightRequired;
        public int Alpha => alpha;

        public MMAL_DISPLAYREGION_T(MMAL_PARAMETER_HEADER_T hdr, uint set, uint displayNum, int fullscreen,
                                    MmalParametersVideo.MmalDisplaytransformT transform, MMAL_RECT_T destRect, MMAL_RECT_T srcRect,
                                    int noAspect, MmalParametersVideo.MmalDisplaymodeT mode, int pixelX, int pixelY,
                                    int layer, int copyrightRequired, int alpha)
        {
            this.Hdr = hdr;
            this.set = set;
            this.displayNum = displayNum;
            this.fullscreen = fullscreen;
            this.transform = transform;
            this.destRect = destRect;
            this.srcRect = srcRect;
            this.noAspect = noAspect;
            this.mode = mode;
            this.pixelX = pixelX;
            this.pixelY = pixelY;
            this.layer = layer;
            this.copyrightRequired = copyrightRequired;
            this.alpha = alpha;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_VIDEO_PROFILE_S
    {
        MmalParametersVideo.MmalVideoProfileT profile;
        MmalParametersVideo.MmalVideoLevelT level;

        public MmalParametersVideo.MmalVideoProfileT Profile => profile;
        public MmalParametersVideo.MmalVideoLevelT Level => level;

        public MMAL_PARAMETER_VIDEO_PROFILE_S(MmalParametersVideo.MmalVideoProfileT profile, MmalParametersVideo.MmalVideoLevelT level)
        {
            this.profile = profile;
            this.level = level;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_VIDEO_PROFILE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        MMAL_PARAMETER_VIDEO_PROFILE_S[] profile;

        public MMAL_PARAMETER_VIDEO_PROFILE_S[] Profile => profile;

        public MMAL_PARAMETER_VIDEO_PROFILE_T(MMAL_PARAMETER_HEADER_T hdr, MMAL_PARAMETER_VIDEO_PROFILE_S[] profile)
        {
            this.Hdr = hdr;
            this.profile = profile;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_VIDEO_ENCODE_RC_MODEL_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        uint rcModel;

        public uint RcModel => rcModel;

        public MMAL_PARAMETER_VIDEO_ENCODE_RC_MODEL_T(MMAL_PARAMETER_HEADER_T hdr, uint rcModel)
        {
            this.Hdr = hdr;
            this.rcModel = rcModel;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_VIDEO_RATECONTROL_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParametersVideo.MmalVideoRatecontrolT control;

        public unsafe MMAL_PARAMETER_HEADER_T* HdrPtr
        {
            get
            {
                fixed (MMAL_PARAMETER_HEADER_T* ptr = &Hdr)
                {
                    return ptr;
                }
            }
        }

        public MmalParametersVideo.MmalVideoRatecontrolT Control => control;

        public MMAL_PARAMETER_VIDEO_RATECONTROL_T(MMAL_PARAMETER_HEADER_T hdr, MmalParametersVideo.MmalVideoRatecontrolT control)
        {
            this.Hdr = hdr;
            this.control = control;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_VIDEO_ENCODER_H264_MB_INTRA_MODES_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParametersVideo.MmalVideoEncodeH264MbIntraModesT mbMode;

        public MmalParametersVideo.MmalVideoEncodeH264MbIntraModesT MbMode => mbMode;

        public MMAL_PARAMETER_VIDEO_ENCODER_H264_MB_INTRA_MODES_T(MMAL_PARAMETER_HEADER_T hdr, MmalParametersVideo.MmalVideoEncodeH264MbIntraModesT mbMode)
        {
            this.Hdr = hdr;
            this.mbMode = mbMode;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_VIDEO_NALUNITFORMAT_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParametersVideo.MmalVideoNalunitformatT format;

        public MmalParametersVideo.MmalVideoNalunitformatT Format => format;

        public MMAL_PARAMETER_VIDEO_NALUNITFORMAT_T(MMAL_PARAMETER_HEADER_T hdr, MmalParametersVideo.MmalVideoNalunitformatT format)
        {
            this.Hdr = hdr;
            this.format = format;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_VIDEO_LEVEL_EXTENSION_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        uint customMaxMbps, customMaxFs, customMaxBrAndCpb;

        public uint CustomMaxMbps => customMaxMbps;
        public uint CustomMaxFs => customMaxFs;
        public uint CustomMaxBrAndCpb => customMaxBrAndCpb;

        public MMAL_PARAMETER_VIDEO_LEVEL_EXTENSION_T(MMAL_PARAMETER_HEADER_T hdr, uint customMaxMbps, uint customMaxFs, uint customMaxBrAndCpb)
        {
            this.Hdr = hdr;
            this.customMaxMbps = customMaxMbps;
            this.customMaxFs = customMaxFs;
            this.customMaxBrAndCpb = customMaxBrAndCpb;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_VIDEO_INTRA_REFRESH_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParametersVideo.MmalVideoIntraRefreshT refreshMode;
        int airMbs, airRef, cirMbs, pirMbs;

        public unsafe MMAL_PARAMETER_HEADER_T* HdrPtr
        {
            get
            {
                fixed (MMAL_PARAMETER_HEADER_T* ptr = &Hdr)
                {
                    return ptr;
                }
            }
        }

        public MmalParametersVideo.MmalVideoIntraRefreshT RefreshMode => refreshMode;
        public int AirMbs => airMbs;
        public int AirRef => airRef;
        public int CirMbs => cirMbs;
        public int PirMbs => pirMbs;

        public MMAL_PARAMETER_VIDEO_INTRA_REFRESH_T(MMAL_PARAMETER_HEADER_T hdr, MmalParametersVideo.MmalVideoIntraRefreshT refreshMode,
                                                    int airMbs, int airRef, int cirMbs, int pirMbs)
        {
            this.Hdr = hdr;
            this.refreshMode = refreshMode;
            this.airMbs = airMbs;
            this.airRef = airRef;
            this.cirMbs = cirMbs;
            this.pirMbs = pirMbs;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_VIDEO_EEDE_ENABLE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        int enable;

        public int Enable => enable;

        public MMAL_PARAMETER_VIDEO_EEDE_ENABLE_T(MMAL_PARAMETER_HEADER_T hdr, int enable)
        {
            this.Hdr = hdr;
            this.enable = enable;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_VIDEO_EEDE_LOSSRATE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        uint lossRate;

        public uint LossRate => lossRate;

        public MMAL_PARAMETER_VIDEO_EEDE_LOSSRATE_T(MMAL_PARAMETER_HEADER_T hdr, uint lossRate)
        {
            this.Hdr = hdr;
            this.lossRate = lossRate;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_VIDEO_DRM_INIT_INFO_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        uint currentTime, ticksPerSec;
        uint[] lhs;

        public uint CurrentTime => currentTime;
        public uint TicksPerSec => ticksPerSec;
        public uint[] Lhs => lhs;

        public MMAL_PARAMETER_VIDEO_DRM_INIT_INFO_T(MMAL_PARAMETER_HEADER_T hdr, uint currentTime, uint ticksPerSec, uint[] lhs)
        {
            this.Hdr = hdr;
            this.currentTime = currentTime;
            this.ticksPerSec = ticksPerSec;
            this.lhs = lhs;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_VIDEO_DRM_PROTECT_BUFFER_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        uint sizeWanted, protect, memHandle;
        IntPtr physAddr;

        public uint SizeWanted => sizeWanted;
        public uint Protect => protect;
        public uint MemHandle => memHandle;
        public IntPtr PhysAddr => physAddr;

        public MMAL_PARAMETER_VIDEO_DRM_PROTECT_BUFFER_T(MMAL_PARAMETER_HEADER_T hdr, uint sizeWanted, uint protect, uint memHandle,
                                                         IntPtr physAddr)
        {
            this.Hdr = hdr;
            this.sizeWanted = sizeWanted;
            this.protect = protect;
            this.memHandle = memHandle;
            this.physAddr = physAddr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_VIDEO_RENDER_STATS_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        int valid;
        uint match, period, phase, pixelClockNominal, hvsStatus;
        uint[] dummy;

        public int Valid => valid;
        public uint Match => match;
        public uint Period => period;
        public uint Phase => phase;
        public uint PixelClockNominal => pixelClockNominal;
        public uint HvsStatus => hvsStatus;
        public uint[] Dummy => dummy;

        public MMAL_PARAMETER_VIDEO_RENDER_STATS_T(MMAL_PARAMETER_HEADER_T hdr, int valid, uint match, uint period,
                                                   uint phase, uint pixelClockNominal, uint hvsStatus, uint[] dummy)
        {
            this.Hdr = hdr;
            this.valid = valid;
            this.match = match;
            this.period = period;
            this.phase = phase;
            this.pixelClockNominal = pixelClockNominal;
            this.hvsStatus = hvsStatus;
            this.dummy = dummy;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_VIDEO_INTERLACE_TYPE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParametersVideo.MmalInterlaceTypeT eMode;
        int bRepeatFirstField;

        public MmalParametersVideo.MmalInterlaceTypeT EMode => eMode;
        public int BRepeatFirstField => bRepeatFirstField;

        public MMAL_PARAMETER_VIDEO_INTERLACE_TYPE_T(MMAL_PARAMETER_HEADER_T hdr, MmalParametersVideo.MmalInterlaceTypeT eMode,
                                                     int bRepeatFirstField)
        {
            this.Hdr = hdr;
            this.eMode = eMode;
            this.bRepeatFirstField = bRepeatFirstField;
        }
    }

    // mmal_parameters_audio.h
    public static class MMALParametersAudio
    {
        public const int MMAL_PARAMETER_AUDIO_DESTINATION = MmalParametersCommon.MmalParameterGroupAudio;
        public const int MMAL_PARAMETER_AUDIO_LATENCY_TARGET = MmalParametersCommon.MmalParameterGroupAudio + 1;
        public const int MMAL_PARAMETER_AUDIO_SOURCE = MmalParametersCommon.MmalParameterGroupAudio + 2;
        public const int MMAL_PARAMETER_AUDIO_PASSTHROUGH = MmalParametersCommon.MmalParameterGroupAudio + 3;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_AUDIO_LATENCY_TARGET_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        int enable;
        uint filter, target, shift;
        int speedFactor, interFactor, adjCap;

        public int Enable => enable;
        public uint Filter => filter;
        public uint Target => target;
        public uint Shift => shift;
        public int SpeedFactor => speedFactor;
        public int InterFactor => interFactor;
        public int AdjCap => adjCap;

        public MMAL_PARAMETER_AUDIO_LATENCY_TARGET_T(MMAL_PARAMETER_HEADER_T hdr, int enable, uint filter, uint target, uint shift,
                                                     int speedFactor, int interFactor, int adjCap)
        {
            this.Hdr = hdr;
            this.enable = enable;
            this.filter = filter;
            this.target = target;
            this.shift = shift;
            this.speedFactor = speedFactor;
            this.interFactor = interFactor;
            this.adjCap = adjCap;
        }
    }

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

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CLOCK_UPDATE_THRESHOLD_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MMAL_CLOCK_UPDATE_THRESHOLD_T value;

        public MMAL_CLOCK_UPDATE_THRESHOLD_T Value => value;

        public MMAL_PARAMETER_CLOCK_UPDATE_THRESHOLD_T(MMAL_PARAMETER_HEADER_T hdr, MMAL_CLOCK_UPDATE_THRESHOLD_T value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CLOCK_DISCONT_THRESHOLD_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MMAL_CLOCK_DISCONT_THRESHOLD_T value;

        public MMAL_CLOCK_DISCONT_THRESHOLD_T Value => value;

        public MMAL_PARAMETER_CLOCK_DISCONT_THRESHOLD_T(MMAL_PARAMETER_HEADER_T hdr, MMAL_CLOCK_DISCONT_THRESHOLD_T value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CLOCK_REQUEST_THRESHOLD_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MMAL_CLOCK_REQUEST_THRESHOLD_T value;

        public MMAL_CLOCK_REQUEST_THRESHOLD_T Value => value;

        public MMAL_PARAMETER_CLOCK_REQUEST_THRESHOLD_T(MMAL_PARAMETER_HEADER_T hdr, MMAL_CLOCK_REQUEST_THRESHOLD_T value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CLOCK_LATENCY_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MMAL_CLOCK_LATENCY_T value;

        public MMAL_CLOCK_LATENCY_T Value => value;

        public MMAL_PARAMETER_CLOCK_LATENCY_T(MMAL_PARAMETER_HEADER_T hdr, MMAL_CLOCK_LATENCY_T value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    public enum MmalParamMirrorT
    {
        MmalParamMirrorNone,
        MmalParamMirrorVertical,
        MmalParamMirrorHorizontal,
        MmalParamMirrorBoth
    }

    public static class MmalParameters
    {
        public const string MmalComponentDefaultVideoDecoder = "vc.ril.video_decode";
        public const string MmalComponentDefaultVideoEncoder = "vc.ril.video_encode";
        public const string MmalComponentDefaultVideoRenderer = "vc.ril.video_render";
        public const string MmalComponentDefaultImageDecoder = "vc.ril.image_decode";
        public const string MmalComponentDefaultImageEncoder = "vc.ril.image_encode";
        public const string MmalComponentDefaultCamera = "vc.ril.camera";
        public const string MmalComponentDefaultVideoConverter = "vc.video_convert";
        public const string MmalComponentDefaultSplitter = "vc.splitter";
        public const string MmalComponentDefaultScheduler = "vc.scheduler";
        public const string MmalComponentDefaultVideoInjecter = "vc.video_inject";
        public const string MmalComponentDefaultVideoSplitter = "vc.ril.video_splitter";
        public const string MmalComponentDefaultAudioDecoder = "none";
        public const string MmalComponentDefaultAudioRenderer = "vc.ril.audio_render";
        public const string MmalComponentDefaultMiracast = "vc.miracast";
        public const string MmalComponentDefaultClock = "vc.clock";
        public const string MmalComponentDefaultCameraInfo = "vc.camera_info";
        
        // These components are not present in the userland headers but do exist.
        public const string MmalComponentDefaultNullSink = "vc.null_sink";
        public const string MmalComponentDefaultResizer = "vc.ril.resize";
        public const string MmalComponentDefaultImageFx = "vc.ril.image_fx";
        public const string MmalComponentIsp = "vc.ril.isp";
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_UINT64_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        ulong value;

        public ulong Value => value;

        public MMAL_PARAMETER_UINT64_T(MMAL_PARAMETER_HEADER_T hdr, ulong value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_INT64_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        long value;

        public long Value => value;

        public MMAL_PARAMETER_INT64_T(MMAL_PARAMETER_HEADER_T hdr, long value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_UINT32_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        uint value;

        public uint Value => value;

        public MMAL_PARAMETER_UINT32_T(MMAL_PARAMETER_HEADER_T hdr, uint value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_INT32_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        int value;

        public int Value => value;

        public MMAL_PARAMETER_INT32_T(MMAL_PARAMETER_HEADER_T hdr, int value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_RATIONAL_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MMAL_RATIONAL_T value;

        public MMAL_RATIONAL_T Value => value;

        public MMAL_PARAMETER_RATIONAL_T(MMAL_PARAMETER_HEADER_T hdr, MMAL_RATIONAL_T value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_BOOLEAN_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        int value;

        public int Value => value;

        public MMAL_PARAMETER_BOOLEAN_T(MMAL_PARAMETER_HEADER_T hdr, int value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_STRING_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        string value;

        public string Value => value;

        public MMAL_PARAMETER_STRING_T(MMAL_PARAMETER_HEADER_T hdr, string value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_SCALEFACTOR_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        uint scaleX, scaleY;

        public uint ScaleX => scaleX;
        public uint ScaleY => scaleY;

        public MMAL_PARAMETER_SCALEFACTOR_T(MMAL_PARAMETER_HEADER_T hdr, uint scaleX, uint scaleY)
        {
            this.Hdr = hdr;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_PARAMETER_MIRROR_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MmalParamMirrorT value;

        public MmalParamMirrorT Value => value;

        public MMAL_PARAMETER_MIRROR_T(MMAL_PARAMETER_HEADER_T hdr, MmalParamMirrorT value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_URI_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        string value;

        public string Value => value;

        public MMAL_PARAMETER_URI_T(MMAL_PARAMETER_HEADER_T hdr, string value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_ENCODING_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        int[] value;

        public int[] Value => value;

        public MMAL_PARAMETER_ENCODING_T(MMAL_PARAMETER_HEADER_T hdr, int[] value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_FRAME_RATE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        MMAL_RATIONAL_T value;

        public MMAL_RATIONAL_T Value => value;

        public MMAL_PARAMETER_FRAME_RATE_T(MMAL_PARAMETER_HEADER_T hdr, MMAL_RATIONAL_T value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CONFIGFILE_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        uint value;

        public uint Value => value;

        public MMAL_PARAMETER_CONFIGFILE_T(MMAL_PARAMETER_HEADER_T hdr, uint value)
        {
            this.Hdr = hdr;
            this.value = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PARAMETER_CONFIGFILE_CHUNK_T
    {
        public MMAL_PARAMETER_HEADER_T Hdr;
        uint size, offset;
        string data;

        public uint Size => size;
        public uint Offset => offset;
        public string Data => data;

        public MMAL_PARAMETER_CONFIGFILE_CHUNK_T(MMAL_PARAMETER_HEADER_T hdr, uint size, uint offset, string data)
        {
            this.Hdr = hdr;
            this.size = size;
            this.offset = offset;
            this.data = data;
        }
    }
}