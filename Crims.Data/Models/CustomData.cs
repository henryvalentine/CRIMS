using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class CustomData : Entity
    {
        public int TableId { get; set; }
        [Index("IX_customdataid", IsUnique = true)]
        public string CustomDataId { get; set; }
        [Index("IX_customfiled_customdata")]
        public string CustomFieldId { get; set; }
        [Index("IX_enrollment_customdata")]
        public string EnrollmentId { get; set; }
        public string ChildCrimsCustomData { get; set; }
        public string CrimsCustomData { get; set; }
        public string CustomListId { get; set; }
        public string ProjectSIteId { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
        public virtual BaseData BaseData { get; set; }
    }
}

