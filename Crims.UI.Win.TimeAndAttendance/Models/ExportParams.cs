using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TandAProject.Models
{
    public class ExportParams
    {
        public bool AllData { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ExportDir { get; set; }
    }
}
