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
using System.Collections.Generic;
using Accord.Extensions.Math;

namespace Accord.Extensions.Imaging
{
    /// <summary>
    /// Contains extension methods for calculating an magnitude image from two images as sqrt(x * x + y * y)
    /// </summary>
    public static class MagnitudeExtensions
    {
        delegate void MagnitudeFunc(IImage img1, IImage img2, IImage magnitudeImg);
        static Dictionary<Type, MagnitudeFunc> magnitudeFuncs;

        static MagnitudeExtensions()
        {
            magnitudeFuncs = new Dictionary<Type, MagnitudeFunc>();
            magnitudeFuncs.Add(typeof(float), magnitude_Float);
            magnitudeFuncs.Add(typeof(double), magnitude_Double);
        }

        /// <summary>
        /// Calculates magnitude using Euclidean distance. 
        /// </summary>
        /// <param name="imageA">First image.</param>
        /// <param name="imageB">Second image.</param>
        /// <returns>Magnitude.</returns>
        public static Image<Gray, float> Magnitude(this Image<Gray, float> imageA, Image<Gray, float> imageB)
        {
            return Magnitude<float>(imageA, imageB);
        }

        /// <summary>
        /// Calculates magnitude using Euclidean distance. 
        /// </summary>
        /// <param name="imageA">First image.</param>
        /// <param name="imageB">Second image.</param>
        /// <returns>Magnitude.</returns>
        public static Image<Gray, double> Magnitude(this Image<Gray, double> imageA, Image<Gray, double> imageB)
        {
            return Magnitude<double>(imageA, imageB);
        }

        internal static Image<Gray, TDepth> Magnitude<TDepth>(Image<Gray, TDepth> imageA, Image<Gray, TDepth> imageB)
            where TDepth : struct
        {
            return magnitude(imageA, imageB) as Image<Gray, TDepth>;
        }

        private static IImage magnitude(IImage imageA, IImage imageB)
        {
            Type channelType = imageA.ColorInfo.ChannelType;

            MagnitudeFunc magnitudeFunc = null;
            if (magnitudeFuncs.TryGetValue(channelType, out magnitudeFunc) == false)
                throw new NotSupportedException(string.Format("Can not calculate magnitude from a image of type {0}", channelType));

            var proc = new ParallelProcessor<bool, IImage>(imageA.Size,
                                               () =>
                                               {
                                                   return Image.Create(imageA.ColorInfo, imageA.Width, imageA.Height);
                                               },
                                               (bool _, IImage dest, Rectangle area) =>
                                               {
                                                   magnitudeFunc(imageA.GetSubRect(area), imageB.GetSubRect(area), dest.GetSubRect(area));
                                               }
                                               /*, new ParallelOptions { ForceSequential = true }*/);

            return proc.Process(true);
        }

        private unsafe static void magnitude_Float(IImage imageA, IImage imageB, IImage magnitudeImage)
        {
            int width = imageA.Width;
            int height = imageA.Height;
            int srcAOffset = imageA.Stride - width * sizeof(float);
            int srcBOffset = imageB.Stride - width * sizeof(float);
            int dstOffset = magnitudeImage.Stride - width * sizeof(float);

            float* srcAPtr = (float*)imageA.ImageData;
            float* srcBPtr = (float*)imageB.ImageData;
            float* dstPtr = (float*)magnitudeImage.ImageData;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    *dstPtr = MathExtensions.Sqrt(*srcAPtr * *srcAPtr + *srcBPtr * *srcBPtr);

                    srcAPtr++;
                    srcBPtr++;
                    dstPtr++;
                }

                srcAPtr = (float*)((byte*)srcAPtr + srcAOffset);
                srcBPtr = (float*)((byte*)srcBPtr + srcBOffset);
                dstPtr = (float*)((byte*)dstPtr + dstOffset);
            }
        }

        private unsafe static void magnitude_Double(IImage imageA, IImage imageB, IImage magnitudeImage)
        {
            int width = imageA.Width;
            int height = imageA.Height;
            int srcAOffset = imageA.Stride - width * sizeof(double);
            int srcBOffset = imageB.Stride - width * sizeof(double);
            int dstOffset = magnitudeImage.Stride - width * sizeof(double);

            double* srcAPtr = (double*)imageA.ImageData;
            double* srcBPtr = (double*)imageB.ImageData;
            double* dstPtr = (double*)magnitudeImage.ImageData;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    *dstPtr = System.Math.Sqrt(*srcAPtr * *srcAPtr + *srcBPtr * *srcBPtr);

                    srcAPtr++;
                    srcBPtr++;
                    dstPtr++;
                }

                srcAPtr = (double*)((byte*)srcAPtr + srcAOffset);
                srcBPtr = (double*)((byte*)srcBPtr + srcBOffset);
                dstPtr = (double*)((byte*)dstPtr + dstOffset);
            }
        }

       

    }
}
