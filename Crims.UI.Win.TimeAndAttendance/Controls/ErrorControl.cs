using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TandAProject.Services;
using TandAProject.Models;

namespace TandAProject.Controls
{
    public partial class ErrorControl : UserControl
    {
        public ApplicationStateChangeNotifyer StateNotifyer;
        public ApplicationMessageNotifyer MessageNotifyer;

        private String ErrorMessage;
        public ErrorControl(string Message)
        {
            this.ErrorMessage = Message;
            InitializeComponent();
        }

        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            StateNotifyer(ApplicationController.State.Idle);
        }

        private void ErrorControl_Load(object sender, EventArgs e)
        {
            label1.Text = ErrorMessage;
            DisplayTimer.Interval = Convert.ToInt32(new AppSettings().MessageDuration);
            DisplayTimer.Start();
        }
    }
}
