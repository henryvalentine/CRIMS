using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class ProjectCustomFieldConfig : EntityTypeConfiguration<ProjectCustomField>
    {
        public ProjectCustomFieldConfig()
        {
            ToTable("ProjectCustomFields");
            HasKey(x => x.TableId);
            Property(t => t.TableId).IsRequired();
            Property(t => t.CustomFieldId).IsRequired().HasMaxLength(50);
            Property(t => t.ProjectCode).IsRequired().HasMaxLength(8);
            //HasOptional(t => t.CustomField).WithMany(t => t.ProjectCustomFields).HasForeignKey(d => d.CustomFieldId);
        }
    }
}