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
using TandAProject.Utils;
using Crims.UI.Win.TimeAndAttendance.Properties;

namespace TandAProject.Controls
{
    public partial class UserInfoControl : UserControl
    {
        private ApplicationController applicationState;

        public ApplicationStateChangeNotifyer StateNotifyer;
        public ApplicationMessageNotifyer MessageNotifyer;

        UserRecord _UserRecord = null;

         public UserInfoControl()
        {
            InitializeComponent();
        }

       public UserInfoControl(ApplicationController applicationState, UserRecord UserRecord)
        {
            this._UserRecord = UserRecord;
            this.applicationState = applicationState;
            InitializeComponent();

            DisplayUserInfo();
            DisplayTimer.Interval = Convert.ToInt32(new AppSettings().MessageDuration);
            DisplayTimer.Start();
        }

        private void DisplayUserInfo()
        {
            labelDateTime.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");

            if (_UserRecord.Photo != null)
            {
                pictureBox.Image = _UserRecord.Photo;
            }

            labelId.Text = _UserRecord.ProjectPrimaryCode;
            labelTitle.Text = _UserRecord.Title;
            labelSurname.Text = _UserRecord.Surname;
            labelFirstName.Text = _UserRecord.FirstName;
            labelMiddleName.Text = _UserRecord.MiddleName;
            labelPhone.Text = _UserRecord.PhoneNumber;

            if (Settings.Default.EnableCData1)
            {
                labelCustom1.Text = _UserRecord.CustomData1;
            }
            else {
                groupBox1.Visible = false;
            }
            if (Settings.Default.EnableCData2)
            {
                labelCustom2.Text = _UserRecord.CustomData2;
            }
            else {
                groupBox2.Visible = false;
            }
            if (Settings.Default.EnableCData3)
            {
                labelCustom3.Text = _UserRecord.CustomData3;
            }
            else {
                groupBox3.Visible = false;
            }
            if (Settings.Default.EnableCData4)
            {
                labelCustom4.Text = _UserRecord.CustomData4;
            }
            else {
                groupBox4.Visible = false;
            }
        }

        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            StateNotifyer(ApplicationController.State.Idle);
        }

        private void UserInfoControl_Load(object sender, EventArgs e)
        {

        }
    }
}
