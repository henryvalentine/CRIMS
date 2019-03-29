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

namespace TandAProject.Controls
{
    public partial class LoginControl : UserControl
    {
        AppSettings AppSettings = new AppSettings();
        //Initialise the Notification Deligate
        public ApplicationStateChangeNotifyer StateNotifyer;
        public ApplicationMessageNotifyer MessageNotifyer;


        public LoginControl()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            StateNotifyer(ApplicationController.State.Idle);
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == AppSettings.SetupPassword)
            {
                StateNotifyer(ApplicationController.State.Setup);
            }
            else {
                StateNotifyer(ApplicationController.State.Login_Failed);
                textBoxPassword.Text = String.Empty;
            }
        }
    }
}
