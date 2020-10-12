using MMALSharp.Native;
using System;

namespace MMALSharp
{
    public static class MMALNativeExceptionHelper
    {
        public static void MMALCheck(MMALUtil.MMAL_STATUS_T status, string message)
        {
            if (status == MMALUtil.MMAL_STATUS_T.MMAL_SUCCESS)
                return;

            throw status switch
            {
                MMALUtil.MMAL_STATUS_T.MMAL_ENOMEM => new MMALNoMemoryException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ENOSPC => new MMALNoSpaceException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_EINVAL => new MMALInvalidException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ENOSYS => new MMALNotImplementedException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ENOENT => new MMALInvalidDirectoryException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ENXIO => new MMALInvalidDeviceException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_EIO => new MMALIOException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ESPIPE => new MMALIllegalSeekException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ECORRUPT => new MMALCorruptException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ENOTREADY => new MMALComponentNotReadyException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ECONFIG => new MMALComponentNotConfiguredException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_EISCONN => new MMALPortConnectedException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_ENOTCONN => new MMALPortNotConnectedException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_EAGAIN => new MMALResourceUnavailableException(message),
                MMALUtil.MMAL_STATUS_T.MMAL_EFAULT => new MMALBadAddressException(message),
                _ => new MMALException(status, $"Unknown error occurred. {message}"),
            };
        }
    }

    public class PiCameraError : Exception
    {
        public PiCameraError(string msg) : base(msg) { }
    }

    public class MMALException : Exception
    {
        public MMALException(MMALUtil.MMAL_STATUS_T status, string message) : base($"Status: {status}. Message: {message}") { }
    }

    public class MMALNoMemoryException : MMALException
    {
        public MMALNoMemoryException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ENOMEM, $"Out of memory. {message}") { }
    }

    public class MMALNoSpaceException : MMALException
    {
        public MMALNoSpaceException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ENOSPC, $"Out of resources. {message}") { }
    }

    public class MMALInvalidException : MMALException
    {
        public MMALInvalidException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_EINVAL, $"Argument is invalid. {message}") { }
    }

    public class MMALNotImplementedException : MMALException
    {
        public MMALNotImplementedException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ENOSYS, $"Function not implemented. {message}") { }
    }

    public class MMALInvalidDirectoryException : MMALException
    {
        public MMALInvalidDirectoryException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ENOENT, $"No such file or directory. {message}") { }
    }

    public class MMALInvalidDeviceException : MMALException
    {
        public MMALInvalidDeviceException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ENXIO, $"No such device or address. {message}") { }
    }

    public class MMALIOException : MMALException
    {
        public MMALIOException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_EIO, $"I/O error. {message}") { }
    }

    public class MMALIllegalSeekException : MMALException
    {
        public MMALIllegalSeekException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ESPIPE, $"Illegal seek. {message}") { }
    }

    public class MMALCorruptException : MMALException
    {
        public MMALCorruptException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ECORRUPT, $"Data is corrupt. {message}") { }
    }

    public class MMALComponentNotReadyException : MMALException
    {
        public MMALComponentNotReadyException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ENOTREADY, $"Component is not ready. {message}") { }
    }

    public class MMALComponentNotConfiguredException : MMALException
    {
        public MMALComponentNotConfiguredException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ECONFIG, $"Component is not configured. {message}") { }
    }

    public class MMALPortConnectedException : MMALException
    {
        public MMALPortConnectedException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_EISCONN, $"Port is already connected. {message}") { }
    }

    public class MMALPortNotConnectedException : MMALException
    {
        public MMALPortNotConnectedException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_ENOTCONN, $"Port is disconnected. {message}") { }
    }

    public class MMALResourceUnavailableException : MMALException
    {
        public MMALResourceUnavailableException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_EAGAIN, $"Resource temporarily unavailable; try again later. {message}") { }
    }

    public class MMALBadAddressException : MMALException
    {
        public MMALBadAddressException(string message) : base(MMALUtil.MMAL_STATUS_T.MMAL_EFAULT, $"Bad address. {message}") { }
    }
}
