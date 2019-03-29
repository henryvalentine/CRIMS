using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class SyncJobHistoryConfig : EntityTypeConfiguration<SyncJobHistory>
    {
        public SyncJobHistoryConfig()
        {
            ToTable("SyncJobs");
            HasKey(x => x.Id);
            Property(t => t.SyncJobId).IsRequired().HasMaxLength(50);
            Property(t => t.RecordId).IsRequired().HasMaxLength(50);
            Property(t => t.SyncJobId);
            HasRequired(s => s.SyncJob).WithMany(m => m.SyncJobHistories).HasForeignKey(f => f.SyncJobId);
            HasRequired(s => s.BaseData).WithMany(m => m.SyncJobHistories).HasForeignKey(f => f.RecordId);
        }
    }
}