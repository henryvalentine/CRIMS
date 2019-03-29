using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crims.Core.Models.Legacy
{
    public class UserRecord
    {
        public int Id { get; set; }
        public string ProjectPrimaryCode { get; set; }
        public string Title { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }
        public string Ministry { get; set; }
        public string WebAccessPin { get; set; }
        public Image Photo { get; set; }
    }
}
