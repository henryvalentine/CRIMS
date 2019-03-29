using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class BaseDataConfig : EntityTypeConfiguration<BaseData>
    {
        public BaseDataConfig()
        {
            //Table mapping
            ToTable("BaseDatas");
            //Primary Key
            HasKey(x => x.EnrollmentId);
            //Properties
            Property(t => t.ProjectCode).IsRequired().HasMaxLength(8);
            Property(t => t.Surname).IsRequired().HasMaxLength(50);
            Property(t => t.Firstname).IsRequired().HasMaxLength(50);
            Property(t => t.Email).HasMaxLength(50); //.IsRequired()
            Property(t => t.EnrollmentDate).IsRequired();
            Property(t => t.EnrollmentId).IsRequired().HasMaxLength(50);
            Property(t => t.MobileNumber).IsRequired().HasMaxLength(20);
            Property(t => t.ProjectCode).IsRequired().HasMaxLength(8);
            Property(t => t.ProjectPrimaryCode).IsRequired();
            Property(t => t.Gender).IsRequired();
        }
    }
}