namespace Crims.UI.Win.Enroll.Forms
{
    partial class WSQConverter
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(24, 123);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(994, 61);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "SELECT BASEDATAID, FINGERPRINTIMAGE,FINGERINDEXID FROM eid_fingerprintimages WHER" +
    "E basedataid IN (\'3472\',\'3473\',\'3474\',\'3476\',\'3480\',\'3482\',\'3486\',\'3487\')";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(834, 263);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(183, 55);
            this.button1.TabIndex = 1;
            this.button1.Text = "Extract and Convert";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Query";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(24, 40);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(994, 46);
            this.richTextBox2.TabIndex = 0;
            this.richTextBox2.Text = "server=localhost;user=root;database={2};port=3306;password=password;Convert Zero " +
    "Datetime=True";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Connection String";
            // 
            // richTextBox3
            // 
            this.richTextBox3.Location = new System.Drawing.Point(24, 220);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(994, 37);
            this.richTextBox3.TabIndex = 0;
            this.richTextBox3.Text = "E:\\Tools & Tutorials\\ARATEK\\wsq convert\\Osun8\\";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Destination Directory";
            // 
            // WSQConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 333);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox1);
            this.Name = "WSQConverter";
            this.Text = "WSQ_Converter";
            this.Load += new System.EventHandler(this.WSQ_Converter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.Label label3;
    }
}