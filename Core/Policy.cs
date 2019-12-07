// ***********************************************************************
// Assembly         : Core
// Author           : pbosch
// Created          : 03-18-2019
//
// Last Modified By : pbosch
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="Policy.cs" company="Highpoint Software Systems, LLC">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Core
{
    /// <summary>
    /// Class Policy.
    /// </summary>
    public class Policy
    {
        /// <summary>
        /// Gets or sets the target immunization rate.
        /// </summary>
        /// <value>The target immunization rate.</value>
        public double TargetImmunizationRate { get; set; }
        /// <summary>
        /// The default
        /// </summary>
        public static readonly Policy DEFAULT = new Policy() { TargetImmunizationRate = 0.15 }; // How many per day relative to susceptible need to be immunized?

        /// <summary>
        /// Initializes a new instance of the <see cref="Policy"/> class.
        /// </summary>
        public Policy() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Policy"/> class.
        /// </summary>
        /// <param name="fromPolicy">From policy.</param>
        public Policy(Policy fromPolicy)
        {
            TargetImmunizationRate = fromPolicy.TargetImmunizationRate;
        }
    }
}
