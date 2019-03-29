using Crims.UI.Win.Enroll.DataServices;
using Crims.UI.Win.Enroll.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Xml;
using Crims.UI.Win.Enroll.Classes;
using Microsoft.Ajax.Utilities;

namespace Crims.UI.Win.Enroll
{
    public partial class FormSettings : Form
    {
        private UserAccountModel _UserProfile;
        public FormSettings()
        {
            InitializeComponent();
        }

        public FormSettings(UserAccountModel userProfile)
        {
            InitializeComponent();
           
            if (string.IsNullOrEmpty(userProfile?.ProfileId))
            {
                tbBiometricSettings.Enabled = false; // this disables the controls on it
                tbBiometricSettings.Visible = false; // this hides the controls on it.

                tbGeneralSetting.Enabled = false; // this disables the controls on it
                tbGeneralSetting.Visible = false; // this hides the controls on it.
            }
            if (!string.IsNullOrEmpty(userProfile?.ProfileId))
            {
                _UserProfile = userProfile;
                
                //tbBiometricSettings.Enabled = !_UserProfile.Role.Contains("Enroll"); 
                //tbBiometricSettings.Visible = !_UserProfile.Role.Contains("Enroll");

                tbGeneralSetting.Enabled = !_UserProfile.Role.Contains("Enroll");
                tbGeneralSetting.Visible = !_UserProfile.Role.Contains("Enroll");
            }
           
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            try
            {
                bool testResult = new DatabaseOpperations().TestDBConnection(Settings.Default.DBServer, Settings.Default.DBName, Settings.Default.DBPort, Settings.Default.DBUser, Settings.Default.DBPassword);
                MessageBox.Show(this, $"Connection to Server {(testResult ? "Successful" : "Failed")}", @"Test Database Server");

            }
            catch (Exception exp)
            {
                MessageBox.Show(this, exp.Message, "Test Database Server");
            }
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            Settings.Default.Save();
            UpdateWebConfig();
            MessageBox.Show(@"Settings Saved");
        }

        public void UpdateAppConfigConnectionStrings(string con, string connectionName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            
            if (connectionStringsSection != null)
            {
                connectionStringsSection.ConnectionStrings[connectionName].ConnectionString = con;
                config.Save();
                ConfigurationManager.RefreshSection("connectionStrings");
            }
            else
            {
                var config2 = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config2.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(connectionName, connectionName));
                config2.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("connectionStrings");
            }
          
        }

        private void UpdateWebConfig()
        {
            try
            {
                var syncConnectionString = "server=" + Settings.Default.syncDBServer + ";" +
                                            "port=" + Settings.Default.syncDBPort + ";" +
                                            "database=" + Settings.Default.syncDBName + ";" +
                                            "uid=" + Settings.Default.syncDBUser + ";"
                                            + "password=" + Settings.Default.syncDBPassword + "";


                UpdateAppConfigConnectionStrings(syncConnectionString, "crimsRemoteDbEntities");

                string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                if (string.IsNullOrEmpty(dir))
                {
                    return;
                }
                var parent = Directory.GetParent(dir).Parent;
                if (parent == null)
                {
                    return;
                }
                var parent2 = parent.FullName;//uncomment this for production
                //var parent2 = parent.FullName.Replace("Crims.WinApp", "Crims.UI.Web.Enroll");//uncomment this for debugging
                var webCofig = Path.Combine(parent2, "Web.config");

                var doc = new System.Xml.XmlDocument();
                doc.Load(webCofig);
                var root = doc.DocumentElement;
                if (root == null)
                {
                    return;
                }
                var xpath = "appSettings/add[@key='siteServerConnection']";
                var node = root.SelectSingleNode(xpath);
                if (node?.Attributes == null)
                {
                    return;
                }
                node.Attributes["value"].Value = syncConnectionString;
                doc.Save(webCofig);
            }
            catch (Exception ex)
            {
                var message = "Exception: Updating WebConfig";
                message += Environment.NewLine + ex;
                //MessageBox.Show(message);
            }
        }

        private void buttonBrowseSavedFilesDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserSavedFilesDir.ShowDialog() == DialogResult.OK)
                textBoxSavedFileDir.Text = folderBrowserSavedFilesDir.SelectedPath;
        }

        private void textBoxSavedFileDir_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonTestSyncServer_Click(object sender, EventArgs e)
        {
             try
            {
                bool testResult = new DatabaseOpperations().TestDBConnection(Settings.Default.syncDBServer, Settings.Default.syncDBName, 
                    Settings.Default.syncDBPort, Settings.Default.syncDBUser, Settings.Default.syncDBPassword);
                MessageBox.Show(this, String.Format("Connection to Server {0}", testResult==true ? "Successful" : "Failed"), "Test Sync Database Server");

            }
            catch (Exception exp)
            {
                MessageBox.Show(this, exp.Message, "Test Sync Database Server");
            }
        }
        
    }
}
