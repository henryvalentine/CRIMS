using System.Collections.Generic;
using Crims.Data.Models;

namespace Crims.Data.Models
{
    public partial class CustomListViewModel
    {
        public int TableId { get; set; }
        public string CustomListName { get; set; }
        public string CustomListId { get; set; }
        public string ParentListId { get; set; }
        public string ParentListName { get; set; }
        public bool HasChildren { get; set; }
        public string ParentNodeId { get; set; }
        public string CustomFieldId { get; set; }
        public string ParentFieldId { get; set; }
        public string CustomFieldName { get; set; }
        public string CustomFieldSize { get; set; }
        public string CustomGroupId { get; set; }
        public string FieldTypeId { get; set; }
        public int TabIndex { get; set; }
        public int Required { get; set; }

        public List<CustomListData> CustomListDatas { get; set; }
        public List<CustomListData> ParentListData { get; set; }
    }

}

