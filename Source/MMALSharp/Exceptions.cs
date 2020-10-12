using MMALSharp.Native;
using System;

namespace MMALSharp
{
    public static class MmalNativeExceptionHelper
    {
        public static void MmalCheck(MMALUtil.MMAL_STATUS_T status, string message)
        {
            if (status == MMALUtil.MMAL_STATUS_T.MMAL_SUCCESS)
                return;

            throw status switch
            {
                MMALUtil.MMAL_STATUS_T.MMAL_ENOMEM => new MmalNoMemoryException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ENOSPC => new MmalNoSpaceException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_EINVAL => new MmalInvalidException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ENOSYS => new MmalNotImplementedException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ENOENT => new MmalInvalidDirectoryException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ENXIO => new MmalInvalidDeviceException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_EIO => new MmalIoException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ESPIPE => new MmalIllegalSeekException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ECORRUPT => new MmalCorruptException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ENOTREADY => new MmalComponentNotReadyException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ECONFIG => new MmalComponentNotConfiguredException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_EISCONN => new MmalPortConnectedException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ENOTCONN => new MmalPortNotConnectedException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_EAGAIN => new MmalResourceUnavailableException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_EFAULT => new MmalBadAddressException(message),
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
        public MmalException(MMALUtil.MMAL_STATUS_T status, string message) : base($"Status: {status}. Message: {message}") { }
    }

    public class MmalNoMemoryException : MmalException
    {
        public MmalNoMemoryException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ENOMEM, $"Out of memory. {message}") { }
    }

    public class MmalNoSpaceException : MmalException
    {
        public MmalNoSpaceException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ENOSPC, $"Out of resources. {message}") { }
    }

    public class MmalInvalidException : MmalException
    {
        public MmalInvalidException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_EINVAL, $"Argument is invalid. {message}") { }
    }

    public class MmalNotImplementedException : MmalException
    {
        public MmalNotImplementedException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ENOSYS, $"Function not implemented. {message}") { }
    }

    public class MmalInvalidDirectoryException : MmalException
    {
        public MmalInvalidDirectoryException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ENOENT, $"No such file or directory. {message}") { }
    }

    public class MmalInvalidDeviceException : MmalException
    {
        public MmalInvalidDeviceException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ENXIO, $"No such device or address. {message}") { }
    }

    public class MmalIoException : MmalException
    {
        public MmalIoException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_EIO, $"I/O error. {message}") { }
    }

    public class MmalIllegalSeekException : MmalException
    {
        public MmalIllegalSeekException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ESPIPE, $"Illegal seek. {message}") { }
    }

    public class MmalCorruptException : MmalException
    {
        public MmalCorruptException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ECORRUPT, $"Data is corrupt. {message}") { }
    }

    public class MmalComponentNotReadyException : MmalException
    {
        public MmalComponentNotReadyException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ENOTREADY, $"Component is not ready. {message}") { }
    }

    public class MmalComponentNotConfiguredException : MmalException
    {
        public MmalComponentNotConfiguredException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ECONFIG, $"Component is not configured. {message}") { }
    }

    public class MmalPortConnectedException : MmalException
    {
        public MmalPortConnectedException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_EISCONN, $"Port is already connected. {message}") { }
    }

    public class MmalPortNotConnectedException : MmalException
    {
        public MmalPortNotConnectedException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ENOTCONN, $"Port is disconnected. {message}") { }
    }

    public class MmalResourceUnavailableException : MmalException
    {
        public MmalResourceUnavailableException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_EAGAIN, $"Resource temporarily unavailable; try again later. {message}") { }
    }

    public class MmalBadAddressException : MmalException
    {
        public MmalBadAddressException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_EFAULT, $"Bad address. {message}") { }
    }
}
