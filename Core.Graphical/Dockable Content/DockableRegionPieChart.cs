using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Series;

namespace Core.Graphical.Dockable_Content
{
    public partial class DockableRegionPieChart : WeifenLuo.WinFormsUI.Docking.DockContent
    {
#pragma warning disable IDE0044 // Add readonly modifier
        private Func<DiseaseNode[,], double[], List<RouteData>, double>[] m_plotTargets;
#pragma warning restore IDE0044 // Add readonly modifier
        private WorldModel m_model;
        private readonly PlotModel m_plotModel;
        private List<Tuple<int, int>> m_cellsOfInterest; 

        public DockableRegionPieChart(string chartTitle) : base()
        {
            ToolTipText = chartTitle;

            m_plotModel = new PlotModel(){Title = chartTitle};

            InitializeComponent();
            m_plotView.Model = m_plotModel;

        }

        private void ModelOnNewIterationAvailable(DiseaseNode[,] diseaseNodes, double[] doubles, List<RouteData> routeData, List<OutbreakResponseTeam> allOrts)
        {
            double dead = 0, killed = 0, susceptible = 0, immune = 0, recoveredImmune = 0, contagiousSymptomatic = 0, contagiousAsymptomatic = 0;
            foreach (Tuple<int, int> tuple in m_cellsOfInterest)
            {
                dead += diseaseNodes[tuple.Item1, tuple.Item2].Dead;
                killed += diseaseNodes[tuple.Item1, tuple.Item2].Killed;
                susceptible += diseaseNodes[tuple.Item1, tuple.Item2].Susceptible;
                immune += diseaseNodes[tuple.Item1, tuple.Item2].Immune;
                recoveredImmune += diseaseNodes[tuple.Item1, tuple.Item2].RecoveredImmune;
                contagiousSymptomatic += diseaseNodes[tuple.Item1, tuple.Item2].ContagiousSymptomatic;
                contagiousAsymptomatic += diseaseNodes[tuple.Item1, tuple.Item2].ContagiousAsymptomatic;
            }

            PieSeries seriesP1 = new PieSeries
            {
                StrokeThickness = 1.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0
            };

            seriesP1.Slices.Add(new PieSlice("Dead", dead) {IsExploded = false, Fill = OxyColors.Black});
            seriesP1.Slices.Add(new PieSlice("Killed", killed) { IsExploded = false, Fill = OxyColors.Gray });
            seriesP1.Slices.Add(new PieSlice("Susceptible", susceptible) { IsExploded = false, Fill = OxyColors.Yellow });
            seriesP1.Slices.Add(new PieSlice("Immune", immune) { IsExploded = false, Fill = OxyColors.Green });
            seriesP1.Slices.Add(new PieSlice("Recovered", recoveredImmune) { IsExploded = false, Fill = OxyColors.DarkGreen });
            seriesP1.Slices.Add(new PieSlice("Symptomatic", contagiousSymptomatic) { IsExploded = false, Fill = OxyColors.Orange });
            seriesP1.Slices.Add(new PieSlice("Asymptomatic", contagiousAsymptomatic) { IsExploded = false, Fill = OxyColors.Red });
            m_plotModel.Series.Clear();
            m_plotModel.Series.Add(seriesP1);
            m_plotModel.InvalidatePlot(true);

        }


        public void Bind(WorldModel worldModel, string countryCode)
        {
            m_cellsOfInterest = new List<Tuple<int, int>>();
            for (int x = 0; x < worldModel.MapData.Width; x++)
            {
                for (int y = 0; y < worldModel.MapData.Height; y++)
                {
                    CellData cd = worldModel.MapData.CellData[x, y];
                    if ( countryCode.Equals(cd.CountryCode)) m_cellsOfInterest.Add(new Tuple<int,int>(x,y));
                }
            }
            m_model = worldModel;
            m_model.NewIterationAvailable += ModelOnNewIterationAvailable;
        }
    }
}
