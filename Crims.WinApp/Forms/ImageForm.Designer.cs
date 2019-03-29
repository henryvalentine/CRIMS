namespace Crims.UI.Win.Enroll.Forms
{
    partial class ImageForm
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxCameras = new System.Windows.Forms.ComboBox();
            this.btCaptureDevicePhoto = new System.Windows.Forms.Button();
            this.buttonUseImage = new System.Windows.Forms.Button();
            this.buttonStopCapture = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.target = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.target)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.target);
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(630, 515);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 543);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Active Device:";
            // 
            // comboBoxCameras
            // 
            this.comboBoxCameras.FormattingEnabled = true;
            this.comboBoxCameras.Location = new System.Drawing.Point(110, 539);
            this.comboBoxCameras.Name = "comboBoxCameras";
            this.comboBoxCameras.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCameras.TabIndex = 2;
            this.comboBoxCameras.SelectedIndexChanged += new System.EventHandler(this.comboBoxCameras_SelectedIndexChanged);
            // 
            // btCaptureDevicePhoto
            // 
            this.btCaptureDevicePhoto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCaptureDevicePhoto.Location = new System.Drawing.Point(246, 534);
            this.btCaptureDevicePhoto.Name = "btCaptureDevicePhoto";
            this.btCaptureDevicePhoto.Size = new System.Drawing.Size(93, 30);
            this.btCaptureDevicePhoto.TabIndex = 3;
            this.btCaptureDevicePhoto.Text = "Start Camera";
            this.btCaptureDevicePhoto.UseVisualStyleBackColor = true;
            this.btCaptureDevicePhoto.Click += new System.EventHandler(this.btCaptureDevicePhoto_Click);
            // 
            // buttonUseImage
            // 
            this.buttonUseImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUseImage.Location = new System.Drawing.Point(477, 534);
            this.buttonUseImage.Name = "buttonUseImage";
            this.buttonUseImage.Size = new System.Drawing.Size(93, 30);
            this.buttonUseImage.TabIndex = 4;
            this.buttonUseImage.Text = "Use Image";
            this.buttonUseImage.UseVisualStyleBackColor = true;
            this.buttonUseImage.Click += new System.EventHandler(this.buttonUseImage_Click);
            // 
            // buttonStopCapture
            // 
            this.buttonStopCapture.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStopCapture.Location = new System.Drawing.Point(360, 534);
            this.buttonStopCapture.Name = "buttonStopCapture";
            this.buttonStopCapture.Size = new System.Drawing.Size(93, 30);
            this.buttonStopCapture.TabIndex = 5;
            this.buttonStopCapture.Text = "Take Photo";
            this.buttonStopCapture.UseVisualStyleBackColor = true;
            this.buttonStopCapture.Click += new System.EventHandler(this.buttonStopCapture_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.Color.Black;
            this.linkLabel1.Location = new System.Drawing.Point(592, 543);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(24, 13);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Exit";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked_1);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // target
            // 
            this.target.Location = new System.Drawing.Point(6, 16);
            this.target.Name = "target";
            this.target.Size = new System.Drawing.Size(620, 480);
            this.target.TabIndex = 0;
            this.target.TabStop = false;
            // 
            // ImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(642, 568);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.buttonStopCapture);
            this.Controls.Add(this.buttonUseImage);
            this.Controls.Add(this.btCaptureDevicePhoto);
            this.Controls.Add(this.comboBoxCameras);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "ImageForm";
            this.Text = "Face Capture";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ImageForm_FormClosed);
            this.Load += new System.EventHandler(this.ImageForm_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.target)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox target;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxCameras;
        private System.Windows.Forms.Button btCaptureDevicePhoto;
        private System.Windows.Forms.Button buttonUseImage;
        private System.Windows.Forms.Button buttonStopCapture;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Timer timer;
    }
}