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

namespace Accord.Extensions.Imaging
{
    /// <summary>
    /// Contains extension methods for method Max(a, b) applicable on an image.
    /// </summary>
    public static class MaxExtensions
    {
        delegate void MaxFunc(IImage img1, IImage img2, IImage maxImg);
        static Dictionary<Type, MaxFunc> maxFuncs;

        static MaxExtensions()
        {
            maxFuncs = new Dictionary<Type, MaxFunc>();
            maxFuncs.Add(typeof(byte), max_Byte);
            maxFuncs.Add(typeof(short), max_Short);
            maxFuncs.Add(typeof(int), max_Int);
            maxFuncs.Add(typeof(float), max_Float);
            maxFuncs.Add(typeof(double), max_Double);
        }

        /// <summary>
        /// Select maximal value for each channel.
        /// </summary>
        /// <param name="imageA">First image.</param>
        /// <param name="imageB">Second image.</param>
        /// <param name="inPlace">Process in place or not.</param>
        /// <returns>MAX(imageA, imageB) per channel</returns>
        public static Image<TColor, TDepth> Max<TColor, TDepth>(this Image<TColor, TDepth> imageA, Image<TColor, TDepth> imageB, bool inPlace = false)
            where TColor: IColor
            where TDepth : struct
        {
            return max(imageA, imageB, inPlace) as Image<TColor, TDepth>;
        }

        private static IImage max(IImage imageA, IImage imageB, bool inPlace)
        {
            Type channelType = imageA.ColorInfo.ChannelType;

            MaxFunc maxFunc = null;
            if (maxFuncs.TryGetValue(channelType, out maxFunc) == false)
                throw new NotSupportedException(string.Format("Can not calculate max from a image of type {0}", channelType));

            var proc = new ParallelProcessor<bool, IImage>(imageA.Size,
                                               () =>
                                               {
                                                   if (!inPlace)
                                                       return Image.Create(imageA.ColorInfo, imageA.Width, imageA.Height);
                                                   else
                                                       return imageA;
                                               },
                                               (bool _, IImage dest, Rectangle area) =>
                                               {
                                                   maxFunc(imageA.GetSubRect(area), imageB.GetSubRect(area), dest.GetSubRect(area));
                                               }
                                               /*, new ParallelOptions { ForceSequential = true }*/);

            return proc.Process(true);
        }

        #region Specific Funcs

        private unsafe static void max_Byte(IImage imageA, IImage imageB, IImage maxImage)
        {
            int width = imageA.Width;
            int height = imageA.Height;
            int nChannels = imageA.ColorInfo.NumberOfChannels;

            int srcAOffset = imageA.Stride - width * sizeof(byte) * nChannels;
            int srcBOffset = imageB.Stride - width * sizeof(byte) * nChannels;
            int dstOffset = maxImage.Stride - width * sizeof(byte) * nChannels;

            byte* srcAPtr = (byte*)imageA.ImageData;
            byte* srcBPtr = (byte*)imageB.ImageData;
            byte* dstPtr = (byte*)maxImage.ImageData;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    for (int channel = 0; channel < nChannels; channel++)
                    {
                        *dstPtr = System.Math.Max(*srcAPtr, *srcBPtr);

                        srcAPtr++;
                        srcBPtr++;
                        dstPtr++;
                    }
                }

