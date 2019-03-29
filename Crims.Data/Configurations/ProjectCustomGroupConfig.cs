using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class ProjectCustomGroupConfig : EntityTypeConfiguration<ProjectCustomGroup>
    {
        public ProjectCustomGroupConfig()
        {
            ToTable("ProjectCustomGroups");
            HasKey(x => x.TableId);
            Property(t => t.TableId).IsRequired();
            Property(t => t.CustomGroupId).IsRequired().HasMaxLength(50);
            Property(t => t.ProjectCode).IsRequired().HasMaxLength(8);
            Property(t => t.TabIndex).IsRequired();
        }
    }
}