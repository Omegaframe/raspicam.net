﻿using System.Runtime.InteropServices;
using MMALSharp.Native.Buffer;
using MMALSharp.Native.Internal;

namespace MMALSharp.Native
{
    public static class MmalClock
    {
        public static int MmalClockEventMagic = "CKLM".ToFourCc();
        public static int MmalClockEventReference = "CREF".ToFourCc();
        public static int MmalClockEventActive = "CACT".ToFourCc();
        public static int MmalClockEventScale = "CSCA".ToFourCc();
        public static int MmalClockEventTime = "CTIM".ToFourCc();
        public static int MmalClockEventUpdateThreshold = "CUTH".ToFourCc();
        public static int MmalClockEventDiscontThreshold = "CDTH".ToFourCc();
        public static int MmalClockEventRequestThreshold = "CRTH".ToFourCc();
        public static int MmalClockEventInputBufferInfo = "CIBI".ToFourCc();
        public static int MmalClockEventOutputBufferInfo = "COBI".ToFourCc();
        public static int MmalClockEventLatency = "CLAT".ToFourCc();
        public static int MmalClockEventInvalid = 0;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_CLOCK_UPDATE_THRESHOLD_T
    {
        long thresholdLower, thresholdUpper;

        public long ThresholdLower => this.thresholdLower;

        public long ThresholdUpper => this.thresholdUpper;

        public MMAL_CLOCK_UPDATE_THRESHOLD_T(long thresholdLower, long thresholdUpper)
        {
            this.thresholdLower = thresholdLower;
            this.thresholdUpper = thresholdUpper;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_CLOCK_DISCONT_THRESHOLD_T
    {
        long threshold, duration;

        public long Threshold => this.threshold;

        public long Duration => this.duration;

        public MMAL_CLOCK_DISCONT_THRESHOLD_T(long threshold, long duration)
        {
            this.threshold = threshold;
            this.duration = duration;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_CLOCK_REQUEST_THRESHOLD_T
    {
        long threshold;
        int thresholdEnable;

        public long Threshold => threshold;
        public int ThresholdEnable => thresholdEnable;

        public MMAL_CLOCK_REQUEST_THRESHOLD_T(long threshold, int thresholdEnable)
        {
            this.threshold = threshold;
            this.thresholdEnable = thresholdEnable;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_CLOCK_BUFFER_INFO_T
    {
        long timestamp;
        uint arrivalTime;

        public long Timestamp => timestamp;
        public uint ArrivalTime => arrivalTime;

        public MMAL_CLOCK_BUFFER_INFO_T(long timestamp, uint arrivalTime)
        {
            this.timestamp = timestamp;
            this.arrivalTime = arrivalTime;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_CLOCK_LATENCY_T
    {
        long target, attackPeriod, attackRate;

        public long Target => target;
        public long AttackPeriod => attackPeriod;
        public long AttackRate => attackRate;

        public MMAL_CLOCK_LATENCY_T(long target, long attackPeriod, long attackRate)
        {
            this.target = target;
            this.attackPeriod = attackPeriod;
            this.attackRate = attackRate;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_CLOCK_EVENT_DATA
    {
        int enable;
        MMAL_RATIONAL_T scale;
        MMAL_CLOCK_UPDATE_THRESHOLD_T updateThreshold;
        MMAL_CLOCK_DISCONT_THRESHOLD_T discontThreshold;
        MMAL_CLOCK_REQUEST_THRESHOLD_T requestThreshold;
        MMAL_CLOCK_BUFFER_INFO_T buffer;
        MMAL_CLOCK_LATENCY_T latency;

        public int Enable => enable;
        public MMAL_RATIONAL_T Scale => scale;
        public MMAL_CLOCK_UPDATE_THRESHOLD_T UpdateThreshold => updateThreshold;
        public MMAL_CLOCK_DISCONT_THRESHOLD_T DiscontThreshold => discontThreshold;
        public MMAL_CLOCK_REQUEST_THRESHOLD_T RequestThreshold => requestThreshold;
        public MMAL_CLOCK_BUFFER_INFO_T Buffer => buffer;
        public MMAL_CLOCK_LATENCY_T Latency => latency;

        public MMAL_CLOCK_EVENT_DATA(int enable, MMAL_RATIONAL_T scale, MMAL_CLOCK_UPDATE_THRESHOLD_T updateThreshold,
                                     MMAL_CLOCK_DISCONT_THRESHOLD_T discontThreshold, MMAL_CLOCK_REQUEST_THRESHOLD_T requestThreshold,
                                     MMAL_CLOCK_BUFFER_INFO_T buffer, MMAL_CLOCK_LATENCY_T latency)
        {
            this.enable = enable;
            this.scale = scale;
            this.updateThreshold = updateThreshold;
            this.discontThreshold = discontThreshold;
            this.requestThreshold = requestThreshold;
            this.buffer = buffer;
            this.latency = latency;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MMAL_CLOCK_EVENT_T
    {
        uint id, magic;
        MmalBufferHeader* buffer;
        uint padding0;
        MMAL_CLOCK_EVENT_DATA data;
        long padding1;

        public uint Id => id;
        public uint Magic => magic;
        public MmalBufferHeader* Buffer => buffer;
        public uint Padding0 => padding0;
        public MMAL_CLOCK_EVENT_DATA Data => data;
        public long Padding1 => padding1;

        public MMAL_CLOCK_EVENT_T(uint id, uint magic, MmalBufferHeader* buffer, uint padding0, MMAL_CLOCK_EVENT_DATA data, long padding1)
        {
            this.id = id;
            this.magic = magic;
            this.buffer = buffer;
            this.padding0 = padding0;
            this.data = data;
            this.padding1 = padding1;
        }
    }
}
