using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using Crims.UI.Web.Enroll.Helpers;

namespace Crims.UI.Web.Enroll.Models
{
    public class IdentityDbInitializer : IDatabaseInitializer<ApplicationDbContext>
    {
        public void InitializeDatabase(ApplicationDbContext context)
        {
            try
            {
                if (!context.Database.Exists())
                {
                    // if database did not exist before - create it
                    context.Database.Create();
                }
                
                //query to check if Identity Tables are present in the database
                var db = context.Database.Connection.Database;
                var identityTableExists = ((IObjectContextAdapter)context).ObjectContext.ExecuteStoreQuery<int>("SELECT COUNT(*) FROM information_schema.tables WHERE table_name = 'aspnetusers' AND table_schema = '" + db + "'");

                //if Identity Tables are not present, create them
                if (identityTableExists.FirstOrDefault() == 0)
                {
                    //Get Identity Script
                    var path = AppDomain.CurrentDomain.BaseDirectory + "OtherScripts";
                    if (string.IsNullOrEmpty(path))
                    {
                        return;
                    }

                    var scriptPaths = Directory.GetFiles(path);
                    if (!scriptPaths.Any())
                    {
                        return;
                    }

                    var identityScript = scriptPaths[0];
                    context.Database.ExecuteSqlCommand(File.ReadAllText(identityScript));

                    //Seed Roles Table
                    context.Roles.AddOrUpdate(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Name = "Super_Admin", Id = "f8b7baa5068c486783212ab42t087bde" });
                    context.Roles.AddOrUpdate(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Name = "Admin", Id = "9ue0baa5068c489741212ab42t085rsx" });
                    context.Roles.AddOrUpdate(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Name = "Site_Administrator", Id = "p2btbda5068c486783212ab42t087bdr" });
                    context.Roles.AddOrUpdate(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Name = "Enrollment_Officer", Id = "7yb7fea5068c486783212ab42t08r89a" });
                    context.Roles.AddOrUpdate(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Name = "Enrollee", Id = "3ie2gyq9068c4869d3245ab42t08r3w0" });
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
            }
        }
    }
}
