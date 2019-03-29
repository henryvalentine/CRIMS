using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class Photograph : Entity
    {
        public int TableId { get; set; }
        [Index("IX_Enrollment_photograph")]
        public string EnrollmentId { get; set; }
        [Index("IX_photographid", IsUnique = true)]
        public string PhotographId { get; set; }
        public byte[] PhotographTemplate { get; set; }
        public byte[] PhotographImage { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
        public string PhotographImagePath { get; set; }
        public virtual BaseData BaseData { get; set; }
    }
}
