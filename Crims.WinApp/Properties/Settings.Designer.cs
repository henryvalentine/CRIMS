﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Crims.UI.Win.Enroll.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("localhost")]
        public string DBServer {
            get {
                return ((string)(this["DBServer"]));
            }
            set {
                this["DBServer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3306")]
        public string DBPort {
            get {
                return ((string)(this["DBPort"]));
            }
            set {
                this["DBPort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("crims_site")]
        public string DBName {
            get {
                return ((string)(this["DBName"]));
            }
            set {
                this["DBName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("crims")]
        public string DBUser {
            get {
                return ((string)(this["DBUser"]));
            }
            set {
                this["DBUser"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("password")]
        public string DBPassword {
            get {
                return ((string)(this["DBPassword"]));
            }
            set {
                this["DBPassword"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\inetpub\\wwwroot\\crimsWeb\\UserRecords")]
        public string SavedFilesDir {
            get {
                return ((string)(this["SavedFilesDir"]));
            }
            set {
                this["SavedFilesDir"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Futronic Single Finger")]
        public string FPScannerType {
            get {
                return ((string)(this["FPScannerType"]));
            }
            set {
                this["FPScannerType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("154.113.0.163")]
        public string syncDBServer {
            get {
                return ((string)(this["syncDBServer"]));
            }
            set {
                this["syncDBServer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3306")]
        public string syncDBPort {
            get {
                return ((string)(this["syncDBPort"]));
            }
            set {
                this["syncDBPort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("crims_site")]
        public string syncDBName {
            get {
                return ((string)(this["syncDBName"]));
            }
            set {
                this["syncDBName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("crims")]
        public string syncDBUser {
            get {
                return ((string)(this["syncDBUser"]));
            }
            set {
                this["syncDBUser"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("password")]
        public string syncDBPassword {
            get {
                return ((string)(this["syncDBPassword"]));
            }
            set {
                this["syncDBPassword"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\crims_site_server\\\\Crims\\\\")]
        public string syncServerFilePath {
            get {
                return ((string)(this["syncServerFilePath"]));
            }
            set {
                this["syncServerFilePath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost/CrimsApi/api/Account/")]
        public string LoginServiceUrl {
            get {
                return ((string)(this["LoginServiceUrl"]));
            }
            set {
                this["LoginServiceUrl"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AuthoriseUser {
            get {
                return ((bool)(this["AuthoriseUser"]));
            }
            set {
                this["AuthoriseUser"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AuthoriseOnlyUserName {
            get {
                return ((bool)(this["AuthoriseOnlyUserName"]));
            }
            set {
                this["AuthoriseOnlyUserName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("80")]
        public decimal FQualityThreshold {
            get {
                return ((decimal)(this["FQualityThreshold"]));
            }
            set {
                this["FQualityThreshold"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SaveBiometricsToFileOption {
            get {
                return ((bool)(this["SaveBiometricsToFileOption"]));
            }
            set {
                this["SaveBiometricsToFileOption"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\crims_site_server\\\\Crims\\\\")]
        public string syncServerMappedPath {
            get {
                return ((string)(this["syncServerMappedPath"]));
            }
            set {
                this["syncServerMappedPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SaveBiometricsToDBOption {
            get {
                return ((bool)(this["SaveBiometricsToDBOption"]));
            }
            set {
                this["SaveBiometricsToDBOption"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Yes")]
        public string UseFaceLift {
            get {
                return ((string)(this["UseFaceLift"]));
            }
            set {
                this["UseFaceLift"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("154")]
        public string FaceListLicense {
            get {
                return ((string)(this["FaceListLicense"]));
            }
            set {
                this["FaceListLicense"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost/crimsWeb")]
        public string WebEnrollUrl {
            get {
                return ((string)(this["WebEnrollUrl"]));
            }
            set {
                this["WebEnrollUrl"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://41.58.134.153/crimsApi/api/ApiHub/EnrolleeDataForms")]
        public string dataFormsDestination {
            get {
                return ((string)(this["dataFormsDestination"]));
            }
            set {
                this["dataFormsDestination"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("95")]
        public decimal MatchingScore {
            get {
                return ((decimal)(this["MatchingScore"]));
            }
            set {
                this["MatchingScore"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("154")]
        public string ImageWidth {
            get {
                return ((string)(this["ImageWidth"]));
            }
            set {
                this["ImageWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("180")]
        public string ImageHeight {
            get {
                return ((string)(this["ImageHeight"]));
            }
            set {
                this["ImageHeight"] = value;
            }
        }
    }
}
