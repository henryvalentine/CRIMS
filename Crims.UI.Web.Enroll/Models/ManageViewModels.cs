using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Crims.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace Crims.UI.Web.Enroll.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }
    public class EnrollmentBackup
    {
        [JsonProperty("approvals")]
        public Approval[] Approvals { get; set; }
        [JsonProperty("customDatas")]
        public CustomData[] CustomDatas { get; set; }
        [JsonProperty("baseData")]
        public BaseData BaseData { get; set; }
        [JsonProperty("fingerprintImages")]
        public FingerprintImage[] FingerprintImages { get; set; }
        [JsonProperty("fingerprintReasons")]
        public FingerprintReason[] FingerprintReasons { get; set; }
        [JsonProperty("fingerprintTemplate")]
        public FingerprintTemplate FingerprintTemplate { get; set; }
        [JsonProperty("photograph")]
        public Photograph Photograph { get; set; }
        [JsonProperty("signature")]
        public Signature Signature { get; set; }

    }
    public class CurrentSessionModel
    {
        public Project Project { get; set; }
        public string LoginName { get; set; }
    }
    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }

    public class AppSettingModel
    {
        public string Id { get; set; }
        public string BiometricTemplatePath { get; set; }
        public DateTime SynchronisationTime { get; set; }
        public string SynchronisationTimeStr { get; set; }
        public int SynchFrequency { get; set; }

    }

    public class BiometricModel
    {
        public string PhotoPath { get; set; }
        public string LeftLittle { get; set; }
        public string LeftRing { get; set; }
        public string LeftMiddle { get; set; }
        public string LeftIndex { get; set; }
        public string LeftThumb { get; set; }
        public string RightThumb { get; set; }
        public string RightIndex { get; set; }
        public string RightMiddle { get; set; }
        public string RightRing { get; set; }
        public string RightLittle { get; set; }
        public string Signature { get; set; }
    }

    public class BiometricDbModel
    {
        public string Photo { get; set; }
        public string LeftLittle { get; set; }
        public string LeftRing { get; set; }
        public string LeftMiddle { get; set; }
        public string LeftIndex { get; set; }
        public string LeftThumb { get; set; }
        public string RightThumb { get; set; }
        public string RightIndex { get; set; }
        public string RightMiddle { get; set; }
        public string RightRing { get; set; }
        public string RightLittle { get; set; }
        public string Signature { get; set; }
        public List<FingerReason> FingerReasons { get; set; }
    }
    public class FingerReason
    {
        public int FingerIndex { get; set; }
        public string Name { get; set; }
        public string Reason { get; set; }
    }
    public class DashboardModel
    {
        public int TotalCapture { get; set; }
        public int TotalApprovals { get; set; }
        public int CompletedRecords { get; set; }
        public int PhotoEnrolled { get; set; }
        public bool DataRetrieved { get; set; }
        public int FingerprintsEnrolled { get; set; }
        public int Signature { get; set; }
        public int NoPhotos { get; set; }
        public int  NoFingerprint { get; set; }
        public int Synchronized { get; set; }
        public string EnrollmentOfficer { get; set; }
        public string ProjectCode { get; set; }
    }

    public class DashboardQuery
    {
        public int Page { get; set; }
        public int NumOfItemsToFetch { get; set; }
        public string EnrollmentOfficer { get; set; }
        public string ProjectCode { get; set; }
    }

    public class UserModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
        public string Sex { get; set; }
        public string Hash { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
    };

}

