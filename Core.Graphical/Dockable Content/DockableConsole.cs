using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Core.Graphical.Dockable_Content
{
    public partial class DockableConsole : DockContent
    {
        public DockableConsole(string title = null)
        {
            InitializeComponent();
            if (title != null) TabText = title;
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.DockBottom | DockAreas.DockTop | DockAreas.Float;
            Console.SetOut(new ControlWriter(m_textBox));
            m_textBox.DoubleBuffering(true);
        }

        private void m_textBox_TextChanged(object sender, EventArgs e)
        {
            m_textBox.SelectionStart = m_textBox.Text.Length;
            m_textBox.ScrollToCaret();
        }
    }
}
