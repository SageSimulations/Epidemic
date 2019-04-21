using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Core;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace InitialGraphical
{
    public partial class PlotForm : Form
    {
        private Func<DiseaseNode[,], double[], List<RouteData>, double>[] m_plotTargets;
        private WorldModel m_model;
        //private PlotModel m_plotModel;
        //private Axis m_value;
        //private Axis m_dateTime;
        //private Series m_series;

        public PlotForm(string title) : base()
        {
            //m_value = new LinearAxis() { Position = AxisPosition.Left };
            //m_dateTime = new DateTimeAxis() { Position = AxisPosition.Bottom };
            //m_series = new LineSeries() { Title = title };

            //m_plotModel = new PlotModel()
            //{
            //    Title = title,
            //    Axes = { m_value, m_dateTime },
            //};
            InitializeComponent();

            m_plotView.Model = new PlotModel()
            {
                Title = title,
                Axes = { new LinearAxis() { Position = AxisPosition.Left }, new DateTimeAxis() { Position = AxisPosition.Bottom } },
            };

        }

        public void Bind(WorldModel model, Func<DiseaseNode[,], double[], List<RouteData>, double> plotTarget)
        {
            m_model = model;
            m_model.NewIterationAvailable += ModelOnNewIterationAvailable;
            m_plotTargets = new [] { plotTarget};
        }

        public void Bind(WorldModel model, Func<DiseaseNode[,], double[], List<RouteData>, double>[] plotTargets, string[] titles)
        {
            for (int i = 0; i < plotTargets.Length; i++)
            {
                m_plotView.Model.Series.Add(new LineSeries() { Title = titles[i] });
            }
            m_model = model;
            m_model.NewIterationAvailable += ModelOnNewIterationAvailable;
            m_plotTargets = plotTargets;
        }



        private void ModelOnNewIterationAvailable(DiseaseNode[,] diseaseNodes, double[] doubles, List<RouteData> routes)
        {
            for (int i = 0; i < m_plotView.Model.Series.Count; i++)
            {
                ((LineSeries)m_plotView.Model.Series[i]).Points.Add(
                    new DataPoint(diseaseNodes[0, 0].TimeSliceNdx * diseaseNodes[0, 0].TimeStep,
                        m_plotTargets[i](diseaseNodes, doubles, routes)));
            }
            m_plotView.Model.InvalidatePlot(true);
        }


    }
}
