using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class CustomFieldType : Entity
    {
        public int TableId { get; set; }
        public string FieldTypeId { get; set; }
        public string FieldTypeName { get; set; }

        //public ICollection<CustomField> CustomFields { get; set; }
    }
}
