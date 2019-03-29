using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Neurotec.Biometrics;
using Neurotec.Biometrics.Client;
using Neurotec.Biometrics.Gui;
using Neurotec.Devices;
using Neurotec.Images;
using Neurotec.IO;
using TandAProject.Services;
using TandAProject.Utils;

namespace TandAProject.Controls
{
    public partial class FingerCaptureControl : UserControl
    {
        #region Public constructor

        public FingerCaptureControl()
        {
            InitializeComponent();
            if (new AppSettings().AppMode.Equals("Verify", StringComparison.InvariantCulture))
                panelId.Visible = true;
            try
            {
                // create a FPScannerMan
                _deviceMan = new NDeviceManager
                {
                  DeviceTypes  = NDeviceType.FingerScanner,
                  AutoPlug  = true
                };
                UpdateScannerList();

                _nfView = new NFingerView();
                _nfView.Dock = DockStyle.Fill;
                _nfView.BackColor = panel.BackColor;
                _nfView.AutoScroll = false;
                panel.Controls.Add(_nfView);
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, AppSettings.ErrorLogPath);
                ShowMessage(ex.Message);
            }
        }

        #endregion
        private NSubject _subject;
        private NFinger _subjectFinger;
        #region Private fields

        AppSettings _AppSettings = new AppSettings();
        private NDeviceManager _deviceMan;
        private NFingerScanner _currentScanner;
        private NFingerView _nfView;
        private NBiometricClient _biometricClient;
        private string _oldTemplateFilename = string.Empty;
        private NBuffer _template;

        List<NDevice> devices = new List<NDevice>();
        bool scannerStatus = true;
        #endregion

        public NBiometricClient BiometricClient
        {
            get { return _biometricClient; }
            set { _biometricClient = value; }
        }

        #region Public properties
        public NBuffer Template
        {
            get
            {
                return _template;
            }
            set
            {
                _template = value;
            }
        }
        
        #endregion

        #region Private methods

        //Initialise the Notification Deligate
        public ApplicationStateChangeNotifyer StateNotifyer;
        public ApplicationMessageNotifyer MessageNotifyer;
        public ActiveTemplateNotifyer TemplateNotifyer;
        public ActiveIDNumberNotifyer IDNumberNotifyer;

        private void OnIsScanningChanged(bool isScanning)
        {
            scannerStatus = !isScanning;
        }

        #endregion

        #region Private form events
        private void EnrollFromScannerLoad(object sender, EventArgs e)
        {
           
        }

        private void UpdateScannerList()
        {
            devices.Clear();
            foreach (NDevice device in _deviceMan.Devices)
            {
                devices.Add(device);
            }
        }

        private void EnrollFromScannerVisibleChanged(object sender, EventArgs e)
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
            timerScannerStatus.Stop();
            NFingerScanner scanner = (NFingerScanner)e.Argument;

            _currentScanner = scanner;

            StartCapturing(scanner);
        }

        private void ScannerPreview(object sender, NFScannerPreviewEventArgs e)
        {
            try
            {
                if (_nfView.Finger.Image != null)
                {
                    var oldImage = _nfView.Finger.Image;
                    _nfView.Finger.Image = null;
                    oldImage.Dispose();
                }
                if (_nfView.Finger.Objects[0].Template != null)
                {
                    var template = _nfView.Finger.Objects[0].Template;
                    _nfView.Finger.Objects[0].Template = null;
                    template.Dispose();
                }

                //lblQuality.Text = e.Status.ToString();
                if (e.Image != null)
                {
                   _nfView.Finger.Image = e.Image;
                }
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, AppSettings.ErrorLogPath);
            }
        }

        private void ScanWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _currentScanner = null;
            
        }

        private void StartCapturing(NFingerScanner scanner)
        {
            //NFingerScanner scanner = scannersListBox.SelectedItem as NFingerScanner;
            _currentScanner = scanner;
            if (_currentScanner == null)
            {
                MessageBox.Show(@"Please select a scanner from the list.");
                return;
            }

            if (scanWorker.IsBusy)
            {
                MessageBox.Show(@"Scan already in progress.");
                return;
            }

            OnIsScanningChanged(true);
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

        private void OnEnrollCompleted(IAsyncResult r)
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

                    OnIsScanningChanged(false);

                    _template = _subject.Save();

                    _nfView.Finger.Objects[0].Template = _subject.Fingers[0].Objects[0].Template;
                    SaveTemplateForVerification(_template);
                    SaveUserIdForVerification(textBoxIDNumber.Text);
                    UpdateStateToParent(ApplicationController.State.Captured_Good);

                    NImage image = _subjectFinger.Image;
                    if (image == null) return;

                    _nfView.Width = (int)image.Width;
                    _nfView.Height = (int)image.Height;
                    _nfView.Finger.Image = image;
                    _nfView.Refresh();
                    
                }
                else
                {
                    MessageBox.Show($"Fingerprint image is of low quality OR The template was not extracted: {status}.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _subject = null;
                    _subjectFinger = null;
                }
            }
        }

        private void OnAttributesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
            {
                BeginInvoke(new Action<NBiometricStatus>(status => lblQuality.Text = status.ToString()), _subjectFinger.Status);
            }
        }

        #endregion

        private void timerScannerStatus_Tick(object sender, EventArgs e)
        {
            ShowMessage(@"Place your finger on the scanner firmly");
            lblQuality.Text = string.Empty;
            if (scannerStatus)
            {
                try
                {
                    int selectedScannerIndex = Convert.ToInt32(_AppSettings.ScannerDevice);
                    NFingerScanner scanner = devices[selectedScannerIndex] as NFingerScanner;

                    if (scanner == null)
                    {
                        ShowMessage(@"No Scanner found.");
                        return;
                    }

                    if (scanWorker.IsBusy)
                    {
                        ShowMessage(@"Scan already in progress.");
                        return;
                    }

                    OnIsScanningChanged(true);
                    scanWorker.RunWorkerAsync(scanner);
                }
                catch (Exception ex)
                {
                    Logger.logToFile(ex, AppSettings.ErrorLogPath);
                    ShowMessage(ex.Message);
                }
            }

        }

        private void UpdateStateToParent(ApplicationController.State state)
        {
            StateNotifyer(state);
        }

        private void ShowMessage(string message)
        {
            MessageNotifyer(message);
        }

        private void SaveTemplateForVerification(NBuffer _template)
        {
            TemplateNotifyer(_template);
        }

        private void SaveUserIdForVerification(string text)
        {
            IDNumberNotifyer(text);
        }
    }
}
