// ***********************************************************************
// Assembly         : Core
// Author           : pbosch
// Created          : 03-01-2019
//
// Last Modified By : pbosch
// Last Modified On : 03-07-2019
// ***********************************************************************
// <copyright file="Utility.cs" company="Highpoint Software Systems, LLC">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Drawing;

namespace Core
{
    /// <summary>
    /// Class Utility.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Gradients the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="fractionOfFrom">The fraction of from.</param>
        /// <returns>Color.</returns>
        public static Color Gradient(Color from, Color to, double fractionOfFrom)
        {
            return Color.FromArgb(
                (int)(to.A * fractionOfFrom + from.A * (1.0 - fractionOfFrom)),
                (int)(to.R * fractionOfFrom + from.R * (1.0 - fractionOfFrom)),
                (int)(to.G * fractionOfFrom + from.G * (1.0 - fractionOfFrom)),
                (int)(to.B * fractionOfFrom + from.B * (1.0 - fractionOfFrom))
                );
        }
    }

    public interface ILineWriter
    {
        void WriteLine(string s);
    }
}
