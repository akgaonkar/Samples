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
using Accord.Extensions.Math;

namespace Accord.Extensions.Imaging
{
    /// <summary>
    /// Contains extension methods for working with images which color is complex number.
    /// </summary>
    public static partial class ComplexImageExtensions
    {
        //Add - standard way
        //Sub - standard way

        /// <summary>
        /// Multiplies two complex images. (imageA * imageB).
        /// </summary>
        /// <param name="imageA">First image.</param>
        /// <param name="imageB">Second image.</param>
        /// <param name="mask">If an mask is not null images are copied where mask values are != 0</param>
        /// <returns>Result image</returns>
        public unsafe static Image<Complex, TDepth> Mul<TDepth>(this Image<Complex, TDepth> imageA, Image<Complex, TDepth> imageB, Image<Gray, byte> mask = null) //TODO: CHECK
            where TDepth : struct
        {
            var maskedA = imageA;
            var maskedB = imageB;

            if(mask != null)
            {
                maskedA = imageA.CopyBlank(); imageA.CopyTo(maskedA, mask);
                maskedB = imageB.CopyBlank(); imageB.CopyTo(maskedB, mask);
            }

            Image<Gray, TDepth>[] imgA_channels = maskedA.SplitChannels(), imgB_channels = maskedB.SplitChannels();

            Image<Gray, TDepth> aRe = imgA_channels[0], aIm = imgA_channels[1];
            Image<Gray, TDepth> bRe = imgB_channels[0], bIm = imgB_channels[1];

            //real = aRe * bRe - aIm * bIm;
            var resReal = aRe.Mul(bRe).Sub(aIm.Mul(bIm));
            //imag = aRe * bIm + aIm * bRe
            var resImag = aRe.Mul(bIm).Add(aIm.Mul(bRe));

            return new Image<Complex, TDepth>(new Image<Gray, TDepth>[] { resReal, resImag });
        }

        /// <summary>
        /// Divides two complex images. (imageA / imageB).
        /// </summary>
        /// <param name="imageA">First image.</param>
        /// <param name="imageB">Second image.</param>
        /// <param name="mask">If an mask is not null images are copied where mask values are != 0</param>
        /// <returns>Result image</returns>
        public unsafe static Image<Complex, TDepth> Div<TDepth>(this Image<Complex, TDepth> imageA, Image<Complex, TDepth> imageB, Image<Gray, byte> mask = null) //TODO: CHECK
            where TDepth : struct
        {
            var maskedA = imageA;
            var maskedB = imageB;

            if (mask != null)
            {
                maskedA = imageA.CopyBlank(); imageA.CopyTo(maskedA, mask);
                maskedB = imageB.CopyBlank(); imageB.CopyTo(maskedB, mask);
            }

            Image<Gray, TDepth>[] imgA_channels = maskedA.SplitChannels(), imgB_channels = maskedB.SplitChannels();

            Image<Gray, TDepth> aRe = imgA_channels[0], aIm = imgA_channels[1];
            Image<Gray, TDepth> bRe = imgB_channels[0], bIm = imgB_channels[1];

            Image<Gray, TDepth> modulusSqr = bRe.Mul(bRe).Add(bIm.Mul(bIm));
            Image<Gray, byte> nonZeroElements = modulusSqr.InRange(new Gray(0 + 1E-5), new Gray(Single.MaxValue)); //TODO: replace this with some threshold (after implemented)
            //mask.And(nonZeroElements, inPlace:true); it is not necessary since source images are masked

            var eig = modulusSqr.CopyBlank();
            eig.SetValue(new Gray(1));
            var invModulusSqr = eig.Div(modulusSqr, false, nonZeroElements);

            //real = (aRe * bRe + aIm * bIm) * invModulusSqr;
            var resReal = aRe.Mul(bRe).Add(aIm.Mul(bIm)).Mul(invModulusSqr);
            //imag = (aIm * bRe - aRe * bIm) * invModulusSqr;
            var resImag = aIm.Mul(bRe).Sub(aRe.Mul(bIm)).Mul(invModulusSqr);

            return new Image<Complex, TDepth>(new Image<Gray, TDepth>[] { resReal, resImag });
        }

        /// <summary>
        /// Calculates magnitude. sqrt(real(image)^2 + imag(image)^2)
        /// </summary>
        /// <returns>Magnitude</returns>
        public unsafe static Image<Gray, TDepth> Magnitude<TDepth>(this Image<Complex, TDepth> image) //TODO: CHECK
           where TDepth : struct
        {
            var channels = image.SplitChannels();
            var mag = MagnitudeExtensions.Magnitude(channels[0], channels[1]);
            return mag;
        }

        /// <summary>
        /// Calculates phase. atan2(imag(image) / real(image))
        /// </summary>
        /// <returns>Magnitude</returns>
        public unsafe static Image<Gray, TDepth> Phase<TDepth>(this Image<Complex, TDepth> image) //TODO: CHECK
           where TDepth : struct
        {
            var channels = image.SplitChannels();
            var phase = PhaseExtensions.Phase(channels[1] /*Im*/, channels[0] /*re*/);
            return phase;
        }
    }
}
