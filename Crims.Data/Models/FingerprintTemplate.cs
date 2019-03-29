using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class FingerprintTemplate : Entity
    {
        public int TableId { get; set; }
        [Index("IX_enrollment_fingerprinttemplate")]
        public string EnrollmentId { get; set; }
        public byte[] Template { get; set; }
        public int? UniquenessStatus { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
        public string FilePath { get; set; }
        public virtual BaseData BaseData { get; set; }
    }
}
