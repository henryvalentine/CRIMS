using System.Data.Entity;

namespace Crims.API.Models
{
    public class IdentityDbConfiguration : DbConfiguration
    {
        public IdentityDbConfiguration()
        {
            SetHistoryContext("MySql.Data.MySqlClient", (conn, schema) => new IdentityDbHistoryContext(conn, schema));
        }
    }
}