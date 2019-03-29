using Crims.UI.Win.Enroll.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Crims.UI.Win.Enroll.Properties;
using Neurotec.Biometrics;
using Neurotec.Biometrics.Client;
using Neurotec.Images;
using Neurotec.IO;


namespace Crims.UI.Win.Enroll.Classes
{
    [Serializable]
    public class BiometricsRecord
    {
        public string TableId { get; set; }
        public string EnrollmentId { get; set; }
        public string ProjectPrimaryCode { get; set; }
        public string Title { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Gender { get; set; }
        public string CreatedById { get; set; }
        public string LastUpdatedById { get; set; }

        public Image Photograph { get; set; }
        public byte[] PhotographTemplate { get; set; }

        public List<FingerImageRecord> FingerprintRecords { get; set; }
        public byte[] FingerTemplates { get; set; }//A combination of all Templates into One Template

        public Image Signature { get; set; }

        public BiometricsRecord()
        {
            FingerprintRecords = new List<FingerImageRecord>();
        }
        
        private bool CheckDuplicate(FingerImageRecord fRecord)
        {
            try
            {
                if (FingerprintRecords.Count == 0)
                {
                    FingerprintRecords.Add(fRecord);
                    return true;
                }

                //NBuffer .GetTemplateBuffer()

                using (var biometricClient = new NBiometricClient())
                {
                    // Set matching threshold
                    biometricClient.MatchingThreshold = (int)Settings.Default.MatchingScore;

                    // Set matching speed
                    biometricClient.FingersMatchingSpeed = NMatchingSpeed.Low;

                    var matcherFound = false;
                    FingerprintRecords.ForEach(item =>
                    {
                        if (item.FingerTemplate != null)
                        {
                            var status = biometricClient.Verify(fRecord.FingerSubject, item.FingerSubject);
                            if (status == NBiometricStatus.Ok)
                            {
                                matcherFound = true;
                                item = fRecord;
                            }
                        }

                    });

                    if (matcherFound)
                    {
                        return false;
                    }
                    FingerprintRecords.Add(fRecord);
                    return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, @"Process Fingerprint");
                return false;
            }
            
        }
        internal int GetActiveUserFingerRecordsCount()
        {
           return FingerprintRecords.Count;
        }
        internal bool SaveActiveUserFingerRecords(NFinger finger, NSubject fingerSubject, FingerDescription fingerDescription)
        {
            try
            {
                var buffArray = fingerSubject?.GetTemplateBuffer().ToArray();

                var fRecord = new FingerImageRecord
                {
                    FingerIndex = GetFingerIndex(fingerDescription),
                    FingerDescription = fingerDescription,
                    FingerRecord = finger?.Objects[0].Template,
                    FingerSubject = fingerSubject,
                    FingerTemplate = buffArray
                };

                if (finger != null)
                {
                    var wsq = finger.Image.Save(NImageFormat.Wsq);
                    fRecord.FingerImage = finger.Image.ToBitmap();
                    fRecord.FingerWsq = wsq?.ToArray();
                    int fingersCount = 0;
                    var status = CheckDuplicate(fRecord);
                    return status;
                }
                return false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            
        }

        private int GetFingerIndex(FingerDescription fingerDescription)
        {
            switch (fingerDescription)
            {
                case FingerDescription.LFLittle:
                    return 1;
                case FingerDescription.LFRing:
                    return 2;
                case FingerDescription.LFMiddle:
                    return 3;
                case FingerDescription.LFIndex:
                    return 4;
                case FingerDescription.LFThumb:
                    return 5;
                case FingerDescription.RFThumb:
                    return 6;
                case FingerDescription.RFIndex:
                    return 7;
                case FingerDescription.RFMiddle:
                    return 8;
                case FingerDescription.RFRing:
                    return 9;
                case FingerDescription.RFLittle:
                    return 10;
                default:
                    return 0;                       
            }
        }
    }
}
