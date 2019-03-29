using Crims.Data.Contracts;
using Crims.Data.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crims.UI.Web.Enroll;
using Crims.UI.Web.Enroll.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using CustomDataViewModel = Crims.UI.Web.Enroll.Models.CustomDataViewModel;

namespace Crims.UI.Web.Controllers
{
    public class CustomDataController : Controller
    {
        private ICustomDataService _customDataService;
        private ICustomListService _customListService;
        private ICustomFieldService _customFieldService;
        private IUnitOfWorkAsync _unitOfWork;
        
        //private OperationStatus _opStatus = new OperationStatus { Status = true };

        public CustomDataController()
        {

        }

        public CustomDataController(ICustomFieldService customFieldService, ICustomDataService customDataService, ICustomListService customListService, IUnitOfWorkAsync unitOfWork)
        {
            _customDataService = customDataService;
            _customListService = customListService;
            _customFieldService = customFieldService;
            _unitOfWork = unitOfWork;
        }
        
        [HttpPost]
        public ActionResult ProcessCustomData(List<CustomDataViewModel> customDataList)
        {
            var acResponse = new ActivityResponse { ListDataList  = new List<CustomListData>(), CustomDataList = new List<CustomDataViewModel>()};
            try
            {
                if (!customDataList.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again later";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                if (customDataList.Any(p => string.IsNullOrEmpty(p.CrimsCustomData) && p.IsRequired))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide all required fields and try again";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                customDataList.ForEach(p =>
                {
                    if (string.IsNullOrEmpty(p.CrimsCustomData)) return;
                    if (string.IsNullOrEmpty(p.CustomDataId) || p.CustomDataId == "0")
                    {
                        var data = new CustomData
                        {
                            CustomDataId = Guid.NewGuid().ToString(),
                            CustomFieldId = p.CustomFieldId,
                            EnrollmentId = p.EnrollmentId,
                            CrimsCustomData = p.CrimsCustomData,
                            DateLastUpdated = DateTime.Now,
                            ChildCrimsCustomData = p.ChildCrimsCustomData,
                            CustomListId = p.CustomListId,
                            ProjectSIteId = p.ProjectSIteId
                        };
                        _customDataService.Insert(data);
                        _unitOfWork.SaveChanges();
                        acResponse.CustomDataList.Add(new CustomDataViewModel
                        {
                            CustomDataId = Guid.NewGuid().ToString(),
                            CustomFieldId = p.CustomFieldId,
                            EnrollmentId = p.EnrollmentId,
                            CrimsCustomData = p.CrimsCustomData,
                            DateLastUpdated = DateTime.Now,
                            ChildCrimsCustomData = p.ChildCrimsCustomData,
                            CustomListId = p.CustomListId,
                            ProjectSIteId = p.ProjectSIteId,
                        });
                    }
                    else
                    {
                        var dataListToUpdate =
                            _customDataService.Query(o => o.CustomDataId == p.CustomDataId).Select().ToList();
                        if (dataListToUpdate.Any())
                        {
                            var dataToUpdate = dataListToUpdate[0];
                            p.DateLastUpdated = DateTime.Now;
                            dataToUpdate.CrimsCustomData = p.CrimsCustomData;
                            dataToUpdate.ChildCrimsCustomData = p.ChildCrimsCustomData;
                            dataToUpdate.CustomListId = p.CustomListId;
                            dataToUpdate.CustomFieldId = p.CustomFieldId;
                            _customDataService.Update(dataToUpdate);
                            _unitOfWork.SaveChanges();
                            acResponse.CustomDataList.Add(new CustomDataViewModel
                            {
                                CustomDataId = p.CustomDataId,
                                CustomFieldId = p.CustomFieldId,
                                EnrollmentId = p.EnrollmentId,
                                CrimsCustomData = p.CrimsCustomData,
                                DateLastUpdated = DateTime.Now,
                                ChildCrimsCustomData = p.ChildCrimsCustomData,
                                CustomListId = p.CustomListId
                            });
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

        // GET: CustomData/EditCustomData/5
        public ActionResult GetCustomData(int? id)
        {
            if (id == null)
            {
                return Json(new CustomData(), JsonRequestBehavior.AllowGet);
            }

            CustomData customData = _customDataService.Find(id);
            if (customData == null)
            {
                return Json(new CustomData(), JsonRequestBehavior.AllowGet);
            }
            return Json(customData, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetUserCustomDatas(string enrollmentId)
        {
            if (string.IsNullOrEmpty(enrollmentId))
            {
                return Json(new List<CustomData>(), JsonRequestBehavior.AllowGet);
            }

            var customDatas = _customDataService.Query(f => f.EnrollmentId == enrollmentId).Select().ToList();

            if (!customDatas.Any())
            {
                return Json(new List<CustomData>(), JsonRequestBehavior.AllowGet);
            }

            var viewModels = new List<CustomDataViewModel>();

            customDatas.ForEach(d =>
            {
                var viewModel = new CustomDataViewModel
                {
                    TableId = d.TableId,
                    CustomDataId = d.CustomDataId,
                    CustomFieldId = d.CustomFieldId,
                    EnrollmentId = d.EnrollmentId,
                    ChildCrimsCustomData = d.ChildCrimsCustomData,
                    CrimsCustomData = d.CrimsCustomData,
                    CustomListId = d.CustomListId,
                    ProjectSIteId = d.ProjectSIteId
                };

                if (!string.IsNullOrEmpty(d.CustomListId))
                {
                    var customList = _customListService.Query(f => f.CustomListId == d.CustomListId && string.IsNullOrEmpty(f.ParentListId)).Select().ToList();

                    if (customList.Any())
                    {
                        viewModel.ParentListId = customList[0].ParentListId;
                        var parentList = customList[0];
                        var childList = _customListService.Query(f => !string.IsNullOrEmpty(f.ParentListId) && f.ParentListId == parentList.CustomListId).Select().ToList();
                        if (childList.Any())
                        {
                            viewModel.HasChildren = true;
                        }
                    }
                    
                }

                viewModels.Add(viewModel);
            });
            return Json(viewModels, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult DeleteCustomData(int customDataTableId)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (customDataTableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid selection.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _customDataService.Delete(customDataTableId);
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
        public ActionResult DeleteCustomDataList(List<string> customDataIds)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (!customDataIds.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid operation.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                customDataIds.ForEach(i =>
                {
                    var datasToDelete = _customDataService.Query(o => o.CustomDataId == i).Select().ToList();
                    if (datasToDelete.Any())
                    {
                        var tt = datasToDelete[0];
                        _customDataService.Delete(tt);
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
        public ActionResult DeleteProjectCustomDatas(List<int> projectCustomDataTableIds)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (!projectCustomDataTableIds.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid operation.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                projectCustomDataTableIds.ForEach(i =>
                {
                    _customDataService.Delete(i);
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
        public Project GetProjectInSession()
        {
            try
            {
                var currentProject = Session["_currentProject"] as Project;
                if (string.IsNullOrEmpty(currentProject?.ProjectCode))
                {
                    return new Project();
                }
                return currentProject;
            }
            catch (Exception ex)
            {
                return new Project();
            }
        }
    }
}
