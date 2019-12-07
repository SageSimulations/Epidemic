
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using CsvHelper;
using CsvHelper.Configuration;

namespace DataFusion
{
    public static class MapDataLoader
    {


        public static MapData FromASC(string densityDataFile, string landAreaFile, string waterAreaFile,
            string airportDataFile, string routeDataFile, string equipmentDataFile, string equipmentLoadingDataFile)
        {
            MapData retval = new MapData();
            ReadLandAreaData(retval, landAreaFile);
            ReadWaterAreaFile(retval, waterAreaFile);
            ReadDensityDataFile(retval, densityDataFile);
            GenerateCountryCodes(retval);
            CreateRouteData(retval, airportDataFile, routeDataFile, equipmentDataFile, equipmentLoadingDataFile);

            return retval;
        }

        private static void GenerateCountryCodes(MapData target)
        {
            Parallel.ForEach(Enumerable.Range(0, target.Width - 1), i =>
            {
                for (int j = 0; j < target.Height; j++)
                {
                    double lat, lon;
                    target.DataXYToLatLon(i, j, out lat, out lon);
                    string cc = ReverseGeocodeService.CountryCodeForLatAndLong(lat, lon);
                    target.CellData[i, j].CountryCode = cc;
                    if ( (i+j) % 10 == 0) Console.WriteLine($"Latitude {lat:F0}, Longitude {lon:F0} : {cc}");
                }
            });
        }

        private static void ReadLandAreaData(MapData target, string landAreaFile)
        {
            double[,] landArea = ReadDataFile(target, landAreaFile);
            for (int i = 0; i < target.Width; i++)
                for (int j = 0; j < target.Height; j++)
                {
                    target.CellData[i, j].LandArea = landArea[i, j];
                }
        }

        private static void ReadDensityDataFile(MapData target, string densityDataFile)
        {
            double[,] popDensity = ReadDataFile(target, densityDataFile);
            for (int i = 0; i < target.Width; i++)
                for (int j = 0; j < target.Height; j++) target.CellData[i, j].PopulationDensity = popDensity[i, j];
        }

        private static void ReadWaterAreaFile(MapData target, string waterAreaFile)
        {
            double[,] waterArea = ReadDataFile(target, waterAreaFile);
            for (int i = 0; i < target.Width; i++)
                for (int j = 0; j < target.Height; j++) target.CellData[i, j].WaterArea = waterArea[i, j];
        }

        private static double[,] ReadDataFile(MapData target, string dataFile)
        {
            bool inHeader = true;
            string[] space = { " " };
            double[,] data = new double[0, 0];
            int row = 0;
            foreach (string line in File.ReadAllLines(dataFile))
            {
                string[] rowValues = line.Split(space, StringSplitOptions.RemoveEmptyEntries);
                if (inHeader)
                {
                    if (rowValues.Length == 2)
                    {
                        if (rowValues[0].Equals("ncols"))
                            target.Width = int.Parse(rowValues[1]);
                        else if (rowValues[0].Equals("nrows"))
                            target.Height = int.Parse(rowValues[1]);
                        else if (rowValues[0].Equals("xllcorner"))
                            target.LLCornerLongitude = int.Parse(rowValues[1]);
                        else if (rowValues[0].Equals("yllcorner"))
                            target.LLCornerLatitude = int.Parse(rowValues[1]);
                        else if (rowValues[0].Equals("noData"))
                            CellData.NO_DATA = int.Parse(rowValues[1]);
                        else if (rowValues[0].Equals("cellsize"))
                            target.MapCellSize = double.Parse(rowValues[1]);
                        else if (rowValues[0].Equals("NODATA_value"))
                            CellData.NO_DATA = int.Parse(rowValues[1]);
                        else
                            throw new ArgumentException($"Unable to parse parameter row : {rowValues[0]} {rowValues[1]}");
                    }
                    else
                    {
                        inHeader = false;
                        data = new double[target.Width, target.Height];
                    }
                }
                else
                {
                    for (int col = 0; col < target.Width; col++)
                    {
                        double datum = double.Parse(rowValues[col]);
                        data[col, row] = datum;
                    }
                    row++;
                }
            }

            if (target.CellData == null)
            {
                target.Width = data.GetLength(0);
                target.Height = data.GetLength(1);
                target.CellData = new CellData[target.Width, target.Height];
                for (int i = 0; i < target.Width; i++)
                    for (int j = 0; j < target.Height; j++)
                    {
                        target.CellData[i, j] = new CellData();
                    }
            }

            return data;
        }

