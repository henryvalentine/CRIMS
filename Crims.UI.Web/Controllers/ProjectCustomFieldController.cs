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
    public class ProjectCustomFieldController : Controller
    {
        private IProjectCustomFieldService _projectCustomFieldService;
        private ICustomFieldService _customFieldService;

        private IProjectCustomGroupService _projectCustomGroupService;
        private ICustomGroupService _customGroupService;

        private IUnitOfWorkAsync _unitOfWork;
        
        public ProjectCustomFieldController()
        {

        }

        public ProjectCustomFieldController(IProjectCustomFieldService projectCustomFieldService, ICustomFieldService customFieldService, IUnitOfWorkAsync unitOfWork)
        {
            _projectCustomFieldService = projectCustomFieldService;
            _customFieldService = customFieldService;
            _unitOfWork = unitOfWork;
        }

        public ActionResult GetView()
        {
            return View();
        }
       
        // GET: ProjectCustomField
        public ActionResult Index()
        {
            var projectCustomField = _projectCustomFieldService.Queryable().ToList();
            var cField = new List<ProjectCustomFieldViewModel>();
            if (projectCustomField.Any())
            {
                projectCustomField.ForEach(c =>
                {
                     var cFieldView = new ProjectCustomFieldViewModel
                     {
                         TableId = c.TableId,
                         CustomFieldId = c.CustomFieldId,
                         ProjectCode = c.ProjectCode
                    };

                    var customList = _customFieldService.Query(l => l.CustomListId == c.CustomFieldId).Select().ToList();
                    if (customList.Any())
                    {
                        cFieldView.CustomFieldName = customList[0].CustomFieldName;
                    }
                    
                    cField.Add(cFieldView);

                });
            }

            return View(cField);
        }
        
        // POST: ProjectCustomField/CreateProjectCustomField
        [HttpPost]
        public ActionResult CreateProjectCustomField(ProjectCustomField projectCustomField)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(projectCustomField.ProjectCode))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Code";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(projectCustomField.CustomFieldId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please select custom Field";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                _projectCustomFieldService.Insert(projectCustomField);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Project Custom Field was successfully Created";
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
        public ActionResult ProcessProjectCustomFields(List<ProjectCustomField> projectCustomFields)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (!projectCustomFields.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again later";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (projectCustomFields.Any(p => string.IsNullOrEmpty(p.CustomFieldId) || string.IsNullOrEmpty(p.ProjectCode)))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide all required fields and try again";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                projectCustomFields.ForEach(p =>
                {
                    var pcFields = _projectCustomFieldService.Query(o => o.CustomFieldId == p.CustomFieldId && o.ProjectCode == p.ProjectCode).Select().ToList();
                    if (!pcFields.Any())
                    {
                        _projectCustomFieldService.Insert(p);
                        _unitOfWork.SaveChanges();
                    }
                });

                acResponse.Code = 5;
                acResponse.Message = "Project Custom Field(s) was successfully processed";
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

        // GET: ProjectCustomField/EditProjectCustomField/5
        public ActionResult GetProjectCustomField(int? id)
        {
            if (id == null)
            {
                return Json(new ProjectCustomField(), JsonRequestBehavior.AllowGet);
            }

            ProjectCustomField projectCustomField = _projectCustomFieldService.Find(id);
            if (projectCustomField == null)
            {
                return Json(new ProjectCustomField(), JsonRequestBehavior.AllowGet);
            }
            return Json(projectCustomField, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjectCustomFieldSelectable()
        {
            var selectable = new ProjectCustomFieldSelectable
            {
                CustomFields = _customFieldService.Queryable().ToList()
            };
            return Json(selectable, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjectCustomFields(string projectCode)
        {
            var projectCustomFields = _projectCustomFieldService.Query(p => p.ProjectCode == projectCode).Select().ToList();
            var cField = new List<ProjectCustomFieldViewModel>();
            if (projectCustomFields.Any())
            {
                projectCustomFields.ForEach(c =>
                {
                    var cFieldView = new ProjectCustomFieldViewModel
                    {
                        TableId = c.TableId,
                        CustomFieldId = c.CustomFieldId,
                        ProjectCode = c.ProjectCode
                    };

                    var customList = _customFieldService.Query(l => l.CustomFieldId == c.CustomFieldId).Select().ToList();
                    if (customList.Any())
                    {
                        cFieldView.CustomFieldName = customList[0].CustomFieldName;
                    }

                    cField.Add(cFieldView);

                });
            }
            
            return Json(cField, JsonRequestBehavior.AllowGet);
        }

        // POST: ProjectCustomField/EditProjectCustomField/5
        [HttpPost]
        public ActionResult EditProjectCustomField(ProjectCustomField projectCustomField)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(projectCustomField.CustomFieldId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please select custom Field";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(projectCustomField.ProjectCode))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please project Code";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (projectCustomField.TableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again later.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                _projectCustomFieldService.Update(projectCustomField);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Project Custom Field was successfully updated";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }
        
        // POST: ProjectCustomField/DeleteProjectCustomField/5
        [HttpPost, ActionName("DeleteProjectCustomField")]
        public ActionResult DeleteProjectCustomField(int projectCustomFieldTableId)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (projectCustomFieldTableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid selection.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _projectCustomFieldService.Delete(projectCustomFieldTableId);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Project Custom Field was successfully deleted.";
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
        public ActionResult DeleteProjectCustomFields(List<int> projectCustomFieldTableIds)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (!projectCustomFieldTableIds.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid operation.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                projectCustomFieldTableIds.ForEach(i =>
                {
                    _projectCustomFieldService.Delete(i);
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
