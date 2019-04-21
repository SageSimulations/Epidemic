using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace Core.Graphical.Dockable_Content
{
    partial class DockableConsole
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockableConsole));
            this.m_textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // m_textBox
            // 
            this.m_textBox.BackColor = System.Drawing.SystemColors.Window;
            this.m_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_textBox.Location = new System.Drawing.Point(0, 3);
            this.m_textBox.Multiline = true;
            this.m_textBox.Name = "m_textBox";
            this.m_textBox.ReadOnly = true;
            this.m_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.m_textBox.Size = new System.Drawing.Size(392, 184);
            this.m_textBox.TabIndex = 0;
            this.m_textBox.TextChanged += new System.EventHandler(this.m_textBox_TextChanged);
            // 
            // DockableConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 190);
            this.Controls.Add(this.m_textBox);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DockableConsole";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.TabText = "DockableConsole";
            this.Text = "DockableConsole";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox m_textBox;
    }
}