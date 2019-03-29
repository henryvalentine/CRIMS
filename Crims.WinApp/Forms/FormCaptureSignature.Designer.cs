namespace Crims.UI.Win.Enroll.Forms
{
    partial class FormCaptureSignature
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
            this.sigPlusNET1 = new Topaz.SigPlusNET();
            this.cmdSaveImage = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.cmdClear = new System.Windows.Forms.Button();
            this.cmdStop = new System.Windows.Forms.Button();
            this.cmdSign = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sigPlusNET1
            // 
            this.sigPlusNET1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.sigPlusNET1.BackColor = System.Drawing.Color.White;
            this.sigPlusNET1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.sigPlusNET1.ForeColor = System.Drawing.Color.Black;
            this.sigPlusNET1.Location = new System.Drawing.Point(19, 73);
            this.sigPlusNET1.Name = "sigPlusNET1";
            this.sigPlusNET1.Size = new System.Drawing.Size(479, 165);
            this.sigPlusNET1.TabIndex = 22;
            this.sigPlusNET1.Text = "sigPlusNET1";
            // 
            // cmdSaveImage
            // 
            this.cmdSaveImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSaveImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSaveImage.Location = new System.Drawing.Point(214, 255);
            this.cmdSaveImage.Name = "cmdSaveImage";
            this.cmdSaveImage.Size = new System.Drawing.Size(75, 32);
            this.cmdSaveImage.TabIndex = 17;
            this.cmdSaveImage.Text = "OK";
            this.cmdSaveImage.UseVisualStyleBackColor = true;
            this.cmdSaveImage.Click += new System.EventHandler(this.cmdSaveImage_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.Location = new System.Drawing.Point(423, 18);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 15;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // cmdClear
            // 
            this.cmdClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClear.Location = new System.Drawing.Point(324, 18);
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(75, 23);
            this.cmdClear.TabIndex = 14;
            this.cmdClear.Text = "Clear";
            this.cmdClear.UseVisualStyleBackColor = true;
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // cmdStop
            // 
            this.cmdStop.Location = new System.Drawing.Point(106, 18);
            this.cmdStop.Name = "cmdStop";
            this.cmdStop.Size = new System.Drawing.Size(75, 23);
            this.cmdStop.TabIndex = 13;
            this.cmdStop.Text = "Stop";
            this.cmdStop.UseVisualStyleBackColor = true;
            this.cmdStop.Visible = false;
            this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
            // 
            // cmdSign
            // 
            this.cmdSign.Location = new System.Drawing.Point(19, 18);
            this.cmdSign.Name = "cmdSign";
            this.cmdSign.Size = new System.Drawing.Size(75, 23);
            this.cmdSign.TabIndex = 12;
            this.cmdSign.Text = "Sign";
            this.cmdSign.UseVisualStyleBackColor = true;
            this.cmdSign.Visible = false;
            this.cmdSign.Click += new System.EventHandler(this.cmdSign_Click);
            // 
            // FormCaptureSignature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 299);
            this.Controls.Add(this.sigPlusNET1);
            this.Controls.Add(this.cmdSaveImage);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdClear);
            this.Controls.Add(this.cmdStop);
            this.Controls.Add(this.cmdSign);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FormCaptureSignature";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Signature Capture";
            this.Shown += new System.EventHandler(this.FormCaptureSignature_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private Topaz.SigPlusNET sigPlusNET1;
        private System.Windows.Forms.Button cmdSaveImage;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Button cmdClear;
        private System.Windows.Forms.Button cmdStop;
        private System.Windows.Forms.Button cmdSign;
    }
}