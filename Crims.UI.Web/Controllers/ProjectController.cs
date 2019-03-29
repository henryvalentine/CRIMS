using Crims.Data.Contracts;
using Crims.Data.Models;
using Crims.Data.Services;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Crims.UI.Web.Helpers;
using Crims.UI.Web.Models;

namespace Crims.UI.Web.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private IProjectService _projectService;
        private ICustomListService _customListService;
        private ICustomFieldService _customFieldService;
        private ICustomListDataService _customListDataService;

        private IProjectCustomListService _projectCustomListService;
        private IProjectCustomFieldService _projectCustomFieldService;
        private IProjectCustomListDataService _projectCustomListDataService;

        private IUnitOfWorkAsync _unitOfWork;
        //private OperationStatus _opStatus = new OperationStatus { Status = true };

        public ProjectController()
        {
           
        }

        public ProjectController(IProjectService projectService, ICustomListService customListService, ICustomFieldService customFieldService, ICustomListDataService customListDataService, IProjectCustomListService projectustomListService, IProjectCustomFieldService projectCustomFieldService, IProjectCustomListDataService projectCustomListDataService, IUnitOfWorkAsync unitOfWork)
        {
            _projectService = projectService;
            _customListService = customListService;
            _customFieldService = customFieldService;
            _customListDataService = customListDataService;

            _projectCustomListService = projectustomListService;
            _projectCustomFieldService = projectCustomFieldService;
            _projectCustomListDataService = projectCustomListDataService;

            _unitOfWork = unitOfWork;

        }
        // GET: Project
        public ActionResult Index()
        {

            return View(_projectService.Queryable().ToList());
        }

        public ActionResult GetSummary()
        {
            try
            {
                var dbModel = new DashboardModel
                {
                    TotalProjects = _projectService.Queryable().Count(),
                    TotalExpired = _projectService.Query(p => p.LicenseExpiryDate <= DateTime.Now).Select().Count()
                };

                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }
                return Json(dbModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new DashboardModel(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetProjectCount()
        {
            var projectCount = _projectService.Queryable().Count();

            return Json(projectCount, JsonRequestBehavior.AllowGet);
        }

        // GET: Project/ProjectDetails/5
        public ActionResult ProjectDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = _projectService.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Project/CreateProject
        public ActionResult CreateProject()
        {
            return View();
        }

        // POST: Project/CreateProject
        [HttpPost]
        public ActionResult CreateProject(Project project)
        {
            var acResponse = new ActivityResponse();
            try
            {

                if (string.IsNullOrEmpty(project.ProjectName))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide project Name";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(project.ProjectDescription))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Project Description";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }


                if (string.IsNullOrEmpty(project.ProjectCode))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Project Code";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (project.LicenseExpiryDate.Year <= 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide a valid Project Expiry Date";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                var similarProjects = _projectService.Query(p => p.ProjectCode.ToLower() == project.ProjectCode.ToLower() || p.ProjectName.ToLower() == project.ProjectName.ToLower()).Select().ToList();
                if (similarProjects.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "A Project with Similar Project Code or Name already exists.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                project.DateCreated = DateTime.Now;
                _projectService.Insert(project);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Project was successfully Created";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                ErrorLogger.LogError(e.StackTrace, e.Source, e.Message);
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }
        
        // GET: Project/EditProject/5
        public ActionResult GetProject(int? id)
        {
            if (id == null)
            {
                return Json(new Project(), JsonRequestBehavior.AllowGet);
            }

            var project = _projectService.Find(id);
            
            //

            if (project == null)
            {
                return Json(new ProjectViewModel(), JsonRequestBehavior.AllowGet);
            }

            var pViewModel = new ProjectViewModel
            {
                TableId = project.TableId,
                ProjectName = project.ProjectName,
                ProjectDescription = project.ProjectDescription,
                ProjectCode = project.ProjectCode,
                DateCreated = project.DateCreated,
                LicenceCode = project.LicenceCode,
                ActivationCode = project.ActivationCode,
                OnlineMode = project.OnlineMode,
                LicenseExpiryDate = project.LicenseExpiryDate,
                ProjectCustomFieldViewModels = new List<ProjectCustomFieldViewModel>(),
                ProjectCustomListDataViewModels = new List<ProjectCustomListDataViewModel>(),
                ProjectCustomListViewModels = new List<ProjectCustomListViewModel>()
            };
            //CustomFields CustomLists CustomListDatas
            var cLists = _projectCustomListService.Query(f => f.ProjectCode == pViewModel.ProjectCode).Select().ToList();
            if (cLists.Any())
            {
                cLists.ForEach(f =>
                {
                    pViewModel.ProjectCustomListViewModels.Add(new ProjectCustomListViewModel
                    {
                        TableId = f.TableId,
                        CustomListId = f.CustomListId,
                        ProjectCode = f.ProjectCode,
                        CustomListName = ""
                    }); 
                });
            }

            var cFields = _projectCustomFieldService.Query(f => f.ProjectCode == pViewModel.ProjectCode).Select().ToList();
            if (cFields.Any())
            {
                cFields.ForEach(f =>
                {
                    pViewModel.ProjectCustomFieldViewModels.Add(new ProjectCustomFieldViewModel
                    {
                        TableId = f.TableId,
                        CustomFieldId = f.CustomFieldId,
                        ProjectCode = f.ProjectCode,
                        CustomFieldName = ""
                    });
                });
            }

            var cListDatas = _projectCustomListDataService.Query(f => f.ProjectCode == pViewModel.ProjectCode).Select().ToList();
            if (cListDatas.Any())
            {
                cListDatas.ForEach(f =>
                {
                    pViewModel.ProjectCustomListDataViewModels.Add(new ProjectCustomListDataViewModel
                    {
                        TableId = f.TableId,
                        CustomListDataId = f.CustomListDataId,
                        ProjectCode = f.ProjectCode,
                        CustomListDataName = ""
                    });
                });
            }

            return Json(pViewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEditView()
        {
            return View("EditProject");
        }

        // POST: Project/EditProject/5
        [HttpPost]
        public ActionResult EditProject(Project project)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (project.TableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(project.ProjectName))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide project Name";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(project.ProjectDescription))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Project Description";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }


                if (string.IsNullOrEmpty(project.ProjectCode))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Project Code";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (project.LicenseExpiryDate.Year <= 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide a valid Project Expiry Date";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                var similarProjects = _projectService.Query(p => (p.TableId != project.TableId) && (p.ProjectCode.ToLower() == project.ProjectCode.ToLower() || p.ProjectName.ToLower() == project.ProjectName.ToLower())).Select().ToList();
                if (similarProjects.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "A Project with Similar Project Code or Name already exists.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _projectService.Update(project);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Project was successfully updated";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                ErrorLogger.LogError(e.InnerException.StackTrace, e.InnerException.Source, e.InnerException.Message);
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Project/DeleteProject/5
        public ActionResult DeleteProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = _projectService.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Project/DeleteProject/5
        [HttpPost, ActionName("DeleteProject")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProject(int id, Project project)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Message = ModelState;
                    return RedirectToAction("Index");
                }
                if (id != project.TableId)
                {
                    ViewBag.Message = "Unable to Delete";
                    return RedirectToAction("Index");
                }
                _projectService.Delete(project);
                _unitOfWork.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return RedirectToAction("Index");
            }
        }


        public ActionResult GetCustomLists()
        {
            var customLists = _customListService.Queryable().ToList();
            return Json(customLists, JsonRequestBehavior.AllowGet);
        }
    }
}
