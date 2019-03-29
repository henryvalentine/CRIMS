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
using Crims.UI.Web.Helpers;
using Crims.UI.Web.Models;

namespace Crims.UI.Web.Controllers
{ 
    [Authorize]
    public class CustomFieldController : Controller
    {
        private ICustomFieldService _customFieldService;
        private ICustomListService _customListService;
        private ICustomGroupService _customGroupService;
        private ICustomFieldTypeService _customFieldTypeService;

        private IUnitOfWorkAsync _unitOfWork;
        
        //private OperationStatus _opStatus = new OperationStatus { Status = true };

        public CustomFieldController()
        {

        }

        public CustomFieldController(ICustomFieldService customFieldService, ICustomGroupService customGroupService, ICustomFieldTypeService customFieldTypeService, ICustomListService customListService, IUnitOfWorkAsync unitOfWork)
        {
            _customFieldService = customFieldService;
            _customListService = customListService;
            _customGroupService = customGroupService;
            _customFieldTypeService = customFieldTypeService;
            _unitOfWork = unitOfWork;
        }

        public ActionResult GetView()
        {
            return View();
        }
       
        // GET: CustomField
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetFields(JQueryDataTableParamModel param)
        {
            try
            {
               var customFields = string.IsNullOrEmpty(param.sSearch) ? _customFieldService.Query().Select().OrderBy(f => f.CustomFieldName).Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList()
                    : _customFieldService.Query().Select().Where(s => s.CustomFieldName.ToLower().Contains(param.sSearch.ToLower())).OrderBy(f => f.CustomFieldName).Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                
                var countG = string.IsNullOrEmpty(param.sSearch) ? _customFieldService.Query().Select().Count() : _customFieldService.Query().Select().Count(s => s.CustomFieldName.ToLower().Contains(param.sSearch.ToLower()));
                var cField = new List<CustomFieldViewModel>();
                
                if (customFields.Any())
                {
                    customFields.ForEach(c =>
                    {
                        var cFieldView = new CustomFieldViewModel
                        {
                            TableId = c.TableId,
                            CustomListId = c.CustomListId,
                            CustomFieldName = c.CustomFieldName,
                            CustomFieldId = c.CustomFieldId,
                            CustomGroupId = c.CustomGroupId,
                            FieldTypeId = c.FieldTypeId,
                            CustomFieldSize = c.CustomFieldSize,
                            TabIndex = c.TabIndex,
                            Required = c.Required
                        };
                        var customList = _customListService.Query(l => l.CustomListId == c.CustomListId).Select().ToList();
                        if (customList.Any())
                        {
                            cFieldView.CustomListName = customList[0].CustomListName;
                        }
                        var customGroups = _customGroupService.Query(l => l.CustomGroupId == c.CustomGroupId).Select().ToList();
                        if (customGroups.Any())
                        {
                            cFieldView.CustomGroupName = customGroups[0].GroupName;
                        }
                        var customFieldTypes = _customFieldTypeService.Query(l => l.FieldTypeId == c.FieldTypeId).Select().ToList();
                        if (customFieldTypes.Any())
                        {
                            cFieldView.FieldTypeName = customFieldTypes[0].FieldTypeName;
                        }
                        cField.Add(cFieldView);

                    });
                }
                
                var result = from c in cField
                             select new[] {c.CustomFieldId, c.CustomFieldName, c.FieldTypeName, c.CustomListName, c.CustomGroupName, c.CustomFieldSize, c.TabIndex.ToString() };
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = countG,
                    iTotalDisplayRecords = countG,
                    aaData = result
                },
                   JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = new List<string>()
                },
                   JsonRequestBehavior.AllowGet);
            }
        }

        // POST: CustomField/CreateCustomField
        [HttpPost]
        public ActionResult CreateCustomField(CustomField customField)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(customField.CustomFieldName))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide custom Field Name";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(customField.CustomGroupId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please select custom Group";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(customField.FieldTypeId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please select custom Field Type";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                //if (string.IsNullOrEmpty(customField.CustomListId))
                //{
                //    acResponse.Code = -1;
                //    acResponse.Message = "Please select custom List";
                //    return Json(acResponse, JsonRequestBehavior.AllowGet);
                //}

                //todo : try implement the commented code below for live environment
                //if (customField.FieldTypeId != "List" || customField.FieldTypeId != "Date")
                //{
                //    if (string.IsNullOrEmpty(customField.CustomFieldSize))
                //    {
                //        acResponse.Code = -1;
                //        acResponse.Message = "Please provide Custom Field Size";
                //        return Json(acResponse, JsonRequestBehavior.AllowGet);
                //    }
                //}

                _customFieldService.Insert(customField);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.ResponseId = customField.CustomFieldId;
                acResponse.Message = "Custom Field was successfully Created";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: CustomField/EditCustomField/5
        public ActionResult GetCustomField(string id)
        {
            if (id == null)
            {
                return Json(new CustomField(), JsonRequestBehavior.AllowGet);
            }

            var customFields = _customFieldService.Query(f => f.CustomFieldId == id).Select().ToList();
            if (!customFields.Any())
            {
                return Json(new CustomField(), JsonRequestBehavior.AllowGet);
            }
            return Json(customFields[0], JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomFields(string projectCustomGroupId)
        {
            if (string.IsNullOrEmpty(projectCustomGroupId))
            {
                return Json(new List<CustomField>(), JsonRequestBehavior.AllowGet);
            }
            
            var customFields = _customFieldService.Query(f => f.CustomGroupId == projectCustomGroupId).Select().ToList();
            if (!customFields.Any())
            {
                return Json(new List<CustomField>(), JsonRequestBehavior.AllowGet);
            }
            return Json(customFields, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllCustomFields()
        {
            var customFields = _customFieldService.Queryable().OrderBy(f => f.CustomFieldName).ToList();
            if (!customFields.Any())
            {
                return Json(new List<CustomField>(), JsonRequestBehavior.AllowGet);
            }
            return Json(customFields, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomFieldSelectable()
        {
            var selectable = new CustomFieldSelectable
            {
                CustomLists = _customListService.Queryable().OrderBy(l => l.CustomListName).ToList(),
                CustomFieldTypes = _customFieldTypeService.Queryable().OrderBy(t => t.FieldTypeName).ToList(),
                CustomGroups = _customGroupService.Queryable().OrderBy(g => g.GroupName).ToList()
            };
            return Json(selectable, JsonRequestBehavior.AllowGet);
        }

        // POST: CustomField/EditCustomField/5
        [HttpPost]
        public ActionResult EditCustomField(CustomField customField)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(customField.CustomFieldName))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide custom Field Name";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(customField.CustomGroupId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please select custom Group";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(customField.FieldTypeId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please select custom Field Type";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                //if (string.IsNullOrEmpty(customField.CustomListId))
                //{
                //    acResponse.Code = -1;
                //    acResponse.Message = "Please select custom List";
                //    return Json(acResponse, JsonRequestBehavior.AllowGet);
                //}

                //todo : try implement the commented code below for live environment
                //if (customField.FieldTypeId != "List" || customField.FieldTypeId != "Date")
                //{
                //    if (string.IsNullOrEmpty(customField.CustomFieldSize))
                //    {
                //        acResponse.Code = -1;
                //        acResponse.Message = "Please provide Custom Field Size";
                //        return Json(acResponse, JsonRequestBehavior.AllowGet);
                //    }
                //}

                _customFieldService.Update(customField);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Custom Field was successfully updated";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }
        
        // POST: CustomField/DeleteCustomField/5
        [HttpPost, ActionName("DeleteCustomField")]
        public ActionResult DeleteCustomField(int customFieldTableId)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (customFieldTableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid selection.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _customFieldService.Delete(customFieldTableId);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Custom Field was successfully deleted.";
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
