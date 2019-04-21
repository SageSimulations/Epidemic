// ***********************************************************************
// Assembly         : Core
// Author           : pbosch
// Created          : 02-18-2019
//
// Last Modified By : pbosch
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="Extensions.cs" company="Highpoint Software Systems, LLC">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Core
{

    /// <summary>
    /// Class ControlExtensions.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Doubles the buffering.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="enable">if set to <c>true</c> [enable].</param>
        public static void DoubleBuffering(this Control control, bool enable)
        {
            var method = typeof(Control).GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            method?.Invoke(control, new object[] { ControlStyles.OptimizedDoubleBuffer, enable });
        }
    }

    /// <summary>
    /// Class ImageExtensions.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Resizes the specified size.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="size">The size.</param>
        /// <returns>Image.</returns>
        public static Image Resize(this Image image, Size size)
        {
            var resizedImage = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppRgb);

            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor; // InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                var pageUnit = GraphicsUnit.Pixel;

                graphics.DrawImage(
                    image,
                    new Rectangle(Point.Empty, size),
                    image.GetBounds(ref pageUnit),
                    GraphicsUnit.Pixel
                    );
            }

            return resizedImage;
        }
    }
}
