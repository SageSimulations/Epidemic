// ***********************************************************************
// Assembly         : Core
// Author           : pbosch
// Created          : 03-07-2019
//
// Last Modified By : pbosch
// Last Modified On : 03-18-2019
// ***********************************************************************
// <copyright file="CellData.cs" company="Highpoint Software Systems, LLC">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    /// <summary>
    /// Enum CellState
    /// </summary>
    [Serializable]
    public enum CellState
    {
        /// <summary>
        /// The unknown
        /// </summary>
        Unknown,
        /// <summary>
        /// The occupied
        /// </summary>
        Occupied,
        /// <summary>
        /// The unoccupied
        /// </summary>
        Unoccupied,
        /// <summary>
        /// The ocean
        /// </summary>
        Ocean
    }

    /// <summary>
    /// Class CellData.
    /// </summary>
    [Serializable]
    public class CellData
    {
        /// <summary>
        /// Gets or sets the population density.
        /// </summary>
        /// <value>The population density.</value>
        public double PopulationDensity { get; set; }
        /// <summary>
        /// Gets or sets the water area.
        /// </summary>
        /// <value>The water area.</value>
        public double WaterArea { get; set; }
        /// <summary>
        /// Gets or sets the land area.
        /// </summary>
        /// <value>The land area.</value>
        public double LandArea { get; set; }

        /// <summary>
        /// The no data
        /// </summary>
        public static int NO_DATA = -9999;
        /// <summary>
        /// The m cell state
        /// </summary>
        private CellState? m_cellState;

        /// <summary>
        /// Gets the state of the cell.
        /// </summary>
        /// <value>The state of the cell.</value>
        public CellState CellState
        {
            get
            {
                if (m_cellState == null)
                {

                    int classification = (IsLegit(LandArea) ? 4 : 0) +
                                         (IsLegit(WaterArea) ? 2 : 0) +
                                         (IsLegit(PopulationDensity) ? 1 : 0);
                    switch (classification)
                    {
                        case 0:
                            PopulationDensity = 0.0;
                            m_cellState = CellState.Ocean;
                            break;
                        case 1:
                            m_cellState = CellState.Unoccupied;
                            break;
                        case 2:
                            PopulationDensity = 0.0;
                            m_cellState = CellState.Ocean;
                            break;
                        case 3:
                            m_cellState = CellState.Unoccupied;
                            break;
                        case 4:
                            m_cellState = CellState.Unknown;
                            break;
                        case 5:
                            if (PopulationDensity > 1E-10)
                            {
                                m_cellState = CellState.Occupied;
                            }
                            else
                            {
                                m_cellState = CellState.Unoccupied;
                            }
                            break;
                        case 6:
                            PopulationDensity = 0.0;
                            m_cellState = CellState.Unoccupied;
                            break;
                        case 7:
                            if (PopulationDensity > 1E-10)
                            {
                                m_cellState = CellState.Occupied;
                            }
                            else
                            {
                                return CellState.Unoccupied;
                            }
                            break;
                        default:
                            m_cellState = CellState.Unknown;
                            break;
                    }
                }
                return m_cellState.Value;
            }
        }

        public string CountryCode { get; set; }

        [NonSerialized]
        private DiseaseNode m_diseaseNode;

        public DiseaseNode DiseaseModel { get { return m_diseaseNode; } set { m_diseaseNode = value; } }

        /// <summary>
        /// Determines whether the specified d is legit.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns><c>true</c> if the specified d is legit; otherwise, <c>false</c>.</returns>
        private bool IsLegit(double d)
        {
            return Math.Abs(d - NO_DATA) > 0.01;
        }

        /// <summary>
        /// Populations the density enumerator.
        /// </summary>
        /// <param name="cellDataArray">The cell data array.</param>
        /// <returns>IEnumerable&lt;System.Double&gt;.</returns>
        public static IEnumerable<double> PopulationDensityEnumerator(CellData[,] cellDataArray)
        {
            return from CellData cellDatum in cellDataArray select cellDatum.PopulationDensity;
        }

    }
}
