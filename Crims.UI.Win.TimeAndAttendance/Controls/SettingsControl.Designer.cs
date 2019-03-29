using Neurotec.Biometrics;
using TandAProject.Services;

namespace TandAProject.Controls
{
    partial class SettingsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsControl));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonTest = new System.Windows.Forms.Button();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.textBoxDBName = new System.Windows.Forms.TextBox();
            this.textBoxDBPort = new System.Windows.Forms.TextBox();
            this.textBoxDBServer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxDBSource = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.checkBoxVerifyField = new System.Windows.Forms.CheckBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label30 = new System.Windows.Forms.Label();
            this.textBoxMessageDuration = new System.Windows.Forms.TextBox();
            this.labelRecordCount = new System.Windows.Forms.Label();
            this.labelTemplateCount = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonBrosweImage = new System.Windows.Forms.Button();
            this.AppBrand = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBoxSun = new System.Windows.Forms.CheckBox();
            this.label29 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxClosingGrace = new System.Windows.Forms.TextBox();
            this.textBoxClosing = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxResumptionGrace = new System.Windows.Forms.TextBox();
            this.textBoxResumption = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBoxSetupPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBoxSyncTime = new System.Windows.Forms.TextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.buttonTestSyncDBServer = new System.Windows.Forms.Button();
            this.checkBoxSyncOverWebService = new System.Windows.Forms.CheckBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.checkEnableSync = new System.Windows.Forms.CheckBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBoxSynchServer = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonAttendanceIE = new System.Windows.Forms.Button();
            this.buttonUserIE = new System.Windows.Forms.Button();
            this.buttonSyncControl = new System.Windows.Forms.Button();
            this.buttonReport = new System.Windows.Forms.Button();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableCData4 = new System.Windows.Forms.CheckBox();
            this.comboBoxCData4Picker = new System.Windows.Forms.ComboBox();
            this.label44 = new System.Windows.Forms.Label();
            this.textBoxCDataLabel4 = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.textBoxCDataField4 = new System.Windows.Forms.TextBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableCData3 = new System.Windows.Forms.CheckBox();
            this.comboBoxCData3Picker = new System.Windows.Forms.ComboBox();
            this.label42 = new System.Windows.Forms.Label();
            this.textBoxCDataLabel3 = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.textBoxCDataField3 = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableCData2 = new System.Windows.Forms.CheckBox();
            this.comboBoxCData2Picker = new System.Windows.Forms.ComboBox();
            this.label37 = new System.Windows.Forms.Label();
            this.textBoxCDataLabel2 = new System.Windows.Forms.TextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.textBoxCDataField2 = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableCData1 = new System.Windows.Forms.CheckBox();
            this.comboBoxCData1Picker = new System.Windows.Forms.ComboBox();
            this.label40 = new System.Windows.Forms.Label();
            this.textBoxCDataLabel1 = new System.Windows.Forms.TextBox();
            this.label39 = new System.Windows.Forms.Label();
            this.textBoxCDataField1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonShutDown = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(3, 84);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(940, 387);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonTest);
            this.tabPage1.Controls.Add(this.textBoxPassword);
            this.tabPage1.Controls.Add(this.textBoxUser);
            this.tabPage1.Controls.Add(this.textBoxDBName);
            this.tabPage1.Controls.Add(this.textBoxDBPort);
            this.tabPage1.Controls.Add(this.textBoxDBServer);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.comboBoxDBSource);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(932, 356);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Database";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(167, 222);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(190, 30);
            this.buttonTest.TabIndex = 21;
            this.buttonTest.Text = "Test Connection";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "DBPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxPassword.Location = new System.Drawing.Point(167, 186);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(193, 23);
            this.textBoxPassword.TabIndex = 16;
            this.textBoxPassword.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.DBPassword;
            // 
            // textBoxUser
            // 
            this.textBoxUser.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "DBUser", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxUser.Location = new System.Drawing.Point(167, 156);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(193, 23);
            this.textBoxUser.TabIndex = 17;
            this.textBoxUser.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.DBUser;
            // 
            // textBoxDBName
            // 
            this.textBoxDBName.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "DBName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxDBName.Location = new System.Drawing.Point(167, 126);
            this.textBoxDBName.Name = "textBoxDBName";
            this.textBoxDBName.Size = new System.Drawing.Size(193, 23);
            this.textBoxDBName.TabIndex = 18;
            this.textBoxDBName.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.DBName;
            // 
            // textBoxDBPort
            // 
            this.textBoxDBPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "DBPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxDBPort.Location = new System.Drawing.Point(167, 91);
            this.textBoxDBPort.Name = "textBoxDBPort";
            this.textBoxDBPort.Size = new System.Drawing.Size(193, 23);
            this.textBoxDBPort.TabIndex = 19;
            this.textBoxDBPort.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.DBPort;
            // 
            // textBoxDBServer
            // 
            this.textBoxDBServer.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "DBServer", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxDBServer.Location = new System.Drawing.Point(167, 61);
            this.textBoxDBServer.Name = "textBoxDBServer";
            this.textBoxDBServer.Size = new System.Drawing.Size(193, 23);
            this.textBoxDBServer.TabIndex = 20;
            this.textBoxDBServer.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.DBServer;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(74, 187);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 17);
            this.label5.TabIndex = 15;
            this.label5.Text = "Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(109, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 17);
            this.label4.TabIndex = 14;
            this.label4.Text = "User";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(78, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 17);
            this.label6.TabIndex = 13;
            this.label6.Text = "Database";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(118, 94);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "Port";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(103, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 17);
            this.label8.TabIndex = 12;
            this.label8.Text = "Server";
            // 
            // comboBoxDBSource
            // 
            this.comboBoxDBSource.Enabled = false;
            this.comboBoxDBSource.FormattingEnabled = true;
            this.comboBoxDBSource.Items.AddRange(new object[] {
            "MySQL",
            "Oracle"});
            this.comboBoxDBSource.Location = new System.Drawing.Point(167, 30);
            this.comboBoxDBSource.Name = "comboBoxDBSource";
            this.comboBoxDBSource.Size = new System.Drawing.Size(193, 24);
            this.comboBoxDBSource.TabIndex = 10;
            this.comboBoxDBSource.Text = "MySQL";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(31, 35);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(118, 17);
            this.label9.TabIndex = 9;
            this.label9.Text = "Database Source";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.checkBoxVerifyField);
            this.tabPage3.Controls.Add(this.panel6);
            this.tabPage3.Controls.Add(this.panel5);
            this.tabPage3.Controls.Add(this.panel4);
            this.tabPage3.Controls.Add(this.panel3);
            this.tabPage3.Controls.Add(this.label30);
            this.tabPage3.Controls.Add(this.textBoxMessageDuration);
            this.tabPage3.Controls.Add(this.labelRecordCount);
            this.tabPage3.Controls.Add(this.labelTemplateCount);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.buttonBrosweImage);
            this.tabPage3.Controls.Add(this.AppBrand);
            this.tabPage3.Controls.Add(this.label19);
            this.tabPage3.Controls.Add(this.label18);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.label16);
            this.tabPage3.Controls.Add(this.label15);
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Controls.Add(this.label20);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.textBox3);
            this.tabPage3.Controls.Add(this.textBox2);
            this.tabPage3.Controls.Add(this.checkBox1);
            this.tabPage3.Controls.Add(this.comboBox3);
            this.tabPage3.Controls.Add(this.comboBox4);
            this.tabPage3.Controls.Add(this.comboBox2);
            this.tabPage3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage3.Location = new System.Drawing.Point(4, 27);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(932, 356);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Application";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // checkBoxVerifyField
            // 
            this.checkBoxVerifyField.AutoSize = true;
            this.checkBoxVerifyField.Checked = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.UseCustomFieldVerifyMode;
            this.checkBoxVerifyField.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "UseCustomFieldVerifyMode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxVerifyField.Location = new System.Drawing.Point(17, 162);
            this.checkBoxVerifyField.Name = "checkBoxVerifyField";
            this.checkBoxVerifyField.Size = new System.Drawing.Size(439, 21);
            this.checkBoxVerifyField.TabIndex = 13;
            this.checkBoxVerifyField.Text = "Show User Data Before Capturing Fingerping in Verification Mode";
            this.checkBoxVerifyField.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel6.BackColor = System.Drawing.Color.DarkGray;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Location = new System.Drawing.Point(10, 202);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(915, 1);
            this.panel6.TabIndex = 12;
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.BackColor = System.Drawing.Color.DarkGray;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Location = new System.Drawing.Point(9, 150);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(915, 1);
            this.panel5.TabIndex = 11;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.DarkGray;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Location = new System.Drawing.Point(9, 98);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(915, 1);
            this.panel4.TabIndex = 10;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.DarkGray;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Location = new System.Drawing.Point(9, 46);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(915, 1);
            this.panel3.TabIndex = 9;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(784, 20);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(127, 13);
            this.label30.TabIndex = 8;
            this.label30.Text = "800 x 100 px. .png format";
            this.label30.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxMessageDuration
            // 
            this.textBoxMessageDuration.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "MessageDuration", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxMessageDuration.Location = new System.Drawing.Point(29, 242);
            this.textBoxMessageDuration.Name = "textBoxMessageDuration";
            this.textBoxMessageDuration.Size = new System.Drawing.Size(80, 23);
            this.textBoxMessageDuration.TabIndex = 7;
            this.textBoxMessageDuration.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.MessageDuration;
            // 
            // labelRecordCount
            // 
            this.labelRecordCount.AutoSize = true;
            this.labelRecordCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRecordCount.Location = new System.Drawing.Point(291, 116);
            this.labelRecordCount.Name = "labelRecordCount";
            this.labelRecordCount.Size = new System.Drawing.Size(15, 15);
            this.labelRecordCount.TabIndex = 6;
            this.labelRecordCount.Text = "0";
            this.labelRecordCount.Click += new System.EventHandler(this.labelRecordCount_Click);
            // 
            // labelTemplateCount
            // 
            this.labelTemplateCount.AutoSize = true;
            this.labelTemplateCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTemplateCount.Location = new System.Drawing.Point(523, 116);
            this.labelTemplateCount.Name = "labelTemplateCount";
            this.labelTemplateCount.Size = new System.Drawing.Size(15, 15);
            this.labelTemplateCount.TabIndex = 6;
            this.labelTemplateCount.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(174, 114);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(113, 17);
            this.label12.TabIndex = 5;
            this.label12.Text = " -  Total Loaded:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(386, 114);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(130, 17);
            this.label11.TabIndex = 5;
            this.label11.Text = "Templates Loaded:";
            // 
            // buttonBrosweImage
            // 
            this.buttonBrosweImage.Location = new System.Drawing.Point(687, 10);
            this.buttonBrosweImage.Name = "buttonBrosweImage";
            this.buttonBrosweImage.Size = new System.Drawing.Size(91, 28);
            this.buttonBrosweImage.TabIndex = 4;
            this.buttonBrosweImage.Text = "Browse...";
            this.buttonBrosweImage.UseVisualStyleBackColor = true;
            this.buttonBrosweImage.Click += new System.EventHandler(this.buttonBrosweImage_Click);
            // 
            // AppBrand
            // 
            this.AppBrand.Location = new System.Drawing.Point(442, 12);
            this.AppBrand.Name = "AppBrand";
            this.AppBrand.Size = new System.Drawing.Size(239, 23);
            this.AppBrand.TabIndex = 3;
            this.AppBrand.Text = "banner.png";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(115, 246);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(72, 13);
            this.label19.TabIndex = 1;
            this.label19.Text = "(Mili-Seconds)";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(26, 215);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(189, 17);
            this.label18.TabIndex = 1;
            this.label18.Text = "Verification Messag Duration";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(324, 11);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(104, 22);
            this.label10.TabIndex = 1;
            this.label10.Text = "BRANDING";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(509, 215);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(107, 17);
            this.label16.TabIndex = 1;
            this.label16.Text = "Failed Message";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(266, 215);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(122, 17);
            this.label15.TabIndex = 1;
            this.label15.Text = "Success Message";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(25, 10);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(158, 22);
            this.label14.TabIndex = 1;
            this.label14.Text = "TERMINAL MODE";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(381, 66);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(133, 17);
            this.label20.TabIndex = 1;
            this.label20.Text = "Matching Threshold";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 65);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(123, 17);
            this.label13.TabIndex = 1;
            this.label13.Text = "Fingerprint Device";
            // 
            // textBox3
            // 
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "FailedMessage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox3.Location = new System.Drawing.Point(506, 242);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(220, 58);
            this.textBox3.TabIndex = 7;
            this.textBox3.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.FailedMessage;
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "SuccessMessage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox2.Location = new System.Drawing.Point(261, 242);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(222, 58);
            this.textBox2.TabIndex = 7;
            this.textBox2.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.SuccessMessage;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.PreloadUserData;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "PreloadUserData", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(17, 114);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(151, 21);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Preload Users Data";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox3
            // 
            this.comboBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "TerminalMode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "Service",
            "Identify",
            "Verify",
            "Server"});
            this.comboBox3.Location = new System.Drawing.Point(190, 8);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(122, 28);
            this.comboBox3.TabIndex = 0;
            this.comboBox3.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.TerminalMode;
            // 
            // comboBox4
            // 
            this.comboBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "MatchingThreshold", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            "100",
            "200",
            "300",
            "400",
            "500",
            "600",
            "700",
            "800",
            "900",
            "1000"});
            this.comboBox4.Location = new System.Drawing.Point(525, 62);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(201, 26);
            this.comboBox4.TabIndex = 0;
            this.comboBox4.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.MatchingThreshold;
            // 
            // comboBox2
            // 
            this.comboBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "FingerPrintScanner", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Scanner-0",
            "Scanner-1",
            "Scanner-2",
            "Scanner-3",
            "Scanner-4"});
            this.comboBox2.Location = new System.Drawing.Point(146, 62);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(201, 26);
            this.comboBox2.TabIndex = 0;
            this.comboBox2.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.FingerPrintScanner;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(932, 356);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Time and Attendance";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox7);
            this.groupBox3.Controls.Add(this.checkBox6);
            this.groupBox3.Controls.Add(this.checkBox5);
            this.groupBox3.Controls.Add(this.checkBox4);
            this.groupBox3.Controls.Add(this.checkBox3);
            this.groupBox3.Controls.Add(this.checkBox2);
            this.groupBox3.Controls.Add(this.checkBoxSun);
            this.groupBox3.Controls.Add(this.label29);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(20, 222);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(473, 108);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Work Schedule";
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(412, 61);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(52, 22);
            this.checkBox7.TabIndex = 8;
            this.checkBox7.Text = "Sat";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Checked = true;
            this.checkBox6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox6.Location = new System.Drawing.Point(355, 61);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(47, 22);
            this.checkBox6.TabIndex = 8;
            this.checkBox6.Text = "Fri";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Checked = true;
            this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox5.Location = new System.Drawing.Point(290, 61);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(55, 22);
            this.checkBox5.TabIndex = 8;
            this.checkBox5.Text = "Thu";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Location = new System.Drawing.Point(219, 61);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(61, 22);
            this.checkBox4.TabIndex = 8;
            this.checkBox4.Text = "Wed";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(154, 61);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(55, 22);
            this.checkBox3.TabIndex = 8;
            this.checkBox3.Text = "Tue";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(84, 61);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(60, 22);
            this.checkBox2.TabIndex = 8;
            this.checkBox2.Text = "Mon";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBoxSun
            // 
            this.checkBoxSun.AutoSize = true;
            this.checkBoxSun.Location = new System.Drawing.Point(18, 61);
            this.checkBoxSun.Name = "checkBoxSun";
            this.checkBoxSun.Size = new System.Drawing.Size(56, 22);
            this.checkBoxSun.TabIndex = 8;
            this.checkBoxSun.Text = "Sun";
            this.checkBoxSun.UseVisualStyleBackColor = true;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(17, 35);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(42, 18);
            this.label29.TabIndex = 6;
            this.label29.Text = "Days";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxClosingGrace);
            this.groupBox2.Controls.Add(this.textBoxClosing);
            this.groupBox2.Controls.Add(this.label28);
            this.groupBox2.Controls.Add(this.label27);
            this.groupBox2.Controls.Add(this.label26);
            this.groupBox2.Controls.Add(this.label25);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(20, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(472, 96);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Closing Hours";
            // 
            // textBoxClosingGrace
            // 
            this.textBoxClosingGrace.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "CloseGrace", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxClosingGrace.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxClosingGrace.Location = new System.Drawing.Point(143, 59);
            this.textBoxClosingGrace.Name = "textBoxClosingGrace";
            this.textBoxClosingGrace.Size = new System.Drawing.Size(114, 24);
            this.textBoxClosingGrace.TabIndex = 7;
            this.textBoxClosingGrace.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.CloseGrace;
            // 
            // textBoxClosing
            // 
            this.textBoxClosing.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "CloseTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxClosing.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxClosing.Location = new System.Drawing.Point(143, 27);
            this.textBoxClosing.Name = "textBoxClosing";
            this.textBoxClosing.Size = new System.Drawing.Size(114, 24);
            this.textBoxClosing.TabIndex = 7;
            this.textBoxClosing.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.CloseTime;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.Location = new System.Drawing.Point(263, 61);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(157, 18);
            this.label28.TabIndex = 6;
            this.label28.Text = "{Hours : Min [hh:mm] }";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(28, 61);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(103, 18);
            this.label27.TabIndex = 6;
            this.label27.Text = "Closing Grace";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(263, 29);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(157, 18);
            this.label26.TabIndex = 6;
            this.label26.Text = "{Hours : Min [hh:mm] }";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(36, 29);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(95, 18);
            this.label25.TabIndex = 6;
            this.label25.Text = "Closing Time";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxResumptionGrace);
            this.groupBox1.Controls.Add(this.textBoxResumption);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.label23);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(20, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(473, 96);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Resumption Hours";
            // 
            // textBoxResumptionGrace
            // 
            this.textBoxResumptionGrace.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "ResumeGrace", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxResumptionGrace.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxResumptionGrace.Location = new System.Drawing.Point(152, 60);
            this.textBoxResumptionGrace.Name = "textBoxResumptionGrace";
            this.textBoxResumptionGrace.Size = new System.Drawing.Size(114, 24);
            this.textBoxResumptionGrace.TabIndex = 7;
            this.textBoxResumptionGrace.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.ResumeGrace;
            // 
            // textBoxResumption
            // 
            this.textBoxResumption.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "ResumeTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxResumption.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxResumption.Location = new System.Drawing.Point(152, 28);
            this.textBoxResumption.Name = "textBoxResumption";
            this.textBoxResumption.Size = new System.Drawing.Size(114, 24);
            this.textBoxResumption.TabIndex = 7;
            this.textBoxResumption.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.ResumeTime;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(272, 62);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(157, 18);
            this.label24.TabIndex = 6;
            this.label24.Text = "{Hours : Min [hh:mm] }";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(7, 62);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(133, 18);
            this.label23.TabIndex = 6;
            this.label23.Text = "Resumption Grace";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(272, 30);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(157, 18);
            this.label22.TabIndex = 6;
            this.label22.Text = "{Hours : Min [hh:mm] }";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(15, 30);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(125, 18);
            this.label21.TabIndex = 6;
            this.label21.Text = "Resumption Time";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.textBox8);
            this.tabPage5.Controls.Add(this.label17);
            this.tabPage5.Controls.Add(this.textBoxSetupPassword);
            this.tabPage5.Controls.Add(this.label3);
            this.tabPage5.Location = new System.Drawing.Point(4, 27);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(932, 356);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Terminal";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(158, 63);
            this.textBox8.Name = "textBox8";
            this.textBox8.PasswordChar = '*';
            this.textBox8.Size = new System.Drawing.Size(220, 24);
            this.textBox8.TabIndex = 19;
            this.textBox8.Text = "magicfinger";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(19, 66);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(132, 18);
            this.label17.TabIndex = 18;
            this.label17.Text = "Confirm Password";
            // 
            // textBoxSetupPassword
            // 
            this.textBoxSetupPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "SetupPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxSetupPassword.Location = new System.Drawing.Point(158, 27);
            this.textBoxSetupPassword.Name = "textBoxSetupPassword";
            this.textBoxSetupPassword.PasswordChar = '*';
            this.textBoxSetupPassword.Size = new System.Drawing.Size(220, 24);
            this.textBoxSetupPassword.TabIndex = 17;
            this.textBoxSetupPassword.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.SetupPassword;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 18);
            this.label3.TabIndex = 16;
            this.label3.Text = "Setup Password";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox5);
            this.tabPage4.Controls.Add(this.groupBox4);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 27);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(932, 356);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Data Management";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBoxSyncTime);
            this.groupBox5.Controls.Add(this.label38);
            this.groupBox5.Controls.Add(this.label36);
            this.groupBox5.Controls.Add(this.textBox7);
            this.groupBox5.Controls.Add(this.label34);
            this.groupBox5.Controls.Add(this.buttonTestSyncDBServer);
            this.groupBox5.Controls.Add(this.checkBoxSyncOverWebService);
            this.groupBox5.Controls.Add(this.textBox4);
            this.groupBox5.Controls.Add(this.checkEnableSync);
            this.groupBox5.Controls.Add(this.textBox5);
            this.groupBox5.Controls.Add(this.textBoxSynchServer);
            this.groupBox5.Controls.Add(this.textBox6);
            this.groupBox5.Controls.Add(this.label35);
            this.groupBox5.Controls.Add(this.label33);
            this.groupBox5.Controls.Add(this.label31);
            this.groupBox5.Controls.Add(this.label32);
            this.groupBox5.Location = new System.Drawing.Point(9, 17);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(816, 128);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Data Sync Settings";
            // 
            // textBoxSyncTime
            // 
            this.textBoxSyncTime.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "SyncTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxSyncTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSyncTime.Location = new System.Drawing.Point(111, 49);
            this.textBoxSyncTime.Name = "textBoxSyncTime";
            this.textBoxSyncTime.Size = new System.Drawing.Size(114, 24);
            this.textBoxSyncTime.TabIndex = 37;
            this.textBoxSyncTime.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.SyncTime;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.Location = new System.Drawing.Point(231, 56);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(138, 13);
            this.label38.TabIndex = 36;
            this.label38.Text = "Daily {Hours : Min [hh:mm] }";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(29, 52);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(74, 17);
            this.label36.TabIndex = 35;
            this.label36.Text = "Sync Time";
            // 
            // textBox7
            // 
            this.textBox7.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "PushServerDBPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox7.Location = new System.Drawing.Point(686, 17);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(111, 23);
            this.textBox7.TabIndex = 34;
            this.textBox7.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.PushServerDBPort;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(622, 20);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(57, 17);
            this.label34.TabIndex = 33;
            this.label34.Text = "DB Port";
            // 
            // buttonTestSyncDBServer
            // 
            this.buttonTestSyncDBServer.Location = new System.Drawing.Point(641, 82);
            this.buttonTestSyncDBServer.Name = "buttonTestSyncDBServer";
            this.buttonTestSyncDBServer.Size = new System.Drawing.Size(156, 30);
            this.buttonTestSyncDBServer.TabIndex = 32;
            this.buttonTestSyncDBServer.Text = "Test Connection";
            this.buttonTestSyncDBServer.UseVisualStyleBackColor = true;
            this.buttonTestSyncDBServer.Click += new System.EventHandler(this.buttonTestSyncDBServer_Click);
            // 
            // checkBoxSyncOverWebService
            // 
            this.checkBoxSyncOverWebService.AutoSize = true;
            this.checkBoxSyncOverWebService.Checked = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.SyncOverWebService;
            this.checkBoxSyncOverWebService.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "SyncOverWebService", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxSyncOverWebService.Enabled = false;
            this.checkBoxSyncOverWebService.Location = new System.Drawing.Point(185, 82);
            this.checkBoxSyncOverWebService.Name = "checkBoxSyncOverWebService";
            this.checkBoxSyncOverWebService.Size = new System.Drawing.Size(177, 21);
            this.checkBoxSyncOverWebService.TabIndex = 3;
            this.checkBoxSyncOverWebService.Text = "Sync Over Web Service";
            this.checkBoxSyncOverWebService.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "PushServerDBPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox4.Location = new System.Drawing.Point(686, 49);
            this.textBox4.Name = "textBox4";
            this.textBox4.PasswordChar = '*';
            this.textBox4.Size = new System.Drawing.Size(111, 23);
            this.textBox4.TabIndex = 27;
            this.textBox4.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.PushServerDBPassword;
            // 
            // checkEnableSync
            // 
            this.checkEnableSync.AutoSize = true;
            this.checkEnableSync.Checked = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.EnableSync;
            this.checkEnableSync.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkEnableSync.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "EnableSync", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkEnableSync.Location = new System.Drawing.Point(21, 82);
            this.checkEnableSync.Name = "checkEnableSync";
            this.checkEnableSync.Size = new System.Drawing.Size(139, 21);
            this.checkEnableSync.TabIndex = 2;
            this.checkEnableSync.Text = "Enable Auto Sync";
            this.checkEnableSync.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            this.textBox5.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "PushServerDBUser", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox5.Location = new System.Drawing.Point(466, 48);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(110, 23);
            this.textBox5.TabIndex = 28;
            this.textBox5.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.PushServerDBUser;
            // 
            // textBoxSynchServer
            // 
            this.textBoxSynchServer.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "PushServer", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxSynchServer.Location = new System.Drawing.Point(111, 20);
            this.textBoxSynchServer.Name = "textBoxSynchServer";
            this.textBoxSynchServer.Size = new System.Drawing.Size(114, 23);
            this.textBoxSynchServer.TabIndex = 31;
            this.textBoxSynchServer.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.PushServer;
            // 
            // textBox6
            // 
            this.textBox6.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "PushServerDB", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox6.Location = new System.Drawing.Point(466, 16);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(110, 23);
            this.textBox6.TabIndex = 29;
            this.textBox6.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.PushServerDB;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(18, 23);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(85, 17);
            this.label35.TabIndex = 23;
            this.label35.Text = "Sync Server";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(434, 19);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(27, 17);
            this.label33.TabIndex = 24;
            this.label33.Text = "DB";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(587, 51);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(92, 17);
            this.label31.TabIndex = 26;
            this.label31.Text = "DB Password";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(398, 51);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(61, 17);
            this.label32.TabIndex = 25;
            this.label32.Text = "DB User";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonAttendanceIE);
            this.groupBox4.Controls.Add(this.buttonUserIE);
            this.groupBox4.Controls.Add(this.buttonSyncControl);
            this.groupBox4.Controls.Add(this.buttonReport);
            this.groupBox4.Location = new System.Drawing.Point(10, 151);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(815, 149);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Tools";
            // 
            // buttonAttendanceIE
            // 
            this.buttonAttendanceIE.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAttendanceIE.Location = new System.Drawing.Point(279, 23);
            this.buttonAttendanceIE.Name = "buttonAttendanceIE";
            this.buttonAttendanceIE.Size = new System.Drawing.Size(251, 36);
            this.buttonAttendanceIE.TabIndex = 1;
            this.buttonAttendanceIE.Text = "Import/Export Attendance Data...";
            this.buttonAttendanceIE.UseVisualStyleBackColor = true;
            this.buttonAttendanceIE.Click += new System.EventHandler(this.buttonAttendanceIE_Click);
            // 
            // buttonUserIE
            // 
            this.buttonUserIE.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUserIE.Location = new System.Drawing.Point(545, 23);
            this.buttonUserIE.Name = "buttonUserIE";
            this.buttonUserIE.Size = new System.Drawing.Size(251, 36);
            this.buttonUserIE.TabIndex = 1;
            this.buttonUserIE.Text = "Import/Export User Data...";
            this.buttonUserIE.UseVisualStyleBackColor = true;
            // 
            // buttonSyncControl
            // 
            this.buttonSyncControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSyncControl.Location = new System.Drawing.Point(15, 77);
            this.buttonSyncControl.Name = "buttonSyncControl";
            this.buttonSyncControl.Size = new System.Drawing.Size(251, 36);
            this.buttonSyncControl.TabIndex = 32;
            this.buttonSyncControl.Text = "Upload Logs to server...";
            this.buttonSyncControl.UseVisualStyleBackColor = true;
            this.buttonSyncControl.Click += new System.EventHandler(this.buttonSyncControl_Click);
            // 
            // buttonReport
            // 
            this.buttonReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonReport.Location = new System.Drawing.Point(15, 23);
            this.buttonReport.Name = "buttonReport";
            this.buttonReport.Size = new System.Drawing.Size(251, 36);
            this.buttonReport.TabIndex = 1;
            this.buttonReport.Text = "Reporting...";
            this.buttonReport.UseVisualStyleBackColor = true;
            this.buttonReport.Click += new System.EventHandler(this.buttonReport_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(4, 27);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(932, 356);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Exception Management";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.groupBox6);
            this.tabPage7.Location = new System.Drawing.Point(4, 27);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(932, 356);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "Customised Fields";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox10);
            this.groupBox6.Controls.Add(this.groupBox9);
            this.groupBox6.Controls.Add(this.groupBox8);
            this.groupBox6.Controls.Add(this.groupBox7);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(29, 20);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(819, 286);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Custom User Records Display";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.checkBoxEnableCData4);
            this.groupBox10.Controls.Add(this.comboBoxCData4Picker);
            this.groupBox10.Controls.Add(this.label44);
            this.groupBox10.Controls.Add(this.textBoxCDataLabel4);
            this.groupBox10.Controls.Add(this.label45);
            this.groupBox10.Controls.Add(this.textBoxCDataField4);
            this.groupBox10.Location = new System.Drawing.Point(407, 156);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(406, 121);
            this.groupBox10.TabIndex = 4;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Custom Field 4";
            // 
            // checkBoxEnableCData4
            // 
            this.checkBoxEnableCData4.AutoSize = true;
            this.checkBoxEnableCData4.Checked = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.EnableCData4;
            this.checkBoxEnableCData4.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "EnableCData4", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxEnableCData4.Location = new System.Drawing.Point(17, 21);
            this.checkBoxEnableCData4.Name = "checkBoxEnableCData4";
            this.checkBoxEnableCData4.Size = new System.Drawing.Size(153, 19);
            this.checkBoxEnableCData4.TabIndex = 4;
            this.checkBoxEnableCData4.Text = "Show custom field data";
            this.checkBoxEnableCData4.UseVisualStyleBackColor = true;
            // 
            // comboBoxCData4Picker
            // 
            this.comboBoxCData4Picker.FormattingEnabled = true;
            this.comboBoxCData4Picker.Location = new System.Drawing.Point(52, 50);
            this.comboBoxCData4Picker.Name = "comboBoxCData4Picker";
            this.comboBoxCData4Picker.Size = new System.Drawing.Size(348, 23);
            this.comboBoxCData4Picker.TabIndex = 3;
            this.comboBoxCData4Picker.SelectedIndexChanged += new System.EventHandler(this.comboBoxCData1Picker_SelectedIndexChanged);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(11, 88);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(38, 15);
            this.label44.TabIndex = 2;
            this.label44.Text = "Label";
            // 
            // textBoxCDataLabel4
            // 
            this.textBoxCDataLabel4.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "CDataLabel4", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxCDataLabel4.Location = new System.Drawing.Point(52, 84);
            this.textBoxCDataLabel4.Multiline = true;
            this.textBoxCDataLabel4.Name = "textBoxCDataLabel4";
            this.textBoxCDataLabel4.Size = new System.Drawing.Size(348, 24);
            this.textBoxCDataLabel4.TabIndex = 1;
            this.textBoxCDataLabel4.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.CDataLabel4;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(14, 53);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(34, 15);
            this.label45.TabIndex = 2;
            this.label45.Text = "Field";
            // 
            // textBoxCDataField4
            // 
            this.textBoxCDataField4.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "CDataField4", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxCDataField4.Location = new System.Drawing.Point(176, 20);
            this.textBoxCDataField4.Multiline = true;
            this.textBoxCDataField4.Name = "textBoxCDataField4";
            this.textBoxCDataField4.ReadOnly = true;
            this.textBoxCDataField4.Size = new System.Drawing.Size(224, 23);
            this.textBoxCDataField4.TabIndex = 1;
            this.textBoxCDataField4.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.CDataField4;
            this.textBoxCDataField4.Visible = false;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.checkBoxEnableCData3);
            this.groupBox9.Controls.Add(this.comboBoxCData3Picker);
            this.groupBox9.Controls.Add(this.label42);
            this.groupBox9.Controls.Add(this.textBoxCDataLabel3);
            this.groupBox9.Controls.Add(this.label43);
            this.groupBox9.Controls.Add(this.textBoxCDataField3);
            this.groupBox9.Location = new System.Drawing.Point(13, 156);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(388, 121);
            this.groupBox9.TabIndex = 4;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Custom Field 3";
            // 
            // checkBoxEnableCData3
            // 
            this.checkBoxEnableCData3.AutoSize = true;
            this.checkBoxEnableCData3.Checked = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.EnableCData3;
            this.checkBoxEnableCData3.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "EnableCData3", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxEnableCData3.Location = new System.Drawing.Point(17, 21);
            this.checkBoxEnableCData3.Name = "checkBoxEnableCData3";
            this.checkBoxEnableCData3.Size = new System.Drawing.Size(153, 19);
            this.checkBoxEnableCData3.TabIndex = 4;
            this.checkBoxEnableCData3.Text = "Show custom field data";
            this.checkBoxEnableCData3.UseVisualStyleBackColor = true;
            // 
            // comboBoxCData3Picker
            // 
            this.comboBoxCData3Picker.FormattingEnabled = true;
            this.comboBoxCData3Picker.Location = new System.Drawing.Point(52, 50);
            this.comboBoxCData3Picker.Name = "comboBoxCData3Picker";
            this.comboBoxCData3Picker.Size = new System.Drawing.Size(330, 23);
            this.comboBoxCData3Picker.TabIndex = 3;
            this.comboBoxCData3Picker.SelectedIndexChanged += new System.EventHandler(this.comboBoxCData1Picker_SelectedIndexChanged);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(11, 88);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(38, 15);
            this.label42.TabIndex = 2;
            this.label42.Text = "Label";
            // 
            // textBoxCDataLabel3
            // 
            this.textBoxCDataLabel3.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "CDataLabel3", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxCDataLabel3.Location = new System.Drawing.Point(52, 84);
            this.textBoxCDataLabel3.Multiline = true;
            this.textBoxCDataLabel3.Name = "textBoxCDataLabel3";
            this.textBoxCDataLabel3.Size = new System.Drawing.Size(330, 24);
            this.textBoxCDataLabel3.TabIndex = 1;
            this.textBoxCDataLabel3.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.CDataLabel3;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(14, 53);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(34, 15);
            this.label43.TabIndex = 2;
            this.label43.Text = "Field";
            // 
            // textBoxCDataField3
            // 
            this.textBoxCDataField3.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "CDataField3", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxCDataField3.Location = new System.Drawing.Point(176, 20);
            this.textBoxCDataField3.Multiline = true;
            this.textBoxCDataField3.Name = "textBoxCDataField3";
            this.textBoxCDataField3.ReadOnly = true;
            this.textBoxCDataField3.Size = new System.Drawing.Size(206, 23);
            this.textBoxCDataField3.TabIndex = 1;
            this.textBoxCDataField3.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.CDataField3;
            this.textBoxCDataField3.Visible = false;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.checkBoxEnableCData2);
            this.groupBox8.Controls.Add(this.comboBoxCData2Picker);
            this.groupBox8.Controls.Add(this.label37);
            this.groupBox8.Controls.Add(this.textBoxCDataLabel2);
            this.groupBox8.Controls.Add(this.label41);
            this.groupBox8.Controls.Add(this.textBoxCDataField2);
            this.groupBox8.Location = new System.Drawing.Point(407, 20);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(406, 121);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Custom Field 2";
            // 
            // checkBoxEnableCData2
            // 
            this.checkBoxEnableCData2.AutoSize = true;
            this.checkBoxEnableCData2.Checked = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.EnableCData2;
            this.checkBoxEnableCData2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "EnableCData2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxEnableCData2.Location = new System.Drawing.Point(17, 21);
            this.checkBoxEnableCData2.Name = "checkBoxEnableCData2";
            this.checkBoxEnableCData2.Size = new System.Drawing.Size(153, 19);
            this.checkBoxEnableCData2.TabIndex = 4;
            this.checkBoxEnableCData2.Text = "Show custom field data";
            this.checkBoxEnableCData2.UseVisualStyleBackColor = true;
            // 
            // comboBoxCData2Picker
            // 
            this.comboBoxCData2Picker.FormattingEnabled = true;
            this.comboBoxCData2Picker.Location = new System.Drawing.Point(52, 50);
            this.comboBoxCData2Picker.Name = "comboBoxCData2Picker";
            this.comboBoxCData2Picker.Size = new System.Drawing.Size(348, 23);
            this.comboBoxCData2Picker.TabIndex = 3;
            this.comboBoxCData2Picker.SelectedIndexChanged += new System.EventHandler(this.comboBoxCData1Picker_SelectedIndexChanged);
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(11, 88);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(38, 15);
            this.label37.TabIndex = 2;
            this.label37.Text = "Label";
            // 
            // textBoxCDataLabel2
            // 
            this.textBoxCDataLabel2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "CDataLabel2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxCDataLabel2.Location = new System.Drawing.Point(52, 84);
            this.textBoxCDataLabel2.Multiline = true;
            this.textBoxCDataLabel2.Name = "textBoxCDataLabel2";
            this.textBoxCDataLabel2.Size = new System.Drawing.Size(348, 24);
            this.textBoxCDataLabel2.TabIndex = 1;
            this.textBoxCDataLabel2.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.CDataLabel2;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(14, 53);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(34, 15);
            this.label41.TabIndex = 2;
            this.label41.Text = "Field";
            // 
            // textBoxCDataField2
            // 
            this.textBoxCDataField2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "CDataField2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxCDataField2.Location = new System.Drawing.Point(176, 20);
            this.textBoxCDataField2.Multiline = true;
            this.textBoxCDataField2.Name = "textBoxCDataField2";
            this.textBoxCDataField2.ReadOnly = true;
            this.textBoxCDataField2.Size = new System.Drawing.Size(224, 23);
            this.textBoxCDataField2.TabIndex = 1;
            this.textBoxCDataField2.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.CDataField2;
            this.textBoxCDataField2.Visible = false;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.checkBoxEnableCData1);
            this.groupBox7.Controls.Add(this.comboBoxCData1Picker);
            this.groupBox7.Controls.Add(this.label40);
            this.groupBox7.Controls.Add(this.textBoxCDataLabel1);
            this.groupBox7.Controls.Add(this.label39);
            this.groupBox7.Controls.Add(this.textBoxCDataField1);
            this.groupBox7.Location = new System.Drawing.Point(13, 20);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(388, 121);
            this.groupBox7.TabIndex = 4;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Custom Field 1";
            // 
            // checkBoxEnableCData1
            // 
            this.checkBoxEnableCData1.AutoSize = true;
            this.checkBoxEnableCData1.Checked = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.EnableCData1;
            this.checkBoxEnableCData1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "EnableCData1", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxEnableCData1.Location = new System.Drawing.Point(17, 21);
            this.checkBoxEnableCData1.Name = "checkBoxEnableCData1";
            this.checkBoxEnableCData1.Size = new System.Drawing.Size(153, 19);
            this.checkBoxEnableCData1.TabIndex = 4;
            this.checkBoxEnableCData1.Text = "Show custom field data";
            this.checkBoxEnableCData1.UseVisualStyleBackColor = true;
            // 
            // comboBoxCData1Picker
            // 
            this.comboBoxCData1Picker.FormattingEnabled = true;
            this.comboBoxCData1Picker.Location = new System.Drawing.Point(52, 50);
            this.comboBoxCData1Picker.Name = "comboBoxCData1Picker";
            this.comboBoxCData1Picker.Size = new System.Drawing.Size(330, 23);
            this.comboBoxCData1Picker.TabIndex = 3;
            this.comboBoxCData1Picker.SelectedIndexChanged += new System.EventHandler(this.comboBoxCData1Picker_SelectedIndexChanged);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(11, 88);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(38, 15);
            this.label40.TabIndex = 2;
            this.label40.Text = "Label";
            // 
            // textBoxCDataLabel1
            // 
            this.textBoxCDataLabel1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "CDataLabel1", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxCDataLabel1.Location = new System.Drawing.Point(52, 84);
            this.textBoxCDataLabel1.Multiline = true;
            this.textBoxCDataLabel1.Name = "textBoxCDataLabel1";
            this.textBoxCDataLabel1.Size = new System.Drawing.Size(330, 24);
            this.textBoxCDataLabel1.TabIndex = 1;
            this.textBoxCDataLabel1.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.CDataLabel1;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(14, 53);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(34, 15);
            this.label39.TabIndex = 2;
            this.label39.Text = "Field";
            // 
            // textBoxCDataField1
            // 
            this.textBoxCDataField1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "CDataField1", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxCDataField1.Location = new System.Drawing.Point(176, 20);
            this.textBoxCDataField1.Multiline = true;
            this.textBoxCDataField1.Name = "textBoxCDataField1";
            this.textBoxCDataField1.ReadOnly = true;
            this.textBoxCDataField1.Size = new System.Drawing.Size(206, 23);
            this.textBoxCDataField1.TabIndex = 1;
            this.textBoxCDataField1.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.CDataField1;
            this.textBoxCDataField1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(946, 78);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.Location = new System.Drawing.Point(17, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(44, 42);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(67, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Settings";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.buttonShutDown);
            this.panel2.Controls.Add(this.buttonClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 480);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(946, 42);
            this.panel2.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(3, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 36);
            this.button2.TabIndex = 0;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonShutDown
            // 
            this.buttonShutDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonShutDown.BackColor = System.Drawing.Color.Red;
            this.buttonShutDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonShutDown.ForeColor = System.Drawing.Color.White;
            this.buttonShutDown.Location = new System.Drawing.Point(724, 3);
            this.buttonShutDown.Name = "buttonShutDown";
            this.buttonShutDown.Size = new System.Drawing.Size(217, 36);
            this.buttonShutDown.TabIndex = 0;
            this.buttonShutDown.Text = "Shut Down Application";
            this.buttonShutDown.UseVisualStyleBackColor = false;
            this.buttonShutDown.Click += new System.EventHandler(this.buttonShutDown_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClose.Location = new System.Drawing.Point(102, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(88, 36);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "banner.png";
            this.openFileDialog1.Filter = "\"Image files|*.png\"";
            this.openFileDialog1.Title = "Select Application Branding Image";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGreen;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tabControl1);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(946, 522);
            this.Load += new System.EventHandler(this.SettingsControl_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button buttonShutDown;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.TextBox textBoxDBName;
        private System.Windows.Forms.TextBox textBoxDBPort;
        private System.Windows.Forms.TextBox textBoxDBServer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxDBSource;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label labelRecordCount;
        private System.Windows.Forms.Label labelTemplateCount;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxMessageDuration;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button buttonReport;
        private System.Windows.Forms.Button buttonAttendanceIE;
        private System.Windows.Forms.Button buttonUserIE;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxClosingGrace;
        private System.Windows.Forms.TextBox textBoxClosing;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxResumptionGrace;
        private System.Windows.Forms.TextBox textBoxResumption;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBoxSun;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox AppBrand;
        private System.Windows.Forms.Button buttonBrosweImage;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.CheckBox checkEnableSync;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBoxSyncOverWebService;
        private System.Windows.Forms.Button buttonTestSyncDBServer;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBoxSynchServer;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Button buttonSyncControl;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TextBox textBoxSyncTime;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ComboBox comboBoxCData1Picker;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.TextBox textBoxCDataLabel1;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TextBox textBoxCDataField1;
        private System.Windows.Forms.CheckBox checkBoxEnableCData1;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.CheckBox checkBoxEnableCData4;
        private System.Windows.Forms.ComboBox comboBoxCData4Picker;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox textBoxCDataLabel4;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.TextBox textBoxCDataField4;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.CheckBox checkBoxEnableCData3;
        private System.Windows.Forms.ComboBox comboBoxCData3Picker;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.TextBox textBoxCDataLabel3;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TextBox textBoxCDataField3;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckBox checkBoxEnableCData2;
        private System.Windows.Forms.ComboBox comboBoxCData2Picker;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox textBoxCDataLabel2;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.TextBox textBoxCDataField2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxSetupPassword;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.CheckBox checkBoxVerifyField;
    }
}
