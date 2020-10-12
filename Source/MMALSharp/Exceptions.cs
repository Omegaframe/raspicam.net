using MMALSharp.Native;
using System;

namespace MMALSharp
{
    public static class MmalNativeExceptionHelper
    {
        public static void MmalCheck(MmalUtil.MmalStatusT status, string message)
        {
            if (status == MmalUtil.MmalStatusT.MmalSuccess)
                return;

            throw status switch
            {
                MmalUtil.MmalStatusT.MmalEnomem => new MmalNoMemoryException(message),
                MmalUtil.MmalStatusT.MmalEnospc => new MmalNoSpaceException(message),
                MmalUtil.MmalStatusT.MmalEinval => new MmalInvalidException(message),
                MmalUtil.MmalStatusT.MmalEnosys => new MmalNotImplementedException(message),
                MmalUtil.MmalStatusT.MmalEnoent => new MmalInvalidDirectoryException(message),
                MmalUtil.MmalStatusT.MmalEnxio => new MmalInvalidDeviceException(message),
                MmalUtil.MmalStatusT.MmalEio => new MmalIoException(message),
                MmalUtil.MmalStatusT.MmalEspipe => new MmalIllegalSeekException(message),
                MmalUtil.MmalStatusT.MmalEcorrupt => new MmalCorruptException(message),
                MmalUtil.MmalStatusT.MmalEnotready => new MmalComponentNotReadyException(message),
                MmalUtil.MmalStatusT.MmalEconfig => new MmalComponentNotConfiguredException(message),
                MmalUtil.MmalStatusT.MmalEisconn => new MmalPortConnectedException(message),
                MmalUtil.MmalStatusT.MmalEnotconn => new MmalPortNotConnectedException(message),
                MmalUtil.MmalStatusT.MmalEagain => new MmalResourceUnavailableException(message),
                MmalUtil.MmalStatusT.MmalEfault => new MmalBadAddressException(message),
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
        public MmalException(MmalUtil.MmalStatusT status, string message) : base($"Status: {status}. Message: {message}") { }
    }

    public class MmalNoMemoryException : MmalException
    {
        public MmalNoMemoryException(string message) : base(MmalUtil.MmalStatusT.MmalEnomem, $"Out of memory. {message}") { }
    }

    public class MmalNoSpaceException : MmalException
    {
        public MmalNoSpaceException(string message) : base(MmalUtil.MmalStatusT.MmalEnospc, $"Out of resources. {message}") { }
    }

    public class MmalInvalidException : MmalException
    {
        public MmalInvalidException(string message) : base(MmalUtil.MmalStatusT.MmalEinval, $"Argument is invalid. {message}") { }
    }

    public class MmalNotImplementedException : MmalException
    {
        public MmalNotImplementedException(string message) : base(MmalUtil.MmalStatusT.MmalEnosys, $"Function not implemented. {message}") { }
    }

    public class MmalInvalidDirectoryException : MmalException
    {
        public MmalInvalidDirectoryException(string message) : base(MmalUtil.MmalStatusT.MmalEnoent, $"No such file or directory. {message}") { }
    }

    public class MmalInvalidDeviceException : MmalException
    {
        public MmalInvalidDeviceException(string message) : base(MmalUtil.MmalStatusT.MmalEnxio, $"No such device or address. {message}") { }
    }

    public class MmalIoException : MmalException
    {
        public MmalIoException(string message) : base(MmalUtil.MmalStatusT.MmalEio, $"I/O error. {message}") { }
    }

    public class MmalIllegalSeekException : MmalException
    {
        public MmalIllegalSeekException(string message) : base(MmalUtil.MmalStatusT.MmalEspipe, $"Illegal seek. {message}") { }
    }

    public class MmalCorruptException : MmalException
    {
        public MmalCorruptException(string message) : base(MmalUtil.MmalStatusT.MmalEcorrupt, $"Data is corrupt. {message}") { }
    }

    public class MmalComponentNotReadyException : MmalException
    {
        public MmalComponentNotReadyException(string message) : base(MmalUtil.MmalStatusT.MmalEnotready, $"Component is not ready. {message}") { }
    }

    public class MmalComponentNotConfiguredException : MmalException
    {
        public MmalComponentNotConfiguredException(string message) : base(MmalUtil.MmalStatusT.MmalEconfig, $"Component is not configured. {message}") { }
    }

    public class MmalPortConnectedException : MmalException
    {
        public MmalPortConnectedException(string message) : base(MmalUtil.MmalStatusT.MmalEisconn, $"Port is already connected. {message}") { }
    }

    public class MmalPortNotConnectedException : MmalException
    {
        public MmalPortNotConnectedException(string message) : base(MmalUtil.MmalStatusT.MmalEnotconn, $"Port is disconnected. {message}") { }
    }

    public class MmalResourceUnavailableException : MmalException
    {
        public MmalResourceUnavailableException(string message) : base(MmalUtil.MmalStatusT.MmalEagain, $"Resource temporarily unavailable; try again later. {message}") { }
    }

    public class MmalBadAddressException : MmalException
    {
        public MmalBadAddressException(string message) : base(MmalUtil.MmalStatusT.MmalEfault, $"Bad address. {message}") { }
    }
}
