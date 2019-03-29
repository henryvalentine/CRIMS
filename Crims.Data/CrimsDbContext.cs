using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using Crims.Data.Models;
using Configurations;
using Repository.Pattern.DataContext;
using System.Data.Entity.Validation;
using Crims.Data.Migrations;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Ef6;

namespace Crims.Data
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class CrimsDbContext : DbContext, IDataContextAsync
    {
        public DbSet<BaseData> BaseDatas { get; set; }
        public DbSet<CustomField> CustomFields { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<CustomFieldType> CustomFieldTypes { get; set; }
        public DbSet<CustomList> CustomLists { get; set; }

        public DbSet<CustomListData> CustomListDatas { get; set; }
        public DbSet<CustomData> CustomDatas { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<FingerprintImage> FingerprintImages { get; set; }
        public DbSet<FingerprintReason> FingerprintReasons { get; set; }
        public DbSet<FingerprintTemplate> FingerprintTemplates { get; set; }
        public DbSet<Photograph> Photographs { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectCustomField> ProjectCustomFields { get; set; }
        public DbSet<ProjectCustomGroup> ProjectCustomGroups { get; set; }

        public DbSet<ProjectCustomList> ProjectCustomLists { get; set; }
        public DbSet<ProjectCustomListData> ProjectCustomListDatas { get; set; }
        public DbSet<Signature> Signature { get; set; }
        public DbSet<SyncJob> SyncJob { get; set; }
        public DbSet<SyncJobHistory> SyncJobHistory { get; set; }
        public DbSet<SystemsFeature> SystemsFeature { get; set; }



        public CrimsDbContext() : base("crimsDbEntities") 
        {
            Configuration.LazyLoadingEnabled = true;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CrimsDbContext, Configuration>("crimsDbEntities"));
        }

        public CrimsDbContext(string connectionString) : base(connectionString)
        {
            Configuration.LazyLoadingEnabled = true;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CrimsDbContext, Configuration>(connectionString));
        }
        public static CrimsDbContext Create()
        {
            return new CrimsDbContext();
        }

        public IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }

        }

        public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState
        {
            Entry(entity).State = StateHelper.ConvertState(entity.ObjectState);
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                ((IObjectState)dbEntityEntry.Entity).ObjectState = StateHelper.ConvertState(dbEntityEntry.State);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Configurations.Add(new CustomFieldConfig());
            modelBuilder.Configurations.Add(new BaseDataConfig());
            modelBuilder.Configurations.Add(new CustomDataConfig());
            modelBuilder.Configurations.Add(new ApprovalConfig());
            modelBuilder.Configurations.Add(new CustomFieldTypeConfig());
            modelBuilder.Configurations.Add(new CustomGroupConfig());
            modelBuilder.Configurations.Add(new CustomListConfig());

            modelBuilder.Configurations.Add(new CustomListDataConfig());
            modelBuilder.Configurations.Add(new FingerprintImageConfig());
            modelBuilder.Configurations.Add(new FingerprintReasonConfig());
            modelBuilder.Configurations.Add(new FingerprintTemplateConfig());
            modelBuilder.Configurations.Add(new PhotographConfig());
            modelBuilder.Configurations.Add(new ProjectConfig());
            modelBuilder.Configurations.Add(new ProjectCustomFieldConfig());

            modelBuilder.Configurations.Add(new ProjectCustomGroupConfig());
            modelBuilder.Configurations.Add(new ProjectCustomListConfig());
            modelBuilder.Configurations.Add(new ProjectCustomListDataConfig());

            modelBuilder.Configurations.Add(new SignaturepConfig());
            modelBuilder.Configurations.Add(new SystemsFeatureConfig());
        }
    }
}
