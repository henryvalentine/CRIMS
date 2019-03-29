using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class CustomListData : Entity
    {
        public int TableId { get; set; }
        public string CustomListDataId { get; set; }
        public string CustomListId { get; set; }
        public string ListDataName { get; set; }
        public string ParentNodeId { get; set; }
        //[ForeignKey("CustomListId")]
        //public CustomList CustomList { get; set; }
    }
}

