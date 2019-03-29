namespace Crims.UI.Win.Enroll.Controls
{
	partial class EnrollFromCamera
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
            this.btnSaveTemplate = new System.Windows.Forms.Button();
            this.btnSaveImage = new System.Windows.Forms.Button();
            this.facesView = new Neurotec.Biometrics.Gui.NFaceView();
            this.cbCamera = new System.Windows.Forms.ComboBox();
            this.btnRefreshList = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnStartExtraction = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.saveTemplateDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pictureBoxCroped = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCroped)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.Enabled = false;
            this.btnSaveTemplate.Location = new System.Drawing.Point(177, 45);
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(105, 21);
            this.btnSaveTemplate.TabIndex = 9;
            this.btnSaveTemplate.Text = "&Save Template";
            this.btnSaveTemplate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveTemplate.UseVisualStyleBackColor = true;
            this.btnSaveTemplate.Visible = false;
            this.btnSaveTemplate.Click += new System.EventHandler(this.BtnSaveTemplateClick);
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.Enabled = false;
            this.btnSaveImage.Location = new System.Drawing.Point(288, 45);
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(79, 21);
            this.btnSaveImage.TabIndex = 10;
            this.btnSaveImage.Text = "Save &Image";
            this.btnSaveImage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveImage.UseVisualStyleBackColor = true;
            this.btnSaveImage.Visible = false;
            this.btnSaveImage.Click += new System.EventHandler(this.BtnSaveImageClick);
            // 
            // facesView
            // 
            this.facesView.AutoScroll = true;
            this.facesView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.facesView.Face = null;
            this.facesView.FaceIds = null;
            this.facesView.IcaoArrowsColor = System.Drawing.Color.Red;
            this.facesView.Location = new System.Drawing.Point(2, 2);
            this.facesView.Name = "facesView";
            this.facesView.ShowEyesConfidence = true;
            this.facesView.ShowIcaoArrows = true;
            this.facesView.ShowMouthConfidence = true;
            this.facesView.ShowNoseConfidence = true;
            this.facesView.ShowTokenImageRectangle = true;
            this.facesView.Size = new System.Drawing.Size(649, 370);
            this.facesView.TabIndex = 13;
            this.facesView.TokenImageRectangleColor = System.Drawing.Color.White;
            this.facesView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.facesView_MouseDown);
            // 
            // cbCamera
            // 
            this.cbCamera.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCamera.FormattingEnabled = true;
            this.cbCamera.Location = new System.Drawing.Point(87, 14);
            this.cbCamera.Name = "cbCamera";
            this.cbCamera.Size = new System.Drawing.Size(581, 21);
            this.cbCamera.TabIndex = 15;
            this.cbCamera.SelectedIndexChanged += new System.EventHandler(this.CbCameraSelectedIndexChanged);
            // 
            // btnRefreshList
            // 
            this.btnRefreshList.Location = new System.Drawing.Point(5, 13);
            this.btnRefreshList.Name = "btnRefreshList";
            this.btnRefreshList.Size = new System.Drawing.Size(75, 21);
            this.btnRefreshList.TabIndex = 17;
            this.btnRefreshList.Text = "Refresh list";
            this.btnRefreshList.UseVisualStyleBackColor = true;
            this.btnRefreshList.Click += new System.EventHandler(this.BtnRefreshListClick);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(7, 45);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(86, 21);
            this.btnStart.TabIndex = 18;
            this.btnStart.Text = "Start capturing";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStartClick);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(98, 45);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(74, 21);
            this.btnStop.TabIndex = 19;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStopClick);
            // 
            // groupBox
            // 
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox.Controls.Add(this.lblStatus);
            this.groupBox.Controls.Add(this.btnStartExtraction);
            this.groupBox.Controls.Add(this.btnStop);
            this.groupBox.Controls.Add(this.buttonCancel);
            this.groupBox.Controls.Add(this.buttonOk);
            this.groupBox.Controls.Add(this.btnSaveImage);
            this.groupBox.Controls.Add(this.cbCamera);
            this.groupBox.Controls.Add(this.btnSaveTemplate);
            this.groupBox.Controls.Add(this.btnStart);
            this.groupBox.Controls.Add(this.btnRefreshList);
            this.groupBox.Location = new System.Drawing.Point(3, 8);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(677, 72);
            this.groupBox.TabIndex = 20;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Cameras";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(316, -8);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(151, 23);
            this.lblStatus.TabIndex = 24;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.Visible = false;
            // 
            // btnStartExtraction
            // 
            this.btnStartExtraction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartExtraction.Enabled = false;
            this.btnStartExtraction.Location = new System.Drawing.Point(396, 45);
            this.btnStartExtraction.Name = "btnStartExtraction";
            this.btnStartExtraction.Size = new System.Drawing.Size(147, 21);
            this.btnStartExtraction.TabIndex = 23;
            this.btnStartExtraction.Text = "Extract Facial Features";
            this.btnStartExtraction.UseVisualStyleBackColor = true;
            this.btnStartExtraction.Click += new System.EventHandler(this.BtnStartExtractionClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(610, 45);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(56, 21);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Close";
            this.buttonCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Enabled = false;
            this.buttonOk.Location = new System.Drawing.Point(549, 45);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(56, 21);
            this.buttonOk.TabIndex = 10;
            this.buttonOk.Text = "OK";
            this.buttonOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Enabled = false;
            this.tabControl1.Location = new System.Drawing.Point(8, 85);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(661, 400);
            this.tabControl1.TabIndex = 25;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.facesView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(653, 374);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Camera View";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pictureBoxCroped);
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(653, 374);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Image Preview";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pictureBoxCroped
            // 
            this.pictureBoxCroped.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxCroped.Location = new System.Drawing.Point(2, 2);
            this.pictureBoxCroped.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxCroped.Name = "pictureBoxCroped";
            this.pictureBoxCroped.Size = new System.Drawing.Size(649, 370);
            this.pictureBoxCroped.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxCroped.TabIndex = 0;
            this.pictureBoxCroped.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Enabled = false;
            this.splitContainer1.Location = new System.Drawing.Point(78, 113);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Size = new System.Drawing.Size(235, 126);
            this.splitContainer1.SplitterDistance = 101;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 1;
            // 
            // EnrollFromCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox);
            this.Name = "EnrollFromCamera";
            this.Size = new System.Drawing.Size(683, 520);
            this.Load += new System.EventHandler(this.EnrollFromCameraLoad);
            this.groupBox.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCroped)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnSaveTemplate;
		private System.Windows.Forms.Button btnSaveImage;
		private Neurotec.Biometrics.Gui.NFaceView facesView;
		private System.Windows.Forms.ComboBox cbCamera;
		private System.Windows.Forms.Button btnRefreshList;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.Button btnStartExtraction;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.SaveFileDialog saveTemplateDialog;
		private System.Windows.Forms.SaveFileDialog saveImageDialog;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBoxCroped;
    }
}
