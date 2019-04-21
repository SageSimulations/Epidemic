// ***********************************************************************
// Assembly         : Core
// Author           : pbosch
// Created          : 03-18-2019
//
// Last Modified By : pbosch
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="DiseaseNode.cs" company="Highpoint Software Systems, LLC">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Diagnostics.CodeAnalysis;


namespace Core
{
    /// <summary>
    /// Class DiseaseNode.
    /// Implements the <see cref="DiseaseNode" />
    /// Implements the <see cref="DiseaseNode" />
    /// </summary>
    /// <seealso cref="DiseaseNode" />
    /// <seealso cref="DiseaseNode" />
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public partial class DiseaseNode
    {
        /// <summary>
        /// The locale
        /// </summary>
        private Locale locale;
        /// <summary>
        /// The disease
        /// </summary>
        private Disease disease;
        /// <summary>
        /// The policy
        /// </summary>
        private Policy policy;

        public CellData MapCell { get; set; }
        /// <summary>
        /// Gets the node policy.
        /// </summary>
        /// <value>The node policy.</value>
        public Policy NodePolicy => policy;
        /// <summary>
        /// Gets the locale data.
        /// </summary>
        /// <value>The locale data.</value>
        public Locale LocaleData => locale;
        /// <summary>
        /// Gets the active disease.
        /// </summary>
        /// <value>The active disease.</value>
        public Disease ActiveDisease => disease;
        /// <summary>
        /// Gets the nominal period.
        /// </summary>
        /// <value>The nominal period.</value>
        public override TimeSpan NominalPeriod { get; } = TimeSpan.FromDays(1.0);
        /// <summary>
        /// Gets the active period.
        /// </summary>
        /// <value>The active period.</value>
        public override TimeSpan ActivePeriod { get; } = TimeSpan.FromDays(1.0);

        /// <summary>
        /// Initializes a new instance of the <see cref="DiseaseNode"/> class.
        /// </summary>
        /// <param name="_policy">The policy.</param>
        /// <param name="_locale">The locale.</param>
        /// <param name="_disease">The disease.</param>
        public DiseaseNode(Policy _policy, Locale _locale, Disease _disease) : this()
        {
            locale = _locale;
            disease = _disease;
            policy = _policy;
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected override void Initialize()
        {
            /* BornImmune */
            m_notSusceptible = locale.Population*disease.NaturalImmunity;
            m_susceptible = locale.Population*(1.0 - disease.NaturalImmunity);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            //return $"S={Susceptible:F2} + NS={NotSusceptible:F2} + CA={ContagiousAsymptomatic:F2} + CS={ContagiousSymptomatic:F2} + NCI={NonContagiousInfected:F2} + RI={RecoveredImmune:F2}";
            return $"S={m_susceptible:F4}, NS={NotSusceptible:F4}, CA={m_contagiousAsymptomatic:F4}, CS={m_contagiousSymptomatic:F4}, NCI={m_nonContagiousInfected:F4}, RI={m_recoveredImmune:F4}, K={m_killed:F4} D={m_dead:F4}";
        }
    }
}