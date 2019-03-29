namespace TandAProject.Controls
{
    partial class DefaultControl
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
                
                _deviceMan = null;
                _currentScanner = null;            
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
            this.panelFingerPrint = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblQuality = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFingerPrint
            // 
            this.panelFingerPrint.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panelFingerPrint.BackColor = System.Drawing.Color.White;
            this.panelFingerPrint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFingerPrint.Location = new System.Drawing.Point(264, 56);
            this.panelFingerPrint.Name = "panelFingerPrint";
            this.panelFingerPrint.Size = new System.Drawing.Size(243, 326);
            this.panelFingerPrint.TabIndex = 6;
            // 
            // panel4
            // 
            this.panel4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Location = new System.Drawing.Point(249, 23);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(270, 392);
            this.panel4.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblQuality);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(268, 31);
            this.panel2.TabIndex = 0;
            // 
            // lblQuality
            // 
            this.lblQuality.BackColor = System.Drawing.Color.White;
            this.lblQuality.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQuality.Location = new System.Drawing.Point(0, 0);
            this.lblQuality.Name = "lblQuality";
            this.lblQuality.Size = new System.Drawing.Size(268, 31);
            this.lblQuality.TabIndex = 8;
            this.lblQuality.Text = ".";
            this.lblQuality.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel5.BackColor = System.Drawing.Color.DarkGray;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Location = new System.Drawing.Point(233, 23);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(303, 392);
            this.panel5.TabIndex = 6;
            // 
            // DefaultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panelFingerPrint);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel5);
            this.Name = "DefaultControl";
            this.Size = new System.Drawing.Size(777, 438);
            this.Load += new System.EventHandler(this.DefaultControl_Load);
            this.VisibleChanged += new System.EventHandler(this.ScannerVisibleChanged);
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelFingerPrint;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblQuality;
    }
}
