using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class ApprovalConfig : EntityTypeConfiguration<Approval>
    {
        public ApprovalConfig()
        {
            //Table Mapping
            ToTable("Approvals");
            //Primary Key
            HasKey(x => x.TableId);
            Property(t => t.EnrollmentId).IsRequired();
            Property(t => t.EnrollmentId).IsRequired().HasMaxLength(50);
            Property(t => t.ApprovalId).IsRequired().HasMaxLength(50);
            Property(t => t.ApprovalId).IsRequired();
        }
    }
}