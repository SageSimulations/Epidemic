namespace Core.Graphical.Dockable_Content
{
    partial class DockableRegionPieChart
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockableTrendChart));
            this.m_plotView = new OxyPlot.WindowsForms.PlotView();
            this.SuspendLayout();
            // 
            // m_plotView
            // 
            this.m_plotView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_plotView.Location = new System.Drawing.Point(0, 0);
            this.m_plotView.Name = "m_plotView";
            this.m_plotView.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.m_plotView.Size = new System.Drawing.Size(339, 253);
            this.m_plotView.TabIndex = 0;
            this.m_plotView.Text = "plotView1";
            this.m_plotView.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.m_plotView.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.m_plotView.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // DockableChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 253);
            this.Controls.Add(this.m_plotView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DockableChart";
            this.ResumeLayout(false);

        }

        #endregion

        private OxyPlot.WindowsForms.PlotView m_plotView;
    }
}
