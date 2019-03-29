using System;

namespace Crims.UI.Win.Enroll.Classes
{
    public class AspNetUsers
    {
        public int AccessFailedCount { get; set; }
        public string Email { get; set; }
        public Boolean EmailConfirmed { get; set; }
        public string Id { get; set; }
        public Boolean LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public Boolean PhoneNumberConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public Boolean TwoFactorEnabled { get; set; }
        public string UserInfo_Id { get; set; }
        public string UserName { get; set; }

    }

    public class AspNetUserRoles
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}