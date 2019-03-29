using System.Collections.Generic;
using Crims.Data.Models;
using CustomDataViewModel = Crims.UI.Web.Enroll.Models.CustomDataViewModel;

namespace Crims.UI.Web.Enroll.Helpers
{
    class ActivityResponse
    {
        public string Message { get; set; }
        public string EnrollmentId { get; set; }
        public string UserRole { get; set; }
        public long Code { get; set; }
        public string DownloadLink { get; set; }
        public string Email { get; set; }
        public string FilePath { get; set; }
        public List<string> SaveMessages { get; set; }
        public List<CustomListData> ListDataList { get; set; }
        public List<CustomDataViewModel> CustomDataList { get; set; }
    }
}
