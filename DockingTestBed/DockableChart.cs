using OxyPlot;
using OxyPlot.Axes;

namespace Core.Graphical.Dockable_Content
{
    public partial class DockableChart : WeifenLuo.WinFormsUI.Docking.DockContent
    {

        public DockableChart(string chartTitle) : base()
        {
            ToolTipText = chartTitle;

            InitializeComponent();

            m_plotView.Model = new PlotModel()
            {
                Title = chartTitle,
                Axes =
                {
                    new LinearAxis() { Position = AxisPosition.Left },
                    new DateTimeAxis() { Position = AxisPosition.Bottom }
                },
            };
        }
    }
}
