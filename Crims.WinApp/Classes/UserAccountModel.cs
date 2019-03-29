using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crims.UI.Win.Enroll.Classes
{
    public class UserAccountModel
    {
        public string UserName { get; set; }
        public List<string> UserRoles { get; set; }

        public UserAccountModel()
        {
            UserRoles = new List<string>();
        }
        
        [JsonProperty("profileId")]
        public string ProfileId { get; set; }

        [JsonProperty("fullname")]
        public string FullName { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("sex")]
        public string Sex { get; set; }

        [JsonProperty("dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message
        {  get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }
    }
    
}
