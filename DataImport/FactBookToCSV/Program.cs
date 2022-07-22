using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using Core;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using CsvHelper.Configuration;

namespace FactBookToCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string,string> countryCodeForName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            using (var reader = new StreamReader(@"../../Data/ISO_3166-1_alpha-3.tdf"))
            {
                var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                    Delimiter = "\t",
                    IgnoreBlankLines = false
                };
                using (var csv = new CsvReader(reader, config)) {
                    foreach (CountryNameAndCode countryNameAndCode in csv.GetRecords<CountryNameAndCode>())
                    {
                        countryCodeForName.Add(countryNameAndCode.Name, countryNameAndCode.Code);
                    }
                }
            }
            string json = File.ReadAllText(@"../../Data/Factbook.json");
            dynamic d = JObject.Parse(json);

            using (TextWriter tw = new StreamWriter(@"../../../../Data/CountryData.dat"))
            {
                tw.WriteLine(SimCountryData.CSVHeader);
                List<SimCountryData> countries = new List<SimCountryData>();
                //foreach (string countryName in countriesOfInterest)

                List<String> skipList = new List<string>(m_skipList);
                // Special Cases:
                // d["countries"]["Chad"].energy.electricity.access.total_electrification = 33;
                JObject chadElectricity = d["countries"]["chad"].data.energy.electricity as JObject;
                chadElectricity.Add("access", JObject.Parse(@"{""populationWithElectricity"":{""value"":""33""}}")); // From 'population without electricity' and 'population' attributes.
                //JObject nauruElectricity = d["countries"]["nauru"].data.energy.electricity as JObject;
                //nauruElectricity.Add("access", JObject.Parse(@"{""populationWithElectricity"":{""value"":""66""}}"));
                JObject taiwanElectricity = d["countries"]["taiwan"].data.energy.electricity as JObject;
                taiwanElectricity.Add("access", JObject.Parse(@"{""populationWithElectricity"":{""value"":""99""}}"));
                JObject FalklandsElectricity = d["countries"]["falkland_islands_islas_malvinas"].data.energy.electricity as JObject;
                FalklandsElectricity.Add("access", JObject.Parse(@"{""populationWithElectricity"":{""value"":""99""}}"));
                JObject WesternSaharaElectricity = d["countries"]["western_sahara"].data.energy.electricity as JObject;
                WesternSaharaElectricity.Add("access", JObject.Parse(@"{""populationWithElectricity"":{""value"":""99""}}"));

                foreach (var country in d["countries"])
                {
                    string name = "Unknown";
                    string failedAt = "name";
                    try
                    {
                        var _countryData = country.First.data;

                        name = _countryData.name.Value;
                        if (skipList.Contains(name)) continue;

                        string countryCode;
                        if (!countryCodeForName.TryGetValue(name, out countryCode))
                        {
                            Console.WriteLine("Failed to get country code for " + name);
                        }

                        failedAt = "totalLand";
                        double totalLand = AttemptToRead(
                            () => _countryData.geography.area.land.value.Value,
                            () => _countryData.geography.area.total.value.Value
                            );

                        failedAt = "population";
                        if (_countryData.people == null) continue;
                        var population = (double) _countryData.people.population.total.Value;

                        failedAt = "birthRate";
                        var birthRate = (double) _countryData.people.birth_rate.births_per_1000_population.Value/365.0;

                        failedAt = "deathRate";
                        var deathRate = (double) _countryData.people.death_rate.deaths_per_1000_population.Value/365.0;

                        failedAt = "sanitation";
                        var sanitation = AttemptToRead(
                            () => _countryData.people.sanitation_facility_access.unimproved.total.value.Value / 100.0,
                            () => MISSING_SANITATION
                            );

                        failedAt = "drsPerCap";
                        double drsPerCap = AttemptToRead(
                            () => _countryData.people.physicians_density.physicians_per_1000_population.Value / 1000.0,
                            () => AVERAGE_DRS_PER_CAP
                            );

                        failedAt = "healthSpendGDP";
                        var healthSpendGDP = AttemptToRead(
                            () => _countryData.people.health_expenditures.percent_of_gdp.Value / 100.0,
                            () => MISSING_GDP_HEALTH_SPEND
                            );

                        failedAt = "totalGDP";
                        var totalGDP = (double) _countryData.economy.gdp.purchasing_power_parity.annual_values[0].value.Value;

                        failedAt = "literacy";
                        var literacy = AttemptToRead(
                            () => _countryData.people.literacy.total_population.value / 100.0,
                            () => AVERAGE_LITERACY
                            );

                        failedAt = "populationWithElectricity";
                        double populationWithElectricity = AttemptToRead(
                            () => _countryData.energy.electricity.access.total_electrification.value / 100.0,
                            () => _countryData.energy.electricity.access.populationWithElectricity.value / 100.0
                            );

                        failedAt = "socialStability";
                        var socialStability = Math.Sqrt(((literacy*literacy) + (populationWithElectricity*populationWithElectricity))/2.0);

                        SimCountryData simCountryData = new SimCountryData()
                        {
                            Name = name,
                            BirthRate = birthRate/365.0,
                            DeathRate = deathRate/365.0,
                            HealthSpendPerCap = healthSpendGDP*totalGDP/population,
                            PhysiciansPerCap = drsPerCap,
                            PopDensity = population/totalLand,
                            Population = (int) population,
                            Sanitation = 1.0 - sanitation,
                            SocialStability = socialStability,
                            CountryCode = countryCode
                        };
                        countries.Add(simCountryData);
                        tw.WriteLine(simCountryData.AsCSV);
                    }
                    catch (RuntimeBinderException rbe)
                    {
                        Console.WriteLine($"\"{name}\" read failed at {failedAt}");
                        // TODO: Log failure
                    }
                }
            }
        }

        private static double AttemptToRead(Func<dynamic> firstSource, Func<dynamic> fallback)
        {
            double retval;
            try
            {
                retval = firstSource();
            }
            catch (RuntimeBinderException e)
            {
                retval = fallback();
            }
            return retval;
        }

        private class CountryNameAndCode
        {
            [Index(0)]
            public string Name { get; set; }
            [Index(1)]
            public string Code { get; set; }
        }

        // Averages of all fully-readable countries.
        // "Country, Population, PopDensity, PhysiciansPerCap, Sanitation, SocialStability, BirthRate,  DeathRate,  HealthSpendPerCap"
        //           46286418.11 204.1517229 0.001571145       0.716908397 0.814612529      0.000155628 5.94529E-05 954.012061

        private static readonly double AVERAGE_DRS_PER_CAP = 0.001571145;
        private static readonly double AVERAGE_LITERACY   = 0.8;// Total guess.
        private static readonly double MISSING_GDP_HEALTH_SPEND = 50; // North Korea and Somalia lack data. $50 per person per year seems about right.
        private static readonly double MISSING_SANITATION = 0.85; // The countries missing sanitation data are (*mostly) very good. (Bermuda, Curacao, Hong Kong, Liechtenstein, New Zealand, Taiwan)

        // Some, like Kosovo, have very, very poor data, so we skip them, and map them to a nearby country at run time.
        private static readonly string[] m_skipList = {"Kosovo", "World", "Akrotiri", "American Samoa", "Anguilla", "Antarctica", "Arctic Ocean", "Aruba", "Ashmore And Cartier Islands", "Atlantic Ocean", "Bermuda", "Bouvet Island", "British Indian Ocean Territory", "British Virgin Islands", "Cayman Islands", "Christmas Island", "Clipperton Island", "Cocos (Keeling) Islands", "Comoros", "Cook Islands", "Coral Sea Islands", "Curacao", "Dhekelia", "Faroe Islands", "French Polynesia", "Gaza Strip", "Gibraltar", "Guam", "Guernsey", "Heard Island And Mcdonald Islands", "Holy See (Vatican City)", "Indian Ocean", "Isle Of Man", "Jan Mayen", "Jersey", "Kiribati", "Macau", "Marshall Islands", "Micronesia, Federated States Of", "Montserrat", "Nauru", "Navassa Island", "Niue", "Norfolk Island", "Northern Mariana Islands", "Pacific Ocean", "Paracel Islands", "Pitcairn Islands", "Saint Barthelemy", "Saint Helena, Ascension, And Tristan Da Cunha", "Saint Kitts And Nevis", "Saint Lucia", "Saint Martin", "Saint Pierre And Miquelon", "Saint Vincent And The Grenadines", "San Marino", "Sao Tome And Principe", "Sint Maarten", "Southern Ocean", "South Georgia And South Sandwich Islands", "Spratly Islands", "Svalbard", "Tokelau", "Turks And Caicos Islands", "Virgin Islands", "Wake Island", "Wallis And Futuna", "West Bank", "European Union"};
    }
}
