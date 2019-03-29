using System.Collections.Generic;
using Crims.Data.Models;

namespace Crims.Data.Models
{
    public partial class ProjectViewModel
    {
        public int TableId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectCode { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string LicenceCode { get; set; }
        public string UserRole { get; set; }
        public string ActivationCode { get; set; }
        public int OnlineMode { get; set; }
        public System.DateTime LicenseExpiryDate { get; set; }
    }

    public partial class ProjectViewModel
    {
        public List<ProjectCustomFieldViewModel> ProjectCustomFieldViewModels { get; set; }
        public List<ProjectCustomListViewModel> ProjectCustomListViewModels { get; set; }
        public List<ProjectCustomListDataViewModel> ProjectCustomListDataViewModels { get; set; }
        public List<CustomList> CustomLists { get; set; }
        public List<CustomField> CustomFields { get; set; }
        public List<CustomListData> CustomListDatas { get; set; }
    }

    public class ProjectCustomSelectibles
    {
        public List<CustomList> CustomLists { get; set; }
        public List<CustomField> CustomFields { get; set; }
        public List<CustomGroup> CustomGroups { get; set; }
        public List<CustomListData> CustomListDatas { get; set; }
    }

    public class ProjectSelectable
    {
        public List<CustomList> CustomLists { get; set; }
        public List<CustomField> CustomFields { get; set; }
        public List<CustomListData> CustomListDatas { get; set; }
    }
    
  

}


