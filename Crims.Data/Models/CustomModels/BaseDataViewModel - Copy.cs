using System;
using System.Collections.Generic;
using Crims.Data.Models;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public class BaseDataViewModel
    {
        public int TableId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectSiteId { get; set; }
        public string TestColumn { get; set; }
        public string EnrollmentId { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string MiddleName { get; set; }
        public string EnrollmentOfficer { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string FormPath { get; set; }
        public string CuntryCode { get; set; }
        public string ProjectPrimaryCode { get; set; }
        public string LastUpdatedby { get; set; }
        public string DOB { get; set; }
        public int ApprovalStatus { get; set; }
        public string EnrollmentDateStr { get; set; }
        public string BiometricStatus { get; set; }
        public System.DateTime EnrollmentDate { get; set; }
        public string ValidIdNumber { get; set; }
        public string ApprovalStatusStr { get; set; }
        public List<ApprovalViewModel> ApprovalHistories { get; set; }
    }

}

