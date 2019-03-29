using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class SyncJob : Entity
    {
        public string Id { get; set; }
        //[Index("IX_userprofile_syncjob", IsUnique = true)]
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public virtual ICollection<SyncJobHistory> SyncJobHistories { get; set; }

    }
}