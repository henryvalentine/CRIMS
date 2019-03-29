using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class ProjectConfig : EntityTypeConfiguration<Project>
    {
        public ProjectConfig()
        {
            ToTable("Projects");
            HasKey(x => x.TableId);
            Property(t => t.TableId).IsRequired();
            Property(t => t.ProjectCode).IsRequired().HasMaxLength(8);
            Property(t => t.DateCreated).IsRequired();
            Property(t => t.LicenseExpiryDate).IsRequired();
            Property(t => t.ProjectName).IsRequired().HasMaxLength(200);
        }
    }
}