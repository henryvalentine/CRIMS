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
    public class CustomGroupControllerTest
    {
        private IDataContextAsync _dummyContext;
        private ICustomGroupService _customGroupService;
        private IQueryable<CustomGroup> _customGroups;
        private IRepositoryAsync<CustomGroup> _customGroupRepository;
        private CustomGroupController _controller;
        private IUnitOfWorkAsync _unitOfWork;

        [TestInitialize()]
        public void Initialize()
        {
            _dummyContext = new DummyDbContext();
            _unitOfWork = new UnitOfWork(_dummyContext);

            _customGroupRepository = new Repository<CustomGroup>(_dummyContext, _unitOfWork);
            _customGroups = new List<CustomGroup>
            {
                new CustomGroup {TableId = 1, GroupName = "Group1", TabIndex= 1, CustomGroupId = "000001"},
                new CustomGroup {TableId = 2, GroupName = "Group2", TabIndex= 2, CustomGroupId = "000002" },
                new CustomGroup {TableId = 3, GroupName = "Group3", TabIndex= 3, CustomGroupId = "000003" },
                new CustomGroup {TableId = 4, GroupName = "Group4", TabIndex= 4, CustomGroupId = "000004"},


            }.AsQueryable();
            _customGroupRepository.InsertRange(_customGroups);
            _unitOfWork.SaveChanges();

            _customGroupService = new CustomGroupService(_customGroupRepository);
            _controller = new CustomGroupController(_customGroupService, _unitOfWork);
        }

        [TestMethod]
        public void Index()
        {
            var result = _controller.Index() as ViewResult;

            var resultTask = result;

            var model = (FakeDbSet<CustomGroup>)resultTask.ViewData.Model;

            Assert.IsNotNull(resultTask);

            Assert.AreEqual(3, model.Count());
        }
        

        [TestMethod]
        public void EditWithProoductAsParameter()
        {
            var customGroup = new CustomGroup { TableId = 1, GroupName = "Group 5", TabIndex = 5, CustomGroupId = "00009"};
            _controller.EditCustomGroup(customGroup);

            var customGroups = _customGroupService.Queryable().ToList();

            CollectionAssert.Contains(customGroups, customGroup);

        }

        [TestMethod]
        public void Delete()
        {
            _controller.DeleteCustomGroup(1);
            var customGroup = _customGroupService.Find((1));
            Assert.AreEqual(customGroup.TableId, 1);
        }

        [TestMethod]
        public void DeleteWithProductAsParameter()
        {
            var beforeDeleteCount = _customGroupService.Queryable().ToList().Count;
            var customGroup = _customGroupService.Find((1));
            _controller.DeleteCustomGroup(customGroup.TableId);

            var afterDeleteCount = _customGroupService.Queryable().ToList().Count;

            Assert.AreEqual(afterDeleteCount, beforeDeleteCount - 1);
        }

    }
}