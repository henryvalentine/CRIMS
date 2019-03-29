using Crims.Data.Models;
using Crims.UI.Win.Enroll.Classes;
using Crims.UI.Win.Enroll.DataServices;
using Crims.UI.Win.Enroll.Enums;
using Crims.UI.Win.Enroll.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows.Forms;
using Crims.UI.Win.Enroll.Helpers;
using Newtonsoft.Json;
using WebGrease.Css.Extensions;

namespace Crims.UI.Win.Enroll.Forms
{
    public partial class CrimsSync : Form
    {
        IList<BaseData> _BaseDatas = null;
        IList<SyncJobHistory> _SyncJobHistory = null;

        //Directory where files will be retrieved.
        private string saveFileDir = Settings.Default.SavedFilesDir;
        private string destFileDir = Settings.Default.syncServerFilePath;
        private UserAccountModel _UserProfile;

        public CrimsSync()
        {
            InitializeComponent();
        }

        public CrimsSync(UserAccountModel userProfile)
        {
            _UserProfile = userProfile;

            InitializeComponent();
        }

        private void Crims_Load(object sender, EventArgs e)

        {

        }

        private void CrimsSync_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private async void buttonLoadRecords_Click(object sender, EventArgs e)
        {
            ResetUi();

            //Sync Approvals from Remote(Site) Server
            toolStripStatusLabel.Text = @"Checking for and synchronising Approvals if any. Please Wait...";
            //DatabaseOpperations.SyncApprovals();
            
            toolStripStatusLabel.Text = @"Loading data to be synchronised. Please Wait...";
            //var totalRecords = DatabaseOpperations.GetTotalRecordCount(); 
            var reasons = new List<FingerprintReason>();
            var result = await Task.Run(() =>
            {
                SyncBacklogInput input = new SyncBacklogInput
                { 
                    SyncMode = GetSyncMode(),
                    FilterStart = dateTimePickerFrom.Value,
                    FilterEnd = dateTimePickerTo.Value,
                    EnrollmentId = textBoxRecordID.Text
                };

                //Load data whose ApprovalStatus == pending for synchronisation using the filter option : 'input'
                return DatabaseOpperations.GetSyncBacklog(new Project(), input, out reasons);
            });

            //Retrieve BaseData Result
            _BaseDatas = result;

            //Display Result Count on the Screen
            //labelTotalRecords.Text = totalRecords.ToString();
            labelRecordInSyncQueue.Text = _BaseDatas.Any() ? _BaseDatas.Count().ToString() : "0";
            
            toolStripStatusLabel.Text = @"Loading Completed";
            DisableUI(true);
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

        private async void buttonBeginSync_Click(object sender, EventArgs e)
        {
            await SynEnrollmentData();
        }

        private async Task SynEnrollmentData()
        {
            var progressHandler = new Progress<int>(value =>
            {
                labelTotalSuccess.Text = value.ToString();
            });
            var progress = (IProgress<int>) progressHandler;

            //Download User Profiles
            toolStripStatusLabel.Text = @"Searching and Downloading New User Profiles, Please Wait... ";
            await Task.Run(() => DatabaseOpperations.DownloadNewUserProfiles());

            toolStripStatusLabel.Text = @"User Profiles Update Completed ";

            if (_UserProfile == null)
            {
                toolStripStatusLabel.Text = @"Sync Job Terminated, User Must Login";
                return;
            }

            //Scroll through records in result
            if (_BaseDatas.Any())
            {
                toolStripStatusLabel.Text = @"Sync Job Processing, Please Wait...";
                var startDate = DateTime.Now;
                labelSyncStartTime.Text = startDate.ToString("dd/MM/yyyy hh:mm tt");
                DisableUI(false);

                //0. Create Entry into the SyncJobLog
                var syncJobId = DatabaseOpperations.CreateSyncJobSession(_UserProfile, startDate);
                
                //1. Find User Custom Datas
                var i = 0;//This is the counter to Updating UI on total Completed So For
                _BaseDatas.ForEach(async res =>
                {
                    var enrollmentRecordInfo = await Task.Run(() =>
                    {
                        try
                        {
                            var enrollmentRecordDetails = new EnrollmentRecord
                            {
                                CustomDatas = DatabaseOpperations.GetEnrolleeCustomData(res.EnrollmentId),
                                Photograph = DatabaseOpperations.GetEnrolleePhotoGraph(res.EnrollmentId),
                                FingerprintTemplate = DatabaseOpperations.GetEnrolleeFingerprintTemplate(res.EnrollmentId),
                                FingerprintImages = DatabaseOpperations.GetEnrolleeFingerprintImages(res.EnrollmentId),
                                Signature = DatabaseOpperations.GetEnrolleeSignature(res.EnrollmentId)
                            };
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
                    if (progress != null)
                    {
                        progress.Report(i);
                    }
                });

                //6. Log Sync End Time for Sync Job
                DatabaseOpperations.CloseSyncJobSession(syncJobId);
                toolStripStatusLabel.Text = @"Sync Job Completed Succesfully";
                labelSyncEndTime.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                DisableUI(true);


            }
            else
            {
                toolStripStatusLabel.Text = @"No Records to Synchronize. Try Applying a Filter.";
            }
        }

        private void buttonCancelSync_Click(object sender, EventArgs e)
        {
            //Stop Sync Opperations
        }
        
        /// <summary>
        /// This is a likely CPU Intensive Opperation. 
        /// The Method has been marked async and the CPU bound TASK(s) is being Excuted in a Background Thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CrimsSync_Shown(object sender, EventArgs e)
        {
            //Load Sync Job History
            toolStripStatusLabel.Text = @"Loading Sync Job History, Please Wait...";
            DisableUI(false);
            //var totalRecords = DatabaseOpperations.GetTotalRecordCount();

            var result = await Task.Run(() => DatabaseOpperations.GetSyncedJobHistory());
            toolStripStatusLabel.Text = @"Sync Job History Loaded Succesfully";
            _SyncJobHistory = (IList<SyncJobHistory>)result;
            DisableUI(true);
        }

        private async void buttonResetSyncHistory_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, @"Are you sure you want to delete all Sync History on this System?","DELETE SYNC HISTORY",MessageBoxButtons.YesNo) == DialogResult.Yes)
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

        private async Task<List<EnrollmentBackup>> InitExport()
        {
            try
            {
                ResetExportUi();
                toolStripStatusLabel.Text = @"Loading data for Export. Please Wait...";
                var result = await Task.Run(() =>
                {
                    var backupMode = GetBackUpMode();
                    if (backupMode == SyncMode.Specific)
                    {
                        if (string.IsNullOrEmpty(txtSingleBackup.Text))
                        {
                            MessageBox.Show(@"Please provide the specific EnrolleeId for Export");
                            return new List<EnrollmentBackup>();
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
                    return DatabaseOpperations.GenerateBackup(new Project(), input);
                });

                //Display Result Count on the Screen
                lblTotalExport.Text = result.Count.ToString();
                toolStripStatusLabel.Text = @"Data Loading Completed";
                return result;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return new List<EnrollmentBackup>();
            }
        }
        private async Task<List<EnrollmentBackup>> InitImport(List<EnrollmentBackup> imports)
        {
            try
            {
                ResetImportUi();
                toolStripStatusLabel.Text = @"Loading data for Export. Please Wait...";
                var result = await Task.Run(() => DatabaseOpperations.ImportFromBackup(imports));
                return result;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return new List<EnrollmentBackup>();
            }
        }
        private void btnExportData_Click(object sender, EventArgs e)
        {
            var enrollmentBackups = InitExport().Result;
            if (!enrollmentBackups.Any())
            {
                MessageBox.Show(@"Data could not be retrieved for Export. Please try again later");
                return;
            }
            
            using (var fdb = new FolderBrowserDialog())
            {
                if (fdb.ShowDialog() == DialogResult.OK)
                {
                   var folderPath = fdb.SelectedPath;
                    var fileName = "crimsBackup_" + DateTime.Now.ToString("dd/MM/yyyy").Replace("/", "_") + ".json";
                    var filePath = Path.Combine(folderPath, fileName);
                    lblExportStart.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                    File.WriteAllText(filePath, JsonConvert.SerializeObject(enrollmentBackups));
                    if (File.Exists(filePath))
                    {
                        lblExportEnd.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                        toolStripStatusLabel.Text = @"Data Export was successfully Completed";
                        Process.Start(fileName, filePath);
                    }
                }
            }
        }
        private void btnStartImport_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        ofd.Title = @"Select backup file";

                        if (string.IsNullOrEmpty(ofd.FileName))
                        {
                            return;
                        }

                        var folderPath = ofd.FileName;
                        lblImportStart.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                        var json = File.ReadAllText(folderPath);
                        if (string.IsNullOrEmpty(json))
                        {
                            MessageBox.Show(@"Backup file is either corrupt or empty. Action terminated.");
                            return;
                        }
                        var enrollmentBackups = JsonConvert.DeserializeObject<List<EnrollmentBackup>>(json);
                        if (!enrollmentBackups.Any() || enrollmentBackups.Any(b => b.BaseData == null) || enrollmentBackups.Any(b => !b.FingerprintImages.Any()))
                        {
                            MessageBox.Show(@"Backup file is either corrupt or empty. Action terminated.");
                            return;
                        }

                        var importedBackup = InitImport(enrollmentBackups).Result;

                        //Display Import Result Count on the Screen
                        lblTotalImported.Text = importedBackup.Count.ToString();
                        toolStripStatusLabel.Text = @"Data Import has been Completed";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task<string> SyncEnrollmentDataForms(string destinationUrl, string projectCode)
        {
            try
            {
                if (!_BaseDatas.Any())
                {
                    MessageBox.Show(@"Please Click on the 'Sync Approvals and Load Records' button!");
                    return string.Empty;
                }
                
                var subfolders = new List<string>();
                var input = new SyncBacklogInput
                {
                    SyncMode = GetBackUpMode(),
                    FilterStart = backupStartDate.Value,
                    FilterEnd = backupEndDate.Value,
                    EnrollmentId = txtSingleBackup.Text
                };

                if (input.SyncMode == SyncMode.Specific)
                {
                    subfolders = new List<string> {Path.Combine(Settings.Default.SavedFilesDir, input.EnrollmentId)};
                }

                if (input.SyncMode == SyncMode.Filtered || input.SyncMode == SyncMode.AllPending)
                {
                    subfolders = new List<string>();
                    _BaseDatas.ForEach(b =>
                    {
                        subfolders.Add(Path.Combine(Settings.Default.SavedFilesDir, b.EnrollmentId));
                    });
                }
                
                if (!subfolders.Any())
                {
                    MessageBox.Show(@"There are no data forms to synchronise.");
                    return string.Empty;
                }
                var message = new HttpRequestMessage();
                var content = new MultipartFormDataContent();
                subfolders.ForEach(d =>
                {
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
                    return string.Empty;
                }
                message.Method = HttpMethod.Post;
                message.Content = content;
                message.RequestUri = new Uri(destinationUrl + "?projectCode=" + projectCode);
                var client = new HttpClient();
                string ty = null;
                await client.SendAsync(message).ContinueWith(task =>
                {
                    if (task.Result.IsSuccessStatusCode)
                    {
                        ty = task.Result.Content.ReadAsStringAsync().Result;
                    }
                    return string.Empty;
                });
                return ty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
       
        private void btnSyncDataForm_Click(object sender, EventArgs e)
        {
            var destinationUrl = Settings.Default.dataFormsDestination;
            if (string.IsNullOrEmpty(destinationUrl))
            {
                MessageBox.Show(@"Please provide the destination URL for synchronising data forms in the settings menu");
                return;
            }
            var project = DatabaseOpperations.GetProject();
            if (string.IsNullOrEmpty(project?.ProjectCode))
            {
                MessageBox.Show(@"An Internal server error was encountered. The action was terminated. Please contact our support team.");
                return;
            }
            var fileSyncResult = SyncEnrollmentDataForms(destinationUrl, project.ProjectCode).Result;
            if (!string.IsNullOrEmpty(fileSyncResult))
            {
                var result = JsonConvert.DeserializeObject<List<FileDesc>>(fileSyncResult);
                MessageBox.Show(result.Count + @" Data form(s) were successfully synchronised.");
            }
           
        }
    }
}
