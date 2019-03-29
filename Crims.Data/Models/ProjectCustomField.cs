using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class ProjectCustomField : Entity
    {
        public int TableId { get; set; }
        public string CustomFieldId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectSiteId { get; set; }
    }
}
