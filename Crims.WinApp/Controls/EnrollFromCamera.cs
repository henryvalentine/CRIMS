using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Web.Mvc;
using System.Windows.Forms;
using Crims.Core.Logging;
using Neurotec.Devices;
using Neurotec.IO;
using Crims.UI.Win.Enroll.Classes;
using Crims.UI.Win.Enroll.Forms;
using Crims.UI.Win.Enroll.Helpers;
using Crims.UI.Win.Enroll.Properties;
using FaceLiftSDK;
using ImageResizer;
using Neurotec.Biometrics;
using Neurotec.Biometrics.Client;
using Neurotec.Images;
using Neurotec.Licensing;
using Neurotec.Samples;

namespace Crims.UI.Win.Enroll.Controls
{
    public partial class EnrollFromCamera : BaseControl
    {
        public CapturePhotoNotifyer CapturePhotoNotifyer;

        #region Private fields

        private readonly List<NImage> _capturedImages = new List<NImage>();
        private NImage _bestFrame;
        private NDeviceManager _deviceManager;
        private NCamera _camera;
        private NBuffer _template;
        private NBiometricClient _biometricClient;
        private NSubject _subject;
        #endregion

        #region Public constructor

        public EnrollFromCamera()
        {
            //_template = template;
            InitializeComponent();
            saveImageDialog.Filter = NImages.GetSaveFileFilterString();
        }

        #endregion

        #region Private methods
        public NBiometricClient BiometricClient
        {
            get { return _biometricClient; }
            set { _biometricClient = value; }
        }
        private void EnableControls(bool capturing)
        {
            btnStart.Enabled = !capturing;
            btnRefreshList.Enabled = !capturing;
            btnStop.Enabled = capturing;
            cbCamera.Enabled = !capturing;

            //btnSaveImage.Enabled = !capturing && _bestFrame != null;
            //btnSaveTemplate.Enabled = !capturing && _template != null;
            buttonOk.Enabled = !capturing && _template != null;

            btnStartExtraction.Enabled = capturing;

            splitContainer1.Enabled = !capturing;
            tabControl1.Enabled = !capturing;
            tabControl1.SelectedIndex = !capturing ? 1 : 0;

            var hasTemplate = !capturing && _subject != null && _subject.Status == NBiometricStatus.Ok;
            btnSaveImage.Enabled = hasTemplate;
            btnSaveTemplate.Enabled = hasTemplate;
        }

