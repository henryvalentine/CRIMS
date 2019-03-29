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
using Crims.UI.Win.TimeAndAttendance.Properties;
using System.IO;

namespace TandAProject.Controls
{
    public partial class SettingsControl : UserControl
    {
        ApplicationController applicationState;
        //Initialise the Notification Deligate
        public ApplicationStateChangeNotifyer StateNotifyer;
        public ApplicationMessageNotifyer MessageNotifyer;
        int totalTemplates;
        int totalRecords;

        List<KeyValuePair<int, string>> CustomFields = null;

        public SettingsControl(ApplicationController applicationState, int TotalTemplates, int TotalRecords)
        {
            InitializeComponent();
            this.applicationState = applicationState;
            this.totalTemplates = TotalTemplates;
            this.totalRecords = TotalRecords;

            GetProjectCustomFields();
        }

        public SettingsControl()
        {
            InitializeComponent();
            GetProjectCustomFields();
        }

        private void GetProjectCustomFields()
        {
            try
            {
                CustomFields = DataServices.GetProjectCustomFields();

                foreach (var cField in CustomFields)
                {
                    comboBoxCData1Picker.Items.Add(cField);
                    comboBoxCData2Picker.Items.Add(cField);
                    comboBoxCData3Picker.Items.Add(cField);
                    comboBoxCData4Picker.Items.Add(cField);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            StateNotifyer(ApplicationController.State.Startup);
            this.Dispose();
        }

        private void buttonShutDown_Click(object sender, EventArgs e)
        {
            StateNotifyer(ApplicationController.State.Dispose);
        }

        private void buttonBrosweImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            AppBrand.Text = openFileDialog1.FileName;
            Image bannerImage = Image.FromFile(new FileInfo(openFileDialog1.FileName).FullName);
            bannerImage.Save("new_client_banner.png");
        }

        private void SettingsControl_Load(object sender, EventArgs e)
        {
            labelTemplateCount.Text = totalTemplates.ToString();
            labelRecordCount.Text = totalRecords.ToString();
        }

        private void labelRecordCount_Click(object sender, EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Settings.Default.Save();
            MessageNotifyer("Settings saved succesfully. Please restart application.");
        }

        private void buttonReport_Click(object sender, EventArgs e)
        {
            StateNotifyer(ApplicationController.State.Report);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageNotifyer("Configure Application Settings");
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            try
            {
                bool testResult = DataServices.TestDBConnection(Settings.Default.DBServer, Settings.Default.DBName, Settings.Default.DBPort, Settings.Default.DBUser, Settings.Default.DBPassword);
                MessageBox.Show(this, String.Format("Connection to Server {0}", testResult ? "Successful" : "Failed"), "Test Database Server");

            }
            catch (Exception exp)
            {
                MessageBox.Show(this,exp.Message,"Application Server..");
            }
        }

        private void buttonAttendanceIE_Click(object sender, EventArgs e)
        {
            StateNotifyer(ApplicationController.State.Attendance_EI);
        }

        private void buttonSyncControl_Click(object sender, EventArgs e)
        {
            StateNotifyer(ApplicationController.State.SyncTool);
        }

        private void buttonTestSyncDBServer_Click(object sender, EventArgs e)
        {
            try
            {
                bool testResult = DataServices.TestDBConnection(Settings.Default.PushServer, Settings.Default.PushServerDB, Settings.Default.PushServerDBPort, Settings.Default.PushServerDBUser, Settings.Default.PushServerDBPassword);
                MessageBox.Show(this, String.Format("Connection to Server {0}", testResult ? "Successful" : "Failed"), "Test Sync Server");

            }
            catch (Exception exp)
            {
                MessageBox.Show(this, exp.Message, "Configure Sync Server..");
            }
        }

        private void comboBoxCData1Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var _sender = ((ComboBox)sender).Name;

            switch (_sender)
            {
                case "comboBoxCData1Picker":

                    textBoxCDataField1.Text = ((KeyValuePair<int, string>)comboBoxCData1Picker.SelectedItem).Key.ToString();
                    textBoxCDataLabel1.Text = ((KeyValuePair<int, string>)comboBoxCData1Picker.SelectedItem).Value;
                    break;

                case "comboBoxCData2Picker":

                    textBoxCDataField2.Text = ((KeyValuePair<int, string>)comboBoxCData2Picker.SelectedItem).Key.ToString();
                    textBoxCDataLabel2.Text = ((KeyValuePair<int, string>)comboBoxCData2Picker.SelectedItem).Value;
                    break;
                case "comboBoxCData3Picker":

                    textBoxCDataField3.Text = ((KeyValuePair<int, string>)comboBoxCData3Picker.SelectedItem).Key.ToString();
                    textBoxCDataLabel3.Text = ((KeyValuePair<int, string>)comboBoxCData3Picker.SelectedItem).Value;
                    break;
                case "comboBoxCData4Picker":

                    textBoxCDataField4.Text = ((KeyValuePair<int, string>)comboBoxCData4Picker.SelectedItem).Key.ToString();
                    textBoxCDataLabel4.Text = ((KeyValuePair<int, string>)comboBoxCData4Picker.SelectedItem).Value;
                    break;
            }
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {

        }
    }
}
