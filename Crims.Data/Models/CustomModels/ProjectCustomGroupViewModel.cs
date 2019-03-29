 
using System.Collections.Generic;
using Crims.Data.Models;

namespace Crims.Data.Models
{
    public partial class ProjectCustomGroupViewModel
    {
        public int TableId { get; set; }
        public string ProjectCode { get; set; }
        public string CustomGroupId { get; set; }
        public int TabIndex { get; set; }
        public string CustomGroupName { get; set; } 
        public List<CustomGroup> CustomGroups { get; set; }
    }

    public class ProjectCustomGroupGen
    {
        public List<ProjectCustomGroupViewModel> ProjectCustomGroupViewModels { get; set; }
        public List<CustomGroup> CustomGroups { get; set; }
    }
}

