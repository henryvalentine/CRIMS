using Crims.UI.Win.TimeAndAttendance.Controls;
using Crims.UI.Win.TimeAndAttendance.Properties;
using Neurotec.Biometrics;
using Neurotec.Devices;
using Neurotec.IO;
using Neurotec.Licensing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TandAProject.Controls;
using TandAProject.Models;
using TandAProject.Services;
using TandAProject.Utils;

namespace TandAProject
{
    public partial class MainForm : Form
    {

        #region Properties
        private AppSettings _AppSettings = new AppSettings();
        private ApplicationController StateController;
        private ApplicationStateChangeNotifyer _ApplicationStateChangeNotifyer;
        private ApplicationMessageNotifyer _ApplicationMessageNotifyer;
        private ActiveTemplateNotifyer _TemplateNotifyer;
        private ActiveIDNumberNotifyer _IDNumberNotifyer;
        
        private List<UserRecord> UserRecords = null;
        private List<DbTemplateRecord> DbTemplates = null;
        private NBuffer Template;

        private MatchingResult MatchedResult = null;
        private UserRecord MatchedRecord = null;
        private string MatchedRecordId;
        private string SearchUserId = null;

        private List<MatchingResult> MatchingResults;

        private SplashScreen splashControl;

        private BackgroundWorker MatcherBGWorker;

        ApplicationController.State CurrentApplicationState = ApplicationController.State.Unknown;
        #endregion

        #region MainForm

        public MainForm()
        {
            StateController = new ApplicationController();
            InitializeComponent();
            //InitNeurotechComponents();
            _ApplicationStateChangeNotifyer = new ApplicationStateChangeNotifyer(StateChange);
            _ApplicationMessageNotifyer = new ApplicationMessageNotifyer(ShowMessage);

            _TemplateNotifyer = new ActiveTemplateNotifyer(SetActiveTemplate);
            _IDNumberNotifyer = new ActiveIDNumberNotifyer(SetActiveIDNumber);

            MatcherBGWorker = new BackgroundWorker();
            this.MatcherBGWorker.WorkerSupportsCancellation = true;
            this.MatcherBGWorker.DoWork += new DoWorkEventHandler(this.DoWork);
            this.MatcherBGWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.WorkerCompleted);

            SetupTerminalBranding();
        }

