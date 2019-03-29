 
using Crims.Data.Models;

namespace Crims.Data.Models
{
    public partial class ProjectLicense
    {
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectCode { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string LicenceCode { get; set; }
        public string ActivationCode { get; set; }
        public int OnlineMode { get; set; }
        public System.DateTime LicenseExpiryDate { get; set; }
		
        public CustomGroup[] CustomGroups { get; set; }
        public CustomField[] CustomFields { get; set; }  
        public CustomList[] CustomLists { get; set; }
        public CustomListData[] CustomListData { get; set; }
        public ProjectCustomGroup[] ProjectCustomGroups { get; set; }
        public ProjectCustomField[] ProjectCustomFields { get; set; }
        public CustomFieldType[] CustomFieldTypes { get; set; }
    }
    
}



