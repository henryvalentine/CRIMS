using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class CustomFieldConfig : EntityTypeConfiguration<CustomField>
    {
        public CustomFieldConfig()
        {
            //Table Mapping
            ToTable("CustomFields");
            //Primary Key
            HasKey(x => x.TableId);
            Property(t => t.CustomFieldName).IsRequired().HasMaxLength(50);
            Property(t => t.CustomFieldId).IsRequired().HasMaxLength(50);
            //HasRequired(t => t.CustomFieldType).WithMany(t => t.CustomFields).HasForeignKey(d => d.FieldTypeId);
            //HasRequired(t => t.CustomList).WithMany(t => t.CustomFields).HasForeignKey(d => d.CustomListId);
            //HasRequired(t => t.CustomGroup).WithMany(t => t.CustomFields).HasForeignKey(d => d.CustomGroupId);
        }
    }
}