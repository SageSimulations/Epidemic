using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Core.Graphical.Dockable_Content
{
    public partial class DockableConsole : DockContent, ILineWriter
    {
        public DockableConsole(string title = null)
        {
            InitializeComponent();
            if (title != null) TabText = title;
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.DockBottom | DockAreas.DockTop | DockAreas.Float;
            m_textBox.DoubleBuffering(true);
        }

        public void WriteLine(string s)
        {
            m_textBox.InvokeIfRequired<TextBox>(delegate (TextBox tb) { tb.Text += (s + Environment.NewLine); });
        }

        private void m_textBox_TextChanged(object sender, EventArgs e)
        {
            m_textBox.SelectionStart = m_textBox.Text.Length;
            m_textBox.ScrollToCaret();
        }
    }
}
