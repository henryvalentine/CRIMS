using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class SignaturepConfig : EntityTypeConfiguration<Signature>
    {
        public SignaturepConfig()
        {
            ToTable("Signatures");
            HasKey(x => x.TableId);
            Property(t => t.TableId).IsRequired();
            Property(t => t.EnrollmentId).IsRequired().HasMaxLength(50);
            Property(t => t.SignatureImage).IsRequired();
            HasRequired(r => r.BaseData).WithMany(m => m.Signatures).HasForeignKey(k => k.EnrollmentId);
        }
    }
}