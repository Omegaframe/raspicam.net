using System;
using MMALSharp.Common;
using MMALSharp.Native;

namespace MMALSharp.Demo
{
    public abstract class OpsBase
    {
        protected MalCamera Cam => MalCamera.Instance;

        public abstract void Operations();
        
        protected Tuple<MmalEncoding, MmalEncoding> ParsePixelFormat()
        {
            Console.WriteLine("\nPlease select an image format.");
            var format = Console.ReadLine();
            Console.WriteLine("\nPlease select a pixel format.");
            var pixelFormat = Console.ReadLine();
            
            if (string.IsNullOrEmpty(format) || string.IsNullOrEmpty(pixelFormat))
            {
                Console.WriteLine("Please enter valid formats.");
                this.ParsePixelFormat();
            }

            var parsedFormat = format.ParseEncoding();
            var parsedPixelFormat = format.ParseEncoding();

            if (parsedFormat == null || parsedPixelFormat == null)
            {
                Console.WriteLine("Could not parse format. Please try again.");
                this.ParsePixelFormat();
            }
            
            return new Tuple<MmalEncoding, MmalEncoding>(parsedFormat, parsedPixelFormat);
        }
    }
}