// ***********************************************************************
// Assembly         : Core
// Author           : pbosch
// Created          : 03-18-2019
//
// Last Modified By : pbosch
// Last Modified On : 03-18-2019
// ***********************************************************************
// <copyright file="DiseaseNode.cs" company="Highpoint Software Systems, LLC">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using Highpoint.Sage.SystemDynamics;


namespace Core
{
    /// <summary>
    /// Class DiseaseNode.
    /// Implements the <see cref="Highpoint.Sage.SystemDynamics.StateBase{Core.DiseaseNode}" />
    /// </summary>
    /// <seealso cref="Highpoint.Sage.SystemDynamics.StateBase{Core.DiseaseNode}" />
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public partial class DiseaseNode : StateBase<DiseaseNode>
    {

        // Constant Auxiliaries

        // ID Numbers for flows.
        /// <summary>
        /// The born immune
        /// </summary>
        private readonly static int BornImmune = 0;
        /// <summary>
        /// The born susceptible
        /// </summary>
        private readonly static int BornSusceptible = 1;
        /// <summary>
        /// The immunized
        /// </summary>
        private readonly static int Immunized = 2;
        /// <summary>
        /// The contracted
        /// </summary>
        private readonly static int Contracted = 3;
        /// <summary>
        /// The progressed1
        /// </summary>
        private readonly static int Progressed1 = 4;
        /// <summary>
        /// The progressed2
        /// </summary>
        private readonly static int Progressed2 = 5;
        /// <summary>
        /// The succumbed
        /// </summary>
        private readonly static int Succumbed = 6;
        /// <summary>
        /// The recovery
        /// </summary>
        private readonly static int Recovery = 7;
        /// <summary>
        /// The died infected
        /// </summary>
        private readonly static int DiedInfected = 8;
        /// <summary>
        /// The succumbed early1
        /// </summary>
        private readonly static int SuccumbedEarly1 = 9;
        /// <summary>
        /// The succumbed early2
        /// </summary>
        private readonly static int SuccumbedEarly2 = 10;
        /// <summary>
        /// The died not susceptible
        /// </summary>
        private readonly static int DiedNotSusceptible = 11;
        /// <summary>
        /// The died susceptible
        /// </summary>
        private readonly static int DiedSusceptible = 12;
        /// <summary>
        /// The remission
        /// </summary>
        private readonly static int Remission = 13;
        /// <summary>
        /// The died recovered immune
        /// </summary>
        private readonly static int DiedRecoveredImmune = 14;

        /// <summary>
        /// Initializes static members of the <see cref="DiseaseNode"/> class.
        /// </summary>
        static DiseaseNode()
        {   // Class-global set up.
        }

        // Stock Names
        /// <summary>
        /// Stocks the names.
        /// </summary>
        /// <returns>System.String[].</returns>
        public override string[] StockNames()
            =>
                new[]
                {
                    "NotSusceptible",
                    "Susceptible",
                    "ContagiousAsymptomatic",
                    "ContagiousSymptomatic",
                    "NonContagiousInfected",
                    "RecoveredImmune",
                    "Killed",
                    "Dead",
                };

        // Flow Names
        /// <summary>
        /// Flows the names.
        /// </summary>
        /// <returns>System.String[].</returns>
        public override string[] FlowNames()
            =>
                new[]
                {
                    "BornImmune",
                    "BornSusceptible",
                    "Immunized",
                    "Contracted",
                    "Progressed1",
                    "Progressed2",
                    "Succumbed",
                    "Recovery",
                    "DiedInfected",
                    "SuccumbedEarly1",
                    "SuccumbedEarly2",
                    "DiedNotSusceptible",
                    "DiedSusceptible",
                    "Remission",
                    "DiedRecoveredImmune"
                };

        /// <summary>
        /// Initializes a new instance of the <see cref="DiseaseNode"/> class.
        /// </summary>
        public DiseaseNode() : this(false){}

        /// <summary>
        /// Initializes a new instance of the <see cref="DiseaseNode"/> class.
        /// </summary>
        /// <param name="fromCopy">if set to <c>true</c> [from copy].</param>
        public DiseaseNode(bool fromCopy = false)
        {
            Start = 0;
            Finish = 720;
            TimeStep = 1;

            m_notSusceptible = 0 /* Need initialization */;
            m_susceptible = 0 /* Need initialization */;
            m_contagiousAsymptomatic = 0 /* Need initialization */;
            m_contagiousSymptomatic = 0 /* Need initialization */;
            m_nonContagiousInfected = 0 /* Need initialization */;
            m_recoveredImmune = 0 /* Need initialization */;
            m_killed = 0 /* Need initialization */;
            m_dead = 0 /* Need initialization */;

            // WARNING - auto-generation needed updating.
            StockSetters = new Action<StateBase<DiseaseNode>, double>[]
            {
                (state, d) => ((DiseaseNode)state).NotSusceptible = d,
                (state, d) => ((DiseaseNode)state).Susceptible = d,
                (state, d) => ((DiseaseNode)state).ContagiousAsymptomatic = d,
                (state, d) => ((DiseaseNode)state).ContagiousSymptomatic = d,
                (state, d) => ((DiseaseNode)state).NonContagiousInfected = d,
                (state, d) => ((DiseaseNode)state).RecoveredImmune = d,
                (state, d) => ((DiseaseNode)state).Killed = d,
                (state, d) => ((DiseaseNode)state).Dead = d,
            };

            // WARNING - auto-generation needed updating.
            StockGetters = new Func<StateBase<DiseaseNode>, double>[]
            {
                state => ((DiseaseNode)state).NotSusceptible,
                state => ((DiseaseNode)state).Susceptible,
                state => ((DiseaseNode)state).ContagiousAsymptomatic,
                state => ((DiseaseNode)state).ContagiousSymptomatic,
                state => ((DiseaseNode)state).NonContagiousInfected,
                state => ((DiseaseNode)state).RecoveredImmune,
                state => ((DiseaseNode)state).Killed,
                state => ((DiseaseNode)state).Dead,
            };

            // WARNING - auto-generation needed updating.
            // Flows = new Func<EpidemicNode, double>[]
            // Flows = new List<Func<StateBase<EpidemicNode>, double>>
            Flows = new List<Func<StateBase<DiseaseNode>, double>>
            {
                /* BornImmune */ state => PeriodAdjust( ((DiseaseNode)state).locale.BirthRate * ((DiseaseNode)state).locale.Population * ((DiseaseNode)state).disease.NaturalImmunity ) ,
                /* BornSusceptible */ state => PeriodAdjust( ((DiseaseNode)state).locale.BirthRate * ((DiseaseNode)state).locale.Population * (1.0 - ((DiseaseNode)state).disease.NaturalImmunity) ) ,
                /* Immunized */ state => PeriodAdjust( ((DiseaseNode)state).m_susceptible * ((DiseaseNode)state).ImmunizationRate ) ,
                /* Contracted */ state => PeriodAdjust( ((DiseaseNode)state).m_susceptible * ExposureRate * ((DiseaseNode)state).ContractionRate ) ,
                /* Progressed1 */ state => PeriodAdjust( ((DiseaseNode)state).m_contagiousAsymptomatic * ((DiseaseNode)state).disease.NominalProgress1Rate ) ,
                /* Progressed2 */ state => PeriodAdjust( ((DiseaseNode)state).m_contagiousSymptomatic * ((DiseaseNode)state).disease.NominalProgress2Rate ) ,
                /* Succumbed */ state => PeriodAdjust( ((DiseaseNode)state).m_nonContagiousInfected * ((DiseaseNode)state).disease.NominalSuccumbRate ) ,
                /* Recovery */ state => PeriodAdjust( ((DiseaseNode)state).m_nonContagiousInfected * ((DiseaseNode)state).disease.NominalRecoveryRate ) ,
                /* DiedInfected */ state => PeriodAdjust( ((DiseaseNode)state).m_nonContagiousInfected * ((DiseaseNode)state).locale.MortalityRate ) ,
                /* SuccumbedEarly1 */ state => PeriodAdjust( ((DiseaseNode)state).m_contagiousAsymptomatic * ((DiseaseNode)state).disease.NominalSuccumbEarly1Rate ) ,
                /* SuccumbedEarly2 */ state => PeriodAdjust( ((DiseaseNode)state).m_contagiousSymptomatic * ((DiseaseNode)state).disease.NominalSuccumbEarly2Rate ) ,
                /* DiedNotSusceptible */ state => PeriodAdjust( ((DiseaseNode)state).NotSusceptible * ((DiseaseNode)state).locale.MortalityRate ) ,
                /* DiedSusceptible */ state => PeriodAdjust( ((DiseaseNode)state).m_susceptible * ((DiseaseNode)state).locale.MortalityRate ) ,
                /* Remission */ state => PeriodAdjust( ((DiseaseNode)state).m_nonContagiousInfected * ((DiseaseNode)state).disease.NominalRemissionRate ) ,
                /* DiedRecoveredImmune */ state => PeriodAdjust( ((DiseaseNode)state).m_recoveredImmune * ((DiseaseNode)state).locale.MortalityRate ) ,
            };

            // WARNING - auto-generation needed updating.
            StockInflows = new List<int[]> { new int[] { BornImmune, Immunized }, new int[] { BornSusceptible, Remission }, new int[] { Contracted }, new int[] { Progressed1 }, new int[] { Progressed2 }, new int[] { Recovery }, new int[] { Succumbed, SuccumbedEarly1, SuccumbedEarly2 }, new int[] { DiedNotSusceptible, DiedSusceptible, DiedRecoveredImmune, DiedInfected } };
            StockOutflows = new List<int[]> { new int[] { DiedNotSusceptible }, new int[] { Contracted, Immunized, DiedSusceptible }, new int[] { Progressed1, SuccumbedEarly1 }, new int[] { Progressed2, SuccumbedEarly2 }, new int[] { Remission, Recovery, DiedInfected, Succumbed }, new int[] { DiedRecoveredImmune }, new int[] { }, new int[] { } };

       }

        // WARNING - auto-generation needed updating.
        // public override EpidemicNode Copy()
        /// <summary>
        /// Copies this instance.
        /// </summary>
        /// <returns>StateBase&lt;DiseaseNode&gt;.</returns>
        public override StateBase<DiseaseNode> Copy()
        {
            DiseaseNode retval = new DiseaseNode(true);
            retval.disease = disease; // WARNING - auto-generation needed updating.
            retval.policy = policy; // WARNING - auto-generation needed updating.
            retval.locale = locale; // WARNING - auto-generation needed updating.
            retval.m_notSusceptible = NotSusceptible;
            retval.m_susceptible = m_susceptible;
            retval.m_contagiousAsymptomatic = m_contagiousAsymptomatic;
            retval.m_contagiousSymptomatic = m_contagiousSymptomatic;
            retval.m_nonContagiousInfected = m_nonContagiousInfected;
            retval.m_recoveredImmune = m_recoveredImmune;
            retval.m_killed = m_killed;
            retval.m_dead = m_dead;
            retval.TimeSliceNdx = TimeSliceNdx;
            retval.Quarantined = Quarantined;
            retval.MapCell = MapCell;
            return retval;
        }

        /// <summary>
        /// Configures the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public override void Configure(XElement parameters = null)
        {

        }

        // Non-constant Auxiliaries ---> ALSO: Figure out the divide-by-zero on population or density == 0.
        /// <summary>
        /// The quarantined
        /// </summary>
        public bool Quarantined = false;
        /// <summary>
        /// Gets the population.
        /// </summary>
        /// <value>The population.</value>
        public double Population => NotSusceptible + m_susceptible + m_contagiousAsymptomatic + m_contagiousSymptomatic + m_nonContagiousInfected + m_recoveredImmune;
        /// <summary>
        /// Gets the immune.
        /// </summary>
        /// <value>The immune.</value>
        public double Immune => NotSusceptible + m_recoveredImmune;
        /// <summary>
        /// Gets the immunization rate.
        /// </summary>
        /// <value>The immunization rate.</value>
        public double ImmunizationRate => FractionAdjust(policy.TargetImmunizationRate * (1.0 - (Immune / (Population <.0001 ? 1 : Population))) * (locale.HealthCareEffectiveness));
        /// <summary>
        /// Gets the exposure rate.
        /// </summary>
        /// <value>The exposure rate.</value>
        public double ExposureRate => FractionAdjust((m_contagiousAsymptomatic + (Quarantined ? (1.0 - locale.HealthCareEffectiveness):1.0)*m_contagiousSymptomatic)/(Population==0?1:Population));
        /// <summary>
        /// Gets the contraction rate.
        /// </summary>
        /// <value>The contraction rate.</value>
        public double ContractionRate => FractionAdjust((disease.NominalContractionRate * locale.PopulationDensity) / (disease.NominalPopulationDensity * locale.Sanitation));
        /// <summary>
        /// Gets the asymptomatic period.
        /// </summary>
        /// <value>The asymptomatic period.</value>
        public double AsymptomaticPeriod => PeriodAdjust(disease.NominalAsymptomaticPeriod);
        /// <summary>
        /// Gets the symptomatic period.
        /// </summary>
        /// <value>The symptomatic period.</value>
        public double SymptomaticPeriod => PeriodAdjust(disease.NominalSymptomaticPeriod);
        /// <summary>
        /// Gets the non contagious infected period.
        /// </summary>
        /// <value>The non contagious infected period.</value>
        public double NonContagiousInfectedPeriod => PeriodAdjust(disease.NominalNonContagiousInfectedPeriod);
        /// <summary>
        /// Gets the remission rate.
        /// </summary>
        /// <value>The remission rate.</value>
        public double RemissionRate => FractionAdjust(disease.NominalSymptomaticPeriod * disease.NominalRemissionRate);
        /// <summary>
        /// Gets the recovery rate.
        /// </summary>
        /// <value>The recovery rate.</value>
        public double RecoveryRate => FractionAdjust(disease.NominalRecoveryRate * locale.HealthCareEffectiveness);
        /// <summary>
        /// Gets the succumbed rate.
        /// </summary>
        /// <value>The succumbed rate.</value>
        public double SuccumbedRate => FractionAdjust(disease.NominalSuccumbRate * locale.HealthCareEffectiveness);
        /// <summary>
        /// Gets the succumbed early1 rate.
        /// </summary>
        /// <value>The succumbed early1 rate.</value>
        public double SuccumbedEarly1Rate => FractionAdjust(disease.NominalSuccumbEarly1Rate * locale.HealthCareEffectiveness);
        /// <summary>
        /// Gets the succumbed early2 rate.
        /// </summary>
        /// <value>The succumbed early2 rate.</value>
        public double SuccumbedEarly2Rate => FractionAdjust(disease.NominalSuccumbEarly2Rate * locale.HealthCareEffectiveness);
        /// <summary>
        /// Gets the mortality rate asymptomatic.
        /// </summary>
        /// <value>The mortality rate asymptomatic.</value>
        public double MortalityRateAsymptomatic => FractionAdjust(disease.NominalAsymptomaticMortalityRate * locale.HealthCareEffectiveness);
        /// <summary>
        /// Gets the mortality rate symptomatic.
        /// </summary>
        /// <value>The mortality rate symptomatic.</value>
        public double MortalityRateSymptomatic => FractionAdjust(disease.NominalSymptomaticMortalityRate * locale.HealthCareEffectiveness);
        /// <summary>
        /// Gets the mortality rate non contagious infected.
        /// </summary>
        /// <value>The mortality rate non contagious infected.</value>
        public double MortalityRateNonContagiousInfected => FractionAdjust(disease.NominalNonContagiousInfectedMortalityRate * locale.HealthCareEffectiveness);

        // These predicates are applied to all values set directly into stocks.
        /// <summary>
        /// The tests
        /// </summary>
        private List<Predicate<double>> Tests = new List<Predicate<double>>() {double.IsNaN, d => d < 0.0};

        /// <summary>
        /// Gets or sets the not susceptible.
        /// </summary>
        /// <value>The not susceptible.</value>
        public double NotSusceptible
        {
            get{return m_notSusceptible;}
            set
            {
                Tests?.ForEach(n => { if (n(value)) Debugger.Break(); });
                m_notSusceptible = value;
            }
        }

        /// <summary>
        /// Gets or sets the dead.
        /// </summary>
        /// <value>The dead.</value>
        public double Dead
        {
            get { return m_dead; }
            set
            {
                Tests?.ForEach(n => { if (n(value)) Debugger.Break(); });
                m_dead = value;
            }
        }

        /// <summary>
        /// Gets or sets the killed.
        /// </summary>
        /// <value>The killed.</value>
        public double Killed
        {
            get { return m_killed; }
            set
            {
                Tests?.ForEach(n => { if (n(value)) Debugger.Break(); });
                m_killed = value;
            }
        }

        /// <summary>
        /// Gets or sets the recovered immune.
        /// </summary>
        /// <value>The recovered immune.</value>
        public double RecoveredImmune
        {
            get { return m_recoveredImmune; }
            set
            {
                Tests?.ForEach(n => { if (n(value)) Debugger.Break(); });
                m_recoveredImmune = value;
            }
        }

        /// <summary>
        /// Gets or sets the non contagious infected.
        /// </summary>
        /// <value>The non contagious infected.</value>
        public double NonContagiousInfected
        {
            get { return m_nonContagiousInfected; }
            set
            {
                Tests?.ForEach(n => { if (n(value)) Debugger.Break(); });
                m_nonContagiousInfected = value;
            }
        }

        /// <summary>
        /// Gets or sets the contagious symptomatic.
        /// </summary>
        /// <value>The contagious symptomatic.</value>
        public double ContagiousSymptomatic
        {
            get { return m_contagiousSymptomatic; }
            set
            {
                Tests?.ForEach(n => { if (n(value)) Debugger.Break(); });
                m_contagiousSymptomatic = value;
            }
        }

        /// <summary>
        /// Gets or sets the contagious asymptomatic.
        /// </summary>
        /// <value>The contagious asymptomatic.</value>
        public double ContagiousAsymptomatic
        {
            get { return m_contagiousAsymptomatic; }
            set
            {
                Tests?.ForEach(n => { if (n(value)) Debugger.Break(); });
                m_contagiousAsymptomatic = value;
            }
        }

        /// <summary>
        /// Gets or sets the susceptible.
        /// </summary>
        /// <value>The susceptible.</value>
        public double Susceptible
        {
            get { return m_susceptible; }
            set
            {
                Tests?.ForEach(n=> {if(n(value)) Debugger.Break();});
                m_susceptible = value;
            }
        }

        // Stocks
        /// <summary>
        /// The m not susceptible
        /// </summary>
        private double m_notSusceptible;
        /// <summary>
        /// The m susceptible
        /// </summary>
        private double m_susceptible;
        /// <summary>
        /// The m contagious asymptomatic
        /// </summary>
        private double m_contagiousAsymptomatic;
        /// <summary>
        /// The m contagious symptomatic
        /// </summary>
        private double m_contagiousSymptomatic;
        /// <summary>
        /// The m non contagious infected
        /// </summary>
        private double m_nonContagiousInfected;
        /// <summary>
        /// The m recovered immune
        /// </summary>
        private double m_recoveredImmune;
        /// <summary>
        /// The m killed
        /// </summary>
        private double m_killed;
        /// <summary>
        /// The m dead
        /// </summary>
        private double m_dead;

        //////////////////////////////////////////////////////////////
        // MACRO IMPLEMENTATIONS
        //////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////

    }
}
