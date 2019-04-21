// ***********************************************************************
// Assembly         : Core
// Author           : pbosch
// Created          : 03-07-2019
//
// Last Modified By : pbosch
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="MapData.cs" company="Highpoint Software Systems, LLC">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core
{
    /// <summary>
    /// Class MapData.
    /// </summary>
    [Serializable]
    public class MapData
    {
        /// <summary>
        /// Gets or sets the cell data.
        /// </summary>
        /// <value>The cell data.</value>
        public CellData[,] CellData { get; set; }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets the ll corner longitude.
        /// </summary>
        /// <value>The ll corner longitude.</value>
        public int LLCornerLongitude { get; set; }
        /// <summary>
        /// Gets or sets the ll corner latitude.
        /// </summary>
        /// <value>The ll corner latitude.</value>
        public int LLCornerLatitude { get; set; }
        /// <summary>
        /// Gets or sets the size of the map cell.
        /// </summary>
        /// <value>The size of the map cell.</value>
        public double MapCellSize { get; set; }
        /// <summary>
        /// Gets or sets the busy routes.
        /// </summary>
        /// <value>The busy routes.</value>
        public List<RouteData> BusyRoutes { get; set; }
        /// <summary>
        /// Gets or sets the airports.
        /// </summary>
        /// <value>The airports.</value>
        public Dictionary<string, AirportData> Airports { get; set; }
        /// <summary>
        /// Gets or sets the data missing.
        /// </summary>
        /// <value>The data missing.</value>
        public double DataMissing { get; set; }
        /// <summary>
        /// Saves to.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="pathname">The pathname.</param>
        public static void SaveTo(MapData data, string pathname)
        {
            FileStream fs = new FileStream(pathname, FileMode.Create);

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            try
            {
                new BinaryFormatter().Serialize(fs, data);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        /// <summary>
        /// Loads from.
        /// </summary>
        /// <param name="pathname">The pathname.</param>
        /// <returns>MapData.</returns>
        public static MapData LoadFrom(string pathname)
        {
            // Declare the hashtable reference.
            MapData retval;

            // Open the file containing the data that you want to deserialize.
            FileStream fs = new FileStream(pathname, FileMode.Open);
            try
            {
                retval = (MapData)new BinaryFormatter().Deserialize(fs);
                RouteData.Airports = retval.Airports;
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }

            return retval;
        }

        /// <summary>
        /// Lats the lon to data xy.
        /// </summary>
        /// <param name="lat">The lat.</param>
        /// <param name="lon">The lon.</param>
        /// <param name="nRows">The n rows.</param>
        /// <param name="xllcorner">The xllcorner.</param>
        /// <param name="yllcorner">The yllcorner.</param>
        /// <param name="cellsize">The cellsize.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public static void LatLonToDataXY(double lat, double lon, int nRows, double xllcorner, double yllcorner, double cellsize, out int x, out int y)
        {
            x = (int)((lon - xllcorner) / cellsize);
            y = nRows + (int)((-lat + yllcorner) / cellsize);
        }

        /// <summary>
        /// Lats the lon to data xy.
        /// </summary>
        /// <param name="lat">The lat.</param>
        /// <param name="lon">The lon.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void LatLonToDataXY(double lat, double lon, out int x, out int y)
        {
            x = (int)((lon - LLCornerLongitude) / MapCellSize);
            y = Height + (int)((-lat + LLCornerLatitude) / MapCellSize);
        }

        /// <summary>
        /// Datas the xy to lat lon.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="lat">The lat.</param>
        /// <param name="lon">The lon.</param>
        public void DataXYToLatLon(int x, int y, out double lat, out double lon)
        {
            lon = LLCornerLongitude + (MapCellSize * x);
            lat = LLCornerLatitude + (MapCellSize * (Height - y));
        }
    }
}
