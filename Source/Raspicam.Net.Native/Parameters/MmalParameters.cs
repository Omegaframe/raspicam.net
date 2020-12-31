namespace MMALSharp.Native.Parameters
{
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
}