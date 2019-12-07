using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;


namespace Core.Graphical.Dockable_Content
{
    public enum MapMode
    {
        Informational,
        Infection
    }

    // TODO: WorldModel needs to be available from the top level.

    public partial class DockableMap : DockContent
    {
        public DockableMap()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document | DockAreas.Float;
            m_mapControl1.MouseMove += ReportVoxelData;
            m_mapControl1.MouseClick += SelectCountry;
        }

        public void AssignWorldModel(WorldModel worldModel)
        {
            m_mapControl1.AssignWorldModel(worldModel);
        }

        public void SetMapMode(MapMode mode)
        {
            switch (mode)
            {
                case MapMode.Informational:

                    m_mapControl1.MouseClick -= InfectAtVoxel;
                    m_mapControl1.Cursor = Cursors.Arrow;
                    break;
                case MapMode.Infection:
                    m_mapControl1.MouseClick += InfectAtVoxel;
                    m_mapControl1.Cursor = Cursors.Cross;
                    break;
                default:
                    break;
            }
        }

        private void SelectCountry(object sender, MouseEventArgs args)
        {
            int x, y;
            GraphicalUtilities.GraphicsCoordsToMapCoords(m_mapControl1.Size, m_mapControl1.MyWorldModel.Size, args.X,
                args.Y, out x,
                out y);
            DiseaseNode n = m_mapControl1.MyWorldModel.NodeAt(x, y);
            SimCountryData cd = m_mapControl1.MyWorldModel.CountryForCode(n.MapCell.CountryCode);
            if ( cd != null ) CountrySelected?.Invoke(cd);
        }

        public event Action<SimCountryData> CountrySelected;

        private void ReportVoxelData(object sender, MouseEventArgs args)
        {
            double lat, lon;
            int x, y;
            GraphicalUtilities.GraphicsCoordsToMapCoords(m_mapControl1.Size, m_mapControl1.MyWorldModel.Size, args.X,
                args.Y, out x,
                out y);
            m_mapControl1.MyWorldModel.MapData.DataXYToLatLon(x, y, out lat, out lon);
            
            DiseaseNode n = m_mapControl1.MyWorldModel.NodeAt(x, y);
            string where = n.MapCell.CountryCode;
            string text = $"data[{x},{y}] ({where}, Lat {lat:f2}/Lon {lon:f2} {n:d4}";

            ReportStatus(text);
        }

        private void InfectAtVoxel(object sender, MouseEventArgs args)
        {
            double lat, lon;
            int x, y;

            GraphicalUtilities.GraphicsCoordsToMapCoords(m_mapControl1.Size, m_mapControl1.MyWorldModel.Size, args.X,
                args.Y, out x, out y);
            m_mapControl1.MyWorldModel.MapData.DataXYToLatLon(x, y, out lat, out lon);
            m_mapControl1.MyWorldModel.InfectAt(x, y);
            m_mapControl1.MouseMove -= InfectAtVoxel;
            m_mapControl1.Cursor = Cursors.Default;
            DateTime when = m_mapControl1.MyWorldModel.Executive.Now;
            if (when == DateTime.MinValue) when = m_mapControl1.MyWorldModel.ExecutionParameters.StartTime;
            string countryName = ReverseGeocodeService.CountryNameForLatAndLong(lat, lon + 2) ?? "Unknown";
            string text =
                $"{when} : Infection introduced at Lat = {Math.Abs(lat):F0} degrees {(lat < 0 ? "North" : "South")}, Lon = {Math.Abs(lon):F0} degrees {(lon > 0 ? "East" : "West")}. ({countryName})";

            Console.WriteLine(text);

            SetMapMode(MapMode.Informational);
        }

        private void ReportStatus(string status)
        {
            Control me = this;
            while (me.Parent != null) me = me.Parent;
            ToolStrip ts = (ToolStrip)me.Controls["m_statusStrip"];
            ToolStripItem tsi = ts?.Items["m_informationalStatusText"];
            if (tsi != null)
            {
                tsi.Text = status;
            }
        }
    }
}
