using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crims.Core.BusinessObjects.Legacy
{
    public class AttendanceDetailsObject
    {
        private string _UserId;
        private string _Title;
        private string _Surname;
        private string _FirstName;
        private string _MiddleName;
        private string _Date;
        private string _Clock_Count;
        private string _Clock_In;
        private string _Clock_Out;
        private string _Time_Spent;
        private string _Ministry;

        public AttendanceDetailsObject(string title, string surname, string firstname, string date, string clock_count, string clock_in, string clock_out, string time_spent)
        {
            _Title = title;
            _Surname = surname;
            _FirstName = firstname;
            _Date = date;
            _Clock_Count = clock_count;
            _Clock_In = clock_in;
            _Clock_Out = clock_out;
            _Time_Spent = time_spent;
        }

        public AttendanceDetailsObject(string userId, string title, string surname, string firstname,string middlename, string date, string clock_count, string clock_in, string clock_out, string time_spent)
        {
            _UserId = userId;
            _Title = title;
            _Surname = surname;
            _FirstName = firstname;
            _MiddleName = middlename;
            _Date = date;
            _Clock_Count = clock_count;
            _Clock_In = clock_in;
            _Clock_Out = clock_out;
            _Time_Spent = time_spent;
        }

        public AttendanceDetailsObject(string userId, string title, string surname, string firstname, string middlename, string ministry, string date, string clock_count, string clock_in, string clock_out, string time_spent)
        {
            _UserId = userId;
            _Title = title;
            _Surname = surname;
            _FirstName = firstname;
            _MiddleName = middlename;
            _Date = date;
            _Clock_Count = clock_count;
            _Clock_In = clock_in;
            _Clock_Out = clock_out;
            _Time_Spent = time_spent;
            _Ministry = ministry;
        }

        public string UserId
        {
            get { return _UserId; }
        }

        public string Title {
            get { return _Title; }
        }

        public string Surname
        {
            get { return _Surname; }
        }

        public string FirstName
        {
            get { return _FirstName; }
        }

        public string MiddleName
        {
            get { return _MiddleName; }
        }

        public string Ministry
        {
            get { return _Ministry; }
        }

        public string Date
        {
            get { return _Date; }
        }

        public string ClockCount
        {
            get { return _Clock_Count; }
        }

        public string ClockIn
        {
            get { return _Clock_In; }
        }

        public string ClockOut
        {
            get { return _Clock_Out; }
        }

        public string TimeSpent
        {
            get { return _Time_Spent; }
        }
    }
}
