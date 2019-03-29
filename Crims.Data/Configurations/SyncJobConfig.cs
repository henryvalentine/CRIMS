using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class SyncJobConfig : EntityTypeConfiguration<SyncJob>
    {
        public SyncJobConfig()
        {
            ToTable("SyncJobs");
            HasKey(x => x.Id);
            Property(t => t.Id).IsRequired().HasMaxLength(50);
            Property(t => t.UserId).IsRequired().HasMaxLength(128);
            Property(t => t.Date);
            Property(t => t.StartDateTime);
            Property(t => t.EndDateTime);
        }
    }
}