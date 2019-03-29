using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Crims.UI.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Crims.API.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser
    // class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationDbContext.UserProfile UserInfo { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("crimsDbEntities")
        {
            Configuration.LazyLoadingEnabled = true;
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer(new IdentityDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<UserProfile> UserInfo { get; set; }

        public class UserProfile
        {
            public string Id { get; set; }
            public string FullName { get; set; }
            public string PhoneNumber { get; set; }
            public string Sex { get; set; }
            public DateTime DateCreated { get; set; }
            public string Status { get; set; }
        }
    }
}