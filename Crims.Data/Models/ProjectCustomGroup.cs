using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class ProjectCustomGroup : Entity
    {
        public int TableId { get; set; }
        public string ProjectCode { get; set; }
        public string CustomGroupId { get; set; }
        public int TabIndex { get; set; }
        public string ProjectSiteId { get; set; }
    }
}
