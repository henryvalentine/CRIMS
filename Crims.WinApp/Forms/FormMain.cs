using Crims.UI.Win.Enroll.Classes;
using Crims.UI.Win.Enroll.DataServices;
using Crims.UI.Win.Enroll.Enums;
using Crims.UI.Win.Enroll.Properties;
using Crims.Win.Enroll;
using Neurotec.Licensing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Reflection;
using System.Security.Principal;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using Crims.Data.Models;
using Crims.UI.Win.Enroll.Helpers;
using Newtonsoft.Json;
using Service.Pattern;
using WebGrease.Css.Extensions;
using Microsoft.Win32;
using Neurotec.Biometrics;
using Neurotec.Images;
using Neurotec.IO;
using Settings = Crims.UI.Win.Enroll.Properties.Settings;

namespace Crims.UI.Win.Enroll.Forms
{
    public partial class FormMain : Form
    {
        private CapturePhotoNotifyer _capturePhotoNotifyer;
        private CaptureFingerNotifyer _captureFingerNotifyer;
        private CaptureSignatureNotifyer _captureSignatureNotifyer;
        //private LoginNotifyer _LoginNotifyer;
        private BusyNotifyer _busyNofityer;
        private BusyScreen _busyScreen;
        private Login _login;
        private Project _currentSessionProject;
        private bool _btnRefreshClicked = false;
        private BiometricsRecord _currentDisplayedEnrollment;
        private ApplicationSetttings _applicationSettings;
        private BiometricsRecord _biometricsRecord;
        private UserAccountModel _userProfile;
        private static CrimsSync _crimsSync;
        private static FormSettings _formSettings;
        private bool _appLoginState;
        private List<string> _backupFiles = new List<string>();
        private int _totalImported = 0;
        private int _totalImportCount = 0;
        private List<FingerReason> _fingerReasons = new List<FingerReason>();
        //Directory where files will be saved.
        private string _saveFileDir = Settings.Default.SavedFilesDir;
        private bool _saveToDb = Settings.Default.SaveBiometricsToDBOption;
        public int OriginalProcessId;



        public FormMain()
        {
            try
            {
                _applicationSettings = new ApplicationSetttings();
                _capturePhotoNotifyer = PhotoCaptureNotifyer;
                _captureFingerNotifyer = FingerCaptureNotifyer;
                _captureSignatureNotifyer = SignatureCaptureNotifyer;
                //_LoginNotifyer = new LoginNotifyer(LoginNotifyer);
                _busyNofityer = BusyScreenNotifyer;
                _busyScreen = new BusyScreen(_busyNofityer);

                InitializeComponent();
                // Start the browser after initializing global component

                InitializeChromium();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                AppErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);

                if (ex.InnerException != null)
                {
                    AppErrorLogger.LogError(ex.InnerException.StackTrace, ex.InnerException.Source, ex.InnerException.Message);
                }
            }
        }


        public static ServiceController[] GetSqlInstances()
        {
            var services = ServiceController.GetServices().Where(x => x.ServiceName.Contains("SQL")).ToArray();
            var servicesToRemove = services.Where(x => x.DisplayName.Contains("Agent") ||
                                                  x.DisplayName.Contains("Browser") ||
                                                  x.DisplayName.Contains("VSS") ||
                                                  x.DisplayName.Contains("Active")).ToArray();
            return services.Except(servicesToRemove).ToArray();
        }
        
        #region Deligate Events
        private void PhotoCaptureNotifyer(Image image, byte[] imageTemplate, bool OKCancel)
        {
            if (OKCancel)
            {
                _biometricsRecord.Photograph = image;
                _biometricsRecord.PhotographTemplate = imageTemplate;
                pictureBoxUserPhoto.Image = image;
            }

            UpdateStatusLabels();
        }

        private void SignatureCaptureNotifyer(Image image, bool OKCancel)
        {
            if (OKCancel)
            {
                _biometricsRecord.Signature = image;
                pictureBoxSignature.Image = image;
            }

            UpdateStatusLabels();
        }

        private bool FingerCaptureNotifyer(NFinger finger, NSubject fingerSubject, FingerDescription fingerDescription, bool oKCancel)
        {
            if (oKCancel)
            {
                var bitMap = finger.Image.ToBitmap();
                var status = _biometricsRecord.SaveActiveUserFingerRecords(finger, fingerSubject, fingerDescription);
                if (status)
                {
                    DisplayFingerImage(fingerDescription, bitMap);
                    UpdateStatusLabels();
                    return true;
                }
                return false;
                
            }
            
            return false;
        }

        private void LoginNotifyer(bool okCancel)
        {
            _appLoginState = okCancel;
            EnabledUiControls(okCancel);
            if (okCancel)
            {
                logoutToolStripMenuItem.Text = @"Hello " + _userProfile.FullName.Split(' ')[0] + @"!";
                buttonRefresh.PerformClick();
                this.BringToFront();
            }
            else
            {
                logoutToolStripMenuItem.Text = @"You are not logged in";
            }

        }

        private void EnabledUiControls(bool oKCancel)
        {
            Invoke(new Action(() =>
            {
                buttonFind.Enabled = buttonNext.Enabled = buttonPrevious.Enabled = buttonRefresh.Enabled = buttonSave.Enabled = oKCancel;
            }));

        }

        private void BusyScreenNotifyer(string message, AppNofityerState state)
        {
            switch (state)
            {
                case AppNofityerState.busy:
                    _busyScreen.ShowDialog();
                    break;
                case AppNofityerState.done:
                    _busyScreen.Visible = true;
                    break;
            }
        }

        #endregion

