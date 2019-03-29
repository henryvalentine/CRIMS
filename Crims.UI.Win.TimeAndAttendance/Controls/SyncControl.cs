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
using MySql.Data.MySqlClient;
using TandAProject.Database;
using Crims.UI.Win.TimeAndAttendance.Properties;

namespace Crims.UI.Win.TimeAndAttendance.Controls
{
    public partial class SyncControl : UserControl
    {
        //Initialise the Notification Deligate
        public ApplicationStateChangeNotifyer StateNotifyer;
        public ApplicationMessageNotifyer MessageNotifyer;

        public IList<AttendanceLog> attentanceLogs;

        bool AutoStartSync;

        BackgroundWorker BgWorker = new BackgroundWorker();

        StringBuilder log = new StringBuilder();

        public SyncControl(bool _AutoStart = false)
        {
            AutoStartSync = _AutoStart;

            InitializeComponent();
            ConfigureBGWorker();

            if (AutoStartSync)
            {
                StartSync();
            }
        }

        private void ConfigureBGWorker()
        {
            BgWorker.WorkerSupportsCancellation = true;
            BgWorker.WorkerReportsProgress = true;
            BgWorker.DoWork += new DoWorkEventHandler(exportBgWorker_DoWork);
            BgWorker.ProgressChanged += new ProgressChangedEventHandler(exportBgWorker_ProgressChanged);
            BgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(exportBgWorker_WorkerCompleted);
        }

        private void exportBgWorker_WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                log.AppendLine(e.Error.Message);
            }

            if (e.Cancelled)
            {
                log.AppendLine("Error: Opperation was canceled");
            }

            buttonStartSync.Enabled = true;
            buttonStopSync.Enabled = false;
            buttonSaveLogs.Enabled = true;
            richTextBoxLog.Text = log.ToString();

            if (MessageBox.Show(this,"Sychcronization with server is completed. Click Ok to Exit. Click Cancel to Review Logs", "Sync Completed",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                StateNotifyer(ApplicationController.State.Idle);
            }
        }

        private void exportBgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {           
            throw new NotImplementedException();
        }

        private void exportBgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            MySqlConnection serverCon = null;
            int affectedRows = 0;

            log.AppendLine("Checking Server Connection...");

            try
            {
                serverCon = new ConnectionManager(
            Settings.Default.PushServer,
            Settings.Default.PushServerDB,
            Settings.Default.PushServerDBPort,
            Settings.Default.PushServerDBUser,
            Settings.Default.PushServerDBPassword).
            getDBConnection();

                log.AppendLine("Server Connection Confirmed...");
            }

            catch (Exception exp)
            {
                log.AppendLine("Unable to continue sync..." + exp.Message);
            }

            if (serverCon.State == ConnectionState.Open)
            {
                int batchSize = 1000;

                try
                {
                    //Determine total number of records pending..
                    log.AppendLine("Checking size of pending data for sync...");
                    int pendingDataSize = DataServices.GetAttendanceClockPendingSyncSize();


                    log.AppendLine("Total logs pending to by synchronised is:" + pendingDataSize);

                    if (pendingDataSize > 0)
                    {
                        //Continue to Process the Sync in Batch until the pending records are all sent to the server..
                        while (affectedRows < pendingDataSize)
                        {
                            attentanceLogs = DataServices.GetAttendanceClockPendingSync(batchSize, 0);
                            log.AppendLine("Loaded " + attentanceLogs.Count() + " Record... Synching.. ");

                            //Create an exit plan for this while loop...
                            if(attentanceLogs.Count < 1)
                            {
                                break;
                            }

                            List<string> processedIds = new List<string>();
                            foreach (AttendanceLog aLog in attentanceLogs)
                            {
                                if (BgWorker.CancellationPending)
                                {
                                    e.Cancel = true;
                                }

                                int result = 0;
                                try
                                {
                                    result = DataServices.LogAttendanceClock(aLog, serverCon);
                                }
                                catch (Exception exp)
                                {
                                    log.AppendLine(exp.Message);
                                }

                                if (result > 0)
                                {
                                    affectedRows += 1;
                                    processedIds.Add(aLog.Id.ToString());
                                }
                                else
                                {
                                    //For every record that is not added to the SErver, 
                                    //we need to reduce the size PendingDataSize to revent our loop from going infinite..
                                    pendingDataSize -= 1;
                                }
                            }

                            //Update SyncStatus of Batch records for succesfully processed Ids on local DB
                            DataServices.UpdateAttendanceClockSyncStatus(processedIds.ToArray(), "1");
                        }
                    }

                    log.AppendLine("Completed. " + affectedRows+ " Records was Synchronised to the server");
                }
                catch (Exception exp)
                {
                    log.AppendLine(exp.Message);
                }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (BgWorker.IsBusy)
            {
                BgWorker.CancelAsync();
            }

            StateNotifyer(ApplicationController.State.Idle);
        }

        private void buttonStopSync_Click(object sender, EventArgs e)
        {
            if (BgWorker.IsBusy)
            {
                BgWorker.CancelAsync();

                buttonStartSync.Enabled = true;
                buttonStopSync.Enabled = false;
                buttonSaveLogs.Enabled = true;
            }
        }

        private void buttonStartSync_Click(object sender, EventArgs e)
        {
           StartSync();
        }

        private void StartSync()
        {
            buttonStartSync.Enabled = false;
            buttonStopSync.Enabled = true;
            buttonSaveLogs.Enabled = false;

            log = new StringBuilder();
            attentanceLogs = new List<AttendanceLog>();
            BgWorker.RunWorkerAsync();
        }
    }
}