        private static void CreateRouteData(MapData target, string airportDataFile, string routeDataFile, string equipmentDataFile,
            string equipmentLoadingDataFile)
        {

            Encoding csvEncoding = new UTF8Encoding(false); // MYSQL requires UTF-8 WITHOUT a BOM.

            int airportRankCutoff = 250;
            List<AirportData> airports;
            List<RouteData> routes;
            Configuration configuration = new Configuration()
            {
                HasHeaderRecord = false,
                MissingFieldFound = null,
                DetectColumnCountChanges = true
            };
            using (var reader = new StreamReader(airportDataFile))
            using (var csv = new CsvReader(reader, configuration))
            {
                airports = new List<AirportData>(csv.GetRecords<AirportData>());
            }
            Dictionary<string, AirportData> airportData = new Dictionary<string, AirportData>();
            airports.ForEach(
                n =>
                {
                    n.Name = n.Name.Replace(",", " -");
                    n.City = n.City.Replace(",", " -");
                    n.Country = n.Country.Replace(",", " -");
                    if (!String.IsNullOrEmpty(n.AirportID) && !airportData.ContainsKey(n.AirportID))
                        airportData.Add(n.AirportID, n);
                }
                );

            target.Airports = airportData;
            using (var reader = new StreamReader(routeDataFile))
            using (var csv = new CsvReader(reader, configuration))
            {
                routes = new List<RouteData>(csv.GetRecords<RouteData>());
            }

            RouteData.Airports = airportData;

            List<RouteData> fubarRoutes = new List<RouteData>(routes.Where(
                n => string.Equals("\\N", n.SourceAirportID) ||
                     string.Equals("\\N", n.DestinationAirportID) ||
                     !airportData.ContainsKey(n.SourceAirportID) ||
                     !airportData.ContainsKey(n.DestinationAirportID)));

            fubarRoutes.ForEach(n => routes.Remove(n));

            Dictionary<string, int> airportUsage = new Dictionary<string, int>();
            foreach (RouteData rd in routes)
            {
                if (!airportUsage.ContainsKey(rd.SourceAirportID)) airportUsage.Add(rd.SourceAirportID, 0);
                if (!airportUsage.ContainsKey(rd.DestinationAirportID)) airportUsage.Add(rd.DestinationAirportID, 0);
                airportUsage[rd.SourceAirportID]++;
                airportUsage[rd.SourceAirportID]++;
            }
            SortedList<int, string> sorted = new SortedList<int, string>(new Ranker());
            foreach (var v in airportUsage) sorted.Add(v.Value, v.Key);

            int rank = 1;
            foreach (KeyValuePair<int, string> pair in sorted.Reverse())
            {

                airportData[pair.Value].IsBusy = true;
                if (rank++ > airportRankCutoff) break;
            }

            foreach (var airportDatum in airportData)
            {
                DeriveMapCoordinatesFor(target, airportDatum.Value);
            }

            int ndx = 0;

            List<EquipmentData> equipment;
            Dictionary<string, EquipmentData> equipmentData = new Dictionary<string, EquipmentData>();
            using (var reader = new StreamReader(equipmentDataFile))
            using (var csv = new CsvReader(reader, configuration))
            {
                equipment = new List<EquipmentData>(csv.GetRecords<EquipmentData>());
            }
            equipment.ForEach(n => n.EquipmentID = ndx++);

            List<EquipmentLoadingData> equipmentLoading;
            using (var reader = new StreamReader(equipmentLoadingDataFile))
            using (var csv = new CsvReader(reader, configuration))
            {
                equipmentLoading = new List<EquipmentLoadingData>(csv.GetRecords<EquipmentLoadingData>());
            }

            Dictionary<string, int> equipmentLoadingData = new Dictionary<string, int>();
            equipmentLoading.ForEach(n => equipmentLoadingData.Add(n.Type, n.PaxCapacity));

            equipment.ForEach(n => { if (!equipmentData.ContainsKey(n.IATACode)) equipmentData.Add(n.IATACode, n); });

            target.BusyRoutes = new List<RouteData>();
            ndx = 0;
            foreach (RouteData routeData in routes)
            {
                AirportData src, dest;
                EquipmentData equipt;
                airportData.TryGetValue(routeData.SourceAirportID, out src);
                airportData.TryGetValue(routeData.DestinationAirportID, out dest);
                equipmentData.TryGetValue(routeData.Equipment, out equipt);
                if (src != null && src.IsBusy && dest != null && dest.IsBusy)
                {
                    target.BusyRoutes.Add(routeData);
                    routeData.RouteID = ndx++;
                }
                if (equipt != null)
                {
                    routeData.Using = equipt;
                }
            }

            int undefinedEquipment = 0;
            foreach (RouteData busyRoute in target.BusyRoutes)
            {
                if (busyRoute.Equipment == null) undefinedEquipment++;
                else
                {
                    string[] allEquipment = busyRoute.Equipment.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (allEquipment.Length > 0)
                    {
                        int paxCap = 0;
                        foreach (string s in allEquipment)
                        {
                            paxCap += equipmentLoadingData[s];
                        }
                        busyRoute.MaxPassengers = paxCap / allEquipment.Length; // Average.
                    }
                }
            }
            if (undefinedEquipment > 0) Console.WriteLine($"There were {undefinedEquipment} routes defined that had no assigned equipment.");


#if GENERATE_PLANES_IN_USE
            List<string> planesInUseList = new List<string>();
            int undefinedEquipment = 0;
            foreach (RouteData busyRoute in m_busyRoutes)
            {
                if (busyRoute.Equipment == null) undefinedEquipment++;
                else
                {
                    string[] allequipt = busyRoute.Equipment.Split(new[] { ' ' });
                    foreach (string equipt in allequipt)
                    {
                        if (!planesInUseList.Contains(equipt))
                            planesInUseList.Add(equipt); //busyRoute.Using.Name);
                    }
                }
            } 
            planesInUseList.ForEach(n=> file.WriteLine(n));
            file.Close();
#endif
            Dictionary<string, int> paxTravel = new Dictionary<string, int>();
            // Consolidate Busy routes
            foreach (RouteData busyRoute in target.BusyRoutes)
            {
                string key = $"{busyRoute.From.AirportID}|{busyRoute.To.AirportID}";
                if (!paxTravel.ContainsKey(key)) paxTravel.Add(key, 0);
                paxTravel[key] += busyRoute.MaxPassengers;
            }

            List<PaxMovementData> paxMovement = new List<PaxMovementData>();
            foreach (KeyValuePair<string, int> pair in paxTravel)
            {
                string[] icaoCodes = pair.Key.Split('|');
                AirportData from = airportData[icaoCodes[0]];
                AirportData to = airportData[icaoCodes[1]];
                paxMovement.Add(new PaxMovementData()
                {
                    FromAirportID = from.AirportID,
                    FromLat = double.Parse(from.Latitude),
                    FromLon = double.Parse(from.Longitude),
                    FromMapX = from.MapX,
                    FromMapY = from.MapY,
                    ToAirportID = to.AirportID,
                    ToLat = double.Parse(to.Latitude),
                    ToLon = double.Parse(to.Longitude),
                    ToMapX = to.MapX,
                    ToMapY = to.MapY,
                    NumberOfPassengers = pair.Value
                });
            }

            foreach (string fn in Directory.GetFiles("../../Data/DBStaging/", "*.csv")) new FileInfo(fn).Delete();

            using (TextWriter writer = new StreamWriter("../../Data/DBStaging/paxMovement.csv", false, csvEncoding))
            {
                var csv = new CsvWriter(writer, configuration);
                csv.WriteRecords(paxMovement); // where values implements IEnumerable
            }

            using (
                TextWriter writer = new StreamWriter("../../Data/DBStaging/airports.csv", false, csvEncoding))
            {
                foreach (AirportData data in airportData.Values)
                {
                    writer.WriteLine(
                        $"{data.AirportID},{data.Name},{data.City},{data.Country},{(string.Equals(data.IATACode, AirportData.UNKNOWN) ? "" : data.IATACode)},{(string.Equals(data.ICAOCode, AirportData.UNKNOWN) ? "" : data.ICAOCode)},{data.Latitude},{data.Longitude},{(string.Equals(data.Altitude, "N") ? "-1" : data.Altitude)},{(string.Equals(data.Timezone, AirportData.UNKNOWN) ? "" : data.Timezone)},{(string.Equals(data.DST, AirportData.UNKNOWN) ? "" : data.DST)},{(string.Equals(data.Tz, AirportData.UNKNOWN) ? "" : data.Tz)},{data.Type},{data.Source},{(data.IsBusy ? "1" : "0")},{data.MapX},{data.MapY}");
                }
            }


            using (
                TextWriter writer = new StreamWriter("../../Data/DBStaging/equipment.csv", false, csvEncoding))
            {
                foreach (EquipmentData data in equipmentData.Values)
                {
                    int load;
                    if (!equipmentLoadingData.TryGetValue(data.IATACode, out load)) load = -1;
                    string iataCode = data.IATACode;
                    if (string.Equals("\\N", iataCode)) iataCode = null;
                    string icaoCode = data.ICAOCode;
                    if (string.Equals("\\N", icaoCode)) icaoCode = null;
                    writer.WriteLine($"{data.EquipmentID},{data.Name},{iataCode},{icaoCode},{load}");
                }
            }

            using (
                TextWriter writer = new StreamWriter("../../Data/DBStaging/routes.csv", false, csvEncoding))
            {
                foreach (RouteData data in target.BusyRoutes)
                {
                    //writer.WriteLine($"{data.RouteID},{(string.Equals(data.AirlineID, AirportData.UNKNOWN) ? "" : data.AirlineID)},{data.SourceAirportID},{data.DestinationAirportID}");
                    writer.WriteLine($"{data.RouteID},{data.SourceAirportID},{data.DestinationAirportID}");
                }
            }

            List<string> missing = new List<string>();
            using (
                TextWriter writer = new StreamWriter("../../Data/DBStaging/map_route_equipment.csv", false, csvEncoding)
                )
            {
                foreach (RouteData data in target.BusyRoutes)
                {
                    string[] allequipt = data.Equipment.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (allequipt.Length > 0)
                        foreach (var equipt in allequipt)
                        {
                            if (!equipmentData.ContainsKey(equipt))
                            {
                                if (!missing.Contains(equipt)) missing.Add(equipt);
                            }
                            else
                            {

                                int equipmentID = equipmentData[equipt].EquipmentID;
                                writer.WriteLine($"{data.RouteID},{equipmentID}");
                            }
                        }
                }
            }

            using (TextWriter writer = new StreamWriter("../../Data/DBStaging/pax_travel.csv", false, csvEncoding))
            {
                foreach (KeyValuePair<string, int> keyValuePair in paxTravel)
                {
                    string[] fromTo = keyValuePair.Key.Split('|');
                    writer.WriteLine($"{keyValuePair.Value}, {fromTo[0]}, {fromTo[1]}");
                }
            }
        }

