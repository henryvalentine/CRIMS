using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TandAProject.Models
{
    public class DbTemplateRecord
    {
        public int baseDataId { get; set; }
        public string UserPrimaryCode { get; set; }
        public int templateId { get; set; }
        public byte[] template { get; set; }
    }
}
