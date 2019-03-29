namespace TandAProject.Controls
{
    partial class ImportExportControl
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
            this.checkBoxAllData = new System.Windows.Forms.CheckBox();
            this.dateTimeStart = new System.Windows.Forms.DateTimePicker();
            this.dateTimeEnd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonImport = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelStatus = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttomPanel = new System.Windows.Forms.Panel();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.openImportFile = new System.Windows.Forms.OpenFileDialog();
            this.saveExportFile = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserExport = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.buttomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxAllData
            // 
            this.checkBoxAllData.AutoSize = true;
            this.checkBoxAllData.Location = new System.Drawing.Point(20, 36);
            this.checkBoxAllData.Name = "checkBoxAllData";
            this.checkBoxAllData.Size = new System.Drawing.Size(178, 22);
            this.checkBoxAllData.TabIndex = 1;
            this.checkBoxAllData.Text = "All data on this terminal";
            this.checkBoxAllData.UseVisualStyleBackColor = true;
            this.checkBoxAllData.Click += new System.EventHandler(this.checkBoxAllData_Click);
            // 
            // dateTimeStart
            // 
            this.dateTimeStart.CustomFormat = "yyyy-MMM-d";
            this.dateTimeStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimeStart.Location = new System.Drawing.Point(108, 71);
            this.dateTimeStart.Name = "dateTimeStart";
            this.dateTimeStart.Size = new System.Drawing.Size(132, 24);
            this.dateTimeStart.TabIndex = 2;
            // 
            // dateTimeEnd
            // 
            this.dateTimeEnd.CustomFormat = "yyyy-MMM-d";
            this.dateTimeEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimeEnd.Location = new System.Drawing.Point(340, 71);
            this.dateTimeEnd.Name = "dateTimeEnd";
            this.dateTimeEnd.Size = new System.Drawing.Size(132, 24);
            this.dateTimeEnd.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Start Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(254, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "End Date:";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(10, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(686, 312);
            this.panel1.TabIndex = 4;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 287);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(684, 23);
            this.progressBar1.TabIndex = 7;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonImport);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(21, 191);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(641, 79);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Import Attendance Logs";
            // 
            // buttonImport
            // 
            this.buttonImport.Location = new System.Drawing.Point(492, 30);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(123, 34);
            this.buttonImport.TabIndex = 4;
            this.buttonImport.Text = "Import...";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(108, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(364, 24);
            this.textBox1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(31, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 18);
            this.label3.TabIndex = 3;
            this.label3.Text = "Browse:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelStatus);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(684, 38);
            this.panel2.TabIndex = 5;
            // 
            // labelStatus
            // 
            this.labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(0, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(684, 38);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "0%";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonExport);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dateTimeEnd);
            this.groupBox1.Controls.Add(this.dateTimeStart);
            this.groupBox1.Controls.Add(this.checkBoxAllData);
            this.groupBox1.Location = new System.Drawing.Point(21, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(641, 124);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Export Attendance Logs";
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(492, 66);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(123, 34);
            this.buttonExport.TabIndex = 4;
            this.buttonExport.Text = "Export...";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // buttomPanel
            // 
            this.buttomPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.buttomPanel.Controls.Add(this.buttonSettings);
            this.buttomPanel.Controls.Add(this.buttonClose);
            this.buttomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttomPanel.Location = new System.Drawing.Point(0, 326);
            this.buttomPanel.Name = "buttomPanel";
            this.buttomPanel.Size = new System.Drawing.Size(707, 58);
            this.buttomPanel.TabIndex = 5;
            // 
            // buttonSettings
            // 
            this.buttonSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSettings.Location = new System.Drawing.Point(502, 12);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(88, 36);
            this.buttonSettings.TabIndex = 2;
            this.buttonSettings.Text = "Settings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClose.Location = new System.Drawing.Point(612, 12);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(88, 36);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // openImportFile
            // 
            this.openImportFile.Filter = "Export Files|*.expt";
            this.openImportFile.InitialDirectory = "C:\\CTA";
            this.openImportFile.Title = "Open Attendance Export Log";
            // 
            // saveExportFile
            // 
            this.saveExportFile.Filter = "Residency Export Files|*.expt";
            this.saveExportFile.InitialDirectory = "C:\\CTA";
            this.saveExportFile.Title = "Save Attendance Export Log";
            // 
            // folderBrowserExport
            // 
            this.folderBrowserExport.Description = "Attendance Export Log Folder";
            this.folderBrowserExport.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // ImportExportControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.buttomPanel);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ImportExportControl";
            this.Size = new System.Drawing.Size(707, 384);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.buttomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBoxAllData;
        private System.Windows.Forms.DateTimePicker dateTimeStart;
        private System.Windows.Forms.DateTimePicker dateTimeEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel buttomPanel;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.OpenFileDialog openImportFile;
        private System.Windows.Forms.SaveFileDialog saveExportFile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserExport;
    }
}
