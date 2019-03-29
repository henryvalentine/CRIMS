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
using AutoMapper;
using Crims.UI.Web.Helpers;
using Crims.UI.Web.Models;

namespace Crims.UI.Web.Controllers
{
    [Authorize]
    public class ProjectCustomListController : Controller
    { 
        private IProjectCustomListService _projectCustomListService;
        private ICustomListService _customListService;
        private IUnitOfWorkAsync _unitOfWork;
        //private OperationStatus _opStatus = new OperationStatus { Status = true };

        public ProjectCustomListController()
        {

        }

        public ProjectCustomListController(ICustomListService customListService, IProjectCustomListService projectCustomListService, IUnitOfWorkAsync unitOfWork)
        {
            _projectCustomListService = projectCustomListService;
            _customListService = customListService;
            _unitOfWork = unitOfWork;

        }

        public ActionResult GetView()
        {
            return View();
        }
        public ActionResult GetProjectCustomLists(string projectCode)
        {
            var projectCustomGroups = _projectCustomListService.Query(p => p.ProjectCode == projectCode).Select().ToList();
            var pcViewList = new List<ProjectCustomListViewModel>();
            if (projectCustomGroups.Any())
            {
                projectCustomGroups.ForEach(c =>
                {
                    var pclView = new ProjectCustomListViewModel
                    {
                        TableId = c.TableId,
                        ProjectCode = c.ProjectCode,
                        CustomListId = c.CustomListId
                    };

                    var groups = _customListService.Query(l => l.CustomListId == c.CustomListId).Select().ToList();

                    if (groups.Any())
                    {
                        pclView.CustomListName = groups[0].CustomListName;
                        pcViewList.Add(pclView);
                    }

                });
            }

            return Json(pcViewList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomLists(string projectCode)
        {
            var projectCustomLists = _projectCustomListService.Query(p => p.ProjectCode == projectCode).Select().ToList();

            var lists = _customListService.Queryable().ToList();

            if (lists.Any())
            {
                if (projectCustomLists.Any())
                {
                    projectCustomLists.ForEach(p =>
                    {
                        lists.Remove(lists.Find(o => o.CustomListId == p.CustomListId));
                    });
                }
            }

            return Json(lists, JsonRequestBehavior.AllowGet);
        }
        // GET: ProjectCustomList
        public ActionResult Index()
        {
            var projectCustomList = _projectCustomListService.Queryable().ToList();
            var cList = new List<ProjectCustomListViewModel>();
            if (projectCustomList.Any())
            {
                projectCustomList.ForEach(c =>
                {
                    var cListView = new ProjectCustomListViewModel
                    {
                        TableId = c.TableId,
                        ProjectCode = c.ProjectCode,
                        CustomListId = c.CustomListId
                    };

                    var parentList = _customListService.Query(l => l.CustomListId == c.CustomListId).Select().ToList();
                    if (parentList.Any())
                    {
                        cListView.CustomListName = parentList[0].CustomListName;
                    }
                    cList.Add(cListView);

                });
            }

            return View(cList);
        }
        
        // POST: ProjectCustomList/CreateProjectCustomList
        [HttpPost]
        public ActionResult CreateProjectCustomList(ProjectCustomList projectCustomList)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(projectCustomList.CustomListId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please select custom List";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(projectCustomList.ProjectCode))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Project Code";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _projectCustomListService.Insert(projectCustomList);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Project Custom List was successfully Created";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ProcessProjectCustomLists(List<ProjectCustomListViewModel> projectCustomLists)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (!projectCustomLists.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again later";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (projectCustomLists.Any(p => string.IsNullOrEmpty(p.CustomListId) || string.IsNullOrEmpty(p.ProjectCode)))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide all required fields and try again";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                projectCustomLists.ForEach(p =>
                {
                    var pcGroups = _projectCustomListService.Query(o => o.CustomListId == p.CustomListId && o.ProjectCode == p.ProjectCode).Select().ToList();
                    if (!pcGroups.Any())
                    {
                        _projectCustomListService.Insert(new ProjectCustomList { TableId = 0, CustomListId = p.CustomListId, ProjectCode = p.ProjectCode });
                        _unitOfWork.SaveChanges();
                    }
                });

                acResponse.Code = 5;
                acResponse.Message = "Project Custom Group(s) was successfully processed";
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

        // GET: ProjectCustomList/EditProjectCustomList/5
        public ActionResult GetProjectCustomList(int? id)
        {
            if (id == null)
            {
                return Json(new ProjectCustomList(), JsonRequestBehavior.AllowGet);
            }

            ProjectCustomList projectCustomList = _projectCustomListService.Find(id);
            if (projectCustomList == null)
            {
                return Json(new ProjectCustomList(), JsonRequestBehavior.AllowGet);
            }
            return Json(projectCustomList, JsonRequestBehavior.AllowGet);
        }
        
        // POST: ProjectCustomList/EditProjectCustomList/5
        [HttpPost]
        public ActionResult EditProjectCustomList(ProjectCustomList projectCustomList)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (projectCustomList.TableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(projectCustomList.CustomListId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please select custom List";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(projectCustomList.ProjectCode))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Project Code";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                _projectCustomListService.Update(projectCustomList);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Custom List was successfully updated";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }
        
        // POST: ProjectCustomList/DeleteProjectCustomList/5
        [HttpPost]
        public ActionResult DeleteProjectCustomList(int projectCustomListTableId)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (projectCustomListTableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid selection.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _projectCustomListService.Delete(projectCustomListTableId);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Custom List was successfully deleted.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteProjectCustomLists(List<int> projectCustomListTableIds)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (!projectCustomListTableIds.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid operation.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                projectCustomListTableIds.ForEach(i =>
                {
                    _projectCustomListService.Delete(i);
                    _unitOfWork.SaveChanges();
                });

                acResponse.Code = 5;
                acResponse.Message = "Selected Item(s) were successfully deleted.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