        // And when this is done...
        /*
ALTER TABLE map_route_equipment
ADD FOREIGN KEY fk_route(RouteID)
REFERENCES routes(RouteID)
ON DELETE NO ACTION
ON UPDATE CASCADE;

ALTER TABLE map_route_equipment
ADD FOREIGN KEY fk_equipment(EquipmentID)
REFERENCES equipment(EquipmentID)
ON DELETE NO ACTION
ON UPDATE CASCADE;

ALTER TABLE routes
ADD FOREIGN KEY fk_fromAirport(SourceAirportID)
REFERENCES airports(AirportID)
ON DELETE NO ACTION
ON UPDATE CASCADE;

ALTER TABLE routes
ADD FOREIGN KEY fk_toAirport(DestinationAirportID)
REFERENCES airports(AirportID)
ON DELETE NO ACTION
ON UPDATE CASCADE;

ALTER TABLE pax_travel
ADD FOREIGN KEY fk_pax_travelToAirport(ToAirportID)
REFERENCES airports(AirportID)
ON DELETE NO ACTION
ON UPDATE CASCADE;

ALTER TABLE pax_travel
ADD FOREIGN KEY fk_pax_travelFromAirport(FromAirportID)
REFERENCES airports(AirportID)
ON DELETE NO ACTION
ON UPDATE CASCADE;
             */


        private class Ranker : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return x < y ? -1 : 1;
            }
        }

        private static void DeriveMapCoordinatesFor(MapData target, AirportData airport)
        {
            double lat, lon;
            if (double.TryParse(airport.Latitude, out lat) && double.TryParse(airport.Longitude, out lon))
            {
                int x, y;
                MapData.LatLonToDataXY(lat, lon, target.Height, target.LLCornerLongitude, target.LLCornerLatitude, target.MapCellSize, out x, out y);
                airport.MapX = x;
                airport.MapY = y;
            }
            else
            {
                Console.WriteLine($"Unable to ascertain the map location of {airport.Name}.");
            }
        }
    }
}
