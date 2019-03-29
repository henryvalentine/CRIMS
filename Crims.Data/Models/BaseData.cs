using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class BaseData : Entity
    {
        [Index("IX_basedata_projectcode")]
        public string ProjectCode { get; set; }
        public string ProjectSiteId { get; set; }
        [Index("IX_enrollment_basedata", IsUnique = true)]
        public string EnrollmentId { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string CuntryCode { get; set; }
        public string ProjectPrimaryCode { get; set; }
        public string DOB { get; set; }
        public int ApprovalStatus { get; set; }
        public System.DateTime EnrollmentDate { get; set; }
        public string ValidIdNumber { get; set; }
        public string FormPath { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedby { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
        public virtual ICollection<CustomData> CustomDatas { get; set; }
        public virtual ICollection<Approval> Approvals { get; set; }
        public virtual ICollection<FingerprintImage> FingerprintImages { get; set; }
        public virtual ICollection<FingerprintTemplate> FingerprintTemplates { get; set; }
        public virtual ICollection<FingerprintReason> FingerprintReasons { get; set; }
        public virtual ICollection<Photograph> Photographs { get; set; }
        public virtual ICollection<Signature> Signatures { get; set; }
        public virtual ICollection<SyncJobHistory> SyncJobHistories { get; set; }
    }
}



