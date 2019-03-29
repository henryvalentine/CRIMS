using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using TandAProject.Database;
using TandAProject.Models;
using TandAProject.Utils;
using System.Drawing;
using System.IO;
using TandAProject.BusinessObjects;
using System.Linq;
using Crims.UI.Win.TimeAndAttendance.Properties;

namespace TandAProject.Services
{
    public class DataServices
    {
        #region Sync

        static MySqlConnection myConn = new ConnectionManager().DbConnection_v2();

        internal static int GetAttendanceClockPendingSyncSize()
        {
            DataTable result = new DataTable();
            MySqlDataAdapter Adapter = new MySqlDataAdapter("SELECT Count(*) FROM eid_attendance_log Where SyncStatus=0", myConn);
            Adapter.Fill(result);

            if (result.Rows.Count > 0)
            {
                return Convert.ToInt32(result.Rows[0][0]);
            }
            else { return 0; }
        }

        internal static List<AttendanceLog> GetAttendanceClockPendingSync(int batchSize, int startPoint, bool limit = true)
        {
            DataTable result = new DataTable();
            string query = string.Format("SELECT * FROM eid_attendance_log Where SyncStatus=0 Limit {0}, {1}", startPoint, batchSize);

            MySqlDataAdapter Adapter = new MySqlDataAdapter(query, myConn);
            Adapter.Fill(result);

            List<AttendanceLog> attendanceLogs = new List<AttendanceLog>();
            foreach (DataRow row in result.Rows)
            {

                AttendanceLog obj = new AttendanceLog
                {
                    Id = (Int32)row["Id"],
                    BaseDataId = Convert.ToString(row["BaseDataId"]),
                    ClockDate = Convert.ToString(row["ClockDate"]),
                    ClockStatus = Convert.ToInt32(row["ClockStatus"]),
                    ClockTime = Convert.ToString(row["ClockTime"]),
                    LastUpdated = Convert.ToString(row["LastUpdated"]),
                    MatchingScore = Convert.ToInt32(row["MatchingScore"]),
                    TempleteId = Convert.ToInt32(row["TempleteId"]),
                    TerminalId = Convert.ToString(row["TerminalId"]),
                    TransactionCode = Convert.ToString(row["TransactionCode"]),
                    TransactionDateTime = Convert.ToString(row["TransactionDateTime"]),
                    UserPrimaryCode = Convert.ToString(row["UserPrimaryCode"])
                };

                attendanceLogs.Add(obj);
            }
            return attendanceLogs;
        }

        internal static List<KeyValuePair<int, string>> GetProjectCustomFields()
        {
            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>();
            DataTable dataTable = new DataTable();
            MySqlDataAdapter Adapter = new MySqlDataAdapter("SELECT CUSTOMFIELDID, CUSTOMFIELDNAME FROM custom_fields", myConn);
            Adapter.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                result.Add(new KeyValuePair<int, string>(Convert.ToInt32(row[0]), row[1].ToString()));
            }
            return result;
        }

        internal static int LogAttendanceClock(AttendanceLog log, MySqlConnection dbCon)
        {
            int result = 0;
            //First check if transaction Exists on the Server
            //If Record Alerady Exist, Returm success
            MySqlDataAdapter Adapter = new MySqlDataAdapter("SELECT * from eid_attendance_log WHERE TransactionCode='" + log.TransactionCode + "'"
                , dbCon);
            int res = Adapter.Fill(new DataTable());
            if (res > 0)
            {
                return res;
            }


            string query = string.Format(
             "INSERT INTO eid_attendance_log " +
             " (TransactionCode," +
             " TerminalId," +
             " BaseDataId," +
             " UserPrimaryCode," +
             " ClockDate," +
             " ClockTime," +
             " TransactionDateTime," +
             " TempleteId," +
             " MatchingScore," +
             " ClockStatus)" +
             " VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
             log.TransactionCode,
             log.TerminalId,
             log.BaseDataId,
             log.UserPrimaryCode,
             Convert.ToDateTime(log.ClockDate).ToString("yyyy-MM-dd"),
             log.ClockTime,
             Convert.ToDateTime(log.TransactionDateTime).ToString("yyyy-MM-dd HH:mm:ss"),
             log.TerminalId,
             log.MatchingScore,
             log.ClockStatus
             );

            using (MySqlCommand command = new MySqlCommand(query, dbCon))
            {
                result = command.ExecuteNonQuery();
            }
            return result;
        }

