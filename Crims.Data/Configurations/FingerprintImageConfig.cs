using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class FingerprintImageConfig : EntityTypeConfiguration<FingerprintImage>
    {
        public FingerprintImageConfig()
        {
            ToTable("FingerprintImages");
            HasKey(x => x.TableId);
            Property(t => t.TableId).IsRequired();
            Property(t => t.EnrollmentId).IsRequired().HasMaxLength(50);
            Property(t => t.FingerIndexId).IsRequired();
            Property(t => t.FingerPrintImage).IsRequired();
            HasRequired(t => t.BaseData).WithMany(r => r.FingerprintImages).HasForeignKey(f => f.EnrollmentId);
        }
    }
}