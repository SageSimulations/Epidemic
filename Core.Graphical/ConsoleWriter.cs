using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Core.Graphical
{
    public class ControlWriter : TextWriter
    {
        private TextBox m_textBox;
        public ControlWriter(TextBox textbox)
        {
            m_textBox = textbox;           
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }

        public override void WriteLine(string value)
        {
            m_textBox.InvokeIfRequired<TextBox>(delegate (TextBox tb) { tb.Text += (value+Environment.NewLine); });
        }
    }
}
