using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crims.Core.Models.Legacy
{
    public class MatchingResult
    {
        public int TemplateId { get; set; }
        public int BaseDataId { get; set; }
        public string UserPrimaryCode { get; set; }
        public int Score { get; set; }
    }
}
