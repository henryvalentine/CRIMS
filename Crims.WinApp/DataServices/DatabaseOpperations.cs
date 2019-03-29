using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crims.UI.Win.Enroll.Classes;
using MySql.Data.MySqlClient;
using Crims.UI.Win.Enroll.Enums;
using Crims.Data;
using Crims.UI.Win.Enroll.Properties;
using Crims.Data.Models;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Configuration;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.Web.Helpers;
using System.Windows.Forms;
using Crims.Core.Logging;
using Crims.UI.Win.Enroll.Forms;
using Crims.UI.Win.Enroll.Helpers;
using Neurotec.Biometrics;
using Neurotec.Biometrics.Client;
using Neurotec.Biometrics.Standards;
using Neurotec.Images;
using Neurotec.IO;
using Neurotec.Licensing;
using Repository.Pattern.Infrastructure;

namespace Crims.UI.Win.Enroll.DataServices
{
    public class DatabaseOpperations
    {
        const string Address = "/local";
        const string Port = "5000";
        const string FingerprintComponents = "Images.WSQ,Biometrics.FingerMatching";

        public DatabaseOpperations()
        {

        }

        private Project _project;

        //public static string localConnectionString = "server=" + Settings.Default.DBServer + ";" +
        //    "port=" + Settings.Default.DBPort + ";" +
        //    "database=" + Settings.Default.DBName + ";" +
        //    "uid=" + Settings.Default.DBUser + ";"
        //    + "password=" + Settings.Default.DBPassword + "";

        //all of a sudden and outta nowhere, the connectionstring pattern declared and commented out above stopped working with CrimsDbContext
        //so i have to define <connectionstrings> node in app.config
        public static string localConnectionString = "crimsDbEntities"; //ConfigurationManager.ConnectionStrings["crimsDbEntities"].ConnectionString; this refused to work also

