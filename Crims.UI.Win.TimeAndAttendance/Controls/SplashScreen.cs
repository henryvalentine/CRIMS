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
using System.Threading;
using TandAProject.Models;
using TandAProject.Utils;

namespace TandAProject.Controls
{
    public partial class SplashScreen : UserControl
    {
        AppSettings AppSettings = new AppSettings();
        private ApplicationController applicationState;
        private System.Windows.Forms.Timer timer;

        //Initialise the Notification Deligate
        public ApplicationStateChangeNotifyer StateNotifyer;
        public ApplicationMessageNotifyer MessageNotifyer;

        public BackgroundWorker bgWorker;

        public List<UserRecord> UserRecords = new List<UserRecord> { };
        public List<DbTemplateRecord> DbTemplateRecords = new List<DbTemplateRecord> { };

        public SplashScreen()
        {
            InitializeComponent();
        }

        public SplashScreen(ApplicationController ApplicationState)
        {
            this.applicationState = ApplicationState;
            InitializeComponent();
            InitialiseProperties();
        }

        public void InitialiseProperties()
        {
            bgWorker = new BackgroundWorker();
            this.bgWorker.WorkerSupportsCancellation = true;
            this.bgWorker.WorkerReportsProgress = true;
            this.bgWorker.DoWork += new DoWorkEventHandler(this.DoWork);
            this.bgWorker.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);
            this.bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.WorkerCompleted);
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Step += e.ProgressPercentage;
            progressBar1.PerformStep();
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer.Stop();


             if (e.Error != null)
            {
                MessageNotifyer("The application has enciuotered an Error during loading of fingerprints. :" + e.Error.Message);
                Logger.logToFile(e.Error, AppSettings.ErrorLogPath);

                StateNotifyer(ApplicationController.State.LoadJobFailed);
            }

            else if (e.Cancelled)
            {
                MessageNotifyer("The application has enciuotered an Error during loading of fingerprints.");
                Logger.logToFile("The background worker opperation was canceled", AppSettings.ErrorLogPath);

                StateNotifyer(ApplicationController.State.LoadJobFailed);
            }

            else {
                StateNotifyer(ApplicationController.State.LoadJobCompleted);
            }
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (AppSettings.PreloadUserData)
                {
                    LoadUserRecords();
                }

                LoadDBTemplates();

            }

            catch (Exception exp)
            {
                Logger.logToFile(exp, AppSettings.ErrorLogPath);
                e.Cancel = true;           
            }
        }

        void LoadUserRecords()
        {
            UserRecords = DataServices.GetAllUserRecords();
        }

        void LoadDBTemplates(){

            int totalDBTEmplates = DataServices.CountAllDBTemplates();

            int startingPoint = 0;
            int limit = 5000;
            int recordsFetched = 0;
            string query = string.Empty;

            while (DbTemplateRecords.Count < totalDBTEmplates) {

                DataTable result = DataServices.GetAllDBTemplates(startingPoint, limit, out query);
                recordsFetched = result.Rows.Count;

                //Log the Query For Debuging... Do not enable in Live version...
                Logger.logToFile(query, AppSettings.ErrorLogPath);

                foreach (DataRow row in result.Rows)
                {
                    DbTemplateRecord record = new DbTemplateRecord();

                    record.baseDataId = Convert.ToInt32(row[0]);
                    record.UserPrimaryCode = row[1].ToString();
                    record.templateId = Convert.ToInt32(row[2]);
                    record.template = DataServices.FetchTemplateFromResultSet(row[3]);

                    DbTemplateRecords.Add(record);
                }

                startingPoint += limit;

                //Stop Processing this loop once we hit out expected Template Count...
                if(recordsFetched == 0)
                {
                    break;
                }

                //Implement Progress Bar
            }
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new System.EventHandler(timer_Tick);
            timer.Enabled = true;
            timer.Interval = 100;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if(!bgWorker.IsBusy)
            bgWorker.RunWorkerAsync();
        }
    }

}
