using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class CustomDataConfig : EntityTypeConfiguration<CustomData>
    {
        public CustomDataConfig()
        {
            //Table Mapping
            ToTable("CustomDatas");
            //Primary Key
            HasKey(x => x.TableId);
            Property(t => t.EnrollmentId).IsRequired().HasMaxLength(50);
            Property(t => t.CustomDataId).IsRequired().HasMaxLength(50);
            Property(t => t.CustomFieldId).IsRequired().HasMaxLength(50);
            Property(t => t.CrimsCustomData).IsRequired();
            HasRequired(t => t.BaseData).WithMany(t => t.CustomDatas).HasForeignKey(d => d.EnrollmentId);
        }
    }
}