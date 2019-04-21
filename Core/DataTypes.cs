// ***********************************************************************
// Assembly         : Core
// Author           : pbosch
// Created          : 02-19-2019
//
// Last Modified By : pbosch
// Last Modified On : 03-08-2019
// ***********************************************************************
// <copyright file="DataTypes.cs" company="Highpoint Software Systems, LLC">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;

namespace Core
{
    /// <summary>
    /// Class AirportData.
    /// </summary>
    [Serializable]
    public class AirportData
    {
        /// <summary>
        /// Gets or sets the airport identifier.
        /// </summary>
        /// <value>The airport identifier.</value>
        public string AirportID { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City { get; set; }
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        public string Country { get; set; }
        /// <summary>
        /// Gets or sets the iata code.
        /// </summary>
        /// <value>The iata code.</value>
        public string IATACode { get; set; }
        /// <summary>
        /// Gets or sets the icao code.
        /// </summary>
        /// <value>The icao code.</value>
        public string ICAOCode { get; set; }
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public string Latitude { get; set; }
        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public string Longitude { get; set; }
        /// <summary>
        /// Gets or sets the altitude.
        /// </summary>
        /// <value>The altitude.</value>
        public string Altitude { get; set; }
        /// <summary>
        /// Gets or sets the timezone.
        /// </summary>
        /// <value>The timezone.</value>
        public string Timezone { get; set; }
        /// <summary>
        /// Gets or sets the DST.
        /// </summary>
        /// <value>The DST.</value>
        public string DST { get; set; }
        /// <summary>
        /// Gets or sets the tz.
        /// </summary>
        /// <value>The tz.</value>
        public string Tz { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        [Ignore]
        public bool IsBusy { get; set; }

        /// <summary>
        /// Gets the airport code.
        /// </summary>
        /// <value>The airport code.</value>
        [Ignore]
        public string AirportCode => string.Equals(IATACode, UNKNOWN) ? ICAOCode : IATACode;

        /// <summary>
        /// Gets or sets the map x.
        /// </summary>
        /// <value>The map x.</value>
        [Ignore]
        public int MapX { get; set; }

        /// <summary>
        /// Gets or sets the map y.
        /// </summary>
        /// <value>The map y.</value>
        [Ignore]
        public int MapY { get; set; }

        /// <summary>
        /// The unknown
        /// </summary>
        [Ignore]
        public static string UNKNOWN = "\\N";

    }

    /// <summary>
    /// Class RouteData.
    /// </summary>
    [Serializable]
    public class RouteData
    {
        /// <summary>
        /// Gets or sets the airports.
        /// </summary>
        /// <value>The airports.</value>
        public static Dictionary<string,AirportData> Airports { get; set; }
        /// <summary>
        /// Gets or sets the airline.
        /// </summary>
        /// <value>The airline.</value>
        public string Airline { get; set; }
        /// <summary>
        /// Gets or sets the airline identifier.
        /// </summary>
        /// <value>The airline identifier.</value>
        public string AirlineID { get; set; }
        /// <summary>
        /// Gets or sets the source airport.
        /// </summary>
        /// <value>The source airport.</value>
        public string SourceAirport { get; set; }
        /// <summary>
        /// Gets or sets the source airport identifier.
        /// </summary>
        /// <value>The source airport identifier.</value>
        public string SourceAirportID { get; set; }
        /// <summary>
        /// Gets or sets the destination airport.
        /// </summary>
        /// <value>The destination airport.</value>
        public string DestinationAirport { get; set; }
        /// <summary>
        /// Gets or sets the destination airport identifier.
        /// </summary>
        /// <value>The destination airport identifier.</value>
        public string DestinationAirportID { get; set; }
        /// <summary>
        /// Gets or sets the code share.
        /// </summary>
        /// <value>The code share.</value>
        public string CodeShare { get; set; }
        /// <summary>
        /// Gets or sets the stops.
        /// </summary>
        /// <value>The stops.</value>
        public string Stops { get; set; }
        /// <summary>
        /// Gets or sets the equipment.
        /// </summary>
        /// <value>The equipment.</value>
        public string Equipment { get; set; }

        /// <summary>
        /// Gets from.
        /// </summary>
        /// <value>From.</value>
        [Ignore]
        public AirportData From => Airports[SourceAirportID];

        /// <summary>
        /// Gets to.
        /// </summary>
        /// <value>To.</value>
        [Ignore]
        public AirportData To => Airports[DestinationAirportID];
        /// <summary>
        /// Gets or sets the using.
        /// </summary>
        /// <value>The using.</value>
        [Ignore]
        public EquipmentData Using { get; set; } // TODO: Normalize this.
        /// <summary>
        /// Gets or sets the maximum passengers.
        /// </summary>
        /// <value>The maximum passengers.</value>
        [Ignore]
        public int MaxPassengers { get; set; }
        /// <summary>
        /// Gets or sets the route identifier.
        /// </summary>
        /// <value>The route identifier.</value>
        [Ignore]
        public int RouteID { get; set; }
    }

    /// <summary>
    /// Class EquipmentData.
    /// </summary>
    [Serializable]
    public class EquipmentData
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the iata code.
        /// </summary>
        /// <value>The iata code.</value>
        public string IATACode { get; set; }
        /// <summary>
        /// Gets or sets the icao code.
        /// </summary>
        /// <value>The icao code.</value>
        public string ICAOCode { get; set; }
        /// <summary>
        /// Gets or sets the equipment identifier.
        /// </summary>
        /// <value>The equipment identifier.</value>
        [Ignore]
        public int EquipmentID { get; set; }
    }
    /// <summary>
    /// Class EquipmentLoadingData.
    /// </summary>
    [Serializable]
    public class EquipmentLoadingData
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets the pax capacity.
        /// </summary>
        /// <value>The pax capacity.</value>
        public int PaxCapacity { get; set; }
    }

    /// <summary>
    /// Class PaxMovementData.
    /// </summary>
    [Serializable]
    public class PaxMovementData
    {
        /// <summary>
        /// Gets or sets from airport identifier.
        /// </summary>
        /// <value>From airport identifier.</value>
        public string FromAirportID { get; set; }
        /// <summary>
        /// Gets or sets from lat.
        /// </summary>
        /// <value>From lat.</value>
        public double FromLat { get; set; }
        /// <summary>
        /// Gets or sets from lon.
        /// </summary>
        /// <value>From lon.</value>
        public double FromLon { get; set; }
        /// <summary>
        /// Gets or sets from map x.
        /// </summary>
        /// <value>From map x.</value>
        public double FromMapX { get; set; }
        /// <summary>
        /// Gets or sets from map y.
        /// </summary>
        /// <value>From map y.</value>
        public double FromMapY { get; set; }
        /// <summary>
        /// Converts to airportid.
        /// </summary>
        /// <value>To airport identifier.</value>
        public string ToAirportID { get; set; }
        /// <summary>
        /// Converts to lat.
        /// </summary>
        /// <value>To lat.</value>
        public double ToLat { get; set; }
        /// <summary>
        /// Converts to lon.
        /// </summary>
        /// <value>To lon.</value>
        public double ToLon { get; set; }
        /// <summary>
        /// Converts to mapx.
        /// </summary>
        /// <value>To map x.</value>
        public double ToMapX { get; set; }
        /// <summary>
        /// Converts to mapy.
        /// </summary>
        /// <value>To map y.</value>
        public double ToMapY { get; set; }
        /// <summary>
        /// Gets or sets the number of passengers.
        /// </summary>
        /// <value>The number of passengers.</value>
        public int NumberOfPassengers { get; set; }
    }
}
