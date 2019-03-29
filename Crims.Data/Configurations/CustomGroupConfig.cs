using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class CustomGroupConfig : EntityTypeConfiguration<CustomGroup>
    {
        public CustomGroupConfig()
        {
            //Table Mapping
            ToTable("CustomGroups");
            //Primary key
            HasKey(x => x.TableId);
            Property(t => t.TableId).IsRequired();
            Property(t => t.GroupName).IsRequired().HasMaxLength(50);
            Property(t => t.CustomGroupId).IsRequired();
            Property(t => t.TabIndex).IsRequired();
        }
    }
}