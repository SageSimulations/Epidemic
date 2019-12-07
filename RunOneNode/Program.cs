using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Core;
using CsvHelper;
using Highpoint.Sage.SystemDynamics;
using Highpoint.Sage.SystemDynamics.Utility;

namespace OneNode
{
    /// <summary>
    /// This program runs one node of the Epidemic model.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
#pragma warning disable 162
            if (false)
                // ReSharper disable once HeuristicUnreachableCode
            {
                string parameterString =
                    @"<Parameters>
                    </Parameters>";
                XElement parameters = XElement.Load(new StringReader(parameterString));
                RunProgram<DiseaseNode>.Run(args, Integrator.Euler, parameters);
            }
            else
            {
                List<SimCountryData> records;
                using (var reader = new StreamReader("../../../Data/MyData.csv"))
                using (var csv = new CsvReader(reader))
                {
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.HeaderValidated = null;
                    records = new List<SimCountryData>(csv.GetRecords<SimCountryData>());
                }

                Policy p = new Policy(Policy.DEFAULT);
                Locale l = new Locale(Locale.DEFAULT);
                Disease d = new Disease(Disease.DEFAULT);

                foreach (SimCountryData country in records)
                {
                    l.Population = country.Population;
                    l.HealthCareEffectiveness = country.HealthCareEffectiveness*country.HealthCareEffectiveness;
                    l.Area = ((double)country.Population) / country.PopDensity;
                    l.DoctorsPerCapita = country.PhysiciansPerCap;
                    l.MortalityRate = country.DeathRate;
                    l.BirthRate = country.BirthRate;
                    l.Sanitation = country.Sanitation*country.Sanitation;
                    l.SocialStability = country.SocialStability;

                    RunProgram<DiseaseNode>.Run(args, Integrator.Euler, null, $"D:/Desktop/DiseaseModelJunque/ComparativeCharts/{country.CountryCode}.csv", null, null, new DiseaseNode(p, l, d) { ContagiousAsymptomatic = 1 });
                    //foreach (EpidemicNode s in Behavior<EpidemicNode>.Run(new EpidemicNode(p, l, d), Integrator.Euler))
                    //{
                    //    Console.Write("{0:0.000},", s.TimeSliceNdx*s.TimeStep);
                    //    foreach (var getter in s.StockGetters)
                    //    {
                    //        Console.Write("{0:0.000},", getter(s));
                    //    }
                    //    Console.WriteLine();
                    //}
                    //break;
                }
            }
#pragma warning restore 162
        }
    }
}

