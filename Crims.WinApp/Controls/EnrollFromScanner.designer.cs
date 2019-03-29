using Crims.UI.Win.Enroll.Controls;

namespace Crims.Win.Enroll.Controls
{
	partial class EnrollFromScanner
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
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.lblFingerprintScanCount = new System.Windows.Forms.Label();
            this.lblfing = new System.Windows.Forms.Label();
            this.lblQuality = new System.Windows.Forms.Label();
            this.saveImageButton = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.scannersGroupBox = new System.Windows.Forms.GroupBox();
            this.refreshListButton = new System.Windows.Forms.Button();
            this.scanButton = new System.Windows.Forms.Button();
            this.scannersListBox = new System.Windows.Forms.ListBox();
            this.saveTemplateButton = new System.Windows.Forms.Button();
            this.licensePanel1 = new Crims.UI.Win.Enroll.Controls.LicensePanel();
            this.scannersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFingerprintScanCount
            // 
            this.lblFingerprintScanCount.AutoSize = true;
            this.lblFingerprintScanCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFingerprintScanCount.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblFingerprintScanCount.Location = new System.Drawing.Point(71, 409);
            this.lblFingerprintScanCount.Name = "lblFingerprintScanCount";
            this.lblFingerprintScanCount.Size = new System.Drawing.Size(15, 15);
            this.lblFingerprintScanCount.TabIndex = 17;
            this.lblFingerprintScanCount.Text = "0";
            // 
            // lblfing
            // 
            this.lblfing.AutoSize = true;
            this.lblfing.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblfing.Location = new System.Drawing.Point(6, 409);
            this.lblfing.Name = "lblfing";
            this.lblfing.Size = new System.Drawing.Size(65, 13);
            this.lblfing.TabIndex = 16;
            this.lblfing.Text = "Scanned :";
            // 
            // lblQuality
            // 
            this.lblQuality.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQuality.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblQuality.Location = new System.Drawing.Point(3, 381);
            this.lblQuality.Name = "lblQuality";
            this.lblQuality.Size = new System.Drawing.Size(454, 20);
            this.lblQuality.TabIndex = 13;
            // 
            // saveImageButton
            // 
            this.saveImageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveImageButton.Enabled = false;
            this.saveImageButton.Location = new System.Drawing.Point(249, 404);
            this.saveImageButton.Name = "saveImageButton";
            this.saveImageButton.Size = new System.Drawing.Size(97, 23);
            this.saveImageButton.TabIndex = 12;
            this.saveImageButton.Text = "Save &Image";
            this.saveImageButton.UseVisualStyleBackColor = true;
            this.saveImageButton.Visible = false;
            this.saveImageButton.Click += new System.EventHandler(this.SaveImageButtonClick);
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Location = new System.Drawing.Point(3, 125);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(454, 193);
            this.panel.TabIndex = 9;
            // 
            // scannersGroupBox
            // 
            this.scannersGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scannersGroupBox.Controls.Add(this.refreshListButton);
            this.scannersGroupBox.Controls.Add(this.scanButton);
            this.scannersGroupBox.Controls.Add(this.scannersListBox);
            this.scannersGroupBox.Location = new System.Drawing.Point(3, 3);
            this.scannersGroupBox.Name = "scannersGroupBox";
            this.scannersGroupBox.Size = new System.Drawing.Size(454, 115);
            this.scannersGroupBox.TabIndex = 11;
            this.scannersGroupBox.TabStop = false;
            this.scannersGroupBox.Text = "Scanners list";
            // 
            // refreshListButton
            // 
            this.refreshListButton.Location = new System.Drawing.Point(6, 80);
            this.refreshListButton.Name = "refreshListButton";
            this.refreshListButton.Size = new System.Drawing.Size(75, 23);
            this.refreshListButton.TabIndex = 10;
            this.refreshListButton.Text = "Refresh list";
            this.refreshListButton.UseVisualStyleBackColor = true;
            this.refreshListButton.Click += new System.EventHandler(this.RefreshListButtonClick);
            // 
            // scanButton
            // 
            this.scanButton.Location = new System.Drawing.Point(193, 80);
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(75, 23);
            this.scanButton.TabIndex = 9;
            this.scanButton.Text = "Scan";
            this.scanButton.UseVisualStyleBackColor = true;
            this.scanButton.Click += new System.EventHandler(this.ScanButtonClick);
            // 
            // scannersListBox
            // 
            this.scannersListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scannersListBox.Location = new System.Drawing.Point(4, 19);
            this.scannersListBox.Name = "scannersListBox";
            this.scannersListBox.Size = new System.Drawing.Size(444, 56);
            this.scannersListBox.TabIndex = 6;
            this.scannersListBox.SelectedIndexChanged += new System.EventHandler(this.scannersListBox_SelectedIndexChanged);
            // 
            // saveTemplateButton
            // 
            this.saveTemplateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveTemplateButton.Enabled = false;
            this.saveTemplateButton.Location = new System.Drawing.Point(352, 404);
            this.saveTemplateButton.Name = "saveTemplateButton";
            this.saveTemplateButton.Size = new System.Drawing.Size(97, 23);
            this.saveTemplateButton.TabIndex = 10;
            this.saveTemplateButton.Text = "Save t&emplate";
            this.saveTemplateButton.UseVisualStyleBackColor = true;
            this.saveTemplateButton.Visible = false;
            this.saveTemplateButton.Click += new System.EventHandler(this.SaveTemplateButtonClick);
            // 
            // licensePanel1
            // 
            this.licensePanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.licensePanel1.Location = new System.Drawing.Point(3, 324);
            this.licensePanel1.Name = "licensePanel1";
            this.licensePanel1.OptionalComponents = "Images.WSQ";
            this.licensePanel1.RequiredComponents = "Biometrics.FingerExtraction,Devices.FingerScanners";
            this.licensePanel1.Size = new System.Drawing.Size(445, 50);
            this.licensePanel1.TabIndex = 0;
            // 
            // EnrollFromScanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.licensePanel1);
            this.Controls.Add(this.lblFingerprintScanCount);
            this.Controls.Add(this.lblfing);
            this.Controls.Add(this.lblQuality);
            this.Controls.Add(this.saveImageButton);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.scannersGroupBox);
            this.Controls.Add(this.saveTemplateButton);
            this.Name = "EnrollFromScanner";
            this.Size = new System.Drawing.Size(460, 436);
            this.Load += new System.EventHandler(this.EnrollFromScannerLoad);
            this.scannersGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblQuality;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Button saveImageButton;
		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.Button refreshListButton;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.Button scanButton;
		private System.Windows.Forms.GroupBox scannersGroupBox;
		private System.Windows.Forms.ListBox scannersListBox;
		private System.Windows.Forms.Button saveTemplateButton;
        private LicensePanel licensePanel1;
        private System.Windows.Forms.Label lblFingerprintScanCount;
        private System.Windows.Forms.Label lblfing;
    }
}
