using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Crims.Data.Contracts;
using Crims.Data.Models;
using Crims.UI.Web.Helpers;
using Crims.UI.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Repository.Pattern.UnitOfWork;

namespace Crims.UI.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IProjectService _projectService;
        private IAppSettingService _appSettingService;
        private IApprovalService _approvalService;
        private IBaseDataService _baseDataService;
        private IUnitOfWorkAsync _unitOfWork;
        
        public HomeController()
        {

        }
        public HomeController(IApprovalService approvalService, IProjectService projectService, IBaseDataService baseDataService, IUnitOfWorkAsync unitOfWork, IAppSettingService appSettingService)
        {
            _projectService = projectService;
            _appSettingService = appSettingService;
            _approvalService = approvalService;
            _baseDataService = baseDataService;
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
                return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetSnapShot()
        {
            var projects = _projectService.Queryable().ToList();

            if (!projects.Any())
            {
                return Json(new DashboardModel(), JsonRequestBehavior.AllowGet);
            }

            var dashboardModel = new DashboardModel
            {
                TotalProjects = projects.Count(),
                TotalExpired = projects.Count(p => p.LicenseExpiryDate < DateTime.Today),
                GlobalRecords = _baseDataService.Queryable().Count()
            };

            return Json(dashboardModel, JsonRequestBehavior.AllowGet);
        }
    }
}