using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class CustomField : Entity
    {
        public int TableId { get; set; }
        public string CustomFieldId { get; set; }
        public string CustomFieldName { get; set; }
        public string CustomFieldSize { get; set; }
        public string CustomListId { get; set; }
        public string CustomGroupId { get; set; }
        public string ParentFieldId { get; set; }
        public string FieldTypeId { get; set; }
        public int TabIndex { get; set; }
        public int Required { get; set; }
        
        //[ForeignKey("CustomListId")]
        //public CustomList CustomList { get; set; }
        //[ForeignKey("CustomGroupId")]
        //public CustomGroup CustomGroup { get; set; }
        //[ForeignKey("FieldTypeId")]
        //public CustomFieldType CustomFieldType { get; set; }
        //public ICollection<ProjectCustomField> ProjectCustomFields { get; set; }
    }
}