        private void ClearCapturedImages()
        {
            foreach (NImage t in _capturedImages)
            {
                t.Dispose();
            }
            _capturedImages.Clear();
        }


        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            bool extractStarted = false;
            try
            { 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        
        //private delegate void UpdateViewDelegate(NFace face, NBiometricStatus status);
        //private void UpdateView(NFace face, NBiometricStatus status)
        //{
        //    if (InvokeRequired)
        //    {
        //        UpdateViewDelegate del = UpdateView;
        //        BeginInvoke(del, face, status);
        //        return;
        //    }

        //    var oldImage = facesView.Face.Image;
        //    if (oldImage != null) oldImage.Dispose();
        //    facesView.Face.Image = face.Image;
        //    if (status != NBiometricStatus.None)
        //    {
        //        lblStatus.Text = status.ToString();
        //    }
        //    else if (_capture)
        //    {
        //        lblStatus.Text = @"Extracting ...";
        //    }
        //}

        #endregion

        #region Private form events
        private void UpdateCameraList()
        {
            cbCamera.BeginUpdate();
            try
            {
                cbCamera.Items.Clear();
                foreach (NDevice device in _deviceManager.Devices)
                {
                    cbCamera.Items.Add(device);
                }

                if (_biometricClient.FaceCaptureDevice == null && cbCamera.Items.Count > 0)
                {
                    cbCamera.SelectedIndex = 0;
                    _biometricClient.FaceCaptureDevice = cbCamera.SelectedItem as NCamera;
                    return;
                }

                if (_biometricClient.FaceCaptureDevice != null)
                {
                    cbCamera.SelectedIndex = cbCamera.Items.IndexOf(_biometricClient.FaceCaptureDevice);
                }
            }
            finally
            {
                cbCamera.EndUpdate();
            }
        }
        private void EnrollFromCameraLoad(object sender, EventArgs e)
        {
            try
            {
                if (!DesignMode)
                {
                    try
                    {
                        lblStatus.Text = string.Empty;
                        _deviceManager = _biometricClient.DeviceManager;
                        saveImageDialog.Filter = NImages.GetSaveFileFilterString();
                        UpdateCameraList();
                        StartCapturing();
                    }
                    catch (Exception ex)
                    {
                        AppUtils.ShowException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                AppErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                if (ex.InnerException != null)
                {
                    AppErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                }
            }
        }

        private void BtnRefreshListClick(object sender, EventArgs e)
        {
            UpdateCameraList();
        }

        private void CbCameraSelectedIndexChanged(object sender, EventArgs e)
        {
            _biometricClient.FaceCaptureDevice = cbCamera.SelectedItem as NCamera;
        }

        private void BtnStartClick(object sender, EventArgs e)
        {
            StartCapturing();
        }

        private void StartCapturing()
        {
            if (_biometricClient.FaceCaptureDevice == null)
            {
                MessageBox.Show(@"Please select camera from the list");
                return;
            }
            // Set face capture from stream
            var face = new NFace { CaptureOptions = NBiometricCaptureOptions.Manual};
            _subject = new NSubject();
            _subject.Faces.Add(face);
            facesView.Face = face;
            _biometricClient.FacesDetectAllFeaturePoints = true;
            // Begin capturing faces
            _biometricClient.BeginCapture(_subject, OnCapturingCompleted, null);
            lblStatus.Text = string.Empty;
            EnableControls(true);

            ////_task = _biometricClient.CreateTask(NBiometricOperations.Capture | NBiometricOperations.Enroll | NBiometricOperations.Segment | NBiometricOperations.AssessQuality, _subject);
            ////_biometricClient.PerformTask(_task);
            ////if (_task.Status == NBiometricStatus.Ok)
            ////{
            ////    pictureBoxCroped.Image = _subject.Faces[0].Image.ToBitmap();
            ////    _croppedPhotoImage = _subject.Faces[0].Image.ToBitmap();
            ////}
        }
        
        private Image _croppedPhotoImage;
        
        #endregion

        #region Private methods
        private void OnCapturingCompleted(IAsyncResult r)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AsyncCallback(OnCapturingCompleted), r);
            }
            else
            {
                try
                {
                    var status = _biometricClient.EndCapture(r);
                    // If Stop button was pushed
                    if (status == NBiometricStatus.Canceled) return;

                    lblStatus.Text = status.ToString();
                    if (status != NBiometricStatus.Ok)
                    {
                        // Since capture failed start capturing again
                        _subject.Faces[0].Image = null;
                        _biometricClient.BeginCapture(_subject, OnCapturingCompleted, null);
                    }
                    else
                    {
                        if (_subject.Faces[0].Image != null)
                        {
                            //facesView.Image = image;
                            var size = new Size { Width = int.Parse(Settings.Default.ImageWidth), Height = int.Parse(Settings.Default.ImageHeight) };
                            var memoryStream = new MemoryStream();
                            _subject.Faces[0].Image.ToBitmap().Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            var faceBoundingRect = _subject.Faces[0].Objects[0].BoundingRect;
                            //var resizedImage = _subject.Faces[0].Image.Crop(50, 50, 50, 50).ToBitmap();

                            //int widthOffset;
                            //if (faceBoundingRect.Left > faceBoundingRect.Right)
                            //{
                            //    //widthOffset = 2*tt.Right/6 + tt.Width;
                            //    widthOffset = (faceBoundingRect.Left - faceBoundingRect.Right) / 3 + faceBoundingRect.Width + 50;
                            //}
                            //else
                            //{
                            //    //widthOffset = 2*tt.Left/6 + tt.Width;
                            //    //widthOffset = tt.Left + tt.Width + 50;

                            //    widthOffset = (faceBoundingRect.Right - faceBoundingRect.Left) / 3 + faceBoundingRect.Width;
                            //}

                            //if (faceBoundingRect.Left > faceBoundingRect.Right)
                            //{
                            //    //widthOffset = tt.Left - tt.Right;
                            //    widthOffset = (faceBoundingRect.Right - faceBoundingRect.Right) / 2 + faceBoundingRect.Width / 2 + 50;
                            //}
                            //else
                            //{
                            //    widthOffset = faceBoundingRect.Right - faceBoundingRect.Left;
                            //    widthOffset = (faceBoundingRect.Width * 10) / 100 + faceBoundingRect.Width;
                            //}

                            var resizedImage = ResizeImage(memoryStream, (faceBoundingRect.Location.X - faceBoundingRect.Left) + faceBoundingRect.Width, faceBoundingRect.Height + 50, true, faceBoundingRect.X, faceBoundingRect.Top, faceBoundingRect.Right, faceBoundingRect.Bottom);
                            if (resizedImage != null)
                            {
                                pictureBoxCroped.Image = resizedImage;
                                _croppedPhotoImage = resizedImage;
                            }
                            _template = _subject.GetTemplateBuffer();
                            EnableControls(false);

                        }

                        //UpdateView(_subject.Faces[0], status);

                        
                    }
                }
                catch (Exception ex)
                {
                    AppUtils.ShowException(ex);
                    lblStatus.Text = string.Empty;
                    EnableControls(false);
                }
            }
        }

