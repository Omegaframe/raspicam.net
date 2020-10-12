// <copyright file="ImageFxData.cs" company="Techyian">
// Copyright (c) Ian Auty and contributors. All rights reserved.
// Licensed under the MIT License. Please see LICENSE.txt for License info.
// </copyright>

using System.Collections.Generic;
using MMALSharp.Native;

namespace MMALSharp.Tests.Data
{
    public class ImageFxData
    {
        public static IEnumerable<object[]> Data
            => new List<object[]>
            {
                // { Effect, throws exception }
                new object[] { MmalParamImagefxT.MmalParamImagefxCartoon, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxBlackboard, true },
                new object[] { MmalParamImagefxT.MmalParamImagefxBlur, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxColourbalance, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxColourpoint, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxColourswap, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxDenoise, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxEmboss, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxFilm, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxGpen, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxHatch, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxNegative, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxOilpaint, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxPastel, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxPosterise, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxSaturation, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxSketch, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxSolarize, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxWashedout, false },
                new object[] { MmalParamImagefxT.MmalParamImagefxWhiteboard, true }
            };
    }
}
