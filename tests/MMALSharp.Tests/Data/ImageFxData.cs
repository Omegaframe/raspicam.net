// <copyright file="ImageFxData.cs" company="Techyian">
// Copyright (c) Ian Auty and contributors. All rights reserved.
// Licensed under the MIT License. Please see LICENSE.txt for License info.
// </copyright>

using System.Collections.Generic;
using MMALSharp.Native;
using MMALSharp.Native.Parameters;

namespace MMALSharp.Tests.Data
{
    public class ImageFxData
    {
        public static IEnumerable<object[]> Data
            => new List<object[]>
            {
                // { Effect, throws exception }
                new object[] { MmalParamImagefxType.MmalParamImagefxCartoon, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxBlackboard, true },
                new object[] { MmalParamImagefxType.MmalParamImagefxBlur, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxColourbalance, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxColourpoint, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxColourswap, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxDenoise, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxEmboss, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxFilm, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxGpen, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxHatch, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxNegative, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxOilpaint, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxPastel, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxPosterise, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxSaturation, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxSketch, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxSolarize, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxWashedout, false },
                new object[] { MmalParamImagefxType.MmalParamImagefxWhiteboard, true }
            };
    }
}
