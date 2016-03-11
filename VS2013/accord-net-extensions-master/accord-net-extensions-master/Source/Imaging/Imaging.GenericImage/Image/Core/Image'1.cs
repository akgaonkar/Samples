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
using Accord.Extensions.Imaging.Converters;

namespace Accord.Extensions.Imaging
{
    /// <summary>
    /// Implements generic image type and provides a basic data manipulation. Other functions are built as extensions making the class light-weight and portable.
    /// <para>Other extensions and satellite assemblies enables multiple interoperability with other libraries such as AForge.NET, OpenCV, EmguCV.</para>
    /// </summary>
    /// <typeparam name="TColor">Color from type <see cref="IColor"/>.</typeparam>
    /// <typeparam name="TDepth">Primitive type.</typeparam>
    public partial class Image<TColor, TDepth> : Image
                                where TColor: IColor
                                where TDepth: struct
    {
        #region Constructor methods

        /// <summary>
        /// Initializes (registers) color converters.
        /// </summary>
        static Image()
        {
            ColorDepthConverters.Initialize();
        }

        /// <summary>
        /// Do not remove! Needed for dynamic image creation via cached expressions.
        /// </summary>
        private Image()
        {
            this.ColorInfo = ColorInfo.GetInfo<TColor, TDepth>(); //an early init is needed during deserialization
        }

        /// <summary>
        /// Construct an image from channels.
        /// </summary>
        /// <param name="channels">Channels. The number of channels must be the same as number of channels specified by this color type.</param>
        public Image(Image<Gray, TDepth>[] channels)
            :this()
        {
            if (ColorInfo.NumberOfChannels != channels.Length)
                throw new Exception("Number of channels must be the same as number of channels specified by this color type!");

            int width = channels[0].Width;
            int height = channels[0].Height;

            Image.Initialize(this, width, height);
            ChannelMerger.MergeChannels<TColor, TDepth>(channels, this);
        }

        /// <summary>
        /// Constructs an image. (allocation)
        /// </summary>
        /// <param name="size">Image size.</param>
        /// <param name="strideAllignment">Stride alignment. Usual practice is that every image row ends with address aligned with 4.</param>
        public Image(Size size, int strideAllignment = 4)
            :this()
        {
            Image.Initialize(this, size.Width, size.Height, strideAllignment);
        }

        /// <summary>
        /// Constructs an image. (allocation)
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="strideAllignment">Stride alignment. Usual practice is that every image row ends with address aligned with 4.</param>
        public Image(int width, int height, int strideAllignment = 4)
            :this()
        {
            Image.Initialize(this, width, height, strideAllignment);
        }

        /// <summary>
        /// Constructs an image and sets pixel value. (allocation)
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="value">User selected image color.</param>
        public Image(int width, int height, TColor value)
            : this()
        {
            Image.Initialize(this, width, height);
            ValueSetter.SetValue(this, value);
        }

        /// <summary>
        /// Constructs an image from unmanaged data. Data is shared.
        /// </summary>
        /// <param name="imageData">Pointer to unmanaged data.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="stride">Image stride.</param>
        /// <param name="parentReference">To prevent object from deallocating use this parameter.</param>
        /// <param name="parentDestructor">If a parent needs to be destroyed or release use this function. (e.g. unpin object - GCHandle)</param>
        public Image(IntPtr imageData, int width, int height, int stride, object parentReference = null, Action<object> parentDestructor = null)
            : this()
        {
            Image.Initialize(this, imageData, width, height, stride, parentReference, parentDestructor);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets image channel.
        /// Channel size must be the same as image size.
        /// </summary>
        /// <param name="channelIdx">Index of an channel to get or replace.</param>
        /// <returns>Image channel.</returns>
        public Image<Gray, TDepth> this[int channelIdx]
        {
            get { return ((IImage)this)[channelIdx] as Image<Gray, TDepth>; }
            set { ((IImage)this)[channelIdx] = value; }
        }

        /// <summary>
        /// Gets or sets image color at an location.
        /// If you need fast access to an image pixel use unmanaged approach.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <param name="col">Column index.</param>
        /// <returns>Color</returns>
        public TColor this[int row, int col]
        {
            get
            {
                IntPtr data = GetData(row, col);
                return HelperMethods.PointerToColor<TColor, TDepth>(data);
            }
            set 
            {
                IntPtr data = GetData(row, col);
                HelperMethods.ColorToPointer<TColor, TDepth>(value, data);
            }
        }

        #endregion

        #region Basic helper methods

        /// <summary>
        /// Gets sub-image from specified area. Data is shared.
        /// </summary>
        /// <param name="rect">Area of an image for sub-image creation.</param>
        /// <returns>Sub-image.</returns>
        public Image<TColor, TDepth> GetSubRect(Rectangle rect)
        {
            return ((IImage)this).GetSubRect(rect) as Image<TColor, TDepth>;
        }

        /// <summary>
        /// Clones an image (data is copied).
        /// </summary>
        /// <returns>New cloned image.</returns>
        public Image<TColor, TDepth> Clone()
        {
            return ((IImage)this).Clone() as Image<TColor, TDepth>;
        }

        /// <summary>
        /// Copies all image information except image data.
        /// Image data is blank-field.
        /// </summary>
        /// <returns>New cloned image with blank data.</returns>
        public Image<TColor, TDepth> CopyBlank()
        {
            return ((IImage)this).CopyBlank() as Image<TColor, TDepth>;
        }

        #endregion
    }

}
