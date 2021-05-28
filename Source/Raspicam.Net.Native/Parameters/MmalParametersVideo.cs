namespace Raspicam.Net.Native.Parameters
{
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
}