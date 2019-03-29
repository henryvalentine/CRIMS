using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Crims.Data;
using Crims.Data.Contracts;
using Crims.Data.Models;
using Crims.Data.Services;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;

namespace Crims.UI.Web.Enroll
{
    public static class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            // Setup the Container Builder
            var builder = new ContainerBuilder();
            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<CrimsDbContext>().As<IDataContextAsync>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkAsync>().InstancePerLifetimeScope();

            builder.RegisterType<Repository<Project>>().As<IRepositoryAsync<Project>>();
            builder.RegisterType<ProjectService>().As<IProjectService>();

            builder.RegisterType<Repository<CustomGroup>>().As<IRepositoryAsync<CustomGroup>>();
            builder.RegisterType<CustomGroupService>().As<ICustomGroupService>();

            builder.RegisterType<Repository<CustomFieldType>>().As<IRepositoryAsync<CustomFieldType>>();
            builder.RegisterType<CustomFieldTypeService>().As<ICustomFieldTypeService>();

            builder.RegisterType<Repository<CustomList>>().As<IRepositoryAsync<CustomList>>();
            builder.RegisterType<CustomListService>().As<ICustomListService>();

            builder.RegisterType<Repository<CustomListData>>().As<IRepositoryAsync<CustomListData>>();
            builder.RegisterType<CustomListDataService>().As<ICustomListDataService>();

            builder.RegisterType<Repository<CustomField>>().As<IRepositoryAsync<CustomField>>();
            builder.RegisterType<CustomFieldService>().As<ICustomFieldService>();

            builder.RegisterType<Repository<ProjectCustomField>>().As<IRepositoryAsync<ProjectCustomField>>();
            builder.RegisterType<ProjectCustomFieldService>().As<IProjectCustomFieldService>();

            builder.RegisterType<Repository<ProjectCustomList>>().As<IRepositoryAsync<ProjectCustomList>>();
            builder.RegisterType<ProjectCustomListService>().As<IProjectCustomListService>();

            builder.RegisterType<Repository<ProjectCustomListData>>().As<IRepositoryAsync<ProjectCustomListData>>();
            builder.RegisterType<ProjectCustomListDataService>().As<IProjectCustomListDataService>();

            builder.RegisterType<Repository<ProjectCustomGroup>>().As<IRepositoryAsync<ProjectCustomGroup>>();
            builder.RegisterType<ProjectCustomGroupService>().As<IProjectCustomGroupService>();

            builder.RegisterType<Repository<BaseData>>().As<IRepositoryAsync<BaseData>>();
            builder.RegisterType<BaseDataService>().As<IBaseDataService>();

            builder.RegisterType<Repository<CustomData>>().As<IRepositoryAsync<CustomData>>();
            builder.RegisterType<CustomDataService>().As<ICustomDataService>();

            builder.RegisterType<Repository<Photograph>>().As<IRepositoryAsync<Photograph>>();
            builder.RegisterType<PhotographService>().As<IPhotographService>();

            builder.RegisterType<Repository<FingerprintImage>>().As<IRepositoryAsync<FingerprintImage>>();
            builder.RegisterType<FingerprintImageService>().As<IFingerprintImageService>();

            builder.RegisterType<Repository<FingerprintTemplate>>().As<IRepositoryAsync<FingerprintTemplate>>();
            builder.RegisterType<FingerprintTemplateService>().As<IFingerprintTemplateService>();

            builder.RegisterType<Repository<AppSetting>>().As<IRepositoryAsync<AppSetting>>();
            builder.RegisterType<AppSettingService>().As<IAppSettingService>();

            builder.RegisterType<Repository<SyncJobHistory>>().As<IRepositoryAsync<SyncJobHistory>>();
            builder.RegisterType<SyncJobHistoryService>().As<ISyncJobHistoryService>();

            builder.RegisterType<Repository<Approval>>().As<IRepositoryAsync<Approval>>();
            builder.RegisterType<ApprovalService>().As<IApprovalService>();

            builder.RegisterType<Repository<Signature>>().As<IRepositoryAsync<Signature>>();
            builder.RegisterType<SignatureService>().As<ISignatureService>();

            builder.RegisterType<Repository<FingerprintReason>>().As<IRepositoryAsync<FingerprintReason>>();
            builder.RegisterType<FingerprintReasonService>().As<IFingerprintReasonService>();

            //// Build the container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}

