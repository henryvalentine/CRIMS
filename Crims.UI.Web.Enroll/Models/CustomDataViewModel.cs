namespace Crims.UI.Web.Enroll.Models
{
    public class CustomDataViewModel
    {
        public int TableId { get; set; }
        public string CustomDataId { get; set; }
        public string CustomFieldId { get; set; }
        public string ParentFieldId { get; set; }
        public string EnrollmentId { get; set; }
        public string CrimsCustomData { get; set; }
        public string ChildCrimsCustomData { get; set; }
        public string CustomListId { get; set; }
        public string ParentListId { get; set; }
        public bool HasChildren { get; set; }
        public string ProjectSIteId { get; set; }
        public bool IsRequired { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
    }
}

