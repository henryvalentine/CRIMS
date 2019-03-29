using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Crims.API.Models
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Sex { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
        public AspNetUser AspNetUser { get; set; }
    }

    public class AspNetUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("emailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [JsonProperty("passwordHash")]
        public string PasswordHash { get; set; }

        [JsonProperty("securityStamp")]
        public string SecurityStamp { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("phoneNumberConfirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [JsonProperty("twoFactorEnabled")]
        public bool TwoFactorEnabled { get; set; }

        [JsonProperty("lockoutEndDateUtc")]
        public DateTime LockoutEndDateUtc { get; set; }

        [JsonProperty("lockoutEnabled")]
        public bool LockoutEnabled { get; set; }

        [JsonProperty("accessFailedCount")]
        public int AccessFailedCount { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("userInfo_Id")]
        public string UserInfo_Id { get; set; }

        [JsonProperty("aspNetUserRole")]
        public AspNetUserRole AspNetUserRole { get; set; }
    }

    public class AspNetUserRole
    {
        [JsonProperty("roleId")]
        public string RoleId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }
    }

    public class UserModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
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