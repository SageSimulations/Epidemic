// ***********************************************************************
// Assembly         : Core
// Author           : pbosch
// Created          : 04-05-2019
//
// Last Modified By : pbosch
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ExecParameters.cs" company="Highpoint Software Systems, LLC">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;

namespace Core
{
    /// <summary>
    /// Class ExecParameters.
    /// </summary>
    public class ExecParameters
    {

        /// <summary>
        /// The default start
        /// </summary>
        [Browsable(false)]
        public static string DEFAULT_START = "1/1/2019";
        /// <summary>
        /// The default end
        /// </summary>
        [Browsable(false)]
        public static string DEFAULT_END = "1/1/2021";
        /// <summary>
        /// The default time increment
        /// </summary>
        [Browsable(false)]
        public static string DEFAULT_TIME_INCREMENT = "1.00:00:00";

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>The start time.</value>
        [Description("The time at which the simulation is to start.")]
        [Category("Simulation")]
        [Browsable(true)]
        [ReadOnly(false)]
        [DisplayName("Start Time")]
        public DateTime StartTime { get; set; } = DateTime.Parse(DEFAULT_START);

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>The end time.</value>
        [Description("The time at which the simulation is to end.")]
        [Category("Simulation")]
        [Browsable(true)]
        [ReadOnly(false)]
        [DisplayName("End Time")]
        public DateTime EndTime { get; set; } = DateTime.Parse(DEFAULT_END);

        /// <summary>
        /// Gets or sets the increment.
        /// </summary>
        /// <value>The increment.</value>
        [Description("The time increments on which the disease progression is to be calculated.")]
        [Category("Simulation")]
        [Browsable(true)]
        [ReadOnly(false)]
        [DisplayName("Disease Increment")]
        public TimeSpan Increment { get; set; } = TimeSpan.Parse(DEFAULT_TIME_INCREMENT);
    }
}
