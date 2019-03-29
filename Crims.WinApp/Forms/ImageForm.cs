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
using Crims.Core.Logging;
using Crims.UI.Win.Enroll.Helpers;
using Crims.UI.Win.Enroll.Properties;
using ImageResizer;
using Neurotec.Biometrics.Gui;
using Neurotec.DeviceManager;
using Neurotec.Devices;
using Neurotec.Images;
using Neurotec.IO;
using Neurotec.Media;
using NDeviceManager = Neurotec.Devices.NDeviceManager;

namespace Crims.UI.Win.Enroll.Forms
{
    public partial class ImageForm : Form
    {
        private NLExtractor extractor;
        private NleDetectionDetails[] facedetectdetails;
        private bool faceInImage = false;
        private NFaceView facesView;
        private NCamera activeCamera;
        private NBuffer _template;
        private NDeviceManager _deviceManager;
        private bool faceAvailabilityStatus = false;
        private String currentImagesCaptured = String.Empty;
        private int HorizontalPst = 0; private String LowerKey = "nil";
        private int VerticalPst = 0; private String CmpID = String.Empty;
        private int BottomPst = 0; private String UpperKey = String.Empty;
        string licencekey = String.Empty;

        PictureBox _destinationImage;
        List<NCamera> _cameras = new List<NCamera>();
        //Camera activeCamera = null;
        Bitmap video;
        public Image CroppedImageClone;

        bool enableCrop = false;
        Image _croppedImage = null;

        private NFExtractor extractor1;
        NFView _nfView;
        private NMatcher matcher = null;

        public int CropWidth = 100;
        public int CropHeight = 100;
        public int DestinationImageWidth = 100;
        public int DestinationImageHeight = 100;

        NMediaFormat[] _cvfs;
        StringBuilder _errorLog = new StringBuilder();

