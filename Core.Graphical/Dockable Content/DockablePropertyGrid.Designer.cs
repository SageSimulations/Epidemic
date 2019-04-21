using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace Core.Graphical.Dockable_Content
{
    partial class DockablePropertyGrid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockablePropertyGrid));
            this.m_propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // m_propertyGrid
            // 
            this.m_propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_propertyGrid.Location = new System.Drawing.Point(0, 3);
            this.m_propertyGrid.Name = "m_propertyGrid";
            this.m_propertyGrid.Size = new System.Drawing.Size(373, 326);
            this.m_propertyGrid.TabIndex = 0;
            // 
            // DockablePropertyGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 332);
            this.Controls.Add(this.m_propertyGrid);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DockablePropertyGrid";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.TabText = "DockablePropertyGrid";
            this.Text = "DockablePropertyGrid";
            this.ResumeLayout(false);

        }

        #endregion

        private PropertyGrid m_propertyGrid;
    }
}