using System;
using Core.Graphical;

namespace DiseaseView
{
    partial class InitialUI
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_mapControl = new Core.Graphical.MapControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.justGoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oneTimesliceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quarantineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.airTravelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ceaseFromToKnownInfectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.haltEntirelyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.foreignAidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_mapControl)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.m_mapControl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(898, 475);
            this.panel1.TabIndex = 0;
            // 
            // m_mapControl
            // 
            this.m_mapControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_mapControl.Location = new System.Drawing.Point(0, 0);
            this.m_mapControl.Name = "m_mapControl";
            this.m_mapControl.Size = new System.Drawing.Size(896, 473);
            this.m_mapControl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_mapControl.TabIndex = 0;
            this.m_mapControl.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem,
            this.infectToolStripMenuItem,
            this.quarantineToolStripMenuItem,
            this.airTravelToolStripMenuItem,
            this.foreignAidToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(898, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.justGoToolStripMenuItem,
            this.oneTimesliceToolStripMenuItem});
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.runToolStripMenuItem.Text = "&Run";
            // 
            // justGoToolStripMenuItem
            // 
            this.justGoToolStripMenuItem.Name = "justGoToolStripMenuItem";
            this.justGoToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.justGoToolStripMenuItem.Text = "&Continuous";
            this.justGoToolStripMenuItem.Click += new System.EventHandler(this.JustGoToolStripMenuItemOnClick);
            // 
            // oneTimesliceToolStripMenuItem
            // 
            this.oneTimesliceToolStripMenuItem.Name = "oneTimesliceToolStripMenuItem";
            this.oneTimesliceToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.oneTimesliceToolStripMenuItem.Text = "&One Timeslice";
            this.oneTimesliceToolStripMenuItem.Click += new System.EventHandler(this.OneTimesliceToolStripMenuItemOnClick);
            // 
            // infectToolStripMenuItem
            // 
            this.infectToolStripMenuItem.Name = "infectToolStripMenuItem";
            this.infectToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.infectToolStripMenuItem.Text = "Infect";
            this.infectToolStripMenuItem.Click += new System.EventHandler(this.infectToolStripMenuItem_Click);
            // 
            // quarantineToolStripMenuItem
            // 
            this.quarantineToolStripMenuItem.Name = "quarantineToolStripMenuItem";
            this.quarantineToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.quarantineToolStripMenuItem.Text = "&Quarantine";
            // 
            // airTravelToolStripMenuItem
            // 
            this.airTravelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ceaseFromToKnownInfectedToolStripMenuItem,
            this.haltEntirelyToolStripMenuItem});
            this.airTravelToolStripMenuItem.Name = "airTravelToolStripMenuItem";
            this.airTravelToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.airTravelToolStripMenuItem.Text = "&Air Travel";
            // 
            // ceaseFromToKnownInfectedToolStripMenuItem
            // 
            this.ceaseFromToKnownInfectedToolStripMenuItem.Name = "ceaseFromToKnownInfectedToolStripMenuItem";
            this.ceaseFromToKnownInfectedToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.ceaseFromToKnownInfectedToolStripMenuItem.Text = "&Cease From/To Known Infected";
            this.ceaseFromToKnownInfectedToolStripMenuItem.Click += new System.EventHandler(this.ceaseFromToKnownInfectedToolStripMenuItem_Click);
            // 
            // haltEntirelyToolStripMenuItem
            // 
            this.haltEntirelyToolStripMenuItem.Name = "haltEntirelyToolStripMenuItem";
            this.haltEntirelyToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.haltEntirelyToolStripMenuItem.Text = "&Halt Entirely";
            this.haltEntirelyToolStripMenuItem.Click += new System.EventHandler(this.haltEntirelyToolStripMenuItem_Click);
            // 
            // foreignAidToolStripMenuItem
            // 
            this.foreignAidToolStripMenuItem.Name = "foreignAidToolStripMenuItem";
            this.foreignAidToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.foreignAidToolStripMenuItem.Text = "Foreign Aid";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(898, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.menuStrip1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(898, 499);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(898, 521);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            this.toolStripContainer1.TopToolStripPanelVisible = false;
            // 
            // InitialUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 521);
            this.Controls.Add(this.toolStripContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "InitialUI";
            this.Text = "Global Pandemic";
            this.Load += new System.EventHandler(this.InitialUI_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_mapControl)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private MapControl m_mapControl;
        private System.Windows.Forms.ToolStripMenuItem infectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quarantineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem foreignAidToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oneTimesliceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem justGoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem airTravelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ceaseFromToKnownInfectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem haltEntirelyToolStripMenuItem;
    }
}

