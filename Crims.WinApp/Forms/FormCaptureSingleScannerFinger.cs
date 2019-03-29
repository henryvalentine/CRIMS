using Crims.UI.Win.Enroll.Classes;
using Crims.UI.Win.Enroll.Enums;
using Crims.Win.Enroll.Controls;
using Neurotec.Devices;
using Neurotec.Licensing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using Crims.UI.Win.Enroll.Forms;
using Neurotec.Biometrics;
using Neurotec.Biometrics.Client;
using Neurotec.Images;
using Neurotec.Interop;
using Neurotec.IO;

namespace Crims.UI.Win.Enroll
{
    public partial class FormCaptureSingleScannerFinger : Form
    {
        private NDeviceManager _deviceManager;
        private NBiometricClient _biometricClient;
        private EnrollFromScanner _EnrollFromSingleFingerScanner;
        private CaptureFingerNotifyer _CaptureFingerNotifyer;
        private FingerDescription _FingerDescription;
        private FormMain _formMain;
        private List<FingerDescription> _fingerDescriptions;
        private const string Address = "/local";
        private const string Port = "5000";
        private const string FingerprintComponents = "Biometrics.FingerExtraction,Biometrics.FingerSegmentation,Biometrics.FingerMatching,Devices.FingerScanners,Images.WSQ";
        private int _fingerprintScanPosition;
        private int _fingerprintCount;
        public FormCaptureSingleScannerFinger(FormMain formMain, CaptureFingerNotifyer captureFingerNotifyer, FingerDescription fingerDescription, int capturedFingersprintCount)
        {
            //Obtain Fingerprint Components Licenses
            try
            {
                _formMain = formMain;
                NLicense.ObtainComponents(Address, Port, FingerprintComponents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FingerDescription = fingerDescription;
            _fingerprintScanPosition = capturedFingersprintCount;
            _fingerprintCount = capturedFingersprintCount;
            _CaptureFingerNotifyer = captureFingerNotifyer;
            InitializeComponent();
            _formMain.UpdateFingerInit();
            _fingerDescriptions = new List<FingerDescription>();
            EnumerateFingerDescription();

        }

        private void FormCaptureSingleScannerFinger_Load(object sender, EventArgs e)
        {
            _biometricClient = new NBiometricClient { UseDeviceManager = true, BiometricTypes = NBiometricType.Finger };
            _biometricClient.Initialize();
            _EnrollFromSingleFingerScanner = new EnrollFromScanner(this, _CaptureFingerNotifyer, _FingerDescription, _fingerprintScanPosition, _fingerprintCount)
            {
                CaptureFingerNotifyer = _CaptureFingerNotifyer,
                Dock = DockStyle.Fill,
                BiometricClient = _biometricClient
            };

            Controls.Add(_EnrollFromSingleFingerScanner);
        }

        private void EnumerateFingerDescription()
        {
            _fingerDescriptions.Add(FingerDescription.LFLittle);
            _fingerDescriptions.Add(FingerDescription.LFRing);
            _fingerDescriptions.Add(FingerDescription.LFMiddle);
            _fingerDescriptions.Add(FingerDescription.LFIndex);
            _fingerDescriptions.Add(FingerDescription.LFThumb);
            _fingerDescriptions.Add(FingerDescription.RFThumb);
            _fingerDescriptions.Add(FingerDescription.RFIndex);
            _fingerDescriptions.Add(FingerDescription.RFMiddle);
            _fingerDescriptions.Add(FingerDescription.RFRing);
            _fingerDescriptions.Add(FingerDescription.RFLittle);
        }

        //Helps to automatically trigger the scanning of next finger if the 
        //number of already captured fingers is less than 10
        public void InitNextFingerScan(FingerDescription previousDescription, int previousFingerprintScanPosition)
        {
            _deviceManager = new NDeviceManager();
            if (previousFingerprintScanPosition > 0)
            {
                _fingerprintScanPosition = previousFingerprintScanPosition;
                _FingerDescription = previousDescription;
            }
            else
            {
                _fingerprintCount = _formMain.GetActiveUserFingerRecordsCount();
                if (_fingerprintCount <= 10)
                {
                    //Get the next finger to be scanned using the previous scanned finger
                     var next = (int)previousDescription;
                     next += 1;
                    _FingerDescription = (FingerDescription)next;
                    _fingerprintScanPosition = next;
                }
            }
            
            _biometricClient = new NBiometricClient { UseDeviceManager = true, BiometricTypes = NBiometricType.Finger };
            _biometricClient.Initialize();

            _EnrollFromSingleFingerScanner = new EnrollFromScanner(this, _CaptureFingerNotifyer, _FingerDescription, _fingerprintScanPosition, _fingerprintCount)
            {
                CaptureFingerNotifyer = _CaptureFingerNotifyer,
                Dock = DockStyle.Fill,
                BiometricClient = _biometricClient
            };
            Controls.Clear();
            Controls.Add(_EnrollFromSingleFingerScanner);
        }

        private void FormCaptureSingleScannerFinger_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_EnrollFromSingleFingerScanner != null)
            {
                _EnrollFromSingleFingerScanner.DisposeNLComponents();
            }
            if (_biometricClient != null)
                _biometricClient.Cancel();
            NLicense.ReleaseComponents(FingerprintComponents);
        }
        public CaptureFingerNotifyer CaptureFingerNotifyer;

        public bool SaveBioRecord(NFinger finger, NSubject fingerSubject, FingerDescription fingerDescription, bool isTrue)
        {
            try
            {
               return _CaptureFingerNotifyer(finger, fingerSubject, fingerDescription, isTrue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

    }
}