        private void DisplayFingerImage(FingerDescription fingerDescription, Image image)
        {
            switch (fingerDescription)
            {
                case FingerDescription.LFLittle:
                    pictureBoxLFLittle.Image = image;
                    break;
                case FingerDescription.LFRing:
                    pictureBoxLFRing.Image = image;
                    break;
                case FingerDescription.LFMiddle:
                    pictureBoxLFMiddle.Image = image;
                    break;
                case FingerDescription.LFIndex:
                    pictureBoxLFIndex.Image = image;
                    break;
                case FingerDescription.LFThumb:
                    pictureBoxLFThumb.Image = image;
                    break;
                case FingerDescription.RFLittle:
                    pictureBoxRFLittle.Image = image;
                    break;
                case FingerDescription.RFRing:
                    pictureBoxRFRing.Image = image;
                    break;
                case FingerDescription.RFMiddle:
                    pictureBoxRFMiddle.Image = image;
                    break;
                case FingerDescription.RFIndex:
                    pictureBoxRFIndex.Image = image;
                    break;
                case FingerDescription.RFThumb:
                    pictureBoxRFThumb.Image = image;
                    break;

            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _formSettings = new FormSettings(_userProfile);
            _formSettings.ShowDialog();
        }

        private void textBoxrecordId_Enter(object sender, EventArgs e)
        {
            if (textBoxrecordId.Text.Trim().Contains("Enrollment Id OR a Primary ID"))
            {
                textBoxrecordId.Clear();
            }
        }

        private void UpdateUserRecordOnUi(BiometricsRecord result, bool saveToDb)
        {
            var userDir = _saveFileDir + "\\" + result.EnrollmentId + "\\";

            ResetUserRecordsOnUi();
            _biometricsRecord = result;

            if (!saveToDb)
            {
                //Fetch Record Biometics From File
                result = FileHelper.GetBiometricsRecord(result, _saveFileDir);
            }

            //Display texts;
            var titleValue = Convert.ToInt32(result.Title);
            var genderValue = Convert.ToInt32(result.Gender);
            labelPrimaryId.Text = result.ProjectPrimaryCode;
            labelTitle.Text = Enum.GetName(typeof(EnumManager.TitleEnum), titleValue).Replace("_", " ");
            labelSurname.Text = result.Surname;
            labelFirstName.Text = result.FirstName;
            labelOtherName.Text = result.MiddleName;
            labelGender.Text = Enum.GetName(typeof(EnumManager.GenderEnum), genderValue).Replace("_", " ");

            //Find Images If Exists and Display

            //Get Signature
            _biometricsRecord.Signature = result.Signature;
            pictureBoxSignature.Image = result.Signature;
            pictureBoxSignature.Refresh();

            _biometricsRecord.Photograph = result.Photograph;
            _biometricsRecord.PhotographTemplate = result.PhotographTemplate;
            pictureBoxUserPhoto.Image = result.Photograph;
            pictureBoxUserPhoto.Refresh();

            _biometricsRecord.FingerprintRecords = result.FingerprintRecords;
            foreach (var fingerRecord in _biometricsRecord.FingerprintRecords)
            {
                DisplayFingerImage(fingerRecord.FingerDescription, fingerRecord.FingerImage);
            }

            UpdateStatusLabels();
        }

        private void ResetUserRecordsOnUi()
        {
            _biometricsRecord = null;
            labelPrimaryId.Text = string.Empty;
            labelTitle.Text = string.Empty;
            labelSurname.Text = string.Empty;
            labelFirstName.Text = string.Empty;
            labelOtherName.Text = string.Empty;
            labelGender.Text = string.Empty;

            pictureBoxUserPhoto.Image = null;
            pictureBoxUserPhoto.Refresh();

            pictureBoxSignature.Image = null;
            pictureBoxSignature.Refresh();

            var leftHandControls = groupBoxLeftHand.Controls;
            var rightHandControls = groupBoxRightHand.Controls;

            foreach (var control in leftHandControls)
            {
                string controlName = control.GetType().Name;
                if (controlName == "PictureBox")
                {
                    ((PictureBox)control).Image = null;
                    ((PictureBox)control).Refresh();
                }
            }
            foreach (var control in rightHandControls)
            {
                string controlName = control.GetType().Name;
                if (controlName == "PictureBox")
                {
                    ((PictureBox)control).Image = null;
                    ((PictureBox)control).Refresh();
                }
            }
        }

        private void pictureBoxUserPhoto_DoubleClick(object sender, EventArgs e)
        {
            if (_biometricsRecord == null)
            {
                MessageBox.Show(@"Error. No Record is loaded. Cick Refresh to load the lattest record of Search for a record.");
                return;
            }
            //if (Settings.Default.UseFaceLift == "Yes")
            //{
            //    using (var streamReader = new StreamReader("pathToLicenseFile"))
            //    {
            //        var faceLiftLicense = streamReader.ReadToEnd();
            //        var faceLift = new AchateUnit(pictureBoxUserPhoto, pictureBoxUserPhoto.Width, pictureBoxUserPhoto.Height, faceLiftLicense, Settings.Default.FaceListLicense);
            //        faceLift.ShowDialog();
            //        if (faceLift.DialogResult == DialogResult.OK)
            //        {
            //            pictureBoxUserPhoto.Image = faceLift.croppedImageClone;
            //            faceLift.Dispose();
            //        }
            //    }
            //}
            //else
            //{

            //new ImageForm(pictureBoxUserPhoto); 
            new FormCapturePhotograph( _capturePhotoNotifyer, _busyNofityer).ShowDialog();
            //}
        }

        public int GetActiveUserFingerRecordsCount()
        {
            return _biometricsRecord.GetActiveUserFingerRecordsCount();
        }
        
        public void UpdateFingerInit()
        {
            lblFingerprintScanInit.Text = @"Scanning in progress...";
        }
        public void CloseFingerScanner(FormCaptureSingleScannerFinger scanner)
        {
            scanner.Close();
            scanner.Dispose();
        }
        public void StartMySql(ServiceController mysqlServiceController)
        {
            mysqlServiceController.Start();
            mysqlServiceController.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 5));
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                _fingerReasons = new List<FingerReason>
                {
                    new FingerReason {Id = 0, Name = "-- Select option --"},
                    new FingerReason {Id = 1, Name = "Accidentally cut"},
                    new FingerReason { Id = 2, Name = "Arm is amputated" },
                    new FingerReason { Id = 3, Name = "Covered by tattoo" },
                    new FingerReason { Id = 4, Name = "Difficult to scan" },
                    new FingerReason { Id = 5, Name = "Mutilated fingerprint" },
                    new FingerReason { Id = 6, Name = "Naturally Unavailable" }
                };

