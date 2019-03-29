using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crims.UI.Win.Enroll.Controls
{
    public class Login : UserControl
    {
        private Label label1;

        public Login()
        {

        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(372, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(275, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Loading, Please Wait...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Login
            // 
            this.Controls.Add(this.label1);
            this.Name = "Login";
            this.Size = new System.Drawing.Size(1004, 206);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
