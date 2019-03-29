 
using System.Collections.Generic;
using Crims.Data.Models;

namespace Crims.Data.Models
{
    public partial class ProjectCustomListDataViewModel
    {
        public int TableId { get; set; }
        public string CustomListDataId { get; set; }
        public string ProjectCode { get; set; }
        public string CustomListDataName { get; set; } 
    }

    public partial class ProjectCustomListDataSelectable
    {
        public List<CustomListData> CustomListDatas { get; set; }
    }
    
}

