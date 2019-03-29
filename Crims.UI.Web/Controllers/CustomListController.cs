using Crims.Data.Contracts;
using Crims.Data.Models;
using Crims.Data.Services;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class CustomListController : Controller
    {
        private ICustomListService _customListService;
        private ICustomListDataService _customListDataService;
        private IUnitOfWorkAsync _unitOfWork;
        //private OperationStatus _opStatus = new OperationStatus { Status = true };

        public CustomListController()
        {

        }

        public CustomListController(ICustomListDataService customListDataService, ICustomListService customListService, IUnitOfWorkAsync unitOfWork)
        {
            _customListService = customListService;
            _customListDataService = customListDataService;
            _unitOfWork = unitOfWork;

        }

        public ActionResult GetView()
        {
            return View();
        }
       
        // GET: CustomList
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetLists(JQueryDataTableParamModel param)
        {
            try
            {
                var customLists = string.IsNullOrEmpty(param.sSearch) ? _customListService.Query().Select(c => new CustomListViewModel
                {
                    TableId = c.TableId,
                    CustomListId = c.CustomListId,
                    CustomListName = c.CustomListName,
                    ParentListId = c.ParentListId
                }).OrderBy(f => f.CustomListName).Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList()
                     : _customListService.Query().Select(c => new CustomListViewModel
                     {
                         TableId = c.TableId,
                         CustomListId = c.CustomListId,
                         CustomListName = c.CustomListName,
                         ParentListId = c.ParentListId
                     }).Where(s => s.CustomListName.ToLower().Contains(param.sSearch.ToLower())).OrderBy(f => f.CustomListName).Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                var countG = string.IsNullOrEmpty(param.sSearch) ? _customListService.Query().Select().Count() : _customListService.Query().Select().Count(s => s.CustomListName.ToLower().Contains(param.sSearch.ToLower()));
                
                if (customLists.Any())
                {
                    customLists.ForEach(c =>
                    {
                        if (!string.IsNullOrEmpty(c.ParentListId))
                        {
                            var parentList = _customListService.Query(l => l.CustomListId == c.ParentListId).Select().ToList();
                            if (parentList.Any())
                            {
                                c.ParentListName = parentList[0].CustomListName;
                            }

                        }

                    });
                }

                var result = from c in customLists
                             select new[] { c.CustomListId, c.CustomListName, c.ParentListName};
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
        
        // POST: CustomList/CreateCustomList
        [HttpPost]
        public ActionResult CreateCustomList(CustomList customList)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(customList.CustomListName))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide custom List Name";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                _customListService.Insert(customList);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.ResponseId = customList.CustomListId;
                acResponse.Message = "Custom List was successfully Created";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: CustomList/EditCustomList/5
        public ActionResult GetCustomList(string id)
        {
            if (id == null)
            {
                return Json(new CustomList(), JsonRequestBehavior.AllowGet);
            }

            var customLists = _customListService.Query(c => c.CustomListId == id).Select().OrderBy(l => l.CustomListName).ToList();
            if (!customLists.Any())
            {
                return Json(new CustomList(), JsonRequestBehavior.AllowGet);
            }
            return Json(customLists[0], JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomLists()
        {
            var customLists = _customListService.Queryable().OrderBy(l => l.CustomListName);
            if (!customLists.Any())
            {
                return Json(new CustomList(), JsonRequestBehavior.AllowGet);
            }
            return Json(customLists, JsonRequestBehavior.AllowGet);
        }

        // POST: CustomList/EditCustomList/5
        [HttpPost]
        public ActionResult EditCustomList(CustomList customList)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (customList.TableId < 1 || string.IsNullOrEmpty(customList.CustomListId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(customList.CustomListName))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide custom List Name";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                var customLists = _customListService.Query(c => c.CustomListId == customList.CustomListId).Select().ToList();
                if (!customLists.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                var list = customLists[0];
                list.CustomListName = customList.CustomListName;
                list.ParentListId = customList.ParentListId;
                _customListService.Update(list);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.ResponseId = customList.CustomListId;
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
        
        public ActionResult DeleteCustomList(string customListId)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(customListId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid selection.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                var customListDatas = _customListDataService.Query(c => c.CustomListId == customListId).Select().ToList();
                if (customListDatas.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "This Item cannot be deleted. It has dependent data.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                var customLists = _customListService.Query(c => c.CustomListId == customListId).Select().ToList();
                if (!customLists.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "This Item could not be deleted. Please try again later.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                var customList = customLists[0];
                _customListService.Delete(customList);
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

        public ActionResult GetCustomLists(string projectCustomListId)
        {
            if (string.IsNullOrEmpty(projectCustomListId))
            {
                return Json(new List<CustomList>(), JsonRequestBehavior.AllowGet);
            }

            var customLists = _customListService.Query(f => f.CustomListId == projectCustomListId).Select().ToList();
            if (!customLists.Any())
            {
                return Json(new List<CustomList>(), JsonRequestBehavior.AllowGet);
            }
            return Json(customLists, JsonRequestBehavior.AllowGet);
        }
        
    }
}
