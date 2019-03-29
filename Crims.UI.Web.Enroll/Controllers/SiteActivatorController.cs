using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Crims.Data.Contracts;
using Crims.Data.Models;
using Crims.UI.Web.Enroll.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Repository.Pattern.UnitOfWork;
using WebGrease.Css.Extensions;

namespace Crims.UI.Web.Enroll.Controllers
{
    public class SiteActivatorController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IProjectCustomFieldService _projectCustomFieldService;
        private readonly ICustomFieldService _customFieldService;
        private readonly IProjectCustomGroupService _projectCustomGroupService;
        private readonly ICustomGroupService _customGroupService;
        private ICustomListDataService _customListDataService;
        private ICustomListService _customListService;
        private ICustomFieldTypeService _customFieldTypeService;

        private readonly IUnitOfWorkAsync _unitOfWork;
        public SiteActivatorController()
        {
            //
        }

        public SiteActivatorController(IProjectService projectService, ICustomFieldTypeService customFieldTypeService, IProjectCustomFieldService projectCustomFieldService, ICustomFieldService customFieldService, ICustomGroupService customGroupService, IProjectCustomGroupService projectCustomGroupService, ICustomListService customListService, ICustomListDataService customListDataService, IUnitOfWorkAsync unitOfWork)
        {
            _projectService = projectService;
            _customListDataService = customListDataService;
            _projectCustomFieldService = projectCustomFieldService;
            _customFieldService = customFieldService;
            _projectCustomGroupService = projectCustomGroupService;
            _customGroupService = customGroupService;
            _customFieldTypeService = customFieldTypeService;

            _customListService = customListService;

            _unitOfWork = unitOfWork;
        }
       
