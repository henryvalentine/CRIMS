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

namespace Crims.UI.Web.Controllers
{
    [Authorize]
    public class CustomFieldTypeController : Controller
    { 
        private ICustomFieldTypeService _customFieldTypeService;
        private IUnitOfWorkAsync _unitOfWork;
        //private OperationStatus _opStatus = new OperationStatus { Status = true };

        public CustomFieldTypeController()
        {

        }

        public CustomFieldTypeController(ICustomFieldTypeService customFieldTypeService, IUnitOfWorkAsync unitOfWork)
        {
            _customFieldTypeService = customFieldTypeService;
            _unitOfWork = unitOfWork;

        }
        // GET: CustomFieldType
        public ActionResult Index()
        {
            var customFieldTypes = _customFieldTypeService.Queryable();
            if (!customFieldTypes.Any())
            {
                return View(new List<CustomFieldType>());
            }
            return View(customFieldTypes.ToList());
        }
        
        // POST: CustomFieldType/CreateCustomFieldType
        [HttpPost]
        public ActionResult CreateCustomFieldType(CustomFieldType customFieldType)
        {
            var acResponse = new ActivityResponse();
            try
            {

                if (string.IsNullOrEmpty(customFieldType.FieldTypeName))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide customFieldType Name";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                _customFieldTypeService.Insert(customFieldType);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Custom Field Type was successfully Created";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: CustomFieldType/EditCustomFieldType/5
        public ActionResult GetCustomFieldType(int? id)
        {
            if (id == null)
            {
                return Json(new CustomFieldType(), JsonRequestBehavior.AllowGet);
            }

            CustomFieldType customFieldType = _customFieldTypeService.Find(id);
            if (customFieldType == null)
            {
                return Json(new CustomFieldType(), JsonRequestBehavior.AllowGet);
            }
            return Json(customFieldType, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetView()
        {
            return View();
        }

        // POST: CustomFieldType/EditCustomFieldType/5
        [HttpPost]
        public ActionResult EditCustomFieldType(CustomFieldType customFieldType)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (customFieldType.TableId < 1 || string.IsNullOrEmpty(customFieldType.FieldTypeId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(customFieldType.FieldTypeName))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide customFieldType Name";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                _customFieldTypeService.Update(customFieldType);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Custom Field Type was successfully updated";
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
        public ActionResult DeleteCustomFieldType(int customFieldTypeTableId)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (customFieldTypeTableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid selection.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _customFieldTypeService.Delete(customFieldTypeTableId);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Custom Field Type was successfully deleted.";
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
