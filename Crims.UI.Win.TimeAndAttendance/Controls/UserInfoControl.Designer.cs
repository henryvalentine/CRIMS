using Neurotec.Biometrics;
using TandAProject.Services;

namespace TandAProject.Controls
{
    partial class UserInfoControl
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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelCustom2 = new System.Windows.Forms.Label();
            this.labelCustom3 = new System.Windows.Forms.Label();
            this.labelCustom1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCustom4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelPhone = new System.Windows.Forms.Label();
            this.labelMiddleName = new System.Windows.Forms.Label();
            this.labelFirstName = new System.Windows.Forms.Label();
            this.labelSurname = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelId = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelDateTime = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.DisplayTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox.BackColor = System.Drawing.Color.White;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(20, 17);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(159, 171);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.BackColor = System.Drawing.Color.MintCream;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelPhone);
            this.panel1.Controls.Add(this.labelMiddleName);
            this.panel1.Controls.Add(this.labelFirstName);
            this.panel1.Controls.Add(this.labelSurname);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Controls.Add(this.labelId);
            this.panel1.Controls.Add(this.pictureBox);
            this.panel1.Location = new System.Drawing.Point(3, 61);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(912, 345);
            this.panel1.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(601, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(38, 20);
            this.label13.TabIndex = 1;
            this.label13.Text = "Title";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(584, 109);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 20);
            this.label11.TabIndex = 1;
            this.label11.Text = "Phone";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "CDataLabel4", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(23, 11);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(14, 15);
            this.label12.TabIndex = 1;
            this.label12.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.CDataLabel4;
            // 
            // labelCustom2
            // 
            this.labelCustom2.AutoSize = true;
            this.labelCustom2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "CDataLabel2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.labelCustom2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustom2.Location = new System.Drawing.Point(16, 9);
            this.labelCustom2.Name = "labelCustom2";
            this.labelCustom2.Size = new System.Drawing.Size(14, 15);
            this.labelCustom2.TabIndex = 1;
            this.labelCustom2.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.CDataLabel2;
            // 
            // labelCustom3
            // 
            this.labelCustom3.AutoSize = true;
            this.labelCustom3.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "CDataLabel3", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.labelCustom3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustom3.Location = new System.Drawing.Point(23, 11);
            this.labelCustom3.Name = "labelCustom3";
            this.labelCustom3.Size = new System.Drawing.Size(14, 15);
            this.labelCustom3.TabIndex = 1;
            this.labelCustom3.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.CDataLabel3;
            // 
            // labelCustom1
            // 
            this.labelCustom1.AutoSize = true;
            this.labelCustom1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default, "CDataLabel1", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.labelCustom1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustom1.Location = new System.Drawing.Point(16, 9);
            this.labelCustom1.Name = "labelCustom1";
            this.labelCustom1.Size = new System.Drawing.Size(14, 15);
            this.labelCustom1.TabIndex = 1;
            this.labelCustom1.Text = global::Crims.UI.Win.TimeAndAttendance.Properties.Settings.Default.CDataLabel1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(208, 107);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 20);
            this.label7.TabIndex = 1;
            this.label7.Text = "Middle Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(553, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 20);
            this.label5.TabIndex = 1;
            this.label5.Text = "First Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(235, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "Surname";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.White;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(21, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(309, 37);
            this.label8.TabIndex = 0;
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(217, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Personel ID";
            // 
            // labelCustom4
            // 
            this.labelCustom4.BackColor = System.Drawing.Color.White;
            this.labelCustom4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelCustom4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustom4.Location = new System.Drawing.Point(13, 29);
            this.labelCustom4.Name = "labelCustom4";
            this.labelCustom4.Size = new System.Drawing.Size(319, 37);
            this.labelCustom4.TabIndex = 0;
            this.labelCustom4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(13, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(319, 37);
            this.label9.TabIndex = 0;
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(21, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(309, 37);
            this.label6.TabIndex = 0;
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPhone
            // 
            this.labelPhone.BackColor = System.Drawing.Color.White;
            this.labelPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPhone.ForeColor = System.Drawing.Color.Firebrick;
            this.labelPhone.Location = new System.Drawing.Point(643, 101);
            this.labelPhone.Name = "labelPhone";
            this.labelPhone.Size = new System.Drawing.Size(264, 33);
            this.labelPhone.TabIndex = 0;
            this.labelPhone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMiddleName
            // 
            this.labelMiddleName.BackColor = System.Drawing.Color.White;
            this.labelMiddleName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMiddleName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMiddleName.ForeColor = System.Drawing.Color.Firebrick;
            this.labelMiddleName.Location = new System.Drawing.Point(314, 101);
            this.labelMiddleName.Name = "labelMiddleName";
            this.labelMiddleName.Size = new System.Drawing.Size(216, 33);
            this.labelMiddleName.TabIndex = 0;
            this.labelMiddleName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFirstName
            // 
            this.labelFirstName.BackColor = System.Drawing.Color.White;
            this.labelFirstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFirstName.ForeColor = System.Drawing.Color.Firebrick;
            this.labelFirstName.Location = new System.Drawing.Point(643, 59);
            this.labelFirstName.Name = "labelFirstName";
            this.labelFirstName.Size = new System.Drawing.Size(264, 33);
            this.labelFirstName.TabIndex = 0;
            this.labelFirstName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelSurname
            // 
            this.labelSurname.BackColor = System.Drawing.Color.White;
            this.labelSurname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelSurname.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSurname.ForeColor = System.Drawing.Color.Firebrick;
            this.labelSurname.Location = new System.Drawing.Point(314, 59);
            this.labelSurname.Name = "labelSurname";
            this.labelSurname.Size = new System.Drawing.Size(216, 33);
            this.labelSurname.TabIndex = 0;
            this.labelSurname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.White;
            this.labelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.Firebrick;
            this.labelTitle.Location = new System.Drawing.Point(643, 17);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(264, 33);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelId
            // 
            this.labelId.BackColor = System.Drawing.Color.White;
            this.labelId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelId.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelId.ForeColor = System.Drawing.Color.Firebrick;
            this.labelId.Location = new System.Drawing.Point(314, 17);
            this.labelId.Name = "labelId";
            this.labelId.Size = new System.Drawing.Size(216, 33);
            this.labelId.TabIndex = 0;
            this.labelId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelDateTime);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(918, 44);
            this.panel2.TabIndex = 2;
            // 
            // labelDateTime
            // 
            this.labelDateTime.BackColor = System.Drawing.Color.White;
            this.labelDateTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDateTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 23.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDateTime.Location = new System.Drawing.Point(0, 0);
            this.labelDateTime.Name = "labelDateTime";
            this.labelDateTime.Size = new System.Drawing.Size(918, 44);
            this.labelDateTime.TabIndex = 1;
            this.labelDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 412);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(918, 38);
            this.panel3.TabIndex = 3;
            // 
            // DisplayTimer
            // 
            this.DisplayTimer.Interval = 500;
            this.DisplayTimer.Tick += new System.EventHandler(this.DisplayTimer_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelCustom1);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Location = new System.Drawing.Point(352, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(343, 75);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelCustom3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(3, 84);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(343, 75);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelCustom2);
            this.groupBox3.Controls.Add(this.labelCustom4);
            this.groupBox3.Location = new System.Drawing.Point(352, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(343, 75);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(343, 75);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.groupBox4);
            this.flowLayoutPanel1.Controls.Add(this.groupBox3);
            this.flowLayoutPanel1.Controls.Add(this.groupBox2);
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(203, 147);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(703, 179);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // UserInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGreen;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "UserInfoControl";
            this.Size = new System.Drawing.Size(918, 450);
            this.Load += new System.EventHandler(this.UserInfoControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelId;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelDateTime;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelCustom1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelPhone;
        private System.Windows.Forms.Label labelMiddleName;
        private System.Windows.Forms.Label labelFirstName;
        private System.Windows.Forms.Label labelSurname;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Timer DisplayTimer;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelCustom2;
        private System.Windows.Forms.Label labelCustom3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelCustom4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
