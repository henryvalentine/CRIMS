using System.Reflection;
using Autofac;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using Crims.Data;
using Crims.Data.Models;
using Crims.Data.Services;
using Crims.Data.Contracts;
using Autofac.Integration.Mvc;
using System.Web.Mvc;

namespace Crims.UI.Web
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

            //// Build the container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}