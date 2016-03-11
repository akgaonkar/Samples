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
using Accord.Extensions.Imaging;
using Accord.Extensions.Imaging.Filters;

namespace Accord.Extensions.Vision
{
    /// <summary>
    /// Storage for pyramidal images and its derivative which are used in LK flow.
    /// Very often sequential images are processed therefore calculations for both images are redundant. 
    /// This class is using this property to skip some calculations and therefore speeds up sequential frame processing by 2x!
    /// </summary>
    /// <typeparam name="TColor">Image color.</typeparam>
    public class PyrLKStorage<TColor> : IDisposable
        where TColor : IColor
    {
        /// <summary>
        /// Creates LK storage.
        /// </summary>
        /// <param name="pyrLevels">Number of pyramid levels. Minimal is 0 - only current image will be used.</param>
        public PyrLKStorage(int pyrLevels)
        {
            this.PyrLevels = pyrLevels;
        }

        /// <summary>
        /// Number of pyramidal levels
        /// </summary>
        public int PyrLevels { get; private set; }

        Image<TColor, float> prevCallCurrImg = null;
        /// <summary>
        /// Calculates pyramid and its derivations.
        /// </summary>
        /// <param name="prevImg">Previous image.</param>
        /// <param name="currImg">Current image.</param>
        public void Process(Image<TColor, float> prevImg, Image<TColor, float> currImg)
        {
            if (prevCallCurrImg != null && prevCallCurrImg.Equals(prevImg)) //reuse calculated structures if can (CurrImg is previous call CurrImg)
            {
                for (int pyrLevel = this.PyrLevels; pyrLevel >= 0; pyrLevel--)
                {
                    PrevImgPyr[pyrLevel] = CurrImgPyr[pyrLevel];
                    PrevImgPyrDerivX[pyrLevel] = CurrImgPyrDerivX[pyrLevel];
                    PrevImgPyrDerivY[pyrLevel] = CurrImgPyrDerivY[pyrLevel];
                }
            }
            else
            {
                Image<TColor, float>[] _prevImgPyr, _prevImgPyrDerivX, _prevImgPyrDerivY;
                calcPyrAndDerivatives(prevImg, this.PyrLevels, out _prevImgPyr, out _prevImgPyrDerivX, out _prevImgPyrDerivY);
                this.PrevImgPyr = _prevImgPyr;
                this.PrevImgPyrDerivX = _prevImgPyrDerivX;
                this.PrevImgPyrDerivY = _prevImgPyrDerivY;
            }

            Image<TColor, float>[] _currImgPyr, _currImgPyrDerivX, _currImgPyrDerivY;
            calcPyrAndDerivatives(currImg, this.PyrLevels, out _currImgPyr, out _currImgPyrDerivX, out _currImgPyrDerivY);
            this.CurrImgPyr = _currImgPyr;
            this.CurrImgPyrDerivX = _currImgPyrDerivX;
            this.CurrImgPyrDerivY = _currImgPyrDerivY;

            prevCallCurrImg = currImg;
        }

        private static void calcPyrAndDerivatives(Image<TColor, float> img, int levels, out Image<TColor, float>[] pyr, out Image<TColor, float>[] derivX, out Image<TColor, float>[] derivY)
        {
            pyr = new Image<TColor, float>[levels + 1];
            derivX = new Image<TColor, float>[levels + 1];
            derivY = new Image<TColor, float>[levels + 1];

            for (int pyrLevel = levels; pyrLevel >= 0; pyrLevel--)
            { 
                var imPyr = img.PyrDown(pyrLevel);

                pyr[pyrLevel] = imPyr;
                derivX[pyrLevel] = imPyr.Sobel(xOrder: 1, yOrder: 0, apertureSize: 3, normalizeKernel: true);
                derivY[pyrLevel] = imPyr.Sobel(xOrder: 0, yOrder: 1, apertureSize: 3, normalizeKernel: true);
            }
        }

        /// <summary>
        /// Previous image pyramid.
        /// </summary>
        public Image<TColor, float>[] PrevImgPyr { get; private set; }
        /// <summary>
        /// Current image pyramid.
        /// </summary>
        public Image<TColor, float>[] CurrImgPyr { get; private set; }

        /// <summary>
        /// Previous image pyramidal horizontal derivatives
        /// </summary>
        public Image<TColor, float>[] PrevImgPyrDerivX { get; private set; }
        /// <summary>
        /// Current image pyramidal horizontal derivatives
        /// </summary>
        public Image<TColor, float>[] CurrImgPyrDerivX { get; private set; }
        /// <summary>
        /// Previous image pyramidal vertical derivatives
        /// </summary>
        public Image<TColor, float>[] PrevImgPyrDerivY { get; private set; }
        /// <summary>
        /// Current image pyramidal vertical derivatives
        /// </summary>
        public Image<TColor, float>[] CurrImgPyrDerivY { get; private set; }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            this.PrevImgPyr = null;
            this.CurrImgPyr = null;

            this.PrevImgPyrDerivX = null;
            this.PrevImgPyrDerivY = null;
            this.CurrImgPyrDerivX = null;
            this.CurrImgPyrDerivY = null;
        }
    }
}
