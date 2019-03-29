using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class PhotographConfig : EntityTypeConfiguration<Photograph>
    {
        public PhotographConfig()
        {
            ToTable("Photographs");
            HasKey(x => x.TableId);
            Property(t => t.TableId).IsRequired();
            Property(t => t.EnrollmentId).IsRequired().HasMaxLength(50);
            Property(t => t.PhotographId).IsRequired().HasMaxLength(50);
            Property(t => t.PhotographImage).IsRequired();
            Property(t => t.PhotographTemplate).IsRequired();
            HasRequired(t => t.BaseData).WithMany(l => l.Photographs).HasForeignKey(f => f.EnrollmentId);
        }
    }
}