using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class SystemsFeatureConfig : EntityTypeConfiguration<SystemsFeature>
    {
        public SystemsFeatureConfig()
        {
            ToTable("SystemsFeatures");
            HasKey(x => x.TableId);
            Property(t => t.TableId).IsRequired();
            Property(t => t.FeatureId).IsRequired().HasMaxLength(50);
            Property(t => t.FeatureName).IsRequired().HasMaxLength(50);
            Property(t => t.Status).IsRequired();
        }
    }
}