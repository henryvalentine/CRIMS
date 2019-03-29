using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crims.UI.Win.Enroll.Enums;

namespace Crims.UI.Win.Enroll.DataServices
{
    class SyncBacklogInput
    {
        public DateTime FilterEnd { get; internal set; }
        public DateTime FilterStart { get; internal set; }
        public SyncMode SyncMode { get; internal set; }
        public string EnrollmentId { get; internal set; }
        public string UserId { get; internal set; }
    }
}
