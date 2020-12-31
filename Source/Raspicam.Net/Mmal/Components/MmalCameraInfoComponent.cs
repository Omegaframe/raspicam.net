using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using MMALSharp.Native.Parameters;
using MMALSharp.Native.Port;
using MMALSharp.Utility;
using static MMALSharp.MmalNativeExceptionHelper;

namespace MMALSharp.Mmal.Components
{
    unsafe class MmalCameraInfoComponent : MmalComponentBase, ICameraInfoComponent
    {
        public string SensorName { get; private set; }
        public int MaxWidth { get; private set; }
        public int MaxHeight { get; private set; }

        public MmalCameraInfoComponent() : base(MmalParameters.MmalComponentDefaultCameraInfo)
        {
            if (ReadV2SensorInfo())
                return;

            if (ReadV1SensorInfo())
                return;

            throw new PiCameraError("Unable to read any Camera Info from Firmware");
        }

        public override void PrintComponent()
        {
            MmalLog.Logger.LogInformation("Component: Camera info");
        }

        bool ReadV2SensorInfo()
        {
            var ptr2 = Marshal.AllocHGlobal(Marshal.SizeOf<MmalParameterCameraInfoV2Type>());
            var str2 = (MmalParameterHeaderType*)ptr2;
            str2->Id = MmalParametersCamera.MmalParameterCameraInfo;
            str2->Size = Marshal.SizeOf<MmalParameterCameraInfoV2Type>();

            try
            {
                MmalCheck(MmalPort.GetParameter(Control.Ptr, str2), string.Empty);

                var p = (IntPtr)str2;
                var s = Marshal.PtrToStructure<MmalParameterCameraInfoV2Type>(p);

                if (s.Cameras == null || s.Cameras.Length <= 0)
                    return false;

                SensorName = s.Cameras[0].CameraName;
                MaxHeight = s.Cameras[0].MaxHeight;
                MaxWidth = s.Cameras[0].MaxWidth;

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr2);
            }
        }

        bool ReadV1SensorInfo()
        {
            var ptr1 = Marshal.AllocHGlobal(Marshal.SizeOf<MmalParameterCameraInfoType>());
            var str1 = (MmalParameterHeaderType*)ptr1;
            str1->Id = MmalParametersCamera.MmalParameterCameraInfo;
            str1->Size = Marshal.SizeOf<MmalParameterCameraInfoType>();

            try
            {
                MmalCheck(MmalPort.GetParameter(Control.Ptr, str1), string.Empty);

                var p = (IntPtr)str1;
                var s = Marshal.PtrToStructure<MmalParameterCameraInfoType>(p);

                if (s.Cameras == null || s.Cameras.Length <= 0)
                    return false;

                SensorName = "OV5647";
                MaxHeight = s.Cameras[0].MaxHeight;
                MaxWidth = s.Cameras[0].MaxWidth;

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr1);
            }
        }
    }
}
