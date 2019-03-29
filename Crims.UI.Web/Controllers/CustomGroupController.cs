using Crims.Data.Contracts;
using Crims.Data.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Linq;
using System.Web.Mvc;
using Crims.UI.Web.Helpers;

namespace Crims.UI.Web.Controllers
{
    public class CustomGroupController : Controller
    {
        private ICustomGroupService _customGroupService;
        private IUnitOfWorkAsync _unitOfWork;
        //private OperationStatus _opStatus = new OperationStatus { Status = true };
        
        public CustomGroupController()
        {

        }

        public CustomGroupController(ICustomGroupService customGroupService, IUnitOfWorkAsync unitOfWork)
        {
            _customGroupService = customGroupService;
            _unitOfWork = unitOfWork;
        }

        public ActionResult GetView()
        {
            return View();
        }
       
        // GET: CustomGroup
        public ActionResult Index()
        {
            return View(_customGroupService.Queryable().ToList());
        }
        
        // POST: CustomGroup/CreateCustomGroup
        [HttpPost]
        public ActionResult CreateCustomGroup(CustomGroup customGroup)
        {
            var acResponse = new ActivityResponse();
            try
            {

                if (string.IsNullOrEmpty(customGroup.GroupName))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide custom Group Name";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (customGroup.TabIndex < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Custom Group TabIndex";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _customGroupService.Insert(customGroup);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "CustomGroup was successfully Created";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: CustomGroup/EditCustomGroup/5
        public ActionResult GetCustomGroup(int? id)
        {
            if (id == null)
            {
                return Json(new CustomGroup(), JsonRequestBehavior.AllowGet);
            }

            CustomGroup customGroup = _customGroupService.Find(id);
            if (customGroup == null)
            {
                return Json(new CustomGroup(), JsonRequestBehavior.AllowGet);
            }
            return Json(customGroup, JsonRequestBehavior.AllowGet);
        }
        
        // POST: CustomGroup/EditCustomGroup/5
        [HttpPost]
        public ActionResult EditCustomGroup(CustomGroup customGroup)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (customGroup.TableId < 1 || string.IsNullOrEmpty(customGroup.CustomGroupId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(customGroup.GroupName))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide custom Group Name";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (customGroup.TabIndex < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Custom Group TabIndex";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                _customGroupService.Update(customGroup);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Custom Group was successfully updated";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }
        
        // POST: CustomGroup/DeleteCustomGroup/5
        [HttpPost]
        public ActionResult DeleteCustomGroup(int customGroupTableId)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (customGroupTableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid selection.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _customGroupService.Delete(customGroupTableId);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Custom Group was successfully deleted.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetCustomGroups()
        {
            var groups = _customGroupService.Queryable().ToList();
            
            return Json(groups, JsonRequestBehavior.AllowGet);
        }
    }
}
