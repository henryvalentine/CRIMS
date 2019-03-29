using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class SyncJobHistory : Entity
    {
        public string Id { get; set; }
        [Index("IX_syncjob_recordId")]
        public string RecordId { get; set; }
        [Index("IX_syncjob_syncjobhistory")]
        public string SyncJobId { get; set; }

        [ForeignKey("SyncJobId")]
        public virtual SyncJob SyncJob { get; set; }
        [ForeignKey("RecordId")]
        public virtual BaseData BaseData { get; set; }

    }
}