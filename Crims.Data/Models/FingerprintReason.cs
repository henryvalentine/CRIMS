using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class FingerprintReason : Entity
    {
        public int TableId { get; set; }
        [Index("IX_Enrollment_fingerprintreason")]
        public string EnrollmentId { get; set; }
        public string FingerReason { get; set; }
        public int FingerIndex { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
        public virtual BaseData BaseData { get; set; }
    }
}
