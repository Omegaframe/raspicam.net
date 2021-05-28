using System;
using Raspicam.Net.Native.Util;

namespace Raspicam.Net
{
    static class MmalNativeExceptionHelper
    {
        public static void MmalCheck(MmalStatusEnum status, string message)
        {
            if (status == MmalStatusEnum.MmalSuccess)
                return;

            throw status switch
            {
                MmalStatusEnum.MmalEnomem => new MmalNoMemoryException(message),
                MmalStatusEnum.MmalEnospc => new MmalNoSpaceException(message),
                MmalStatusEnum.MmalEinval => new MmalInvalidException(message),
                MmalStatusEnum.MmalEnosys => new MmalNotImplementedException(message),
                MmalStatusEnum.MmalEnoent => new MmalInvalidDirectoryException(message),
                MmalStatusEnum.MmalEnxio => new MmalInvalidDeviceException(message),
                MmalStatusEnum.MmalEio => new MmalIoException(message),
                MmalStatusEnum.MmalEspipe => new MmalIllegalSeekException(message),
                MmalStatusEnum.MmalEcorrupt => new MmalCorruptException(message),
                MmalStatusEnum.MmalEnotready => new MmalComponentNotReadyException(message),
                MmalStatusEnum.MmalEconfig => new MmalComponentNotConfiguredException(message),
                MmalStatusEnum.MmalEisconn => new MmalPortConnectedException(message),
                MmalStatusEnum.MmalEnotconn => new MmalPortNotConnectedException(message),
                MmalStatusEnum.MmalEagain => new MmalResourceUnavailableException(message),
                MmalStatusEnum.MmalEfault => new MmalBadAddressException(message),
                _ => new MmalException(status, $"Unknown error occurred. {message}"),
            };
        }
    }

    public class PiCameraError : Exception
    {
        public PiCameraError(string msg) : base(msg) { }
    }

    public class MmalException : Exception
    {
        public MmalException(MmalStatusEnum status, string message) : base($"Status: {status}. Message: {message}") { }
    }

    public class MmalNoMemoryException : MmalException
    {
        public MmalNoMemoryException(string message) : base(MmalStatusEnum.MmalEnomem, $"Out of memory. {message}") { }
    }

    public class MmalNoSpaceException : MmalException
    {
        public MmalNoSpaceException(string message) : base(MmalStatusEnum.MmalEnospc, $"Out of resources. {message}") { }
    }

    public class MmalInvalidException : MmalException
    {
        public MmalInvalidException(string message) : base(MmalStatusEnum.MmalEinval, $"Argument is invalid. {message}") { }
    }

    public class MmalNotImplementedException : MmalException
    {
        public MmalNotImplementedException(string message) : base(MmalStatusEnum.MmalEnosys, $"Function not implemented. {message}") { }
    }

    public class MmalInvalidDirectoryException : MmalException
    {
        public MmalInvalidDirectoryException(string message) : base(MmalStatusEnum.MmalEnoent, $"No such file or directory. {message}") { }
    }

    public class MmalInvalidDeviceException : MmalException
    {
        public MmalInvalidDeviceException(string message) : base(MmalStatusEnum.MmalEnxio, $"No such device or address. {message}") { }
    }

    public class MmalIoException : MmalException
    {
        public MmalIoException(string message) : base(MmalStatusEnum.MmalEio, $"I/O error. {message}") { }
    }

    public class MmalIllegalSeekException : MmalException
    {
        public MmalIllegalSeekException(string message) : base(MmalStatusEnum.MmalEspipe, $"Illegal seek. {message}") { }
    }

    public class MmalCorruptException : MmalException
    {
        public MmalCorruptException(string message) : base(MmalStatusEnum.MmalEcorrupt, $"Data is corrupt. {message}") { }
    }

    public class MmalComponentNotReadyException : MmalException
    {
        public MmalComponentNotReadyException(string message) : base(MmalStatusEnum.MmalEnotready, $"Component is not ready. {message}") { }
    }

    public class MmalComponentNotConfiguredException : MmalException
    {
        public MmalComponentNotConfiguredException(string message) : base(MmalStatusEnum.MmalEconfig, $"Component is not configured. {message}") { }
    }

    public class MmalPortConnectedException : MmalException
    {
        public MmalPortConnectedException(string message) : base(MmalStatusEnum.MmalEisconn, $"Port is already connected. {message}") { }
    }

    public class MmalPortNotConnectedException : MmalException
    {
        public MmalPortNotConnectedException(string message) : base(MmalStatusEnum.MmalEnotconn, $"Port is disconnected. {message}") { }
    }

    public class MmalResourceUnavailableException : MmalException
    {
        public MmalResourceUnavailableException(string message) : base(MmalStatusEnum.MmalEagain, $"Resource temporarily unavailable; try again later. {message}") { }
    }

    public class MmalBadAddressException : MmalException
    {
        public MmalBadAddressException(string message) : base(MmalStatusEnum.MmalEfault, $"Bad address. {message}") { }
    }
}