        [HttpPost]
        public ActionResult InstallLicense()
        {
            var acResponse = new ActivityResponse();

            try
            {
                if (Request.Files != null)
                {
                    var file = Request.Files[0];

                    if (file == null || file.ContentLength < 1)
                    {
                        acResponse.Code = -1;
                        acResponse.Message = "The provided Site License File could not be correctly accessed. Please provide all required fields and try again later";
                        return Json(acResponse, JsonRequestBehavior.AllowGet);
                    }

                    const string folderPath = "~/TempProject";

                    var mainPath = Server.MapPath(folderPath);

                    if (!Directory.Exists(mainPath))
                    {
                        Directory.CreateDirectory(mainPath);
                        var dInfo = new DirectoryInfo(mainPath);
                        var dSecurity = dInfo.GetAccessControl();
                        dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                        dInfo.SetAccessControl(dSecurity);
                    }

                    var fileName = file.FileName;

                    var existingFiles = Directory.GetFiles(mainPath);
                    if (existingFiles.Any())
                    {
                        existingFiles.ForEach(System.IO.File.Delete);
                    }

                    var path = Path.Combine(mainPath, fileName);
                    
                    file.SaveAs(path);

                    var projectLicense = JsonConvert.DeserializeObject<ProjectLicense>(System.IO.File.ReadAllText(path));
                    
                    var customGroups = new List<CustomGroup>(projectLicense.CustomGroups);
                    var customFieldTypes = new List<CustomFieldType>(projectLicense.CustomFieldTypes);
                    var customFields = new List<CustomField>(projectLicense.CustomFields);
                    var customLists = new List<CustomList>(projectLicense.CustomLists);
                    var customListData = new List<CustomListData>(projectLicense.CustomListData);
                    var projectCustomGroups = new List<ProjectCustomGroup>(projectLicense.ProjectCustomGroups);
                    var projectCustomFields = new List<ProjectCustomField>(projectLicense.ProjectCustomFields);

                    if (string.IsNullOrEmpty(projectLicense?.ProjectCode) || !customFieldTypes.Any() || !customGroups.Any() || !customFields.Any() || !customLists.Any() || !customListData.Any() || !projectCustomGroups.Any() || !projectCustomFields.Any())
                    {
                        acResponse.Code = -1;
                        acResponse.Message = "The provided Project setup File could not be correctly accessed. Please provide all required fields and try again later";
                        return Json(acResponse, JsonRequestBehavior.AllowGet);
                    }

                    var processedProjectTableId = 0;
                   
                    var projects = _projectService.Query(p => p.ProjectCode == projectLicense.ProjectCode).Select().ToList();
                    if (projects.Any())
                    {
                        var project = projects[0];
                        project.ProjectName = projectLicense.ProjectName;
                        project.ProjectDescription = projectLicense.ProjectDescription;
                        project.ActivationCode = projectLicense.ActivationCode;
                        project.OnlineMode = projectLicense.OnlineMode;
                        project.LicenseExpiryDate = projectLicense.LicenseExpiryDate;
                        _projectService.Update(project);
                        _unitOfWork.SaveChanges();
                        processedProjectTableId = project.TableId;
                    }
                    else
                    {
                        var project = new Project
                        {
                            ProjectName = projectLicense.ProjectName,
                            ProjectDescription = projectLicense.ProjectDescription,
                            ProjectCode = projectLicense.ProjectCode,
                            DateCreated = DateTime.Now,
                            LicenceCode = projectLicense.LicenceCode,
                            ActivationCode = projectLicense.ActivationCode,
                            OnlineMode = projectLicense.OnlineMode,
                            LicenseExpiryDate = projectLicense.LicenseExpiryDate
                        };
                        _projectService.Insert(project);
                        processedProjectTableId = _unitOfWork.SaveChanges();
                    }
                   
                    if (processedProjectTableId < 1)
                    {
                        acResponse.Code = -1;
                        acResponse.Message = "Project Licensing failed. Please try again later of contact support.";
                        return Json(acResponse, JsonRequestBehavior.AllowGet);
                    }
                    var oldFieldTypes = _customFieldTypeService.Queryable().ToList();
                    if (oldFieldTypes.Any())
                    {
                        customFieldTypes.ForEach(g =>
                        {
                            var fieldTypes = oldFieldTypes.Where(x => x.FieldTypeId == g.FieldTypeId).ToList();
                            if (fieldTypes.Any())
                            {
                                var f = fieldTypes[0];
                                f.FieldTypeName = g.FieldTypeName;
                                _customFieldTypeService.Update(f);
                                _unitOfWork.SaveChanges();
                            }
                            else
                            {
                                _customFieldTypeService.Insert(g);
                                _unitOfWork.SaveChanges();
                            }
                        });
                    }
                    else
                    {
                        _customFieldTypeService.InsertRange(customFieldTypes);
                    }
                    
                    var projectCustomGroupEntities = _projectCustomGroupService.Query(g => g.ProjectCode == projectLicense.ProjectCode).Select().ToList();
                    if (projectCustomGroupEntities.Any())
                    {
                        projectCustomGroups.ForEach(g =>
                        {
                            var group = projectCustomGroupEntities.Find(d => d.CustomGroupId == g.CustomGroupId);
                            if (!string.IsNullOrEmpty(@group?.CustomGroupId))
                            {
                                group.TabIndex = g.TabIndex;
                                _projectCustomGroupService.Update(group);
                                _unitOfWork.SaveChanges();

                            }
                            else
                            {
                                _projectCustomGroupService.Insert(g);
                                _unitOfWork.SaveChanges();
                            }
                           
                        });
                    }
                    else
                    {
                        _projectCustomGroupService.InsertRange(projectCustomGroups);
                    }

                    
                    var projectCustomFieldEntities = _projectCustomFieldService.Query(f =>  f.ProjectCode == projectLicense.ProjectCode).Select().ToList();
                    if (projectCustomFieldEntities.Any())
                    {
                        projectCustomFields.ForEach(g =>
                        {
                            var field = projectCustomFieldEntities.Find(d => d.CustomFieldId == g.CustomFieldId);
                            if (string.IsNullOrEmpty(field?.CustomFieldId))
                            {
                                _projectCustomFieldService.Insert(g);
                                _unitOfWork.SaveChanges();
                            }

                        });
                    }
                    else
                    {
                        _projectCustomFieldService.InsertRange(projectCustomFields);
                    }
                    
                    var customGroupEntities = _customGroupService.Queryable().ToList();
                    if (customGroupEntities.Any())
                    {
                        customGroups.ForEach(g =>
                        {
                            var cGroup = customGroupEntities.Find(d => d.CustomGroupId == g.CustomGroupId);
                            if (!string.IsNullOrEmpty(cGroup?.CustomGroupId))
                            {
                                cGroup.GroupName = g.GroupName;
                                cGroup.TabIndex = g.TabIndex;
                                _customGroupService.Update(cGroup);
                                _unitOfWork.SaveChanges();
                            }
                            else
                            {
                                g.TableId = 0;
                                _customGroupService.Insert(g);
                                _unitOfWork.SaveChanges();
                            }

                        });
                    }
                    else
                    {
                        _customGroupService.InsertRange(customGroups);
                    }
                    
                    var customFieldEntities = _customFieldService.Queryable().ToList();
                    if (customFieldEntities.Any())
                    {
                        customFields.ForEach(g =>
                        {
                            var cField = customFieldEntities.Find(d => d.CustomFieldId == g.CustomFieldId);
                            if (!string.IsNullOrEmpty(cField?.CustomFieldId))
                            {
                                cField.CustomFieldName = g.CustomFieldName;
                                cField.TabIndex = g.TabIndex;
                                cField.CustomFieldSize = g.CustomFieldSize;
                                cField.CustomListId = g.CustomListId;
                                cField.ParentFieldId = g.ParentFieldId;
                                cField.Required = g.Required;
                                cField.FieldTypeId = g.FieldTypeId;
                                cField.CustomGroupId = g.CustomGroupId;
                                _customFieldService.Update(cField);
                                _unitOfWork.SaveChanges();
                            }
                            else
                            {
                                g.TableId = 0;
                                _customFieldService.Insert(g);
                                _unitOfWork.SaveChanges();
                            }

                        });
                    }
                    else
                    {
                        _customFieldService.InsertRange(customFields);
                    }
                    
                    var customListEntities = _customListService.Queryable().ToList();
                    if (customListEntities.Any())
                    {
                        customLists.ForEach(g =>
                        {
                            var cList = customListEntities.Find(d => d.CustomListId == g.CustomListId);
                            if (!string.IsNullOrEmpty(cList?.CustomListId))
                            {
                                cList.CustomListName = g.CustomListName;
                                cList.CustomListName = g.CustomListName;
                                cList.ParentListId = g.ParentListId;
                                _customListService.Update(cList);
                                _unitOfWork.SaveChanges();
                            }
                            else
                            {
                                g.TableId = 0;
                                _customListService.Insert(g);
                                _unitOfWork.SaveChanges();
                            }

                        });
                    }
                    else
                    {
                        _customListService.InsertRange(customLists);
                    }

                    var customListDataEntities = _customListDataService.Queryable().ToList();
                    if (customListDataEntities.Any())
                    {
                        customListData.ForEach(g =>
                        {
                            var cListData = customListDataEntities.Find(d => d.CustomListId == g.CustomListId);
                            if (!string.IsNullOrEmpty(cListData?.CustomListId))
                            {
                                cListData.CustomListId = g.CustomListId;
                                cListData.ListDataName = g.ListDataName;
                                cListData.ParentNodeId = g.ParentNodeId;
                                _customListDataService.Update(cListData);
                                _unitOfWork.SaveChanges();
                            }
                            else
                            {
                                g.TableId = 0;
                                _customListDataService.Insert(g);
                                _unitOfWork.SaveChanges();
                            }

                        });
                    }
                    else
                    {
                        _customListDataService.InsertRange(customListData);
                    }

                    _unitOfWork.SaveChanges();
                    
                    //Necesary to initialise and construct the Identity Tables in the created database
                    var usermanager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    usermanager?.FindByEmail("siteadmin@chams.com");

                    acResponse.Code = 5;
                    acResponse.Message = "The Project License was successfully installed.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                    
                }
                acResponse.Code = -1;
                acResponse.Message = "The provided Site License File could not be correctly accessed. Please provide all required fields and try again later";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.StackTrace, e.Source, e.Message);
                acResponse.Code = -1;
                acResponse.Message = e.Message;
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SiteActivation()
        {
            return View();
        }

        #region Helpers


        #endregion
    }
}
