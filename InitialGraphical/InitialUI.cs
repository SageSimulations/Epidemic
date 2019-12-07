using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Core;
using Core.Graphical;
using InitialGraphical;

namespace DiseaseView
{
    public partial class InitialUI : Form
    {
        private readonly MapData m_data;
        private WorldModel m_worldModel;
        private bool m_infectionMode;

        public InitialUI(MapData data)
        {
            m_data = data;
            InitializeComponent();
        }

        private void InitialUI_Load(object sender, EventArgs e)
        {
            m_worldModel = new WorldModel(m_data,SimCountryData.LoadFrom(@"../../../Data/CountryData.dat"));
            m_mapControl.AssignWorldModel(m_worldModel);

            PlotForm dc1 = new PlotForm("Mortality");
            dc1.Bind(m_worldModel,
            new[]{
                GetTotal(n => n.Killed),
                GetTotal(n => n.Dead)
            },
            new[]{
                "Killed",
                "Dead"
            });

            PlotForm dc2 = new PlotForm("Disease Stages");
            dc2.Bind(m_worldModel,
            new[]{
                GetTotal(n => n.ContagiousAsymptomatic),
                GetTotal(n => n.ContagiousSymptomatic),
                GetTotal(n => n.NonContagiousInfected),
                GetTotal(n => n.Immune),
            },
            new[]
            {
                "ContagiousAsymptomatic",
                "ContagiousSymptomatic",
                "NonContagiousInfected",
                "Immune"
            });

            m_mapControl.Show();
            dc1.Show();
            dc2.Show();




            /*PlotForm m_plotWindow1 = new PlotForm("Killed");
            m_plotWindow1.Bind(m_worldModel, GetTotal(n => n.Killed));
            m_plotWindow1.Show();

            PlotForm m_plotWindow2 = new PlotForm("Immune");
            m_plotWindow2.Bind(m_worldModel, GetTotal(n => n.Immune));
            m_plotWindow2.Show();

            PlotForm m_plotWindow3 = new PlotForm("Total Population");
            m_plotWindow3.Bind(m_worldModel, GetTotal(n => n.Population));
            m_plotWindow3.Show();

            PlotForm m_plotWindow4 = new PlotForm("Immunization Effort");
            m_plotWindow4.Bind(m_worldModel, GetTotal(n => n.Flows[2](n)));
            m_plotWindow4.Show();*/


            m_mapControl.MouseMove += (o, args) =>
            {
                double lat, lon;
                int x, y;
                GraphicalUtilities.GraphicsCoordsToMapCoords(panel1.Size, m_worldModel.Size, args.X, args.Y, out x,
                    out y);
                m_data.DataXYToLatLon(x, y, out lat, out lon);
                string where = ReverseGeocodeService.CountryNameForLatAndLong(lat,lon+2)??"Unknown";
                DiseaseNode n = m_worldModel.NodeAt(x, y);
                toolStripStatusLabel1.Text = $"data[{x},{y}] is {where}, Lat {lat:f2}/Lon {lon:f2} {n:d4}";
                Console.WriteLine(toolStripStatusLabel1.Text);
            };

            toolStripStatusLabel1.TextChanged += (o, args) => Console.WriteLine(((ToolStripStatusLabel) o).Text);


        }

        private Func<DiseaseNode[,], double[], List<RouteData>, double> GetTotal(Func<DiseaseNode, double> selector)
        {
            return (nodes, doubles, arg3) => nodes.Cast<DiseaseNode>().Sum(selector);
        }

        private double GetTotal(DiseaseNode[,] arg1, double[] arg2, List<RouteData> arg3, Func<DiseaseNode,double> selector )
        {
            return arg1.Cast<DiseaseNode>().Sum(selector);
        }



        private ManualResetEvent m_manualResetEvent1 = new ManualResetEvent(true);
        private ManualResetEvent m_manualResetEvent2 = new ManualResetEvent(true);
        private bool stopped = true;
        private bool threadRunning = false;

        private void JustGoToolStripMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            if (stopped)
            {
                if (!threadRunning)
                {
                    ThreadPool.QueueUserWorkItem(
                        state => m_worldModel.Run(m_manualResetEvent1, m_manualResetEvent2));
                    threadRunning = true;
                }
                else
                {
                    m_manualResetEvent1.Set();
                    m_manualResetEvent2.Set();
                }
                justGoToolStripMenuItem.Text = "&Stop";
                stopped = false;
            }
            else
            {
                m_manualResetEvent1.Reset();
                justGoToolStripMenuItem.Text = "&Continuous";
                stopped = true;
            }
        }

        private void OneTimesliceToolStripMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            if (!threadRunning)
            {
                m_manualResetEvent2.Reset();
                ThreadPool.QueueUserWorkItem(
                    state => m_worldModel.Run(m_manualResetEvent1, m_manualResetEvent2));
                m_manualResetEvent1.Set();
                m_manualResetEvent1.Reset();
                m_manualResetEvent2.Set();
                threadRunning = true;
            }
            else
            {
                m_manualResetEvent2.Reset();
                m_manualResetEvent1.Set();
                m_manualResetEvent1.Reset();
                m_manualResetEvent2.Set();
            }
        }

        private void infectToolStripMenuItem_Click(object sender, EventArgs args)
        {
            if (!m_infectionMode)
            {
                infectToolStripMenuItem.Text = "Stop Infecting";
                m_mapControl.MouseClick += DoInfection;
            }
            else
            {
                m_mapControl.MouseClick -= DoInfection;
                infectToolStripMenuItem.Text = "Infect";
            }
            m_infectionMode = !m_infectionMode;

        }

        /// <summary>
        /// Does the infection.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void DoInfection(object sender, MouseEventArgs args)
        {
            double lat, lon;
            int x, y;

            GraphicalUtilities.GraphicsCoordsToMapCoords(m_mapControl.Size, m_worldModel.Size, args.X, args.Y, out x, out y);
            m_data.DataXYToLatLon(x, y, out lat, out lon);
            m_worldModel.InfectAt(x,y);
            toolStripStatusLabel1.Text =
                $"Infection introduced at Lat = {Math.Abs(lat):F0} degrees {(lat < 0 ? "North" : "South")}, Lon = {Math.Abs(lon):F0} degrees {(lon > 0 ? "East" : "West")}.";
        }

        private void ceaseFromToKnownInfectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_worldModel.NoAirTravelFromToKnownInfected = !m_worldModel.NoAirTravelFromToKnownInfected;
            ceaseFromToKnownInfectedToolStripMenuItem.Text = m_worldModel.NoAirTravelFromToKnownInfected
                ? "&Resume all air travel"
                : "&Cease From/To known infected areas";
        }

        private void haltEntirelyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_worldModel.NoAirTravelAtAll = !m_worldModel.NoAirTravelAtAll;
            haltEntirelyToolStripMenuItem.Text = m_worldModel.NoAirTravelAtAll
                ? "&Resume global air travel"
                : "&Halt Entirely";

        }
    }
}
