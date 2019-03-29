using Crims.Data.Models;
using System.Data.Entity.ModelConfiguration;

namespace Configurations
{
    internal class CustomListDataConfig : EntityTypeConfiguration<CustomListData>
    {
        public CustomListDataConfig()
        {
            ToTable("CustomListDatas");
            HasKey(x => x.TableId);
            Property(t => t.TableId).IsRequired();
            Property(t => t.ListDataName).IsRequired().HasMaxLength(250);
            Property(t => t.CustomListDataId).IsRequired();
            Property(t => t.CustomListId).IsRequired();
            //HasRequired(t => t.CustomList).WithMany(t => t.CustomListDatas).HasForeignKey(d => d.CustomListId);
        }
    }
}