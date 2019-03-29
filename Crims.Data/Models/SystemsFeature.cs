using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class SystemsFeature : Entity
    {
        public int TableId { get; set; }
        public string FeatureId { get; set; }
        public string FeatureName { get; set; }
        public string Status { get; set; }
        public string Deleted { get; set; }
    }
}
