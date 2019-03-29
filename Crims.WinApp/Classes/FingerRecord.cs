using Crims.UI.Win.Enroll.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurotec.Biometrics;

namespace Crims.UI.Win.Enroll.Classes
{
    [Serializable]
    public class FingerImageRecord
    {
        public int FingerIndex;
        public Image FingerImage { get; set; }
        public NFRecord FingerRecord { get; set; }
        public NSubject FingerSubject { get; set; }
        public byte[] FingerTemplate { get; set; }
        public byte[] FingerWsq { get; set; }
        public FingerDescription FingerDescription { get; set; }
    }

    public class Finger
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class FingerReason
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