        #region Tools
        internal static int QueryAndConvertToWQS(string query, string connectionString, string destinationDirectory)
        {
            using (MySqlConnection conn = new MySqlConnectionManager().DbConnection(connectionString))
            {
                DataTable result = new DataTable();
                MySqlDataAdapter Adapter = new MySqlDataAdapter(query, conn);
                Adapter.Fill(result);

                int totalProcessed = 0;
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        //Save File in Destination Directory
                        File.WriteAllBytes(destinationDirectory + row[0].ToString() + "_" + row[2].ToString() + ".wsq", ((byte[])row[1]).ToArray());
                        totalProcessed++;
                    }
                }
                return totalProcessed;
            }
        }
        #endregion

        //public static string syncConnectionString = "server=" + Settings.Default.syncDBServer + ";" +
        //    "port=" + Settings.Default.syncDBPort + ";" +
        //    "database=" + Settings.Default.syncDBName + ";" +
        //    "uid=" + Settings.Default.syncDBUser + ";"
        //    + "password=" + Settings.Default.syncDBPassword + "";

        public static string syncConnectionString = "crimsRemoteDbEntities";
        internal static int GetTotalRecordCount(Project project)
        {
            using (CrimsDbContext dbContext = new CrimsDbContext(localConnectionString))
            {
                return dbContext.BaseDatas.Count(b => b.ProjectCode == project.ProjectCode);
            }
        }

        internal static Project GetProject()
        {
            using (CrimsDbContext dbContext = new CrimsDbContext(localConnectionString))
            {
                var projects = dbContext.Projects.ToList();
                if (!projects.Any())
                {
                    return new Project();
                }
                return projects[0];
            }
        }

        internal bool TestDBConnection(string server, string db, string port, string user, string password)
        {
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnectionManager().DbConnection(server, db, port, user, password);
                if (conn != null)
                {
                    return conn.State == ConnectionState.Open ? true : false;
                }
                else { return false; }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open) { conn.Close(); conn.Dispose(); }
            }
        }

        internal static DataTable AuthoriseOnlyUserName(string userName)
        {
            using (MySqlConnection conn = new MySqlConnectionManager().DbConnection())
            {
                DataTable result = new DataTable();
                string query = string.Format("SELECT * FROM aspnetusers where Email = '{0}' Limit 1", userName);

                MySqlDataAdapter Adapter = new MySqlDataAdapter(query, conn);
                Adapter.Fill(result);

                return result;
            }
        }

        private static BiometricsRecord FillBiometricsRecord(DataRow resRow)
        {
            BiometricsRecord biometricsRecord = new BiometricsRecord
            {
                TableId = resRow["TableId"].ToString(),
                EnrollmentId = resRow["EnrollmentId"].ToString(),
                ProjectPrimaryCode = resRow["ProjectPrimaryCode"].ToString(),
                Gender = resRow["Gender"].ToString(),
                Title = resRow["Title"].ToString(),
                Surname = resRow["Surname"].ToString(),
                FirstName = resRow["FirstName"].ToString(),
                MiddleName = resRow["MiddleName"].ToString()
            };
            return biometricsRecord;
        }

        internal static int GetCaptureSyncUpdateStatus(Project project, string userId, out int totalSyc)
        {
            try
            {
                var pending = (int) EnumManager.ApprovalStatus.Pending;

                using (CrimsDbContext db = new CrimsDbContext(localConnectionString))
                {
                    var today = DateTime.Today;
                    var enrolments =
                        (from b in
                            db.BaseDatas.Where(b => b.EnrollmentDate >= today && (b.CreatedBy == userId || b.LastUpdatedby == userId))
                            join f in db.FingerprintImages on b.EnrollmentId equals f.EnrollmentId
                            join p in db.Photographs on b.EnrollmentId equals p.EnrollmentId
                            join c in db.CustomDatas on b.EnrollmentId equals c.EnrollmentId
                            select b
                            ).ToList();
                    
                    var capturedList = new List<BaseData>();
                    var syncList = new List<SyncJobHistory>();
                    if (enrolments.Any())
                    {
                        enrolments.ForEach(e =>
                        {
                            if (!capturedList.Exists(b => b.EnrollmentId == e.EnrollmentId))
                            {
                                capturedList.Add(e);
                            }
                        });
                    }
                   
                    var synchronisedData = (from s in db.SyncJob.Where(j => j.Date >= today && j.UserId == userId)
                        join sh in db.SyncJobHistory on s.Id equals sh.SyncJobId
                        select sh
                        ).ToList();

                    if (synchronisedData.Any())
                    {
                        synchronisedData.ForEach(e =>
                        {
                            if (!syncList.Exists(b => b.RecordId == e.RecordId))
                            {
                                syncList.Add(e);
                            }
                        });
                    }

                    totalSyc = syncList.Count;
                    return capturedList.Count;
                }
            }
            catch (Exception ex)
            {
                totalSyc = 0;
                return 0;
            }
        }

        internal static int GetCaptureSyncUpdateStatus2(Project project, string userId, out int totalSyc)
        {
            var today = DateTime.Today;
            try
            {
                var localConnectionStringx = "server=" + Settings.Default.DBServer + ";" +
                                             "port=" + Settings.Default.DBPort + ";" +
                                             "database=" + Settings.Default.DBName + ";" +
                                             "uid=" + Settings.Default.DBUser + ";"
                                             + "password=" + Settings.Default.DBPassword + "";
                var conn = new MySqlConnection(localConnectionStringx);

                var sql = $"SELECT t.EnrollmentId FROM basedatas t WHERE (t.ProjectCode = '{project.ProjectCode}' AND t.EnrollmentDate >= '{today:yyyy-MM-dd hh:mm:ss}' AND (t.CreatedBy = '{userId}' OR t.LastUpdatedby = '{userId}') AND (SELECT COUNT(f.EnrollmentId) FROM fingerprintimages f WHERE f.EnrollmentId = t.EnrollmentId) > 0 AND (SELECT COUNT(p.EnrollmentId) FROM photographs p WHERE p.EnrollmentId = t.EnrollmentId) > 0 " +
                          $"AND (SELECT COUNT(q.EnrollmentId) FROM customdatas q WHERE q.EnrollmentId = t.EnrollmentId) > 0 )";

                var synchronised = 0;
                var captured = 0;
                var cmd = new MySqlCommand(sql, conn);
                conn.Open();

                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        if (!rdr.HasRows) continue;
                        captured += 1;
                    }
                }
                //using (var cmd = new MySqlCommand(sql, conn))
                //{
                //    captured = Convert.ToInt32(cmd.ExecuteScalar());
                //}

                sql = $"SELECT t.Id FROM syncjobhistories t JOIN syncjobs s ON t.SyncJobId = s.Id WHERE (s.UserId = '{userId}' AND s.Date >='{today:yyyy-MM-dd hh:mm:ss}')";

                cmd = new MySqlCommand(sql, conn);
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        if (!rdr.HasRows) continue;
                        synchronised += 1;
                    }
                }

                totalSyc = synchronised;
                return captured;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Data Synchronisation");
                totalSyc = 0;
                return 0;
            }
        }

        internal static int GetOnlyCaptureUpdateStatus(Project project, string userId)
        {
            try
            {
                using (CrimsDbContext db = new CrimsDbContext(localConnectionString))
                {
                    var today = DateTime.Today;
                    var enrolments =
                    (from b in
                        db.BaseDatas.Where(b => b.EnrollmentDate >= today && (b.CreatedBy == userId || b.LastUpdatedby == userId))
                        join f in db.FingerprintImages on b.EnrollmentId equals f.EnrollmentId
                        join p in db.Photographs on b.EnrollmentId equals p.EnrollmentId
                        join c in db.CustomDatas on b.EnrollmentId equals c.EnrollmentId
                        select b
                    ).ToList();

                    var capturedList = new List<BaseData>();
                    if (enrolments.Any())
                    {
                        enrolments.ForEach(e =>
                        {
                            if (!capturedList.Exists(b => b.EnrollmentId == e.EnrollmentId))
                            {
                                capturedList.Add(e);
                            }
                        });
                    }
                    
                    return capturedList.Count;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        
        #region Sync User Profiles
        internal static int DownloadNewUserProfiles()
        {
            try
            {
                //get User Profile Data from the Server
                List<AspNetUsers> _AspNetUsers = null;
                List<AspNetUserRoles> _AspNetUserRoles = null;
                List<UserProfile> _UserProfile = null;
                var roles = new List<AspNetRoles>();
                GetServerUsers(out _AspNetUsers, out _UserProfile, out _AspNetUserRoles, out roles);

                //Sve users in Local DB is Users Does not Exist
                int totalUserAdded = 0;
                var localConnectionStringx = "server=" + Settings.Default.DBServer + ";" +
                                                           "port=" + Settings.Default.DBPort + ";" +
                                                           "database=" + Settings.Default.DBName + ";" +
                                                           "uid=" + Settings.Default.DBUser + ";"
                                                           + "password=" + Settings.Default.DBPassword + "";
                
                using (var conn1 = new MySqlConnection(localConnectionStringx))
                {
                    conn1.Open();
                    //Check if user exists before installing
                    var table1 = new DataTable();
                    new MySqlDataAdapter($"SELECT * from AspNetRoles", conn1).Fill(table1);

                    if (!(table1.Rows.Count > 0))
                    {
                        roles.ForEach(r =>
                        {
                            string query = string.Format(
                                $"Insert into AspNetRoles (Id,Name) VALUES ('{r.Id}','{r.Name}')");
                            new MySqlCommand(query, conn1).ExecuteNonQuery();
                        });
                    }
                }

                foreach (var user in _UserProfile)
                {
                    using (var conn = new MySqlConnection(localConnectionStringx))
                    {
                        conn.Open();
                        //Check if user exists before installing
                        DataTable table = new DataTable();
                        new MySqlDataAdapter($"SELECT ID from UserProfiles WHERE ID='{user.Id.Trim()}'", conn).Fill(table);

                        if (!(table.Rows.Count > 0))
                        {
                            string query = string.Format(
                                " Insert into UserProfiles (Id,FullName,PhoneNumber,Sex,DateCreated,Status)" +
                                $" VALUES ('{user.Id}','{user.FullName}','{user.PhoneNumber}','{user.Sex}','{user.DateCreated.ToString("yyyy-MM-dd hh:mm:ss")}','{user.Status}')");

                            var userProfileInserted = new MySqlCommand(query, conn).ExecuteNonQuery();

                            if (userProfileInserted > 0)
                            {
                                //Get AspNetUser for this UserProfile and Save
                                var aspnetUserProfileUsers = _AspNetUsers.Where(x => x.UserInfo_Id == user.Id).ToList();
                                if (aspnetUserProfileUsers.Count > 0)
                                {
                                    foreach (var aspnetUser in aspnetUserProfileUsers)
                                    {
                                        string aspnetUserQuery = string.Format(
                                            " Insert into aspnetUsers (AccessFailedCount, Email, EmailConfirmed, Id, LockoutEnabled, " +
                                            " PasswordHash, PhoneNumber, PhoneNumberConfirmed, SecurityStamp, TwoFactorEnabled, UserInfo_Id, UserName)" +
                                            string.Format(
                                                " VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                                                aspnetUser.AccessFailedCount, aspnetUser.Email,
                                                aspnetUser.EmailConfirmed ? "1" : "0", aspnetUser.Id,
                                                aspnetUser.LockoutEnabled ? "1" : "0",
                                                aspnetUser.PasswordHash, aspnetUser.PhoneNumber,
                                                aspnetUser.PhoneNumberConfirmed ? "1" : "0", aspnetUser.SecurityStamp,
                                                aspnetUser.TwoFactorEnabled ? "1" : "0",
                                                aspnetUser.UserInfo_Id, aspnetUser.UserName));

                                        new MySqlCommand(aspnetUserQuery, conn).ExecuteNonQuery();

                                        //Get AspNetUserRoles for this UserProfile and Save
                                        var aspnetUserRoles = _AspNetUserRoles.Where(x => x.UserId == aspnetUser.Id).ToList();
                                        if (aspnetUserRoles.Count > 0)
                                        {
                                            foreach (var n in aspnetUserRoles)
                                            {
                                                string aspnetUserRoleQuery =
                                                    string.Format(
                                                        " Insert into aspnetUserRoles (UserId, RoleId)  VALUES ('{0}','{1}')",
                                                        n.UserId, n.RoleId);
                                                new MySqlCommand(aspnetUserRoleQuery, conn).ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }

                                totalUserAdded++;
                            }
                        }
                        else
                        {

                            using (var cn = new MySqlConnection(localConnectionStringx))
                            {
                                cn.Open();
                                var profileCmd = new MySqlCommand
                                {
                                    Connection = cn,
                                    CommandText = $"update userprofiles set FullName = '{user.FullName}',PhoneNumber='{user.PhoneNumber}',Sex='{user.Sex}',Status='{user.Status}' where Id = '{user.Id}'"
                                };
                                profileCmd.ExecuteNonQuery();

                                var aspnetUserProfileUsers = _AspNetUsers.Where(x => x.UserInfo_Id == user.Id).ToList();
                                if (aspnetUserProfileUsers.Any())
                                {
                                    var aspnetUser = aspnetUserProfileUsers[0];
                                    var aspNetCmd = new MySqlCommand
                                    {
                                        Connection = cn,
                                        CommandText = $"update aspnetUsers set Email ='{aspnetUser.Email}',PhoneNumber='{aspnetUser.PhoneNumber}',PasswordHash='{aspnetUser.PasswordHash}',UserName='{aspnetUser.UserName}',SecurityStamp='{aspnetUser.SecurityStamp}' where UserInfo_Id = '{user.Id}'"
                                    };

                                    //insere.CommandText = "INSERT INTO cliente(cod, nome, endereco) Values(@cod + ,'@nome', '@end');";
                                    //insere.Parameters.AddWithValue("@cod", cod);
                                    //insere.Parameters.AddWithValue("@nome", nome);
                                    //insere.Parameters.AddWithValue("@end", end);
                                    //insere.ExecuteNonQuery();
                                    //conect.Close();

                                    aspNetCmd.ExecuteNonQuery();

                                    var aspnetUserRoles = _AspNetUserRoles.Where(x => x.UserId == aspnetUser.Id).ToList();
                                    if (aspnetUserRoles.Count > 0)
                                    {
                                        foreach (var n in aspnetUserRoles)
                                        {
                                            var aspnetUserRoleQuery = $"update aspnetUserRoles set UserId='{n.UserId}', RoleId='{n.RoleId}' where UserId = '{aspnetUser.Id}'";
                                            new MySqlCommand(aspnetUserRoleQuery, cn).ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return totalUserAdded;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void GetServerUsers(out List<AspNetUsers> _AspNetUsers, out List<UserProfile> _UserProfile, out List<AspNetUserRoles> _AspNetUserRoles, out List<AspNetRoles> roles)
        {
            _AspNetUsers = new List<AspNetUsers>() { };
            _UserProfile = new List<UserProfile>() { };
            roles = new List<AspNetRoles>() { };
            _AspNetUserRoles = new List<AspNetUserRoles>() { };

            using (MySqlConnection conn = new MySqlConnectionManager().DbConnection(Settings.Default.syncDBServer, Settings.Default.syncDBName, Settings.Default.syncDBPort, Settings.Default.syncDBUser, Settings.Default.syncDBPassword))
            {
                //Download User Profiles from the Server
                DownloadUserProfiles(_UserProfile, conn);

                //Download AspNetUsers From the Server
                DownloadAspnetUserProfiles(_AspNetUsers, conn);

                //Download Roles
                DownloadRoles(roles, conn);
                //Download User Roles from the Server
                DownloadUserRoles(_AspNetUserRoles, conn);
            }
        }
        private static void DownloadUserProfiles(List<UserProfile> _UserProfile, MySqlConnection conn)
        {
            var userProfileResult = new DataTable();
            var query1 = "SELECT Id,FullName,PhoneNumber,Sex,DateCreated,Status FROM UserProfiles";
            var adapter = new MySqlDataAdapter(query1, conn);
            adapter.Fill(userProfileResult);
            if (userProfileResult.Rows.Count > 0)
            {
                foreach (DataRow row in userProfileResult.Rows)
                {
                    UserProfile users = new UserProfile
                    {
                        Id = row[0].ToString(),
                        FullName = row[1].ToString(),
                        PhoneNumber = row[2].ToString(),
                        Sex = row[3].ToString(),
                        DateCreated = DateTime.Parse(row[4].ToString()),
                        Status = row[5].ToString()
                    };
                    _UserProfile.Add(users);
                }
            }
        }
        private static void DownloadAspnetUserProfiles(List<AspNetUsers> aspNetUsers, MySqlConnection conn)
        {
            try
            {
                DataTable aspNetUserResult = new DataTable();
                string query2 = "SELECT AccessFailedCount, Email, EmailConfirmed, Id, LockoutEnabled, " +
                                " PasswordHash, PhoneNumber, PhoneNumberConfirmed, SecurityStamp, TwoFactorEnabled, UserInfo_Id, UserName FROM aspnetUsers";
                MySqlDataAdapter Adapter2 = new MySqlDataAdapter(query2, conn);
                Adapter2.Fill(aspNetUserResult);


                if (aspNetUserResult.Rows.Count > 0)
                {
                    foreach (DataRow row in aspNetUserResult.Rows)
                    {
                        var users = new AspNetUsers
                        {
                            AccessFailedCount = (int)row[0],
                            Email = row[1].ToString(),
                            EmailConfirmed = (bool)row[2],
                            Id = row[3].ToString(),
                            LockoutEnabled = (bool)row[4],
                            PasswordHash = row[5].ToString(),
                            PhoneNumber = row[6].ToString(),
                            PhoneNumberConfirmed = (bool)row[7],
                            SecurityStamp = row[8].ToString(),
                            TwoFactorEnabled = (bool)row[9],
                            UserInfo_Id = row[10].ToString(),
                            UserName = row[11].ToString()
                        };
                        aspNetUsers.Add(users);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private static void DownloadRoles(List<AspNetRoles> aspNetUserRoles, MySqlConnection conn)
        {
            var query = "SELECT * FROM AspnetRoles";
            var cmd = new MySqlCommand(query, conn);
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    if (!rdr.HasRows) continue;
                    var role = new AspNetRoles
                    {
                        Id = rdr["Id"].ToString(),
                        Name = rdr["Name"].ToString()
                    };
                    aspNetUserRoles.Add(role);
                }
            }
        }
        private static void DownloadUserRoles(List<AspNetUserRoles> aspNetUserRoles, MySqlConnection conn)
        {
            DataTable userRolesResult = new DataTable();
            string query3 = string.Format("SELECT UserId, RoleId FROM AspNetUserRoles");
            MySqlDataAdapter Adapter3 = new MySqlDataAdapter(query3, conn);
            Adapter3.Fill(userRolesResult);
            if (userRolesResult.Rows.Count > 0)
            {
                foreach (DataRow row in userRolesResult.Rows)
                {
                    AspNetUserRoles roles = new AspNetUserRoles
                    {
                        UserId = row[0].ToString(),
                        RoleId = row[1].ToString(),
                    };
                    aspNetUserRoles.Add(roles);
                }
            }
        }
        #endregion

        #region Sync Job Management
        internal static IList<BaseData> GetSyncBacklog(Project project, SyncBacklogInput input, out List<FingerprintReason> fingerprintReasons)
        {
            var reasons = new List<FingerprintReason>();
            try
            {
                var pending = (int)EnumManager.ApprovalStatus.Pending;
                var localConnectionStringx = "server=" + Settings.Default.DBServer + ";" +
                "port=" + Settings.Default.DBPort + ";" +
                "database=" + Settings.Default.DBName + ";" +
                "uid=" + Settings.Default.DBUser + ";"
                + "password=" + Settings.Default.DBPassword + "";
                var conn = new MySqlConnection(localConnectionStringx);

                var sql = "";
                var baseDataList = new List<BaseData>();

                if (input.SyncMode == SyncMode.Filtered)
                {
                    sql = $"SELECT t.* FROM basedatas t WHERE (t.ProjectCode = '{project.ProjectCode}' AND t.EnrollmentDate >= '{input.FilterStart:yyyy-MM-dd hh:mm:ss}' AND t.EnrollmentDate <= '{input.FilterEnd:yyyy-MM-dd hh:mm:ss}' AND (SELECT COUNT(f.EnrollmentId) FROM fingerprintimages f WHERE f.EnrollmentId = t.EnrollmentId) > 0 AND (SELECT COUNT(p.EnrollmentId) FROM photographs p WHERE p.EnrollmentId = t.EnrollmentId) > 0 " +
                        $"AND (SELECT COUNT(q.EnrollmentId) FROM customdatas q WHERE q.EnrollmentId = t.EnrollmentId) > 0 AND (SELECT COUNT(h.RecordId) FROM syncjobhistories h WHERE h.RecordId = t.EnrollmentId) < 1 AND t.ApprovalStatus = {pending})";

                }

                else if (input.SyncMode == SyncMode.AllPending)
                {
                    sql = $"SELECT t.* FROM basedatas t WHERE (t.ProjectCode = '{project.ProjectCode}' AND (SELECT COUNT(f.EnrollmentId) FROM fingerprintimages f WHERE f.EnrollmentId = t.EnrollmentId) > 0 AND (SELECT COUNT(p.EnrollmentId) FROM photographs p WHERE p.EnrollmentId = t.EnrollmentId) > 0 AND (SELECT COUNT(h.EnrollmentId) FROM approvals h WHERE h.EnrollmentId = t.EnrollmentId) = 0 " +
                            $"AND (SELECT COUNT(q.EnrollmentId) FROM customdatas q WHERE q.EnrollmentId = t.EnrollmentId) > 0 AND (SELECT COUNT(h.RecordId) FROM syncjobhistories h WHERE h.RecordId = t.EnrollmentId) < 1 AND t.ApprovalStatus = {pending})";
                }

                else
                if (input.SyncMode == SyncMode.Specific)
                {
                    sql = $"SELECT t.* FROM basedatas t WHERE (t.ProjectCode = '{project.ProjectCode}' AND t.EnrollmentId = '{input.EnrollmentId}' AND (SELECT COUNT(f.EnrollmentId) FROM fingerprintimages f WHERE f.EnrollmentId = t.EnrollmentId) > 0 AND (SELECT COUNT(p.EnrollmentId) FROM photographs p WHERE p.EnrollmentId = t.EnrollmentId) > 0 AND (SELECT COUNT(q.EnrollmentId) FROM customdatas q WHERE q.EnrollmentId = t.EnrollmentId) > 0" +
                        $"AND t.ApprovalStatus = {pending})";
                }

                if (string.IsNullOrEmpty(sql))
                {
                    fingerprintReasons = reasons;
                    return new List<BaseData>();
                }

                conn.Open();
                var cmd = new MySqlCommand(sql, conn);

                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        if (!rdr.HasRows) continue;
                        var enrollment = new BaseData
                        {
                            EnrollmentId = rdr["EnrollmentId"].ToString(),
                            ProjectCode = rdr["ProjectCode"].ToString(),
                            ProjectSiteId = rdr["ProjectSiteId"].ToString(),
                            Surname = rdr["Surname"].ToString(),
                            Firstname = rdr["Firstname"].ToString(),
                            MiddleName = rdr["MiddleName"].ToString(),
                            Gender = rdr["Gender"].ToString(),
                            Title = rdr["Title"].ToString(),
                            FormPath = rdr["FormPath"].ToString(),
                            CreatedBy = rdr["CreatedBy"].ToString(),
                            LastUpdatedby = rdr["LastUpdatedby"].ToString(),
                            DateLastUpdated = Convert.ToDateTime(rdr["DateLastUpdated"].ToString()),
                            Email = rdr["Email"].ToString(),
                            MobileNumber = rdr["MobileNumber"].ToString(),
                            CuntryCode = rdr["CuntryCode"].ToString(),
                            ProjectPrimaryCode = rdr["ProjectPrimaryCode"].ToString(),
                            DOB = rdr["DOB"].ToString(),
                            ApprovalStatus = Convert.ToInt32(rdr["ApprovalStatus"].ToString()),
                            EnrollmentDate = Convert.ToDateTime(rdr["EnrollmentDate"].ToString()),
                            ValidIdNumber = rdr["ValidIdNumber"].ToString()
                        };

                        baseDataList.Add(enrollment);
                    }
                }

                if (baseDataList.Any())
                {
                    using (var dbContext = new CrimsDbContext(localConnectionString))
                    {
                        baseDataList.ForEach(b =>
                        {
                            var fingerReasons = dbContext.FingerprintReasons.Where(f => f.EnrollmentId == b.EnrollmentId).ToList();
                            if (fingerReasons.Any())
                            {
                                reasons.AddRange(fingerReasons);
                            }
                        });
                        
                    }
                }

                fingerprintReasons = reasons;
                return baseDataList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Data Synchronisation");
                fingerprintReasons = reasons;
                return new List<BaseData>();
            }
        }
        internal static string CreateSyncJobSession(UserAccountModel userProfile, DateTime startDate)
        {
            SyncJob _syncJob = new SyncJob()
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                StartDateTime = startDate,
                UserId = userProfile.ProfileId
            };

            using (CrimsDbContext dbContext = new CrimsDbContext(localConnectionString))
            {
                dbContext.SyncJob.Add(_syncJob);
                dbContext.SaveChanges();
                return _syncJob.Id;
            }
        }
        internal static void CloseSyncJobSession(string syncJobId)
        {
            using (CrimsDbContext dbContext = new CrimsDbContext(localConnectionString))
            {
                var syncJob = dbContext.SyncJob.FirstOrDefault(x => x.Id == syncJobId);
                if (syncJob != null)
                {
                    syncJob.EndDateTime = DateTime.Now;
                    dbContext.SaveChanges();
                }
            }
        }
        internal static void ResetSyncHistory()
        {
            using (CrimsDbContext dbContext = new CrimsDbContext(localConnectionString))
            {
                var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dbContext).ObjectContext;
                objCtx.ExecuteStoreCommand("TRUNCATE TABLE SyncJobHistories");
                objCtx.ExecuteStoreCommand("TRUNCATE TABLE SyncJobs");
                objCtx.SaveChanges();
            }
        }
        internal static IEnumerable<SyncJobHistory> GetSyncedJobHistory()
        {
            using (CrimsDbContext dbContext = new CrimsDbContext(localConnectionString))
            {
                return dbContext.SyncJobHistory.ToList();
            }
        }
        internal static void UpdateDbSyncHistory(string syncJobId, string enrollmentId)
        {
            using (CrimsDbContext dbContext = new CrimsDbContext(localConnectionString))
            {
                SyncJobHistory syncJobHistory = new SyncJobHistory
                {
                    Id = Guid.NewGuid().ToString(),
                    RecordId = enrollmentId,
                    SyncJobId = syncJobId
                };
                dbContext.SyncJobHistory.Add(syncJobHistory);
                dbContext.SaveChanges();
            }

        }
        #endregion

        #region IMPORT AND EXPORT
        internal static List<EnrollmentBackup> GenerateBackup(Project project, SyncBacklogInput input)
        {
            try
            {
                var enrollmentBackups = new List<EnrollmentBackup>();
                var baseDatas = new List<BaseData>();

                using (var dbContext = new CrimsDbContext(localConnectionString))
                {
                    if (input.SyncMode == SyncMode.Filtered)
                    {
                        baseDatas = dbContext.BaseDatas.Where(b => b.ProjectCode == project.ProjectCode && b.EnrollmentDate >= input.FilterStart && b.EnrollmentDate <= input.FilterEnd)
                            .Include("FingerprintReasons").Include("FingerprintImages").Include("Approvals").Include("CustomDatas")
                            .Include("Photographs").Include("Signatures").Include("FingerprintTemplates")
                            .ToList();
                    }

                    else if (input.SyncMode == SyncMode.AllPending)
                    {
                        baseDatas = dbContext.BaseDatas
                            .Include("FingerprintReasons").Include("FingerprintImages").Include("Approvals").Include("CustomDatas")
                            .Include("Photographs").Include("Signatures").Include("FingerprintTemplates")
                            .ToList();
                    }

                    else if (input.SyncMode == SyncMode.Specific)
                    {
                        baseDatas = dbContext.BaseDatas.Where(b => b.EnrollmentId == input.EnrollmentId)
                            .Include("FingerprintReasons").Include("FingerprintImages").Include("Approvals").Include("CustomDatas")
                            .Include("Photographs").Include("Signatures").Include("FingerprintTemplates")
                            .ToList();
                    }

                    if (!baseDatas.Any())
                    {
                        return enrollmentBackups;
                    }

                    baseDatas.ForEach(b =>
                    {
                        var enrollmentBackup = new EnrollmentBackup
                        {
                            BaseData = b
                        };
                        var customDatas = b.CustomDatas.ToList();
                        if (customDatas.Any())
                        {
                            enrollmentBackup.CustomDatas = customDatas.ToArray();
                        }
                        var fingerprintImages = b.FingerprintImages.ToList();
                        if (fingerprintImages.Any())
                        {
                            enrollmentBackup.FingerprintImages = fingerprintImages.ToArray();
                        }
                        var approvals = b.Approvals.ToList();
                        if (approvals.Any())
                        {
                            enrollmentBackup.Approvals = approvals.ToArray();
                        }
                        var fingerprintTemplates = b.FingerprintTemplates.ToList();
                        if (fingerprintTemplates.Any())
                        {
                            enrollmentBackup.FingerprintTemplate = fingerprintTemplates[0];
                        }
                        var photographs = b.Photographs.ToList();
                        if (photographs.Any())
                        {
                            enrollmentBackup.Photograph = photographs[0];
                        }
                        var signatures = b.Signatures.ToList();
                        if (signatures.Any())
                        {
                            enrollmentBackup.Signature = signatures[0];
                        }

                        var fingerprintReasons = b.FingerprintReasons.ToList();
                        if (fingerprintReasons.Any())
                        {
                            enrollmentBackup.FingerprintReasons = fingerprintReasons.ToArray();
                        }

                        if (customDatas.Any() && fingerprintImages.Any() && photographs.Any())
                        {
                            enrollmentBackups.Add(enrollmentBackup);
                        }

                    });

                    return enrollmentBackups;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        internal static List<EnrollmentBackup> ImportFromBackup(List<EnrollmentBackup> imports)
        {
            try
            {
                var importedList = new List<EnrollmentBackup>();

                imports.ForEach(b =>
                {
                    using (var dbContext = new CrimsDbContext(localConnectionString))
                    {
                        var middleName = !string.IsNullOrEmpty(b.BaseData.MiddleName) ? b.BaseData.MiddleName.Replace("'", "''").ToUpper() : null;
                        var baseDatas = dbContext.BaseDatas.Where(a => a.EnrollmentId == b.BaseData.EnrollmentId).ToList();
                        if (baseDatas.Any())
                        {
                            var baseData = baseDatas[0];
                            baseData.ApprovalStatus = b.BaseData.ApprovalStatus;
                            baseData.DOB = b.BaseData.DOB;
                            baseData.DateLastUpdated = b.BaseData.DateLastUpdated;
                            baseData.Email = b.BaseData.Email.ToUpper();
                            baseData.Firstname = b.BaseData.Firstname.ToUpper();
                            baseData.MiddleName = middleName;
                            baseData.Gender = b.BaseData.Gender;
                            baseData.Title = b.BaseData.Title;
                            baseData.ProjectPrimaryCode = b.BaseData.ProjectPrimaryCode;
                            baseData.ValidIdNumber = b.BaseData.ValidIdNumber;
                            baseData.Surname = b.BaseData.Surname.ToUpper();
                            baseData.MobileNumber = b.BaseData.MobileNumber;
                            baseData.CuntryCode = b.BaseData.CuntryCode;
                            dbContext.Entry(baseData).State = EntityState.Modified;
                        }
                        else
                        {
                            b.BaseData.MiddleName = middleName;
                            b.BaseData.Email = b.BaseData.Email.ToUpper();
                            b.BaseData.Firstname = b.BaseData.Firstname.ToUpper();
                            b.BaseData.Surname = b.BaseData.Surname.ToUpper();
                            dbContext.BaseDatas.Add(b.BaseData);
                        }

                        dbContext.SaveChanges();

                        if (b.CustomDatas.Any())
                        {
                            b.CustomDatas.ToList().ForEach(c =>
                            {
                                var customDatas = dbContext.CustomDatas.Where(d => d.CustomDataId == c.CustomDataId)
                                    .ToList();
                                if (customDatas.Any())
                                {
                                    var customData = customDatas[0];
                                    customData.CrimsCustomData = c.CrimsCustomData;
                                    customData.CustomFieldId = c.CustomFieldId;
                                    customData.CustomListId = c.CustomListId;
                                    customData.ChildCrimsCustomData = c.ChildCrimsCustomData;
                                    customData.CustomFieldId = c.CustomFieldId;
                                    customData.DateLastUpdated = c.DateLastUpdated;
                                    dbContext.Entry(customData).State = EntityState.Modified;
                                }
                                else
                                {
                                    dbContext.CustomDatas.Add(c);
                                }
                            });
                            dbContext.SaveChanges();
                        }


                        if (b.Approvals != null && b.Approvals.Any())
                        {
                            b.Approvals.ToList().ForEach(a =>
                            {
                                var approvals = dbContext.Approvals.Where(d => d.ApprovalId == a.ApprovalId).ToList();
                                if (!approvals.Any())
                                {
                                    dbContext.Approvals.Add(a);
                                }
                            });
                            dbContext.SaveChanges();
                        }

                        if (b.FingerprintImages != null && b.FingerprintImages.Any())
                        {
                            b.FingerprintImages.ToList().ForEach(f =>
                            {
                                var fingerprints = dbContext.FingerprintImages
                                    .Where(d => d.FingerIndexId == f.FingerIndexId && d.EnrollmentId == f.EnrollmentId)
                                    .ToList();
                                if (fingerprints.Any())
                                {
                                    var fingerprint = fingerprints[0];
                                    fingerprint.DateLastUpdated = f.DateLastUpdated;
                                    fingerprint.FingerPrintImage = f.FingerPrintImage;
                                    dbContext.Entry(fingerprint).State = EntityState.Modified;
                                }
                                else
                                {
                                    dbContext.FingerprintImages.Add(f);
                                }
                            });
                            dbContext.SaveChanges();
                        }

                        if (b.FingerprintReasons != null && b.FingerprintReasons.Any())
                        {
                            b.FingerprintReasons.ToList().ForEach(f =>
                            {
                                var fingerprintReasons = dbContext.FingerprintReasons
                                    .Where(d => d.FingerIndex == f.FingerIndex && d.EnrollmentId == f.EnrollmentId)
                                    .ToList();
                                if (fingerprintReasons.Any())
                                {
                                    var fingerprint = fingerprintReasons[0];
                                    fingerprint.DateLastUpdated = f.DateLastUpdated;
                                    fingerprint.FingerReason = f.FingerReason;
                                    dbContext.Entry(fingerprint).State = EntityState.Modified;
                                }
                                else
                                {
                                    dbContext.FingerprintReasons.Add(f);
                                }
                            });
                            dbContext.SaveChanges();
                        }

                        if (!string.IsNullOrEmpty(b.FingerprintTemplate?.EnrollmentId))
                        {
                            var fingerprintTemplates = dbContext.FingerprintTemplates
                                .Where(c => c.EnrollmentId == b.FingerprintTemplate.EnrollmentId).ToList();
                            if (fingerprintTemplates.Any())
                            {
                                var fingerprintTemplate = fingerprintTemplates[0];
                                fingerprintTemplate.DateLastUpdated = b.FingerprintTemplate.DateLastUpdated;
                                fingerprintTemplate.Template = b.FingerprintTemplate.Template;
                                fingerprintTemplate.UniquenessStatus = b.FingerprintTemplate.UniquenessStatus;
                                dbContext.Entry(fingerprintTemplate).State = EntityState.Modified;
                            }
                            else
                            {
                                dbContext.FingerprintTemplates.Add(b.FingerprintTemplate);
                            }
                            dbContext.SaveChanges();
                        }
                        if (!string.IsNullOrEmpty(b.Photograph?.EnrollmentId))
                        {
                            var photographs = dbContext.Photographs
                                .Where(c => c.EnrollmentId == b.Photograph.EnrollmentId).ToList();
                            if (photographs.Any())
                            {
                                var photograph = photographs[0];
                                photograph.PhotographTemplate = b.Photograph.PhotographTemplate;
                                photograph.DateLastUpdated = b.Photograph.DateLastUpdated;
                                photograph.PhotographImage = b.Photograph.PhotographImage;
                                dbContext.Entry(photograph).State = EntityState.Modified;
                            }
                            else
                            {
                                dbContext.Photographs.Add(b.Photograph);
                            }
                            dbContext.SaveChanges();
                        }

                        if (b.Signature != null && !string.IsNullOrEmpty(b.Signature.EnrollmentId))
                        {
                            var signatures = dbContext.Signature.Where(c => c.EnrollmentId == b.Signature.EnrollmentId)
                                .ToList();
                            if (signatures.Any())
                            {
                                var signature = signatures[0];
                                signature.DateLastUpdated = b.Signature.DateLastUpdated;
                                signature.SignatureImage = b.Signature.SignatureImage;
                                dbContext.Entry(signature).State = EntityState.Modified;
                            }
                            else
                            {
                                dbContext.Signature.Add(b.Signature);
                            }
                            dbContext.SaveChanges();
                        }

                        importedList.Add(b);
                    }
                });


                return importedList;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        internal static void ImportFromBackup(List<EnrollmentBackup> imports, FormMain form)
        {
            try
            {
                var backupConnectionString = "server=" + Settings.Default.DBServer + ";" +
                                             "port=" + Settings.Default.DBPort + ";" +
                                             "database=" + Settings.Default.DBName + ";" +
                                             "uid=" + Settings.Default.DBUser + ";"
                                             + "password=" + Settings.Default.DBPassword + "";
                
                imports.ForEach(b =>
                {
                    using (var conn = new MySqlConnection(backupConnectionString))
                    {
                        conn.Open();
                        var table = new DataTable();
                        new MySqlDataAdapter($"SELECT EnrollmentId from basedatas WHERE EnrollmentId='{b.BaseData.EnrollmentId}'", conn).Fill(table);

                        int basedataProcessed;
                        var middleName = !string.IsNullOrEmpty(b.BaseData.MiddleName) ? b.BaseData.MiddleName.Replace("'", "''").ToUpper() : null;
                        using (var cmd = new MySqlCommand(null, conn) { CommandTimeout = 604800 })
                        {
                            if (!(table.Rows.Count > 0))
                            {
                                var query =
                                    "INSERT INTO basedatas (EnrollmentId,ProjectCode,CreatedBy,LastUpdatedby,FormPath,ProjectSiteId,ApprovalStatus,DOB,EnrollmentDate,DateLastUpdated,Email,Firstname," +
                                    "MiddleName,Gender,Title,ProjectPrimaryCode,CuntryCode,MobileNumber,ValidIdNumber,Surname)" +
                                    " VALUES (@enrollmentid,@projectcode,@createdby,@lastupdatedby,@formpath," +
                                    "@projectsiteid,@approvalstatus,@dob,@enrollmentdate,@datelastupdated,@email," +
                                    "@firstname,@middleName,@gender,@title,@projectprimarycode,@cuntryCode,@mobilenumber,@validIdnumber,@surname)";
                                cmd.CommandText = query;
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add("@enrollmentid", MySqlDbType.String).Value = b.BaseData.EnrollmentId;
                                cmd.Parameters.Add("@projectcode", MySqlDbType.String).Value = b.BaseData.ProjectCode;
                                cmd.Parameters.Add("@createdby", MySqlDbType.String).Value = b.BaseData.CreatedBy;
                                cmd.Parameters.Add("@lastupdatedby", MySqlDbType.String).Value = b.BaseData.LastUpdatedby;
                                cmd.Parameters.Add("@formpath", MySqlDbType.String).Value = b.BaseData.FormPath;
                                cmd.Parameters.Add("@projectsiteid", MySqlDbType.String).Value = b.BaseData.ProjectSiteId;
                                cmd.Parameters.Add("@approvalstatus", MySqlDbType.String).Value = b.BaseData.ApprovalStatus;
                                cmd.Parameters.Add("@dob", MySqlDbType.String).Value = b.BaseData.DOB;
                                cmd.Parameters.Add("@enrollmentdate", MySqlDbType.DateTime).Value = b.BaseData.EnrollmentDate;
                                cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.BaseData.DateLastUpdated;
                                cmd.Parameters.Add("@email", MySqlDbType.String).Value = b.BaseData.Email;
                                cmd.Parameters.Add("@firstname", MySqlDbType.String).Value = b.BaseData.Firstname.ToUpper();
                                cmd.Parameters.Add("@middleName", MySqlDbType.String).Value = middleName;
                                cmd.Parameters.Add("@gender", MySqlDbType.String).Value = b.BaseData.Gender;
                                cmd.Parameters.Add("@title", MySqlDbType.String).Value = b.BaseData.Title;
                                cmd.Parameters.Add("@projectprimarycode", MySqlDbType.String).Value = b.BaseData.ProjectPrimaryCode;
                                cmd.Parameters.Add("@cuntryCode", MySqlDbType.String).Value = b.BaseData.CuntryCode;
                                cmd.Parameters.Add("@mobilenumber", MySqlDbType.String).Value = b.BaseData.MobileNumber;
                                cmd.Parameters.Add("@validIdnumber", MySqlDbType.String).Value = b.BaseData.ValidIdNumber;
                                cmd.Parameters.Add("@surname", MySqlDbType.String).Value = b.BaseData.Surname.ToUpper();
                                basedataProcessed = cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                var query = "UPDATE basedatas SET LastUpdatedby = lastupdatedby,ApprovalStatus = @approvalstatus,DOB=@dob," +
                                            "DateLastUpdated=@datelastupdated,Email=@email,Firstname=@firstname,MiddleName=@middleName" +
                                            ",Gender=@gender,Title=@title,CuntryCode=@cuntryCode,ProjectPrimaryCode=@projectprimarycode,ValidIdNumber=@validIdnumber" +
                                            ",Surname=@surname,MobileNumber=@mobilenumber WHERE EnrollmentId = '{b.BaseData.EnrollmentId}'";

                                cmd.CommandText = query;
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add("@lastupdatedby", MySqlDbType.String).Value = b.BaseData.LastUpdatedby;
                                cmd.Parameters.Add("@formpath", MySqlDbType.String).Value = b.BaseData.FormPath;
                                cmd.Parameters.Add("@projectsiteid", MySqlDbType.String).Value = b.BaseData.ProjectSiteId;
                                cmd.Parameters.Add("@approvalstatus", MySqlDbType.String).Value = b.BaseData.ApprovalStatus;
                                cmd.Parameters.Add("@dob", MySqlDbType.String).Value = b.BaseData.DOB;
                                cmd.Parameters.Add("@enrollmentdate", MySqlDbType.DateTime).Value = b.BaseData.EnrollmentDate;
                                cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.BaseData.DateLastUpdated;
                                cmd.Parameters.Add("@email", MySqlDbType.String).Value = b.BaseData.Email;
                                cmd.Parameters.Add("@firstname", MySqlDbType.String).Value = b.BaseData.Firstname.ToUpper();
                                cmd.Parameters.Add("@middleName", MySqlDbType.String).Value = middleName;
                                cmd.Parameters.Add("@gender", MySqlDbType.String).Value = b.BaseData.Gender;
                                cmd.Parameters.Add("@title", MySqlDbType.String).Value = b.BaseData.Title;
                                cmd.Parameters.Add("@projectprimarycode", MySqlDbType.String).Value = b.BaseData.ProjectPrimaryCode;
                                cmd.Parameters.Add("@cuntryCode", MySqlDbType.String).Value = b.BaseData.CuntryCode;
                                cmd.Parameters.Add("@mobilenumber", MySqlDbType.String).Value = b.BaseData.MobileNumber;
                                cmd.Parameters.Add("@validIdnumber", MySqlDbType.String).Value = b.BaseData.ValidIdNumber;
                                cmd.Parameters.Add("@surname", MySqlDbType.String).Value = b.BaseData.Surname.ToUpper();
                                basedataProcessed = cmd.ExecuteNonQuery();
                            }

                            if (basedataProcessed > 0)
                            {
                                if (b.CustomDatas.Any())
                                {
                                    b.CustomDatas.ToList().ForEach(c =>
                                    {
                                        var csTable = new DataTable();
                                        new MySqlDataAdapter($"SELECT CustomDataId FROM customdatas WHERE EnrollmentId = '{c.EnrollmentId}' AND CustomDataId='{c.CustomDataId}'", conn).Fill(csTable);
                                        if (!(csTable.Rows.Count > 0))
                                        {
                                            var query = "INSERT INTO customdatas (EnrollmentId,CustomDataId,ProjectSIteId,CrimsCustomData,CustomFieldId,CustomListId,ChildCrimsCustomData,DateLastUpdated)" +
                                                        " VALUES (@enrollmentid,@customdataid,@projectsiteid,@crimscustomdata,@customfieldid,@customlistid," +
                                                        "@childcrimscustomdata,@datelastupdated)";
                                            cmd.Parameters.Clear();
                                            cmd.CommandText = query;
                                            cmd.Parameters.Add("@enrollmentid", MySqlDbType.String).Value = c.EnrollmentId;
                                            cmd.Parameters.Add("@customdataid", MySqlDbType.String).Value = c.CustomDataId;
                                            cmd.Parameters.Add("@crimscustomdata", MySqlDbType.String).Value = c.CrimsCustomData;
                                            cmd.Parameters.Add("@projectsiteid", MySqlDbType.String).Value = c.ProjectSIteId;
                                            cmd.Parameters.Add("@customfieldid", MySqlDbType.String).Value = c.CustomFieldId;
                                            cmd.Parameters.Add("@customlistid", MySqlDbType.String).Value = c.CustomListId;
                                            cmd.Parameters.Add("@childcrimscustomdata", MySqlDbType.String).Value = c.ChildCrimsCustomData;
                                            cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = c.DateLastUpdated;
                                            basedataProcessed = cmd.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            var query = "UPDATE customdatas SET CrimsCustomData = @crimscustomdata,CustomFieldId=@customfieldid," +
                                                        $"CustomListId=@customlistid,ChildCrimsCustomData=@childcrimscustomdata,DateLastUpdated=@datelastupdated WHERE CustomDataId = '{c.CustomDataId}'";

                                            cmd.CommandText = query;
                                            cmd.Parameters.Clear();
                                            cmd.Parameters.Add("@crimscustomdata", MySqlDbType.String).Value = c.CrimsCustomData;
                                            cmd.Parameters.Add("@customfieldid", MySqlDbType.String).Value = c.CustomFieldId;
                                            cmd.Parameters.Add("@customlistid", MySqlDbType.String).Value = c.CustomListId;
                                            cmd.Parameters.Add("@childcrimscustomdata", MySqlDbType.String).Value = c.ChildCrimsCustomData;
                                            cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = c.DateLastUpdated;
                                            basedataProcessed = cmd.ExecuteNonQuery();
                                        }

                                    });
                                }

                                if (b.FingerprintImages != null && b.FingerprintImages.Any())
                                {
                                    b.FingerprintImages.ToList().ForEach(c =>
                                    {
                                        var fiTable = new DataTable();
                                        new MySqlDataAdapter($"SELECT EnrollmentId from fingerprintimages WHERE EnrollmentId='{c.EnrollmentId}' AND FingerIndexId='{c.FingerIndexId}'", conn).Fill(fiTable);
                                        if (!(fiTable.Rows.Count > 0))
                                        {
                                            var query = "INSERT INTO fingerprintimages (EnrollmentId,FilePath,FingerIndexId,DateLastUpdated,FingerPrintImage)" +
                                                        " VALUES (@enrollmentid,@filepath,@fingerindexid,@datelastupdated,@fingerprintimage)";
                                            cmd.CommandText = query;
                                            cmd.Parameters.Clear();
                                            cmd.Parameters.Add("@enrollmentid", MySqlDbType.String).Value = c.EnrollmentId;
                                            cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = c.FilePath;
                                            cmd.Parameters.Add("@fingerindexid", MySqlDbType.Int32).Value = c.FingerIndexId;
                                            cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = c.DateLastUpdated;
                                            cmd.Parameters.Add("@fingerprintimage", MySqlDbType.VarBinary).Value = c.FingerPrintImage;
                                            cmd.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            var query = $"UPDATE fingerprintimages SET DateLastUpdated = @datelastupdated,FingerPrintImage=@fingerprintimage,FilePath=@filepath WHERE EnrollmentId = '{c.EnrollmentId}' AND FingerIndexId='{c.FingerIndexId}'";
                                            cmd.CommandText = query;
                                            cmd.Parameters.Clear();
                                            cmd.Parameters.Add("@EnrollmentId", MySqlDbType.String).Value = c.EnrollmentId;
                                            cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = c.FilePath;
                                            cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = c.DateLastUpdated;
                                            cmd.Parameters.Add("@fingerprintimage", MySqlDbType.VarBinary).Value = c.FingerPrintImage;
                                            cmd.ExecuteNonQuery();
                                        }
                                    });
                                }

                                if (b.FingerprintReasons != null && b.FingerprintReasons.Any())
                                {
                                    b.FingerprintReasons.ToList().ForEach(c =>
                                    {
                                        var frTable = new DataTable();
                                        new MySqlDataAdapter($"SELECT EnrollmentId from fingerprintreasons WHERE EnrollmentId='{c.EnrollmentId}' AND FingerIndex='{c.FingerIndex}'", conn).Fill(frTable);
                                        if (!(frTable.Rows.Count > 0))
                                        {
                                            var query = "INSERT INTO fingerprintreasons (EnrollmentId,FingerIndex,DateLastUpdated,fingerReason)" +
                                                        $" VALUES ('{c.EnrollmentId}','{c.FingerIndex}','{c.DateLastUpdated:yyyy-MM-dd hh:mm:ss}','{c.FingerReason.Replace("'", "''")}')";
                                            cmd.CommandText = query;
                                            cmd.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            var query = $"UPDATE fingerprintreasons SET DateLastUpdated = '{c.DateLastUpdated:yyyy-MM-dd hh:mm:ss}',fingerReason='{c.FingerReason}' WHERE EnrollmentId = '{c.EnrollmentId}' AND FingerIndex='{c.FingerIndex}'";
                                            cmd.CommandText = query;
                                            cmd.ExecuteNonQuery();
                                        }

                                    });
                                }

                                if (!string.IsNullOrEmpty(b.FingerprintTemplate?.EnrollmentId))
                                {
                                    var ftTable = new DataTable();
                                    new MySqlDataAdapter($"SELECT EnrollmentId from fingerprinttemplates WHERE EnrollmentId='{b.FingerprintTemplate.EnrollmentId}'", conn).Fill(ftTable);
                                    if (!(ftTable.Rows.Count > 0))
                                    {
                                        var query = "INSERT INTO fingerprinttemplates (EnrollmentId,Template,FilePath,UniquenessStatus,DateLastUpdated)" +
                                                    " VALUES (@enrollmentId,@template,@filepath,@uniquenessstatus,@datelastupdated)";

                                        cmd.CommandText = query;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.Add("@enrollmentId", MySqlDbType.String).Value = b.FingerprintTemplate.EnrollmentId;
                                        cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = b.FingerprintTemplate.FilePath;
                                        cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.FingerprintTemplate.DateLastUpdated;
                                        cmd.Parameters.Add("@uniquenessstatus", MySqlDbType.Int32).Value = b.FingerprintTemplate.UniquenessStatus;
                                        cmd.Parameters.Add("@template", MySqlDbType.Blob).Value = b.FingerprintTemplate.Template;
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        var query = $"UPDATE fingerprinttemplates SET DateLastUpdated = @datelastupdated,Template=@template,UniquenessStatus=@uniquenessstatus WHERE EnrollmentId = '{b.FingerprintTemplate.EnrollmentId}'";
                                        cmd.CommandText = query;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = b.FingerprintTemplate.FilePath;
                                        cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.FingerprintTemplate.DateLastUpdated;
                                        cmd.Parameters.Add("@uniquenessstatus", MySqlDbType.Int32).Value = b.FingerprintTemplate.UniquenessStatus;
                                        cmd.Parameters.Add("@template", MySqlDbType.Blob).Value = b.FingerprintTemplate.Template;
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                if (!string.IsNullOrEmpty(b.Photograph?.EnrollmentId))
                                {
                                    var phTable = new DataTable();
                                    new MySqlDataAdapter($"SELECT EnrollmentId from photographs WHERE EnrollmentId='{b.Photograph.EnrollmentId}'", conn).Fill(phTable);
                                    if (!(phTable.Rows.Count > 0))
                                    {
                                        var query = "INSERT INTO photographs (EnrollmentId,PhotographId,PhotographTemplate,PhotographImagePath,DateLastUpdated,PhotographImage)" +
                                                    " VALUES (@enrollmentid,@photographid,@photographtemplate,@photographimagepath,@datelastupdated,@photographimage)";

                                        cmd.CommandText = query;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.Add("@enrollmentid", MySqlDbType.String).Value = b.Photograph.EnrollmentId;
                                        cmd.Parameters.Add("@photographimagepath", MySqlDbType.String).Value = b.Photograph.PhotographImagePath;
                                        cmd.Parameters.Add("@photographid", MySqlDbType.String).Value = b.Photograph.PhotographId;
                                        cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.Photograph.DateLastUpdated;
                                        cmd.Parameters.Add("@photographtemplate", MySqlDbType.Blob).Value = b.Photograph.PhotographTemplate;
                                        cmd.Parameters.Add("@photographimage", MySqlDbType.Blob).Value = b.Photograph.PhotographImage;
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        var query = $"UPDATE photographs SET DateLastUpdated = @datelastupdated,PhotographTemplate=@photographtemplate,PhotographImage=@photographimage WHERE EnrollmentId = '{b.Photograph.EnrollmentId}'";
                                        cmd.CommandText = query;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.Add("@photographimagepath", MySqlDbType.String).Value = b.Photograph.PhotographImagePath;
                                        cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.Photograph.DateLastUpdated;
                                        cmd.Parameters.Add("@photographtemplate", MySqlDbType.Blob).Value = b.Photograph.PhotographTemplate;
                                        cmd.Parameters.Add("@photographimage", MySqlDbType.Blob).Value = b.Photograph.PhotographImage;
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                if (!string.IsNullOrEmpty(b.Signature?.EnrollmentId))
                                {
                                    var phTable = new DataTable();
                                    new MySqlDataAdapter($"SELECT EnrollmentId from signatures WHERE EnrollmentId='{b.Signature.EnrollmentId}'", conn).Fill(phTable);
                                    if (!(phTable.Rows.Count > 0))
                                    {
                                        var query = "INSERT INTO signatures (EnrollmentId,SignatureImage,FilePath,DateLastUpdated)" +
                                                    " VALUES (@enrollmentid,@signatureimage,@filepath,@datelastupdated)";
                                        cmd.CommandText = query;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.Add("@enrollmentid", MySqlDbType.String).Value = b.Signature.EnrollmentId;
                                        cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = b.Signature.FilePath;
                                        cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.Signature.DateLastUpdated;
                                        cmd.Parameters.Add("@signatureimage", MySqlDbType.Blob).Value = b.Signature.SignatureImage;
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        var query = $"UPDATE signatures SET DateLastUpdated = @datelastupdated,SignatureImage=@signatureimage,FilePath=@filepath WHERE EnrollmentId = '{b.Signature.EnrollmentId}'";
                                        cmd.CommandText = query;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = b.Signature.FilePath;
                                        cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.Signature.DateLastUpdated;
                                        cmd.Parameters.Add("@signatureimage", MySqlDbType.Blob).Value = b.Signature.SignatureImage;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            form.UpdateUi(1);
                        }
                    }

                });

            }
            catch (Exception ex)
            {
                var msg = !string.IsNullOrEmpty(ex.InnerException?.Message) ? ex.InnerException.Message : ex.Message;
                MessageBox.Show(msg, @"Data Backup");
                form.UpdateActivityStatus(@"An error was encountered. Waiting for user action...");
            }

        }
        #endregion
        internal static int SynchroniseDataToServer(EnrollmentRecord b)
        {
            try
            {
                using (var conn = new MySqlConnection(syncConnectionString))
                {
                    conn.Open();
                    var table = new DataTable();
                    new MySqlDataAdapter($"SELECT EnrollmentId from basedatas WHERE EnrollmentId='{b.BaseData.EnrollmentId}'", conn).Fill(table);

                    int basedataProcessed;
                    var middleName = !string.IsNullOrEmpty(b.BaseData.MiddleName) ? b.BaseData.MiddleName.ToUpper() : null;
                    using (var cmd = new MySqlCommand(null, conn) { CommandTimeout = 604800 })
                    {
                        if (!(table.Rows.Count > 0))
                        {
                            var query =
                                "INSERT INTO basedatas (EnrollmentId,ProjectCode,CreatedBy,LastUpdatedby,FormPath,ProjectSiteId,ApprovalStatus,DOB,EnrollmentDate,DateLastUpdated,Email,Firstname," +
                                "MiddleName,Gender,Title,ProjectPrimaryCode,CuntryCode,MobileNumber,ValidIdNumber,Surname)" +
                                " VALUES (@enrollmentid,@projectcode,@createdby,@lastupdatedby,@formpath," +
                                "@projectsiteid,@approvalstatus,@dob,@enrollmentdate,@datelastupdated,@email," +
                                "@firstname,@middleName,@gender,@title,@projectprimarycode,@cuntryCode,@mobilenumber,@validIdnumber,@surname)";
                            cmd.CommandText = query;
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add("@enrollmentid", MySqlDbType.String).Value = b.BaseData.EnrollmentId;
                            cmd.Parameters.Add("@projectcode", MySqlDbType.String).Value = b.BaseData.ProjectCode;
                            cmd.Parameters.Add("@createdby", MySqlDbType.String).Value = b.BaseData.CreatedBy;
                            cmd.Parameters.Add("@lastupdatedby", MySqlDbType.String).Value = b.BaseData.LastUpdatedby;
                            cmd.Parameters.Add("@formpath", MySqlDbType.String).Value = b.BaseData.FormPath;
                            cmd.Parameters.Add("@projectsiteid", MySqlDbType.String).Value = b.BaseData.ProjectSiteId;
                            cmd.Parameters.Add("@approvalstatus", MySqlDbType.String).Value = b.BaseData.ApprovalStatus;
                            cmd.Parameters.Add("@dob", MySqlDbType.String).Value = b.BaseData.DOB;
                            cmd.Parameters.Add("@enrollmentdate", MySqlDbType.DateTime).Value = b.BaseData.EnrollmentDate;
                            cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.BaseData.DateLastUpdated;
                            cmd.Parameters.Add("@email", MySqlDbType.String).Value = b.BaseData.Email;
                            cmd.Parameters.Add("@firstname", MySqlDbType.String).Value = b.BaseData.Firstname.ToUpper();
                            cmd.Parameters.Add("@middleName", MySqlDbType.String).Value = middleName;
                            cmd.Parameters.Add("@gender", MySqlDbType.String).Value = b.BaseData.Gender;
                            cmd.Parameters.Add("@title", MySqlDbType.String).Value = b.BaseData.Title;
                            cmd.Parameters.Add("@projectprimarycode", MySqlDbType.String).Value = b.BaseData.ProjectPrimaryCode;
                            cmd.Parameters.Add("@cuntryCode", MySqlDbType.String).Value = b.BaseData.CuntryCode;
                            cmd.Parameters.Add("@mobilenumber", MySqlDbType.String).Value = b.BaseData.MobileNumber;
                            cmd.Parameters.Add("@validIdnumber", MySqlDbType.String).Value = b.BaseData.ValidIdNumber;
                            cmd.Parameters.Add("@surname", MySqlDbType.String).Value = b.BaseData.Surname.ToUpper();
                            basedataProcessed = cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            var query = "UPDATE basedatas SET LastUpdatedby = lastupdatedby,ApprovalStatus = @approvalstatus,DOB=@dob," +
                                        "DateLastUpdated=@datelastupdated,Email=@email,Firstname=@firstname,MiddleName=@middleName" +
                                        ",Gender=@gender,Title=@title,CuntryCode=@cuntryCode,ProjectPrimaryCode=@projectprimarycode,ValidIdNumber=@validIdnumber" +
                                        ",Surname=@surname,MobileNumber=@mobilenumber WHERE EnrollmentId = '{b.BaseData.EnrollmentId}'";

                            cmd.CommandText = query;
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add("@lastupdatedby", MySqlDbType.String).Value = b.BaseData.LastUpdatedby;
                            cmd.Parameters.Add("@formpath", MySqlDbType.String).Value = b.BaseData.FormPath;
                            cmd.Parameters.Add("@projectsiteid", MySqlDbType.String).Value = b.BaseData.ProjectSiteId;
                            cmd.Parameters.Add("@approvalstatus", MySqlDbType.String).Value = b.BaseData.ApprovalStatus;
                            cmd.Parameters.Add("@dob", MySqlDbType.String).Value = b.BaseData.DOB;
                            cmd.Parameters.Add("@enrollmentdate", MySqlDbType.DateTime).Value = b.BaseData.EnrollmentDate;
                            cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.BaseData.DateLastUpdated;
                            cmd.Parameters.Add("@email", MySqlDbType.String).Value = b.BaseData.Email;
                            cmd.Parameters.Add("@firstname", MySqlDbType.String).Value = b.BaseData.Firstname.ToUpper();
                            cmd.Parameters.Add("@middleName", MySqlDbType.String).Value = middleName;
                            cmd.Parameters.Add("@gender", MySqlDbType.String).Value = b.BaseData.Gender;
                            cmd.Parameters.Add("@title", MySqlDbType.String).Value = b.BaseData.Title;
                            cmd.Parameters.Add("@projectprimarycode", MySqlDbType.String).Value = b.BaseData.ProjectPrimaryCode;
                            cmd.Parameters.Add("@cuntryCode", MySqlDbType.String).Value = b.BaseData.CuntryCode;
                            cmd.Parameters.Add("@mobilenumber", MySqlDbType.String).Value = b.BaseData.MobileNumber;
                            cmd.Parameters.Add("@validIdnumber", MySqlDbType.String).Value = b.BaseData.ValidIdNumber;
                            cmd.Parameters.Add("@surname", MySqlDbType.String).Value = b.BaseData.Surname.ToUpper();
                            basedataProcessed = cmd.ExecuteNonQuery();
                        }

                        if (basedataProcessed > 0)
                        {
                            if (b.CustomDatas.Any())
                            {
                                b.CustomDatas.ToList().ForEach(c =>
                                {
                                    var csTable = new DataTable();
                                    new MySqlDataAdapter($"SELECT CustomDataId FROM customdatas WHERE EnrollmentId = '{c.EnrollmentId}' AND CustomDataId='{c.CustomDataId}'", conn).Fill(csTable);
                                    if (!(csTable.Rows.Count > 0))
                                    {
                                        var query = "INSERT INTO customdatas (EnrollmentId,CustomDataId,ProjectSIteId,CrimsCustomData,CustomFieldId,CustomListId,ChildCrimsCustomData,DateLastUpdated)" +
                                                    " VALUES (@enrollmentid,@customdataid,@projectsiteid,@crimscustomdata,@customfieldid,@customlistid," +
                                                    "@childcrimscustomdata,@datelastupdated)";
                                        cmd.Parameters.Clear();
                                        cmd.CommandText = query;
                                        cmd.Parameters.Add("@enrollmentid", MySqlDbType.String).Value = c.EnrollmentId;
                                        cmd.Parameters.Add("@customdataid", MySqlDbType.String).Value = c.CustomDataId;
                                        cmd.Parameters.Add("@crimscustomdata", MySqlDbType.String).Value = c.CrimsCustomData;
                                        cmd.Parameters.Add("@projectsiteid", MySqlDbType.String).Value = c.ProjectSIteId;
                                        cmd.Parameters.Add("@customfieldid", MySqlDbType.String).Value = c.CustomFieldId;
                                        cmd.Parameters.Add("@customlistid", MySqlDbType.String).Value = c.CustomListId;
                                        cmd.Parameters.Add("@childcrimscustomdata", MySqlDbType.String).Value = c.ChildCrimsCustomData;
                                        cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = c.DateLastUpdated;
                                        basedataProcessed = cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        var query = "UPDATE customdatas SET CrimsCustomData = @crimscustomdata,CustomFieldId=@customfieldid," +
                                                    $"CustomListId=@customlistid,ChildCrimsCustomData=@childcrimscustomdata,DateLastUpdated=@datelastupdated WHERE CustomDataId = '{c.CustomDataId}'";

                                        cmd.CommandText = query;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.Add("@crimscustomdata", MySqlDbType.String).Value = c.CrimsCustomData;
                                        cmd.Parameters.Add("@customfieldid", MySqlDbType.String).Value = c.CustomFieldId;
                                        cmd.Parameters.Add("@customlistid", MySqlDbType.String).Value = c.CustomListId;
                                        cmd.Parameters.Add("@childcrimscustomdata", MySqlDbType.String).Value = c.ChildCrimsCustomData;
                                        cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = c.DateLastUpdated;
                                        basedataProcessed = cmd.ExecuteNonQuery();
                                    }

                                });
                            }

                            if (b.FingerprintImages != null && b.FingerprintImages.Any())
                            {
                                b.FingerprintImages.ToList().ForEach(c =>
                                {
                                    var fiTable = new DataTable();
                                    new MySqlDataAdapter($"SELECT EnrollmentId from fingerprintimages WHERE EnrollmentId='{c.EnrollmentId}' AND FingerIndexId='{c.FingerIndexId}'", conn).Fill(fiTable);
                                    if (!(fiTable.Rows.Count > 0))
                                    {
                                        var query = "INSERT INTO fingerprintimages (EnrollmentId,FilePath,FingerIndexId,DateLastUpdated,FingerPrintImage)" +
                                                    " VALUES (@enrollmentid,@filepath,@fingerindexid,@datelastupdated,@fingerprintimage)";
                                        cmd.CommandText = query;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.Add("@enrollmentid", MySqlDbType.String).Value = c.EnrollmentId;
                                        cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = c.FilePath;
                                        cmd.Parameters.Add("@fingerindexid", MySqlDbType.Int32).Value = c.FingerIndexId;
                                        cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = c.DateLastUpdated;
                                        cmd.Parameters.Add("@fingerprintimage", MySqlDbType.VarBinary).Value = c.FingerPrintImage;
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        var query = $"UPDATE fingerprintimages SET DateLastUpdated = @datelastupdated,FingerPrintImage=@fingerprintimage,FilePath=@filepath WHERE EnrollmentId = '{c.EnrollmentId}' AND FingerIndexId='{c.FingerIndexId}'";
                                        cmd.CommandText = query;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.Add("@EnrollmentId", MySqlDbType.String).Value = c.EnrollmentId;
                                        cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = c.FilePath;
                                        cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = c.DateLastUpdated;
                                        cmd.Parameters.Add("@fingerprintimage", MySqlDbType.VarBinary).Value = c.FingerPrintImage;
                                        cmd.ExecuteNonQuery();
                                    }
                                });
                            }

                            if (b.FingerprintReasons != null && b.FingerprintReasons.Any())
                            {
                                b.FingerprintReasons.ToList().ForEach(c =>
                                {
                                    var frTable = new DataTable();
                                    new MySqlDataAdapter($"SELECT EnrollmentId from fingerprintreasons WHERE EnrollmentId='{c.EnrollmentId}' AND FingerIndex='{c.FingerIndex}'", conn).Fill(frTable);
                                    if (!(frTable.Rows.Count > 0))
                                    {
                                        var query = "INSERT INTO fingerprintreasons (EnrollmentId,FingerIndex,DateLastUpdated,FingerReason)" +
                                                    $" VALUES ('{c.EnrollmentId}','{c.FingerIndex}','{c.DateLastUpdated:yyyy-MM-dd hh:mm:ss}','{c.FingerReason.Replace("'", "''")}')";
                                        cmd.CommandText = query;
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        var query = $"UPDATE fingerprintreasons SET DateLastUpdated = '{c.DateLastUpdated:yyyy-MM-dd hh:mm:ss}',FingerReason='{c.FingerReason}' WHERE EnrollmentId = '{c.EnrollmentId}' AND FingerIndex='{c.FingerIndex}'";
                                        cmd.CommandText = query;
                                        cmd.ExecuteNonQuery();
                                    }

                                });
                            }

                            if (!string.IsNullOrEmpty(b.FingerprintTemplate?.EnrollmentId))
                            {
                                var ftTable = new DataTable();
                                new MySqlDataAdapter($"SELECT EnrollmentId from fingerprinttemplates WHERE EnrollmentId='{b.FingerprintTemplate.EnrollmentId}'", conn).Fill(ftTable);
                                if (!(ftTable.Rows.Count > 0))
                                {
                                    var query = "INSERT INTO fingerprinttemplates (EnrollmentId,Template,FilePath,UniquenessStatus,DateLastUpdated)" +
                                                " VALUES (@enrollmentId,@template,@filepath,@uniquenessstatus,@datelastupdated)";

                                    cmd.CommandText = query;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@enrollmentId", MySqlDbType.String).Value = b.FingerprintTemplate.EnrollmentId;
                                    cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = b.FingerprintTemplate.FilePath;
                                    cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.FingerprintTemplate.DateLastUpdated;
                                    cmd.Parameters.Add("@uniquenessstatus", MySqlDbType.Int32).Value = b.FingerprintTemplate.UniquenessStatus;
                                    cmd.Parameters.Add("@template", MySqlDbType.Blob).Value = b.FingerprintTemplate.Template;
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    var query = $"UPDATE fingerprinttemplates SET DateLastUpdated = @datelastupdated,Template=@template,UniquenessStatus=@uniquenessstatus WHERE EnrollmentId = '{b.FingerprintTemplate.EnrollmentId}'";
                                    cmd.CommandText = query;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = b.FingerprintTemplate.FilePath;
                                    cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.FingerprintTemplate.DateLastUpdated;
                                    cmd.Parameters.Add("@uniquenessstatus", MySqlDbType.Int32).Value = b.FingerprintTemplate.UniquenessStatus;
                                    cmd.Parameters.Add("@template", MySqlDbType.Blob).Value = b.FingerprintTemplate.Template;
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            if (!string.IsNullOrEmpty(b.Photograph?.EnrollmentId))
                            {
                                var phTable = new DataTable();
                                new MySqlDataAdapter($"SELECT EnrollmentId from photographs WHERE EnrollmentId='{b.Photograph.EnrollmentId}'", conn).Fill(phTable);
                                if (!(phTable.Rows.Count > 0))
                                {
                                    var query = "INSERT INTO photographs (EnrollmentId,PhotographId,PhotographTemplate,PhotographImagePath,DateLastUpdated,PhotographImage)" +
                                                " VALUES (@enrollmentid,@photographid,@photographtemplate,@photographimagepath,@datelastupdated,@photographimage)";

                                    cmd.CommandText = query;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@enrollmentid", MySqlDbType.String).Value = b.Photograph.EnrollmentId;
                                    cmd.Parameters.Add("@photographimagepath", MySqlDbType.String).Value = b.Photograph.PhotographImagePath;
                                    cmd.Parameters.Add("@photographid", MySqlDbType.String).Value = b.Photograph.PhotographId;
                                    cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.Photograph.DateLastUpdated;
                                    cmd.Parameters.Add("@photographtemplate", MySqlDbType.Blob).Value = b.Photograph.PhotographTemplate;
                                    cmd.Parameters.Add("@photographimage", MySqlDbType.Blob).Value = b.Photograph.PhotographImage;
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    var query = $"UPDATE photographs SET DateLastUpdated = @datelastupdated,PhotographTemplate=@photographtemplate,PhotographImage=@photographimage WHERE EnrollmentId = '{b.Photograph.EnrollmentId}'";
                                    cmd.CommandText = query;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@photographimagepath", MySqlDbType.String).Value = b.Photograph.PhotographImagePath;
                                    cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.Photograph.DateLastUpdated;
                                    cmd.Parameters.Add("@photographtemplate", MySqlDbType.Blob).Value = b.Photograph.PhotographTemplate;
                                    cmd.Parameters.Add("@photographimage", MySqlDbType.Blob).Value = b.Photograph.PhotographImage;
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            if (!string.IsNullOrEmpty(b.Signature?.EnrollmentId))
                            {
                                var phTable = new DataTable();
                                new MySqlDataAdapter($"SELECT EnrollmentId from signatures WHERE EnrollmentId='{b.Signature.EnrollmentId}'", conn).Fill(phTable);
                                if (!(phTable.Rows.Count > 0))
                                {
                                    var query = "INSERT INTO signatures (EnrollmentId,SignatureImage,FilePath,DateLastUpdated)" +
                                                " VALUES (@enrollmentid,@signatureimage,@filepath,@datelastupdated)";
                                    cmd.CommandText = query;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@enrollmentid", MySqlDbType.String).Value = b.Signature.EnrollmentId;
                                    cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = b.Signature.FilePath;
                                    cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.Signature.DateLastUpdated;
                                    cmd.Parameters.Add("@signatureimage", MySqlDbType.Blob).Value = b.Signature.SignatureImage;
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    var query = $"UPDATE signatures SET DateLastUpdated = @datelastupdated,SignatureImage=@signatureimage,FilePath=@filepath WHERE EnrollmentId = '{b.Signature.EnrollmentId}'";
                                    cmd.CommandText = query;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = b.Signature.FilePath;
                                    cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.Signature.DateLastUpdated;
                                    cmd.Parameters.Add("@signatureimage", MySqlDbType.Blob).Value = b.Signature.SignatureImage;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Data Synchronisation", !string.IsNullOrEmpty(ex.InnerException?.Message) ? ex.InnerException.Message : ex.Message);
                return 0;
            }
        }
        internal static bool SynchroniseFingerprintReasons(List<FingerprintReason> fingerprintReasons)
        {
            try
            {
                using (var dbContext = new CrimsDbContext(syncConnectionString))
                {
                    fingerprintReasons.ForEach(f =>
                    {
                        var fingerReasons = dbContext.FingerprintReasons.Where(r => r.EnrollmentId == f.EnrollmentId && r.FingerIndex == f.FingerIndex).ToList();
                        if (fingerReasons.Any())
                        {
                            var reason = fingerReasons[0];
                            reason.FingerReason = f.FingerReason;
                            reason.DateLastUpdated = DateTime.Now;
                            reason.ObjectState = ObjectState.Modified;
                        }
                        else
                        {
                            dbContext.FingerprintReasons.Add(f);
                        }
                    });
                    
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Synchronise Fingerprint Reasons");
                return false;
            }
        }
        internal static int SyncApprovals(Project project)
        {
            try
            {
                List<string> enrollmentIds;
                var approvalRecords = new List<ApprovalRecord>();

                var pending = (int)EnumManager.ApprovalStatus.Pending;
                using (var dbContext = new CrimsDbContext(localConnectionString))
                {
                    enrollmentIds = (from b in dbContext.BaseDatas.Where(d => d.ProjectCode == project.ProjectCode && d.ApprovalStatus == pending)
                                     select b.EnrollmentId).ToList();
                }

                if (!enrollmentIds.Any())
                {
                    return -1;
                }

                using (var dbContext = new CrimsDbContext(syncConnectionString))
                {
                    enrollmentIds.ForEach(i =>
                    {
                        var approvals = dbContext.Approvals.Where(x => x.EnrollmentId == i).ToList();
                        if (approvals.Any())
                        {
                            var baseDatas = dbContext.BaseDatas.Where(x => x.EnrollmentId == i).ToList();
                            if (baseDatas.Any())
                            {
                                var approvalRecord = new ApprovalRecord
                                {
                                    ApprovalStatus = baseDatas[0].ApprovalStatus,
                                    EnrollmentId = baseDatas[0].EnrollmentId,
                                    Approvals = approvals
                                };
                                approvalRecords.Add(approvalRecord);
                            }
                        }
                    });
                }

                if (approvalRecords.Any())
                {
                    using (var dbContext = new CrimsDbContext(localConnectionString))
                    {
                        approvalRecords.ForEach(a =>
                        {
                            var baseDataList = dbContext.BaseDatas.Where(r => r.EnrollmentId == a.EnrollmentId).ToList();
                            if (baseDataList.Any())
                            {
                                var c = baseDataList[0];
                                c.ApprovalStatus = a.ApprovalStatus;
                                dbContext.Entry(c).State = EntityState.Modified;
                                dbContext.Approvals.AddRange(a.Approvals);
                            }
                        });

                        dbContext.SaveChanges();
                    }
                }
                return approvalRecords.Count;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        internal static IList<CustomData> GetEnrolleeCustomData(string enrollmentId)
        {
            using (var dbContext = new CrimsDbContext(localConnectionString))
            {
                return dbContext.CustomDatas.Where(x => x.EnrollmentId == enrollmentId).ToList();
            }
        }
        internal static IList<FingerprintReason> GetFingerprintReasons(string enrollmentId)
        {
            using (var dbContext = new CrimsDbContext(localConnectionString))
            {
                return dbContext.FingerprintReasons.Where(x => x.EnrollmentId == enrollmentId).ToList();
            }
        }
        internal static IList<FingerprintImage> GetEnrolleeFingerprintImages(string enrollmentId)
        {
            using (var dbContext = new CrimsDbContext(localConnectionString))
            {
                return dbContext.FingerprintImages.Where(x => x.EnrollmentId == enrollmentId).ToList();
            }
        }
        internal static FingerprintTemplate GetEnrolleeFingerprintTemplate(string enrollmentId)
        {
            using (var dbContext = new CrimsDbContext(localConnectionString))
            {
                var templates = dbContext.FingerprintTemplates.Where(x => x.EnrollmentId == enrollmentId).ToList();
                if (!templates.Any())
                {
                    return new FingerprintTemplate();
                }
                return templates[0];
            }
        }
        internal static Signature GetEnrolleeSignature(string enrollmentId)
        {
            using (var dbContext = new CrimsDbContext(localConnectionString))
            {
                var signatures = dbContext.Signature.Where(x => x.EnrollmentId == enrollmentId).ToList();
                if (!signatures.Any())
                {
                    return new Signature();
                }
                return signatures[0];
            }
        }
        internal static Photograph GetEnrolleePhotoGraph(string enrollmentId)
        {
            using (var dbContext = new CrimsDbContext(localConnectionString))
            {
                var photographs = dbContext.Photographs.Where(x => x.EnrollmentId == enrollmentId).ToList();
                if (!photographs.Any())
                {
                    return new Photograph();
                }
                return photographs[0];
            }
        }
        internal static void SaveBiometricsRecordToDb(BiometricsRecord biometricsRecord, List<FingerprintReason> fingerprintReasons, string connectionString)
        {
            try
            {
                var _nFTemplate = new NFTemplate();
                using (var dbContext = new CrimsDbContext(connectionString))
                {
                    //Save User Signature
                    if (biometricsRecord.Signature != null)
                    {
                        var signature = dbContext.Signature.FirstOrDefault(x => x.EnrollmentId == biometricsRecord.EnrollmentId);
                        if (signature == null)
                        {
                            var ms = new MemoryStream();
                            biometricsRecord.Signature.Save(ms, ImageFormat.Jpeg);
                            dbContext.Signature.Add(new Signature
                            {
                                SignatureImage = GetImageMemoryStream(biometricsRecord.Signature, System.Drawing.Imaging.ImageFormat.Jpeg),
                                DateLastUpdated = DateTime.Now,
                                EnrollmentId = biometricsRecord.EnrollmentId,
                                FilePath = biometricsRecord.EnrollmentId + "\\" + "sign_image.jpeg"
                            });
                        }
                        else
                        {
                            signature.SignatureImage = GetImageMemoryStream(biometricsRecord.Signature, System.Drawing.Imaging.ImageFormat.Jpeg);
                            signature.DateLastUpdated = DateTime.Now;
                            signature.EnrollmentId = biometricsRecord.EnrollmentId;
                            signature.FilePath = biometricsRecord.EnrollmentId + "\\" + "sign_image.jpeg";
                            signature.ObjectState = Repository.Pattern.Infrastructure.ObjectState.Modified;
                        }
                        dbContext.SaveChanges();
                    }

                    //Save User Photo & Template
                    if (biometricsRecord.Photograph != null)
                    {
                        var photpgraph = dbContext.Photographs.FirstOrDefault(x => x.EnrollmentId == biometricsRecord.EnrollmentId);
                        if (photpgraph == null)
                        {
                            dbContext.Photographs.Add(new Photograph
                            {
                                PhotographImage = GetImageMemoryStream(biometricsRecord.Photograph, System.Drawing.Imaging.ImageFormat.Jpeg),
                                DateLastUpdated = DateTime.Now,
                                EnrollmentId = biometricsRecord.EnrollmentId,
                                PhotographImagePath = biometricsRecord.EnrollmentId + "\\" + "photo_image.jpeg",
                                PhotographId = Guid.NewGuid().ToString(),
                                PhotographTemplate = biometricsRecord.PhotographTemplate

                            });
                        }
                        else
                        {
                            photpgraph.PhotographImage = GetImageMemoryStream(biometricsRecord.Photograph, System.Drawing.Imaging.ImageFormat.Jpeg);
                            photpgraph.DateLastUpdated = DateTime.Now;
                            photpgraph.EnrollmentId = biometricsRecord.EnrollmentId;
                            photpgraph.PhotographImagePath = biometricsRecord.EnrollmentId + "\\" + "photo_image.jpeg";
                            photpgraph.PhotographId = Guid.NewGuid().ToString();
                            photpgraph.PhotographTemplate = biometricsRecord.PhotographTemplate;
                            photpgraph.ObjectState = Repository.Pattern.Infrastructure.ObjectState.Modified;
                        }
                        dbContext.SaveChanges();
                    }

                    //save Fingerprint Images
                    if (biometricsRecord.FingerprintRecords.Count > 0)
                    {
                        var fingerprintImages = dbContext.FingerprintImages.Where(x => x.EnrollmentId == biometricsRecord.EnrollmentId).ToList();
                        foreach (var userFingerprint in biometricsRecord.FingerprintRecords)
                        {
                            //Check if Finger Image Exist and Add new or Update
                            var fingerImage = fingerprintImages.FirstOrDefault(x => x.FingerIndexId == userFingerprint.FingerIndex);

                            if (fingerImage == null)
                            {
                                dbContext.FingerprintImages.Add(new FingerprintImage
                                {
                                    EnrollmentId = biometricsRecord.EnrollmentId,
                                    DateLastUpdated = DateTime.Now,
                                    FilePath = biometricsRecord.EnrollmentId + "\\" + userFingerprint.FingerDescription + ".jpg",
                                    FingerIndexId = userFingerprint.FingerIndex,
                                    FingerPrintImage = userFingerprint.FingerWsq //GetImageMemoryStream(userFingerprint.fingerImage, System.Drawing.Imaging.ImageFormat.Jpeg)

                                });
                            }
                            else
                            {
                                fingerImage.EnrollmentId = biometricsRecord.EnrollmentId;
                                fingerImage.DateLastUpdated = DateTime.Now;
                                fingerImage.FilePath = biometricsRecord.EnrollmentId + "\\" + userFingerprint.FingerDescription + ".jpg";
                                fingerImage.FingerIndexId = userFingerprint.FingerIndex;
                                fingerImage.FingerPrintImage = userFingerprint.FingerWsq; //GetImageMemoryStream(userFingerprint.fingerImage, System.Drawing.Imaging.ImageFormat.Jpeg);

                            }

                            //Add FingerTemplate to NFTemplate
                            //Build Grouped Finger Template from Available Finger Images
                            
                            if (userFingerprint.FingerRecord != null)
                            {
                                _nFTemplate.Records.Add(userFingerprint.FingerRecord);
                            }
                            else
                            {
                                var wsq = new NBuffer(userFingerprint.FingerWsq);

                                //var image = NImage.FromMemory(wsq, NImageFormat.Wsq).ToBitmap();

                                //NFinger finger, NBuffer fingerTemplate
                                using (var biometricClient = new NBiometricClient())
                                using (var subject = new NSubject())
                                using (var finger = new NFinger())
                                {
                                    //Read finger image from enrollment and add it to NFinger object
                                    finger.Image = NImage.FromMemory(wsq, NImageFormat.Wsq);
                                    //add NFinger object to NSubject
                                    subject.Fingers.Add(finger);
                                    ////Set finger template size (recommended, for enroll to database, is large) (optional)
                                    //biometricClient.FingersTemplateSize = NTemplateSize.Large;

                                    //Create template from added finger image
                                    var status = biometricClient.CreateTemplate(subject);
                                    if (status == NBiometricStatus.Ok)
                                    {
                                        userFingerprint.FingerRecord = finger.Objects[0].Template;
                                        _nFTemplate.Records.Add(finger.Objects[0].Template);
                                    }
                                }
                                //NLicense.ReleaseComponents("Biometrics.FingerExtraction");
                             }
                        }

                        // var buffArray = fingerprintTemplate.ToArray();

                        //Save Fingerprint Template
                        var fingerprintTemplates = dbContext.FingerprintTemplates.FirstOrDefault(x => x.EnrollmentId == biometricsRecord.EnrollmentId);

                        //Pack Grouped FingerTemplates
                        var ms = new NMemoryStream();
                        _nFTemplate.Save(ms);
                        biometricsRecord.FingerTemplates = ms.ToArray();

                        if (fingerprintTemplates == null)
                        {
                            dbContext.FingerprintTemplates.Add(new FingerprintTemplate
                            {
                                EnrollmentId = biometricsRecord.EnrollmentId,
                                DateLastUpdated = DateTime.Now,
                                FilePath = biometricsRecord.EnrollmentId + "\\fingertemplates.tem",
                                Template = biometricsRecord.FingerTemplates,
                                UniquenessStatus = 0
                            });
                        }
                        else
                        {
                            fingerprintTemplates.EnrollmentId = biometricsRecord.EnrollmentId;
                            fingerprintTemplates.DateLastUpdated = DateTime.Now;
                            fingerprintTemplates.FilePath = biometricsRecord.EnrollmentId + "\\fingertemplates.tem";
                            fingerprintTemplates.Template = biometricsRecord.FingerTemplates;
                            fingerprintTemplates.ObjectState = Repository.Pattern.Infrastructure.ObjectState.Modified;
                        }

                        if (fingerprintReasons.Any())
                        {
                            var savedReasons = dbContext.FingerprintReasons.Where(x => x.EnrollmentId == biometricsRecord.EnrollmentId).ToList();
                            if (savedReasons.Any())
                            {
                                fingerprintReasons.ForEach(f =>
                                {
                                    var updatedReason = savedReasons.Find(t => t.FingerIndex == f.FingerIndex);
                                    if (updatedReason != null)
                                    {
                                        updatedReason.FingerReason = f.FingerReason;
                                        updatedReason.DateLastUpdated = DateTime.Now;
                                        updatedReason.ObjectState = Repository.Pattern.Infrastructure.ObjectState.Modified;
                                    }  
                                    else
                                    {
                                        dbContext.FingerprintReasons.Add(f);
                                    }
                                });
                            }
                            else
                            {
                                dbContext.FingerprintReasons.AddRange(fingerprintReasons);
                            }
                        }

                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        internal static BiometricsRecord GetLattestRecord(Project project, UserAccountModel userProfile)
        {
            BiometricsRecord biometricsRecord = null;
            using (MySqlConnection conn = new MySqlConnectionManager().DbConnection())
            {
                DataTable result = new DataTable();
                string query = $"SELECT * FROM basedatas where t.ProjectCode ='{project.ProjectCode}' AND CreatedBy='{ userProfile.ProfileId}' ORDER BY TableId DESC Limit 1";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                adapter.Fill(result);

                if (result.Rows.Count > 0)
                {
                    DataRow resRow = result.Rows[0];
                    biometricsRecord = FillBiometricsRecord(resRow);
                }
            }

            return biometricsRecord;
        }
        internal static BiometricsRecord FindRecord(Project project, string recordId, UserAccountModel userProfile)
        {
            BiometricsRecord biometricsRecord = null;
            using (MySqlConnection conn = new MySqlConnectionManager().DbConnection())
            {
                DataTable result = new DataTable();
                string query = $"SELECT * FROM basedatas where t.ProjectCode ='{project.ProjectCode}' AND CreatedBy='{userProfile.ProfileId}' and EnrollmentId = '{recordId}' Limit 1";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                adapter.Fill(result);

                if (result.Rows.Count > 0)
                {
                    DataRow resRow = result.Rows[0];
                    biometricsRecord = FillBiometricsRecord(resRow);
                }

            }
            return biometricsRecord;
        }
        internal static BiometricsRecord PreviousRecord(Project project, string tableId, UserAccountModel userProfile)
        {
            BiometricsRecord biometricsRecord = null;
            using (MySqlConnection conn = new MySqlConnectionManager().DbConnection())
            {
                DataTable result = new DataTable();
                string query = $"SELECT * FROM basedatas  where t.ProjectCode ='{project.ProjectCode}' AND CreatedBy='{userProfile.ProfileId}' and tableId < {tableId} Limit 1";

                var adapter = new MySqlDataAdapter(query, conn);
                adapter.Fill(result);

                if (result.Rows.Count > 0)
                {
                    DataRow resRow = result.Rows[0];
                    biometricsRecord = FillBiometricsRecord(resRow);
                }
            }

            return biometricsRecord;
        }
        internal static BiometricsRecord NextRecord(Project project, string tableId, UserAccountModel userProfile)
        {
            BiometricsRecord biometricsRecord = null;
            using (var conn = new MySqlConnectionManager().DbConnection())
            {
                var result = new DataTable();
                string query = $"SELECT * FROM basedatas  where t.ProjectCode ='{project.ProjectCode}' AND CreatedBy='{userProfile.ProfileId}' and tableId > {tableId} Limit 1";

                var adapter = new MySqlDataAdapter(query, conn);
                adapter.Fill(result);

                if (result.Rows.Count > 0)
                {
                    DataRow resRow = result.Rows[0];
                    biometricsRecord = FillBiometricsRecord(resRow);
                }
            }

            return biometricsRecord;
        }
        internal static BiometricsRecord GetPreviousRecordFromDb(UserAccountModel userProfile)
        {
            BiometricsRecord biometricsRecord = null;

            return biometricsRecord;
        }
        internal static BiometricsRecord GetNextFirstRecordFromDb(UserAccountModel userProfile)
        {
            BiometricsRecord biometricsRecord = null;

            return biometricsRecord;
        }
        internal static BiometricsRecord FindRecordFromDb(UserAccountModel userProfile)
        {
            BiometricsRecord biometricsRecord = null;

            return biometricsRecord;
        }
        internal static BiometricsRecord GetDisplayRecordFromDb(Project project, UserAccountModel userProfile, DisplayRecordPosition position, BiometricsRecord currentBiometricsRecord, out List<FingerprintReason> reasons)
        {
            var biometricsRecord = new BiometricsRecord();
            var fingerReasons = new List<FingerprintReason>();
            try
            {
                //Obtain Fingerprint Components Licenses
                try
                {
                    NLicense.ObtainComponents(Address, Port, FingerprintComponents);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    reasons = fingerReasons;
                    return new BiometricsRecord();
                }
                
                using (CrimsDbContext dbContext = new CrimsDbContext(localConnectionString))
                {
                    BaseData baseData = null;

                    switch (position)
                    {
                        case DisplayRecordPosition.lattest:
                            baseData = dbContext.BaseDatas.Where(x => x.ProjectCode == project.ProjectCode && x.CreatedBy == userProfile.ProfileId)
                                .Include("FingerprintReasons").Include("FingerprintImages")
                                .Include("Photographs").Include("Signatures").Include("FingerprintTemplates")
                                .OrderByDescending(y => y.DateLastUpdated).FirstOrDefault();
                            break;
                        case DisplayRecordPosition.first:
                            baseData = dbContext.BaseDatas.Where((x => x.ProjectCode == project.ProjectCode && x.CreatedBy == userProfile.ProfileId))
                                .Include("FingerprintReasons").Include("FingerprintImages")
                                .Include("Photographs").Include("Signatures").Include("FingerprintTemplates")
                                .OrderBy(x => x.EnrollmentDate).Take(1).FirstOrDefault();
                            break;
                        case DisplayRecordPosition.next:
                            baseData = dbContext.BaseDatas.Where(x => x.ProjectCode == project.ProjectCode && x.CreatedBy == userProfile.ProfileId && x.EnrollmentDate > currentBiometricsRecord.EnrollmentDate)
                                .Include("FingerprintReasons").Include("FingerprintImages")
                                .Include("Photographs").Include("Signatures").Include("FingerprintTemplates")
                                .OrderBy(x => x.EnrollmentDate).Take(1).FirstOrDefault();
                            break;
                        case DisplayRecordPosition.previous:
                            baseData = dbContext.BaseDatas.Where(x => x.ProjectCode == project.ProjectCode && x.CreatedBy == userProfile.ProfileId && x.EnrollmentDate < currentBiometricsRecord.EnrollmentDate)
                                .Include("FingerprintReasons").Include("FingerprintImages")
                                .Include("Photographs").Include("Signatures").Include("FingerprintTemplates")
                                .OrderByDescending(x => x.EnrollmentDate).Take(1).FirstOrDefault();
                            break;
                    }

                    if (baseData != null)
                    {
                        var photograph = baseData.Photographs.Any() ? baseData.Photographs.ElementAt(0) : new Photograph();
                        var signature = baseData.Signatures.Any() ? baseData.Signatures.ElementAt(0) : new Signature();
                        var fingerprintImages = baseData.FingerprintImages.Any() ? baseData.FingerprintImages : new List<FingerprintImage>();
                        var fingeprintTemplate = baseData.FingerprintTemplates.Any() ? baseData.FingerprintTemplates.ElementAt(0) : new FingerprintTemplate();
                        fingerReasons = baseData.FingerprintReasons.Any() ? baseData.FingerprintReasons.ToList() : new List<FingerprintReason>();

                        //Update Biometric Record
                        biometricsRecord.EnrollmentDate = baseData.EnrollmentDate;
                        biometricsRecord.EnrollmentId = baseData.EnrollmentId;
                        biometricsRecord.FirstName = baseData.Firstname;
                        biometricsRecord.Surname = baseData.Surname;
                        biometricsRecord.MiddleName = baseData.MiddleName;
                        biometricsRecord.Title = baseData.Title;
                        biometricsRecord.Gender = baseData.Gender;
                        biometricsRecord.ProjectPrimaryCode = baseData.ProjectPrimaryCode;

                        if (photograph != null)
                        {
                            if (photograph.PhotographImage != null)
                                biometricsRecord.Photograph = (Bitmap)(TypeDescriptor.GetConverter(typeof(Bitmap)).ConvertFrom(photograph.PhotographImage));
                            if (photograph.PhotographTemplate != null)
                                biometricsRecord.PhotographTemplate = photograph.PhotographTemplate;
                        }
                        if (fingeprintTemplate != null && fingeprintTemplate.Template != null)
                            biometricsRecord.FingerTemplates = fingeprintTemplate.Template;
                        if (signature != null && signature.SignatureImage != null)
                            biometricsRecord.Signature = (Bitmap)(TypeDescriptor.GetConverter(typeof(Bitmap)).ConvertFrom(signature.SignatureImage));

                        if (!fingerprintImages.Any()) { reasons = fingerReasons; return biometricsRecord;}

                        foreach (var fImage in fingerprintImages)
                        {
                            var fingerprintBuffer = new NBuffer(fImage.FingerPrintImage);

                            //NFinger finger, NBuffer fingerTemplate
                            var biometricClient = new NBiometricClient();
                            var subject = new NSubject();
                            //Read finger image from enrollment and add it to NFinger object
                            var finger = new NFinger { Image = NImage.FromMemory(fingerprintBuffer, NImageFormat.Wsq) };
                            
                            //add NFinger object to NSubject
                            subject.Fingers.Add(finger);
                            ////Set finger template size (recommended, for enroll to database, is large) (optional)
                            //biometricClient.FingersTemplateSize = NTemplateSize.Large;

                            //Create template from added finger image
                            var status = biometricClient.CreateTemplate(subject);
                            if (status == NBiometricStatus.Ok)
                            {
                                biometricsRecord.SaveActiveUserFingerRecords(finger, subject, GetFingerDescription(fImage.FingerIndexId));
                            }

                        }
                    }
                    reasons = fingerReasons;
                    return biometricsRecord;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                reasons = fingerReasons;
                return biometricsRecord;
            }
        }
        internal static BiometricsRecord GetEnrollmentByEnrollmentId(Project project, string searchCriteria, out List<FingerprintReason> reasons)
        {
            var biometricsRecord = new BiometricsRecord();
            try
            {
                //Obtain Fingerprint Components Licenses
                try
                {
                    NLicense.ObtainComponents(Address, Port, FingerprintComponents);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    reasons = new List<FingerprintReason>();
                    return new BiometricsRecord();
                }
                using (CrimsDbContext dbContext = new CrimsDbContext(localConnectionString))
                {
                    BaseData baseData = null;

                    var baseDatas = dbContext.BaseDatas.Where(x => x.ProjectCode == project.ProjectCode && x.EnrollmentId == searchCriteria || x.ProjectPrimaryCode == searchCriteria)
                        .Include("FingerprintReasons").Include("FingerprintImages")
                        .Include("Photographs").Include("Signatures").Include("FingerprintTemplates")
                        .ToList();
                    baseData = baseDatas.Any() ? baseDatas[0] : new BaseData();
                    var fingerReasons = new List<FingerprintReason>();
                    if (!string.IsNullOrEmpty(baseData?.EnrollmentId))
                    {
                        var photograph = baseData.Photographs.Any()? baseData.Photographs.ElementAt(0) : new Photograph();
                        var signature = baseData.Signatures.Any() ? baseData.Signatures.ElementAt(0) : new Signature();
                        var fingerprintImages = baseData.FingerprintImages.Any()? baseData.FingerprintImages : new List<FingerprintImage>();
                        var fingeprintTemplate = baseData.FingerprintTemplates.Any()? baseData.FingerprintTemplates.ElementAt(0) : new FingerprintTemplate();
                        fingerReasons = baseData.FingerprintReasons.Any() ? baseData.FingerprintReasons.ToList() : new List<FingerprintReason>();

                        //Update Biometric Record
                        biometricsRecord.EnrollmentDate = baseData.EnrollmentDate;
                        biometricsRecord.EnrollmentId = baseData.EnrollmentId;
                        biometricsRecord.FirstName = baseData.Firstname;
                        biometricsRecord.Surname = baseData.Surname;
                        biometricsRecord.MiddleName = baseData.MiddleName;
                        biometricsRecord.Title = baseData.Title;
                        biometricsRecord.Gender = baseData.Gender;
                        biometricsRecord.ProjectPrimaryCode = baseData.ProjectPrimaryCode;

                        if (photograph != null)
                        {
                            if (photograph.PhotographImage != null)
                                biometricsRecord.Photograph = (Bitmap)(TypeDescriptor.GetConverter(typeof(Bitmap)).ConvertFrom(photograph.PhotographImage));
                            if (photograph.PhotographTemplate != null)
                                biometricsRecord.PhotographTemplate = photograph.PhotographTemplate;
                        }
                        if (fingeprintTemplate?.Template != null)
                            biometricsRecord.FingerTemplates = fingeprintTemplate.Template;
                        if (signature?.SignatureImage != null)
                            biometricsRecord.Signature = (Bitmap)(TypeDescriptor.GetConverter(typeof(Bitmap)).ConvertFrom(signature.SignatureImage));

                        if (!fingerprintImages.Any())
                        {
                            reasons = fingerReasons;
                            return biometricsRecord;
                        }

                        foreach (var fImage in fingerprintImages)
                        {
                            var fingerprintBuffer = new NBuffer(fImage.FingerPrintImage);

                            //NFinger finger, NBuffer fingerTemplate
                           
                            var biometricClient = new NBiometricClient();
                            var subject = new NSubject();
                            var finger = new NFinger {Image = NImage.FromMemory(fingerprintBuffer, NImageFormat.Wsq)};
                            //Read finger image from enrollment and add it to NFinger object
                            //add NFinger object to NSubject
                            subject.Fingers.Add(finger);
                            ////Set finger template size (recommended, for enroll to database, is large) (optional)
                            //biometricClient.FingersTemplateSize = NTemplateSize.Large;

                            //Create template from added finger image
                            var status = biometricClient.CreateTemplate(subject);
                            if (status == NBiometricStatus.Ok)
                            {
                                biometricsRecord.SaveActiveUserFingerRecords(finger, subject, GetFingerDescription(fImage.FingerIndexId));
                            }
                        }
                    }
                    reasons = fingerReasons;
                    return biometricsRecord;
              }
            }
            catch (Exception ex)
            {
                reasons = new List<FingerprintReason>();
                return biometricsRecord;
            }
        }
        private static byte[] GetImageMemoryStream(Image image, ImageFormat format)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, format);
            return ms.ToArray();
        }
        internal static EnrollmentRecord GetEnrollment(string enrollmentId)
        {
            try
            {
                using (CrimsDbContext dbContext = new CrimsDbContext(localConnectionString))
                {
                    BaseData baseData = null;

                    var baseDatas = dbContext.BaseDatas.Where(x => x.EnrollmentId == enrollmentId)
                        .Include("FingerprintReasons").Include("FingerprintImages").Include("CustomDatas")
                        .Include("Photographs").Include("Signatures").Include("FingerprintTemplates")
                        .ToList();

                    if (!baseDatas.Any())
                    {
                        return new EnrollmentRecord();
                    }

                    baseData = baseDatas[0];
                    
                    var photograph = baseData.Photographs.Any() ? baseData.Photographs.ElementAt(0) : new Photograph();
                    var signature = baseData.Signatures.Any() ? baseData.Signatures.ElementAt(0) : new Signature();
                    var fingerprintImages = baseData.FingerprintImages.ToList();
                    var fingeprintTemplate = baseData.FingerprintTemplates.Any() ? baseData.FingerprintTemplates.ElementAt(0) : new FingerprintTemplate();
                    var fingerReasons = baseData.FingerprintReasons.ToList();
                    var customDatas = baseData.CustomDatas.ToList();
                   
                    if (!customDatas.Any() || !fingerprintImages.Any() || photograph.TableId < 1)
                    {
                        return new EnrollmentRecord();
                    }
                    var enrollmentRecord = new EnrollmentRecord
                    {
                        CustomDatas = customDatas,
                        Photograph = photograph,
                        FingerprintTemplate = fingeprintTemplate,
                        FingerprintImages = fingerprintImages,
                        Signature = signature,
                        FingerprintReasons = fingerReasons
                    };

                    return enrollmentRecord;
                }
            }
            catch (Exception ex)
            {
                return new EnrollmentRecord();
            }
        }
        private static MemoryStream ConvertImageToMemoryStream(MemoryStream ms, Image image, ImageFormat format)
        {
            //MemoryStream ms = new MemoryStream();
            image.Save(ms, format);
            return ms;
        }
        private static FingerDescription GetFingerDescription(int fingerIndexId)
        {
            switch (fingerIndexId)
            {
                case 1:
                    return FingerDescription.LFLittle;
                case 2:
                    return FingerDescription.LFRing;
                case 3:
                    return FingerDescription.LFMiddle;
                case 4:
                    return FingerDescription.LFIndex;
                case 5:
                    return FingerDescription.LFThumb;
                case 6:
                    return FingerDescription.RFThumb;
                case 7:
                    return FingerDescription.RFIndex;
                case 8:
                    return FingerDescription.RFMiddle;
                case 9:
                    return FingerDescription.RFRing;
                case 10:
                    return FingerDescription.RFLittle;
                default:
                    return FingerDescription.Unknown;

            }
        }

    }
}
