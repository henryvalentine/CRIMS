using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TandAProject.Services;
using TandAProject.Models;
using TandAProject.Utils.Serializer;
using TandAProject.Utils;

namespace TandAProject.Controls
{

    public partial class ImportExportControl : UserControl
    {
        AppSettings AppSettings = new AppSettings();

        //Initialise the Notification Deligate
        public ApplicationStateChangeNotifyer StateNotifyer;
        public ApplicationMessageNotifyer MessageNotifyer;

        BackgroundWorker exportBgWorker = new BackgroundWorker();
        BackgroundWorker importBgWorker = new BackgroundWorker();

        public ImportExportControl()
        {
            InitializeComponent();
            exportBgWorker.WorkerSupportsCancellation = true;
            exportBgWorker.WorkerReportsProgress = true;
            exportBgWorker.DoWork += new DoWorkEventHandler(exportBgWorker_DoWork);
            exportBgWorker.ProgressChanged += new ProgressChangedEventHandler(exportBgWorker_ProgressChanged);
            exportBgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(exportBgWorker_WorkerCompleted);

            importBgWorker.WorkerSupportsCancellation = true;
            importBgWorker.WorkerReportsProgress = true;
            importBgWorker.DoWork += new DoWorkEventHandler(importBgWorker_DoWork);
            importBgWorker.ProgressChanged += new ProgressChangedEventHandler(importBgWorker_ProgressChanged);
            importBgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(importBgWorker_WorkerCompleted);

        }

        #region Import 
        private void importBgWorker_WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageNotifyer("Error :" + e.Error.Message);
                Logger.logToFile(e.Error, AppSettings.ErrorLogPath);
            }

            else if (e.Cancelled)
            {
                MessageNotifyer("Error: Opperation was canceled");
            }
            else
            {
                if (e.Result != null)
                {
                    MessageNotifyer(Convert.ToString(e.Result));
                }
                else
                {
                    MessageNotifyer("Export Completed Successfully");
                }
            }


            EnableUIControls(true);
        }

        private void importBgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelStatus.Text = e.ProgressPercentage.ToString()+"%";
        }

        private void importBgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string fileName = (string)e.Argument;
            double bgWorkerProgress = 0.0;
            List<AttendanceLog> attendancelogs = JsonSerialization.ReadFromJsonFile<List<AttendanceLog>>(fileName);

            int totalProcessed = 0;
            foreach (AttendanceLog log in attendancelogs)
            {
                totalProcessed += DataServices.LogAttendanceClock(log);

                bgWorkerProgress = bgWorkerProgress += 1;
                var PercentageProgress = bgWorkerProgress / attendancelogs.Count;
                var PercentageComplete = PercentageProgress * 100;
                importBgWorker.ReportProgress(Convert.ToInt32(PercentageComplete), null);
            }
            e.Result = String.Format(" {0} Records imported succesfully from {1} found in {2}",totalProcessed,attendancelogs.Count,fileName);
        }

        #endregion

        #region Export
        private void exportBgWorker_WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageNotifyer("Error :" + e.Error.Message);
                Logger.logToFile(e.Error, AppSettings.ErrorLogPath);
            }

            else if (e.Cancelled)
            {
                MessageNotifyer("Error: Opperation was canceled");
            }
            else
            {
                if (e.Result != null)
                {
                    MessageNotifyer(Convert.ToString(e.Result));
                }
                else
                {
                    MessageNotifyer("Export Completed Successfully");
                }
            }
            

            EnableUIControls(true);
        }

        private void exportBgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelStatus.Text = e.ProgressPercentage.ToString() + "%";
        }

        private void exportBgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ExportParams exportParams = (ExportParams)e.Argument;
            //Load Attendance Logs
            DataTable result = DataServices.LoadAttendanceClock(exportParams);

            Double bgWorkerProgress = 0.0;
            List<AttendanceLog> attendanceLogs = new List<AttendanceLog>();
            foreach (DataRow row in result.Rows)
            {

                AttendanceLog obj = new AttendanceLog
                {
                    Id = (Int32)row["Id"],
                    BaseDataId = Convert.ToString(row["BaseDataId"]),
                    ClockDate = Convert.ToString(row["ClockDate"]),
                    ClockStatus = Convert.ToInt32(row["ClockStatus"]),
                    ClockTime = Convert.ToString(row["ClockTime"]),
                    LastUpdated = Convert.ToString(row["LastUpdated"]),
                    MatchingScore = Convert.ToInt32(row["MatchingScore"]),
                    TempleteId = Convert.ToInt32(row["TempleteId"]),
                    TerminalId = Convert.ToString(row["TerminalId"]),
                    TransactionCode = Convert.ToString(row["TransactionCode"]),
                    TransactionDateTime = Convert.ToString(row["TransactionDateTime"]),
                    UserPrimaryCode = Convert.ToString(row["UserPrimaryCode"])
                };

                attendanceLogs.Add(obj);
                bgWorkerProgress = bgWorkerProgress += 1;
                var PercentageProgress = bgWorkerProgress / result.Rows.Count;
                var PercentageComplete = PercentageProgress * 100;
                exportBgWorker.ReportProgress(Convert.ToInt32(PercentageComplete), null);
            }
            
            //Write Attendance Logs To file
            string fileAppendage = exportParams.AllData ? "_All_" : exportParams.StartDate + "_to_" + exportParams.EndDate;
            string fileName = exportParams.ExportDir + "\\Attendance_Log_Export_" + fileAppendage + DateTime.Now.ToString("yyyyMMddhhmmsss")+".expt";
            JsonSerialization.WriteToJsonFile<List<AttendanceLog>>(fileName, attendanceLogs);

            e.Result = attendanceLogs.Count+ " Records Exported Succesfully to "+fileName;
        }
        #endregion

        private void EnableUIControls(bool status)
        {
            buttonExport.Enabled = status;
            buttonImport.Enabled = status;
            checkBoxAllData.Enabled = status;
            dateTimeEnd.Enabled = status;
            dateTimeStart.Enabled = status;
        }

        private void ImportExportControl_Load(object sender, EventArgs e)
        {

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            StateNotifyer(ApplicationController.State.Idle);
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            StateNotifyer(ApplicationController.State.Setup);

        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            MessageNotifyer("Please wait while the attendance log is exporting...");

            if (folderBrowserExport.ShowDialog() == DialogResult.OK) {

                ExportParams exportParams = new ExportParams
                {
                    ExportDir = folderBrowserExport.SelectedPath,
                    AllData = checkBoxAllData.Checked,
                    StartDate = dateTimeStart.Value.ToString("yyyy-MM-dd"),
                    EndDate = dateTimeEnd.Value.ToString("yyyy-MM-dd")
                };

                //Disable UI Controls
                EnableUIControls(false); 
                exportBgWorker.RunWorkerAsync(exportParams);
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            if (openImportFile.ShowDialog() == DialogResult.OK)
            {
                string fileName = openImportFile.FileName;
                MessageNotifyer("Please wait while the attendance log is Importing...");

                EnableUIControls(false);
                importBgWorker.RunWorkerAsync(fileName);
            }
        }

        private void checkBoxAllData_Click(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked)
                dateTimeEnd.Enabled = dateTimeStart.Enabled = false;
            else
                dateTimeEnd.Enabled = dateTimeStart.Enabled = true;
        }
    }
}
