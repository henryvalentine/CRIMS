using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class AppSetting : Entity
    {
        public string Id { get; set; }
        public string BiometricTemplatePath { get; set; }
        public DateTime SynchronisationTime { get; set; }
        public int SynchFrequency { get; set; }

    }
}