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
    /// Flip image direction. 
    /// They can be used as bit flags.
    /// </summary>
    [Flags]
    public enum FlipDirection
    {
        /// <summary>
        /// No flipping.
        /// </summary>
        None = 0x0,
        /// <summary>
        /// Horizontal flipping.
        /// </summary>
        Horizontal = 0x1,
        /// <summary>
        /// Vertical flipping
        /// </summary>
        Vertical = 0x2,
        /// <summary>
        /// All flipping (horizontal + vertical).
        /// </summary>
        All = 0x3
    }

    /// <summary>
    /// Contains extension methods for image flipping.
    /// </summary>
    public static class ImageFlipping
    {
        delegate void ImageFlipFunc(IImage srcImg, IImage dstImg, FlipDirection flip);
        static Dictionary<Type, ImageFlipFunc> imageFlipFuncs = null;

        static ImageFlipping()
        {
            imageFlipFuncs = new Dictionary<Type, ImageFlipFunc>();
            imageFlipFuncs.Add(typeof(byte), flipImage_Byte);
            imageFlipFuncs.Add(typeof(short), flipImage_Short);
            imageFlipFuncs.Add(typeof(int), flipImage_Int);
            imageFlipFuncs.Add(typeof(float), flipImage_Float);
            imageFlipFuncs.Add(typeof(double), flipImage_Double);
        }

        /// <summary>
        /// Flips an input image horizontally / vertically / both directions / or none (data copy).
        /// </summary>
        /// <param name="img">Input image.</param>
        /// <param name="flip">Flip direction.</param>
        /// <param name="inPlace">Do it in place.</param>
        /// <returns>Returns flipped image. If <paramref name="inPlace"/> was set to true the result can be omitted.</returns>
        public static Image<TColor, TDepth> FlipImage<TColor, TDepth>(this Image<TColor, TDepth> img, FlipDirection flip, bool inPlace = false)
            where TColor:IColor
            where TDepth:struct
        { 
            return FlipImage((IImage)img, flip, inPlace) as Image<TColor, TDepth>;
        }

        /// <summary>
        /// Flips an input image horizontally / vertically / both directions / or none (data copy).
        /// </summary>
        /// <param name="img">Input image.</param>
        /// <param name="flip">Flip direction.</param>
        /// <param name="inPlace">Do it in place.</param>
        /// <returns>Returns flipped image. If <paramref name="inPlace"/> was set to true the result can be omitted.</returns>
        public static IImage FlipImage(this IImage img, FlipDirection flip, bool inPlace = false)
        {
            IImage dest = img;
            if (!inPlace)
                dest = img.CopyBlank();
           
            FlipImage((IImage)img, dest, flip);
            return dest;
        }

        /// <summary>
        /// Flips an input image horizontally / vertically / both directions / or none (data copy).
        /// </summary>
        /// <param name="srcImg">Source image.</param>
        /// <param name="dstImg">DEstination image. Must have the same size as source image.</param>
        /// <param name="flip">Flip direction.</param>
        public static void FlipImage(IImage srcImg, IImage dstImg, FlipDirection flip)
        {
            Type channelType = srcImg.ColorInfo.ChannelType;

            ImageFlipFunc flipFunc = null;
            if (imageFlipFuncs.TryGetValue(channelType, out flipFunc) == false)
                throw new Exception(string.Format("FlipImage can no process an image of type {0}", channelType));

            flipFunc(srcImg, dstImg, flip);
        }

        private unsafe static void flipImage_Byte(IImage srcImg, IImage dstImg, FlipDirection flip)
        {
            int dstStartRow = 0, verticalShift = 0; //for vertical flipping
            int startCol = 0, direction = 1; //for horizontal flipping

            if ((flip & FlipDirection.Vertical) != 0)
            {
                dstStartRow = dstImg.Stride * (dstImg.Height - 1);
                verticalShift = -2 * dstImg.Stride;
            }
            if ((flip & FlipDirection.Horizontal) != 0)
            {
                startCol = (dstImg.Width - 1) * dstImg.ColorInfo.NumberOfChannels * sizeof(byte);
                direction = -1;
            }

            byte* srcPtr = (byte*)srcImg.ImageData;
            byte* dstPtr = (byte*)((byte*)dstImg.ImageData + dstStartRow + startCol);

            int nChannels = srcImg.ColorInfo.NumberOfChannels;
            int dstStride = dstImg.Stride;
            int offset = srcImg.Stride - srcImg.Width * nChannels * sizeof(byte);

            int width = srcImg.Width;
            int height = srcImg.Height;

            for (int row = 0; row < height; row++)
            {
                byte* dstRowPtr = dstPtr;

                for (int col = 0; col < width; col++)
                {
                    for (int ch = 0; ch < nChannels; ch++)
                    {
                        dstRowPtr[ch] = srcPtr[ch];
                    }

                    srcPtr += nChannels;
                    dstRowPtr += nChannels * direction;
                }

                srcPtr = (byte*)((byte*)srcPtr + offset);
                dstPtr = (byte*)((byte*)dstPtr + dstStride + verticalShift);
            }
        }

        private unsafe static void flipImage_Short(IImage srcImg, IImage dstImg, FlipDirection flip)
        {
            int dstStartRow = 0, verticalShift = 0; //for vertical flipping
            int startCol = 0, direction = 1; //for horizontal flipping

            if ((flip & FlipDirection.Vertical) != 0)
            {
                dstStartRow = dstImg.Stride * (dstImg.Height - 1);
                verticalShift = -2 * dstImg.Stride;
            }
            if ((flip & FlipDirection.Horizontal) != 0)
            {
                startCol = (dstImg.Width - 1) * dstImg.ColorInfo.NumberOfChannels * sizeof(short);
                direction = -1;
            }

            short* srcPtr = (short*)srcImg.ImageData;
            short* dstPtr = (short*)((byte*)dstImg.ImageData + dstStartRow + startCol);

            int nChannels = srcImg.ColorInfo.NumberOfChannels;
            int dstStride = dstImg.Stride;
            int offset = srcImg.Stride - srcImg.Width * nChannels * sizeof(short);

            int width = srcImg.Width;
            int height = srcImg.Height;

            for (int row = 0; row < height; row++)
            {
                short* dstRowPtr = dstPtr;

                for (int col = 0; col < width; col++)
                {
                    for (int ch = 0; ch < nChannels; ch++)
                    {
                        dstRowPtr[ch] = srcPtr[ch];
                    }

                    srcPtr += nChannels;
                    dstRowPtr += nChannels * direction;
                }

                srcPtr = (short*)((byte*)srcPtr + offset);
                dstPtr = (short*)((byte*)dstPtr + dstStride + verticalShift);
            }
        }

        private unsafe static void flipImage_Int(IImage srcImg, IImage dstImg, FlipDirection flip)
        {
            int dstStartRow = 0, verticalShift = 0; //for vertical flipping
            int startCol = 0, direction = 1; //for horizontal flipping

            if ((flip & FlipDirection.Vertical) != 0)
            {
                dstStartRow = dstImg.Stride * (dstImg.Height - 1);
                verticalShift = -2 * dstImg.Stride;
            }
            if ((flip & FlipDirection.Horizontal) != 0)
            {
                startCol = (dstImg.Width - 1) * dstImg.ColorInfo.NumberOfChannels * sizeof(int);
                direction = -1;
            }

            int* srcPtr = (int*)srcImg.ImageData;
            int* dstPtr = (int*)((byte*)dstImg.ImageData + dstStartRow + startCol);

            int nChannels = srcImg.ColorInfo.NumberOfChannels;
            int dstStride = dstImg.Stride;
            int offset = srcImg.Stride - srcImg.Width * nChannels * sizeof(int);

            int width = srcImg.Width;
            int height = srcImg.Height;

            for (int row = 0; row < height; row++)
            {
                int* dstRowPtr = dstPtr;

                for (int col = 0; col < width; col++)
                {
                    for (int ch = 0; ch < nChannels; ch++)
                    {
                        dstRowPtr[ch] = srcPtr[ch];
                    }

                    srcPtr += nChannels;
                    dstRowPtr += nChannels * direction;
                }

                srcPtr = (int*)((byte*)srcPtr + offset);
                dstPtr = (int*)((byte*)dstPtr + dstStride + verticalShift);
            }
        }

        private unsafe static void flipImage_Float(IImage srcImg, IImage dstImg, FlipDirection flip)
        {
            int dstStartRow = 0, verticalShift = 0; //for vertical flipping
            int startCol = 0, direction = 1; //for horizontal flipping

            if ((flip & FlipDirection.Vertical) != 0)
            {
                dstStartRow = dstImg.Stride * (dstImg.Height - 1);
                verticalShift = -2 * dstImg.Stride;
            }
            if ((flip & FlipDirection.Horizontal) != 0)
            {
                startCol = (dstImg.Width-1) * dstImg.ColorInfo.NumberOfChannels * sizeof(float);
                direction = -1;
            }

            float* srcPtr = (float*)srcImg.ImageData;
            float* dstPtr = (float*)((byte*)dstImg.ImageData + dstStartRow + startCol);

            int nChannels = srcImg.ColorInfo.NumberOfChannels;
            int dstStride = dstImg.Stride;
            int offset = srcImg.Stride - srcImg.Width * nChannels * sizeof(float);

            int width = srcImg.Width;
            int height = srcImg.Height;

            for (int row = 0; row < height; row++)
            {
                float* dstRowPtr = dstPtr;

                for (int col = 0; col < width; col++)
                {
                    for (int ch = 0; ch < nChannels; ch++)
                    {
                        dstRowPtr[ch] = srcPtr[ch];
                    }

                    srcPtr += nChannels;
                    dstRowPtr += nChannels * direction;
                }

                srcPtr = (float*)((byte*)srcPtr + offset);
                dstPtr = (float*)((byte*)dstPtr + dstStride + verticalShift);
            }
        }

        private unsafe static void flipImage_Double(IImage srcImg, IImage dstImg, FlipDirection flip)
        {
            int dstStartRow = 0, verticalShift = 0; //for vertical flipping
            int startCol = 0, direction = 1; //for horizontal flipping

            if ((flip & FlipDirection.Vertical) != 0)
            {
                dstStartRow = dstImg.Stride * (dstImg.Height - 1);
                verticalShift = -2 * dstImg.Stride;
            }
            if ((flip & FlipDirection.Horizontal) != 0)
            {
                startCol = (dstImg.Width - 1) * dstImg.ColorInfo.NumberOfChannels * sizeof(double);
                direction = -1;
            }

            double* srcPtr = (double*)srcImg.ImageData;
            double* dstPtr = (double*)((byte*)dstImg.ImageData + dstStartRow + startCol);

            int nChannels = srcImg.ColorInfo.NumberOfChannels;
            int dstStride = dstImg.Stride;
            int offset = srcImg.Stride - srcImg.Width * nChannels * sizeof(double);

            int width = srcImg.Width;
            int height = srcImg.Height;

            for (int row = 0; row < height; row++)
            {
                double* dstRowPtr = dstPtr;

                for (int col = 0; col < width; col++)
                {
                    for (int ch = 0; ch < nChannels; ch++)
                    {
                        dstRowPtr[ch] = srcPtr[ch];
                    }

                    srcPtr += nChannels;
                    dstRowPtr += nChannels * direction;
                }

                srcPtr = (double*)((byte*)srcPtr + offset);
                dstPtr = (double*)((byte*)dstPtr + dstStride + verticalShift);
            }
        }

    }
}
