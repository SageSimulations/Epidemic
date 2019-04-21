// ***********************************************************************
// Assembly         : Core
// Author           : pbosch
// Created          : 03-18-2019
//
// Last Modified By : pbosch
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="CountryData.cs" company="Highpoint Software Systems, LLC">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace Core
{
    /// <summary>
    /// Class CountryData.
    /// </summary>
    public class CountryData
    {
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        [Index(0)]
        [Category("Identity"), Description("The name of the country")]
        public string Country { get; set; }
        /// <summary>
        /// Gets or sets the ISO_3166-1_alpha-3 country code.
        /// </summary>
        /// <value>The ISO_3166-1_alpha-3 country code.</value>
        [Index(1)]
        [Category("Identity"), Description("the ISO_3166-1 alpha-3 country code")]
        public string CountryCode { get; set; }

        [Ignore]
        [Category("Politics"), Description("The country's government")]
        public NationalGovernment Government { get; set; }

        /// <summary>
        /// Gets or sets the population.
        /// </summary>
        /// <value>The population.</value>
        [Index(2)]
        [Category("Demographics")]
        public int Population { get; set; }

        /// <summary>
        /// Gets or sets the pop density.
        /// </summary>
        /// <value>The pop density.</value>
        [Index(3)]
        [Category("Demographics"), Description("Population Density")]
        public double PopDensity { get; set; }

        /// <summary>
        /// Gets or sets the physicians per cap.
        /// </summary>
        /// <value>The physicians per cap.</value>
        [Index(4)]
        [Category("Health"), Description("Number of physicians per person")]
        public double PhysiciansPerCap { get; set; }
        /// <summary>
        /// Gets or sets the sanitation.
        /// </summary>
        /// <value>The sanitation.</value>
        [Index(5)]
        [Category("Health"), Description("Rating from 0 (poor) to 1 (good) of the country's sanitation")]
        public double Sanitation { get; set; }
        /// <summary>
        /// Gets or sets the social stability.
        /// </summary>
        /// <value>The social stability.</value>
        [Index(6)]
        [Category("Politics"), Description("Rating from 0 (poor) to 1 (good) of the country's social stability")]
        public double SocialStability { get; set; }
        /// <summary>
        /// Gets or sets the birth rate.
        /// </summary>
        /// <value>The birth rate.</value>
        [Index(7)]
        [Category("Demographics"), Description("Births per person per day")]
        public double BirthRate { get; set; }
        /// <summary>
        /// Gets or sets the death rate.
        /// </summary>
        /// <value>The death rate.</value>
        [Index(8)]
        [Category("Demographics"), Description("Deaths per person per day")]
        public double DeathRate { get; set; }
        /// <summary>
        /// Gets or sets the health spend per cap.
        /// </summary>
        /// <value>The health spend per cap.</value>
        [Index(9)]
        [Category("Health"), Description("Dollars spent per person on health care.")]
        public double HealthSpendPerCap { get; set; }
        /// <summary>
        /// Gets the health care effectiveness.
        /// </summary>
        /// <value>The health care effectiveness.</value>
        /// 
        [Ignore]
        [Browsable(false)]
        public double HealthCareEffectiveness => Math.Min(HealthSpendPerCap / 5000.0, 1.0);

        [Browsable(false)]
        public string AsCSV => $"\"{Country}\",{CountryCode}, {Population}, {PopDensity}, {PhysiciansPerCap}, {Sanitation}, {SocialStability}, {BirthRate}, {DeathRate}, {HealthSpendPerCap}";

        [Browsable(false)]
        public static string CSVHeader => "Country, CountryCode, Population, PopDensity, PhysiciansPerCap, Sanitation, SocialStability, BirthRate, DeathRate, HealthSpendPerCap";


        public static List<CountryData> LoadFrom(string sourcePath)
        {
            List<CountryData> records;
            using (var reader = new StreamReader(sourcePath))
            {
                using (
                    var csv = new CsvReader(reader, new Configuration() {HasHeaderRecord = true, HeaderValidated = null})
                    )
                {
                    csv.Configuration.RegisterClassMap<CountryDataMap>();
                    var v = csv.GetRecords<CountryData>();
                    records = new List<CountryData>(v);
                }
            }

            return records;
        }

        public sealed class CountryDataMap : ClassMap<CountryData>
        {
            public CountryDataMap()
            {
                AutoMap();
                Map(m => m.Government).Ignore();
            }
        }
    }
}