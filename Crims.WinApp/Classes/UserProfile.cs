using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crims.UI.Win.Enroll.Classes
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string   PhoneNumber { get; set; }
        public string Sex { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
    }
}
