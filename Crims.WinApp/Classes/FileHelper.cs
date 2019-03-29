using Crims.Core.Utils.Serializer;
using Crims.UI.Win.Enroll.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurotec.Biometrics;
using Neurotec.Biometrics.Client;
using Neurotec.Images;
using Neurotec.IO;

namespace Crims.UI.Win.Enroll.Classes
{
    public class FileHelper
    {

        internal static BiometricsRecord GetBiometricsRecord(BiometricsRecord _BiometricsRecord, string SourceFileDir)
        {
            String userDir = SourceFileDir + "\\" + _BiometricsRecord.EnrollmentId + "\\";

            //Find Images If Exists and Save in Biometrics Record

            //Get Signature
            if (File.Exists(userDir + "sign_image.jpg"))
            {
                byte[] imageBytes = File.ReadAllBytes(userDir + "sign_image.jpg");
                Image image = Image.FromStream(new MemoryStream(imageBytes));
                _BiometricsRecord.Signature = image;
            }

            //Get Photo
            if (File.Exists(userDir + "photo_image.jpg"))
            {
                byte[] imageBytes = File.ReadAllBytes(userDir + "photo_image.jpg");
                Image image = Image.FromStream(new MemoryStream(imageBytes));
                _BiometricsRecord.Photograph = image;
            }

            //Get Photo Template
            if (File.Exists(userDir + "photo_template.tem"))
            {
                byte[] imageBytes = File.ReadAllBytes(userDir + "photo_template.tem");
                _BiometricsRecord.PhotographTemplate = imageBytes;
            }

            //Get Finger Images
            foreach (var fingerDescription in Enum.GetValues(typeof(FingerDescription)))
            {
                string fingerFile = userDir + fingerDescription + ".jpg";
                string fingerTemplateFile = userDir + fingerDescription + ".tem";

                Image image = null;
                if (File.Exists(fingerFile))
                {
                    byte[] imageBytes = File.ReadAllBytes(fingerFile);
                    image = Image.FromStream(new MemoryStream(imageBytes));
                }

                //Get Finger Template
                if (File.Exists(fingerTemplateFile))
                {
                    var imageBytes = File.ReadAllBytes(fingerTemplateFile);
                    var fingerprintBuffer = new NBuffer(imageBytes);

                    //NFinger finger, NBuffer fingerTemplate
                    using (var biometricClient = new NBiometricClient())
                    using (var subject = new NSubject())
                    using (var finger = new NFinger())
                    {
                        //Read finger image from enrollment and add it to NFinger object
                        finger.Image = NImage.FromMemory(fingerprintBuffer, NImageFormat.Wsq);
                        //add NFinger object to NSubject
                        subject.Fingers.Add(finger);
                        ////Set finger template size (recommended, for enroll to database, is large) (optional)
                        //biometricClient.FingersTemplateSize = NTemplateSize.Large;

                        //Create template from added finger image
                        var status = biometricClient.CreateTemplate(subject);
                        if (status == NBiometricStatus.Ok)
                        {
                            _BiometricsRecord.SaveActiveUserFingerRecords(finger, subject, (FingerDescription)fingerDescription);
                        }
                    }
                }
            }
            //Get Grouped Finger Templates
            if (File.Exists(userDir + "fingers_template.tem"))
            {
                byte[] imageBytes = File.ReadAllBytes(userDir + "fingers_template.tem");
                _BiometricsRecord.FingerTemplates = imageBytes;
            }
            return _BiometricsRecord;
        }

        internal bool SaveBiometricsRecords(BiometricsRecord BiometricsRecord, string DestFileDir)
        {
            NFTemplate _NFTemplate = new NFTemplate();
            //Create Folder for the User
            String userDir = DestFileDir + "\\" + BiometricsRecord.EnrollmentId + "\\";
            if (!Directory.Exists(userDir))
            {
                DirectoryInfo dir = Directory.CreateDirectory(userDir);
            }

            //Save User Signature
            if (BiometricsRecord.Signature != null)
            {
                BiometricsRecord.Signature.Save(userDir + "sign_image.jpg");
            }

            //Save User Photo
            if (BiometricsRecord.Photograph != null)
            {
                BiometricsRecord.Photograph.Save(userDir + "photo_image.jpg");
                File.WriteAllBytes(userDir + "photo_template.tem", BiometricsRecord.PhotographTemplate.ToArray());
            }

            if (BiometricsRecord.FingerprintRecords.Count > 0)
            {
                //save Fingerprint
                foreach (var userFingerprint in BiometricsRecord.FingerprintRecords)
                {
                    userFingerprint.FingerImage.Save(userDir + userFingerprint.FingerDescription + ".jpg");
                    File.WriteAllBytes(userDir + userFingerprint.FingerDescription + ".tem", userFingerprint.FingerTemplate.ToArray());

                    //Add FingerTemplate to NFTemplate

                    if (userFingerprint.FingerRecord != null)
                    {
                        _NFTemplate.Records.Add(userFingerprint.FingerRecord);
                    }
                    else
                    {
                        var wsq = new NBuffer(userFingerprint.FingerWsq);

                        //var image = NImage.FromMemory(wsq, NImageFormat.Wsq).ToBitmap();

                        //NFinger finger, NBuffer fingerTemplate
                        using (var biometricClient = new NBiometricClient())
                        using (var subject = new NSubject())
                        using (var finger = new NFinger())
                        {
                            //Read finger image from enrollment and add it to NFinger object
                            finger.Image = NImage.FromMemory(wsq, NImageFormat.Wsq);
                            //add NFinger object to NSubject
                            subject.Fingers.Add(finger);
                            ////Set finger template size (recommended, for enroll to database, is large) (optional)
                            //biometricClient.FingersTemplateSize = NTemplateSize.Large;

                            //Create template from added finger image
                            var status = biometricClient.CreateTemplate(subject);
                            if (status == NBiometricStatus.Ok)
                            {
                                userFingerprint.FingerRecord = finger.Objects[0].Template;
                                _NFTemplate.Records.Add(finger.Objects[0].Template);
                            }
                        }
                        //NLicense.ReleaseComponents("Biometrics.FingerExtraction");
                    }

                }

                //Save Grouped FingerTemplates
                var ms = new NMemoryStream();
                _NFTemplate.Save(ms);
                BiometricsRecord.FingerTemplates = ms.ToArray();
                File.WriteAllBytes(userDir + "fingers_template.tem", BiometricsRecord.FingerTemplates);

            }

            //Save User Record as Serilalised Binary Data
            BinarySerialization.WriteToBinaryFile(userDir + "UserRecord.crims", BiometricsRecord);

            //If the code runs to this point without Failure, We are good
            return true;
        }
    }
}
