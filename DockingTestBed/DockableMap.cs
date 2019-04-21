using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Core.Graphical.Dockable_Content
{
    public partial class DockableMap : DockContent
    {
        public DockableMap()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document | DockAreas.Float;
        }
    }
}
