 
using System.Collections.Generic;
using Crims.Data.Models;

namespace Crims.Data.Models
{
    public partial class ProjectCustomListViewModel
    {
        public int TableId { get; set; }
        public string CustomListId { get; set; }
        public string ProjectCode { get; set; }
        public string CustomListName { get; set; } 
    }

    public partial class CustomListSelectable
    {
        public List<CustomList> CustomLists { get; set; }
    }
}

