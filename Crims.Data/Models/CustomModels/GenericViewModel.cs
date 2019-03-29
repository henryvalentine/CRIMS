using System.Collections.Generic;
using Crims.Data.Models;

namespace Crims.Data.Models
{
    public partial class GenericViewModel
    {
        public int TableId { get; set; }
        public string CustomFieldId { get; set; }
        public string CustomFieldName { get; set; }
        public string CustomFieldSize { get; set; }
        public string ParentFieldId { get; set; }
        public string CustomListId { get; set; }
        public string CustomGroupId { get; set; }
        public string FieldTypeId { get; set; }
        public int TabIndex { get; set; }
        public int Required { get; set; }
        
        public CustomListViewModel CustomList { get; set; }
        public CustomFieldType CustomFieldType { get; set; }
    }

    

    public partial class GenericViewModel
    {
        public string CustomListName { get; set; }
        public string CustomListDataName { get; set; }
        public string CustomGroupName { get; set; }
        public bool HasChildren { get; set; }
        public string ParentListId { get; set; }
        public string FieldTypeName { get; set; }
        public string CustomDataId { get; set; }
        public string EnrollmentId { get; set; }
        public string CrimsCustomData { get; set; }
        public string PrecedingField { get; set; }
    }
    

    public partial class CustomFieldSelectable
    {
        public List<CustomGroup> CustomGroups { get; set; }
        public List<CustomList> CustomLists { get; set; }
        public List<CustomFieldType> CustomFieldTypes { get; set; }
    }

    public class CustomGroupViewModel
    {
        public int TableId { get; set; }
        public string CustomGroupId { get; set; }
        public string GroupName { get; set; }
        public int TabIndex { get; set; }
        public List<GenericViewModel> CustomFieldViewModels { get; set; }
    }

    public class GenericObjectModel
    {
        public string ProjectSiteId { get; set; }
        public int TableId { get; set; }
        public string Name { get; set; }
        public string EnrollmentId { get; set; }
        public string ProjectPrimaryCode { get; set; }
        public string FormPath { get; set; }
        public List<CustomGroupViewModel> CustomGroupViewModels { get; set; }
    }

    public class ParentListModel
    {
        public string ParentListId { get; set; }
        public string ParentNode { get; set; } 
        public string CustomDataId { get; set; }
        public string PrecedingField { get; set; }
    }

    public class UserDataModel
    {
        public string ProjectSiteId { get; set; }
        public int TableId { get; set; }
        public string Firstname { get; set; }
        public string MiddleName { get; set; }
        public string EnrollmentId { get; set; }
        public string ProjectPrimaryCode { get; set; }
        public string FormPath { get; set; }
        public byte[] FormData { get; set; }
        public string ProjectCode { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string CuntryCode { get; set; }
        public string DOB { get; set; }
        public int ApprovalStatus { get; set; }
        public System.DateTime EnrollmentDate { get; set; }
        public string ValidIdNumber { get; set; }
        public List<CustomDataViewModel> CustomDataViewModels { get; set; }
    }
}

