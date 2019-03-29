using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class CustomList : Entity
    {
        public int TableId { get; set; }
        public string CustomListId { get; set; }
        public string CustomListName { get; set; }
        public string ParentListId { get; set; }
        
        //public ICollection<CustomListData> CustomListDatas { get; set; }
        //public ICollection<CustomField> CustomFields { get; set; }
    }
}
