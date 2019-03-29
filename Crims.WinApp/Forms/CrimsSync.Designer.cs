namespace Crims.UI.Win.Enroll.Forms
{
    partial class CrimsSync
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label26 = new System.Windows.Forms.Label();
            this.buttonLoadRecords = new System.Windows.Forms.Button();
            this.textBoxRecordID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.radioButtonFilter = new System.Windows.Forms.RadioButton();
            this.radioButtonOne = new System.Windows.Forms.RadioButton();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label25 = new System.Windows.Forms.Label();
            this.btnSyncDataForm = new System.Windows.Forms.Button();
            this.labelSyncEndTime = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelSyncStartTime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelTotalFailed = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelRecordInSyncQueue = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelTotalSuccess = new System.Windows.Forms.Label();
            this.labelTotalRecords = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonBeginSync = new System.Windows.Forms.Button();
            this.buttonResetSyncHistory = new System.Windows.Forms.Button();
            this.buttonCancelSync = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblExportEnd = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblExportStart = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtSingleBackup = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.backupEndDate = new System.Windows.Forms.DateTimePicker();
            this.backupStartDate = new System.Windows.Forms.DateTimePicker();
            this.rdDateRangeBackup = new System.Windows.Forms.RadioButton();
            this.rdSingleBackup = new System.Windows.Forms.RadioButton();
            this.rdAllBackup = new System.Windows.Forms.RadioButton();
            this.label16 = new System.Windows.Forms.Label();
            this.lblTotalExport = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.btnExportData = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lblTotalImported = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lblImportEnd = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblImportStart = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblTotalImportCount = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.btnStartImport = new System.Windows.Forms.Button();
            this.btnCancelImport = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.buttonLoadRecords);
            this.groupBox1.Controls.Add(this.textBoxRecordID);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dateTimePickerTo);
            this.groupBox1.Controls.Add(this.dateTimePickerFrom);
            this.groupBox1.Controls.Add(this.radioButtonFilter);
            this.groupBox1.Controls.Add(this.radioButtonOne);
            this.groupBox1.Controls.Add(this.radioButtonAll);
            this.groupBox1.Location = new System.Drawing.Point(1, 25);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(737, 196);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sync Options";
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(531, 62);
            this.label26.Name = "label26";
            this.label26.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label26.Size = new System.Drawing.Size(186, 28);
            this.label26.TabIndex = 21;
            this.label26.Text = "N.B: Click this button first to initiate the Sync process";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonLoadRecords
            // 
            this.buttonLoadRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLoadRecords.Location = new System.Drawing.Point(531, 17);
            this.buttonLoadRecords.Margin = new System.Windows.Forms.Padding(2);
            this.buttonLoadRecords.Name = "buttonLoadRecords";
            this.buttonLoadRecords.Size = new System.Drawing.Size(194, 40);
            this.buttonLoadRecords.TabIndex = 4;
            this.buttonLoadRecords.Text = "Refresh and Sync Approvals";
            this.buttonLoadRecords.UseVisualStyleBackColor = true;
            this.buttonLoadRecords.Click += new System.EventHandler(this.buttonLoadRecords_Click);
            // 
            // textBoxRecordID
            // 
            this.textBoxRecordID.Location = new System.Drawing.Point(78, 159);
            this.textBoxRecordID.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxRecordID.Name = "textBoxRecordID";
            this.textBoxRecordID.Size = new System.Drawing.Size(278, 20);
            this.textBoxRecordID.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(202, 94);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "To:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 166);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Record Id:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 94);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "From:";
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerTo.Location = new System.Drawing.Point(226, 92);
            this.dateTimePickerTo.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(130, 20);
            this.dateTimePickerTo.TabIndex = 1;
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerFrom.Location = new System.Drawing.Point(51, 92);
            this.dateTimePickerFrom.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(130, 20);
            this.dateTimePickerFrom.TabIndex = 1;
            // 
            // radioButtonFilter
            // 
            this.radioButtonFilter.AutoSize = true;
            this.radioButtonFilter.Location = new System.Drawing.Point(11, 62);
            this.radioButtonFilter.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonFilter.Name = "radioButtonFilter";
            this.radioButtonFilter.Size = new System.Drawing.Size(198, 17);
            this.radioButtonFilter.TabIndex = 0;
            this.radioButtonFilter.Text = "Records Filtered By Enrollment Date:";
            this.radioButtonFilter.UseVisualStyleBackColor = true;
            // 
            // radioButtonOne
            // 
            this.radioButtonOne.AutoSize = true;
            this.radioButtonOne.Location = new System.Drawing.Point(11, 129);
            this.radioButtonOne.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonOne.Name = "radioButtonOne";
            this.radioButtonOne.Size = new System.Drawing.Size(114, 17);
            this.radioButtonOne.TabIndex = 0;
            this.radioButtonOne.Text = "A Specific Record:";
            this.radioButtonOne.UseVisualStyleBackColor = true;
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Checked = true;
            this.radioButtonAll.Location = new System.Drawing.Point(11, 31);
            this.radioButtonAll.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(124, 17);
            this.radioButtonAll.TabIndex = 0;
            this.radioButtonAll.TabStop = true;
            this.radioButtonAll.Text = "All Pending Records:";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 442);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 9, 0);
            this.statusStrip1.Size = new System.Drawing.Size(745, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(745, 442);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label25);
            this.tabPage1.Controls.Add(this.btnSyncDataForm);
            this.tabPage1.Controls.Add(this.labelSyncEndTime);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.labelSyncStartTime);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.labelTotalFailed);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.labelRecordInSyncQueue);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.labelTotalSuccess);
            this.tabPage1.Controls.Add(this.labelTotalRecords);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.buttonBeginSync);
            this.tabPage1.Controls.Add(this.buttonResetSyncHistory);
            this.tabPage1.Controls.Add(this.buttonCancelSync);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(737, 416);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Data Synchronisation";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(537, 329);
            this.label25.Name = "label25";
            this.label25.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label25.Size = new System.Drawing.Size(189, 41);
            this.label25.TabIndex = 20;
            this.label25.Text = "Note: The number of Data forms to synchronise depends on the \'Sync Option\' select" +
    "ed above";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSyncDataForm
            // 
            this.btnSyncDataForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSyncDataForm.Location = new System.Drawing.Point(540, 375);
            this.btnSyncDataForm.Margin = new System.Windows.Forms.Padding(2);
            this.btnSyncDataForm.Name = "btnSyncDataForm";
            this.btnSyncDataForm.Size = new System.Drawing.Size(186, 31);
            this.btnSyncDataForm.TabIndex = 5;
            this.btnSyncDataForm.Text = "Sync Data Forms";
            this.btnSyncDataForm.UseVisualStyleBackColor = true;
            this.btnSyncDataForm.Click += new System.EventHandler(this.btnSyncDataForm_Click);
            // 
            // labelSyncEndTime
            // 
            this.labelSyncEndTime.BackColor = System.Drawing.Color.Lavender;
            this.labelSyncEndTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSyncEndTime.Location = new System.Drawing.Point(131, 299);
            this.labelSyncEndTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSyncEndTime.Name = "labelSyncEndTime";
            this.labelSyncEndTime.Size = new System.Drawing.Size(373, 14);
            this.labelSyncEndTime.TabIndex = 8;
            this.labelSyncEndTime.Text = "0";
            this.labelSyncEndTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(64, 299);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Sync Ended";
            // 
            // labelSyncStartTime
            // 
            this.labelSyncStartTime.BackColor = System.Drawing.Color.Lavender;
            this.labelSyncStartTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSyncStartTime.Location = new System.Drawing.Point(131, 278);
            this.labelSyncStartTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSyncStartTime.Name = "labelSyncStartTime";
            this.labelSyncStartTime.Size = new System.Drawing.Size(373, 14);
            this.labelSyncStartTime.TabIndex = 10;
            this.labelSyncStartTime.Text = "0";
            this.labelSyncStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(60, 278);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Sync Started";
            // 
            // labelTotalFailed
            // 
            this.labelTotalFailed.BackColor = System.Drawing.Color.Lavender;
            this.labelTotalFailed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalFailed.Location = new System.Drawing.Point(131, 343);
            this.labelTotalFailed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTotalFailed.Name = "labelTotalFailed";
            this.labelTotalFailed.Size = new System.Drawing.Size(373, 14);
            this.labelTotalFailed.TabIndex = 12;
            this.labelTotalFailed.Text = "0";
            this.labelTotalFailed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(67, 343);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Total Failed";
            // 
            // labelRecordInSyncQueue
            // 
            this.labelRecordInSyncQueue.BackColor = System.Drawing.Color.Lavender;
            this.labelRecordInSyncQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRecordInSyncQueue.Location = new System.Drawing.Point(131, 258);
            this.labelRecordInSyncQueue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRecordInSyncQueue.Name = "labelRecordInSyncQueue";
            this.labelRecordInSyncQueue.Size = new System.Drawing.Size(373, 14);
            this.labelRecordInSyncQueue.TabIndex = 14;
            this.labelRecordInSyncQueue.Text = "0";
            this.labelRecordInSyncQueue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 258);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Records in Sync Queue";
            // 
            // labelTotalSuccess
            // 
            this.labelTotalSuccess.BackColor = System.Drawing.Color.Lavender;
            this.labelTotalSuccess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalSuccess.Location = new System.Drawing.Point(131, 321);
            this.labelTotalSuccess.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTotalSuccess.Name = "labelTotalSuccess";
            this.labelTotalSuccess.Size = new System.Drawing.Size(373, 14);
            this.labelTotalSuccess.TabIndex = 16;
            this.labelTotalSuccess.Text = "0";
            this.labelTotalSuccess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTotalRecords
            // 
            this.labelTotalRecords.BackColor = System.Drawing.Color.Lavender;
            this.labelTotalRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalRecords.Location = new System.Drawing.Point(131, 237);
            this.labelTotalRecords.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTotalRecords.Name = "labelTotalRecords";
            this.labelTotalRecords.Size = new System.Drawing.Size(373, 14);
            this.labelTotalRecords.TabIndex = 17;
            this.labelTotalRecords.Text = "0";
            this.labelTotalRecords.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(43, 321);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Total Successful";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 237);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Total Records";
            // 
            // buttonBeginSync
            // 
            this.buttonBeginSync.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBeginSync.Location = new System.Drawing.Point(46, 375);
            this.buttonBeginSync.Margin = new System.Windows.Forms.Padding(2);
            this.buttonBeginSync.Name = "buttonBeginSync";
            this.buttonBeginSync.Size = new System.Drawing.Size(105, 31);
            this.buttonBeginSync.TabIndex = 5;
            this.buttonBeginSync.Text = "Sync Data";
            this.buttonBeginSync.UseVisualStyleBackColor = true;
            this.buttonBeginSync.Click += new System.EventHandler(this.buttonBeginSync_Click);
            // 
            // buttonResetSyncHistory
            // 
            this.buttonResetSyncHistory.Location = new System.Drawing.Point(216, 375);
            this.buttonResetSyncHistory.Margin = new System.Windows.Forms.Padding(2);
            this.buttonResetSyncHistory.Name = "buttonResetSyncHistory";
            this.buttonResetSyncHistory.Size = new System.Drawing.Size(108, 31);
            this.buttonResetSyncHistory.TabIndex = 6;
            this.buttonResetSyncHistory.Text = "Reset Sync History";
            this.buttonResetSyncHistory.UseVisualStyleBackColor = true;
            this.buttonResetSyncHistory.Click += new System.EventHandler(this.buttonResetSyncHistory_Click);
            // 
            // buttonCancelSync
            // 
            this.buttonCancelSync.Enabled = false;
            this.buttonCancelSync.Location = new System.Drawing.Point(387, 375);
            this.buttonCancelSync.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancelSync.Name = "buttonCancelSync";
            this.buttonCancelSync.Size = new System.Drawing.Size(117, 31);
            this.buttonCancelSync.TabIndex = 7;
            this.buttonCancelSync.Text = "Cancel Sync";
            this.buttonCancelSync.UseVisualStyleBackColor = true;
            this.buttonCancelSync.Click += new System.EventHandler(this.buttonCancelSync_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblExportEnd);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.lblExportStart);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.label16);
            this.tabPage2.Controls.Add(this.lblTotalExport);
            this.tabPage2.Controls.Add(this.label24);
            this.tabPage2.Controls.Add(this.btnExportData);
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(737, 416);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Data Export";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblExportEnd
            // 
            this.lblExportEnd.BackColor = System.Drawing.Color.Lavender;
            this.lblExportEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExportEnd.Location = new System.Drawing.Point(128, 331);
            this.lblExportEnd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblExportEnd.Name = "lblExportEnd";
            this.lblExportEnd.Size = new System.Drawing.Size(373, 14);
            this.lblExportEnd.TabIndex = 24;
            this.lblExportEnd.Text = "0";
            this.lblExportEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(58, 331);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 13);
            this.label11.TabIndex = 25;
            this.label11.Text = "Export Ended";
            // 
            // lblExportStart
            // 
            this.lblExportStart.BackColor = System.Drawing.Color.Lavender;
            this.lblExportStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExportStart.Location = new System.Drawing.Point(128, 301);
            this.lblExportStart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblExportStart.Name = "lblExportStart";
            this.lblExportStart.Size = new System.Drawing.Size(373, 14);
            this.lblExportStart.TabIndex = 26;
            this.lblExportStart.Text = "0";
            this.lblExportStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtSingleBackup);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.backupEndDate);
            this.groupBox2.Controls.Add(this.backupStartDate);
            this.groupBox2.Controls.Add(this.rdDateRangeBackup);
            this.groupBox2.Controls.Add(this.rdSingleBackup);
            this.groupBox2.Controls.Add(this.rdAllBackup);
            this.groupBox2.Location = new System.Drawing.Point(0, 27);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(737, 203);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Export Options";
            // 
            // txtSingleBackup
            // 
            this.txtSingleBackup.Location = new System.Drawing.Point(78, 154);
            this.txtSingleBackup.Margin = new System.Windows.Forms.Padding(2);
            this.txtSingleBackup.Multiline = true;
            this.txtSingleBackup.Name = "txtSingleBackup";
            this.txtSingleBackup.Size = new System.Drawing.Size(278, 26);
            this.txtSingleBackup.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(202, 89);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(23, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "To:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(17, 161);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(57, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Record Id:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(13, 89);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(33, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "From:";
            // 
            // backupEndDate
            // 
            this.backupEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.backupEndDate.Location = new System.Drawing.Point(226, 87);
            this.backupEndDate.Margin = new System.Windows.Forms.Padding(2);
            this.backupEndDate.Name = "backupEndDate";
            this.backupEndDate.Size = new System.Drawing.Size(130, 20);
            this.backupEndDate.TabIndex = 1;
            // 
            // backupStartDate
            // 
            this.backupStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.backupStartDate.Location = new System.Drawing.Point(51, 87);
            this.backupStartDate.Margin = new System.Windows.Forms.Padding(2);
            this.backupStartDate.Name = "backupStartDate";
            this.backupStartDate.Size = new System.Drawing.Size(130, 20);
            this.backupStartDate.TabIndex = 1;
            // 
            // rdDateRangeBackup
            // 
            this.rdDateRangeBackup.AutoSize = true;
            this.rdDateRangeBackup.Location = new System.Drawing.Point(11, 57);
            this.rdDateRangeBackup.Margin = new System.Windows.Forms.Padding(2);
            this.rdDateRangeBackup.Name = "rdDateRangeBackup";
            this.rdDateRangeBackup.Size = new System.Drawing.Size(198, 17);
            this.rdDateRangeBackup.TabIndex = 0;
            this.rdDateRangeBackup.Text = "Records Filtered By Enrollment Date:";
            this.rdDateRangeBackup.UseVisualStyleBackColor = true;
            // 
            // rdSingleBackup
            // 
            this.rdSingleBackup.AutoSize = true;
            this.rdSingleBackup.Location = new System.Drawing.Point(11, 124);
            this.rdSingleBackup.Margin = new System.Windows.Forms.Padding(2);
            this.rdSingleBackup.Name = "rdSingleBackup";
            this.rdSingleBackup.Size = new System.Drawing.Size(114, 17);
            this.rdSingleBackup.TabIndex = 0;
            this.rdSingleBackup.Text = "A Specific Record:";
            this.rdSingleBackup.UseVisualStyleBackColor = true;
            // 
            // rdAllBackup
            // 
            this.rdAllBackup.AutoSize = true;
            this.rdAllBackup.Checked = true;
            this.rdAllBackup.Location = new System.Drawing.Point(11, 17);
            this.rdAllBackup.Margin = new System.Windows.Forms.Padding(2);
            this.rdAllBackup.Name = "rdAllBackup";
            this.rdAllBackup.Size = new System.Drawing.Size(124, 17);
            this.rdAllBackup.TabIndex = 0;
            this.rdAllBackup.TabStop = true;
            this.rdAllBackup.Text = "All Pending Records:";
            this.rdAllBackup.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(55, 301);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(74, 13);
            this.label16.TabIndex = 27;
            this.label16.Text = "Export Started";
            // 
            // lblTotalExport
            // 
            this.lblTotalExport.BackColor = System.Drawing.Color.Lavender;
            this.lblTotalExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalExport.Location = new System.Drawing.Point(128, 271);
            this.lblTotalExport.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalExport.Name = "lblTotalExport";
            this.lblTotalExport.Size = new System.Drawing.Size(373, 14);
            this.lblTotalExport.TabIndex = 33;
            this.lblTotalExport.Text = "0";
            this.lblTotalExport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(52, 271);
            this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(74, 13);
            this.label24.TabIndex = 35;
            this.label24.Text = "Total Records";
            // 
            // btnExportData
            // 
            this.btnExportData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportData.Location = new System.Drawing.Point(66, 370);
            this.btnExportData.Margin = new System.Windows.Forms.Padding(2);
            this.btnExportData.Name = "btnExportData";
            this.btnExportData.Size = new System.Drawing.Size(105, 31);
            this.btnExportData.TabIndex = 21;
            this.btnExportData.Text = "Export";
            this.btnExportData.UseVisualStyleBackColor = true;
            this.btnExportData.Click += new System.EventHandler(this.btnExportData_Click);
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(205, 370);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(117, 31);
            this.button4.TabIndex = 23;
            this.button4.Text = "Cancel";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lblTotalImported);
            this.tabPage3.Controls.Add(this.label17);
            this.tabPage3.Controls.Add(this.lblImportEnd);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.lblImportStart);
            this.tabPage3.Controls.Add(this.label18);
            this.tabPage3.Controls.Add(this.lblTotalImportCount);
            this.tabPage3.Controls.Add(this.label20);
            this.tabPage3.Controls.Add(this.btnStartImport);
            this.tabPage3.Controls.Add(this.btnCancelImport);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage3.Size = new System.Drawing.Size(737, 416);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Data Import";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lblTotalImported
            // 
            this.lblTotalImported.BackColor = System.Drawing.Color.Lavender;
            this.lblTotalImported.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalImported.Location = new System.Drawing.Point(213, 222);
            this.lblTotalImported.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalImported.Name = "lblTotalImported";
            this.lblTotalImported.Size = new System.Drawing.Size(373, 14);
            this.lblTotalImported.TabIndex = 44;
            this.lblTotalImported.Text = "0";
            this.lblTotalImported.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(137, 222);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(75, 13);
            this.label17.TabIndex = 45;
            this.label17.Text = "Total Imported";
            // 
            // lblImportEnd
            // 
            this.lblImportEnd.BackColor = System.Drawing.Color.Lavender;
            this.lblImportEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImportEnd.Location = new System.Drawing.Point(213, 189);
            this.lblImportEnd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblImportEnd.Name = "lblImportEnd";
            this.lblImportEnd.Size = new System.Drawing.Size(373, 14);
            this.lblImportEnd.TabIndex = 38;
            this.lblImportEnd.Text = "0";
            this.lblImportEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(143, 189);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 13);
            this.label12.TabIndex = 39;
            this.label12.Text = "Import Ended";
            // 
            // lblImportStart
            // 
            this.lblImportStart.BackColor = System.Drawing.Color.Lavender;
            this.lblImportStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImportStart.Location = new System.Drawing.Point(213, 159);
            this.lblImportStart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblImportStart.Name = "lblImportStart";
            this.lblImportStart.Size = new System.Drawing.Size(373, 14);
            this.lblImportStart.TabIndex = 40;
            this.lblImportStart.Text = "0";
            this.lblImportStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(140, 159);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(73, 13);
            this.label18.TabIndex = 41;
            this.label18.Text = "Import Started";
            // 
            // lblTotalImportCount
            // 
            this.lblTotalImportCount.BackColor = System.Drawing.Color.Lavender;
            this.lblTotalImportCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalImportCount.Location = new System.Drawing.Point(213, 129);
            this.lblTotalImportCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalImportCount.Name = "lblTotalImportCount";
            this.lblTotalImportCount.Size = new System.Drawing.Size(373, 14);
            this.lblTotalImportCount.TabIndex = 42;
            this.lblTotalImportCount.Text = "0";
            this.lblTotalImportCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(137, 129);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(74, 13);
            this.label20.TabIndex = 43;
            this.label20.Text = "Total Records";
            // 
            // btnStartImport
            // 
            this.btnStartImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartImport.Location = new System.Drawing.Point(147, 281);
            this.btnStartImport.Margin = new System.Windows.Forms.Padding(2);
            this.btnStartImport.Name = "btnStartImport";
            this.btnStartImport.Size = new System.Drawing.Size(125, 31);
            this.btnStartImport.TabIndex = 36;
            this.btnStartImport.Text = "Select Import File";
            this.btnStartImport.UseVisualStyleBackColor = true;
            this.btnStartImport.Click += new System.EventHandler(this.btnStartImport_Click);
            // 
            // btnCancelImport
            // 
            this.btnCancelImport.Enabled = false;
            this.btnCancelImport.Location = new System.Drawing.Point(294, 281);
            this.btnCancelImport.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancelImport.Name = "btnCancelImport";
            this.btnCancelImport.Size = new System.Drawing.Size(117, 31);
            this.btnCancelImport.TabIndex = 37;
            this.btnCancelImport.Text = "Cancel";
            this.btnCancelImport.UseVisualStyleBackColor = true;
            // 
            // CrimsSync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 464);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(539, 404);
            this.Name = "CrimsSync";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Crims.Sync";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CrimsSync_FormClosing);
            this.Load += new System.EventHandler(this.Crims_Load);
            this.Shown += new System.EventHandler(this.CrimsSync_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxRecordID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.RadioButton radioButtonFilter;
        private System.Windows.Forms.RadioButton radioButtonOne;
        private System.Windows.Forms.RadioButton radioButtonAll;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button buttonLoadRecords;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label labelSyncEndTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelSyncStartTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelTotalFailed;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelRecordInSyncQueue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelTotalSuccess;
        private System.Windows.Forms.Label labelTotalRecords;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonBeginSync;
        private System.Windows.Forms.Button buttonResetSyncHistory;
        private System.Windows.Forms.Button buttonCancelSync;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnSyncDataForm;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtSingleBackup;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker backupEndDate;
        private System.Windows.Forms.DateTimePicker backupStartDate;
        private System.Windows.Forms.RadioButton rdDateRangeBackup;
        private System.Windows.Forms.RadioButton rdSingleBackup;
        private System.Windows.Forms.RadioButton rdAllBackup;
        private System.Windows.Forms.Label lblTotalExport;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button btnExportData;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label lblExportEnd;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblExportStart;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lblImportEnd;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblImportStart;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lblTotalImportCount;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btnStartImport;
        private System.Windows.Forms.Button btnCancelImport;
        private System.Windows.Forms.Label lblTotalImported;
        private System.Windows.Forms.Label label17;
    }
}