        public ImageForm(PictureBox destinationImage)
        {
            InitializeComponent();
            try
            {
                //Activating Biometrics Licenses
                IList<string> licenses = new List<string>(new [] { "FacesExtractor", "FacesBSS" });
                AppUtils.ObtainLicenses(licenses);

                //Create a MegaMatcher Biometrics extractor Instance
                // extractor1 = new NFExtractor();
                // matcher = new NFMatcher();

                //  currentImagesCaptured = getdestination;

                facesView = new NLView();
                facesView.Click += new EventHandler(facesView_Click);
                facesView.Visible = true; facesView.Anchor = AnchorStyles.Left;
                facesView.Size.Height.Equals(467);
                facesView.Size.Width.Equals(350); facesView.Dock = DockStyle.Fill;
                target.Controls.Add(facesView);

                this._destinationImage = destinationImage;


                CropWidth = 350;
                CropHeight = 467;
                DestinationImageWidth = 176;
                DestinationImageHeight = 176;

                _nfView = new NFView
                {
                    Height = 0,
                    Width = 0
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"License", @"Biometric", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        
        private void DisposeImage(PictureBox pictureBox)
        {
            //menuFileSaveAs.Enabled = false;				// disable "Save As" menu entry
            Image oldImg = pictureBox.Image;
            pictureBox.Image = null;					// empty picture box
            if (oldImg != null)
                oldImg.Dispose();						// dispose old image (free memory, unlock file)
        }
        
        private void UpdateCameraList()
        {
            comboBoxCameras.BeginUpdate();
            try
            {
                comboBoxCameras.Items.Clear();
                _deviceManager.Refresh();
                foreach (var device in _deviceManager.Devices)
                {
                    comboBoxCameras.Items.Add(device);
                }

                if (activeCamera != null && activeCamera.IsDisposed) activeCamera = null;

                if (activeCamera == null && comboBoxCameras.Items.Count > 0)
                {
                    comboBoxCameras.SelectedIndex = 0;
                    comboBoxCameras.SelectedItem = comboBoxCameras.Items[0];
                    btCaptureDevicePhoto.Enabled = true;
                    buttonUseImage.Enabled = false;
                    return;
                }

                if (activeCamera != null)
                {
                    comboBoxCameras.SelectedIndex = comboBoxCameras.Items.IndexOf(activeCamera);
                }
            }
            catch (Exception exp)
            {
                _errorLog.Append(exp + Environment.NewLine);
                comboBoxCameras.EndUpdate();
                buttonStopCapture.Enabled = btCaptureDevicePhoto.Enabled = false;
                MessageBox.Show(exp.ToString());
            }
            
        }
        
        //private void buttonStopCapture_Click(object sender, EventArgs e)
        //{
        //    StopVideoCapture();
        //}
        
        private Bitmap ResizeImage(MemoryStream sourceStream, int width, int height, bool crop)
        {
            try
            {
                // Resize settings
                var resizeSettings = new Instructions
                {
                    Scale = ScaleMode.DownscaleOnly,
                    Width = width,
                    Height = height,
                    Anchor = AnchorLocation.MiddleCenter,
                    Mode = FitMode.Max
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

        private void facesView_Click(object sender, EventArgs e)
        {
            if (facesView.Image != null)
            {
                //if (enableCrop)
                //{
                    //Check if camera is running and stop
                    if (activeCamera != null && activeCamera.IsCapturing)
                    {
                        StopVideoCapture();
                    }
                    try
                    {
                        //int w = Int32.Parse(cropWidth.ToString());
                        //int h = Int32.Parse(cropHeight.ToString());

                        //var point = this.target.PointToClient(Control.MousePosition);

                        //croppedImage = Utils.cropSection((Image)facesView.Image.Clone(), int.Parse(point.X.ToString()), int.Parse(point.Y.ToString()), w, h);
                        var size = new Size { Width = int.Parse(Settings.Default.ImageWidth), Height = int.Parse(Settings.Default.ImageHeight) };
                        var memoryStream = new MemoryStream();
                        var image = (Image) facesView.Image.Clone();
                        image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        var resizedImage = ResizeImage(memoryStream, size.Width, size.Height, true);
                        if (resizedImage != null)
                        {
                            _croppedImage = resizedImage;
                        }

                    }
                    catch (Exception exp)
                    {
                        _errorLog.Append(exp + Environment.NewLine);
                    }
                //}
                //else
                //{
                //    MessageBox.Show(this, @"Please Click on Use Image Button To Use Cropped Image", this.Text);
                //}
            }
            else
            {
                MessageBox.Show(this, @"There is no Image to Crop", this.Text);
            }
        }

        private void StopVideoCapture()
        {
            try
            {
                backgroundWorker.CancelAsync();
                timer.Stop();


                if (facesView.Face.Image != null & facedetectdetails.Length > 0)
                {
                    HorizontalPst = (((facedetectdetails[0].Eyes.First.X) + (facedetectdetails[0].Eyes.Second.X)) / 2);

                    VerticalPst = (facedetectdetails[0].Eyes.First.Y + facedetectdetails[0].Eyes.Second.Y) / 2;
                    BottomPst = facedetectdetails[0].Face.Rectangle.Bottom;



                    var image = activeCamera.GetFrame();
                    var ntfi = new Ntfi();
                    var token = ntfi.CreateTokenFaceImage(image, facedetectdetails[0].Eyes.First, facedetectdetails[0].Eyes.Second);
                    _croppedImage = token.ToBitmap();
                    faceInImage = true; buttonUseImage.Enabled = true;
                    activeCamera.StopCapturing();
                    /* commented by Ayo
                    if (VerticalPst < 320)
                    {
                        if (HorizontalPst > 230)
                        {
                            if (BottomPst < 335)
                            {
                                NImage image = activeCamera.GetCurrentFrame();
                                Ntfi ntfi = new Ntfi();
                                NImage token = ntfi.CreateTokenFaceImage(image, facedetectdetails[0].Eyes.First, facedetectdetails[0].Eyes.Second);
                                croppedImage = token.ToBitmap();
                                faceInImage = true; buttonUseImage.Enabled = true;
                                activeCamera.StopCapturing();
                            }
                            else { MessageBox.Show("Image Not Position Properly!", "Bottom Post"); }

                        }
                        else
                        {
                            MessageBox.Show("Image Not position Properly!", "Horzontal Post");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Image too close to camera!", "Vertical Post");
                    }
                    */
                    //end ayo comments
                }
                else
                {
                    faceInImage = false;
                    MessageBox.Show(this, @"No eyes and face detected", @"Face Biometrics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception exp)
            {
                _errorLog.Append(exp.ToString() + Environment.NewLine);
            }

            finally
            {
                btCaptureDevicePhoto.Enabled = true;
                buttonStopCapture.Enabled = false;

            }
        }
       
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                activeCamera = (NCamera)e.Argument;
                activeCamera.StartCapturing();
                while (activeCamera.IsCapturing)
                {
                    if (backgroundWorker.CancellationPending)
                    {

                        activeCamera.StopCapturing();
                    }

                    if (activeCamera != null && activeCamera.IsCapturing)
                    {
                        using (NImage image = activeCamera.GetFrame())
                        {
                            video = image.ToBitmap();

                            using (NLExtractor extractor = new NLExtractor())
                            {
                                // convert image to grayscale
                                NGrayscaleImage grayscale = (NGrayscaleImage)NImage.FromImage(NPixelFormat.Grayscale, 0, image);
                                extractor.MaxRecordsPerTemplate = 1;
                                // detect all faces that are suitable for face recognition in the image
                                NleFace[] faces = extractor.DetectFaces(grayscale);
                                //NleDetectionDetails[] detectionDetails 
                                facedetectdetails = new NleDetectionDetails[faces.Length];
                                for (int i = 0; i < facedetectdetails.Length; i++)
                                {
                                    facedetectdetails[i] = extractor.DetectFacialFeatures(grayscale, faces[i]);
                                }
                                facesView.DrawConfidenceForEyes = true;
                                facesView.DrawFaceConfidence = true;
                                facesView.DetectionDetails = facedetectdetails;

                                for (int i = 0; i < facedetectdetails.Length; i++)
                                {
                                    faceAvailabilityStatus = facedetectdetails[i].FaceAvailable;
                                }
                                if (facesView.DrawConfidenceForEyes == true & facesView.DrawFaceConfidence == true)
                                {
                                    faceAvailabilityStatus = true;
                                }
                                else
                                {
                                    faceAvailabilityStatus = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                //   MessageBox.Show("Error Capturing Image - Close and re-open browser window");
                // Logger.LogError(exp.ToString());
                //errorLog.Append(exp.ToString() + Environment.NewLine);
                e.Cancel = true;
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    _errorLog.Append(Environment.NewLine + "Image Capture Process Was Canceled...");
                }
                else if (e.Error != null)
                {
                    _errorLog.Append(Environment.NewLine + "An error has occurred during Image capture...");
                    _errorLog.Append(e.Error.ToString());

                }

                _errorLog.Append(Environment.NewLine + "Image Capture Process Completed at " + DateTime.Now.ToString());
                _errorLog.Append(Environment.NewLine + Environment.NewLine);
            }
            catch (Exception exp)
            {
                _errorLog.Append(exp.ToString() + Environment.NewLine);
            }
            finally
            {

                if (faceInImage == true)
                {
                    buttonUseImage.Enabled = true;
                }
                else
                {
                    buttonUseImage.Enabled = false;
                }

                btCaptureDevicePhoto.Enabled = true;
                buttonStopCapture.Enabled = false;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                facesView.Face.Image = video;
            }
            catch (Exception exp)
            {
                _errorLog.Append(exp.ToString() + Environment.NewLine);
            }
        }
        
        private void ImageForm_Load(object sender, EventArgs e)
        {
            UpdateCameraList();
        }

        private void comboBoxCameras_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeCamera = comboBoxCameras.SelectedItem as NCamera;
        }

        private void ImageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
            Dispose();
        }

        private void buttonUseImage_Click(object sender, EventArgs e)
        {
            try
            {

                if (_croppedImage != null & faceAvailabilityStatus == true)
                {
                    CroppedImageClone = (Image)_croppedImage.Clone();
                    Bitmap bmpCropped = new Bitmap(DestinationImageWidth, DestinationImageHeight);

                    // Get a Graphics object from the Bitmap for drawing.
                    Graphics grBitmap = Graphics.FromImage(bmpCropped);

                    // Draw the image on the Bitmap anchored at the upper left corner.
                    grBitmap.DrawImage(CroppedImageClone, 0, 0, DestinationImageWidth, DestinationImageHeight);

                    // Set the database image to the new cropped image.
                    _croppedImage = bmpCropped;
                    // Set the PictureBox image to the new cropped image.

                    _destinationImage.Image = _croppedImage;
                    this.DialogResult = DialogResult.OK;
                    //IList<string> licenses = new List<string>(new string[] { "FacesExtractor", "FacesBSS" });
                    //AppUtils.ReleaseLicenses(licenses);
                    //this.Visible = false;
                    Close();
                    Dispose();
                }
                else
                {
                    MessageBox.Show(@"Eyes not detected.");
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Close();
            Dispose();
        }

        private void btCaptureDevicePhoto_Click(object sender, EventArgs e)
        {
            try
            {
                buttonUseImage.Enabled = true;
                _croppedImage = null;
                var camera = _cameras[comboBoxCameras.SelectedIndex];
                _cvfs = activeCamera.GetFormats();

                timer.Start();
                backgroundWorker.RunWorkerAsync(camera);
            }
            catch (Exception exp)
            {
                _errorLog.Append(exp.ToString() + Environment.NewLine);
            }

            finally
            {
                btCaptureDevicePhoto.Enabled = false;
                buttonStopCapture.Enabled = true;
            }
        }
        
        private void buttonStopCapture_Click(object sender, EventArgs e)
        {
            StopVideoCapture();
        }
    }
}
