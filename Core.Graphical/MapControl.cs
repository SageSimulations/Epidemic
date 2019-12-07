#define USE_PARALLEL
#define ACCOMODATE_AIR_TRAVEL
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Core.Graphical
{
    public class MapControl : PictureBox
    {
        private ILineWriter console = Locator.Resolve<ILineWriter>("console");
        private Bitmap m_foundation;
        private Func<DiseaseNode, Color> m_colorMapper = DefaultColorMapper;
        private static double COLOR_THRESHOLD = 1E-5;

        public WorldModel MyWorldModel { get; set; }
    

        public void AssignWorldModel(WorldModel worldModel)
        {
            MyWorldModel = worldModel;
            m_foundation = GraphicalUtilities.CreateFoundationBitmap(worldModel.MapData, false);
            Image = m_foundation;
            worldModel.NewIterationAvailable += Render;
        }

        private void Render(DiseaseNode[,] nodes, double[] routeContamination, List<RouteData> busyRoutes, List<OutbreakResponseTeam> orts)
        {
            this.InvokeIfRequired(delegate (MapControl mc) { mc._Render(nodes, routeContamination, busyRoutes, orts); });
        }

        private void _Render(DiseaseNode[,] nodes, double[] routeContamination, List<RouteData> busyRoutes, List<OutbreakResponseTeam> orts)
        {
            int dataWidth = nodes.GetLength(0);
            int dataHeight = nodes.GetLength(1);
            Bitmap image = (Bitmap)m_foundation.Clone();

            // Add epidemic layer.
            for (int x = 0; x < dataWidth; x++)
            {
                for (int y = 0; y < dataHeight; y++)
                {
                    DiseaseNode theNode = nodes[x, y];
                    Color c = m_colorMapper(theNode);
                    if (c.A > 0) image.SetPixel(x, y, c);
                }
            }


            // Add routes.
            int nActiveRoutes = 0;
            //Link Colors:
            Color linkRed = Color.FromArgb(3, 255, 0, 0);
            Color linkOrange = Color.FromArgb(3, 255, 165, 0);
            Color linkGreen = Color.FromArgb(3, 0, 255, 0);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                IEnumerator<RouteData> rd = busyRoutes.GetEnumerator();
                for (int i = 0; i < routeContamination.Length; i++)
                {
                    Color linkColor;
                    double trouble = routeContamination[i];
                    if (!double.IsNegativeInfinity(trouble))
                    {
                        if (trouble < .1) linkColor = linkGreen;
                        else if (trouble < 2) linkColor = linkOrange;
                        else linkColor = linkRed;
                        rd.MoveNext();
                        RouteData route = rd.Current;
                        if (route != null)
                        {
                            int xf = route.From.MapX;
                            int yf = route.From.MapY;
                            int xt = route.To.MapX;
                            int yt = route.To.MapY;
                            graphics.DrawLine(new Pen(linkColor, 1.0f), xf, yf, xt, yt);
                            nActiveRoutes++;
                        }
                        else
                        {
                            throw new NullReferenceException();
                        }
                    }
                }

                // Now draw the Outbreak Response Teams.
                foreach (OutbreakResponseTeam ort in orts)
                {
                    graphics.DrawEllipse(new Pen(Color.Black, 1.0f), HWrap(ort.Location.X-1), VWrap(ort.Location.Y-1), 2, 2);
                    graphics.DrawEllipse(new Pen(ort.Color, 3.0f), HWrap(ort.Location.X - 2), VWrap(ort.Location.Y - 2), 4, 4);
                    graphics.DrawEllipse(new Pen(Color.White, 1.0f), HWrap(ort.Location.X - 3), VWrap(ort.Location.Y - 3), 6, 6);
                }
            }
            //Console.WriteLine($"There are {nActiveRoutes} active routes.");
            Image = image;
        }

        private static Color XDefaultColorMapper(DiseaseNode node)
        {
            if (double.IsNaN(node.Population)) return Color.Black;

            if (node.Population < 0.001) return Color.Transparent;

            double impact = Math.Abs(node.ContagiousAsymptomatic) + Math.Abs(node.ContagiousSymptomatic) + Math.Abs(node.NonContagiousInfected + node.Killed);
            if (impact == 0) return Color.Transparent;

            double severity = Math.Sqrt(impact) / Math.Sqrt(node.Population);

            double d = Math.Max(Math.Min(6.0, Math.Log(severity) + 3), 0); // zero to six

            if (d < 3.0) return Utility.Gradient(Color.Yellow, Color.Red, d / 3); // Zero to 3 is yellow to red.

            return Utility.Gradient(Color.Red, Color.Black, Math.Min(((d - 3) / 3.0), 1.0));
            //return Color.FromArgb(alpha, red, green, 0); // <--- TODO: Population can go negative. Fix this.   
        }

        private static Color DefaultColorMapper(DiseaseNode node)
        {
            if (double.IsNaN(node.Population)) return Color.Black;

            if (node.Population < 0.001) return Color.Transparent;

            double severity = Math.Min(Math.Abs(512 * (node.ContagiousAsymptomatic + node.ContagiousSymptomatic +
                              node.Killed) / node.Population), 255);
            if (severity < COLOR_THRESHOLD) return Color.Transparent;


            double d = Math.Max(Math.Min(6.0, Math.Log(severity) + 3.0), 0); // zero to six

            if (d < 3.0) return Utility.Gradient(Color.Yellow, Color.Red, d / 3); // Zero to 3 is yellow to red.

            return Utility.Gradient(Color.Red, Color.Black, Math.Min(((d - 3) / 6.0), 1.0));
            //return Color.FromArgb(alpha, red, green, 0); // <--- TODO: Population can go negative. Fix this.   
        }

        private int HWrap(int x)
        {
            return x > 0
                        ? x < MyWorldModel.Size.Width ? x : x - MyWorldModel.Size.Width
                        : x + MyWorldModel.Size.Width;
        }

        private int VWrap(int y)
        {
            return y > 0
                        ? y < MyWorldModel.Size.Height ? y : y - MyWorldModel.Size.Height
                        : y + MyWorldModel.Size.Height;
        }
    }
}
