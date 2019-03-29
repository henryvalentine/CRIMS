﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crims
{
    public class AppSettings
    {
        public static string DebugLogPath = ConfigurationManager.AppSettings["DebugLoggingPath"];
        public static string ErrorLogPath = ConfigurationManager.AppSettings["ErrorLoggingPath"];

        //MySqlDatabase Settings
        string _DBSource { get; set; }
        string _DBServer { get; set; }
        string _DBName { get; set; }
        string _DBUser { get; set; }
        string _DBPort { get; set; }
        string _DBPassword { get; set; }
        string _DBConnectionString { get; set; }
        bool _PreloadUserData { get; set; }
        string _VerificationMode { get; set; }
        string _SetupPassword { get; set; }
        string _ScannerDevice { get; set; }
        string _AppMode { get; set; }
        string _MatchingThreshold { get; set; }
        int _MessageDuration { get; set; }

        string _Success_Message { get; set; }
        string _Failed_Message { get; set; }

        //Primary Data Settings
        string PrimaryDataTable { get; set; }
        string PrimaryKeyField { get; set; }

        string _ResumptionTime { get; set; }
        string _ResumptionGraceTime { get; set; }
        string _ClosingTime { get; set; }
        string _ClosingGraceTime { get; set; }

        public string DBSource { get { return _DBSource; } set { _DBSource = value; } }
        public string DBServer { get { return _DBServer; } set { _DBServer = value; } }
        public string DBName { get { return _DBName; } set { _DBName = value; } }
        public string DBUser { get { return _DBUser; } set { _DBUser = value; } }
        public string DBPort { get { return _DBPort; } set { _DBPort = value; } }
        public string DBPassword { get { return _DBPassword; } set { _DBPassword = value; } }
        public string DBConnectionString { get { return _DBConnectionString; } set { _DBConnectionString = value; } }
        public bool PreloadUserData { get { return _PreloadUserData; } set { _PreloadUserData = value; } }
        public string VerificationMode { get { return _VerificationMode; } set { _VerificationMode = value; } }
        public string SetupPassword { get { return _SetupPassword; } set { _SetupPassword = value; } }

        public string ScannerDevice { get { return _ScannerDevice; } set { _ScannerDevice = value; } }
        public string AppMode { get { return _AppMode; } set { _AppMode = value; } }
        public string MatchingThreshold { get { return _MatchingThreshold; } set { _MatchingThreshold = value; } }
        public int MessageDuration { get { return _MessageDuration; } set { _MessageDuration = value; } }
    
        public string Success_Message { get { return _Success_Message; }  set { _Success_Message = value; } }
        public string Failed_Message { get { return _Failed_Message; } set { _Failed_Message = value; } }

        public string ResumptionTime { get { return _ResumptionTime; } set { _ResumptionTime = value; } }
        public string ResumptionGraceTime { get { return _ResumptionGraceTime; } set { _ResumptionGraceTime = value; } }
        public string ClosingTime { get { return _ClosingTime; } set { _ClosingTime = value; } }
        public string ClosingGraceTime { get { return _ClosingGraceTime; } set { _ClosingGraceTime = value; } }

        public AppSettings()
        {
            _DBSource = "";
            _DBServer = "";
            _DBName = "";
            _DBPort = "";
            _DBUser = "";
            _DBPassword = "";
            _DBConnectionString = "";
        }
    }
}
