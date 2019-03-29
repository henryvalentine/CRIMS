using System.Collections.Generic;
using Crims.Data.Models;

namespace Crims.API.Helpers
{
    public class ActivityResponse
    {
        public string Message { get; set; }
        public long Code { get; set; }
        public int FailedCount { get; set; }
        public string DownloadLink { get; set; }
        public List<string> SaveMessages { get; set; }
        public List<CustomListData> ListDataList { get; set; }
        public List<CustomData> CustomDataList { get; set; }
    }

    public class ApiResponse
    {
        public string Message { get; set; }
        public long Code { get; set; }
        public dynamic Response { get; set; }
    }

    public class FileDesc
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }

        public FileDesc(string n, string p, long s)
        {
            Name = n;
            Path = p;
            Size = s;
        }
    }
}