                srcAPtr = (byte*)((byte*)srcAPtr + srcAOffset);
                srcBPtr = (byte*)((byte*)srcBPtr + srcBOffset);
                dstPtr = (byte*)((byte*)dstPtr + dstOffset);
            }
        }

        private unsafe static void max_Short(IImage imageA, IImage imageB, IImage maxImage)
        {
            int width = imageA.Width;
            int height = imageA.Height;
            int nChannels = imageA.ColorInfo.NumberOfChannels;

            int srcAOffset = imageA.Stride - width * sizeof(short) * nChannels;
            int srcBOffset = imageB.Stride - width * sizeof(short) * nChannels;
            int dstOffset = maxImage.Stride - width * sizeof(short) * nChannels;

            short* srcAPtr = (short*)imageA.ImageData;
            short* srcBPtr = (short*)imageB.ImageData;
            short* dstPtr = (short*)maxImage.ImageData;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    for (int channel = 0; channel < nChannels; channel++)
                    {
                        *dstPtr = System.Math.Max(*srcAPtr, *srcBPtr);

                        srcAPtr++;
                        srcBPtr++;
                        dstPtr++;
                    }
                }

                srcAPtr = (short*)((byte*)srcAPtr + srcAOffset);
                srcBPtr = (short*)((byte*)srcBPtr + srcBOffset);
                dstPtr = (short*)((byte*)dstPtr + dstOffset);
            }
        }

        private unsafe static void max_Int(IImage imageA, IImage imageB, IImage maxImage)
        {
            int width = imageA.Width;
            int height = imageA.Height;
            int nChannels = imageA.ColorInfo.NumberOfChannels;

            int srcAOffset = imageA.Stride - width * sizeof(int) * nChannels;
            int srcBOffset = imageB.Stride - width * sizeof(int) * nChannels;
            int dstOffset = maxImage.Stride - width * sizeof(int) * nChannels;

            int* srcAPtr = (int*)imageA.ImageData;
            int* srcBPtr = (int*)imageB.ImageData;
            int* dstPtr = (int*)maxImage.ImageData;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    for (int channel = 0; channel < nChannels; channel++)
                    {
                        *dstPtr = System.Math.Max(*srcAPtr, *srcBPtr);

                        srcAPtr++;
                        srcBPtr++;
                        dstPtr++;
                    }
                }

                srcAPtr = (int*)((byte*)srcAPtr + srcAOffset);
                srcBPtr = (int*)((byte*)srcBPtr + srcBOffset);
                dstPtr = (int*)((byte*)dstPtr + dstOffset);
            }
        }

        private unsafe static void max_Float(IImage imageA, IImage imageB, IImage maxImage)
        {
            int width = imageA.Width;
            int height = imageA.Height;
            int nChannels = imageA.ColorInfo.NumberOfChannels;

            int srcAOffset = imageA.Stride - width * sizeof(float) * nChannels;
            int srcBOffset = imageB.Stride - width * sizeof(float) * nChannels;
            int dstOffset = maxImage.Stride - width * sizeof(float) * nChannels;
         
            float* srcAPtr = (float*)imageA.ImageData;
            float* srcBPtr = (float*)imageB.ImageData;
            float* dstPtr = (float*)maxImage.ImageData;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    for (int channel = 0; channel < nChannels; channel++)
                    {
                        *dstPtr = System.Math.Max(*srcAPtr, *srcBPtr);

                        srcAPtr++;
                        srcBPtr++;
                        dstPtr++;
                    }
                }

                srcAPtr = (float*)((byte*)srcAPtr + srcAOffset);
                srcBPtr = (float*)((byte*)srcBPtr + srcBOffset);
                dstPtr = (float*)((byte*)dstPtr + dstOffset);
            }
        }

        private unsafe static void max_Double(IImage imageA, IImage imageB, IImage maxImage)
        {
            int width = imageA.Width;
            int height = imageA.Height;
            int nChannels = imageA.ColorInfo.NumberOfChannels;

            int srcAOffset = imageA.Stride - width * sizeof(double) * nChannels;
            int srcBOffset = imageB.Stride - width * sizeof(double) * nChannels;
            int dstOffset = maxImage.Stride - width * sizeof(double) * nChannels;

            double* srcAPtr = (double*)imageA.ImageData;
            double* srcBPtr = (double*)imageB.ImageData;
            double* dstPtr = (double*)maxImage.ImageData;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    for (int channel = 0; channel < nChannels; channel++)
                    {
                        *dstPtr = System.Math.Max(*srcAPtr, *srcBPtr);

                        srcAPtr++;
                        srcBPtr++;
                        dstPtr++;
                    }
                }

                srcAPtr = (double*)((byte*)srcAPtr + srcAOffset);
                srcBPtr = (double*)((byte*)srcBPtr + srcBOffset);
                dstPtr = (double*)((byte*)dstPtr + dstOffset);
            }
        }
 
        #endregion

    }
}
