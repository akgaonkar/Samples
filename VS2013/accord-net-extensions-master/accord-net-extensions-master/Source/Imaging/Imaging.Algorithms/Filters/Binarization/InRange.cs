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
using System.Linq;

namespace Accord.Extensions.Imaging
{
    /// <summary>
    /// Contains extension methods for pixel in range checking.
    /// </summary>
    public static class InRangeFilter
    {
        delegate void InRangeFunc(IImage src, Array min, Array max, int[] channelIndicies, Image<Gray, byte> dest, byte valueToSet);
        static Dictionary<Type, InRangeFunc> inRangeFilters;

        static InRangeFilter()
        {
            inRangeFilters = new Dictionary<Type, InRangeFunc>();
            inRangeFilters.Add(typeof(byte), inRangeByte);
            inRangeFilters.Add(typeof(short), inRangeShort);
            inRangeFilters.Add(typeof(int), inRangeInt);
            inRangeFilters.Add(typeof(float), inRangeFloat);
            inRangeFilters.Add(typeof(double), inRangeDouble);
        }

        /// <summary>
        /// Checks and produces mask where values != 0 means that values on those indicies are in the range.
        /// </summary>
        /// <param name="img">Input image.</param>
        /// <param name="min">Minimal value.</param>
        /// <param name="max">Maximal value.</param>
        /// <param name="valueToSet">Value to set to result mask.</param>
        /// <param name="channelIndicies">Which channel indicies to check. If not used then it is assumed that all indicies are used.</param>
        /// <returns>Mask</returns>
        public static Image<Gray, byte> InRange<TColor, TDepth>(this Image<TColor, TDepth> img, TColor min, TColor max, byte valueToSet = 255, params int[] channelIndicies)
            where TColor : IColor
            where TDepth : struct
        {
            TDepth[] minArr = HelperMethods.ColorToArray<TColor, TDepth>(min);
            TDepth[] maxArr = HelperMethods.ColorToArray<TColor, TDepth>(max);

            return InRange(img, minArr, maxArr, valueToSet, channelIndicies);
        }

        internal static Image<Gray, byte> InRange<TColor, TDepth>(this Image<TColor, TDepth> img, TDepth[] minArr, TDepth[] maxArr, byte valueToSet = 255, params int[] channelIndicies)
            where TColor : IColor
            where TDepth : struct
        {
            Type depthType = img.ColorInfo.ChannelType;

            InRangeFunc inRangeFilter = null;
            if (inRangeFilters.TryGetValue(typeof(TDepth), out inRangeFilter) == false)
                throw new Exception(string.Format("InRange function can not process image of color depth type {0}", depthType));

            if (channelIndicies == null || channelIndicies.Length == 0)
                channelIndicies = Enumerable.Range(0, ColorInfo.GetInfo<TColor, TDepth>().NumberOfChannels).ToArray();

            if (channelIndicies.Length > img.ColorInfo.NumberOfChannels)
                throw new Exception("Number of processed channels must not exceed the number of available image channels!");

            ParallelProcessor<IImage, Image<Gray, byte>> proc = new ParallelProcessor<IImage, Image<Gray, byte>>(img.Size,
                                                                                         () => //called once
                                                                                         {
                                                                                             var destImg = new Image<Gray, byte>(img.Width, img.Height);
                                                                                             destImg.SetValue(new Gray(255)); //initialize destination mask.
                                                                                             return destImg;
                                                                                         },

                                                                                         (IImage srcImg, Image<Gray, byte> dstImg, Rectangle area) => //called for every thread
                                                                                         {
                                                                                             var srcPatch = srcImg.GetSubRect(area);
                                                                                             var destPatch = dstImg.GetSubRect(area);
                                                                                             inRangeFilter(srcPatch, minArr, maxArr, channelIndicies, destPatch, valueToSet);
                                                                                         }
                                                                                        /*,new ParallelOptions { ForceSequential = true}*/);

            var dest = proc.Process(img);

            return dest as Image<Gray, byte>;
        }

        #region InRange functions

        private unsafe static void inRangeByte(IImage src, Array min, Array max, int[] channelIndicies, Image<Gray, byte> dest, byte valueToSet)
        {
            int nChannels = src.ColorInfo.NumberOfChannels;
            int width = src.Width;
            int height = src.Height;

            int srcShift = src.Stride - width * nChannels * sizeof(byte);
            int destShift = dest.Stride - width * 1 * sizeof(byte);

            foreach (var channel in channelIndicies)
            {
                byte minVal = (byte)min.GetValue(channel);
                byte maxVal = (byte)max.GetValue(channel);

                byte* srcPtr = (byte*)src.ImageData + channel;
                byte* destPtr = (byte*)dest.ImageData + channel;

                for (int r = 0; r < height; r++)
                {
                    for (int c = 0; c < width; c++)
                    {
                        byte srcVal = *srcPtr;
                        bool prevVal = *destPtr != 0;

                        bool val = (srcVal >= minVal) && (srcVal <= maxVal) && prevVal;
                        *destPtr = (byte)(*((byte*)(&val)) * valueToSet); //set value if true

                        srcPtr += nChannels;
                        destPtr++;
                    }

                    srcPtr = (byte*)((byte*)srcPtr + srcShift);
                    destPtr += destShift;
                }
            }
        }

