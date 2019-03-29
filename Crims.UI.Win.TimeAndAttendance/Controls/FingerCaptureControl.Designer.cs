namespace TandAProject.Controls
{
    partial class FingerCaptureControl
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
            this.lblQuality = new System.Windows.Forms.Label();
            this.panel = new System.Windows.Forms.Panel();
            this.scanWorker = new System.ComponentModel.BackgroundWorker();
            this.timerScannerStatus = new System.Windows.Forms.Timer(this.components);
            this.panelId = new System.Windows.Forms.Panel();
            this.textBoxIDNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelId.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblQuality
            // 
            this.lblQuality.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQuality.Location = new System.Drawing.Point(5, 4);
            this.lblQuality.Name = "lblQuality";
            this.lblQuality.Size = new System.Drawing.Size(401, 35);
            this.lblQuality.TabIndex = 18;
            this.lblQuality.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.BackColor = System.Drawing.Color.LightGreen;
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Location = new System.Drawing.Point(6, 93);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(393, 377);
            this.panel.TabIndex = 14;
            // 
            // scanWorker
            // 
            this.scanWorker.WorkerSupportsCancellation = true;
            this.scanWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ScanWorkerDoWork);
            this.scanWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ScanWorkerRunWorkerCompleted);
            // 
            // timerScannerStatus
            // 
            this.timerScannerStatus.Enabled = true;
            this.timerScannerStatus.Interval = 2000;
            this.timerScannerStatus.Tick += new System.EventHandler(this.timerScannerStatus_Tick);
            // 
            // panelId
            // 
            this.panelId.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelId.Controls.Add(this.textBoxIDNumber);
            this.panelId.Controls.Add(this.label1);
            this.panelId.Location = new System.Drawing.Point(11, 42);
            this.panelId.Name = "panelId";
            this.panelId.Size = new System.Drawing.Size(387, 37);
            this.panelId.TabIndex = 19;
            this.panelId.Visible = false;
            // 
            // textBoxIDNumber
            // 
            this.textBoxIDNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxIDNumber.Location = new System.Drawing.Point(145, 5);
            this.textBoxIDNumber.Name = "textBoxIDNumber";
            this.textBoxIDNumber.Size = new System.Drawing.Size(233, 26);
            this.textBoxIDNumber.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter ID Number";
            // 
            // FingerCaptureControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGreen;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panelId);
            this.Controls.Add(this.lblQuality);
            this.Controls.Add(this.panel);
            this.Name = "FingerCaptureControl";
            this.Size = new System.Drawing.Size(404, 478);
            this.Load += new System.EventHandler(this.EnrollFromScannerLoad);
            this.VisibleChanged += new System.EventHandler(this.EnrollFromScannerVisibleChanged);
            this.panelId.ResumeLayout(false);
            this.panelId.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblQuality;
        private System.Windows.Forms.Panel panel;
        private System.ComponentModel.BackgroundWorker scanWorker;
        private System.Windows.Forms.Timer timerScannerStatus;
        private System.Windows.Forms.Panel panelId;
        private System.Windows.Forms.TextBox textBoxIDNumber;
        private System.Windows.Forms.Label label1;
    }
}
