using System;
using System.IO;
using Core;

namespace DataFusion
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string sourcePath = "../../Data/";
            string destinationPath = "../../../../Data/";

            string densityData = Path.Combine(sourcePath,"gpw_v4_population_density_rev10_2015_30_min.asc");
            string landAreaData = Path.Combine(sourcePath, "gpw_v4_land_water_area_rev10_landareakm_30_min.asc");
            string waterAreaData = Path.Combine(sourcePath, "gpw_v4_land_water_area_rev10_waterareakm_30_min.asc");
            string airports = Path.Combine(sourcePath, "airports.dat");
            string routes = Path.Combine(sourcePath, "routes.dat");
            string equipment = Path.Combine(sourcePath, "planes.dat");
            string equipmentLoading = Path.Combine(sourcePath, "equipmentLoading.dat");

            MapData data = MapDataLoader.FromASC(densityData, landAreaData, waterAreaData, airports, routes, equipment, equipmentLoading);

            MapData.SaveTo(data, Path.Combine(destinationPath, "MapData.dat"));

        }
    }
}
