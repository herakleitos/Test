using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;


namespace Chaint.Common.Devices.Utils
{
     /// <summary>
    /// Help functions for Image.
    /// </summary>
    public static class ImageHelper
    {
        #region Methods

        /// <summary>
        /// Gets the image from bytes.
        /// </summary>
        /// <param name="imageBytes">The image bytes.</param>
        /// return Image or <see langword="null"/>
        public static Image GetImageFromBytes(byte[] imageBytes)
        {
            if (imageBytes == null)
            {
                return null;
            }
            if (imageBytes.Length <= 0)
            {
                return null;
            }
            try
            {
                MemoryStream ms = new MemoryStream(imageBytes);
                return Image.FromStream(ms);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the bytes from image.
        /// </summary>
        /// <param name="image">The img to convert.</param>
        /// <returns>
        /// return Image Bytes or <see langword="null"/>
        /// </returns>
        public static byte[] GetBytesFromImage(Image image)
        {
            return GetBytesFromImage(image, ImageFormat.Png);
        }

        /// <summary>
        /// Gets the bytes from image.
        /// </summary>
        /// <param name="image">The image to convert.</param>
        /// <param name="format">The format of image.</param>
        /// <returns>
        /// return Image Bytes or <see langword="null"/>
        /// </returns>
        public static byte[] GetBytesFromImage(Image image, ImageFormat format)
        {
            byte[] ret = null;

            if (image == null)
            {
                return ret;
            }
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, format);
                    ret = ms.ToArray();
                }
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
            return ret;
        }

        /// <summary>
        /// Calculates the transparent color of the Bitmap.
        /// </summary>
        /// <param name="image">The bitmap for which color is calculated.</param>
        /// <returns>transparent color of the Bitmap</returns>
        public static Color CalculateTransparentColor(Bitmap image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            return image.GetPixel(0, image.Height - 1);
        }

        #endregion
    }

}
