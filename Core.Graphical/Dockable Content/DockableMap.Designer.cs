using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace Core.Graphical.Dockable_Content
{
    partial class DockableMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockableMap));
            this.m_mapControl1 = new Core.Graphical.MapControl();
            ((System.ComponentModel.ISupportInitialize)(this.m_mapControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // m_mapControl1
            // 
            this.m_mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_mapControl1.Location = new System.Drawing.Point(0, 3);
            this.m_mapControl1.Name = "m_mapControl1";
            this.m_mapControl1.Size = new System.Drawing.Size(392, 184);
            this.m_mapControl1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_mapControl1.TabIndex = 0;
            this.m_mapControl1.TabStop = false;
            // 
            // DockableMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 190);
            this.Controls.Add(this.m_mapControl1);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DockableMap";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.TabText = "DockableMap";
            this.Text = "DockableMap";
            ((System.ComponentModel.ISupportInitialize)(this.m_mapControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MapControl m_mapControl1;
    }
}