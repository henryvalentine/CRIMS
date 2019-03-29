 
using System.Collections.Generic;
using Crims.Data.Models;

namespace Crims.Data.Models
{
    public partial class ProjectCustomFieldViewModel
    {
        public int TableId { get; set; }
        public string CustomFieldId { get; set; }
        public string CustomFieldName { get; set; } 
        public string ProjectCode { get; set; }
        public int TabIndex { get; set; }
      
        public string CustomFieldSize { get; set; }
        public string CustomListId { get; set; }
        public string CustomGroupId { get; set; }
        public string FieldTypeId { get; set; }
        public int Required { get; set; }

        public string FieldTypeName { get; set; }
        public string GroupName { get; set; }
        public int CustomGroupTabIndex { get; set; }
        
        public string CustomListName { get; set; }
        public string ParentCustomListId { get; set; }
    }
    
    public partial class ProjectCustomFieldSelectable
    {
        public List<CustomField> CustomFields { get; set; }
    }
}

