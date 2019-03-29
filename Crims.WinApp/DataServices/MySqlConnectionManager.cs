using Crims.UI.Win.Enroll.Properties;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crims.UI.Win.Enroll.DataServices
{
    public class MySqlConnectionManager
    {
        private MySqlConnection _db = null;
        private Settings settings;

        public MySqlConnectionManager()
        {
            settings = new Settings();
        }
        public MySqlConnection DbConnection(string server, string db, string port, string user, string password)
        {
            _db = null;

            string connectionString = String.Format("server={0};user={1};database={2};port={3};password={4};Convert Zero Datetime=True",
                server, user, db, port,password);

            _db = new MySqlConnection(connectionString);

            if (_db.State == System.Data.ConnectionState.Open)
            {
                return _db;
            }
            else
            {
                _db.Open();
                return _db;
            }
        }

        public MySqlConnection DbConnection(string connectionString)
        {
            _db = null;

            _db = new MySqlConnection(connectionString);

            if (_db.State == System.Data.ConnectionState.Open)
            {
                return _db;
            }
            else
            {
                _db.Open();
                return _db;
            }
        }

        public MySqlConnection DbConnection()
        {
            _db = null;

            string connectionString = String.Format("server={0};user={1};database={2};port={3};password={4};Convert Zero Datetime=True",
                settings.DBServer, settings.DBUser, settings.DBName, settings.DBPort, settings.DBPassword);

            _db = new MySqlConnection(connectionString);

            if (_db.State == System.Data.ConnectionState.Open)
            {
                return _db;
            }
            else
            {
                _db.Open();
                return _db;
            }
        }

    }
}
