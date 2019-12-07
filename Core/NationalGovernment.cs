using System;
using System.Collections.Generic;
using System.Linq;
using Highpoint.Sage.SimCore;

namespace Core
{
    public class NationalGovernment
    {
        private static readonly double DISPATCH_FREQUENCY = 0.5;
        private static readonly Random s_dispatchDecider = new Random();
        private readonly WorldModel m_worldModel;
        private ILineWriter console = Locator.Resolve<ILineWriter>("console");

        public string Name => SimCountryData?.Name ?? "Unknown";

        internal NationalGovernment(WorldModel worldModel)
        {
            m_worldModel = worldModel;
            m_worldModel.Executive.ExecutiveStarted_SingleShot += exec =>
            {
                // If the model is starting, and we are equipped with Outbreak Response Teams,
                // we will register as observers of newly-infected disease cells...
                if (ResponseTeams.Count > 0) m_worldModel.OnNewlyInfectedNode += WorldModelOnOnNewlyInfectedNode;
            };
        }

        private void WorldModelOnOnNewlyInfectedNode(Coordinates coordinates)
        {
            // If a random number is less than DISPATCH_FREQUENCY, and there are yet-unassigned ORTs,
            // send one there by air. Don't want all governments instantly sending their ORTs to the first
            // ten or fifteen infected nodes, do we? Wouldn't look right.
            if (s_dispatchDecider.NextDouble() < DISPATCH_FREQUENCY)
            {
                OutbreakResponseTeam nextUp = ResponseTeams.FirstOrDefault(n => n.StandingBy);
                //if ( nextUp != null ) console.WriteLine($"{SimCountryData.Name} is sending {nextUp} to {coordinates}.");
                nextUp?.SetDestinationAndSpeed(coordinates, OutbreakResponseTeam.AIRCRAFT_MOVE_SPEED);
            }
            //else
            //{
            //    console.WriteLine($"{SimCountryData.Name} will not respond to infection at {coordinates}.");
            //}
        }

        public WorldModel WorldModel { get; set; }

        public List<Coordinates> DiseaseNodeCoordinates { get; internal set; }

        public SimCountryData SimCountryData { get; internal set; }

        public List<Policy> Policies { get; internal set; }

        public List<OutbreakResponseTeam> ResponseTeams { get; } = new List<OutbreakResponseTeam>();

        public override string ToString()
        {
            return $"Government of {SimCountryData?.Name??"???"}";
        }

        /// <summary>
        /// Updates this country's disease nodes with disease-impacting country-specific data
        /// such as HealthCareEffectiveness and SocialStability.
        /// </summary>
        /// <param name="initializing">if set to <c>true</c> [initializing].</param>
        internal void UpdateDiseaseNodes(bool initializing = false)
        {
            foreach (Coordinates coordinates in DiseaseNodeCoordinates)
            {
                if (initializing)
                {
                    DiseaseNode dn = WorldModel.DiseaseNodes[coordinates.X, coordinates.Y];
                    dn.LocaleData.HealthCareEffectiveness = SimCountryData.HealthCareEffectiveness;
                    dn.LocaleData.SocialStability = SimCountryData.SocialStability;
                    dn.LocaleData.ForeignImmunizationAidPerCapita = 0.0;
                    // Birth rate, mortality, doctors per capita... 
                }
                else
                {
                    // TODO: Some day, we'll have dynamic evolution of ... stuff.
                }
            }
        }

        /// <summary>
        /// The national government is given an opportunity to reassess itself after each cycle.
        /// </summary>
        /// <param name="exec">The execute.</param>
        internal void Reassess(IExecutive exec)
        {
            double evidentlyInfected = 0;
            double population = 0;
            foreach (Coordinates dnCoordinates in DiseaseNodeCoordinates)
            {
                DiseaseNode dn = WorldModel.DiseaseNodes[dnCoordinates.X, dnCoordinates.Y];
                evidentlyInfected += dn.NonContagiousInfected;
                evidentlyInfected += dn.ContagiousAsymptomatic;
                evidentlyInfected += dn.ContagiousSymptomatic;
                evidentlyInfected += dn.Killed;
                population += dn.Population;
            }

            // Once we have greater than half a percent of the 
            // population infected, establish a quarantine on every
            // cell in the country.
            if (evidentlyInfected > (.005*population))
            {

                bool alreadyQuarantined = false;
                foreach (Coordinates dnCoordinates in DiseaseNodeCoordinates)
                {
                    DiseaseNode dn = WorldModel.DiseaseNodes[dnCoordinates.X, dnCoordinates.Y];
                    if (dn.Quarantined)
                    {
                        alreadyQuarantined = true;
                        break;
                    }
                    dn.Quarantined = true;
                }
                if ( !alreadyQuarantined ) console.WriteLine($"{exec.Now} : {SimCountryData.Name} is quarantining all of its cells.");
            }
        }
    }
}