        private unsafe static void inRangeShort(IImage src, Array min, Array max, int[] channelIndicies, Image<Gray, byte> dest, byte valueToSet)
        {
            int nChannels = src.ColorInfo.NumberOfChannels;
            int width = src.Width;
            int height = src.Height;

            int srcShift = src.Stride - width * nChannels * sizeof(short);
            int destShift = dest.Stride - width * 1 * sizeof(byte);

            foreach (var channel in channelIndicies)
            {
                short minVal = (short)min.GetValue(channel);
                short maxVal = (short)max.GetValue(channel);

                short* srcPtr = (short*)src.ImageData + channel;
                byte* destPtr = (byte*)dest.ImageData + channel;

                for (int r = 0; r < height; r++)
                {
                    for (int c = 0; c < width; c++)
                    {
                        short srcVal = *srcPtr;
                        bool prevVal = *destPtr != 0;

                        bool val = (srcVal >= minVal) && (srcVal <= maxVal) && prevVal;
                        *destPtr = (byte)(*((byte*)(&val)) * valueToSet); //set value if true

                        srcPtr += nChannels;
                        destPtr++;
                    }

                    srcPtr = (short*)((byte*)srcPtr + srcShift);
                    destPtr += destShift;
                }
            }
        }

        private unsafe static void inRangeInt(IImage src, Array min, Array max, int[] channelIndicies, Image<Gray, byte> dest, byte valueToSet)
        {
            int nChannels = src.ColorInfo.NumberOfChannels;
            int width = src.Width;
            int height = src.Height;

            int srcShift = src.Stride - width * nChannels * sizeof(int);
            int destShift = dest.Stride - width * 1 * sizeof(byte);

            foreach (var channel in channelIndicies)
            {
                int minVal = (int)min.GetValue(channel);
                int maxVal = (int)max.GetValue(channel);

                int* srcPtr = (int*)src.ImageData + channel;
                byte* destPtr = (byte*)dest.ImageData + channel;

                for (int r = 0; r < height; r++)
                {
                    for (int c = 0; c < width; c++)
                    {
                        int srcVal = *srcPtr;
                        bool prevVal = *destPtr != 0;

                        bool val = (srcVal >= minVal) && (srcVal <= maxVal) && prevVal;
                        *destPtr = (byte)(*((byte*)(&val)) * valueToSet); //set value if true

                        srcPtr += nChannels;
                        destPtr++;
                    }

                    srcPtr = (int*)((byte*)srcPtr + srcShift);
                    destPtr += destShift;
                }
            }
        }

        private unsafe static void inRangeFloat(IImage src, Array min, Array max, int[] channelIndicies, Image<Gray, byte> dest, byte valueToSet)
        {
            int nChannels = src.ColorInfo.NumberOfChannels;
            int width = src.Width;
            int height = src.Height;

            int srcShift = src.Stride - width * nChannels * sizeof(float);
            int destShift = dest.Stride - width * 1 * sizeof(byte);

            foreach (var channel in channelIndicies)
            {
                float minVal = (float)min.GetValue(channel);
                float maxVal = (float)max.GetValue(channel);

                float* srcPtr = (float*)src.ImageData + channel;
                byte* destPtr = (byte*)dest.ImageData + channel;

                for (int r = 0; r < height; r++)
                {
                    for (int c = 0; c < width; c++)
                    {
                        float srcVal = *srcPtr;
                        bool prevVal = *destPtr != 0;

                        bool val = (srcVal >= minVal) && (srcVal <= maxVal) && prevVal;
                        *destPtr = (byte)(*((byte*)(&val)) * valueToSet); //set value if true

                        srcPtr += nChannels;
                        destPtr++;
                    }

                    srcPtr = (float*)((byte*)srcPtr + srcShift);
                    destPtr += destShift;
                }
            }
        }

        private unsafe static void inRangeDouble(IImage src, Array min, Array max, int[] channelIndicies, Image<Gray, byte> dest, byte valueToSet)
        {
            int nChannels = src.ColorInfo.NumberOfChannels;
            int width = src.Width;
            int height = src.Height;

            int srcShift = src.Stride - width * nChannels * sizeof(double);
            int destShift = dest.Stride - width * 1 * sizeof(byte);

            foreach (var channel in channelIndicies)
            {
                double minVal = (double)min.GetValue(channel);
                double maxVal = (double)max.GetValue(channel);

                double* srcPtr = (double*)src.ImageData + channel;
                byte* destPtr = (byte*)dest.ImageData + channel;

                for (int r = 0; r < height; r++)
                {
                    for (int c = 0; c < width; c++)
                    {
                        double srcVal = *srcPtr;
                        bool prevVal = *destPtr != 0;

                        bool val = (srcVal >= minVal) && (srcVal <= maxVal) && prevVal;
                        *destPtr = (byte)(*((byte*)(&val)) * valueToSet); //set value if true

                        srcPtr += nChannels;
                        destPtr++;
                    }

                    srcPtr = (double*)((byte*)srcPtr + srcShift);
                    destPtr += destShift;
                }
            }
        }

        #endregion

    }
}
