using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TandAProject.Utils;

namespace TandAProject.Database
{
    class ConnectionManager
    {
        string connectionString { get; set; }
        string DBServer { get; set; }
        string DBPort { get; set; }
        string DBUser { get; set; }
        string DBPassword { get; set; }
        string DBName { get; set; }
        MySqlConnection mySqlConnection { get; set; }

        AppSettings appSettings;

        private static MySqlConnection _db = null;
        private MySqlConnection _db2 = null;
        public ConnectionManager()
        {
        }
        public static MySqlConnection DbConnection()
        {

            if (_db == null)
            {                
                AppSettings settings = new AppSettings();
                string connectionString = String.Format("server={0};user={1};database={2};port={3};password={4}",
                    settings.DBServer, settings.DBUser, settings.DBName, settings.DBPort, settings.DBPassword);
                _db = new MySqlConnection(connectionString);

                _db.Open();

                return _db;
            }
            else {
                return _db;
            }
        }

        public MySqlConnection DbConnection_v2()
        {
            _db2 = null;

            AppSettings settings = new AppSettings();
            string connectionString = String.Format("server={0};user={1};database={2};port={3};password={4};Convert Zero Datetime=True",
                settings.DBServer, settings.DBUser, settings.DBName, settings.DBPort, settings.DBPassword);

            _db2 = new MySqlConnection(connectionString);
            _db2.Open();

            return _db2;
        }

        public ConnectionManager(AppSettings AppSettings)
        {
            this.appSettings = AppSettings;
            this.DBServer = AppSettings.DBServer;
            this.DBName = AppSettings.DBName;
            this.DBPort = AppSettings.DBPort;
            this.DBUser = AppSettings.DBUser;
            this.DBPassword = AppSettings.DBPassword;

            this.connectionString = String.Format("server={0};user={1};database={2};port={3};password={4}", DBServer, DBUser, DBName, DBPort, DBPassword);
        }

        public ConnectionManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ConnectionManager(MySqlConnection conn)
        {
            this.mySqlConnection = conn;
        }

        public ConnectionManager(string DBServer, string DBName, string DBPort, string DBUser, string DBPassword)
        {
            this.DBServer = DBServer;
            this.DBPort = DBPort;
            this.DBUser = DBUser;
            this.DBPassword = DBPassword;
            this.DBName = DBName;
            this.connectionString = String.Format("server={0};user={1};database={2};port={3};password={4}",
                DBServer, DBUser, DBName, DBPort, DBPassword);
        }

        public MySqlConnection getDBConnection()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        public MySqlConnection getDBConnection(string connectionString)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);

            return conn;
        }

        public MySqlConnection getDBConnectionInstance()
        {
            return this.mySqlConnection;
        }

        private void readMySqlConfig(string filePath)
        {
            try
            {

                string PNString = File.ReadAllText(filePath);

                string[] settings = PNString.Split(new char[] { ',' });

                this.DBServer = settings[0];
                this.DBPort = settings[1];
                this.DBUser = settings[2];
                this.DBPassword = settings[3];
                this.DBName = settings[4];


            }
            catch (Exception exp)
            {
                Logger.logToFile(exp, AppSettings.ErrorLogPath);
            }
        }

        public void Dispose()
        {
            if (mySqlConnection != null)
            {
                mySqlConnection.Close();
            }
        }
    }
}
