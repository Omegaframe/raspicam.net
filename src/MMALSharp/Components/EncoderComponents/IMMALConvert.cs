﻿// <copyright file="IMMALConvert.cs" company="Techyian">
// Copyright (c) Ian Auty. All rights reserved.
// Licensed under the MIT License. Please see LICENSE.txt for License info.
// </copyright>

using System.Threading.Tasks;

namespace MMALSharp.Components
{
    /// <summary>
    /// Supports converting user provided image data.
    /// </summary>
    public interface IMMALConvert
    {
        /// <summary>
        /// Encodes/decodes user provided image data.
        /// </summary>
        /// <param name="outputPort">The output port to begin processing on.</param>
        /// <returns>An awaitable task.</returns>
        Task Convert(int outputPort);
    }
}