                //Check if service is running
                var mysqlServiceControllers = ServiceController.GetServices().Where(x => x.ServiceName.ToLower().Contains("mysql")).ToArray();
                if (mysqlServiceControllers.Any())
                {
                    var mysqlServiceController = mysqlServiceControllers[0];
                    if (mysqlServiceController.Status == ServiceControllerStatus.Stopped || mysqlServiceController.Status == ServiceControllerStatus.Paused)
                    {
                        StartMySql(mysqlServiceController);

                        if (mysqlServiceController.Status != ServiceControllerStatus.StartPending || mysqlServiceController.Status != ServiceControllerStatus.Running)
                        {
                            StartMySql(mysqlServiceController);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(@"You need to install MySQL Server (version 5.7 or higher) for the application to run properly.");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"The MySQL Server (MySQL57 or above) could not be started automatically. Please go to services and start it manually");
                Application.Exit();
                Close();
                Dispose();
            }

            var sqlServices = GetSqlInstances();
            if (sqlServices.Any())
            {
                try
                {
                    var services = sqlServices.Where(s => s.ServiceName.Contains("MSSQL$SQLEXPRESS")).ToList();
                    if (services.Any())
                    {
                        if (services[0].Status != ServiceControllerStatus.Running && services[0].Status != ServiceControllerStatus.StartPending)
                        {
                            services[0].Start();
                        }
                    }
                    else
                    {
                        MessageBox.Show(@"You need to install SQL Server (SQLEXPRESS) for the application to run properly.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"The SQL Server (SQLEXPRESS) could not be started automatically. Please go to services and start it manually");
                }
            }

            try
            {
                LoadFingerIndexReasons();
                var components = "Biometrics.FingerExtraction,Biometrics.FingerMatching,Biometrics.FaceExtraction,Biometrics.FaceMatching,Biometrics.FaceDetection,Devices.Cameras,Devices.FingerScanners,Images.WSQ";
                var requiredComponents = components.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < requiredComponents.Length; i++)
                {
                    var item = requiredComponents[i];
                    if (NLicense.IsComponentActivated(item))
                    {
                        toolStripStatusLabelLicenses.ForeColor = Color.Green;
                        toolStripStatusLabelLicenses.Text += @" " + item;
                    }
                    else
                    {
                        toolStripStatusLabelLicenses.ForeColor = Color.Red;
                        toolStripStatusLabelLicenses.Text += @" " + item;
                    }
                    if (i != requiredComponents.Length - 1)
                    {
                        toolStripStatusLabelLicenses.ForeColor = Color.Black;
                        toolStripStatusLabelLicenses.Text += " " + item;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                AppErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                if (ex.InnerException != null)
                {
                    AppErrorLogger.LogError(ex.InnerException.StackTrace, ex.InnerException.Source, ex.InnerException.Message);
                }
            }
        }

       
        private void LoadFingerIndexReasons()
        {
            var reasonControls = new List<ComboBox>();
            var leftReasonControls = panelReasonsLeft.Controls.OfType<ComboBox>().ToList();
            var rightReasonControls = panelReasonsRight.Controls.OfType<ComboBox>().ToList();
            reasonControls.AddRange(leftReasonControls);
            reasonControls.AddRange(rightReasonControls);
            reasonControls.ForEach(c =>
            {
                c.BindingContext = new BindingContext();
                c.DataSource = _fingerReasons;
                c.SelectedItem = _fingerReasons[0];
                c.DisplayMember = "Name";
                c.ValueMember = "Id";
            });
            //// ___________________ LEFT HAND ___________________
            ////cmbFingerReason1.BindingContext = new BindingContext();
            //cmbFingerReason1.DataSource = fingerReasons;
            //cmbFingerReason1.SelectedItem = fingerReasons[0];
            //cmbFingerReason1.DisplayMember = "Name";
            //cmbFingerReason1.ValueMember = "Id";

            //cmbFingerReason2.BindingContext = new BindingContext();
            //cmbFingerReason2.DataSource = fingerReasons;
            //cmbFingerReason2.SelectedItem = fingerReasons[0];
            //cmbFingerReason2.DisplayMember = "Name";
            //cmbFingerReason2.ValueMember = "Id";

            //cmbFingerReason3.BindingContext = new BindingContext();
            //cmbFingerReason3.DataSource = fingerReasons;
            //cmbFingerReason3.SelectedItem = fingerReasons[0];
            //cmbFingerReason3.DisplayMember = "Name";
            //cmbFingerReason3.ValueMember = "Id";

            //cmbFingerReason4.BindingContext = new BindingContext();
            //cmbFingerReason4.DataSource = fingerReasons;
            //cmbFingerReason4.SelectedItem = fingerReasons[0];
            //cmbFingerReason4.DisplayMember = "Name";
            //cmbFingerReason4.ValueMember = "Id";

            //cmbFingerReason5.BindingContext = new BindingContext();
            //cmbFingerReason5.DataSource = fingerReasons;
            //cmbFingerReason5.SelectedItem = fingerReasons[0];
            //cmbFingerReason5.DisplayMember = "Name";
            //cmbFingerReason5.ValueMember = "Id";

            ////_____________________________ RIGHT HAND ______________________
            //cmbFingerReason6.BindingContext = new BindingContext();
            //cmbFingerReason6.DataSource = fingerReasons;
            //cmbFingerReason6.SelectedItem = fingerReasons[0];
            //cmbFingerReason6.DisplayMember = "Name";
            //cmbFingerReason6.ValueMember = "Id";

            //cmbFingerReason7.BindingContext = new BindingContext();
            //cmbFingerReason7.DataSource = fingerReasons;
            //cmbFingerReason7.SelectedItem = fingerReasons[0];
            //cmbFingerReason7.DisplayMember = "Name";
            //cmbFingerReason7.ValueMember = "Id";

            //cmbFingerReason8.BindingContext = new BindingContext();
            //cmbFingerReason8.DataSource = fingerReasons;
            //cmbFingerReason8.SelectedItem = fingerReasons[0];
            //cmbFingerReason8.DisplayMember = "Name";
            //cmbFingerReason8.ValueMember = "Id";

            //cmbFingerReason9.BindingContext = new BindingContext();
            //cmbFingerReason9.DataSource = fingerReasons;
            //cmbFingerReason9.SelectedItem = fingerReasons[0];
            //cmbFingerReason9.DisplayMember = "Name";
            //cmbFingerReason9.ValueMember = "Id";

            //cmbFingerReason10.BindingContext = new BindingContext();
            //cmbFingerReason10.DataSource = fingerReasons;
            //cmbFingerReason10.SelectedItem = fingerReasons[0];
            //cmbFingerReason10.DisplayMember = "Name";
            //cmbFingerReason10.ValueMember = "Id";


        }

        private void UpdateStatusLabels()
        {
            toolStripStatusLabelPhoto.Text = _biometricsRecord.Photograph != null ? "Photograph: Yes" : "Photograph No";
            toolStripStatusLabelSign.Text = _biometricsRecord.Signature != null ? "Signature: Yes" : "Signature No";
            var totalFingers = _biometricsRecord.FingerprintRecords.Count();
            toolStripStatusLabelFingers.Text = @"Fingerprints: " + totalFingers.ToString();
        }

        private BiometricsRecord FetchDisplayRecordFromDb(DisplayRecordPosition position, BiometricsRecord biometricsRecord, UserAccountModel userProfile, out List<FingerprintReason> reasons)
        {
            BiometricsRecord result = null;
            var fingerReasons = new List<FingerprintReason>();
            try
            {
                result = !_saveToDb ? DatabaseOpperations.GetLattestRecord(_currentSessionProject, userProfile) : DatabaseOpperations.GetDisplayRecordFromDb(_currentSessionProject, userProfile, position, biometricsRecord, out fingerReasons);

                if (result.EnrollmentId == null)
                {
                    MessageBox.Show(@"No Record Found");
                }
                else
                {
                    UpdateUserRecordOnUi(result, _saveToDb);
                }
                reasons = fingerReasons;
                return result;
            }
            catch (Exception ex)
            {
                reasons = fingerReasons;
                return result;
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            _btnRefreshClicked = true;
            ResetSelectedFingerReasons();
            BiometricsRecord result;

            textBoxrecordId.Text = @"Enter a record Id Number";
            if (!_saveToDb)
            {
                result = DatabaseOpperations.GetLattestRecord(_currentSessionProject, _userProfile);
                if (result.EnrollmentId == null)
                {
                    MessageBox.Show(@"No Record Found");
                    return;
                }
                _currentDisplayedEnrollment = result;
                UpdateUserRecordOnUi(result, _saveToDb);
            }
            else
            {
                try
                {
                    List<FingerprintReason> reasons;
                    var position = DisplayRecordPosition.lattest;
                    result = FetchDisplayRecordFromDb(position, _currentDisplayedEnrollment, _userProfile, out reasons);
                    _currentDisplayedEnrollment = result;

                    if (reasons.Any())
                    {
                        PopulateSelectedFingerReasons(reasons);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Unable to load lattest record. Check Database Connection. Error - " + ex.Message);
                }
            }
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            var searchCriteria = textBoxrecordId.Text.Trim();
            ResetSelectedFingerReasons();
            if (string.IsNullOrEmpty(searchCriteria) || searchCriteria.Contains("Enter an Enrollment Id OR a Primary ID"))
            {
                MessageBox.Show(@"Please enter an Enrollment Id OR a Primary ID");
                return;
            }

            if (!_saveToDb)
            {
                var result = DatabaseOpperations.FindRecord(_currentSessionProject, searchCriteria, _userProfile);
                UpdateUserRecordOnUi(result, _saveToDb);
            }
            else
            {
                try
                {
                    List<FingerprintReason> reasons;
                    var result = DatabaseOpperations.GetEnrollmentByEnrollmentId(_currentSessionProject, searchCriteria, out reasons);

                    if (result.EnrollmentId == null)
                    {
                        MessageBox.Show(@"No Record Found");
                        return;
                    }

                    UpdateUserRecordOnUi(result, _saveToDb);
                    if (reasons.Any())
                    {
                        PopulateSelectedFingerReasons(reasons);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Unable to load lattest record. Check Database Connection. Error - " + ex.Message);
                }
            }
        }

        private void PopulateSelectedFingerReasons(List<FingerprintReason> reasons)
        {
            var reasonControls = new List<ComboBox>();
            var leftReasonControls = panelReasonsLeft.Controls.OfType<ComboBox>().ToList();
            var rightReasonControls = panelReasonsRight.Controls.OfType<ComboBox>().ToList();
            reasonControls.AddRange(leftReasonControls);
            reasonControls.AddRange(rightReasonControls);

            reasons.ForEach(f =>
            {
                reasonControls.ForEach(c =>
                {
                    if (c.Name == "cmbFingerReason" + f.FingerIndex)
                    {
                        c.Text = f.FingerReason;
                    }
                });
            });
        }

        private void ResetSelectedFingerReasons()
        {
            var reasonControls = new List<ComboBox>();
            var leftReasonControls = panelReasonsLeft.Controls.OfType<ComboBox>().ToList();
            var rightReasonControls = panelReasonsRight.Controls.OfType<ComboBox>().ToList();
            reasonControls.AddRange(leftReasonControls);
            reasonControls.AddRange(rightReasonControls);
            reasonControls.ForEach(c =>
            {
                c.SelectedItem = _fingerReasons[0];
                c.DisplayMember = "Name";
                c.ValueMember = "Id";
            });
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            var fingerReasons = new List<FingerprintReason>();
            ResetSelectedFingerReasons();
            if (!_btnRefreshClicked)
            {
                MessageBox.Show(@"Please click the 'Refresh' button First");
                return;
            }

            if (string.IsNullOrEmpty(_currentDisplayedEnrollment?.EnrollmentId))
            {
                MessageBox.Show(@"No record found");
                return;
            }

            if (!_saveToDb)
            {
                var result = DatabaseOpperations.PreviousRecord(_currentSessionProject, _biometricsRecord.TableId, _userProfile);

                if (string.IsNullOrEmpty(result.EnrollmentId))
                {
                    MessageBox.Show(@"No Record Found");
                    return;
                }
                UpdateUserRecordOnUi(result, _saveToDb);
            }
            else
            {
                try
                {
                    var position = DisplayRecordPosition.previous;
                    var result = FetchDisplayRecordFromDb(position, _currentDisplayedEnrollment, _userProfile, out fingerReasons);
                    if (!string.IsNullOrEmpty(result?.EnrollmentId))
                    {
                        _currentDisplayedEnrollment = result;
                        if (fingerReasons.Any())
                        {
                            PopulateSelectedFingerReasons(fingerReasons);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Unable to load lattest record. Check Database Connection. Error - " + ex.Message);
                }
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            var fingerReasons = new List<FingerprintReason>();
            ResetSelectedFingerReasons();
            if (!_btnRefreshClicked)
            {
                MessageBox.Show(@"Please click the 'Refresh' button First");
                return;
            }

            if (string.IsNullOrEmpty(_currentDisplayedEnrollment?.EnrollmentId))
            {
                MessageBox.Show(@"No record found");
                return;
            }

            if (!_saveToDb)
            {
                var result = DatabaseOpperations.PreviousRecord(_currentSessionProject, _biometricsRecord.TableId, _userProfile);

                if (result.EnrollmentId == null)
                {
                    MessageBox.Show(@"No Record Found");
                    return;
                }
                else
                {
                    UpdateUserRecordOnUi(result, _saveToDb);

                }
            }
            else
            {
                try
                {
                    DisplayRecordPosition position = DisplayRecordPosition.next;
                    var result = FetchDisplayRecordFromDb(position, _currentDisplayedEnrollment, _userProfile, out fingerReasons);
                    if (!string.IsNullOrEmpty(result?.EnrollmentId))
                    {
                        _currentDisplayedEnrollment = result;
                        if (fingerReasons.Any())
                        {
                            PopulateSelectedFingerReasons(fingerReasons);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Unable to load lattest record. Check Database Connection. Error - " + ex.Message);
                }
            }
        }
        private void FingerprintBoxClick(object sender, EventArgs e)
        {
            if (_biometricsRecord == null)
            {
                MessageBox.Show(@"Error. No Record is loaded. Cick Refresh to load the lattest record of Search for a record.");
                return;
            }

            var fingerBoxName = ((PictureBox)sender).Name;

            if (fingerBoxName != "pictureBoxLFLittle")
            {
                MessageBox.Show(@"Please start fingerprint scanning from the Left Little Figer");
                return;
            }
            
            var fingersCount = GetActiveUserFingerRecordsCount();
            FingerDescription desc;
            if (fingersCount < 1)
            {
                desc = FingerDescription.LFLittle;
            }
            else
            {
                if (fingersCount < 10)
                {
                    var increment = fingersCount;
                    increment += 1;
                    desc = (FingerDescription)increment;
                }
                else
                {
                    desc = (FingerDescription)fingersCount;
                }
            }

            lblFingerprintScanInit.Text = @"Working on it. Please wait....";
            new FormCaptureSingleScannerFinger(this, _captureFingerNotifyer, desc, fingersCount).ShowDialog();
            lblFingerprintScanInit.Text = @"Click the green 'Little' to start scanning";
        }
        private void FingerprintBoxDoubleClick(object sender, EventArgs e)
        {
            if (_biometricsRecord == null)
            {
                MessageBox.Show(@"Error. No Record is loaded. Cick Refresh to load the lattest record of Search for a record.");
                return;
            }

            var fingerBoxName = ((PictureBox)sender).Name.Replace("pictureBox", "");

            if (string.IsNullOrEmpty(fingerBoxName))
            {
                MessageBox.Show(@"A systematic error has been encountered. Please restart the application and try again");
                return;
            }

            var desc = (FingerDescription)Enum.Parse(typeof(FingerDescription), fingerBoxName);
            var fingersCount = GetActiveUserFingerRecordsCount();
            lblFingerprintScanInit.Text = @"Working on it. Please wait....";
            new FormCaptureSingleScannerFinger(this, _captureFingerNotifyer, desc, fingersCount).ShowDialog();
            lblFingerprintScanInit.Text = @"Click the green 'Little' to start scanning";
        }
        private async void buttonSave_Click(object sender, EventArgs e)
        {
            if (!_appLoginState)
            {
                return;
            }

            labelStatus.Text = @"Validating Enrollment Data...";
            //Validate User Biometrics Record
            string validationErrorMsg;
            if (!ValidateUserRecord(_biometricsRecord, out validationErrorMsg))
            {
                MessageBox.Show(validationErrorMsg);
                return;
            }

            var fingerprintReasons = new List<FingerprintReason>();
            var reasonControls = new List<ComboBox>();
            var leftReasonControls = panelReasonsLeft.Controls.OfType<ComboBox>().ToList();
            var rightReasonControls = panelReasonsRight.Controls.OfType<ComboBox>().ToList();
            reasonControls.AddRange(leftReasonControls);
            reasonControls.AddRange(rightReasonControls);
            reasonControls.ForEach(c =>
            {
                if ((int)c.SelectedValue > 0)
                {
                    var fingerIndex = int.Parse(c.Name.Replace("cmbFingerReason", ""));
                    fingerprintReasons.Add(new FingerprintReason
                    {
                        FingerIndex = fingerIndex,
                        EnrollmentId = _biometricsRecord.EnrollmentId,
                        FingerReason = c.Text,
                        DateLastUpdated = DateTime.Now
                    });
                }
            });

            if (_biometricsRecord.FingerprintRecords.Count < 10 && !fingerprintReasons.Any())
            {
                MessageBox.Show(@"Since the captured fingerprints are incomplete (less than 10), you need to specify the Reason(s)", @"Enrollment Processing");
                labelStatus.Text = @"Ready";
                return;
            }

            labelStatus.Text = @"Extracting Fresh Templates And Saving. Please Wait...";
            await Task.Run(() =>
            {
                //Save to File or Database
                if (Settings.Default.SaveBiometricsToDBOption)
                {
                    DatabaseOpperations.SaveBiometricsRecordToDb(_biometricsRecord, fingerprintReasons, DatabaseOpperations.localConnectionString);
                }
                else
                {
                    new FileHelper().SaveBiometricsRecords(_biometricsRecord, _saveFileDir);
                }
            });

            MessageBox.Show(@"Records Saved Succesfully");
            labelStatus.Text = @"Ready";
            ResetSelectedFingerReasons();
            Invoke(new Action(() =>
            {
                var captured = DatabaseOpperations.GetOnlyCaptureUpdateStatus(_currentSessionProject, _userProfile.ProfileId);
                todayCapture.Text = captured.ToString();
            }));
        }

        private bool ValidateUserRecord(BiometricsRecord _BiometricsRecord, out string ValidationErrorMsg)
        {
            if (_BiometricsRecord == null)
            {
                ValidationErrorMsg = "Error. No Record is loaded. Cick Refresh to load the lattest record of Search for a record.";
                return false;
            }

            ValidationErrorMsg = string.Empty;
            if (_BiometricsRecord.PhotographTemplate == null)
            {
                ValidationErrorMsg = "Photo does not have template. Please recapture photo.";
                return false;
            }
            return true;
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(@"Do you want to exit the Crims Biometrics Capture Application?", @"Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                Cef.Shutdown();
            }
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
        }
        
        private void ShowSyncTool()
        {
            if (_crimsSync != null && _crimsSync.Visible)
            {
                _crimsSync.BringToFront();
                return;
            }
            if (_crimsSync == null)
            {
                _crimsSync = new CrimsSync(_userProfile);
                _crimsSync.Show();
            }
            else
            {
                _crimsSync.Dispose();
                _crimsSync = null;
                _crimsSync = new CrimsSync(_userProfile);
                _crimsSync.Show();
            }
        }
        public void ShowLogin()
        {
            if (_login == null)
            {
                //_Login = new Login(_LoginNotifyer);
                _login.ShowDialog();
            }
            else
            {
                _login.Dispose();
                _login = null;
                //_Login = new Login(_LoginNotifyer);
                _login.ShowDialog();
            }

        }
    
        private void pictureBoxSignature_DoubleClick(object sender, EventArgs e)
        {
            if (_biometricsRecord == null)
            {
                MessageBox.Show(@"Error. No Record is loaded. Cick Refresh to load the lattest record of Search for a record.");
                return;
            }
            new FormCaptureSignature(_captureSignatureNotifyer).ShowDialog();
        }
        private void wSQConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new WSQConverter().ShowDialog();
        }

        #region Web Browser
        public ChromiumWebBrowser ChromeBrowser;
        public void InitializeChromium()
        {
            try
            {
                var url = Settings.Default.WebEnrollUrl;
                ChromeBrowser = new ChromiumWebBrowser(url);
                panel6.Controls.Add(ChromeBrowser);
                ChromeBrowser.Dock = DockStyle.Fill;
                ChromeBrowser.LoadingStateChanged += ChromeView_NavStateChanged;
                ChromeBrowser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
                ChromeBrowser.RegisterAsyncJsObject("callbackObj", this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                try
                {
                    AppErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);

                    if (ex.InnerException != null)
                    {
                        AppErrorLogger.LogError(ex.InnerException.StackTrace, ex.InnerException.Source, ex.InnerException.Message);
                    }
                }
                catch (Exception ex1)
                {
                    MessageBox.Show(ex1.Message);
                }
            }

        }
        public void ShowDevTools()
        {
            ChromeBrowser.ShowDevTools(); ;
        }

        public void OnIsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs args)
        {
            if (args.IsBrowserInitialized)
            {
                //Stuffs i'll do after the browser is done initialising

            }
        }
        public void LogOut()
        {
            _userProfile = null;
            Invoke(new Action(() =>
            {
                logoutToolStripMenuItem.Text = @"You are not logged in";

                ResetUserRecordsOnUi();
            }));

        }

        //The names of the functions below must start with lower case 
        //so that the javascript call can find them
        //'Invoke(new Action(() =>{}); This helps to access the controls of 
        //the main thread that created this form when a javascript callback occurs
        public void loginSuccessful(string userInfo)
        {
            if (string.IsNullOrEmpty(userInfo))
            {
                MessageBox.Show(@"An error was encountered. The Login process could not be completed");
                return;
            }
            var userDetails = JsonConvert.DeserializeObject<UserAccountModel>(userInfo);
            if (string.IsNullOrEmpty(userDetails.ProfileId))
            {
                MessageBox.Show(@"An error was encountered. The Login process could not be completed");
                return;
            }
            _userProfile = userDetails;
            Invoke(new Action(() =>
            {
                LoginNotifyer(true);
            }));
            
        }
        public void setCurrentSessionProject(string currentProject)
        {
            if (string.IsNullOrEmpty(currentProject))
            {
                MessageBox.Show(@"An error was encountered. Please restart the application and sign in again.");
                return;
            }
            var project = JsonConvert.DeserializeObject<Project>(currentProject);
            if (string.IsNullOrEmpty(project?.ProjectCode))
            {
                MessageBox.Show(@"An error was encountered. Please restart the application and sign in again.");
                return;
            }
            _currentSessionProject = project;
            Invoke(new Action(() =>
            {
                lblCurrentSessionProject.Text = _currentSessionProject.ProjectName + @" (" + _currentSessionProject.ProjectCode + @")";
                int synched;
                var captured = DatabaseOpperations.GetCaptureSyncUpdateStatus(_currentSessionProject, _userProfile.ProfileId, out synched);
                todayCapture.Text = captured.ToString();
                todaySync.Text = synched.ToString();
            }));
        }

        public void logOut()
        {
            LogOut();
        }
        public void findRecord(string enrollmentId)
        {
            if (string.IsNullOrEmpty(enrollmentId))
            {
                MessageBox.Show(@"Invalid selection");
                return;
            }

            Invoke(new Action(() =>
            {
                tabControl1.SelectTab(tabPage1);
                textBoxrecordId.Text = string.Empty;
                textBoxrecordId.Text = enrollmentId;
                buttonFind.PerformClick();
            }));
        }

        //Event listener
        private void ChromeView_NavStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                if (!e.IsLoading)
                {
                    pbPreloader.Visible = false;
                    ChromeBrowser.Visible = true;
                }
                else
                {
                    ChromeBrowser.Visible = false;
                    pbPreloader.Visible = true;
                }
            }));

        }
        private void lnkLblRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChromeBrowser.Reload();
        }
        #endregion

        #region SYNCHRONISATIONS AND BACKUPS
        IList<BaseData> _baseDatas = new List<BaseData>();
        List<FingerprintReason> _fingerprintReasons = new List<FingerprintReason>();
        private async void buttonLoadRecords_Click(object sender, EventArgs e)
        {
            try
            {
                ResetUi();

                //Sync Approvals from Remote(Site) Server
                toolStripStatusLabel.Text = @"Checking for and synchronising Approvals if any. Please Wait...";
                DatabaseOpperations.SyncApprovals(_currentSessionProject);

                toolStripStatusLabel.Text = @"Loading data to be synchronised. Please Wait...";
                var totalRecords = DatabaseOpperations.GetTotalRecordCount(_currentSessionProject);
                List<FingerprintReason> fingerprintReasons = new List<FingerprintReason>();
                var result = await Task.Run(() =>
                {
                    var input = new SyncBacklogInput
                                {
                                    SyncMode = GetSyncMode(),
                                    FilterStart = dateTimePickerFrom.Value.Date,
                                    FilterEnd = dateTimePickerTo.Value.Date.Add(new TimeSpan(23, 59, 59)),
                                    EnrollmentId = singleRecordID.Text
                                };

                    //Load data whose ApprovalStatus == pending for synchronisation using the filter option : 'input'
                    return DatabaseOpperations.GetSyncBacklog(_currentSessionProject, input, out fingerprintReasons);
                });

                //Retrieve BaseData Result
                _baseDatas = result;
                _fingerprintReasons = fingerprintReasons;
                //Display Result Count on the Screen
                labelTotalRecords.Text = totalRecords.ToString();
                labelRecordInSyncQueue.Text = _baseDatas.Any() ? _baseDatas.Count().ToString() : "0";
                toolStripStatusLabel.Text = @"Data Loaded for Synchronisation";
                DisableUI(true);
            }
            catch (Exception ex)
            {
                toolStripStatusLabel.Text = ex.Message;
                DisableUI(false);
            }
        }
        private void ResetUi()
        {
            labelTotalRecords.Text = string.Empty;
            labelRecordInSyncQueue.Text = string.Empty;
            labelSyncStartTime.Text = string.Empty;
            labelSyncEndTime.Text = string.Empty;
            labelTotalSuccess.Text = string.Empty;
            labelTotalFailed.Text = string.Empty;
            DisableUI(false);
        }
        private void ResetExportUi()
        {
            lblTotalExport.Text = string.Empty;
            lblExportStart.Text = string.Empty;
            lblExportEnd.Text = string.Empty;
        }
        private void ResetImportUi()
        {
            lblTotalImportCount.Text = string.Empty;
            lblTotalImported.Text = string.Empty;
            lblImportStart.Text = string.Empty;
            lblImportEnd.Text = string.Empty;
        }
        private void DisableUI(bool v)
        {
            buttonBeginSync.Enabled = v;
            buttonLoadRecords.Enabled = v;
            buttonCancelSync.Enabled = !v;
            buttonResetSyncHistory.Enabled = v;
        }
        private void DisableUIx(bool v)
        {
            buttonBeginSync.Enabled = v;
            buttonCancelSync.Enabled = !v;
            buttonResetSyncHistory.Enabled = v;
        }
        private SyncMode GetSyncMode()
        {
            if (radioButtonAll.Checked)
                return SyncMode.AllPending;
            if (radioButtonFilter.Checked)
                return SyncMode.Filtered;
            if (radioButtonOne.Checked)
                return SyncMode.Specific;
            else
                return SyncMode.None;
        }
        private SyncMode GetBackUpMode()
        {
            if (rdAllBackup.Checked)
                return SyncMode.AllPending;
            if (rdDateRangeBackup.Checked)
                return SyncMode.Filtered;
            if (rdSingleBackup.Checked)
                return SyncMode.Specific;
            else
                return SyncMode.None;
        }
        private void buttonBeginSync_Click(object sender, EventArgs e)
        {
            SyncEnrollmentData();
        }
        private void UpdateSynchronisationProgress(int update)
        {
            labelTotalSuccess.Text = update.ToString();
            if (update == _baseDatas.Count)
            {
                if (_fingerprintReasons.Any())
                {
                   var reasonStatus = DatabaseOpperations.SynchroniseFingerprintReasons(_fingerprintReasons);
                    if (reasonStatus)
                    {
                        //Log Sync End Time for Sync Job
                        DatabaseOpperations.CloseSyncJobSession(_syncJobId);
                    }
                }
                toolStripStatusLabel.Text = @"Sync Job Completed Succesfully";
                labelSyncEndTime.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

                var previousSync = todaySync.Text;
                if (previousSync != "0")
                {
                    int outInt;
                    var parseRes = int.TryParse(previousSync, out outInt);
                    if (parseRes && outInt > 0)
                    {
                        todaySync.Text = (outInt + update).ToString();
                    }
                    else
                    {
                        todaySync.Text = update.ToString();
                    }
                }
                else
                {
                    todaySync.Text = update.ToString();
                }
            }
        }
        private string _syncJobId = "";
        //private int _dataCount = 0;
        private void SyncEnrollmentData()
        {
            try
            {
                if (_userProfile == null)
                {
                    toolStripStatusLabel.Text = @"Sync Job Terminated, User Must Login";
                    return;
                }

                //Scroll through records in result
                if (_baseDatas.Any())
                {
                    toolStripStatusLabel.Text = @"Sync Job Processing, Please Wait...";
                    var startDate = DateTime.Now;
                    labelSyncStartTime.Text = startDate.ToString("dd/MM/yyyy hh:mm tt");
                    DisableUI(false);

                    //0. Create Entry into the SyncJobLog
                    var syncJobId = DatabaseOpperations.CreateSyncJobSession(_userProfile, startDate);
                    if (string.IsNullOrEmpty(syncJobId))
                    {
                        MessageBox.Show(@"The Synchronisation process could not be initiated. Please try again later", @"Data Synchronisation");
                        return;
                    }
                    _syncJobId = syncJobId;
                    //1. Find User Custom Datas
                    var i = 0;//This is the counter to Updating UI on total Completed So For
                    _baseDatas.ForEach(async res =>
                    {
                        var enrollmentRecordInfo = await Task.Run(() =>
                        {
                            try
                            {
                                var enrollmentRecordDetails = DatabaseOpperations.GetEnrollment(res.EnrollmentId);
                                return enrollmentRecordDetails;
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                                return new EnrollmentRecord();
                            }
                        });

                        if (enrollmentRecordInfo == null || !enrollmentRecordInfo.CustomDatas.Any())
                        {
                            MessageBox.Show(@"An error was encountered. Data to be synchronised could not be retrieved");
                            return;
                        }

                        enrollmentRecordInfo.BaseData = res;

                        if (!enrollmentRecordInfo.CustomDatas.Any())
                        {
                            MessageBox.Show(@"This Enrollment will not be Synchronised with empty Custom data!");
                            return;
                        }

                        if (!enrollmentRecordInfo.FingerprintImages.Any())
                        {
                            MessageBox.Show(@"This Enrollment will not be Synchronised without Fingerprints");
                            return;
                        }

                        if (string.IsNullOrEmpty(enrollmentRecordInfo.Photograph?.EnrollmentId))
                        {
                            MessageBox.Show(@"This Enrollment will not be Synchronised without Fingerprints");
                            return;
                        }

                        await Task.Run(() => DatabaseOpperations.SynchroniseDataToServer(enrollmentRecordInfo));

                        //5. Update Local Database SyncJobHistoy for specific Record
                        DatabaseOpperations.UpdateDbSyncHistory(syncJobId, res.EnrollmentId);

                        i++; // IncreamentItems Completed Counter
                        UpdateSynchronisationProgress(i);
                    });
                    
                    toolStripStatusLabel.Text = @"Synchronisation in progress....";
                    DisableUI(true);
                }
                else
                {
                    toolStripStatusLabel.Text = @"No Records to Synchronize. Try Applying a Filter.";
                    DisableUI(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                DisableUI(false);
            }
        }
        private void buttonCancelSync_Click(object sender, EventArgs e)
        {
            //Stop Sync Opperations
        }
        private async void buttonResetSyncHistory_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, @"Are you sure you want to delete all Sync History on this System?", @"DELETE SYNC HISTORY", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //Load Sync Job History
                toolStripStatusLabel.Text = @"Deleting Sync Job History, Please Wait...";
                DisableUI(false);

                await Task.Run(() =>
                {
                    DatabaseOpperations.ResetSyncHistory();
                });

                toolStripStatusLabel.Text = @"Sync Job History Deleted Succesfully";
                DisableUI(true);
            }
        }
        private void InitExport()
        {
            try
            {
                var backupMode = GetBackUpMode();
                if (backupMode == SyncMode.Specific)
                {
                    if (string.IsNullOrEmpty(txtSingleBackup.Text))
                    {
                        MessageBox.Show(@"Please provide the specific EnrolleeId for Export");
                        return;
                    }
                }

                var input = new SyncBacklogInput
                {
                    SyncMode = GetBackUpMode(),
                    FilterStart = backupStartDate.Value,
                    FilterEnd = backupEndDate.Value,
                    EnrollmentId = txtSingleBackup.Text
                };

                //Load data for backup using the filter option : 'input'
                var enrollmentBackups = DatabaseOpperations.GenerateBackup(_currentSessionProject, input);
                if (!enrollmentBackups.Any())
                {
                    MessageBox.Show(@"Data could not be retrieved for Export. Please try again later");
                    return;
                }

                Invoke(new Action(() =>
                {
                    //Display Result Count on the Screen
                    lblTotalExport.Text = enrollmentBackups.Count.ToString();
                    lblExportStart.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                    toolStripStatusLabel.Text = @"Data Export Started";
                }));

                var fileName = "crimsBackup_" + DateTime.Now.ToString("dd/MM/yyyy").Replace("/", "_") + ".json";
                var filePath = Path.Combine(txtSaveExportDirectory.Text, fileName);
                File.WriteAllText(filePath, JsonConvert.SerializeObject(enrollmentBackups));
                if (File.Exists(filePath))
                {
                    Invoke(new Action(() =>
                    {
                        lblExportEnd.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                        toolStripStatusLabel.Text = @"Data Export was successfully Completed";
                    }));

                    // suppose that we have a test.txt at E:\
                    if (!File.Exists(filePath))
                    {
                        return;
                    }

                    // combine the arguments together
                    // it doesn't matter if there is a space after ','
                    string argument = "/select, \"" + filePath + "\"";

                    Process.Start("explorer.exe", argument);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private string _importFilePath;
        public void UpdateActivityStatus(string message)
        {
            Invoke(new Action(() =>
            {
                toolStripStatusLabel.Text = message;
            }));
        }
        public void UpdateUi(int current)
        {
            _totalImported += current;
            Invoke(new Action(() =>
            {
                lblTotalImported.Text = _totalImported.ToString();
                if (_totalImported == _totalImportCount)
                {
                    lblImportEnd.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                    toolStripStatusLabel.Text = @"Data Import has been Completed";
                }
            }));
        }
        private void InitImport()
        {
            try
            {
                var enrollmentBackups = new List<EnrollmentBackup>();
                UpdateActivityStatus(@"Retrieving backup data. Please wait...");

                _backupFiles.ForEach(f =>
                {
                    if (!File.Exists(f))
                    {
                        return;
                    }

                    var json = File.ReadAllText(f);
                    if (string.IsNullOrEmpty(json))
                    {
                        return;
                    }
                    var tempList = JsonConvert.DeserializeObject<List<EnrollmentBackup>>(json);
                    if (!tempList.Any() || tempList.Any(b => b.BaseData == null) || tempList.Any(b => !b.FingerprintImages.Any()))
                    {
                        return;
                    }
                    enrollmentBackups.AddRange(tempList);
                });

                if (!enrollmentBackups.Any())
                {
                    MessageBox.Show(@"No backup data could be retrieved. Please try again and make sure the backup file(s) are valid", @"Data backup");
                    return;
                }

                _totalImportCount = enrollmentBackups.Count;

                Invoke(new Action(() =>
                {
                    lblTotalImportCount.Text = _totalImportCount.ToString();
                    lblImportStart.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                    toolStripStatusLabel.Text = @"Backing up data to server. Please wait...";
                    BackupData(enrollmentBackups);
                }));

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private async void BackupData(List<EnrollmentBackup> enrollmentBackups)
        {
            await Task.Run(() => DatabaseOpperations.ImportFromBackup(enrollmentBackups, this));
        }
        private void btnExportData_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtSaveExportDirectory.Text))
                {
                    MessageBox.Show(@"Please select the 'Data Export Save Directory'");
                    return;
                }

                ResetExportUi();
                toolStripStatusLabel.Text = @"Loading data for Export. Please Wait...";
                var newThread = new Thread(InitExport);
                newThread.SetApartmentState(ApartmentState.STA);
                newThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnStartImport_Click(object sender, EventArgs e)
        {
            try
            {
                ResetImportUi();

                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.Description = @"Select Backup files folder";
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        if (string.IsNullOrEmpty(fbd.SelectedPath))
                        {
                            return;
                        }

                        var backupFiles = Directory.GetFiles(fbd.SelectedPath, "*.json", SearchOption.TopDirectoryOnly).ToList();
                        if (!backupFiles.Any())
                        {
                            MessageBox.Show(@"No backup file(s) found", @"Dat Backup");
                            return;
                        }
                        lblTotalImportCount.Text = @"0";
                        lblImportStart.Text = @"N/A";
                        lblImportEnd.Text = @"N/A";
                        _backupFiles = new List<string>();
                        lblBackupFilesFound.Text = backupFiles.Count.ToString();
                        _backupFiles = backupFiles;
                        var newThread = new Thread(InitImport);
                        newThread.SetApartmentState(ApartmentState.STA);
                        newThread.Start();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void SyncEnrollmentDataForms()
        {
            try
            {
                if (_baseDatas == null || !_baseDatas.Any())
                {
                    MessageBox.Show(@"Please Click on the 'Sync Approvals and Load Records' button first!");
                    return;
                }

                var destinationUrl = Settings.Default.dataFormsDestination;
                if (string.IsNullOrEmpty(destinationUrl))
                {
                    MessageBox.Show(@"Please provide the destination URL for synchronising data forms in the settings menu");
                    return;
                }
                var project = _currentSessionProject;
                if (string.IsNullOrEmpty(project?.ProjectCode))
                {
                    MessageBox.Show(@"An Internal server error was encountered. The action was terminated. Please contact our support team.");
                    return;
                }

                var subfolders = new List<string>();
                var input = new SyncBacklogInput
                {
                    SyncMode = GetBackUpMode(),
                    FilterStart = backupStartDate.Value,
                    FilterEnd = backupEndDate.Value,
                    EnrollmentId = txtSingleBackup.Text
                };

                var rootPath = Settings.Default.SavedFilesDir;
                if (!rootPath.Contains(project.ProjectCode))
                {
                    rootPath = Path.Combine(rootPath, project.ProjectCode);
                }

                if (input.SyncMode == SyncMode.Specific)
                {
                    subfolders = new List<string> { Path.Combine(rootPath, input.EnrollmentId) };
                }

                if (input.SyncMode == SyncMode.Filtered || input.SyncMode == SyncMode.AllPending)
                {
                    subfolders = new List<string>();
                    _baseDatas.ForEach(b =>
                    {
                        subfolders.Add(Path.Combine(rootPath, b.EnrollmentId));
                    });
                }

                if (!subfolders.Any())
                {
                    MessageBox.Show(@"There are no data forms to synchronise.");
                    return;
                }
                var message = new HttpRequestMessage();
                var content = new MultipartFormDataContent();
                subfolders.ForEach(d =>
                {
                    if (!Directory.Exists(d))
                    {
                        return;
                    }
                    var myFiles = Directory.GetFiles(d);
                    if (myFiles.Any())
                    {
                        var dirName = new DirectoryInfo(d).Name;
                        if (!string.IsNullOrEmpty(dirName))
                        {
                            var files = myFiles.Where(s =>
                            {
                                var fileName = Path.GetFileName(s);
                                return fileName != null && fileName.Contains(dirName.Replace("-", "") + "_dataForm");
                            }).ToList();

                            if (files.Any())
                            {
                                var filestream = new FileStream(files[0], FileMode.Open);
                                var fName = Path.GetFileName(files[0]);
                                content.Add(new StreamContent(filestream), dirName, fName);
                            }

                        }
                    }

                });

                if (!content.Any())
                {
                    MessageBox.Show(@"The data forms to be synchronised could not be retrieved.");
                    return;
                }
                message.Method = HttpMethod.Post;
                message.Content = content;
                message.RequestUri = new Uri(destinationUrl + "?projectCode=" + project.ProjectCode);
                var client = new HttpClient();
                string ty = null;
                try
                {
                    await client.SendAsync(message).ContinueWith(task =>
                    {
                        if (task.Result.IsSuccessStatusCode)
                        {
                            ty = task.Result.Content.ReadAsStringAsync().Result;
                        }
                        return string.Empty;
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                var result = JsonConvert.DeserializeObject<List<FileDesc>>(ty);
                Invoke(new Action(() =>
                {
                    toolStripStatusLabel.Text = @"Enrollment Data forms Synchronisation. Completed";
                }));
                MessageBox.Show(result.Count + @" Data form(s) were successfully synchronised.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnSyncDataForm_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = @"Synchronising Enrollment Data forms. Please Wait... ";
            var newThread = new Thread(SyncEnrollmentDataForms);
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
        }
        private void btnBrowseExportDirectory_Click(object sender, EventArgs e)
        {
            if (fdbSaveExport.ShowDialog() == DialogResult.OK)
            {
                txtSaveExportDirectory.Text = fdbSaveExport.SelectedPath;
            }
        }
        private async void btnSyncUserAccounts_Click(object sender, EventArgs e)
        {
            try
            {
                //Download User Profiles
                toolStripStatusLabel.Text = @"Searching and Downloading New User Profiles. Please Wait... ";
                await Task.Run(() => DatabaseOpperations.DownloadNewUserProfiles());
                toolStripStatusLabel.Text = @"User Profiles Update Completed ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        
       
    }
}
