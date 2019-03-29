using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Crims.Data.Contracts;
using Crims.Data.Models;
using Crims.UI.Web.Enroll.Helpers;
using Crims.UI.Web.Enroll.Models;
using Newtonsoft.Json;
using Repository.Pattern.UnitOfWork;

namespace Crims.UI.Web.Enroll.Controllers
{
    public class ProjectCustomDataController : Controller  //  
    {
        private IProjectCustomListDataService _projectCustomListDataService;
        private IProjectCustomGroupService _projectCustomGroupService;
        private ICustomListDataService _customListDataService;
        private ICustomFieldService _customFieldService;
        private ICustomListService _customListService;
        private ICustomGroupService _customGroupService;
        private ICustomFieldTypeService _customFieldTypeService;
        private IBaseDataService _baseDataService;
        private IUnitOfWorkAsync _unitOfWork;
        //private OperationStatus _opStatus = new OperationStatus { Status = true };

        public ProjectCustomDataController()
        {

        } 

        public ProjectCustomDataController(IProjectCustomGroupService projectCustomGroupService, ICustomFieldService customFieldService, IBaseDataService baseDataService, ICustomGroupService customGroupService, ICustomFieldTypeService customFieldTypeService, ICustomListService customListService, ICustomListDataService customListDataService, IProjectCustomListDataService projectCustomListDataService, IUnitOfWorkAsync unitOfWork)
        {
            _projectCustomListDataService = projectCustomListDataService;
            _customListDataService = customListDataService;
            _projectCustomGroupService = projectCustomGroupService;
            _baseDataService = baseDataService;
            _customFieldService = customFieldService;
            _customListService = customListService;
            _customGroupService = customGroupService;
            _customFieldTypeService = customFieldTypeService;
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

        public ActionResult GetCustomDataGroupFields_New(string baseDataId)
        {
            var genObj = new GenericObjectModel
            {
                CustomGroupViewModels = new List<CustomGroupViewModel>()
            };
            try
            {
                var currentProject = GetProjectInSession();
                if (string.IsNullOrEmpty(currentProject?.ProjectCode))
                {
                    return Json(new List<CustomGroupViewModel>(), JsonRequestBehavior.AllowGet);
                }
                var projectCustomGroups = _projectCustomGroupService.Query(g => g.ProjectCode == currentProject.ProjectCode).Select().ToList();
                if (!projectCustomGroups.Any())
                {
                    return Json(new List<CustomGroupViewModel>(), JsonRequestBehavior.AllowGet);
                }
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
                genObj.FormPath = baseData.FormPath;

                var customGroups = new List<CustomGroupViewModel>();
                projectCustomGroups.ForEach(t =>
                {
                    var newGroups = _customGroupService.Query(d => d.CustomGroupId == t.CustomGroupId).Select(g => new CustomGroupViewModel
                    {
                        TableId = g.TableId,
                        CustomGroupId = g.CustomGroupId,
                        GroupName = g.GroupName,
                        TabIndex = g.TabIndex

                    }).ToList();

                    if (newGroups.Any())
                    {
                        customGroups.Add(newGroups[0]);
                    }
                });
                

                if (!customGroups.Any())
                {
                    return Json(genObj, JsonRequestBehavior.AllowGet);
                }

                customGroups.ForEach(c =>
                {
                    c.CustomFieldViewModels = new List<GenericViewModel>();

                    var customFields = _customFieldService.Query(g => g.CustomGroupId == c.CustomGroupId).Select().ToList();

                    if (customFields.Any())
                    {
                        customFields.ForEach(cFieldView =>
                        {
                            var customFieldTypes = _customFieldTypeService.Query(f => f.FieldTypeId == cFieldView.FieldTypeId).Select().ToList();
                            if (!customFieldTypes.Any())
                            {
                                return;
                            }

                            var customFieldType = customFieldTypes[0];
                            var customFieldViewModel = new GenericViewModel
                            {
                                CustomList = new CustomListViewModel(),
                                CustomFieldType = new CustomFieldType()
                            };

                            if (customFieldType.FieldTypeName == "List")
                            {
                                var customList = _customListService.Query(s => s.CustomListId == cFieldView.CustomListId && string.IsNullOrEmpty(s.ParentListId)).Select().ToList();
                                if (customList.Any())
                                {
                                    var list = customList[0];
                                    var l1 = new CustomListViewModel
                                    {
                                        CustomListName = list.CustomListName,
                                        CustomListId = list.CustomListId,
                                        HasChildren = false,
                                        ParentListId = list.ParentListId,
                                        CustomListDatas = new List<CustomListData>()
                                    };

                                    //var l1Data = _customListDataService.Query(s => s.CustomListId == l1.CustomListId).Select().ToList();

                                    //if (l1Data.Any())
                                    //{
                                    //    l1.CustomListDatas = l1Data;
                                    //}

                                    var childList =  _customListService.Query(s => s.ParentListId == list.CustomListId).Select().ToList();
                                    if (childList.Any())
                                    {
                                        l1.HasChildren = true;
                                    }

                                    customFieldViewModel.TableId = cFieldView.TableId;
                                    customFieldViewModel.CustomFieldId = cFieldView.CustomFieldId;
                                    customFieldViewModel.CustomFieldName = cFieldView.CustomFieldName;
                                    customFieldViewModel.CustomFieldSize = cFieldView.CustomFieldSize;
                                    customFieldViewModel.CustomListId = cFieldView.CustomListId;
                                    customFieldViewModel.ParentFieldId = cFieldView.ParentFieldId;
                                    customFieldViewModel.CustomGroupId = cFieldView.CustomGroupId;
                                    customFieldViewModel.FieldTypeId = cFieldView.FieldTypeId;
                                    customFieldViewModel.TabIndex = cFieldView.TabIndex;
                                    customFieldViewModel.Required = cFieldView.Required;
                                    customFieldViewModel.CustomFieldType = customFieldType;
                                    customFieldViewModel.CustomList = l1;
                                }
                            }
                            else
                            {
                                customFieldViewModel = new GenericViewModel
                                {
                                    TableId = cFieldView.TableId,
                                    CustomFieldId = cFieldView.CustomFieldId,
                                    CustomFieldName = cFieldView.CustomFieldName,
                                    CustomFieldSize = cFieldView.CustomFieldSize,
                                    ParentFieldId = cFieldView.ParentFieldId,
                                    CustomListId = cFieldView.CustomListId,
                                    CustomGroupId = cFieldView.CustomGroupId,
                                    FieldTypeId = cFieldView.FieldTypeId,
                                    TabIndex = cFieldView.TabIndex,
                                    Required = cFieldView.Required,
                                    CustomFieldType = customFieldType
                                };
                            }
                           
                            c.CustomFieldViewModels.Add(customFieldViewModel);
                            
                        });
                    }
                    genObj.CustomGroupViewModels.Add(c);
                });

                genObj.CustomGroupViewModels.ForEach(g =>
                {
                    g.CustomFieldViewModels = g.CustomFieldViewModels.OrderBy(f => f.TabIndex).ToList();
                });
                
                var ordered = genObj.CustomGroupViewModels.OrderBy(m => m.TabIndex).ToList();
                return Json(ordered, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<CustomGroupViewModel>(), JsonRequestBehavior.AllowGet);
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

        public ActionResult GetCustomListDatasByParentList(string parentListId, string parentNode, string parentCustomFieldId)
        {
            try
            {
                var customList = _customListService.Query(q => q.ParentListId == parentListId).Select(p => new CustomListViewModel
                {
                    CustomListName = p.CustomListName,
                    CustomListId = p.CustomListId,
                    ParentListId = p.ParentListId
                }).ToList();

                if (!customList.Any())
                {
                    return Json(new CustomListViewModel(), JsonRequestBehavior.AllowGet);
                }
                var l1 = customList[0];
                List<CustomField> customFields;
                CustomField customField;

                if (customList.Count > 1)
                {
                     customFields = _customFieldService.Query(q => q.ParentFieldId == parentCustomFieldId).Select().ToList();
                    if (!customFields.Any())
                    {
                        return Json(new CustomListViewModel(), JsonRequestBehavior.AllowGet);
                    }

                    customField = customFields[0];

                    customList = _customListService.Query(q => q.CustomListId == customField.CustomListId).Select(p => new CustomListViewModel
                    {
                        CustomListName = p.CustomListName,
                        CustomListId = p.CustomListId,
                        ParentListId = p.ParentListId
                    }).ToList();

                    if (!customList.Any())
                    {
                        return Json(new CustomListViewModel(), JsonRequestBehavior.AllowGet);
                    }

                    l1 = customList[0];
                }
                else
                {
                    l1 = customList[0];
                    customFields = _customFieldService.Query(q => q.ParentFieldId == parentCustomFieldId).Select().ToList();
                    if (!customFields.Any())
                    {
                        customFields = _customFieldService.Query(q => q.CustomListId == l1.CustomListId).Select().ToList();
                        if (!customFields.Any())
                        {
                            return Json(new CustomListViewModel(), JsonRequestBehavior.AllowGet);
                        }
                        customField = customFields[0];
                    }
                    else
                    {
                        customField = customFields[0];
                    }
                }
                
                l1.CustomListDatas = new List<CustomListData>();
                l1.CustomFieldId = customField.CustomFieldId;
                l1.ParentFieldId = customField.ParentFieldId;
                l1.CustomFieldName = customField.CustomFieldName;
                l1.CustomFieldSize = customField.CustomFieldSize;
                l1.CustomGroupId = customField.CustomGroupId;
                l1.Required = customField.Required;

                var l1Data = _customListDataService.Query(s => s.CustomListId == l1.CustomListId && s.ParentNodeId == parentNode).Select().ToList();

                if (l1Data.Any())
                {
                    l1.CustomListDatas = l1Data;
                }
                //else
                //{
                //    l1Data = _customListDataService.Query(s => s.CustomListId == l1.CustomListId).Select().ToList();
                //    if (l1Data.Any())
                //    {
                //        l1.CustomListDatas = l1Data;
                //    }
                //    else
                //    {
                //        return Json(new CustomListViewModel(), JsonRequestBehavior.AllowGet);
                //    }
                //}

                var childList = _customListService.Query(s => s.ParentListId == l1.CustomListId).Select().ToList();

                if (childList.Any())
                {
                    l1.HasChildren = true;
                }

                return Json(l1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<CustomListViewModel>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetCustomListDatas(string listId)
        {
            try
            {
                var listData = new List<CustomListData>();
                listData = _customListDataService.Query(s => s.CustomListId == listId).Select().ToList();
                return Json(listData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<CustomListData>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetCustomDataGroupFields(string baseDataId)
        {
            var genObj = new GenericObjectModel
            {
                CustomGroupViewModels = new List<CustomGroupViewModel>()
            };
            try
            {
                var currentProject = GetProjectInSession();
                if (string.IsNullOrEmpty(currentProject?.ProjectCode))
                {
                    return Json(new List<CustomGroupViewModel>(), JsonRequestBehavior.AllowGet);
                }
                var projectCustomGroups = _projectCustomGroupService.Query(g => g.ProjectCode == currentProject.ProjectCode).Select().ToList();
                if (!projectCustomGroups.Any())
                {
                    return Json(new List<CustomGroupViewModel>(), JsonRequestBehavior.AllowGet);
                }

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
                genObj.FormPath = baseData.FormPath;

                var customGroups = new List<CustomGroupViewModel>();
                projectCustomGroups.ForEach(t =>
                {
                    var newGroups = _customGroupService.Query(d => d.CustomGroupId == t.CustomGroupId).Select(g => new CustomGroupViewModel
                    {
                        TableId = g.TableId,
                        CustomGroupId = g.CustomGroupId,
                        GroupName = g.GroupName,
                        TabIndex = g.TabIndex

                    }).ToList();

                    if (newGroups.Any())
                    {
                        customGroups.Add(newGroups[0]);
                    }
                });


                if (!customGroups.Any())
                {
                    return Json(genObj, JsonRequestBehavior.AllowGet);
                }

                customGroups.ForEach(c =>
                {
                    c.CustomFieldViewModels = new List<GenericViewModel>();

                    var customFields = _customFieldService.Query(g => g.CustomGroupId == c.CustomGroupId).Select().ToList();

                    if (customFields.Any())
                    {
                        customFields.ForEach(cFieldView =>
                        {
                            var customFieldTypes = _customFieldTypeService.Query(f => f.FieldTypeId == cFieldView.FieldTypeId).Select().ToList();
                            if (!customFieldTypes.Any())
                            {
                                return;
                            }

                            var customFieldType = customFieldTypes[0];
                            var customFieldViewModel = new GenericViewModel
                            {
                                CustomList = new CustomListViewModel(),
                                CustomFieldType = new CustomFieldType()
                            };

                            if (customFieldType.FieldTypeName == "List")
                            {
                                var customList = _customListService.Query(s => s.CustomListId == cFieldView.CustomListId && string.IsNullOrEmpty(s.ParentListId)).Select().ToList();
                                if (customList.Any())
                                {
                                    var list = customList[0];
                                    var l1 = new CustomListViewModel
                                    {
                                        CustomListName = list.CustomListName,
                                        CustomListId = list.CustomListId,
                                        HasChildren = false,
                                        ParentListId = list.ParentListId,
                                        CustomListDatas = new List<CustomListData>()
                                    };

                                    var l1Data = _customListDataService.Query(s => s.CustomListId == l1.CustomListId).Select().ToList();

                                    if (l1Data.Any())
                                    {
                                        l1.CustomListDatas = l1Data;
                                    }

                                    var childList = _customListService.Query(s => s.ParentListId == list.CustomListId).Select().ToList();
                                    if (childList.Any())
                                    {
                                        l1.HasChildren = true;
                                    }

                                    customFieldViewModel.TableId = cFieldView.TableId;
                                    customFieldViewModel.CustomFieldId = cFieldView.CustomFieldId;
                                    customFieldViewModel.CustomFieldName = cFieldView.CustomFieldName;
                                    customFieldViewModel.CustomFieldSize = cFieldView.CustomFieldSize;
                                    customFieldViewModel.CustomListId = cFieldView.CustomListId;
                                    customFieldViewModel.ParentFieldId = cFieldView.ParentFieldId;
                                    customFieldViewModel.CustomGroupId = cFieldView.CustomGroupId;
                                    customFieldViewModel.FieldTypeId = cFieldView.FieldTypeId;
                                    customFieldViewModel.TabIndex = cFieldView.TabIndex;
                                    customFieldViewModel.Required = cFieldView.Required;
                                    customFieldViewModel.CustomFieldType = customFieldType;
                                    customFieldViewModel.CustomList = l1;
                                }
                            }
                            else
                            {
                                customFieldViewModel = new GenericViewModel
                                {
                                    TableId = cFieldView.TableId,
                                    CustomFieldId = cFieldView.CustomFieldId,
                                    CustomFieldName = cFieldView.CustomFieldName,
                                    CustomFieldSize = cFieldView.CustomFieldSize,
                                    ParentFieldId = cFieldView.ParentFieldId,
                                    CustomListId = cFieldView.CustomListId,
                                    CustomGroupId = cFieldView.CustomGroupId,
                                    FieldTypeId = cFieldView.FieldTypeId,
                                    TabIndex = cFieldView.TabIndex,
                                    Required = cFieldView.Required,
                                    CustomFieldType = customFieldType
                                };
                            }

                            c.CustomFieldViewModels.Add(customFieldViewModel);

                        });
                    }
                    genObj.CustomGroupViewModels.Add(c);
                });

                customGroups.ForEach(g =>
                {
                    g.CustomFieldViewModels = g.CustomFieldViewModels.OrderBy(f => f.TabIndex).ToList();
                });

                customGroups = customGroups.OrderBy(m => m.TabIndex).ToList();
                return Json(genObj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<CustomGroupViewModel>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetCustomListDatasByParentList2(string parentListId, string parentNode, string parentCustomFieldId)
        {
            try
            {
                var customList = _customListService.Query(q => q.ParentListId == parentListId).Select(p => new CustomListViewModel
                {
                    CustomListName = p.CustomListName,
                    CustomListId = p.CustomListId,
                    ParentListId = p.ParentListId
                }).ToList();

                if (!customList.Any())
                {
                    return Json(new CustomListViewModel(), JsonRequestBehavior.AllowGet);
                }
                var l1 = customList[0];
                CustomField customField;

                if (customList.Count > 1)
                {
                    var customFields = _customFieldService.Query(q => q.ParentFieldId == parentCustomFieldId).Select().ToList();
                    if (!customFields.Any())
                    {
                        return Json(new CustomListViewModel(), JsonRequestBehavior.AllowGet);
                    }

                    customField = customFields[0];

                    customList = _customListService.Query(q => q.CustomListId == customField.CustomListId).Select(p => new CustomListViewModel
                    {
                        CustomListName = p.CustomListName,
                        CustomListId = p.CustomListId,
                        ParentListId = p.ParentListId
                    }).ToList();

                    if (!customList.Any())
                    {
                        return Json(new CustomListViewModel(), JsonRequestBehavior.AllowGet);
                    }

                    l1 = customList[0];
                }
                else
                {
                    var customFields = _customFieldService.Query(q => q.CustomListId == l1.CustomListId).Select().ToList();
                    if (!customFields.Any())
                    {
                        return Json(new CustomListViewModel(), JsonRequestBehavior.AllowGet);
                    }
                    customField = customFields[0];
                }

                l1.CustomListDatas = new List<CustomListData>();
                l1.CustomFieldId = customField.CustomFieldId;
                l1.CustomFieldName = customField.CustomFieldName;
                l1.CustomFieldSize = customField.CustomFieldSize;
                l1.CustomGroupId = customField.CustomGroupId;
                l1.Required = customField.Required;

                var l1Data = _customListDataService.Query(s => s.CustomListId == l1.CustomListId && s.ParentNodeId == parentNode).Select().ToList();

                if (l1Data.Any())
                {
                    l1.CustomListDatas = l1Data;
                }

                var childList = _customListService.Query(s => s.ParentListId == l1.CustomListId).Select().ToList();

                if (childList.Any())
                {
                    l1.HasChildren = true;
                }

                return Json(l1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<CustomGroupViewModel>(), JsonRequestBehavior.AllowGet);
            }
        }
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

