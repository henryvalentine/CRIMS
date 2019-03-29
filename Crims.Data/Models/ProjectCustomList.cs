using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class ProjectCustomList : Entity
    {
        public int TableId { get; set; }
        public string CustomListId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectSiteId { get; set; }
    }
}
