using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class Approval : Entity
    {
        public int TableId { get; set; }
        [Index("IX_approvalid", IsUnique = true)]
        public string ApprovalId { get; set; }
        public string Comment { get; set; }
        [Index("IX_Enrollment_approval")]
        public string EnrollmentId { get; set; }
        public DateTime DateProcessed { get; set; }
        public string ProcessedById { get; set; }
        public int ApprovalStatus { get; set; }
    }
}
