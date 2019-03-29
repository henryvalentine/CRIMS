using System.Collections.Generic;
using Crims.Data.Models;

namespace Crims.UI.Win.Enroll.Classes
{
    public class ApprovalRecord
    {
        public IList<Approval> Approvals { get; set; }
        public string EnrollmentId { get; set; }
        public int ApprovalStatus { get; set; }

    }
    
}