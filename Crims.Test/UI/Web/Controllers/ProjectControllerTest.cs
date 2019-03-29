using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crims.Data.Models;
using Crims.Data.Services;
using Crims.Data.Contracts;
using Crims.UI.Web.Controllers;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using Service.Pattern;
using Crims.Test.Fake;

namespace Crims.Tests.UI.Web.Controllers
{
    [TestClass]
    public class ProjectControllerTest
    {
        private IDataContextAsync _dummyContext;
        private IProjectService projectService;
        private IQueryable<Project> projects;
        private IRepositoryAsync<Project> _projectRepository;
        private ProjectController controller;
        private IUnitOfWorkAsync unitOfWork;
        
        private ICustomListService _customListService;
        private ICustomFieldService _customFieldService;
        private ICustomListDataService _customListDataService;
        private IProjectCustomListService _projectCustomListService;
        private IProjectCustomFieldService _projectCustomFieldService;
        private IProjectCustomListDataService _projectCustomListDataService;

        [TestInitialize()]
        public void Initialize()
        {
            _dummyContext = new DummyDbContext();
            unitOfWork = new UnitOfWork(_dummyContext);

            _projectRepository = new Repository<Project>(_dummyContext, unitOfWork);
            projects = new List<Project>
            {
                new Project {TableId = 1, ProjectCode = "P001",ProjectName = "Proj1", ProjectDescription= "Proj1 Desc" },
                new Project {TableId = 2, ProjectCode = "P002",ProjectName = "Proj2", ProjectDescription= "Proj2 Desc" },
                new Project {TableId = 3, ProjectCode = "P003",ProjectName = "Proj3", ProjectDescription= "Proj3 Desc" },
                new Project {TableId = 4, ProjectCode = "P004",ProjectName = "Proj4", ProjectDescription= "Proj4 Desc" }
            }.AsQueryable();
            _projectRepository.InsertRange(projects);
            unitOfWork.SaveChanges();

            projectService = new ProjectService(_projectRepository);
            controller = new ProjectController(projectService, _customListService, _customFieldService, _customListDataService, _projectCustomListService, _projectCustomFieldService, _projectCustomListDataService, unitOfWork);
        }

        [TestMethod]
        public void Index()
        {
            var result = controller.Index() as ViewResult;

            var resultTask = result;

            var model = (FakeDbSet<Project>)resultTask.ViewData.Model;

            Assert.IsNotNull(resultTask);

            Assert.AreEqual(3, model.Count());
        }

        [TestMethod]
        public void Create()
        {
            var result = controller.CreateProject();

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Details()
        {
            var result = controller.ProjectDetails(1);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateWithParam()
        {
            var project = new Project { TableId = 5,ProjectCode="P005", ProjectName = "Proj5", ProjectDescription= "Proj5 Desc" };
            controller.CreateProject(project);
            var projectx = projectService.Queryable().ToList();

            CollectionAssert.Contains(projectx, project);
        }

        [TestMethod]
        public void Edit()
        {
            controller.GetProject(1);
            var project = projectService.Find((1));
            Assert.AreEqual(project.TableId, 1);
        }

        [TestMethod]
        public void EditWithProoductAsParameter()
        {
            var project = new Project { TableId = 1, ProjectCode = "P001", ProjectName = "Proj1", ProjectDescription = "Proj1 Desc New" };
            controller.EditProject(project);

            var projects = projectService.Queryable().ToList();

            CollectionAssert.Contains(projects, project);

        }

        [TestMethod]
        public void Delete()
        {
            controller.DeleteProject(1);
            var project = projectService.Find((1));
            Assert.AreEqual(project.TableId, 1);
        }

        [TestMethod]
        public void DeleteWithProductAsParameter()
        {
            var beforeDeleteCount = projectService.Queryable().ToList().Count;
            var project = projectService.Find((1));
            controller.DeleteProject(project.TableId, project);

            var afterDeleteCount = projectService.Queryable().ToList().Count;

            Assert.AreEqual(afterDeleteCount, beforeDeleteCount - 1);
        }

    }
}