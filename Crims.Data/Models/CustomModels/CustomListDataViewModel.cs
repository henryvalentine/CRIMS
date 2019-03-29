
namespace Crims.Data.Models
{
    public partial class CustomListDataViewModel
    {
        public int TableId { get; set; }
        public string CustomListId { get; set; }
        public string CustomListDataId { get; set; }
        public string ListDataName { get; set; }
    }

    public partial class CustomListDataViewModel
    {
        public string CustomListName { get; set; }
        public string ParentNodeId { get; set; }
        public string ParentData { get; set; }
    }
}
