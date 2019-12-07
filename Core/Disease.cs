// ***********************************************************************
// Assembly         : Core
// Author           : pbosch
// Created          : 03-18-2019
//
// Last Modified By : pbosch
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="Disease.cs" company="Highpoint Software Systems, LLC">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Core
{

    /// <summary>
    /// Class Disease.
    /// </summary>
    public class Disease
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Disease"/> class.
        /// </summary>
        public Disease() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Disease"/> class.
        /// </summary>
        /// <param name="fromDisease">From disease.</param>
        public Disease(Disease fromDisease)
        {
            NaturalImmunity = fromDisease.NaturalImmunity;
            NominalContractionRate = fromDisease.NominalContractionRate;
            NominalAsymptomaticPeriod = fromDisease.NominalAsymptomaticPeriod;
            NominalSuccumbEarly1Rate = fromDisease.NominalSuccumbEarly1Rate;
            NominalSuccumbEarly2Rate = fromDisease.NominalSuccumbEarly2Rate;
            NominalProgress1Rate = fromDisease.NominalProgress1Rate;
            NominalProgress2Rate = fromDisease.NominalProgress2Rate;
            NominalSuccumbRate = fromDisease.NominalSuccumbRate;
            NominalRemissionRate = fromDisease.NominalRemissionRate;
            NominalRecoveryRate = fromDisease.NominalRecoveryRate;
            NominalMortalityRate = fromDisease.NominalMortalityRate;
            NominalSymptomaticPeriod = fromDisease.NominalSymptomaticPeriod;
            NominalAsymptomaticMortalityRate = fromDisease.NominalAsymptomaticMortalityRate;
            NominalNonContagiousInfectedMortalityRate = fromDisease.NominalNonContagiousInfectedMortalityRate;
            NominalNonContagiousInfectedPeriod = fromDisease.NominalNonContagiousInfectedPeriod;
            NominalPopulationDensity = fromDisease.NominalPopulationDensity;
            NominalSymptomaticMortalityRate = fromDisease.NominalSymptomaticMortalityRate;
        }

        /// <summary>
        /// Gets or sets the natural immunity.
        /// </summary>
        /// <value>The natural immunity.</value>
        public double NaturalImmunity { get; set; }
        /// <summary>
        /// Gets or sets the nominal contraction rate.
        /// </summary>
        /// <value>The nominal contraction rate.</value>
        public double NominalContractionRate { get; set; }
        /// <summary>
        /// Gets or sets the nominal asymptomatic period.
        /// </summary>
        /// <value>The nominal asymptomatic period.</value>
        public double NominalAsymptomaticPeriod { get; set; }
        /// <summary>
        /// Gets or sets the nominal succumb early1 rate.
        /// </summary>
        /// <value>The nominal succumb early1 rate.</value>
        public double NominalSuccumbEarly1Rate { get; set; }
        /// <summary>
        /// Gets or sets the nominal succumb early2 rate.
        /// </summary>
        /// <value>The nominal succumb early2 rate.</value>
        public double NominalSuccumbEarly2Rate { get; set; }
        /// <summary>
        /// Gets or sets the nominal progress1 rate.
        /// </summary>
        /// <value>The nominal progress1 rate.</value>
        public double NominalProgress1Rate { get; set; }
        /// <summary>
        /// Gets or sets the nominal progress2 rate.
        /// </summary>
        /// <value>The nominal progress2 rate.</value>
        public double NominalProgress2Rate { get; set; }
        /// <summary>
        /// Gets or sets the nominal succumb rate.
        /// </summary>
        /// <value>The nominal succumb rate.</value>
        public double NominalSuccumbRate { get; set; }
        /// <summary>
        /// Gets or sets the nominal remission rate.
        /// </summary>
        /// <value>The nominal remission rate.</value>
        public double NominalRemissionRate { get; set; }
        /// <summary>
        /// Gets or sets the nominal recovery rate.
        /// </summary>
        /// <value>The nominal recovery rate.</value>
        public double NominalRecoveryRate { get; set; }
        /// <summary>
        /// Gets or sets the nominal mortality rate.
        /// </summary>
        /// <value>The nominal mortality rate.</value>
        public double NominalMortalityRate { get; set; }
        /// <summary>
        /// Gets or sets the nominal symptomatic period.
        /// </summary>
        /// <value>The nominal symptomatic period.</value>
        public double NominalSymptomaticPeriod { get; set; }
        /// <summary>
        /// Gets or sets the nominal asymptomatic mortality rate.
        /// </summary>
        /// <value>The nominal asymptomatic mortality rate.</value>
        public double NominalAsymptomaticMortalityRate { get; set; }
        /// <summary>
        /// Gets or sets the nominal symptomatic mortality rate.
        /// </summary>
        /// <value>The nominal symptomatic mortality rate.</value>
        public double NominalSymptomaticMortalityRate { get; set; }
        /// <summary>
        /// Gets or sets the nominal non contagious infected period.
        /// </summary>
        /// <value>The nominal non contagious infected period.</value>
        public double NominalNonContagiousInfectedPeriod { get; set; }         // <-- Same as |
        /// <summary>
        /// Gets or sets the nominal non contagious infected mortality rate.
        /// </summary>
        /// <value>The nominal non contagious infected mortality rate.</value>
        public double NominalNonContagiousInfectedMortalityRate { get; set; }  // <-----------
        /// <summary>
        /// Gets or sets the nominal population density.
        /// </summary>
        /// <value>The nominal population density.</value>
        public double NominalPopulationDensity { get; set; }

        /// <summary>
        /// The default
        /// </summary>
        public static readonly Disease DEFAULT = new Disease()
        {
            NaturalImmunity = 0.05,
            NominalContractionRate = 0.5,
            NominalAsymptomaticPeriod = 0.05,
            NominalSuccumbEarly1Rate = 0.01,
            NominalSuccumbEarly2Rate = 0.03,
            NominalProgress1Rate = 0.05,
            NominalProgress2Rate = 0.07,
            NominalSuccumbRate = 0.15,
            NominalRemissionRate = 0.05,
            NominalRecoveryRate = 0.09,
            NominalMortalityRate = 0.000005,
            NominalSymptomaticPeriod = .15,
            NominalAsymptomaticMortalityRate = 0.000005,
            NominalNonContagiousInfectedMortalityRate = 0.000005,
            NominalNonContagiousInfectedPeriod = .15,
            NominalPopulationDensity = 300,
            NominalSymptomaticMortalityRate = 0.000005
        };
    }
}
