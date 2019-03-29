namespace Crims.UI.Win.TimeAndAttendance.Controls
{
    partial class SyncControl
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
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSaveLogs = new System.Windows.Forms.Button();
            this.buttonStopSync = new System.Windows.Forms.Button();
            this.buttonStartSync = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxLog.Location = new System.Drawing.Point(0, 56);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.richTextBoxLog.Size = new System.Drawing.Size(731, 405);
            this.richTextBoxLog.TabIndex = 0;
            this.richTextBoxLog.Text = "";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.buttonClose);
            this.panel1.Controls.Add(this.buttonSaveLogs);
            this.panel1.Controls.Add(this.buttonStartSync);
            this.panel1.Controls.Add(this.buttonStopSync);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(731, 50);
            this.panel1.TabIndex = 1;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(256, 13);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSaveLogs
            // 
            this.buttonSaveLogs.Location = new System.Drawing.Point(175, 13);
            this.buttonSaveLogs.Name = "buttonSaveLogs";
            this.buttonSaveLogs.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveLogs.TabIndex = 0;
            this.buttonSaveLogs.Text = "Save Logs...";
            this.buttonSaveLogs.UseVisualStyleBackColor = true;
            // 
            // buttonStopSync
            // 
            this.buttonStopSync.Location = new System.Drawing.Point(94, 13);
            this.buttonStopSync.Name = "buttonStopSync";
            this.buttonStopSync.Size = new System.Drawing.Size(75, 23);
            this.buttonStopSync.TabIndex = 0;
            this.buttonStopSync.Text = "Stop Sync";
            this.buttonStopSync.UseVisualStyleBackColor = true;
            this.buttonStopSync.Click += new System.EventHandler(this.buttonStopSync_Click);
            // 
            // buttonStartSync
            // 
            this.buttonStartSync.Location = new System.Drawing.Point(13, 13);
            this.buttonStartSync.Name = "buttonStartSync";
            this.buttonStartSync.Size = new System.Drawing.Size(75, 23);
            this.buttonStartSync.TabIndex = 0;
            this.buttonStartSync.Text = "Start Sync";
            this.buttonStartSync.UseVisualStyleBackColor = true;
            this.buttonStartSync.Click += new System.EventHandler(this.buttonStartSync_Click);
            // 
            // SyncControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.richTextBoxLog);
            this.Name = "SyncControl";
            this.Size = new System.Drawing.Size(731, 461);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSaveLogs;
        private System.Windows.Forms.Button buttonStopSync;
        private System.Windows.Forms.Button buttonStartSync;
    }
}
