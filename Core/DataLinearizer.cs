// ***********************************************************************
// Assembly         : Core
// Author           : pbosch
// Created          : 02-18-2019
//
// Last Modified By : pbosch
// Last Modified On : 03-18-2019
// ***********************************************************************
// <copyright file="DataLinearizer.cs" company="Highpoint Software Systems, LLC">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// Class DataLinearizer takes a data population that is logarithmically distributed (y = e^x) and
    /// translates it to a (roughly) linearly binned population.
    /// </summary>
    /// <summary>
    /// Class DataLinearizer.
    /// </summary>
    public class DataLinearizer
    {
        /// <summary>
        /// The m log minimum
        /// </summary>
        private double m_logMin;
        /// <summary>
        /// The m delta
        /// </summary>
        private double m_delta;
        /// <summary>
        /// The m data adjustment
        /// </summary>
        private double m_dataAdjustment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLinearizer"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="noValue">The no value.</param>
        /// <param name="nBins">The n bins.</param>
        /// <param name="adjustment">The adjustment.</param>
        public DataLinearizer(IEnumerator<double> data, double noValue, int nBins, double adjustment = .01)
        {
            double min = double.MaxValue;
            double max = double.MinValue;
            while (!data.MoveNext())
            {
                if ( Math.Abs(data.Current - noValue) > .001)
                {
                    min = Math.Min(min, data.Current);
                    max = Math.Max(max, data.Current);
                }
            }

            if (Math.Sign(min) != Math.Sign(max))
            {
                m_dataAdjustment = adjustment - min;
                min += m_dataAdjustment;
                max += m_dataAdjustment;
            }

            m_logMin = Math.Log(min);
            m_delta = (Math.Log(max) - Math.Log(min)) / nBins;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataLinearizer"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="noValue">The no value.</param>
        /// <param name="nBins">The n bins.</param>
        /// <param name="adjustment">The adjustment.</param>
        public DataLinearizer(IEnumerable<double> data, double noValue, int nBins, double adjustment = .01)
        {

            double min = double.MaxValue;
            double max = double.MinValue;
            foreach (double datum in data)
            {
                if (Math.Abs(datum - noValue) > .001)
                {
                    min = Math.Min(min, datum);
                    max = Math.Max(max, datum);
                }
            }

            if (Math.Sign(min) != Math.Sign(max))
            {
                m_dataAdjustment = adjustment - min;
                min += m_dataAdjustment;
                max += m_dataAdjustment;
            }

            m_logMin = Math.Log(min);
            m_delta = (Math.Log(max)-Math.Log(min))/nBins;
        }

        /// <summary>
        /// Gets the bin for.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns>System.Int32.</returns>
        public int getBinFor(double val)
        {
            double logVal = Math.Log(val + m_dataAdjustment);
            return (int) ((logVal - m_logMin)/m_delta);
        }
    }
}
