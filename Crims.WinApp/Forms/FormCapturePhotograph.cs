using Crims.UI.Win.Enroll.Classes;
using Crims.UI.Win.Enroll.Controls;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using Crims.UI.Win.Enroll.Helpers;
using Neurotec.Biometrics;
using Neurotec.Biometrics.Client;
using Neurotec.Plugins;

namespace Crims.UI.Win.Enroll
{
    
    public partial class FormCapturePhotograph : Form
    {
        NDeviceManager _deviceManager;
        
        private CapturePhotoNotifyer _CapturePhotoNotifyer;
        private BusyNotifyer _BusyNotifyer;
        private NBiometricClient _biometricClient;
        private EnrollFromCamera _enrollFromCamera;

        const string Address = "/local";
        const string Port = "5000";
        string _faceComponents = "Biometrics.FaceExtraction,Devices.Cameras,Biometrics.FaceSegmentsDetection,Biometrics.FaceDetection";
        //const string AdditionalComponents = "Biometrics.FaceSegmentsDetection"; Biometrics.FaceDetection,

        public FormCapturePhotograph(CapturePhotoNotifyer capturePhotoNotifyer,BusyNotifyer busyNotifyer)
        {
            _CapturePhotoNotifyer = capturePhotoNotifyer;
            _BusyNotifyer = busyNotifyer;
            InitializeComponent();
            ObtainLicenses();
        }

        public void ObtainLicenses()
        {
            //Obtain Face Components Licenses

            try
            {
                if (!NLicense.ObtainComponents(Address, Port, _faceComponents))
                {
                    throw new ApplicationException($"Could not obtain licenses for components: {components}");
                }
                //if (NLicense.ObtainComponents(Address, Port, AdditionalComponents))
                //{
                //    _faceComponents += "," + AdditionalComponents;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //finally
            //{
            //    NLicense.ReleaseComponents(_faceComponents);
            //}
            
        }
        private void FormCapturePhotograph_Shown(object sender, EventArgs e)
        {
            
        }

        private void FormCapturePhotograph_Load(object sender, EventArgs e)
        {
            try
            {
                _biometricClient = new NBiometricClient { BiometricTypes = NBiometricType.Face, UseDeviceManager = true, FacesCheckIcaoCompliance = true};
                _biometricClient.Initialize();

                _enrollFromCamera = new EnrollFromCamera
                {
                    CapturePhotoNotifyer = _CapturePhotoNotifyer,
                    Dock = DockStyle.Fill,
                    BiometricClient = _biometricClient
                };
                
                Controls.Add(_enrollFromCamera);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                AppErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                if (ex.InnerException != null)
                {
                    AppErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                }
            }
        }

        private void FormCapturePhotograph_FormClosing(object sender, FormClosingEventArgs e)
        {
            _biometricClient?.Cancel();
            NLicense.ReleaseComponents(_faceComponents);
        }
        
    }
}
