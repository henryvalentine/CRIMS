using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class ProjectCustomListConfig : EntityTypeConfiguration<ProjectCustomList>
    {
        public ProjectCustomListConfig()
        {
            ToTable("ProjectCustomLists");
            HasKey(x => x.TableId);
            Property(t => t.TableId).IsRequired();
            Property(t => t.CustomListId).IsRequired().HasMaxLength(50);
            Property(t => t.ProjectCode).IsRequired().HasMaxLength(8);
        }
    }
}