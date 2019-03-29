using System.Data.Entity;

namespace Crims.UI.Web.Models
{
    public class IdentityDbConfiguration : DbConfiguration
    {
        public IdentityDbConfiguration()
        {
            SetHistoryContext("MySql.Data.MySqlClient", (conn, schema) => new IdentityDbHistoryContext(conn, schema));
        }
    }
}