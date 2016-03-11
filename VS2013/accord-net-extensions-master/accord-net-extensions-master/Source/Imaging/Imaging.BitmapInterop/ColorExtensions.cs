#region Licence and Terms
// Accord.NET Extensions Framework
// https://github.com/dajuric/accord-net-extensions
//
// Copyright © Darko Jurić, 2014 
// darko.juric2@gmail.com
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU Lesser General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU Lesser General Public License for more details.
// 
//   You should have received a copy of the GNU Lesser General Public License
//   along with this program.  If not, see <https://www.gnu.org/licenses/lgpl.txt>.
//
#endregion

using System;
using System.Drawing;

namespace Accord.Extensions.Imaging
{
    /// <summary>
    /// Contains extension methods for interoperability between <see cref="System.Drawing.Color"/> and <see cref="Accord.Extensions.Imaging.ColorInfo"/>.
    /// Also provides some conversions between <see cref="System.Drawing.Color"/> and <see cref="Accord.Extensions.Imaging.Bgr32"/>.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Gets System.Drawing.Color from TColor
        /// </summary>
        /// <typeparam name="TColor">Member of IColor</typeparam>
        /// <param name="color">Color.</param>
        /// <param name="opacity">Opacity. If color has 4 channels opacity is discarded.</param>
        /// <returns>System.Drawing.Color</returns>
        public static System.Drawing.Color ToColor<TColor>(this TColor color, byte opacity = Byte.MaxValue)
            where TColor : IColor
        {
            int[] colorArr = HelperMethods.ColorToArray<TColor, int>(color);
            correctValueMapping<TColor>(ref colorArr);

            switch (colorArr.Length)
            {
                case 1:
                    return Color.FromArgb(opacity, 0, colorArr[0]);
                case 2:
                    return Color.FromArgb(opacity, colorArr[0], colorArr[1]);
                case 3:
                    return Color.FromArgb(opacity, colorArr[0], colorArr[1], colorArr[2]);
                case 4:
                    return Color.FromArgb(colorArr[0], colorArr[1], colorArr[2], colorArr[3]);
            }

            throw new Exception("Unknown color model!");
        }

        private static void correctValueMapping<TColor>(ref int[] colorArr)
             where TColor : IColor
        {
            if (ColorInfo.GetInfo<TColor, double>().ConversionCodename == "BGR") //TODO (priority: lowest): other way to do that (without harcoding) - converters ?
            {
                var temp = colorArr[0];
                colorArr[0] = colorArr[2];
                colorArr[2] = temp;
            }
        }

        /// <summary>
        /// Converts (casts) the color into 32-bit BGR color.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <returns>Bgr representation.</returns>
        public static Bgr32 ToBgr(this System.Drawing.Color color)
        {
            return new Bgr32 { B = color.B, G = color.G, R = color.R };
        }
    }
}
