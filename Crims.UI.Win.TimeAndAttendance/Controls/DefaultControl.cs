using Neurotec.Biometrics;
using Neurotec.Biometrics.Gui;
using Neurotec.IO;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Neurotec.Devices;
using Neurotec.Images;
using TandAProject.Utils;
using TandAProject.Services;
using System.Threading;
using Neurotec.Biometrics.Client;

namespace TandAProject.Controls
{
    public partial class DefaultControl : UserControl, IDisposable
    {
        private NBuffer _template;

        private NFingerScanner _currentScanner;
        private NSubject _subject;
        private NFinger _subjectFinger;
        private string _oldTemplateFilename = string.Empty;
        private NDeviceManager _deviceMan;

        private NFingerView _nfView;

        ApplicationController applicationState;

        //Initialise the Notification Deligate
        public ApplicationStateChangeNotifyer StateNotifyer;
        public ApplicationMessageNotifyer MessageNotifyer;
        private NBiometricClient _biometricClient;
        public NBiometricClient BiometricClient
        {
            get { return _biometricClient; }
            set { _biometricClient = value; }
        }

        public BackgroundWorker scanWorker;

        public DefaultControl()
        {
            InitializeComponent();
        }

        public DefaultControl(ApplicationController ApplicationState)
        {
            InitializeComponent();

            this.applicationState = ApplicationState;

            scanWorker = new BackgroundWorker();
            this.scanWorker.WorkerSupportsCancellation = true;
            this.scanWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ScanWorkerDoWork);
            this.scanWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ScanWorkerRunWorkerCompleted);
        }

        private void BeginCapture()
        {

        }

        private void ScannerVisibleChanged(object sender, EventArgs e)
        {
            if (Visible) return;
            if (!scanWorker.IsBusy) return;

            scanWorker.CancelAsync();
            if (_currentScanner != null)
            {
                MethodInvoker invoker = new MethodInvoker(_currentScanner.Cancel);
                invoker.BeginInvoke(null, null);
            }
        }

        private void ScanWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            NFingerScanner scanner = (NFingerScanner)e.Argument;

            _currentScanner = scanner;

            scanner.Preview += new EventHandler<NFScannerPreviewEventArgs>(ScannerPreview);
            try
            {
                e.Result = scanner.Capture();
            }
            finally
            {
                scanner.Preview -= new EventHandler<NFScannerPreviewEventArgs>(ScannerPreview);                
            }

            if (((BackgroundWorker)sender).CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private void ScannerPreview(object sender, NFScannerPreviewEventArgs e)
        {
            if (_nfView.Finger.Image != null)
            {
                var oldImage = _nfView.Finger.Image;
                _nfView.Finger.Image = null;
                oldImage.Dispose();
            }
            if (_nfView.Finger.Objects[0].Template != null)
            {
                NFRecord template = _nfView.Finger.Objects[0].Template;
                _nfView.Finger.Objects[0].Template = null;
                template.Dispose();
            }

            //lblQuality.Text = e.Status.ToString();
            if (e.Image != null)
            {
                _nfView.Finger.Image = e.Image;
            }
        }

        private void ScanWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _currentScanner = null;

            if (_nfView.Finger.Image != null)
            {
                _nfView.Finger.Image.Dispose();
                _nfView.Finger.Image = null;
            }
            if (_nfView.Finger.Objects[0].Template != null)
            {
                _nfView.Finger.Objects[0].Template.Dispose();
                _nfView.Finger.Objects[0].Template = null;
            }
            lblQuality.Text = string.Empty;


            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                Logger.logToFile(e.Error, AppSettings.ErrorLogPath);
                return;
            }

            if (e.Cancelled)
            {
                lblQuality.Text = @"Scanning canceled.";
                return;
            }

            NImage image = e.Result as NImage;
            if (image == null) return;

            _nfView.Width = (int)panelFingerPrint.Width;
            _nfView.Height = (int)panelFingerPrint.Height;
            
           
            _nfView.Finger.Image = image;
            _nfView.Refresh();
        }

        private void OnEnrollCompleted(IAsyncResult r)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new AsyncCallback(OnEnrollCompleted), r);
                }
                else
                {
                    var task = _biometricClient.EndPerformTask(r);
                    var status = task.Status;

                    // Check if extraction was canceled
                    if (status == NBiometricStatus.Canceled) return;

                    if (status == NBiometricStatus.Ok)
                    {
                        lblQuality.Text = $"Quality: {_subjectFinger.Objects[0].Quality}";

                        _template = _subject.Save();

                        ShowMessage(@"Please wait while your fingerprint is being processed...");
                        UpdateStateToParent(ApplicationController.State.Captured_Good);
                    }
                    else
                    {
                        lblQuality.Text = string.Empty;

                        ShowMessage(@"Fingerprint image is of low quality");
                        UpdateStateToParent(ApplicationController.State.Captured_Bad);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(string.Format("Extraction error: {0}", ex.Message));
                Logger.logToFile(ex, AppSettings.ErrorLogPath);

            }
        }

        private void UpdateStateToParent(ApplicationController.State state)
        {
            StateNotifyer(state);
        }

        private void ShowMessage(string message) {
            MessageNotifyer(message);
        }

        private void DefaultControl_Load(object sender, EventArgs e)
        {
            InitScannerBeginCapture();
        }

        private void InitScannerBeginCapture()
        {
            try
            {
                _deviceMan = new NDeviceManager
                {
                   DeviceTypes = NDeviceType.FingerScanner,
                   AutoPlug = true,
                   
                };
                _nfView = new NFingerView();
                _nfView.Dock = DockStyle.Fill;
                _nfView.AutoScroll = false;
                _nfView.AutoSize = true;
                _nfView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                panelFingerPrint.Controls.Add(_nfView);

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                Logger.logToFile(ex, AppSettings.ErrorLogPath);
            }

            StartCapturing();
        }

        private void OnAttributesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
            {
                BeginInvoke(new Action<NBiometricStatus>(status => lblQuality.Text = status.ToString()), _subjectFinger.Status);
            }
        }
        
        private void StartCapturing()
        {
            //NFingerScanner scanner = scannersListBox.SelectedItem as NFingerScanner;
            var scanner = (NFingerScanner)_deviceMan.Devices[1];
            if (scanner == null)
            {
                MessageBox.Show(@"Please select a scanner from the list.");
                return;
            }

            if (scanWorker.IsBusy)
            {
                MessageBox.Show(@"Scan already in progress.");
                return;
            }
            scanWorker.RunWorkerAsync(_currentScanner);

            // Create a finger
            _subjectFinger = new NFinger();

            // Add finger to the subject and fingerView
            _subject = new NSubject();
            _subject.Fingers.Add(_subjectFinger);
            _subjectFinger.PropertyChanged += OnAttributesPropertyChanged;
            _nfView.Finger = _subjectFinger;
            _nfView.ShownImage = ShownImage.Original;

            // Begin capturing
            _biometricClient.FingersReturnBinarizedImage = true;
            var task = _biometricClient.CreateTask(NBiometricOperations.Capture | NBiometricOperations.CreateTemplate, _subject);
            _biometricClient.BeginPerformTask(task, OnEnrollCompleted, null);
        }
    }
}
