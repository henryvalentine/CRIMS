using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Crims.Data.Contracts;
using Crims.Data.Models;
using Crims.UI.Web.Enroll.Helpers;
using Crims.UI.Web.Enroll.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MySql.Data.MySqlClient;
using Repository.Pattern.UnitOfWork;

namespace Crims.UI.Web.Enroll.Controllers
{
    public class ApprovalController : Controller
    {
        private IApprovalService _approvalService;
        private ICustomDataService _customDataService;
        private ICustomListService _customListService;
        private ICustomFieldService _customFieldService;
        private ICustomListDataService _customListDataService;
        private ICustomGroupService _customGroupService;
        private ICustomFieldTypeService _customFieldTypeService;
        private IBaseDataService _baseDataService;
        private IUnitOfWorkAsync _unitOfWork;
        public ApprovalController()
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Login", "Account");
            }
        }

        public ApprovalController(ICustomFieldService customFieldService, ICustomListDataService customListDataService, ICustomGroupService customGroupService, IBaseDataService baseDataService, ICustomDataService customDataService, ICustomListService customListService, ICustomFieldTypeService customFieldTypeService, IProjectService projectService, IApprovalService approvalService, IUnitOfWorkAsync unitOfWork)
        {
            _approvalService = approvalService;
            _customDataService = customDataService;
            _customFieldTypeService = customFieldTypeService;
            _baseDataService = baseDataService;
            _customGroupService = customGroupService;
            _customListDataService = customListDataService;
            _customListService = customListService;
            _customFieldService = customFieldService;
            _unitOfWork = unitOfWork;
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
        public ActionResult Index()
        {
            var currentProject = GetProjectInSession();
            if (currentProject == null)
            {
                return View(new List<BaseDataViewModel>());
            }
            var userId = User.Identity.GetUserId();
            var registeredUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Where(e => e.Id == userId).Include("UserInfo").ToList();
            if (!registeredUsers.Any())
            {
                return View(new List<BaseDataViewModel>());
            }

            var enrolledBy = registeredUsers[0].UserInfo.Id;
            var approved = (int)EnumManager.ApprovalStatus.Approved;
            var baseDataList = new List<BaseDataViewModel>();

            var connStr = ConfigurationManager.ConnectionStrings["crimsDbEntities"].ConnectionString; ;
            var conn = new MySqlConnection(connStr);
            conn.Open();

            if (User.IsInRole("Admin") || User.IsInRole("Site_Administrator"))
            {
                var sql = $"SELECT t.*, o.* FROM basedatas t JOIN userprofiles o ON o.Id = t.CreatedBy WHERE t.ProjectCode ='{currentProject.ProjectCode}' AND t.ApprovalStatus = '{approved}'";

                var cmd = new MySqlCommand(sql, conn);
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        if (!rdr.HasRows) continue;
                        var dateRegistered = Convert.ToDateTime(rdr["EnrollmentDate"].ToString());
                        var enrollmentDateStr = dateRegistered.ToString("dd/MM/yyyy");
                        var newUser = new BaseDataViewModel
                        {
                            TableId = Convert.ToInt32(rdr["TableId"].ToString()),
                            EnrollmentId = rdr["EnrollmentId"].ToString(),
                            ProjectCode = rdr["ProjectCode"].ToString(),
                            ProjectSiteId = rdr["ProjectSiteId"].ToString(),
                            Surname = rdr["Surname"].ToString(),
                            Firstname = rdr["Firstname"].ToString(),
                            EnrollmentOfficer = rdr["FullName"] + " (" + rdr["PhoneNumber"] + ")",
                            MiddleName = rdr["MiddleName"].ToString(),
                            Gender = rdr["Gender"].ToString(),
                            Title = rdr["Title"].ToString(),
                            Email = rdr["Email"].ToString(),
                            EnrollmentDateStr = enrollmentDateStr,
                            MobileNumber = rdr["MobileNumber"].ToString(),
                            CuntryCode = rdr["CuntryCode"].ToString(),
                            ProjectPrimaryCode = rdr["ProjectPrimaryCode"].ToString(),
                            DOB = rdr["DOB"].ToString(),
                            BiometricStatus = "Completed",
                            ApprovalStatus = Convert.ToInt32(rdr["ApprovalStatus"].ToString()),
                            EnrollmentDate = dateRegistered,
                            ValidIdNumber = rdr["ValidIdNumber"].ToString()
                        };

                        baseDataList.Add(newUser);
                    }
                }
            }

            else
            {
                if (!User.IsInRole("Enrollee"))
                {
                    var sql2 = $"SELECT t.*, o.* FROM basedatas t JOIN userprofiles o ON o.Id = t.CreatedBy WHERE t.ProjectCode ='{currentProject.ProjectCode}' AND  t.ApprovalStatus = '{approved}' AND (t.CreatedBy = '{enrolledBy}' OR t.LastUpdatedby = '{enrolledBy}')";

                    var cmd2 = new MySqlCommand(sql2, conn);
                    using (var rdr = cmd2.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (!rdr.HasRows) continue;
                            var dateRegistered = Convert.ToDateTime(rdr["EnrollmentDate"].ToString());
                            var enrollmentDateStr = dateRegistered.ToString("dd/MM/yyyy");
                            var newUser = new BaseDataViewModel
                            {
                                TableId = Convert.ToInt32(rdr["TableId"].ToString()),
                                EnrollmentId = rdr["EnrollmentId"].ToString(),
                                ProjectCode = rdr["ProjectCode"].ToString(),
                                ProjectSiteId = rdr["ProjectSiteId"].ToString(),
                                Surname = rdr["Surname"].ToString(),
                                Firstname = rdr["Firstname"].ToString(),
                                EnrollmentOfficer = rdr["FullName"] + " (" + rdr["PhoneNumber"] + ")",
                                MiddleName = rdr["MiddleName"].ToString(),
                                Gender = rdr["Gender"].ToString(),
                                Title = rdr["Title"].ToString(),
                                EnrollmentDateStr = enrollmentDateStr,
                                Email = rdr["Email"].ToString(),
                                MobileNumber = rdr["MobileNumber"].ToString(),
                                CuntryCode = rdr["CuntryCode"].ToString(),
                                ProjectPrimaryCode = rdr["ProjectPrimaryCode"].ToString(),
                                DOB = rdr["DOB"].ToString(),
                                BiometricStatus = "Completed",
                                ApprovalStatus = Convert.ToInt32(rdr["ApprovalStatus"].ToString()),
                                EnrollmentDate = dateRegistered,
                                ValidIdNumber = rdr["ValidIdNumber"].ToString()
                            };

                            baseDataList.Add(newUser);
                        }
                    }
                }
                else
                {
                    return Redirect("Enrollment");
                }
            }
        
            if (!baseDataList.Any())
            {
                return View(new List<BaseDataViewModel>());
            }

            baseDataList.ForEach(p =>
            { 
                var enumName = Enum.GetName(typeof(EnumManager.ApprovalStatus), p.ApprovalStatus);
                if (enumName != null)p.ApprovalStatusStr = enumName.Replace("_", " ");
            });

            return View(baseDataList);
        }

        [HttpPost]
        public ActionResult ProcessApproval(Approval approval)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Your session has timed out. Please log in to continue";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                var userId = User.Identity.GetUserId();
                var registeredUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Where(e => e.Id == userId).Include("UserInfo").ToList();
                if (!registeredUsers.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please try again or contact the support team.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(approval.EnrollmentId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An unknown error was encountered. Please refresg the page and try again";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                if (approval.ApprovalStatus < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Approval Status";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
               
                var baseDatas = _baseDataService.Query(b => b.EnrollmentId == approval.EnrollmentId).Select().ToList();
                var baseData = baseDatas[0];
                baseData.DateLastUpdated = DateTime.Now;
                baseData.ApprovalStatus = approval.ApprovalStatus;
                _unitOfWork.SaveChanges();

                approval.DateProcessed = baseData.DateLastUpdated;
                
                approval.ProcessedById = registeredUsers[0].UserInfo.Id;

                if (string.IsNullOrEmpty(approval.ApprovalId))
                {
                    approval.ApprovalId = Guid.NewGuid().ToString();
                    _approvalService.Insert(approval);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    _approvalService.Update(approval);
                    _unitOfWork.SaveChanges();
                }

                acResponse.Code = 5;
                acResponse.Message = "Approval Process was successfully completed";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetApproval(int id)
        {
            try
            {
                if (id < 1)
                {
                    return Json(new Approval(), JsonRequestBehavior.AllowGet);
                }

                var querries = _approvalService.Query(b => b.TableId == id).Select().ToList();
                if (!querries.Any())
                {
                    return Json(new Approval(), JsonRequestBehavior.AllowGet);
                }

                var query = querries[0];
                
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new Approval(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetApprovalHistories(string enrollmentId)
        {
            try
            {
                if (string.IsNullOrEmpty(enrollmentId))
                {
                    return Json(new Approval(), JsonRequestBehavior.AllowGet);
                }

                var baseDataList = _baseDataService.Query(b => b.EnrollmentId == enrollmentId).Select(b => new BaseDataViewModel
                {
                    ProjectCode = b.ProjectCode,
                    ProjectSiteId = b.ProjectSiteId,
                    EnrollmentId = b.EnrollmentId,
                    Surname = b.Surname,
                    Firstname = b.Firstname,
                    MiddleName = b.MiddleName,
                    Gender = b.Gender,
                    Title = b.Title,
                    Email = b.Email,
                    MobileNumber = b.MobileNumber,
                    CuntryCode = b.CuntryCode,
                    ProjectPrimaryCode = b.ProjectPrimaryCode,
                    DOB = b.DOB,
                    ApprovalStatus = b.ApprovalStatus,
                    EnrollmentDate = b.EnrollmentDate,
                    ValidIdNumber = b.ValidIdNumber

                }).ToList();

                if (!baseDataList.Any())
                {
                    return Json(new Approval(), JsonRequestBehavior.AllowGet);
                }

                var baseData = baseDataList[0];
                
                var enumName = Enum.GetName(typeof(EnumManager.ApprovalStatus), baseData.ApprovalStatus);
                if (enumName != null) baseData.ApprovalStatusStr = enumName.Replace("_", " ");
             
                var querries = _approvalService.Query(b => b.EnrollmentId == baseData.EnrollmentId).Select(a => new ApprovalViewModel
                {
                    TableId = a.TableId,
                    ApprovalId = a.ApprovalId,
                    Comment = a.Comment,
                    EnrollmentId = a.EnrollmentId,
                    DateProcessed = a.DateProcessed,
                    ProcessedById = a.ProcessedById,
                    ApprovalStatus = a.ApprovalStatus

                }).OrderBy(c => c.DateProcessed).ToList();

                if (!querries.Any())
                {
                    return Json(new Approval(), JsonRequestBehavior.AllowGet);
                }
                querries.ForEach(h =>
                {
                    var enumName2 = Enum.GetName(typeof(EnumManager.ApprovalStatus), baseData.ApprovalStatus);
                    if (enumName2 != null) h.ApprovalStatusStr = enumName2.Replace("_", " ");
                    h.DateProcessedStr = h.DateProcessed.ToString("dd/MM/yyyy");
                    
                    var approvers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Where(e => e.UserInfo.Id == h.ProcessedById).Include("UserInfo").ToList();
                    if (approvers.Any())
                    {
                        h.ProcessedByName = approvers[0].UserInfo.FullName;
                    }
                    
                });

                baseData.ApprovalHistories = querries;

                return Json(baseData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new Approval(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteApproval(int approvalTableId)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (approvalTableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid selection.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _approvalService.Delete(approvalTableId);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Base Data was successfully deleted.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetCustomDataGroupAndFields(string baseDataId)
        {
            var genObj = new GenericObjectModel
            {
                CustomGroupViewModels = new List<CustomGroupViewModel>()
            };

            try
            {
                var baseDataList = _baseDataService.Query(b => b.EnrollmentId == baseDataId).Select().ToList();
                if (!baseDataList.Any())
                {
                    return Json(genObj, JsonRequestBehavior.AllowGet);
                }

                var baseData = baseDataList[0];

                genObj.Name = baseData.Firstname + " " + baseData.Surname;
                genObj.ProjectSiteId = baseData.ProjectSiteId;
                genObj.EnrollmentId = baseData.EnrollmentId;
                genObj.ProjectPrimaryCode = baseData.ProjectPrimaryCode;
                
                var customGroups = new List<CustomGroupViewModel>();

                var enrolledCustomDataList = _customDataService.Query(q => q.EnrollmentId == baseDataId).Select().ToList();
                if (!enrolledCustomDataList.Any())
                {
                    return Json(genObj, JsonRequestBehavior.AllowGet);
                }

                enrolledCustomDataList.ForEach(r =>
                {
                    var customFields = _customFieldService.Query(g => g.CustomFieldId == r.CustomFieldId).Select().ToList();

                    if (customFields.Any())
                    {
                        var cFieldView = customFields[0];
                        var customFieldTypes = _customFieldTypeService.Query(f => f.FieldTypeId == cFieldView.FieldTypeId).Select().ToList();
                        if (!customFieldTypes.Any())
                        {
                            return;
                        }

                        var groupF = customGroups.Find(t => t.CustomGroupId == cFieldView.CustomGroupId);

                        if (groupF == null || groupF.TableId < 1)
                        {
                            var fieldCustomGroups = _customGroupService.Query(o => o.CustomGroupId == cFieldView.CustomGroupId).Select().ToList();

                            if (!fieldCustomGroups.Any())
                            {
                                return;
                            }
                            var g = fieldCustomGroups[0];
                            groupF = new CustomGroupViewModel
                            {
                                TableId = g.TableId,
                                CustomGroupId = g.CustomGroupId,
                                GroupName = g.GroupName,
                                TabIndex = g.TabIndex,
                                CustomFieldViewModels = new List<GenericViewModel>()
                            };
                        }

                        var customFieldType = customFieldTypes[0];
                        var customFieldViewModel = new GenericViewModel
                        {
                            FieldTypeName = customFieldTypes[0].FieldTypeName
                        };

                        if (!string.IsNullOrEmpty(r.CustomListId))
                        {
                            var customList = _customListService.Query(s => s.CustomListId == r.CustomListId && string.IsNullOrEmpty(s.ParentListId)).Select().ToList();
                            if (customList.Any())
                            {
                                var list = customList[0];

                                customFieldViewModel.CustomListId = r.CustomListId;
                                var l1Data = _customListDataService.Query(s => s.CustomListDataId == r.CrimsCustomData).Select().ToList();

                                if (l1Data.Any())
                                {
                                    customFieldViewModel.CustomListDataName = l1Data[0].ListDataName;
                                }

                                var childList = _customListService.Query(s => s.ParentListId == list.CustomListId).Select().ToList();
                                if (childList.Any())
                                {
                                    customFieldViewModel.HasChildren = true;
                                }

                                customFieldViewModel.TableId = r.TableId;
                                customFieldViewModel.TabIndex = cFieldView.TabIndex;
                                customFieldViewModel.CustomFieldId = r.CustomFieldId;
                                customFieldViewModel.CustomFieldName = cFieldView.CustomFieldName;
                                customFieldViewModel.CustomListId = cFieldView.CustomListId;
                                customFieldViewModel.CustomListName = list.CustomListName;
                                customFieldViewModel.CustomGroupId = cFieldView.CustomGroupId;
                                customFieldViewModel.FieldTypeId = cFieldView.FieldTypeId;
                                customFieldViewModel.FieldTypeName = customFieldType.FieldTypeName;
                                customFieldViewModel.CrimsCustomData = r.CrimsCustomData;
                                customFieldViewModel.EnrollmentId = r.EnrollmentId;
                                customFieldViewModel.CustomDataId = r.CustomDataId;
                            }
                        }
                        else
                        {
                            customFieldViewModel = new GenericViewModel
                            {
                                TableId = r.TableId,
                                CustomFieldId = r.CustomFieldId,
                                CustomFieldName = cFieldView.CustomFieldName,
                                CustomListId = cFieldView.CustomListId,
                                TabIndex = cFieldView.TabIndex,
                                CustomGroupId = cFieldView.CustomGroupId,
                                FieldTypeId = cFieldView.FieldTypeId,
                                CustomFieldType = customFieldType,
                                FieldTypeName = customFieldType.FieldTypeName,
                                CrimsCustomData = r.CrimsCustomData,
                                EnrollmentId = r.EnrollmentId,
                                CustomDataId = r.CustomDataId,
                            };

                        }
                        
                        if (customGroups.Exists(t => t.CustomGroupId == groupF.CustomGroupId))
                        {
                            groupF.CustomFieldViewModels.Add(customFieldViewModel);
                        }
                        else
                        {
                            groupF.CustomFieldViewModels.Add(customFieldViewModel);
                            customGroups.Add(groupF);
                        }
                    }

                });

                
                customGroups.ForEach(g =>
                {
                    if (g.CustomFieldViewModels != null && g.CustomFieldViewModels.Any())
                    {
                        g.CustomFieldViewModels = g.CustomFieldViewModels.OrderBy(f => f.TabIndex).ToList();
                    }
                });

                customGroups = customGroups.OrderBy(m => m.TabIndex).ToList();
                genObj.CustomGroupViewModels = customGroups;
                return Json(genObj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new GenericObjectModel(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetCustomLists(string customListId)
        {
            try
            {
                var customList = _customListService.Query(s => s.CustomListId == customListId).Select().ToList();

                if (customList.Any())
                {
                    var p = customList[0];

                    var l1 = new CustomListViewModel
                    {
                        CustomListName = p.CustomListName,
                        CustomListId = p.CustomListId,
                        ParentListId = p.ParentListId,
                        CustomListDatas = new List<CustomListData>()
                    };

                    var l1Data = _customListDataService.Query(s => s.CustomListId == l1.CustomListId).Select().ToList();

                    if (l1Data.Any())
                    {
                        l1.CustomListDatas = l1Data;
                    }

                    var childList = _customListService.Query(s => s.ParentListId == p.CustomListId).Select().ToList();

                    if (childList.Any())
                    {
                        l1.HasChildren = true;
                    }

                    return Json(l1, JsonRequestBehavior.AllowGet);
                }
                return Json(new List<CustomGroupViewModel>(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new List<CustomGroupViewModel>(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetCustomListApprovalDataByParentList(List<ParentListModel> parentModels)
        {
            try
            {
                if (!parentModels.Any())
                {
                    return Json(new List<CustomGroupViewModel>(), JsonRequestBehavior.AllowGet);
                }

                var listViewModels = new List<GenericViewModel>();

                parentModels.ForEach(r =>
                {
                    var customList = _customListService.Query(s => s.ParentListId == r.ParentListId).Select().ToList();

                    if (customList.Any())
                    {
                        var l1 = customList[0];
                        
                        var customFieldViewModel = new GenericViewModel();
                        
                        var customDataList = _customDataService.Query(d => d.CustomListId == l1.CustomListId).Select().ToList();
                        if (!customDataList.Any())
                        {
                            return;
                        }


                        var customData = customDataList[0];

                        var customListData = _customListDataService.Query(s => s.CustomListDataId == customData.CrimsCustomData).Select().ToList();
                        if (customListData.Any())
                        {
                            customFieldViewModel.CustomListDataName = customListData[0].ListDataName;
                        }

                        var customFields = _customFieldService.Query(q => q.CustomListId == l1.CustomListId).Select().ToList();
                        if (!customFields.Any())
                        {
                            return;
                        }
                        
                        var cFieldView = customFields[0];
                        
                        var childList = _customListService.Query(s => s.ParentListId == l1.CustomListId).Select().ToList();
                        if (childList.Any())
                        {
                            customFieldViewModel.HasChildren = true;
                        }

                        customFieldViewModel.TableId = customData.TableId;
                        customFieldViewModel.TabIndex = cFieldView.TabIndex;
                        customFieldViewModel.CustomFieldId = customData.CustomFieldId;
                        customFieldViewModel.CustomFieldName = cFieldView.CustomFieldName;
                        customFieldViewModel.CustomListId = customData.CustomListId;
                        customFieldViewModel.CustomGroupId = cFieldView.CustomGroupId;
                        customFieldViewModel.FieldTypeId = cFieldView.FieldTypeId;
                        customFieldViewModel.PrecedingField = r.PrecedingField;
                        customFieldViewModel.CrimsCustomData = customData.CrimsCustomData;
                        customFieldViewModel.EnrollmentId = customData.EnrollmentId;
                        customFieldViewModel.CustomDataId = customData.CustomDataId;
                        listViewModels.Add(customFieldViewModel);
                    }
                });

                return Json(listViewModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<CustomGroupViewModel>(), JsonRequestBehavior.AllowGet);
            }
        }

    }
}
