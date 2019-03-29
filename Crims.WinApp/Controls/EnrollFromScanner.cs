using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Crims.UI.Win.Enroll;
using Neurotec.Biometrics.Gui;
using Neurotec.Devices;
using Neurotec.IO;
using Crims.UI.Win.Enroll.Classes;
using Crims.UI.Win.Enroll.Controls;
using Crims.UI.Win.Enroll.Enums;
using Crims.UI.Win.Enroll.Forms;
using Crims.UI.Win.Enroll.Properties;
using Neurotec.Biometrics;
using Neurotec.Biometrics.Client;
using Neurotec.Images;

namespace Crims.Win.Enroll.Controls
{
	public partial class EnrollFromScanner : BaseControl
	{
		#region Private fields
        
		private NDeviceManager _deviceMan;
		private NFingerScanner _currentScanner;
		private NFingerView _nfView;
        private string _oldTemplateFilename = string.Empty;
        private NDeviceManager _deviceManager;
        private NBiometricClient _biometricClient;
        private NSubject _subject;
        private NFinger _subjectFinger;
		private NBuffer _template;
        private decimal recordQuality = 0;
        private int _fingerprintScanPosition = 0;
	    private int _fingerprintCount;
        private FormCaptureSingleScannerFinger _formSingleScanFinger;
        
        #endregion
        
        public CaptureFingerNotifyer CaptureFingerNotifyer;
        private FingerDescription _fingerDescription;
        #region Public constructor

        public NBiometricClient BiometricClient
        {
            get { return _biometricClient; }
            set { _biometricClient = value; }
        }

        public EnrollFromScanner(FormCaptureSingleScannerFinger formCaptureSingleScannerFinger, CaptureFingerNotifyer _captureFingerNotifyer, FingerDescription fingerDescription, int fingerprintScanPosition, int fingerprintCount)
		{
            _fingerDescription = fingerDescription;
            _formSingleScanFinger = formCaptureSingleScannerFinger;
		    _fingerprintCount =  fingerprintCount;
            _fingerprintScanPosition = fingerprintScanPosition;
            InitializeComponent();
            lblFingerprintScanCount.Text = _fingerprintCount.ToString();
           
        }

        #endregion

        #region Private methods
       
        private void OnIsScanningChanged(bool isScanning)
		{
			scanButton.Enabled = !isScanning;
			//cancelScanningButton.Enabled = isScanning;
			refreshListButton.Enabled = !isScanning;
			saveTemplateButton.Enabled = !isScanning;
			saveImageButton.Enabled = !isScanning;
		}

		private bool CancelScanning()
		{
            DisposeNLComponents();
            CaptureFingerNotifyer(null, null, FingerDescription.Unknown);
            ParentForm.Dispose();

            return true;
		}

		#endregion

		#region Private form events

		private void EnrollFromScannerLoad(object sender, EventArgs e)
		{
            Invoke(new Action(() =>
            {
                try
                {
                    _deviceManager = _biometricClient.DeviceManager;
                    UpdateScannerList();
                    saveFileDialog.Filter = NImages.GetSaveFileFilterString();

                    _nfView = new NFingerView
                    {
                        Dock = DockStyle.Fill,
                        AutoScroll = true
                    };
                    panel.Controls.Add(_nfView);
                    StartCapturing();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
               
            }));
           
        }
        
        private void UpdateScannerList()
        {
            scannersListBox.BeginUpdate();
            try
            {
                scannersListBox.Items.Clear();
                if (_deviceManager != null)
                {
                    foreach (var item in _deviceManager.Devices)
                    {
                        scannersListBox.Items.Add(item);
                    }
                }
            }
            finally
            {
                scannersListBox.EndUpdate();
                if (scannersListBox.Items.Count > 0)
                {
                    scannersListBox.SelectedIndex = 0;
                    _biometricClient.FingerScanner = scannersListBox.SelectedItem as NFScanner;
                }
                else
                {
                    _biometricClient.FingerScanner = null;
                }
            }
        }
        
        private void ScanButtonClick(object sender, EventArgs e)
		{
            StartCapturing();
		}
   
        private void StartCapturing()
        {
            //NFingerScanner scanner = scannersListBox.SelectedItem as NFingerScanner;
            _currentScanner = scannersListBox.SelectedItem as NFingerScanner;
            if (_currentScanner == null)
            {
                MessageBox.Show(@"Please select a scanner from the list.");
                return;
            }

            OnIsScanningChanged(true);
            //scanWorker.RunWorkerAsync(_currentScanner);

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
            var task = _biometricClient.CreateTask(NBiometricOperations.Detect | NBiometricOperations.Capture | NBiometricOperations.CreateTemplate | NBiometricOperations.AssessQuality, _subject);
            _biometricClient.BeginPerformTask(task, OnEnrollCompleted, null);
        }

        private void OnEnrollCompleted(IAsyncResult r)
        {
            Invoke(new Action(() =>
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new AsyncCallback(OnEnrollCompleted), r);
                }
                else
                {
                    var task = _biometricClient.EndPerformTask(r);
                    EnableButtons(false);
                    var status = task.Status;

                    // Check if extraction was canceled
                    if (status == NBiometricStatus.Canceled) return;

                    if (status == NBiometricStatus.Ok)
                    {
                        lblQuality.Text = $"Quality: {_subjectFinger.Objects[0].Quality}";

                        decimal.TryParse(_subjectFinger.Objects[0].Quality.ToString(), out recordQuality);

                        //Send the FingerImage and Extracted Template with FingerIdentifyer to MainForm
                        //CaptureFingerNotifyer(image.ToBitmap(), _template.ToArray(),_FingerDescription, true);
                        //This will only be done when the OK button is clicked

                        //AUTOMATICALLY CLICKING THE OK BUTTON
                        var fingerQuality = Settings.Default.FQualityThreshold;
                        if (recordQuality >= fingerQuality)
                        {
                            CancelWork();
                            DisposeNLComponents();

                            try
                            {
                                //var status = CaptureFingerNotifyer(_imageShow.ToBitmap(), fMs.ToArray(), _template.ToArray(), _fingerDescription, true);
                                var saveStatus = _formSingleScanFinger.SaveBioRecord(_subjectFinger, _subject, _fingerDescription, true);

                                if (saveStatus)
                                {
                                    if (_fingerprintScanPosition == 10)
                                    {
                                        ParentForm.Dispose();
                                    }
                                    else
                                    {
                                        _formSingleScanFinger.InitNextFingerScan(_fingerDescription, 0);
                                        Dispose();
                                    }
                                }
                                else
                                {

                                    _formSingleScanFinger.InitNextFingerScan(_fingerDescription, _fingerprintScanPosition);
                                    Dispose();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"The template was not extracted: {status}.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _subject = null;
                        _subjectFinger = null;
                        EnableButtons(false);
                    }
                }

                if (_subjectFinger == null)
                {
                    EnableButtons(false);
                }
                else
                {
                    EnableButtons(true);
                }
            }));
        }

        private void OnAttributesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
            {
                BeginInvoke(new Action<NBiometricStatus>(status => lblQuality.Text = status.ToString()), _subjectFinger.Status);
            }
        }

