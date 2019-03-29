using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class CustomFieldTypeConfig : EntityTypeConfiguration<CustomFieldType>
    {
        public CustomFieldTypeConfig()
        {
            ToTable("CustomFieldTypes");
            HasKey(x => x.TableId);
            Property(t => t.TableId).IsRequired();
            Property(t => t.FieldTypeName).IsRequired().HasMaxLength(50);
            Property(t => t.FieldTypeId).IsRequired();
        }
    }
}