        private void BtnStopClick(object sender, EventArgs e)
        {
            _biometricClient.Cancel();
            EnableControls(false);
        }

        private void BtnSaveTemplateClick(object sender, EventArgs e)
        {
            if (saveTemplateDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(saveTemplateDialog.FileName, _subject.GetTemplateBuffer().ToArray());
            }
        }

        private void BtnStartExtractionClick(object sender, EventArgs e)
        {
            //lblStatus.Text = string.Empty;
            //_capture = true;

            lblStatus.Text = @"Extracting ...";
            // Begin extraction
            _biometricClient.ForceStart();
        }

        private void BtnSaveImageClick(object sender, EventArgs e)
        {
            if (saveImageDialog.ShowDialog() == DialogResult.OK)
            {
                _subject.Faces[0].Image.Save(saveImageDialog.FileName);
            }
        }

        #endregion

        #region Public methods
        
        public void DisposeNComponents()
        {
            if (_deviceManager != null && !_deviceManager.IsDisposed)
            {
                _deviceManager.Dispose();
            }

            if (_camera != null && !_camera.IsDisposed)
                _camera.Dispose();
            EnableControls(false);
            if (_biometricClient != null)
            {
                _biometricClient.Reset();
                _biometricClient.Cancel();
            }
        }

        #endregion

        // The area we are selecting.
        private int X0, Y0, X1 = 100, Y1 = 100;

        private void facesView_MouseDown(object sender, MouseEventArgs e)
        {
            X0 = e.X;
            Y0 = e.Y;

            // Make a Bitmap to display the selection rectangle.
            var bm = new Bitmap(_subject.Faces[0].Image.ToBitmap());
            
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.DrawRectangle(Pens.Red, X0, Y0, 450, 350);
            }
            // Display the temporary bitmap.
            //facesView.Face = bm;
            _croppedPhotoImage = bm;

        }

        private Bitmap ResizeImage(MemoryStream sourceStream, int width, int height, bool crop, double x1, double x2, double y1, double y2)
        {
            try
            {
                // Resize settings
                var resizeSettings = new Instructions
                {
                    Scale = ScaleMode.DownscaleOnly,
                    Width = width,
                    Height = height,
                    CropRectangle = new [] {x1, y1, x2, y2},
                    Anchor = AnchorLocation.MiddleCenter
                };
                
                if (crop) resizeSettings.Mode = FitMode.Crop;
                // Resize image
                var destinationStream = new MemoryStream();
                sourceStream.Seek(0, SeekOrigin.Begin);
                new ImageJob(sourceStream, destinationStream, resizeSettings).Build();
                // Save resized image
                var bm = new Bitmap(destinationStream);
                return bm;
            }
            catch (Exception ex)
            {
                ErrorLogger.LoggError(ex.StackTrace, ex.Source, ex.ToString());
                return null;
            }
        }
        
        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (pictureBoxCroped.Image == null)
            {
                MessageBox.Show(this, @"Please Crop the Picture before you Click OK");
                return;
            }
            CancelWork();
            DisposeNComponents();
            CapturePhotoNotifyer(_croppedPhotoImage, _template.ToArray(), true);
            ParentForm.Dispose();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DisposeNComponents();
            CapturePhotoNotifyer(null, null);
            ParentForm.Dispose();
        }

    }
}
