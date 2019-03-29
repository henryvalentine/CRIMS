using Crims.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crims.UI.Web.Helpers
{
    class ActivityResponse
    {
        public string Message { get; set; }
        public string EnrollmentId { get; set; }
        public string UserRole { get; set; }
        public long Code { get; set; }
        public string DownloadLink { get; set; }
        public string ResponseId { get; set; }
        public string Email { get; set; }
        public string FilePath { get; set; }
        public List<string> SaveMessages { get; set; }
        public List<CustomListDataViewModel> ListDataList { get; set; }
        public List<CustomData> CustomDataList { get; set; }
    }
}
