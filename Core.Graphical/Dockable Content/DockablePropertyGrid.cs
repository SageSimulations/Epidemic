using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Core.Graphical.Dockable_Content
{
    public partial class DockablePropertyGrid : DockContent
    {
        public DockablePropertyGrid()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.DockBottom | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop |
                        DockAreas.Float;
        }

        public PropertySort PropertySort => m_propertyGrid.PropertySort;

        //public bool ToolbarVisible => m_propertyGrid.ToolbarVisible;

        //public PropertySort PropertySort
        //{
        //    get { return m_propertyGrid.PropertySort; }
        //    set { m_propertyGrid.PropertySort = value; }
        //}
        public bool ToolbarVisible
        {
            get { return m_propertyGrid.ToolbarVisible; }
            set { m_propertyGrid.ToolbarVisible = value; }
        }

        public object SelectedObject
        {
            get { return m_propertyGrid.SelectedObject; }
            set { m_propertyGrid.SelectedObject = value; }
        }
    }
}
