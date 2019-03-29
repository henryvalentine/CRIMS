namespace TandAProject.Controls
{
    partial class ReportManagerControl
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
            this.centerPanel1 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dailyAttendanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attendanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attendanceSummaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.movementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.absenteeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.latenessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.absconderReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dailyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attendanceToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.absentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attendanceMovementReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.absenteeReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.latenessReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.movementReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerPanel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // centerPanel1
            // 
            this.centerPanel1.AutoSize = true;
            this.centerPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.centerPanel1.Controls.Add(this.panel1);
            this.centerPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centerPanel1.Location = new System.Drawing.Point(0, 24);
            this.centerPanel1.Name = "centerPanel1";
            this.centerPanel1.Size = new System.Drawing.Size(800, 453);
            this.centerPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(798, 451);
            this.panel1.TabIndex = 3;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.reportsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dailyAttendanceToolStripMenuItem,
            this.dailyToolStripMenuItem});
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.reportsToolStripMenuItem.Text = "Reports";
            // 
            // dailyAttendanceToolStripMenuItem
            // 
            this.dailyAttendanceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.attendanceToolStripMenuItem,
            this.attendanceSummaryToolStripMenuItem,
            this.movementToolStripMenuItem,
            this.toolStripSeparator1,
            this.absenteeToolStripMenuItem,
            this.latenessToolStripMenuItem,
            this.absconderReportToolStripMenuItem});
            this.dailyAttendanceToolStripMenuItem.Name = "dailyAttendanceToolStripMenuItem";
            this.dailyAttendanceToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dailyAttendanceToolStripMenuItem.Text = "Organization";
            // 
            // attendanceToolStripMenuItem
            // 
            this.attendanceToolStripMenuItem.Name = "attendanceToolStripMenuItem";
            this.attendanceToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.attendanceToolStripMenuItem.Text = "Details Report";
            this.attendanceToolStripMenuItem.Click += new System.EventHandler(this.attendanceToolStripMenuItem_Click);
            // 
            // attendanceSummaryToolStripMenuItem
            // 
            this.attendanceSummaryToolStripMenuItem.Name = "attendanceSummaryToolStripMenuItem";
            this.attendanceSummaryToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.attendanceSummaryToolStripMenuItem.Text = "Summary Report";
            this.attendanceSummaryToolStripMenuItem.Click += new System.EventHandler(this.attendanceSummaryToolStripMenuItem_Click);
            // 
            // movementToolStripMenuItem
            // 
            this.movementToolStripMenuItem.Name = "movementToolStripMenuItem";
            this.movementToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.movementToolStripMenuItem.Text = "Movement Report";
            this.movementToolStripMenuItem.Click += new System.EventHandler(this.movementToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(167, 6);
            // 
            // absenteeToolStripMenuItem
            // 
            this.absenteeToolStripMenuItem.Name = "absenteeToolStripMenuItem";
            this.absenteeToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.absenteeToolStripMenuItem.Text = "Absentee Report";
            this.absenteeToolStripMenuItem.Click += new System.EventHandler(this.absenteeToolStripMenuItem_Click);
            // 
            // latenessToolStripMenuItem
            // 
            this.latenessToolStripMenuItem.Name = "latenessToolStripMenuItem";
            this.latenessToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.latenessToolStripMenuItem.Text = "Lateness Report";
            this.latenessToolStripMenuItem.Click += new System.EventHandler(this.latenessToolStripMenuItem_Click);
            // 
            // absconderReportToolStripMenuItem
            // 
            this.absconderReportToolStripMenuItem.Name = "absconderReportToolStripMenuItem";
            this.absconderReportToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.absconderReportToolStripMenuItem.Text = "Absconder Report";
            this.absconderReportToolStripMenuItem.Click += new System.EventHandler(this.absconderReportToolStripMenuItem_Click);
            // 
            // dailyToolStripMenuItem
            // 
            this.dailyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.attendanceToolStripMenuItem2,
            this.absentToolStripMenuItem,
            this.attendanceMovementReportToolStripMenuItem,
            this.toolStripSeparator2,
            this.absenteeReportToolStripMenuItem,
            this.latenessReportToolStripMenuItem,
            this.movementReportToolStripMenuItem});
            this.dailyToolStripMenuItem.Name = "dailyToolStripMenuItem";
            this.dailyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dailyToolStripMenuItem.Text = "User";
            // 
            // attendanceToolStripMenuItem2
            // 
            this.attendanceToolStripMenuItem2.Name = "attendanceToolStripMenuItem2";
            this.attendanceToolStripMenuItem2.Size = new System.Drawing.Size(170, 22);
            this.attendanceToolStripMenuItem2.Text = "Details Report";
            // 
            // absentToolStripMenuItem
            // 
            this.absentToolStripMenuItem.Name = "absentToolStripMenuItem";
            this.absentToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.absentToolStripMenuItem.Text = "Summary Report";
            // 
            // attendanceMovementReportToolStripMenuItem
            // 
            this.attendanceMovementReportToolStripMenuItem.Name = "attendanceMovementReportToolStripMenuItem";
            this.attendanceMovementReportToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.attendanceMovementReportToolStripMenuItem.Text = "Movement Report";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(167, 6);
            // 
            // absenteeReportToolStripMenuItem
            // 
            this.absenteeReportToolStripMenuItem.Name = "absenteeReportToolStripMenuItem";
            this.absenteeReportToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.absenteeReportToolStripMenuItem.Text = "Absentee Report";
            // 
            // latenessReportToolStripMenuItem
            // 
            this.latenessReportToolStripMenuItem.Name = "latenessReportToolStripMenuItem";
            this.latenessReportToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.latenessReportToolStripMenuItem.Text = "Lateness Report";
            // 
            // movementReportToolStripMenuItem
            // 
            this.movementReportToolStripMenuItem.Name = "movementReportToolStripMenuItem";
            this.movementReportToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.movementReportToolStripMenuItem.Text = "Movement Report";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // ReportManagerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.centerPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "ReportManagerControl";
            this.Size = new System.Drawing.Size(800, 477);
            this.centerPanel1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel centerPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dailyAttendanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem attendanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem movementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem absenteeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dailyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem attendanceToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem absentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem latenessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem attendanceSummaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem absconderReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem attendanceMovementReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem absenteeReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem latenessReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem movementReportToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
    }
}
