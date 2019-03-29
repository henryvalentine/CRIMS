using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class CustomGroup : Entity
    {
        public int TableId { get; set; }
        public string CustomGroupId { get; set; }
        public string GroupName { get; set; }
        public int TabIndex { get; set; }

        //public ICollection<CustomField> CustomFields { get; set; }
    }
}

