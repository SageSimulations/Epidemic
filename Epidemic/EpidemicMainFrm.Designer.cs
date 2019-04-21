using Core;

namespace Epidemic
{
    partial class Epidemic
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Epidemic));
            this.m_statusStrip = new System.Windows.Forms.StatusStrip();
            this.DateTimeStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.Progress = new System.Windows.Forms.ToolStripProgressBar();
            this.m_informationalStatusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.infectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transportationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.airTravelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.permitAllFlightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ceaseForKnownInfectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ceaseEntirelyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStrip = new System.Windows.Forms.ToolStrip();
            this.m_btnReset = new System.Windows.Forms.ToolStripButton();
            this.m_btnRunPause = new System.Windows.Forms.ToolStripButton();
            this.m_btnStop = new System.Windows.Forms.ToolStripButton();
            this.m_btnInfect = new System.Windows.Forms.ToolStripButton();
            this.m_btnFlightPolicy = new System.Windows.Forms.ToolStripButton();
            this.m_dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.m_statusStrip.SuspendLayout();
            this.m_menuStrip.SuspendLayout();
            this.m_toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_statusStrip
            // 
            this.m_statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DateTimeStatus,
            this.Progress,
            this.m_informationalStatusText});
            this.m_statusStrip.Location = new System.Drawing.Point(0, 532);
            this.m_statusStrip.Name = "m_statusStrip";
            this.m_statusStrip.Size = new System.Drawing.Size(994, 22);
            this.m_statusStrip.TabIndex = 2;
            this.m_statusStrip.Text = "statusStrip1";
            // 
            // DateTimeStatus
            // 
            this.DateTimeStatus.AutoSize = false;
            this.DateTimeStatus.Name = "DateTimeStatus";
            this.DateTimeStatus.Size = new System.Drawing.Size(150, 17);
            this.DateTimeStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DateTimeStatus.ToolTipText = "Model DateTime";
            // 
            // Progress
            // 
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(100, 16);
            this.Progress.ToolTipText = "Simulation Progress";
            // 
            // m_informationalStatusText
            // 
            this.m_informationalStatusText.AccessibleName = "Progress";
            this.m_informationalStatusText.ActiveLinkColor = System.Drawing.Color.Red;
            this.m_informationalStatusText.AutoSize = false;
            this.m_informationalStatusText.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_informationalStatusText.Name = "m_informationalStatusText";
            this.m_informationalStatusText.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.m_informationalStatusText.Size = new System.Drawing.Size(727, 17);
            this.m_informationalStatusText.Spring = true;
            this.m_informationalStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_informationalStatusText.ToolTipText = "General information.";
            // 
            // m_menuStrip
            // 
            this.m_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.infectionToolStripMenuItem,
            this.transportationToolStripMenuItem});
            this.m_menuStrip.Location = new System.Drawing.Point(0, 0);
            this.m_menuStrip.Name = "m_menuStrip";
            this.m_menuStrip.Size = new System.Drawing.Size(994, 24);
            this.m_menuStrip.TabIndex = 3;
            this.m_menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.newToolStripMenuItem1});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem1
            // 
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            this.newToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.newToolStripMenuItem1.Text = "New...";
            // 
            // infectionToolStripMenuItem
            // 
            this.infectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem});
            this.infectionToolStripMenuItem.Name = "infectionToolStripMenuItem";
            this.infectionToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.infectionToolStripMenuItem.Text = "Infection";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.addToolStripMenuItem.Text = "Add Infection";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // transportationToolStripMenuItem
            // 
            this.transportationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.airTravelToolStripMenuItem});
            this.transportationToolStripMenuItem.Name = "transportationToolStripMenuItem";
            this.transportationToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.transportationToolStripMenuItem.Text = "Transportation";
            // 
            // airTravelToolStripMenuItem
            // 
            this.airTravelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.permitAllFlightsToolStripMenuItem,
            this.ceaseForKnownInfectedToolStripMenuItem,
            this.ceaseEntirelyToolStripMenuItem});
            this.airTravelToolStripMenuItem.Name = "airTravelToolStripMenuItem";
            this.airTravelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.airTravelToolStripMenuItem.Text = "Air Travel";
            // 
            // permitAllFlightsToolStripMenuItem
            // 
            this.permitAllFlightsToolStripMenuItem.Name = "permitAllFlightsToolStripMenuItem";
            this.permitAllFlightsToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.permitAllFlightsToolStripMenuItem.Text = "Unrestricted";
            this.permitAllFlightsToolStripMenuItem.Click += new System.EventHandler(this.permitAllFlightsToolStripMenuItem_Click);
            // 
            // ceaseForKnownInfectedToolStripMenuItem
            // 
            this.ceaseForKnownInfectedToolStripMenuItem.Name = "ceaseForKnownInfectedToolStripMenuItem";
            this.ceaseForKnownInfectedToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.ceaseForKnownInfectedToolStripMenuItem.Text = "Cease for known infected";
            this.ceaseForKnownInfectedToolStripMenuItem.Click += new System.EventHandler(this.ceaseFromtoKnownInfectedToolStripMenuItem_Click);
            // 
            // ceaseEntirelyToolStripMenuItem
            // 
            this.ceaseEntirelyToolStripMenuItem.Name = "ceaseEntirelyToolStripMenuItem";
            this.ceaseEntirelyToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.ceaseEntirelyToolStripMenuItem.Text = "Cease entirely";
            this.ceaseEntirelyToolStripMenuItem.Click += new System.EventHandler(this.ceaseEntirelyToolStripMenuItem_Click);
            // 
            // m_toolStrip
            // 
            this.m_toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_btnReset,
            this.m_btnRunPause,
            this.m_btnStop,
            this.m_btnInfect,
            this.m_btnFlightPolicy});
            this.m_toolStrip.Location = new System.Drawing.Point(0, 24);
            this.m_toolStrip.Name = "m_toolStrip";
            this.m_toolStrip.Size = new System.Drawing.Size(994, 25);
            this.m_toolStrip.TabIndex = 4;
            this.m_toolStrip.Text = "toolStrip1";
            // 
            // m_btnReset
            // 
            this.m_btnReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_btnReset.Enabled = false;
            this.m_btnReset.Image = ((System.Drawing.Image)(resources.GetObject("m_btnReset.Image")));
            this.m_btnReset.ImageTransparentColor = System.Drawing.Color.White;
            this.m_btnReset.Margin = new System.Windows.Forms.Padding(1);
            this.m_btnReset.Name = "m_btnReset";
            this.m_btnReset.Size = new System.Drawing.Size(23, 23);
            this.m_btnReset.Text = "toolStripButton2";
            this.m_btnReset.Click += new System.EventHandler(this.m_btnReset_Click);
            // 
            // m_btnRunPause
            // 
            this.m_btnRunPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_btnRunPause.Image = ((System.Drawing.Image)(resources.GetObject("m_btnRunPause.Image")));
            this.m_btnRunPause.ImageTransparentColor = System.Drawing.Color.White;
            this.m_btnRunPause.Margin = new System.Windows.Forms.Padding(1);
            this.m_btnRunPause.Name = "m_btnRunPause";
            this.m_btnRunPause.Size = new System.Drawing.Size(23, 23);
            this.m_btnRunPause.Text = "Play";
            this.m_btnRunPause.Click += new System.EventHandler(this.m_btnRunPause_Click);
            // 
            // m_btnStop
            // 
            this.m_btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_btnStop.Enabled = false;
            this.m_btnStop.Image = ((System.Drawing.Image)(resources.GetObject("m_btnStop.Image")));
            this.m_btnStop.ImageTransparentColor = System.Drawing.Color.White;
            this.m_btnStop.Name = "m_btnStop";
            this.m_btnStop.Size = new System.Drawing.Size(23, 22);
            this.m_btnStop.Text = "Stop";
            this.m_btnStop.Click += new System.EventHandler(this.m_btnStop_Click);
            // 
            // m_btnInfect
            // 
            this.m_btnInfect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_btnInfect.Image = ((System.Drawing.Image)(resources.GetObject("m_btnInfect.Image")));
            this.m_btnInfect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_btnInfect.Name = "m_btnInfect";
            this.m_btnInfect.Size = new System.Drawing.Size(23, 22);
            this.m_btnInfect.Text = "Infect";
            this.m_btnInfect.Click += new System.EventHandler(this.m_btnInfect_Click);
            // 
            // m_btnFlightPolicy
            // 
            this.m_btnFlightPolicy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_btnFlightPolicy.Image = ((System.Drawing.Image)(resources.GetObject("m_btnFlightPolicy.Image")));
            this.m_btnFlightPolicy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_btnFlightPolicy.Name = "m_btnFlightPolicy";
            this.m_btnFlightPolicy.Size = new System.Drawing.Size(23, 22);
            this.m_btnFlightPolicy.Text = "Flight Policy";
            this.m_btnFlightPolicy.ToolTipText = "All Flights Permitted";
            this.m_btnFlightPolicy.Click += new System.EventHandler(this.m_btnFlightPolicy_Click);
            // 
            // m_dockPanel
            // 
            this.m_dockPanel.AutoSize = true;
            this.m_dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dockPanel.Location = new System.Drawing.Point(0, 49);
            this.m_dockPanel.Margin = new System.Windows.Forms.Padding(10);
            this.m_dockPanel.Name = "m_dockPanel";
            this.m_dockPanel.ShowDocumentIcon = true;
            this.m_dockPanel.Size = new System.Drawing.Size(994, 483);
            this.m_dockPanel.TabIndex = 0;
            // 
            // Epidemic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 554);
            this.Controls.Add(this.m_dockPanel);
            this.Controls.Add(this.m_toolStrip);
            this.Controls.Add(this.m_statusStrip);
            this.Controls.Add(this.m_menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.m_menuStrip;
            this.Name = "Epidemic";
            this.Text = "Epidemic";
            this.m_statusStrip.ResumeLayout(false);
            this.m_statusStrip.PerformLayout();
            this.m_menuStrip.ResumeLayout(false);
            this.m_menuStrip.PerformLayout();
            this.m_toolStrip.ResumeLayout(false);
            this.m_toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip m_statusStrip;
        private System.Windows.Forms.MenuStrip m_menuStrip;
        private System.Windows.Forms.ToolStrip m_toolStrip;
        private WeifenLuo.WinFormsUI.Docking.DockPanel m_dockPanel;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem1;
        private System.Windows.Forms.ToolStripButton m_btnRunPause;
        private System.Windows.Forms.ToolStripButton m_btnStop;
        private System.Windows.Forms.ToolStripButton m_btnReset;
        private System.Windows.Forms.ToolStripMenuItem infectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton m_btnInfect;
        private System.Windows.Forms.ToolStripStatusLabel DateTimeStatus;
        private System.Windows.Forms.ToolStripStatusLabel m_informationalStatusText;
        private System.Windows.Forms.ToolStripProgressBar Progress;
        private System.Windows.Forms.ToolStripMenuItem transportationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem airTravelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ceaseForKnownInfectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ceaseEntirelyToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton m_btnFlightPolicy;
        private System.Windows.Forms.ToolStripMenuItem permitAllFlightsToolStripMenuItem;
    }
}