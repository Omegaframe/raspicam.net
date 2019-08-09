﻿// <copyright file="ImageFileDecodeInputPort.cs" company="Techyian">
// Copyright (c) Ian Auty. All rights reserved.
// Licensed under the MIT License. Please see LICENSE.txt for License info.
// </copyright>

using System;
using System.Threading.Tasks;
using MMALSharp.Common.Utility;
using MMALSharp.Components;
using MMALSharp.Handlers;
using MMALSharp.Native;

namespace MMALSharp.Ports.Inputs
{
    /// <summary>
    /// A custom port definition used specifically when using encoder conversion functionality.
    /// </summary>
    public unsafe class VideoFileDecodeInputPort : InputPort
    {
        /// <summary>
        /// Creates a new instance of <see cref="VideoFileDecodeInputPort"/>. 
        /// </summary>
        /// <param name="ptr">The native pointer.</param>
        /// <param name="comp">The component this port is associated with.</param>
        /// <param name="type">The type of port.</param>
        /// <param name="guid">Managed unique identifier for this port.</param>
        public VideoFileDecodeInputPort(IntPtr ptr, MMALComponentBase comp, PortType type, Guid guid)
            : base(ptr, comp, type, guid)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="VideoFileDecodeInputPort"/>. 
        /// </summary>
        /// <param name="ptr">The native pointer.</param>
        /// <param name="comp">The component this port is associated with.</param>
        /// <param name="type">The type of port.</param>
        /// <param name="guid">Managed unique identifier for this port.</param>
        /// <param name="handler">The capture handler for this port.</param>
        public VideoFileDecodeInputPort(IntPtr ptr, IComponent comp, PortType type, Guid guid, ICaptureHandler handler)
            : base(ptr, comp, type, guid, handler)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="VideoFileDecodeInputPort"/>.
        /// </summary>
        /// <param name="copyFrom">The port to copy data from.</param>
        public VideoFileDecodeInputPort(IPort copyFrom)
            : base((IntPtr)copyFrom.Ptr, copyFrom.ComponentReference, copyFrom.PortType, copyFrom.Guid, copyFrom.Handler)
        {
        }

        internal override void NativeInputPortCallback(MMAL_PORT_T* port, MMAL_BUFFER_HEADER_T* buffer)
        {
            if (MMALCameraConfig.Debug)
            {
                MMALLog.Logger.Debug("Releasing input port buffer");
            }

            var bufferImpl = new MMALBufferImpl(buffer);
            bufferImpl.Release();

            if (!this.Trigger.Task.IsCompleted)
            {
                Task.Run(() => { this.Trigger.SetResult(true); });
            }
        }
    }
}
