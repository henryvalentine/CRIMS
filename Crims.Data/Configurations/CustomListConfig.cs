using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class CustomListConfig : EntityTypeConfiguration<CustomList>
    {
        public CustomListConfig()
        {
            ToTable("CustomLists");
            HasKey(x => x.TableId);
            Property(t => t.TableId).IsRequired();
            Property(t => t.CustomListName).IsRequired().HasMaxLength(50);
            Property(t => t.CustomListId).IsRequired();
        }
    }
}