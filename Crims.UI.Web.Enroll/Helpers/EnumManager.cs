using System.Collections.Generic;
using Crims.Data.Models;

namespace Crims.UI.Web.Enroll.Helpers
{
    public class EnumManager
    {
        public enum ApprovalStatus
        {
            Pending = 1,
            Approved,
            Rejected,
            LockedForApproval,
            Recycled
        }

        public enum FingerDescription
        {
            LFLittle = 1,
            LFRing = 2,
            LFMiddle = 3,
            LFIndex = 4,
            LFThumb = 5,
            RFThumb = 6,
            RFIndex = 7,
            RFMiddle = 8,
            RFRing = 9,
            RFLittle = 10,
            Unknown = 11
        }
    }
}
