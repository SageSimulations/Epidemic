using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace Core.Graphical
{
    public partial class PlotControl : PlotView
    {
        private Func<DiseaseNode[,], double[], List<RouteData>, double> m_plotTarget;
        private WorldModel m_model;
        private Axis m_value;
        private Axis m_dateTime;
        private PlotModel m_plotModel;
        private Series m_series;

        public PlotControl(string title) : base()
        {
            m_value = new LinearAxis() { Position = AxisPosition.Left };
            m_dateTime = new DateTimeAxis() {Position = AxisPosition.Bottom };
            m_series = new LineSeries() {Title = title};

            m_plotModel = new PlotModel()
            {
                Title = title,
                Axes = { m_value, m_dateTime },
                Series = { m_series }

            };
        }

        public void Bind(WorldModel model, Func<DiseaseNode[,], double[], List<RouteData>, double> plotTarget)
        {
            m_model = model;
            m_model.NewIterationAvailable +=ModelOnNewIterationAvailable;
            m_plotTarget = plotTarget;
        }

        private void ModelOnNewIterationAvailable(DiseaseNode[,] diseaseNodes, double[] doubles, List<RouteData> arg3)
        {
            ((LineSeries)m_series).Points.Add(new DataPoint(diseaseNodes[0, 0].TimeSliceNdx * diseaseNodes[0, 0].TimeStep, m_plotTarget(diseaseNodes, doubles, arg3)));
            m_plotModel.InvalidatePlot(true);
        }
    }
}
