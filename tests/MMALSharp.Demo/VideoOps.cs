using System;
using System.Threading;
using System.Threading.Tasks;
using MMALSharp.Common;
using MMALSharp.Components;
using MMALSharp.Components.EncoderComponents;
using MMALSharp.Native;
using MMALSharp.Ports;
using MMALSharp.Processing.Handlers;

namespace MMALSharp.Demo
{
    public class VideoOps : OpsBase
    {
        public override void Operations()
        {
            Console.WriteLine("\nVideo Operations:");
            
            Console.WriteLine("1.    Take Video");
            
            var key = Console.ReadKey();
            var formats = ParsePixelFormat();

            switch (key.KeyChar)
            {
                case '1':
                    TakeVideoOperations(formats.Item1, formats.Item2);
                    break;
            }
            
            Program.OperationsHandler();
        }

        void TakeVideoOperations(MmalEncoding encoding, MmalEncoding pixelFormat)
        {
            Console.WriteLine("\nPlease enter a file extension.");
            var extension = Console.ReadLine();
            Console.WriteLine("\nPlease enter the bitrate value.");
            var bitrate = Console.ReadLine();
            Console.WriteLine("\nPlease enter the number of seconds to record for.");
            var seconds = Console.ReadLine();
            
            var intSeconds = 0;

            if (!int.TryParse(bitrate, out var intBitrate) || !int.TryParse(seconds, out intSeconds))
            {
                Console.WriteLine("Invalid values entered, please try again.");
                TakeVideoOperations(encoding, pixelFormat);
            }
            
            TakeVideoManual(extension, encoding, pixelFormat, intBitrate, intSeconds).GetAwaiter().GetResult();
        }

        async Task TakeVideoManual(string extension, MmalEncoding encoding, MmalEncoding pixelFormat, int bitrate, int seconds)
        {
            using var vidCaptureHandler = new VideoStreamCaptureHandler($"/home/pi/videos/", extension);
            using var vidEncoder = new MmalVideoEncoder();
            using var renderer = new MmalVideoRenderer();

            Cam.ConfigureCameraSettings();

            var portConfig = new MmalPortConfig(encoding, pixelFormat, bitrate: bitrate);
                
            vidEncoder.ConfigureOutputPort(portConfig, vidCaptureHandler);

            Cam.Camera.VideoPort.ConnectTo(vidEncoder);
            Cam.Camera.PreviewPort.ConnectTo(renderer);
                                                  
            // Camera warm up time
            await Task.Delay(2000);
            
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));
                                
            await Cam.ProcessAsync(Cam.Camera.VideoPort, cts.Token);
        }
    }
}