        internal static void UpdateAttendanceClockSyncStatus(string[] logIds, string status)
        {
            string Ids = String.Empty;

            if (logIds.Length > 0)
            {
                foreach (string s in logIds)
                {
                    Ids += s + ",";
                }

                string query = string.Format("UPDATE eid_attendance_log SET SyncStatus =1 WHERE ID in ({0}) ", Ids.TrimEnd(new char[] { ',' }));

                using (MySqlCommand command = new MySqlCommand(query, myConn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        internal static bool TestDBConnection(string server, string db, string port, string user, string password)
        {
            try
            {
                var conn = new ConnectionManager(server, db, port, user, password).getDBConnection();
                return conn.State == ConnectionState.Open ? true : false;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        internal static List<UserRecord> GetAllUserRecords()
        {
            DataTable result = new DataTable();
            MySqlDataAdapter Adapter;

            string query =
            " SELECT distinct " +
            " eid_base_data.BASEDATAID," +
            " eid_base_data.PROJECTPRIMARYCODE," +
            " eid_base_data.TITLEPREFIX," +
            " eid_base_data.SURNAME," +
            " eid_base_data.FIRSTNAME," +
            " eid_base_data.MIDDLENAME," +
            " eid_base_data.MOBILENUMBER," +
            " eid_photographs.PHOTOGRAPH" +
            " FROM" +
            " eid_base_data" +
            " LEFT JOIN eid_photographs ON eid_base_data.PROJECTPRIMARYCODE = eid_photographs.PROJECTPRIMARYCODE ";


            MySqlCommand command = new MySqlCommand();
            command.Connection = myConn;
            command.CommandText = query;

            Adapter = new MySqlDataAdapter(command);
            MySqlCommandBuilder cb = new MySqlCommandBuilder(Adapter);

            Adapter.Fill(result);

            if (result.Rows.Count > 0)
            {

                List<UserRecord> res = new List<UserRecord>();
                foreach (DataRow row in result.Rows)
                {
                    UserRecord record = new UserRecord();

                    record.Id = Convert.ToInt32(row[0]);
                    record.ProjectPrimaryCode = row[1].ToString();
                    record.Title = row[2].ToString();
                    record.Surname = row[3].ToString();
                    record.FirstName = row[4].ToString();
                    record.MiddleName = row[5].ToString();
                    record.PhoneNumber = row[6].ToString();
                    record.CustomData1 = row[7].ToString();
                    record.Photo = FetchPhotoFromResultSet(row[8]);
                    res.Add(record);
                }
                return res;
            }

            else { return new List<UserRecord> { }; }

        }

        internal static UserRecord FindSampleUserRecord()
        {
            DataTable result = new DataTable();
            MySqlDataAdapter Adapter;

            string query =
            " SELECT DISTINCT " +
            " eid_base_data.BASEDATAID," +
            " eid_base_data.PROJECTPRIMARYCODE," +
            " eid_base_data.TITLEPREFIX," +
            " eid_base_data.SURNAME," +
            " eid_base_data.FIRSTNAME," +
            " eid_base_data.MIDDLENAME," +
            " eid_base_data.MOBILENUMBER," +
            " eid_photographs.PHOTOGRAPH" +
            " FROM" +
            " eid_base_data" +
            " LEFT JOIN eid_photographs on eid_base_data.BASEDATAID = eid_photographs.BASEDATAID LIMIT 1";


            MySqlCommand command = new MySqlCommand();
            command.Connection = myConn;
            command.CommandText = query;

            Adapter = new MySqlDataAdapter(command);
            MySqlCommandBuilder cb = new MySqlCommandBuilder(Adapter);

            Adapter.Fill(result);

            if (result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];
                UserRecord res = new UserRecord();

                res.Id = Convert.ToInt32(row[0]);
                res.ProjectPrimaryCode = row[1].ToString();
                res.Title = row[2].ToString();
                res.Surname = row[3].ToString();
                res.FirstName = row[4].ToString();
                res.MiddleName = row[5].ToString();
                res.PhoneNumber = row[6].ToString();
                res.Photo = FetchPhotoFromResultSet(row[7]);

                //Load User Custom Data
                AddCustomDataToUserRecords(res, out res);

                return res;
            }

            else { return null; }
        }

        internal static DataTable LoadAttendanceClock(ExportParams exporrParams)
        {
            DataTable result = new DataTable();
            string query =
                "SELECT Id, TransactionCode, TerminalId, BaseDataId, UserPrimaryCode, " +
                "ClockDate, ClockTime, TransactionDateTime, TempleteId, MatchingScore, LastUpdated, ClockStatus " +
                string.Format(" FROM eid_attendance_log {0}", exporrParams.AllData ? "" :
                string.Format(" WHERE ClockDate>= '{0}' AND ClockDate <= '{1}'", exporrParams.StartDate, exporrParams.EndDate));

            MySqlDataAdapter Adapter = new MySqlDataAdapter(query, myConn);
            Adapter.Fill(result);

            return result;
        }

        internal static UserRecord FindUserRecord(string projectPrimaryCode)
        {
            UserRecord res = new UserRecord();

            //Load Base Data
            DataTable BaseDataResult = new DataTable();
            string query =
            String.Format(
                " SELECT DISTINCT BASEDATAID, PROJECTPRIMARYCODE, TITLEPREFIX, SURNAME, FIRSTNAME, MIDDLENAME, MOBILENUMBER, WEBACCESSPIN" +
                " FROM eid_base_data WHERE PROJECTPRIMARYCODE='{0}'", projectPrimaryCode);

            MySqlDataAdapter Adapter = new MySqlDataAdapter(query, myConn);
            Adapter.Fill(BaseDataResult);
            if (BaseDataResult.Rows.Count > 0)
            {
                DataRow row = BaseDataResult.Rows[0];
                res.Id = Convert.ToInt32(row[0]);
                res.ProjectPrimaryCode = row[1].ToString();
                res.Title = row[2].ToString();
                res.Surname = row[3].ToString();
                res.FirstName = row[4].ToString();
                res.MiddleName = row[5].ToString();
                res.PhoneNumber = row[6].ToString();
                res.WebAccessPin = row[7].ToString();
            }

            //Load Photograph
            DataTable PhotoResult = new DataTable();
            string PhotoQuery = String.Format("SELECT DISTINCT PHOTOGRAPH FROM eid_photographs WHERE PROJECTPRIMARYCODE='{0}'", projectPrimaryCode);
            MySqlDataAdapter PhotoAdapter = new MySqlDataAdapter(PhotoQuery, myConn);
            PhotoAdapter.Fill(PhotoResult);
            if (PhotoResult.Rows.Count > 0)
            {
                DataRow row = PhotoResult.Rows[0];
                res.Photo = FetchPhotoFromResultSet(row[0]);
            }

            //Load User Custom Data
            AddCustomDataToUserRecords(res, out res);

            return res;
        }

        private static void AddCustomDataToUserRecords(UserRecord res1, out UserRecord res2)
        {
            res2 = res1;
            res2.CustomData1 = GetUserCustomData(Settings.Default.CDataField1, res2.Id);
            res2.CustomData2 = GetUserCustomData(Settings.Default.CDataField2, res2.Id);
            res2.CustomData3 = GetUserCustomData(Settings.Default.CDataField3, res2.Id);
            res2.CustomData4 = GetUserCustomData(Settings.Default.CDataField4, res2.Id);


        }

        private static string GetUserCustomData(string cDataFieldId, int Id)
        {
            DataTable CustomDataResult = new DataTable();
            string CustomDataQuery = String.Format("SELECT DISTINCT CUSTOMFIELDDATA FROM custom_data WHERE CUSTOMFIELDID='{0}' AND BASEDATAID='{1}' LIMIT 1", cDataFieldId, Id);
            MySqlDataAdapter CustomDataAdapter = new MySqlDataAdapter(CustomDataQuery, myConn);
            CustomDataAdapter.Fill(CustomDataResult);
            if (CustomDataResult.Rows.Count > 0)
            {
                DataRow row = CustomDataResult.Rows[0];
                return row[0].ToString();
            }
            else { return string.Empty; }
        }

        private static string FindUserMinistry(string projectPrimaryCode, string MinistryCustomFieldId)
        {
            DataTable CustomDataResult = new DataTable();
            string CustomDataQuery = String.Format("SELECT DISTINCT CUSTOMFIELDDATA FROM custom_data WHERE CUSTOMFIELDID='{0}' AND PROJECTPRIMARYCODE='{1}' LIMIT 1", MinistryCustomFieldId, projectPrimaryCode);
            MySqlDataAdapter CustomDataAdapter = new MySqlDataAdapter(CustomDataQuery, myConn);
            CustomDataAdapter.Fill(CustomDataResult);
            if (CustomDataResult.Rows.Count > 0)
            {
                DataRow row = CustomDataResult.Rows[0];
                return row[0].ToString();
            }
            else { return string.Empty; }
        }

        public static int CountAllDBTemplates()
        {
            DataTable result = new DataTable();
            MySqlDataAdapter Adapter;

            string query =
            " SELECT COUNT(*) from eid_fingerprinttemplates";

            MySqlCommand command = new MySqlCommand();
            command.Connection = myConn;
            command.CommandText = query;

            Adapter = new MySqlDataAdapter(command);
            MySqlCommandBuilder cb = new MySqlCommandBuilder(Adapter);

            Adapter.Fill(result);
            if (result.Rows.Count > 0)
            {
                return Convert.ToInt32(result.Rows[0][0]);
            }
            else { return 0; }
        }


        internal static DataTable GetAllDBTemplates(int startingPoint, int limit, out string Query)
        {
            using (MySqlConnection con = new ConnectionManager().DbConnection_v2())
            {
                DataTable result = new DataTable();
                MySqlDataAdapter Adapter;

                string query =
                " SELECT " +
                 " eid_fingerprinttemplates.BASEDATAID, " +
                " eid_fingerprinttemplates.PROJECTPRIMARYCODE, " +
                 " eid_fingerprinttemplates.FINGERPRINTTEMPLATEID, " +
                " eid_fingerprinttemplates.FINGERPRINTTEMPLATE " +
                " from eid_fingerprinttemplates " +
                String.Format("LIMIT {0}, {1}", startingPoint, limit);

                //Log the Query For Debuging...
                Query = query;

                MySqlCommand command = new MySqlCommand();
                command.Connection = con;
                command.CommandText = query;

                Adapter = new MySqlDataAdapter(command);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(Adapter);

                Adapter.Fill(result);

                return result;
            }
        }

        internal static Image FetchPhotoFromResultSet(object row)
        {
            try
            {
                if (row != null)
                {
                    using (MemoryStream bitmapStreem = new MemoryStream((byte[])row))
                    {
                        return new Bitmap(bitmapStreem);
                    }
                }
                else { return null; }
            }
            catch (Exception exp)
            {
                Logger.logToFile(exp, AppSettings.ErrorLogPath);
                return null;
            }
        }

        internal static byte[] FetchTemplateFromResultSet(Object row)
        {
            //Trace Buggy Templates and Prevent Crashing of loop.
            try
            {
                return (byte[])row;
            }
            catch (Exception exp)
            {
                Logger.logToFile(exp, AppSettings.ErrorLogPath);
                return null;
            }
        }

        internal static void LogAttendanceClock(MatchingResult matchedResult)
        {
            string transactionCode = matchedResult.BaseDataId + matchedResult.TemplateId + matchedResult.UserPrimaryCode +
                DateTime.Now.ToString("yyyyMMddHHmmsss");
            string query = string.Format(
                 "INSERT INTO eid_attendance_log " + 
                 " (TransactionCode," +
                 " TerminalId," +
                 " BaseDataId," +
                 " UserPrimaryCode," +
                 " ClockDate," +
                 " ClockTime," +
                 " TransactionDateTime," +
                 " TempleteId," +
                 " MatchingScore)" +
                 " VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                 transactionCode,
                 "0",
                 matchedResult.BaseDataId,
                 matchedResult.UserPrimaryCode,
                 DateTime.Now.ToString("yyyy-MM-dd"),
                 DateTime.Now.ToString("HH:mm:ss"),
                 DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                 matchedResult.TemplateId,
                 matchedResult.Score
                 );

            try
            {
                using (MySqlCommand command = new MySqlCommand(query, myConn))
                {                    
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, AppSettings.ErrorLogPath);
            }
        }

        internal static int LogAttendanceClock(AttendanceLog log)
        {
            int result = 0;
            try
            {
                string query = string.Format(
                 "INSERT INTO eid_attendance_log " +
                 " (TransactionCode," +
                 " TerminalId," +
                 " BaseDataId," +
                 " UserPrimaryCode," +
                 " ClockDate," +
                 " ClockTime," +
                 " TransactionDateTime," +
                 " TempleteId," +
                 " MatchingScore)" +
                 " VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                 log.TransactionCode,
                 log.TerminalId,
                 log.BaseDataId,
                 log.UserPrimaryCode,
                 Convert.ToDateTime(log.ClockDate).ToString("yyyy-MM-dd"),
                 log.ClockTime,
                 Convert.ToDateTime(log.TransactionDateTime).ToString("yyyy-MM-dd HH:mm:ss"),
                 log.TerminalId,
                 log.MatchingScore
                 );
                using (MySqlCommand command = new MySqlCommand(query, myConn))
                {                    
                    result = command.ExecuteNonQuery();
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, AppSettings.ErrorLogPath);
                return result;
            }
        }

        internal static DataTable GetAttendanceReport()
        {
            DataTable result = new DataTable();
            MySqlDataAdapter Adapter;

            string query = " SELECT " +
            
            " eid_base_data.TITLEPREFIX as 'Title'," +
            " eid_base_data.SURNAME as 'Surname'," +
            " eid_base_data.FIRSTNAME as 'First_Name'," +
            " eid_attendance_log.ClockDate as 'Date'," +
            " COUNT(eid_attendance_log.ClockTime) as 'Clock_Count'," +
            " MIN(eid_attendance_log.ClockTime) as 'Clock_In'," +
            " MAX(eid_attendance_log.ClockTime) as 'Clock_Out'," +
            " TIMEDIFF(MAX(eid_attendance_log.TransactionDateTime), MIN(eid_attendance_log.TransactionDateTime)) as 'Time_Spent'" +
            " FROM" +
            " eid_attendance_log" +
            " LEFT JOIN eid_base_data ON eid_attendance_log.userPrimaryCode = eid_base_data.PROJECTPRIMARYCODE" +
            " GROUP BY" +
            " eid_attendance_log.userPrimaryCode," +
            " eid_attendance_log.ClockDate";

            MySqlCommand command = new MySqlCommand();
            command.Connection = myConn;
            command.CommandText = query;

            Adapter = new MySqlDataAdapter(command);
            MySqlCommandBuilder cb = new MySqlCommandBuilder(Adapter);

            Adapter.Fill(result);

            return result;
        }

        internal static DataTable GetAttendanceReport(string startDate, string endDate)
        {
            DataTable result = new DataTable();

            string query = " SELECT " +
            " eid_base_data.TITLEPREFIX as 'Title'," +
            " eid_base_data.SURNAME as 'Surname'," +
            " eid_base_data.FIRSTNAME as 'First_Name'," +
            " eid_attendance_log.ClockDate as 'Date'," +
            " COUNT(eid_attendance_log.ClockTime) as 'Clock_Count'," +
            " MIN(eid_attendance_log.ClockTime) as 'Clock_In'," +
            " MAX(eid_attendance_log.ClockTime) as 'Clock_Out'," +
            " TIMEDIFF(MAX(eid_attendance_log.TransactionDateTime), MIN(eid_attendance_log.TransactionDateTime)) as 'Time_Spent'" +
            " FROM" +
            " eid_attendance_log" +
            " LEFT JOIN eid_base_data ON eid_attendance_log.userPrimaryCode = eid_base_data.PROJECTPRIMARYCODE" +
             string.Format(" WHERE eid_attendance_log.ClockDate>= '{0}' AND eid_attendance_log.ClockDate <= '{1}'", startDate, endDate) +
            " GROUP BY" +
            " eid_attendance_log.userPrimaryCode," +
            " eid_attendance_log.ClockDate";

            MySqlDataAdapter Adapter = new MySqlDataAdapter(query,myConn);
            Adapter.Fill(result);

            return result;
        }

        internal static List<AttendanceDetailsObject> GetAttendanceDetailsReport(string startDate, string endDate)
        {
            DataTable result = new DataTable();
            List<AttendanceDetailsObject> res = new List<AttendanceDetailsObject>();

            string query = " SELECT " +
            " eid_base_data.PROJECTPRIMARYCODE as 'PPCode'," +
            " eid_base_data.WEBACCESSPIN as 'UserId'," +
            " eid_base_data.TITLEPREFIX as 'Title'," +
            " eid_base_data.SURNAME as 'Surname'," +
            " eid_base_data.MIDDLENAME as 'Middle_Name'," +
            " eid_base_data.FIRSTNAME as 'First_Name'," +
            " eid_attendance_log.ClockDate as 'Date'," +
            " COUNT(eid_attendance_log.ClockTime) as 'Clock_Count'," +
            " MIN(eid_attendance_log.ClockTime) as 'Clock_In'," +
            " MAX(eid_attendance_log.ClockTime) as 'Clock_Out'," +
            " TIMEDIFF(MAX(eid_attendance_log.TransactionDateTime), MIN(eid_attendance_log.TransactionDateTime)) as 'Time_Spent'" +
            " FROM" +
            " eid_attendance_log" +
            " LEFT JOIN eid_base_data ON eid_attendance_log.userPrimaryCode = eid_base_data.PROJECTPRIMARYCODE" +
             string.Format(" WHERE eid_attendance_log.ClockDate>= '{0}' AND eid_attendance_log.ClockDate <= '{1}'", startDate, endDate) +
            " GROUP BY" +
            " eid_attendance_log.ClockDate, eid_attendance_log.userPrimaryCode" +
            " "+
            " ORDER BY eid_attendance_log.TransactionDateTime";

            MySqlDataAdapter Adapter = new MySqlDataAdapter(query, myConn);
            Adapter.Fill(result);

            foreach (DataRow row in result.Rows)
            {
                AttendanceDetailsObject obj = new AttendanceDetailsObject(
                    row["UserId"].ToString(),
                    row["Title"].ToString(),
                    row["Surname"].ToString(),
                    row["First_Name"].ToString(),
                     row["Middle_Name"].ToString(),
                     FindUserMinistry(row["PPCode"].ToString(),Settings.Default.CDataField1),
                    ((DateTime)row["Date"]).ToString("yyyy-MMM-dd"),
                    row["Clock_Count"].ToString(),
                    row["Clock_In"].ToString(),
                    row["Clock_Out"].ToString(),
                    Math.Round(TimeSpan.Parse(row["Time_Spent"].ToString()).TotalHours,0).ToString()
                    );

                res.Add(obj);
            }

            return res;
        }

        internal static IList<AttendanceDetails> GetSummaryReport(string startDate, string endDate)
        {
            DataTable result = new DataTable();
            List<AttendanceDetails> res = new List<AttendanceDetails>();

            string query = " SELECT " +
            " eid_base_data.PROJECTPRIMARYCODE as 'PPCode'," +
            " eid_base_data.WEBACCESSPIN as 'UserId'," +
            " eid_base_data.TITLEPREFIX as 'Title'," +
            " eid_base_data.SURNAME as 'Surname'," +
            " eid_base_data.FIRSTNAME as 'First_Name'," +
            " eid_base_data.MIDDLENAME as 'Middle_Name'," +
            " eid_attendance_log.ClockDate as 'Date'," +
            " COUNT(eid_attendance_log.ClockTime) as 'Clock_Count'," +
            " MIN(eid_attendance_log.ClockTime) as 'Clock_In'," +
            " MAX(eid_attendance_log.ClockTime) as 'Clock_Out'," +
            " TIMEDIFF(MAX(eid_attendance_log.TransactionDateTime), MIN(eid_attendance_log.TransactionDateTime)) as 'Time_Spent'" +
            " FROM" +
            " eid_attendance_log" +
            " LEFT JOIN eid_base_data ON eid_attendance_log.userPrimaryCode = eid_base_data.PROJECTPRIMARYCODE" +
             string.Format(" WHERE eid_attendance_log.ClockDate>= '{0}' AND eid_attendance_log.ClockDate <= '{1}'", startDate, endDate) +
            " GROUP BY" +
            " eid_attendance_log.userPrimaryCode, eid_attendance_log.ClockDate" +
            " ORDER BY eid_attendance_log.TransactionDateTime";

            MySqlDataAdapter Adapter = new MySqlDataAdapter(query, myConn);
            Adapter.Fill(result);

            foreach (DataRow row in result.Rows)
            {
                AttendanceDetails obj = new AttendanceDetails
                {
                    UserId = row["UserId"].ToString(),
                    Title = row["Title"].ToString(),
                    Surname = row["Surname"].ToString(),
                    FirstName = row["First_Name"].ToString(),
                    MiddleName = row["Middle_Name"].ToString(),
                    ProjectPrimaryCode = row["PPCode"].ToString(),
                    Date = ((DateTime)row["Date"]).ToString("yyyy-MMM-dd"),
                    ClockCount = row["Clock_Count"].ToString(),
                    ClockIn = row["Clock_In"].ToString(),
                    ClockOut = row["Clock_Out"].ToString(),
                    TimeSpent = null,
                    Ministry = FindUserMinistry(row["PPCode"].ToString(), Settings.Default.CDataField1),
                };

                res.Add(obj);
            }
                        
            return res.OrderBy(x=> x.ProjectPrimaryCode).ToList();
        }

        internal static object GetTimeSpentSummary(string startDate, string endDate, out string expextedWorkHours)
        {
            List<AttendanceDetailsObject> res = new List<AttendanceDetailsObject>();
            double summarisedWorkHours = ((((Convert.ToDateTime(endDate) - Convert.ToDateTime(startDate)).TotalDays) + 1) * 8);
            expextedWorkHours = summarisedWorkHours.ToString();

            var rawResult = GetSummaryReport(startDate, endDate);

            if (rawResult.Count>0)
            {
                var distinctUser = rawResult.GroupBy(x => x.ProjectPrimaryCode).Select(y => new { ProjectPrimaryCode = y.Key });
                var distinctDates = rawResult.GroupBy(x => x.Date).Select(y => new { Date = y.Key });

                foreach (var user in distinctUser)
                {
                    string timeSpent = GetTotalTimeSpentbyUser(user.ProjectPrimaryCode, rawResult.ToList(), summarisedWorkHours);
                    var userRes = rawResult.FirstOrDefault(x => x.ProjectPrimaryCode == user.ProjectPrimaryCode);
                    AttendanceDetailsObject AttendanceDetailsObject = new AttendanceDetailsObject(
                        userRes.UserId,
                        userRes.Title,
                        userRes.Surname,
                        userRes.FirstName,
                        userRes.MiddleName,
                        userRes.Ministry,
                        userRes.Date,
                        userRes.ClockCount,
                        userRes.ClockIn,
                        userRes.ClockOut,
                        timeSpent
                        );
                    res.Add(AttendanceDetailsObject);

                } 
            }

            return res;
        }

        private static string GetTotalTimeSpentbyUser(string projectPrimaryCode, List<AttendanceDetails> rawResult, double expectedWorkingHours)
        {
            string res = string.Empty;
            var userDailyClock = rawResult.Where(x => x.ProjectPrimaryCode == projectPrimaryCode).GroupBy(x => x.Date).Select(x => new {
                Date = x.Key,
                ClockIn = x.FirstOrDefault().ClockIn,
                ClockOut = x.FirstOrDefault().ClockOut
            });

            Double TotalDailyHoursHours = 0;

            foreach (var userRes in userDailyClock)
            {
                var MinTime = TimeSpan.Parse(userRes.ClockIn.ToString()).TotalHours;
                var MaxTime = TimeSpan.Parse(userRes.ClockOut.ToString()).TotalHours;
                TotalDailyHoursHours += MaxTime - MinTime;
            }

            double weeklyWorkHours = Math.Round(TotalDailyHoursHours, 0);

            res = weeklyWorkHours > expectedWorkingHours ? expectedWorkingHours.ToString() : weeklyWorkHours.ToString();
            return res;
        }

        internal static List<AttendanceDetailsObject> GeMovementReport(string startDate, string endDate)
        {
            DataTable result = new DataTable();
            List<AttendanceDetailsObject> res = new List<AttendanceDetailsObject>();

            string query = " SELECT " +
            " eid_base_data.PROJECTPRIMARYCODE as 'PPCode'," +
            " eid_base_data.WEBACCESSPIN as 'UserId'," +
            " eid_base_data.TITLEPREFIX as 'Title'," +
            " eid_base_data.SURNAME as 'Surname'," +
            " eid_base_data.FIRSTNAME as 'First_Name'," +
            " eid_base_data.MIDDLENAME as 'Middle_Name'," +
            " eid_attendance_log.ClockDate as 'Date'," +
            " eid_attendance_log.ClockTime as 'Clock_Time'" +
            " FROM" +
            " eid_attendance_log" +
            " LEFT JOIN eid_base_data ON eid_attendance_log.userPrimaryCode = eid_base_data.PROJECTPRIMARYCODE" +
             string.Format(" WHERE eid_attendance_log.ClockDate>= '{0}' AND eid_attendance_log.ClockDate <= '{1}'", startDate, endDate)+
             " ORDER BY eid_attendance_log.TransactionDateTime";

            MySqlDataAdapter Adapter = new MySqlDataAdapter(query, myConn);
            Adapter.Fill(result);

            foreach (DataRow row in result.Rows)
            {
                AttendanceDetailsObject obj = new AttendanceDetailsObject(
                    row["UserId"].ToString(),
                    row["Title"].ToString(),
                    row["Surname"].ToString(),
                    row["First_Name"].ToString(),
                     row["Middle_Name"].ToString(),
                     FindUserMinistry(row["PPCode"].ToString(), Settings.Default.CDataField1),
                    ((DateTime)row["Date"]).ToString("yyyy-MMM-dd"),
                    null,
                    row["Clock_Time"].ToString(),
                    null,
                    null
                    );

                res.Add(obj);
            }
            return res;
        }

        internal static object GetLatenessReport(string resumptionGrace, string startDate, string endDate)
        {
            DataTable result = new DataTable();
            List<AttendanceDetailsObject> res = new List<AttendanceDetailsObject>();

            string query = " SELECT " +
            " eid_base_data.PROJECTPRIMARYCODE as 'PPCode'," +
            " eid_base_data.WEBACCESSPIN as 'UserId'," +
            " eid_base_data.TITLEPREFIX as 'Title'," +
            " eid_base_data.SURNAME as 'Surname'," +
            " eid_base_data.FIRSTNAME as 'First_Name'," +
            " eid_base_data.MIDDLENAME as 'Middle_Name'," +
            " eid_attendance_log.ClockDate," +
            " COUNT(eid_attendance_log.ClockTime) as 'Clock_Count'," +
            " MIN(eid_attendance_log.ClockTime) as 'Clock_In'," +
            " MAX(eid_attendance_log.ClockTime) as 'Clock_Out'," +
            " TIMEDIFF(MAX(eid_attendance_log.TransactionDateTime), MIN(eid_attendance_log.TransactionDateTime)) as 'Time_Spent'" +
            " FROM" +
            " eid_attendance_log" +
            " INNER JOIN eid_base_data ON eid_attendance_log.userPrimaryCode = eid_base_data.PROJECTPRIMARYCODE" +
             string.Format(" WHERE eid_attendance_log.ClockDate>= '{0}' AND eid_attendance_log.ClockDate <= '{1}'", startDate, endDate) +
            " GROUP BY" +
            " eid_attendance_log.userPrimaryCode, eid_attendance_log.ClockDate" +
            string.Format(" HAVING  Clock_In > '{0}' ORDER BY eid_attendance_log.TransactionDateTime", resumptionGrace)
            ;


            MySqlDataAdapter Adapter = new MySqlDataAdapter(query, myConn);
            Adapter.Fill(result);

            foreach (DataRow row in result.Rows)
            {
                double weeklyWorkHours = Math.Round(TimeSpan.Parse(row["Time_Spent"].ToString()).TotalHours, 0);
                string summarisedWorkHours = weeklyWorkHours > 40 ? "40 Hours" : weeklyWorkHours.ToString() + " Hours";
                AttendanceDetailsObject obj = new AttendanceDetailsObject(
                    row["UserId"].ToString(),
                    row["Title"].ToString(),
                    row["Surname"].ToString(),
                    row["First_Name"].ToString(),
                     row["Middle_Name"].ToString(),
                     FindUserMinistry(row["PPCode"].ToString(), Settings.Default.CDataField1),
                    ((DateTime)row["ClockDate"]).ToString("yyyy-MMM-dd"),
                    row["Clock_Count"].ToString(),
                    row["Clock_In"].ToString(),
                    row["Clock_Out"].ToString(),
                    Math.Round(TimeSpan.Parse(row["Time_Spent"].ToString()).TotalHours, 0).ToString()
                   );

                res.Add(obj);
            }

            return res;
        }

        internal static List<AttendanceDetailsObject> GeAbsenteeReport(string startDate, string endDate)
        {
            DataTable result = new DataTable();
            List<AttendanceDetailsObject> res = new List<AttendanceDetailsObject>();

            string query = " SELECT " +
            " eid_base_data.PROJECTPRIMARYCODE as 'PPCode'," +
            " WEBACCESSPIN as 'UserId'," +
            " TITLEPREFIX as 'Title'," +
            " SURNAME as 'Surname'," +
            " FIRSTNAME as 'First_Name'," +
            " MIDDLENAME as 'Middle_Name'" +
            " FROM" +
            " eid_base_data" +
            " WHERE PROJECTPRIMARYCODE NOT IN" +
             string.Format(" ( SELECT  userPrimaryCode from eid_attendance_log WHERE ClockDate>= '{0}' AND ClockDate <= '{1}')", startDate,endDate)+
             " ORDER BY Surname, Firstname, Middlename";

            MySqlDataAdapter Adapter = new MySqlDataAdapter(query, myConn);
            Adapter.Fill(result);

            foreach (DataRow row in result.Rows)
            {
                AttendanceDetailsObject obj = new AttendanceDetailsObject(
                    row["UserId"].ToString(),
                    row["Title"].ToString(),
                    row["Surname"].ToString(),
                    row["First_Name"].ToString(),
                    row["Middle_Name"].ToString(),
                    FindUserMinistry(row["PPCode"].ToString(), Settings.Default.CDataField1),
                    null,
                    null,
                    null,
                    null,
                    null
                    );
                res.Add(obj);
            }
            return res;
        }

        internal static object GetAbsconderReport(string startDate, string endDate)
        {
            DataTable result = new DataTable();
            List<AttendanceDetailsObject> res = new List<AttendanceDetailsObject>();

            string query = " SELECT " +
            " eid_base_data.PROJECTPRIMARYCODE as 'PPCode'," +
            " eid_base_data.WEBACCESSPIN as 'UserId'," +
            " eid_base_data.TITLEPREFIX as 'Title'," +
            " eid_base_data.SURNAME as 'Surname'," +
            " eid_base_data.FIRSTNAME as 'First_Name'," +
            " eid_base_data.MIDDLENAME as 'Middle_Name'," +
            " eid_attendance_log.ClockDate," +
            " COUNT(eid_attendance_log.ClockTime) as 'Clock_Count'," +
            " MIN(eid_attendance_log.ClockTime) as 'Clock_In'," +
            " MAX(eid_attendance_log.ClockTime) as 'Clock_Out'," +
            " TIMEDIFF(MAX(eid_attendance_log.TransactionDateTime), MIN(eid_attendance_log.TransactionDateTime)) as 'Time_Spent'" +
            " FROM" +
            " eid_attendance_log" +
            " INNER JOIN eid_base_data ON eid_attendance_log.userPrimaryCode = eid_base_data.PROJECTPRIMARYCODE" +
             string.Format(" WHERE eid_attendance_log.ClockDate>= '{0}' AND eid_attendance_log.ClockDate <= '{1}'", startDate, endDate) +
            " GROUP BY" +
            " eid_attendance_log.ClockDate, eid_attendance_log.userPrimaryCode"+
            " HAVING Clock_Count=1 ORDER BY eid_attendance_log.TransactionDateTime";


            MySqlDataAdapter Adapter = new MySqlDataAdapter(query, myConn);
            Adapter.Fill(result);

            foreach (DataRow row in result.Rows)
            {
                AttendanceDetailsObject obj = new AttendanceDetailsObject(
                    row["UserId"].ToString(),
                    row["Title"].ToString(),
                    row["Surname"].ToString(),
                    row["First_Name"].ToString(),
                    row["Middle_Name"].ToString(),
                    FindUserMinistry(row["PPCode"].ToString(), Settings.Default.CDataField1),
                    ((DateTime)row["ClockDate"]).ToString("yyyy-MMM-dd"),
                    row["Clock_Count"].ToString(),
                    row["Clock_In"].ToString(),
                    row["Clock_Out"].ToString(),
                    row["Time_Spent"].ToString()
                    );                   

                res.Add(obj);
            }

            return res;
        }
    }
}
