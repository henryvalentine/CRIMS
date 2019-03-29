using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class FingerprintTemplateConfig : EntityTypeConfiguration<FingerprintTemplate>
    {
        public FingerprintTemplateConfig()
        {
            ToTable("FingerprintTemplates");
            HasKey(x => x.TableId);
            Property(t => t.TableId).IsRequired();
            Property(t => t.EnrollmentId).IsRequired().HasMaxLength(50);
            Property(t => t.Template).IsRequired();
            Property(t => t.UniquenessStatus).IsRequired();
            HasRequired(r => r.BaseData).WithMany(m => m.FingerprintTemplates).HasForeignKey(f => f.EnrollmentId);

        }
    }
}