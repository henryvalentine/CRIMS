using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TandAProject.Models
{
    public class AttendanceDetails
    {
       
        public string UserId
        {
            get; set;
        }

        public string Title
        {
            get; set;
        }
        public string ProjectPrimaryCode
        {
            get; set;
        }
        public string Surname
        {
            get; set;
        }

        public string FirstName
        {
            get; set;
        }

        public string MiddleName
        {
            get; set;
        }

        public string Ministry
        {
            get; set;
        }

        public string Date
        {
            get; set;
        }

        public string ClockCount
        {
            get; set;
        }

        public string ClockIn
        {
            get; set;
        }

        public string ClockOut
        {
            get; set;
        }

        public string TimeSpent
        {
            get; set;
        }
    }
}
