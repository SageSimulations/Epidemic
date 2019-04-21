// ***********************************************************************
// Assembly         : Core
// Author           : pbosch
// Created          : 03-18-2019
//
// Last Modified By : pbosch
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="Locale.cs" company="Highpoint Software Systems, LLC">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Core
{

    /// <summary>
    /// Class Locale.
    /// </summary>
    public class Locale
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Locale"/> class.
        /// </summary>
        protected Locale() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Locale"/> class.
        /// </summary>
        /// <param name="fromLocale">From locale.</param>
        public Locale(Locale fromLocale)
        {
            Population = fromLocale.Population;
            Area = fromLocale.Area;
            BirthRate = fromLocale.BirthRate;
            DoctorsPerCapita = fromLocale.DoctorsPerCapita;
            ForeignImmunizationAidPerCapita = fromLocale.ForeignImmunizationAidPerCapita;
            HealthCareEffectiveness = fromLocale.HealthCareEffectiveness;
            ImmunizationAidEffectivenessFactor = fromLocale.ImmunizationAidEffectivenessFactor;
            ImmunizationEffectivenessFactor = fromLocale.ImmunizationEffectivenessFactor;
            MortalityRate = fromLocale.MortalityRate;
            Sanitation = fromLocale.Sanitation;
            Mobility = 0.30; // 30% travel more than 30 KM in a day.
            //MobilityMean = 0.3;
            //MobilityStDev = 0.3;

        }
        /// <summary>
        /// Gets or sets the population.
        /// </summary>
        /// <value>The population.</value>
        public double Population { get; set; }
        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        /// <value>The area.</value>
        public double Area { get; set; }
        /// <summary>
        /// Gets or sets the birth rate.
        /// </summary>
        /// <value>The birth rate.</value>
        public double BirthRate { get; set; }
        /// <summary>
        /// Gets the population density.
        /// </summary>
        /// <value>The population density.</value>
        public double PopulationDensity => Population / Area;
        /// <summary>
        /// Gets or sets the doctors per capita.
        /// </summary>
        /// <value>The doctors per capita.</value>
        public double DoctorsPerCapita { get; set; }
        /// <summary>
        /// Gets or sets the foreign immunization aid per capita.
        /// </summary>
        /// <value>The foreign immunization aid per capita.</value>
        public double ForeignImmunizationAidPerCapita { get; set; }
        /// <summary>
        /// Gets or sets the health care effectiveness.
        /// </summary>
        /// <value>The health care effectiveness.</value>
        public double HealthCareEffectiveness { get; set; }
        /// <summary>
        /// [ERROR: invalid expression PropertyAccessText] [ERROR: invalid expression PropertyName.Words.TheAndAll].
        /// </summary>
        /// <value>[ERROR: invalid expression PropertyName.Words.TheAndAllAsSentence].</value>
        public double ImmunizationAidEffectivenessFactor { get; set; }

        /// <summary>
        /// Gets or sets the immunization effectiveness factor.
        /// </summary>
        /// <value>The immunization effectiveness factor.</value>
        public double ImmunizationEffectivenessFactor { get; set; }
        /// <summary>
        /// Gets or sets the sanitation.
        /// </summary>
        /// <value>The sanitation.</value>
        public double Sanitation { get; set; }

        /// <summary>
        /// Gets or sets the social stability.
        /// </summary>
        /// <value>The social stability.</value>
        public double SocialStability { get; set; }

        /// <summary>
        /// Gets or sets the mortality rate.
        /// </summary>
        /// <value>The mortality rate.</value>
        public double MortalityRate { get; set; }
        /// <summary>
        /// Gets or sets the mobility.
        /// </summary>
        /// <value>The mobility.</value>
        public double Mobility { get; set; }

        /// <summary>
        /// The default
        /// </summary>
        public static readonly Locale DEFAULT = new Locale()
        {
            Population = 350000,
            Area = 1000,
            BirthRate = 9.05e-5,
            /*33-ish per year per thousand.*/
            DoctorsPerCapita = .00391,
            /*3.91 per thousand*/
            ForeignImmunizationAidPerCapita = 25.0,
            HealthCareEffectiveness = 0.85,
            ImmunizationAidEffectivenessFactor = .65,
            ImmunizationEffectivenessFactor = .90,
            MortalityRate = 1.92e-5,
            /*7-ish per year per thousand.*/// Average worldwide, per CIAWFB.
            Sanitation = .85
        };
    }
}
