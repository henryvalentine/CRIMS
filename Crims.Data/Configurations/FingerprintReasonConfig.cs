using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class FingerprintReasonConfig : EntityTypeConfiguration<FingerprintReason>
    {
        public FingerprintReasonConfig()
        {
            ToTable("FingerprintReasons");
            HasKey(x => x.TableId);
            Property(t => t.TableId).IsRequired();
            Property(t => t.EnrollmentId).IsRequired().HasMaxLength(50);
            Property(t => t.TableId).IsRequired();
            Property(t => t.FingerIndex).IsRequired();
            Property(t => t.FingerReason).IsRequired();
            HasRequired(r => r.BaseData).WithMany(m => m.FingerprintReasons).HasForeignKey(f => f.EnrollmentId);
        }
    }
}