        private void CancelScanningButtonClick(object sender, EventArgs e)
		{
			//cancelScanningButton.Enabled = !CancelScanning();
		}

		private void RefreshListButtonClick(object sender, EventArgs e)
		{
			UpdateScannerList();
		}

		private void SaveImageButtonClick(object sender, EventArgs e)
		{
			if (_nfView.Finger.Image == null) return;
			saveFileDialog.Filter = NImages.GetSaveFileFilterString();
			saveFileDialog.Title = @"Save Image File";

			if (saveFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			var fileName = saveFileDialog.FileName;
			try
			{
                _subjectFinger.Image.Save(fileName);
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format("Error saving to file \"{0}\": {1}", fileName, ex),
								Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void SaveTemplateButtonClick(object sender, EventArgs e)
		{
			if (_template == null) return;
			saveFileDialog.Filter = @"Template files (*.dat)|*.dat";
			saveFileDialog.Title = @"Save Template File";
			if (_oldTemplateFilename != string.Empty)
			{
				saveFileDialog.FileName = _oldTemplateFilename;
			}

			if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
			_oldTemplateFilename = saveFileDialog.FileName;

			// save template to file
			File.WriteAllBytes(saveFileDialog.FileName, _subject.GetTemplateBuffer().ToArray());
		}
        
        private void EnableButtons(bool v)
        {
            saveTemplateButton.Enabled = v;
            saveImageButton.Enabled = v;
            //buttonOK.Enabled = v; //Note : This is Disabled so that the NFingerView can be manually refreshed by clicking the 'Scan' button when the extraction process hangs
            scanButton.Enabled = v;
        }

        #endregion

        public void DisposeNLComponents()
        {
            if (_deviceMan != null && !_deviceMan.IsDisposed) _deviceMan.Dispose();
            if (_currentScanner != null && !_currentScanner.IsDisposed) _currentScanner.Dispose();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            var fingerQuality = Settings.Default.FQualityThreshold;
            decimal.TryParse(_subjectFinger.Objects[0].Quality.ToString(), out recordQuality);
            if (recordQuality >= fingerQuality)
            {
                CancelWork();
                DisposeNLComponents();
                try
                {
                    var saveStatus = _formSingleScanFinger.SaveBioRecord(_subjectFinger, _subject,
                        _fingerDescription, true);

                    if (saveStatus)
                    {
                        if (_fingerprintScanPosition == 10)
                        {
                            ParentForm.Dispose();
                        }
                        else
                        {
                            _formSingleScanFinger.InitNextFingerScan(_fingerDescription, 0);
                            Dispose();
                        }
                    }
                    else
                    {

                        _formSingleScanFinger.InitNextFingerScan(_fingerDescription, _fingerprintScanPosition);
                        Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show(this,
                    $"Fingerprint image does not meet expected quality of {fingerQuality}. Click on Scan button to re-capture. ",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void scannersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _biometricClient.FingerScanner = scannersListBox.SelectedItem as NFScanner;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            CancelScanning();
            DisposeNLComponents();
            CaptureFingerNotifyer(null, null, FingerDescription.Unknown );
            Dispose();
        }
    }
}
