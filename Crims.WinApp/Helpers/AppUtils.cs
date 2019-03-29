using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Neurotec.Biometrics.Gui;
using Neurotec.Licensing;

namespace Crims.UI.Win.Enroll.Helpers
{
    class AppUtils
    {
        private const string LicensesConfiguration = "NLicenses.cfg";

        private static Dictionary<string, string> _licenseCfg = null;

        public static Image ResizeImage(Bitmap imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        public static Image CropSection(Image sourceImage, int x, int y, int w, int h)
        {
            Image cropedImage;
            try
            {
                Rectangle recSource = new Rectangle(x, y, w, h);
                Bitmap bmpCropped = new Bitmap(w, h);

                // Get a Graphics object from the Bitmap for drawing.
                Graphics grBitmap = Graphics.FromImage(bmpCropped);

                // Draw the image on the Bitmap anchored at the upper left corner.
                grBitmap.DrawImage((Image)sourceImage.Clone(), 0, 0, recSource, GraphicsUnit.Pixel);

                // Set the PictureBox image to the new cropped image.
                cropedImage = bmpCropped;
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return cropedImage;
        }

        // Load licenses configuration file with names of licenses to obtain
        private static void LoadConfiguration()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();

            string path = Path.Combine(GetAssemblyPath(), LicensesConfiguration);
            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                string[] values = line.Split('=');
                string value = (values.Length > 1) ? values[1] : "";
                if (values.Length > 0)
                {
                    // values[0] is the key
                    config.Add(values[0].Trim(), value.Trim());
                }
            }

            _licenseCfg = config;
        }

        public static string GetAssemblyPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static void ObtainLicenses(string license)
        {
            ObtainLicenses(new string[] { license });
        }

        public static void ObtainLicenses(IList<string> licenses)
        {
#if !N_NOT_USES_LICENSES
            int i, j;

            if (_licenseCfg == null)
            {
                LoadConfiguration();
            }

            for (i = 0; i < licenses.Count; i++)
            {
                if (_licenseCfg.ContainsKey(licenses[i]))
                    licenses[i] = _licenseCfg[licenses[i]];
                else
                    licenses[i] = string.Empty;
            }

            // Remove duplicates
            for (i = 0; i < licenses.Count - 1; i++)
            {
                if (licenses[i] == string.Empty)
                {
                    continue;
                }

                for (j = i + 1; j < licenses.Count; j++)
                {
                    if (licenses[i] == licenses[j])
                    {
                        licenses[j] = string.Empty;
                    }
                }
            }

            string licenseServer = _licenseCfg["Address"];
            string licensePort = _licenseCfg["Port"];

            for (i = 0; i < licenses.Count; i++)
            {
                if (licenses[i] == string.Empty)
                    continue;

                try
                {
                    bool available = NLicense.Obtain("/local", "5000", licenses[i]);

                    if (!available)
                    {
                        throw new Exception(string.Format("license for {0} was not obtained", licenses[i]));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Error while obtaining license. Please check if Neurotec License Manager is running. Details: {0}", ex.Message));
                }
            }
#endif
        }

        public static void ReleaseLicenses(IList<string> licenses)
        {
#if !N_NOT_USES_LICENSES
            for (int i = 0; i < licenses.Count; i++)
            {
                if (licenses[i] == string.Empty)
                    continue;

                try
                {
                    NLicense.Release(licenses[i]);
                }
                catch (Exception)
                {
                }
            }
#endif
        }

        public static void showSectionCords(Panel sourcePictureBox, int x, int y, int w, int h)
        {
            sourcePictureBox.Refresh();
            // Draw a red rectangle to show where the image will be cropped.
            Rectangle recCropBox = new Rectangle(x, y, w, h);
            sourcePictureBox.CreateGraphics().DrawRectangle(Pens.Red, recCropBox);
        }

        public static void loadCollectionParameter(Object listControl, string parameter, char splitChar)
        {
            String[] collectionItems = parameter.Split(splitChar);
            Type type = listControl.GetType();

            string typeName = type.Name;

            if (typeName == "ComboBox")
            {
                ComboBox combo = (ComboBox)listControl;
                combo.Items.AddRange(collectionItems);
            }

            if (typeName == "ListBox")
            {
                ListBox list = (ListBox)listControl;
                list.Items.AddRange(collectionItems);
            }

        }

        public static int MatchingThresholdFromString(string value)
        {
            double p = Math.Log10(Math.Max(double.Epsilon, Math.Min(1,
                double.Parse(value.Replace(CultureInfo.CurrentCulture.NumberFormat.PercentSymbol, "")) / 100)));
            int i = Math.Max(0, (int)Math.Round(-12 * p));
            return i;
        }


        public static void ShowException(Exception ex)
        {
            while ((ex is AggregateException) && (ex.InnerException != null))
                ex = ex.InnerException;

            MessageBox.Show(ex.ToString(), null, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