        /// <summary>
        /// Client Branding is Expcted to be a an 800 x 100 Image
        /// Client Brand Image is expected to be called client_banner.png
        /// When a client brand is being changed, the new image will be called new_client_banner.png
        /// If new_client_banner.png exisits in the execution directory, 
        /// 1. client_banner.png will be deleted
        /// 2. new_client_banner.png will be renamed to client_banner.png
        /// If client_banner.png and new_client_banner.png does not exist
        /// 1. Application will attempt to use the default branding which is banner.png
        /// </summary>
        private void SetupTerminalBranding()
        {
            //checked if a new brand has been set.
            //If So, Delete old brand and set new brand
            if (File.Exists("new_client_banner.png"))
            {
                //Delete Existing BRanding
                if(File.Exists("client_banner.png")){ File.Delete("client_banner.png"); }

                //Rename new brand
                File.Copy("new_client_banner.png", "client_banner.png");
                File.Delete("new_client_banner.png");
            }

            try
            {
                if (File.Exists("client_banner.png"))
                {
                    label1.Image = Image.FromFile("client_banner.png");
                }
                else
                {
                    label1.Image = Image.FromFile("banner.png");
                }
            }
            catch (Exception exp)
            {
                Logger.logToFile(exp, AppSettings.ErrorLogPath);
                ShowError(exp.Message);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (_AppSettings.AppMode == "Service")
            {
                ShowLogin();
            }
            else 
            {
                ShowSplash();
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.O)
            {
                ShowLogin();
            }

            //Test User Data Display...
            else if (e.Control && e.KeyCode == Keys.F12)
            {
                ShowSampleUserInfo();
            }
        }

        private void CloseMainForm()
        {
            if (MessageBox.Show("Certain Processes are currently running.\n" +
                "Are you sure you want to close the window?", "Exit Application",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                this.Close();
            }
            else
            {
                return;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            const string Components = "Biometrics.FingerExtraction,Biometrics.FingerMatching,Devices.FingerScanners,Images.WSQ";
            NLicense.ReleaseComponents(Components);
        }
        #endregion

        #region Identification
        private void DoWork(object sender, DoWorkEventArgs e)
        {

            MatchingResults = null;
            MatchedRecord = null;
            MatchedResult = null;
            MatchedRecordId = string.Empty;
            List<DbTemplateRecord> usergDbTemplates = new List<DbTemplateRecord> { };
            try
            {
                MatchingResults = new List<MatchingResult>();

                //Search Through only user's BaseDataId During Verification Mode
                bool verifyMode = _AppSettings.AppMode.Equals("Verify", StringComparison.InvariantCulture);
                if (verifyMode && SearchUserId !=null)
                {
                    usergDbTemplates = DbTemplates.Where(x => x.UserPrimaryCode == SearchUserId.Trim()).ToList();
                }


                //User a filtered Down DB Templete if the System is in Verify Mode 
                //Otherwise, use the Full Pre-loaded DbTemplates
                foreach (var n in (verifyMode ? usergDbTemplates : DbTemplates))
                {
                    if (n.template != null)
                    {
                        //int score = Matcher.IdentifyNext(n.template);

                        //if (score > 0)
                        //{
                        //    MatchingResult result = new MatchingResult
                        //    {
                        //        UserPrimaryCode = n.UserPrimaryCode,
                        //        BaseDataId = n.baseDataId,
                        //        TemplateId = n.templateId,
                        //        Score = score
                        //    };
                        //    MatchingResults.Add(result);
                        //}
                    }
                }
                if (MatchingResults.Count > 0)
                {
                    e.Result = MatchingResults.OrderByDescending(x => x.Score).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, AppSettings.ErrorLogPath);
            }
            finally
            {
                //Matcher.IdentifyEnd();
            }
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ShowMessage("Error :" + e.Error.Message);
            }

            if (e.Cancelled)
            {
                ShowMessage("Error: Opperation was canceled");
            }
            if (e.Result!=null)
            {
                MatchedResult = (MatchingResult)e.Result;
                MatchedRecordId = MatchedResult.UserPrimaryCode;

                //Log Clocking if the highest Matching Guys Meets Matching THreshold
                if (MatchedResult.Score >= Convert.ToUInt32(_AppSettings.MatchingThreshold))
                {
                    if (_AppSettings.AppMode.Equals("Service", StringComparison.InvariantCulture))
                    {
                        //Do not log Matching Results on Service Mode
                        StateChange(ApplicationController.State.Identified);
                    }
                    else
                    {
                        LogAttendanceClock(MatchedResult);
                        StateChange(ApplicationController.State.Identified);
                    }
                }
                else
                {
                    if (_AppSettings.AppMode.Equals("Service", StringComparison.InvariantCulture))
                    {
                        //Let Admin Know it is a Low Match When in Service Mode
                        StateChange(ApplicationController.State.Identified_Low_Match);
                    }
                    else
                    {
                        //Just Tell User It has Failed. No need to tell him Low Match
                        StateChange(ApplicationController.State.Identify_failed);
                    }
                }
            }
            else
            {
                StateChange(ApplicationController.State.Identify_failed);
            }
        }

        private void LogAttendanceClock(MatchingResult matchedResult)
        {
            DataServices.LogAttendanceClock(matchedResult);
        }
        #endregion

        #region Neurotech Components
   
        #endregion

        #region Deligate Callbacks

        private void SetActiveIDNumber(string IDNumber)
        {
            this.SearchUserId = IDNumber;
        }

        private void SetActiveTemplate(NBuffer template)
        {
            this.Template = null;
            this.Template = template;
        }

        public void ShowMessage(string message)
        {
            MessageLabel.Text = message;
            this.Refresh();
            this.Focus();
        }

        public void ShowMessage(string message, bool PlaySound, Object audioMedia)
        {
            MessageLabel.Text = message;
            this.Refresh();
            this.Focus();
        }

        public void StateChange(ApplicationController.State state)
        {
            try
            {
                switch (state)
                {
                    case ApplicationController.State.Captured_Good:
                        //this.Template = enrollFromScanner.Template;
                        ShowMessage("Please wait while your fingerprint is being processed...");
                        RemoveAllCenterPanelControls(); // Remove the Finger Capture Control...

                        if (this.DbTemplates != null && this.DbTemplates.Count() > 0)
                        {
                            MatcherBGWorker.RunWorkerAsync();
                        }
                        else
                        {
                            ShowMessage("Fingerprints failed to load. Contact Administrator");
                        }

                        break;
                    case ApplicationController.State.Captured_Bad:
                        ShowFingerCaptureControl();
                        break;
                    case ApplicationController.State.Capturing:
                        break;
                    case ApplicationController.State.Idle:
                        ShowFingerCaptureControl();
                        break;
                    case ApplicationController.State.Identifying:
                        ShowMessage("Please Wait...");
                        break;
                    case ApplicationController.State.LoadJobFailed:
                        //Do Nothing... To enable Application Display Error...
                        break;
                    case ApplicationController.State.LoadJobCompleted:
                        if (_AppSettings.PreloadUserData)
                        {
                            this.UserRecords = splashControl.UserRecords;
                        }

                        this.DbTemplates = splashControl.DbTemplateRecords;
                        if (this.DbTemplates != null && this.DbTemplates.Count() > 0)
                        {
                            ShowFingerCaptureControl();
                        }
                        else
                        {
                            ShowMessage("Fingerprints failed to load. Contact Administrator");
                        }
                        break;
                    case ApplicationController.State.Loading:
                        ShowMessage("Please Wait...");
                        break;
                    case ApplicationController.State.Startup:
                        ShowSplash();
                        break;
                    case ApplicationController.State.Verifying:
                        ShowMessage("Please Wait...");
                        break;
                    case ApplicationController.State.Identified:
                        ShowUserInfo(MatchedRecordId);
                        break;
                    case ApplicationController.State.Identified_Low_Match:
                        ShowUserInfo(MatchedRecordId);
                        break;
                    case ApplicationController.State.Identify_failed:
                        ShowError(_AppSettings.Failed_Message);
                        break;
                    case ApplicationController.State.Verified:
                        ShowUserInfo(MatchedRecordId);
                        break;
                    case ApplicationController.State.Verify_Failed:
                        ShowError(_AppSettings.Failed_Message);
                        break;
                    case ApplicationController.State.Dispose:
                        CloseMainForm();
                        break;
                    case ApplicationController.State.No_User_Found:
                        ShowError("Failed to displayed verified user record. Please notify the system administrator.");
                        break;
                    case ApplicationController.State.Report:
                        ShowMessage("View and Print Time and Attendance Reports");
                        ShowReport();
                        break;
                    case ApplicationController.State.Setup:
                        ShowMessage("Configure Application Settings");
                        ShowSettings();
                        break;
                    case ApplicationController.State.Login_Failed:
                        ShowMessage("Invalid Login Details!");
                        break;
                    case ApplicationController.State.Attendance_EI:
                        ShowMessage("Import and Export Attendance Logs");
                        ShowImportExportControl();
                        break;
                    case ApplicationController.State.SyncTool:
                        ShowSyncScreen();
                        break;
                }
            }
            catch (Exception exp)
            {

                Logger.logToFile(exp, AppSettings.ErrorLogPath);
            }
        }

        #endregion

        #region User Controls
        private void ShowImportExportControl()
        {
            ImportExportControl control = new ImportExportControl();
            control.StateNotifyer = _ApplicationStateChangeNotifyer;
            control.MessageNotifyer = _ApplicationMessageNotifyer;

            AddControl(control, ApplicationController.State.Attendance_EI);
        }

        private void ShowReport()
        {
            Form reportForm = new Form();

            ReportManagerControl control = new ReportManagerControl();
            control.StateNotifyer = _ApplicationStateChangeNotifyer;
            control.MessageNotifyer = _ApplicationMessageNotifyer;
            AddControl(control, ApplicationController.State.Report);
        }

        void ShowFingerCaptureControl()
        {
            try
            {
                ShowMessage("Please wait...");

                FingerCaptureControl control = new FingerCaptureControl();
                //control.Extractor = Extractor;
                control.StateNotifyer = _ApplicationStateChangeNotifyer;
                control.MessageNotifyer = _ApplicationMessageNotifyer;
                control.TemplateNotifyer = _TemplateNotifyer;
                control.IDNumberNotifyer = _IDNumberNotifyer;

                AddControl(control);

            }
            catch (Exception exp)
            {

               Logger.logToFile(exp, AppSettings.ErrorLogPath);
            }
        }

        void ShowSampleUserInfo()
        {
            ShowMessage(_AppSettings.Success_Message);

            if (_AppSettings.PreloadUserData)
            {
                MatchedRecord = UserRecords.FirstOrDefault();
            }
            else
            {
                try
                {
                    MatchedRecord = DataServices.FindSampleUserRecord();
                }
                catch (Exception exp)
                {
                    Logger.logToFile(exp, AppSettings.ErrorLogPath);
                    ShowError(exp.Message);
                }
            }

            if (MatchedRecord == null)
            {
                ShowError("Cannot Find User Record");
            }
            else
            {
                ShowUserInfo(MatchedRecord);
            }
        }

        void ShowUserInfo(string userPrimaryCode)
        {
            ShowMessage(_AppSettings.Success_Message);

            if (_AppSettings.PreloadUserData)
            {
                MatchedRecord = UserRecords.FirstOrDefault(x => x.ProjectPrimaryCode == userPrimaryCode);
            }
            else
            {
                try
                {
                    MatchedRecord = DataServices.FindUserRecord(userPrimaryCode);
                }
                catch (Exception exp)
                {
                    Logger.logToFile(exp, AppSettings.ErrorLogPath);
                    ShowError(exp.Message);
                }
            }

            if (MatchedRecord == null)
            {
                ShowError("Cannot Find User Record");
            }
            else
            {
                ShowUserInfo(MatchedRecord);
            }
        }

        void ShowUserInfo(UserRecord user)
        {
            if (_AppSettings.AppMode.Equals("Service",StringComparison.InvariantCulture))
            {
                string addonMsg = MatchedResult != null ? MatchedResult.Score.ToString() : "";
                ShowMessage(_AppSettings.Success_Message+". Score="+addonMsg);
            }
            else
            {
                ShowMessage(_AppSettings.Success_Message);
            }
            UserInfoControl userInfoControl = new UserInfoControl(StateController, user);
            userInfoControl.StateNotifyer = _ApplicationStateChangeNotifyer;
            userInfoControl.MessageNotifyer = _ApplicationMessageNotifyer;

            AddControl(userInfoControl);
        }

        void ShowSettings()
        {
            int totalRecords = UserRecords != null ? UserRecords.Count() : 0;
            int totalTemplates = DbTemplates != null ? DbTemplates.Count() : 0;

            SettingsControl control = new SettingsControl(StateController, totalTemplates, totalRecords);
            control.StateNotifyer = _ApplicationStateChangeNotifyer;
            control.MessageNotifyer = _ApplicationMessageNotifyer;

            AddControl(control);
        }

        void ShowSyncScreen(bool autoStart = false)
        {
            ShowMessage("Synchronize Transaction Logs to Server");
            SyncControl control = new SyncControl(autoStart);
            control.StateNotifyer = _ApplicationStateChangeNotifyer;
            control.MessageNotifyer = _ApplicationMessageNotifyer;

            AddControl(control, ApplicationController.State.SyncTool);
        }

        void ShowLogin()
        {
            ShowMessage("Login to access application settings");

            LoginControl control = new LoginControl();
            control.StateNotifyer = _ApplicationStateChangeNotifyer;
            control.MessageNotifyer = _ApplicationMessageNotifyer;

            AddControl(control, ApplicationController.State.Login);
        }

        void ShowSplash()
        {
            ShowMessage("Please Wait...");

            splashControl = new SplashScreen(StateController);
            splashControl.StateNotifyer = _ApplicationStateChangeNotifyer;
            splashControl.MessageNotifyer = _ApplicationMessageNotifyer;

            AddControl(splashControl, ApplicationController.State.Startup);
        }

        void ShowError(string message)
        {
            ShowMessage("Your fingerprint cannot be verified.");

            ErrorControl FailedVerificationControl = new ErrorControl(message);
            FailedVerificationControl.StateNotifyer = _ApplicationStateChangeNotifyer;
            FailedVerificationControl.MessageNotifyer = _ApplicationMessageNotifyer;

            AddControl(FailedVerificationControl);
        }

        public void AddControl(Control control, ApplicationController.State ControlState)
        {
            RemoveAllCenterPanelControls();
            centerPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            control.Visible = true;
            this.CurrentApplicationState = ControlState;
        }

        public void AddControl(Control control)
        {
            RemoveAllCenterPanelControls();
            centerPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            control.Visible = true;
        }

        public void RemoveCenterPanelControl(Control control)
        {
            if (control != null) { control.Dispose(); }
        }

        public void RemoveAllCenterPanelControls()
        {
            foreach(Control c in centerPanel.Controls)
            {
                c.Dispose();
                centerPanel.Controls.Remove(c);
            }
        }

        #endregion

        private void timerBGProcess_Tick(object sender, EventArgs e)
        {
            //Check if Auto Sync Is enabled
            //Queue in Background Process if Enabled
            if (Settings.Default.EnableSync)
            {
                CheckAndExecuteSyncTool();
            }
        }

        private void CheckAndExecuteSyncTool()
        {
            var currentTime = DateTime.Now.ToString("hh:mm");
            var RunTime = Settings.Default.SyncTime;
            if (currentTime.Equals(RunTime, StringComparison.InvariantCulture) && CurrentApplicationState!= ApplicationController.State.SyncTool)
            {
                ShowSyncScreen(true);
            }
        }
    }
}
