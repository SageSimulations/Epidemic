using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace Core.Graphical
{
    public static class GraphicalUtilities
    {
        public static void GraphicsCoordsToMapCoords(Size graphicsSize, Size worldModelSize, int graphicsX, int graphicsY, out int mapX, out int mapY)
        {
            mapX = (int)((double)graphicsX * ((double)worldModelSize.Width / (double)graphicsSize.Width));
            mapY = (int)((double)graphicsY * ((double)worldModelSize.Height / (double)graphicsSize.Height));
            //Console.WriteLine($"Graphics ({graphicsX},{graphicsY}) is map ({mapX},{mapY}).");
        }

        public static Bitmap CreateFoundationBitmap(MapData mapData, bool includeAirRoutes = true)
        {
            Bitmap bitmap = new Bitmap(mapData.Width, mapData.Height, PixelFormat.Format32bppArgb);

            DataLinearizer dl = new DataLinearizer(CellData.PopulationDensityEnumerator(mapData.CellData), Core.CellData.NO_DATA, 255, 5);
            for (int col = 0; col < mapData.Width; col++)
            {
                for (int row = 0; row < mapData.Height; row++)
                {
                    Color color = Color.Transparent;
                    switch (mapData.CellData[col, row].CellState)
                    {
                        case CellState.Unknown:
                            color = Color.Wheat;
                            break;
                        case CellState.Occupied:
                            double popDensity = mapData.CellData[col, row].PopulationDensity;
                            int intensity = 255 - dl.getBinFor(popDensity);
                            color = Color.FromArgb(intensity, intensity, intensity);
                            break;
                        case CellState.Unoccupied:
                            color = Color.White;
                            break;
                        case CellState.Ocean:
                            color = Color.Blue;
                            break;
                        default:
                            color = Color.Empty;
                            break;
                    }
                    bitmap.SetPixel(col, row, color);
                }
            }

            if (includeAirRoutes)
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    foreach (RouteData rd in mapData.BusyRoutes)
                    {
                        //file.WriteLine($"Route {i} is from {rt.From.Name} at {rt.From.Latitude},{rt.From.Longitude} ({rt.From.MapX},{rt.From.MapY}) to {rt.To.Name} at {rt.To.Latitude},{rt.To.Longitude} ({rt.To.MapX},{rt.To.MapY})");
                        graphics.DrawLine(new Pen(Color.FromArgb(4, 255, 0, 128), 1.0f), rd.From.MapX, rd.From.MapY,
                            rd.To.MapX, rd.To.MapY);
                    }
                }
            }

            return bitmap;
        }
    }

    public static class ControlHelpers
    {
        public static void InvokeIfRequired<T>(this T control, Action<T> action) where T : ISynchronizeInvoke
        {
            try
            {
                if (control.InvokeRequired)
                {

                    control.Invoke(new Action(() => action(control)), null);
                }
                else
                {
                    action(control);
                }
            }
            catch (ObjectDisposedException sode)
            {
                // Do nothing. Windows was closed.
                Console.WriteLine(sode.Message);
            }
        }
    }
}
