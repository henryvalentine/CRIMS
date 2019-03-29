using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class AppSettingConfig : EntityTypeConfiguration<AppSetting>
    {
        public AppSettingConfig()
        {
            ToTable("AppSettings");
            HasKey(x => x.Id);
            Property(t => t.Id).IsRequired();
            Property(t => t.BiometricTemplatePath);
            Property(t => t.SynchronisationTime);
            Property(t => t.SynchronisationTime);
        }
    }
}