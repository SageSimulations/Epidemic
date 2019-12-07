using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Core;
using Core.Graphical;
using Core.Graphical.Dockable_Content;
using Highpoint.Sage.SimCore;
using Newtonsoft.Json.Linq;
using WeifenLuo.WinFormsUI.Docking;

namespace Epidemic
{
    public partial class Epidemic : Form
    {
        private ILineWriter m_console;
        private Image m_imgRun;
        private Image m_imgPause;


        //private SplashScreen _splashScreen;
        //private bool _showSplash = true;
        private DockableMap m_map;

        public WorldModel WorldModel { get; private set; }

        public Epidemic()
        {

            InitializeComponent();
            m_imgRun = Properties.Resources.Run;
            m_imgPause = Properties.Resources.Pause;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            //SetSplashScreen();
            LoadNewDefaultModelAndExtraStuff();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadNewDefaultModelAndExtraStuff();
        }

        private void LoadNewDefaultModelAndExtraStuff()
        {
            m_console = new DockableConsole("Log");
            Locator.Register(() => m_console, "console");

            List<OutbreakResponseTeam> orts = new List<OutbreakResponseTeam>();

            Dictionary<string, Tuple<int, Color>> ortCounts = new Dictionary<string, Tuple<int,Color>>();
            string json = File.ReadAllText(@"Data/Governments.dat");
            dynamic d = JObject.Parse(json);
            foreach (dynamic govt in d["Governments"])
            {
                string cc = govt["cc"];
                int numberOfORTs = govt["ORTs"];
                Color color = Color.FromName(govt["Color"].ToString());
                ortCounts.Add(cc,new Tuple<int, Color> (numberOfORTs, color));
            }

            WorldModel = new WorldModel(MapData.LoadFrom("Data/MapData.dat"), SimCountryData.LoadFrom("Data/CountryData.dat"), ortCounts);
            WorldModel.Executive.ClockAboutToChange += exec =>
            {
                DateTime newTime = ((IExecEvent) exec.EventList[0]).When;
                double howMuch = 100 * (newTime - WorldModel.ExecutionParameters.StartTime).TotalHours/
                (WorldModel.ExecutionParameters.EndTime - WorldModel.ExecutionParameters.StartTime).TotalHours;

                m_statusStrip.InvokeIfRequired(strip =>
                {
                    ((ToolStripProgressBar) strip.Items["Progress"]).Value = (int) howMuch;
                    ((ToolStripStatusLabel) strip.Items["DateTimeStatus"]).Text = newTime.ToString("dd MMM yyy HH:mm:ss");
                });
            };
            WorldModel.Controller.StateChanged += HandleStateChange;

            m_map = new DockableMap();
            m_map.AssignWorldModel(WorldModel);
            DockableTrendChart dc1 = new DockableTrendChart("Mortality");
            dc1.Bind(WorldModel,
                new[]
                {
                    GetTotal(n => n.Killed),
                    GetTotal(n => n.Dead)
                },
                new[]
                {
                    "Killed",
                    "Dead"
                });

            DockableTrendChart dc2 = new DockableTrendChart("Disease Stages");
            dc2.Bind(WorldModel,
                new[]
                {
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

            DockableRegionPieChart drpc1 = new DockableRegionPieChart("Nepal");
            drpc1.Bind(WorldModel, "NPL");
            drpc1.Show(m_dockPanel, DockState.DockLeft);

            DockableRegionPieChart drpc2 = new DockableRegionPieChart("Ireland");
            drpc2.Bind(WorldModel, "IRL");
            drpc2.Show(m_dockPanel, DockState.DockLeft);

            DockablePropertyGrid dpg = new DockablePropertyGrid()
            {
                SelectedObject = WorldModel.ExecutionParameters,
                TabText = "Simulation Timing"
            };
            dpg.Show(m_dockPanel, DockState.DockLeft);

            DockablePropertyGrid country = new DockablePropertyGrid()
            {
                SelectedObject = WorldModel.CountryForCode("USA"),
                TabText = "Selected Country",
                ToolTipText = "Selected Country",
            };
            country.ToolbarVisible = false;
            m_map.CountrySelected += data => country.SelectedObject = data;

            m_map.MdiParent = this;
            m_map.Show(m_dockPanel, DockState.Document);
            ((DockableConsole)m_console).Show(m_dockPanel, DockState.DockBottom);
            country.Show(dpg.Pane, DockAlignment.Bottom,0.6);
            dc1.Show(m_dockPanel, DockState.DockRight);
            dc2.Show(dc1.Pane, DockAlignment.Bottom, 0.5);


        }

        private Func<DiseaseNode[,], double[], List<RouteData>, double> GetTotal(Func<DiseaseNode, double> selector)
        {
            return (nodes, doubles, arg3) => nodes.Cast<DiseaseNode>().Sum(selector);
        }

        private Func<DiseaseNode[,], double[], List<RouteData>, double> ForCountry(Func<DiseaseNode, double> selector)
        {
            return (nodes, doubles, arg3) => nodes.Cast<DiseaseNode>().Sum(selector);
        }

        private void m_btnReset_Click(object sender, EventArgs e)
        {
            WorldModel.Controller.Reset();
        }
        private void m_btnRunPause_Click(object sender, EventArgs e)
        {
            WorldModel.Controller.PlayPause();
        }
        private void m_btnStop_Click(object sender, EventArgs e)
        {
            WorldModel.Controller.Stop();
        }

        private void HandleStateChange(Controller.State obj)
        {
            switch (obj)
            {
                case Controller.State.Stopped:
                    m_btnRunPause.Enabled = false;
                    m_btnRunPause.Image = m_imgRun;
                    m_btnReset.Enabled = true;
                    m_btnStop.Enabled = false;
                    break;
                case Controller.State.Paused:
                    m_btnRunPause.Enabled = true;
                    m_btnRunPause.Image = m_imgRun;
                    m_btnReset.Enabled = true;
                    m_btnStop.Enabled = true;
                    break;
                case Controller.State.Ready:
                    ((ProgressBar)m_statusStrip.Controls["Progress"]).Value = 0;
                    m_btnRunPause.Enabled = true;
                    m_btnRunPause.Image = m_imgRun;
                    m_btnReset.Enabled = true;
                    m_btnStop.Enabled = false;
                    break;
                case Controller.State.Running:
                    m_btnRunPause.Enabled = true;
                    m_btnRunPause.Image = m_imgPause;
                    m_btnReset.Enabled = true;
                    m_btnStop.Enabled = true;
                    break;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            try
            {
                WorldModel.Executive.Abort();
            }
            catch (ThreadAbortException tae)
            {
                Thread.ResetAbort(); // TODO: Ugly AF.
            }
        }

        private void m_btnInfect_Click(object sender, EventArgs e)
        {
            m_map.SetMapMode(MapMode.Infection);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_map.SetMapMode(MapMode.Infection);
        }

        private void m_btnFlightPolicy_Click(object sender, EventArgs e)
        {
            switch (WorldModel.GlobalAirTravelPolicy.Restrictions)
            {
                case AirTravelPolicy._Restrictions.Unrestricted:
                    SetFlightMode(AirTravelPolicy._Restrictions.OnlyCleanToClean);
                    break;
                case AirTravelPolicy._Restrictions.OnlyCleanToClean:
                    SetFlightMode(AirTravelPolicy._Restrictions.Grounded);
                    break;
                case AirTravelPolicy._Restrictions.Grounded:
                    SetFlightMode(AirTravelPolicy._Restrictions.Unrestricted);
                    break;
                default:
                    break;
            }
        }

        private void SetFlightMode(AirTravelPolicy._Restrictions newRestriction)
        {
            WorldModel.GlobalAirTravelPolicy.Restrictions = newRestriction;
            m_btnFlightPolicy.ToolTipText = WorldModel.GlobalAirTravelPolicy.RestrictionText;
            switch (newRestriction)
            {
                case AirTravelPolicy._Restrictions.Unrestricted:
                    m_btnFlightPolicy.Image = Properties.Resources.AllFlights;
                    WorldModel.NoAirTravelAtAll = false;
                    WorldModel.NoAirTravelFromToKnownInfected = false;
                    break;
                case AirTravelPolicy._Restrictions.OnlyCleanToClean:
                    m_btnFlightPolicy.Image = Properties.Resources.SomeFlights;
                    WorldModel.NoAirTravelAtAll = false;
                    WorldModel.NoAirTravelFromToKnownInfected = true;
                    break;
                case AirTravelPolicy._Restrictions.Grounded:
                    m_btnFlightPolicy.Image = Properties.Resources.NoFlights;
                    WorldModel.NoAirTravelAtAll = true;
                    WorldModel.NoAirTravelFromToKnownInfected = true;
                    break;
                default:
                    break;
            }
        }

        private void permitAllFlightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetFlightMode(AirTravelPolicy._Restrictions.Unrestricted);
        }

        private void ceaseFromtoKnownInfectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetFlightMode(AirTravelPolicy._Restrictions.OnlyCleanToClean);
        }

        private void ceaseEntirelyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetFlightMode(AirTravelPolicy._Restrictions.Grounded);
        }
    }
}
