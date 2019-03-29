using System.Collections.Generic;

namespace Crims.Data.Models
{
    public partial class CustomDataViewModel
    {
        public string ParentListId { get; set; }
        public bool HasChildren { get; set; }
        public int TableId { get; set; }
        public string CustomDataId { get; set; }
        public string CustomFieldId { get; set; }
        public string EnrollmentId { get; set; }
        public string CrimsCustomData { get; set; }
        public string CustomListId { get; set; }
        public string ProjectSIteId { get; set; }

        public List<CustomListData> CustomListDatas { get; set; }
    }

}

