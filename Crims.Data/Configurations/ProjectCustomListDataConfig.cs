using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class ProjectCustomListDataConfig : EntityTypeConfiguration<ProjectCustomListData>
    {
        public ProjectCustomListDataConfig()
        {
            ToTable("ProjectCustomListDatas");
            HasKey(x => x.TableId);
            Property(t => t.TableId).IsRequired();
            Property(t => t.CustomListDataId).IsRequired().HasMaxLength(50);
            Property(t => t.ProjectCode).IsRequired().HasMaxLength(8);
        }
    }
}