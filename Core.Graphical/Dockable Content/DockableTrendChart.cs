using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Core.Graphical.Dockable_Content
{
    public partial class DockableTrendChart : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private Func<DiseaseNode[,], double[], List<RouteData>, double>[] m_plotTargets;
        private WorldModel m_model;
        private readonly PlotModel m_plotModel;

        public DockableTrendChart(string chartTitle) : base()
        {
            ToolTipText = chartTitle;

            m_plotModel = new PlotModel()
            {
                Title = chartTitle,
                Axes =
                {
                    new LinearAxis() { Position = AxisPosition.Left },
                    new DateTimeAxis() { Position = AxisPosition.Bottom }
                },
            };

            InitializeComponent();
            m_plotView.Model = m_plotModel;
        }

        public void Bind(WorldModel model, Func<DiseaseNode[,], double[], List<RouteData>, double>[] plotTargets, string[] titles)
        {
            for (int i = 0; i < plotTargets.Length; i++)
            {
                m_plotModel.Series.Add(new LineSeries(){ Title=titles[i]});
            }
            m_model = model;
            m_model.NewIterationAvailable += ModelOnNewIterationAvailable;
            m_plotTargets = plotTargets;
        }

        private void ModelOnNewIterationAvailable(DiseaseNode[,] diseaseNodes, double[] doubles, List<RouteData> routeData, List<OutbreakResponseTeam> allOrts )
        {
            for ( int i = 0 ; i < m_plotModel.Series.Count; i++) {
                ((LineSeries)m_plotModel.Series[i]).Points.Add(
                    new DataPoint(m_model.Executive.Now.ToOADate(), //diseaseNodes[0, 0].TimeSliceNdx*diseaseNodes[0, 0].TimeStep,
                        m_plotTargets[i](diseaseNodes, doubles, routeData)));
            }
            m_plotModel.InvalidatePlot(true);
        }
    }
}
