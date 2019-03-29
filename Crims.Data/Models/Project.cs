using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class Project : Entity
    {
        public int TableId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        [Index("IX_project_projectcode", IsUnique = true)]
        public string ProjectCode { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string LicenceCode { get; set; }
        public string ActivationCode { get; set; }
        public int OnlineMode { get; set; }
        public System.DateTime LicenseExpiryDate { get; set; }
    }
}

