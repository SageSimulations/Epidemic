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
            this.m_textBox = textbox;
            
        }

        public override void Write(char value)
        {
            this.m_textBox.InvokeIfRequired<TextBox>(delegate(TextBox tb) { tb.Text += value; });
        }

        public override void Write(string value)
        {
            m_textBox.Text += value;
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }
}
