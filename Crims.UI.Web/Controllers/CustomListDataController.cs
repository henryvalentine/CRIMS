using Crims.Data.Contracts;
using Crims.Data.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Crims.UI.Web.Helpers;
using Crims.UI.Web.Models;

namespace Crims.UI.Web.Controllers
{
    [Authorize]
    public class CustomListDataController : Controller
    {
        private ICustomListDataService _customListDataService;
        private ICustomListService _customListService;
        private IUnitOfWorkAsync _unitOfWork;
        
        //private OperationStatus _opStatus = new OperationStatus { Status = true };

        public CustomListDataController()
        {

        }

        public CustomListDataController(ICustomListDataService customListDataService, ICustomListService customListService, IUnitOfWorkAsync unitOfWork)
        {
            _customListDataService = customListDataService;
            _customListService = customListService;
            _unitOfWork = unitOfWork;
        }

        public ActionResult GetView()
        {
            return View();
        }
       
        // GET: CustomListData
        public ActionResult Index()
        {
            //list.Add(new CustomListViewModel
            //{
            //    TableId = l.TableId,
            //    CustomListId = l.CustomListId,
            //    CustomListName = l.CustomListName,
            //    ParentListId = l.ParentListId

            //});
           return View();
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }

        // POST: CustomListData/CreateCustomListData
        [HttpPost]
        public ActionResult CreateCustomListData(CustomListData customListData)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(customListData.ListDataName))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide custom List Data Name";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(customListData.CustomListDataId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please select custom List";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                customListData.CustomListDataId = Guid.NewGuid().ToString();
                _customListDataService.Insert(customListData);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Custom List Data was successfully Created";
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
        public ActionResult ProcessCustomListData(List<CustomListDataViewModel> listDataList)
        {
            var acResponse = new ActivityResponse {ListDataList = listDataList, SaveMessages = new List<string>()};
            try
            {
                if (!listDataList.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again later";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (listDataList.Any(p => string.IsNullOrEmpty(p.CustomListId) || string.IsNullOrEmpty(p.ListDataName)))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide all required fields and try again";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                listDataList.ForEach(p =>
                {
                    if (string.IsNullOrEmpty(p.CustomListDataId) || p.CustomListDataId == "0")
                    {
                        var pcFields = _customListDataService.Query(o => o.ListDataName == p.ListDataName && o.CustomListId == p.CustomListId).Select().ToList();
                        if (!pcFields.Any())
                        {
                            var data = new CustomListData
                            {
                                CustomListDataId = Guid.NewGuid().ToString(),
                                CustomListId = p.CustomListId,
                                ListDataName = p.ListDataName
                            };

                            if (!string.IsNullOrEmpty(p.ParentNodeId))
                            {
                                data.ParentNodeId = p.ParentNodeId;
                            }
                            else if (!string.IsNullOrEmpty(p.ParentData))
                            {
                                var pNodeData = _customListDataService.Query(o => o.ListDataName.ToLower() == p.ParentData.ToLower()).Select().ToList();
                                if (pNodeData.Any())
                                {
                                    data.ParentNodeId = pNodeData[0].CustomListDataId;
                                }
                            }
                            
                            _customListDataService.Insert(data);
                            _unitOfWork.SaveChanges();

                        }

                    }
                    else
                    {

                        //Checks if CustomListData Name Exists if the CustomListDataId is not null/empty
                        var dupDatas = _customListDataService.Query(o => o.ListDataName == p.ListDataName && o.CustomListId == p.CustomListId && o.CustomListDataId != p.CustomListDataId).Select().ToList();
                        if (!dupDatas.Any())
                        {
                            var dataListToUpdate = _customListDataService.Query(o => o.CustomListDataId == p.CustomListDataId).Select().ToList();
                            if (dataListToUpdate.Any())
                            {
                                var dataToUpdate = dataListToUpdate[0];
                                if (!string.IsNullOrEmpty(p.ParentNodeId))
                                {
                                    dataToUpdate.ParentNodeId = p.ParentNodeId;
                                }
                                else if (!string.IsNullOrEmpty(p.ParentData))
                                {
                                    var pNodeData = _customListDataService.Query(o => o.ListDataName.ToLower() == p.ParentData.ToLower()).Select().ToList();
                                    if (pNodeData.Any())
                                    {
                                        dataToUpdate.ParentNodeId = pNodeData[0].CustomListDataId;
                                    }
                                }

                                dataToUpdate.ListDataName = p.ListDataName;
                                dataToUpdate.CustomListId = p.CustomListId;
                                _customListDataService.Update(dataToUpdate);
                                 _unitOfWork.SaveChanges();
                            }
                        }

                    }
                    
                });

                acResponse.Code = 5;
                acResponse.Message = "Process was successfully completed";
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

        // GET: CustomListData/EditCustomListData/5
        public ActionResult GetCustomListData(int? id)
        {
            if (id == null)
            {
                return Json(new CustomListData(), JsonRequestBehavior.AllowGet);
            }

            CustomListData customListData = _customListDataService.Find(id);
            if (customListData == null)
            {
                return Json(new CustomListData(), JsonRequestBehavior.AllowGet);
            }
            return Json(customListData, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult GetCustomListDataByCustomList(string customListId, string parentCustomListId)
        {
            var cList = new CustomListViewModel
            {
                ParentListData = new List<CustomListData>(),
                CustomListDatas = new List<CustomListData>()
            };

            if (string.IsNullOrEmpty(customListId))
            {
                return Json(cList, JsonRequestBehavior.AllowGet);
            }

            var list = _customListDataService.Query(c => c.CustomListId == customListId).Select().ToList();

            if (list.Any())
            {
                list.ForEach(l =>
                {
                    cList.CustomListDatas.Add(new CustomListData
                    {
                        TableId = l.TableId,
                        CustomListDataId = l.CustomListDataId,
                        CustomListId = l.CustomListId,
                        ListDataName = l.ListDataName,
                        ParentNodeId = l.ParentNodeId
                    });
                });
            }

            if (!string.IsNullOrEmpty(parentCustomListId))
            {
                var parentListData = _customListDataService.Query(c => c.CustomListId == parentCustomListId).Select().ToList();

                if (parentListData.Any())
                {
                    parentListData.ForEach(l =>
                    {
                        cList.ParentListData.Add(new CustomListData
                        {
                            TableId = l.TableId,
                            CustomListDataId = l.CustomListDataId,
                            CustomListId = l.CustomListId,
                            ListDataName = l.ListDataName,
                            ParentNodeId = l.ParentNodeId
                        });
                    });
                }
            }

            return Json(cList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomListDatas(string projectCustomListId)
        {
            if (string.IsNullOrEmpty(projectCustomListId))
            {
                return Json(new List<CustomListData>(), JsonRequestBehavior.AllowGet);
            }

            var customListDatas = _customListDataService.Query(f => f.CustomListId == projectCustomListId).Select().ToList();
            if (!customListDatas.Any())
            {
                return Json(new List<CustomListData>(), JsonRequestBehavior.AllowGet);
            }
            return Json(customListDatas, JsonRequestBehavior.AllowGet);
        }

        // POST: CustomListData/EditCustomListData/5
        [HttpPost]
        public ActionResult EditCustomListData(CustomListData customListData)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (customListData.TableId < 1 || string.IsNullOrEmpty(customListData.CustomListDataId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(customListData.ListDataName))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide custom List Data Name";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(customListData.CustomListDataId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please select custom List";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _customListDataService.Update(customListData);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Custom List Data was successfully updated";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }
        
        // POST: CustomListData/DeleteCustomListData/5
        [HttpPost, ActionName("DeleteCustomListData")]
        public ActionResult DeleteCustomListData(int customListDataTableId)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (customListDataTableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid selection.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _customListDataService.Delete(customListDataTableId);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Custom List Data was successfully deleted.";
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
        public ActionResult DeleteCustomListDataList(List<string> customListDataIds)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (!customListDataIds.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid operation.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                customListDataIds.ForEach(i =>
                {
                    var datasToDelete = _customListDataService.Query(o => o.CustomListDataId == i).Select().ToList();
                    if (datasToDelete.Any())
                    {
                        var tt = datasToDelete[0];
                        _customListDataService.Delete(tt);
                        _unitOfWork.SaveChanges();
                    }
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

        [HttpPost]
        public ActionResult DeleteProjectCustomListDatas(List<int> projectCustomListDataTableIds)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (!projectCustomListDataTableIds.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid operation.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                projectCustomListDataTableIds.ForEach(i =>
                {
                    _customListDataService.Delete(i);
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

        public ActionResult GetCustomLists()
        {
            var customLists = _customListService.Queryable().ToList();
            if (!customLists.Any())
            {
                return Json(new List<CustomList>(), JsonRequestBehavior.AllowGet);
            }
            return Json(customLists, JsonRequestBehavior.AllowGet);
        }
    }
}
