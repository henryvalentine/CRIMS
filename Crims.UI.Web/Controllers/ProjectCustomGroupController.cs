using Crims.Data.Contracts;
using Crims.Data.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Crims.UI.Web.Helpers;
using Crims.UI.Web.Models;

namespace Crims.UI.Web.Controllers
{
    [Authorize]
    public class ProjectCustomGroupController : Controller
    {
        private IProjectCustomGroupService _projectCustomGroupService;
        private ICustomGroupService _customGroupService;
        private IUnitOfWorkAsync _unitOfWork;
        //private OperationStatus _opStatus = new OperationStatus { Status = true };

        public ProjectCustomGroupController()
        {

        }

        public ProjectCustomGroupController(ICustomGroupService customGroupService, IProjectCustomGroupService projectCustomGroupService, IUnitOfWorkAsync unitOfWork)
        {
            _projectCustomGroupService = projectCustomGroupService;
            _customGroupService = customGroupService;
            _unitOfWork = unitOfWork;

        }

       public ActionResult GetProjectCustomGroups(string projectCode)
       {
            var projectCustomGroups = _projectCustomGroupService.Query(p => p.ProjectCode == projectCode).Select().ToList();
           var pcViewList = new List<ProjectCustomGroupViewModel>();
            if (projectCustomGroups.Any())
            {
                projectCustomGroups.ForEach(c =>
                {
                    var pcgView = new ProjectCustomGroupViewModel
                    {
                        TableId = c.TableId,
                        ProjectCode = c.ProjectCode,
                        CustomGroupId = c.CustomGroupId,
                        TabIndex = c.TabIndex
                    };

                    var groups = _customGroupService.Query(l => l.CustomGroupId == c.CustomGroupId).Select().ToList();

                    if (groups.Any())
                    {
                        pcgView.CustomGroupName = groups[0].GroupName;
                        pcViewList.Add(pcgView);
                    }
                    
                });
            }

            return Json(pcViewList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomGroups(string projectCode)
        {
            var projectCustomGroups = _projectCustomGroupService.Query(p => p.ProjectCode == projectCode).Select().ToList();

            var groups = _customGroupService.Queryable().ToList();

            if (groups.Any())
            {
                if (projectCustomGroups.Any())
                {
                    projectCustomGroups.ForEach(p =>
                    {
                        groups.Remove(groups.Find(o => o.CustomGroupId == p.CustomGroupId));
                    });
                }
            }

            return Json(groups, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult CreateProjectCustomGroup(ProjectCustomGroup projectCustomGroup)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(projectCustomGroup.CustomGroupId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please select at least one custom Group";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(projectCustomGroup.ProjectCode))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Project Code";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _projectCustomGroupService.Insert(projectCustomGroup);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Project Custom Group was successfully Created";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: ProjectCustomGroup/EditProjectCustomGroup/5
        public ActionResult GetProjectCustomGroup(int? id)
        {
            if (id == null)
            {
                return Json(new ProjectCustomGroup(), JsonRequestBehavior.AllowGet);
            }

            ProjectCustomGroup projectCustomGroup = _projectCustomGroupService.Find(id);
            if (projectCustomGroup == null)
            {
                return Json(new ProjectCustomGroup(), JsonRequestBehavior.AllowGet);
            }
            return Json(projectCustomGroup, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ProcessProjectCustomGroups(List<ProjectCustomGroupViewModel> projectCustomGroups)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (!projectCustomGroups.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again later";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (projectCustomGroups.Any(p => p.TabIndex < 1 || string.IsNullOrEmpty(p.CustomGroupId) || string.IsNullOrEmpty(p.ProjectCode)))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide all required fields and try again";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                projectCustomGroups.ForEach(p =>
                {
                    var pcGroups = _projectCustomGroupService.Query(o => o.CustomGroupId == p.CustomGroupId && o.ProjectCode == p.ProjectCode).Select().ToList();
                    if (!pcGroups.Any())
                    {
                        _projectCustomGroupService.Insert(new ProjectCustomGroup {TableId = 0,TabIndex = p.TabIndex, CustomGroupId = p.CustomGroupId, ProjectCode = p.ProjectCode});
                        _unitOfWork.SaveChanges();
                    }
                    else
                    {
                        var pcg = pcGroups[0];
                        pcg.TabIndex = p.TabIndex;
                        _projectCustomGroupService.Update(pcg);
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

        // POST: ProjectCustomGroup/EditProjectCustomGroup/5
        [HttpPost]
        public ActionResult EditProjectCustomGroup(ProjectCustomGroup projectCustomGroup)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (projectCustomGroup.TableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(projectCustomGroup.CustomGroupId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please select custom List Data";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(projectCustomGroup.ProjectCode))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Project Code";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                _projectCustomGroupService.Update(projectCustomGroup);
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
        
        // POST: ProjectCustomGroup/DeleteProjectCustomGroup/5
        [HttpPost, ActionName("DeleteProjectCustomGroup")]
        public ActionResult DeleteProjectCustomGroup(int projectCustomGroupTableId)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (projectCustomGroupTableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid selection.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _projectCustomGroupService.Delete(projectCustomGroupTableId);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Project Custom List Data was successfully deleted.";
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
        public ActionResult DeleteProjectCustomGroupList(List<int> projectCustomGroupTableIds)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (!projectCustomGroupTableIds.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid operation.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                projectCustomGroupTableIds.ForEach(i =>
                {
                    _projectCustomGroupService.Delete(i);
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
