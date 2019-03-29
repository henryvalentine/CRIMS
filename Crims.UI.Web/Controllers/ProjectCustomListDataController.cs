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
    public class ProjectCustomListDataController : Controller
    {
        private IProjectCustomListDataService _projectCustomListDataService;
        private ICustomListDataService _customListDataService;
        private IUnitOfWorkAsync _unitOfWork;
        //private OperationStatus _opStatus = new OperationStatus { Status = true };

        public ProjectCustomListDataController()
        {

        } 

        public ProjectCustomListDataController(ICustomListDataService customListDataService, IProjectCustomListDataService projectCustomListDataService, IUnitOfWorkAsync unitOfWork)
        {
            _projectCustomListDataService = projectCustomListDataService;
            _customListDataService = customListDataService;
            _unitOfWork = unitOfWork;

        }
        
        // GET: ProjectCustomListData
        public ActionResult Index()
        {
            var projectCustomListData = _projectCustomListDataService.Queryable().ToList();
            var cListData = new List<ProjectCustomListDataViewModel>();
            if (projectCustomListData.Any())
            {
                projectCustomListData.ForEach(c =>
                {
                    var cListDataView = new ProjectCustomListDataViewModel
                    {
                        TableId = c.TableId,
                        ProjectCode = c.ProjectCode,
                        CustomListDataId = c.CustomListDataId
                    };

                    var parentList = _customListDataService.Query(l => l.CustomListDataId == c.CustomListDataId).Select().ToList();
                    if (parentList.Any())
                    {
                        cListDataView.CustomListDataName = parentList[0].ListDataName;
                    }
                    cListData.Add(cListDataView);

                });
            }

            return View(cListData);
        }
        
        // POST: ProjectCustomListData/CreateProjectCustomListData
        [HttpPost]
        public ActionResult CreateProjectCustomListData(ProjectCustomListData projectCustomListData)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(projectCustomListData.CustomListDataId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please select custom List";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(projectCustomListData.ProjectCode))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Project Code";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _projectCustomListDataService.Insert(projectCustomListData);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Project Custom List Data was successfully Created";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: ProjectCustomListData/EditProjectCustomListData/5
        public ActionResult GetProjectCustomListData(int? id)
        {
            if (id == null)
            {
                return Json(new ProjectCustomListData(), JsonRequestBehavior.AllowGet);
            }

            ProjectCustomListData projectCustomListData = _projectCustomListDataService.Find(id);
            if (projectCustomListData == null)
            {
                return Json(new ProjectCustomListData(), JsonRequestBehavior.AllowGet);
            }
            return Json(projectCustomListData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjectCustomListDatas(string projectCode)
        {
            if (string.IsNullOrEmpty(projectCode))
            {
                return Json(new List<CustomListData>(), JsonRequestBehavior.AllowGet);
            }

            var pclDataList = _projectCustomListDataService.Query(p => p.ProjectCode == projectCode).Select().ToList();

            if (!pclDataList.Any())
            {
                return Json(new List<ProjectCustomListDataViewModel>(), JsonRequestBehavior.AllowGet);
            }

            var pcldViemodels = new List<ProjectCustomListDataViewModel>();

            pclDataList.ForEach(o =>
            {
                var customListDatas = _customListDataService.Query(f => f.CustomListDataId == o.CustomListDataId).Select().ToList();
                if (customListDatas.Any())
                {
                    pcldViemodels.Add(new ProjectCustomListDataViewModel
                    {
                        TableId = o.TableId,
                        CustomListDataId = o.CustomListDataId,
                        ProjectCode = o.ProjectCode,
                        CustomListDataName = customListDatas[0].ListDataName
                    });
                }
            });
            
            return Json(pcldViemodels, JsonRequestBehavior.AllowGet);
        }

        // POST: ProjectCustomListData/EditProjectCustomListData/5
        [HttpPost]
        public ActionResult EditProjectCustomListData(ProjectCustomListData projectCustomListData)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (projectCustomListData.TableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(projectCustomListData.CustomListDataId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please select custom List Data";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(projectCustomListData.ProjectCode))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Project Code";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                _projectCustomListDataService.Update(projectCustomListData);
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
        
        // POST: ProjectCustomListData/DeleteProjectCustomListData/5
        [HttpPost, ActionName("DeleteProjectCustomListData")]
        public ActionResult DeleteProjectCustomListData(int projectCustomListDataTableId)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (projectCustomListDataTableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid selection.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _projectCustomListDataService.Delete(projectCustomListDataTableId);
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
        public ActionResult ProcessProjectCustomListData(List<ProjectCustomListData> projectCustomListData)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (!projectCustomListData.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again later";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (projectCustomListData.Any(p => string.IsNullOrEmpty(p.CustomListDataId) || string.IsNullOrEmpty(p.ProjectCode)))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide all required fields and try again";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                projectCustomListData.ForEach(p =>
                {
                    var pclDatas = _projectCustomListDataService.Query(o => o.CustomListDataId == p.CustomListDataId && o.ProjectCode == p.ProjectCode).Select().ToList();
                    if (!pclDatas.Any())
                    {
                        _projectCustomListDataService.Insert(p);
                        _unitOfWork.SaveChanges();
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
                    _projectCustomListDataService.Delete(i);
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

