﻿using MMALSharp.Native;
using System.Collections.Generic;

namespace MMALSharp
{
    internal static class MMALParameterHelpers
    {
        /// <summary>
        /// Contains a list of parameters which can be used with <see cref="PortExtensions.GetParameter"/> and <see cref="PortExtensions.SetParameter"/>.
        /// </summary>
        public static IReadOnlyCollection<Parameter> ParameterHelper = new List<Parameter>
        {
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_ANTISHAKE, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_ANTISHAKE"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_BRIGHTNESS, typeof(MMAL_PARAMETER_RATIONAL_T), "MMAL_PARAMETER_BRIGHTNESS"),
            new Parameter(MMALParametersCommon.MMAL_PARAMETER_BUFFER_FLAG_FILTER, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_BUFFER_FLAG_FILTER"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_CAMERA_BURST_CAPTURE, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_CAMERA_BURST_CAPTURE"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_CAMERA_CUSTOM_SENSOR_CONFIG, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_CAMERA_CUSTOM_SENSOR_CONFIG"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_CAMERA_MIN_ISO, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_CAMERA_MIN_ISO"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_CAMERA_NUM, typeof(MMAL_PARAMETER_INT32_T), "MMAL_PARAMETER_CAMERA_NUM"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_CAPTURE_EXPOSURE_COMP, typeof(MMAL_PARAMETER_INT32_T), "MMAL_PARAMETER_CAPTURE_EXPOSURE_COMP"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_CAPTURE, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_CAPTURE"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_CAPTURE_STATS_PASS, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_CAPTURE_STATS_PASS"),
            new Parameter(MMALParametersClock.MMAL_PARAMETER_CLOCK_ACTIVE, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_CLOCK_ACTIVE"),
            new Parameter(MMALParametersClock.MMAL_PARAMETER_CLOCK_ENABLE_BUFFER_INFO, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_CLOCK_ENABLE_BUFFER_INFO"),
            new Parameter(MMALParametersClock.MMAL_PARAMETER_CLOCK_FRAME_RATE, typeof(MMAL_PARAMETER_RATIONAL_T), "MMAL_PARAMETER_CLOCK_FRAME_RATE"),
            new Parameter(MMALParametersClock.MMAL_PARAMETER_CLOCK_SCALE, typeof(MMAL_PARAMETER_RATIONAL_T), "MMAL_PARAMETER_CLOCK_SCALE"),
            new Parameter(MMALParametersClock.MMAL_PARAMETER_CLOCK_TIME, typeof(MMAL_PARAMETER_INT64_T), "MMAL_PARAMETER_CLOCK_TIME"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_CONTRAST, typeof(MMAL_PARAMETER_RATIONAL_T), "MMAL_PARAMETER_CONTRAST"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_DPF_CONFIG, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_DPF_CONFIG"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_ENABLE_RAW_CAPTURE, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_ENABLE_RAW_CAPTURE"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_EXIF_DISABLE, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_EXIF_DISABLE"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_EXPOSURE_COMP, typeof(MMAL_PARAMETER_INT32_T), "MMAL_PARAMETER_EXPOSURE_COMP"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_EXTRA_BUFFERS, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_EXTRA_BUFFERS"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_FLASH_REQUIRED, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_FLASH_REQUIRED"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_FRAME_RATE, typeof(MMAL_PARAMETER_RATIONAL_T), "MMAL_PARAMETER_FRAME_RATE"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_INTRAPERIOD, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_INTRAPERIOD"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_ISO, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_ISO"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_JPEG_ATTACH_LOG, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_JPEG_ATTACH_LOG"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_JPEG_Q_FACTOR, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_JPEG_Q_FACTOR"),
            new Parameter(MMALParametersCommon.MMAL_PARAMETER_LOCKSTEP_ENABLE, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_LOCKSTEP_ENABLE"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_MB_ROWS_PER_SLICE, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_MB_ROWS_PER_SLICE"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_MINIMISE_FRAGMENTATION, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_MINIMISE_FRAGMENTATION"),
            new Parameter(MMALParametersCommon.MMAL_PARAMETER_NO_IMAGE_PADDING, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_NO_IMAGE_PADDING"),
            new Parameter(MMALParametersCommon.MMAL_PARAMETER_POWERMON_ENABLE, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_POWERMON_ENABLE"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_ROTATION, typeof(MMAL_PARAMETER_INT32_T), "MMAL_PARAMETER_ROTATION"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_SATURATION, typeof(MMAL_PARAMETER_RATIONAL_T), "MMAL_PARAMETER_SATURATION"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_SHARPNESS, typeof(MMAL_PARAMETER_RATIONAL_T), "MMAL_PARAMETER_SHARPNESS"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_SHUTTER_SPEED, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_SHUTTER_SPEED"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_STILLS_DENOISE, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_STILLS_DENOISE"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_SW_SATURATION_DISABLE, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_SW_SATURATION_DISABLE"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_SW_SHARPEN_DISABLE, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_SW_SHARPEN_DISABLE"),
            new Parameter(MMALParametersCommon.MMAL_PARAMETER_SYSTEM_TIME, typeof(MMAL_PARAMETER_UINT64_T), "MMAL_PARAMETER_SYSTEM_TIME"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_ALIGN_HORIZ, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_VIDEO_ALIGN_HORIZ"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_ALIGN_VERT, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_VIDEO_ALIGN_VERT"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_BIT_RATE, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_VIDEO_BIT_RATE"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_VIDEO_DENOISE, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_VIDEO_DENOISE"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_DROPPABLE_PFRAMES, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_VIDEO_DROPPABLE_PFRAMES"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_ENCODE_FRAME_LIMIT_BITS, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_VIDEO_ENCODE_FRAME_LIMIT_BITS"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_ENCODE_INITIAL_QUANT, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_VIDEO_ENCODE_INITIAL_QUANT"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_ENCODE_INLINE_HEADER, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_VIDEO_ENCODE_INLINE_HEADER"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_ENCODE_INLINE_VECTORS, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_VIDEO_ENCODE_INLINE_VECTORS"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_ENCODE_MAX_QUANT, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_VIDEO_ENCODE_MAX_QUANT"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_ENCODE_MIN_QUANT, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_VIDEO_ENCODE_MIN_QUANT"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_ENCODE_PEAK_RATE, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_VIDEO_ENCODE_PEAK_RATE"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_ENCODE_QP_P, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_VIDEO_ENCODE_QP_P"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_ENCODE_RC_SLICE_DQUANT, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_VIDEO_ENCODE_RC_SLICE_DQUANT"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_ENCODE_SEI_ENABLE, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_VIDEO_ENCODE_SEI_ENABLE"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_ENCODE_SPS_TIMINGS, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_VIDEO_ENCODE_SPS_TIMINGS"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_FRAME_RATE, typeof(MMAL_PARAMETER_RATIONAL_T), "MMAL_PARAMETER_VIDEO_FRAME_RATE"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_IMMUTABLE_INPUT, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_VIDEO_IMMUTABLE_INPUT"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_INTERPOLATE_TIMESTAMPS, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_VIDEO_INTERPOLATE_TIMESTAMPS"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_MAX_NUM_CALLBACKS, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_VIDEO_MAX_NUM_CALLBACKS"),
            new Parameter(MMALParametersVideo.MMAL_PARAMETER_VIDEO_REQUEST_I_FRAME, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_VIDEO_REQUEST_I_FRAME"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_VIDEO_STABILISATION, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_VIDEO_STABILISATION"),
            new Parameter(MMALParametersCommon.MMAL_PARAMETER_ZERO_COPY, typeof(MMAL_PARAMETER_BOOLEAN_T), "MMAL_PARAMETER_ZERO_COPY"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_JPEG_RESTART_INTERVAL, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_JPEG_RESTART_INTERVAL"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_CAMERA_ISP_BLOCK_OVERRIDE, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_CAMERA_ISP_BLOCK_OVERRIDE"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_BLACK_LEVEL, typeof(MMAL_PARAMETER_UINT32_T), "MMAL_PARAMETER_BLACK_LEVEL"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_OUTPUT_SHIFT, typeof(MMAL_PARAMETER_INT32_T), "MMAL_PARAMETER_OUTPUT_SHIFT"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_CCM_SHIFT, typeof(MMAL_PARAMETER_INT32_T), "MMAL_PARAMETER_CCM_SHIFT"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_ANALOG_GAIN, typeof(MMAL_PARAMETER_RATIONAL_T), "MMAL_PARAMETER_ANALOG_GAIN"),
            new Parameter(MMALParametersCamera.MMAL_PARAMETER_DIGITAL_GAIN, typeof(MMAL_PARAMETER_RATIONAL_T), "MMAL_PARAMETER_DIGITAL_GAIN")
        };
    }
}
