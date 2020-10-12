using System.Runtime.InteropServices;
using MMALSharp.Native.Util;

namespace MMALSharp.Native.Parameters
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalParameterFaceTrackFaceType
    {
        public int FaceId;
        public int Score;
        public MmalRect FaceRect;
        public MmalRect[] EyeRect;
        public MmalRect MouthRect;

        public MmalParameterFaceTrackFaceType(int faceId, int score, MmalRect faceRect, MmalRect[] eyeRect, MmalRect mouthRect)
        {
            FaceId = faceId;
            Score = score;
            FaceRect = faceRect;
            EyeRect = eyeRect;
            MouthRect = mouthRect;
        }
    }
}