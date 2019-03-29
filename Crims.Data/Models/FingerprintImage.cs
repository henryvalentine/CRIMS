using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class FingerprintImage : Entity
    {
        public int TableId { get; set; }
        [Index("IX_Enrollment_fingerprintimage")]
        public string EnrollmentId { get; set; }
        public byte[] FingerPrintImage { get; set; }
        [Index("IX_fingerprintindex")]
        public int FingerIndexId { get; set; }
        public string FilePath { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
        public virtual BaseData BaseData { get; set; }
    }
}
