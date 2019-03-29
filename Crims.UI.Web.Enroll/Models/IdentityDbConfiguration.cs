using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Crims.UI.Web.Enroll.Models
{
    public class IdentityDbConfiguration : DbConfiguration
    {
        public IdentityDbConfiguration()
        {
            SetHistoryContext("MySql.Data.MySqlClient", (conn, schema) => new IdentityDbHistoryContext(conn, schema));
        }
    }
}