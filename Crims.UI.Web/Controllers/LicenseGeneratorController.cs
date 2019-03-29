using Crims.Data.Contracts;
using Crims.Data.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crims.UI.Web.Helpers;
using Crims.UI.Web.Models;
using Newtonsoft.Json;

namespace Crims.UI.Web.Controllers
{
    [Authorize]
    public class LicenseGeneratorController : Controller
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
        public LicenseGeneratorController()
        {

        }
         
        public LicenseGeneratorController(IProjectService projectService, IProjectCustomFieldService projectCustomFieldService, ICustomFieldService customFieldService, ICustomGroupService customGroupService, IProjectCustomGroupService projectCustomGroupService, ICustomFieldTypeService customFieldTypeService, ICustomListService customListService, ICustomListDataService customListDataService, IUnitOfWorkAsync unitOfWork)
        {
            _projectService = projectService;
            _customListDataService = customListDataService;
            _projectCustomFieldService = projectCustomFieldService;
            _customFieldService = customFieldService;
            _projectCustomGroupService = projectCustomGroupService;
            _customGroupService = customGroupService;

            _customListService = customListService;
            _customFieldTypeService = customFieldTypeService;

            _unitOfWork = unitOfWork;
        }
       
        public ActionResult GenerateLicense(string projectCode)
        {
            var acResponse = new ActivityResponse();
           
            try
            {
                if (string.IsNullOrEmpty(projectCode))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "The selected Project information could not be accessed. Please try again later";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                var projects = _projectService.Query(p => p.ProjectCode == projectCode).Select().ToList();

                if (!projects.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "The selected Project information could not be accessed. Please try again later";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                var fieldTypes = _customFieldTypeService.Queryable().ToList();
                if (!fieldTypes.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Project Custom Fields could not be retrieved. Please ensure all required setups are handled appropriately before proceeding.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                var projectCustomGroups = _projectCustomGroupService.Query(g => g.ProjectCode == projectCode).Select().ToList();
                if (!projectCustomGroups.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Project Custom Groups could not be retrieved. Please ensure all required setups are handled appropriately before proceeding.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                var projectCustomFields = _projectCustomFieldService.Query(g => g.ProjectCode == projectCode).Select().ToList();
                if (!projectCustomFields.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Project Custom Fields could not be retrieved. Please ensure all required setups are handled appropriately before proceeding.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                var project = projects[0];

                var projectLicense = new ProjectLicense
                {
                    ProjectName = project.ProjectName,
                    ProjectDescription = project.ProjectDescription,
                    ProjectCode = project.ProjectCode,
                    LicenseExpiryDate = project.LicenseExpiryDate,
                    DateCreated = project.DateCreated,
                    ProjectCustomGroups = projectCustomGroups.ToArray(),
                    ProjectCustomFields = projectCustomFields.ToArray()
                };

                var customGroups = new List<CustomGroup>();
                var customFields = new List<CustomField>();
                var customList = new List<CustomList>();
                var customListData = new List<CustomListData>();
                    
                projectCustomGroups.ForEach(c =>
                {
                    var groups = _customGroupService.Query(l => l.CustomGroupId == c.CustomGroupId).Select().ToList();

                    if (groups.Any())
                    {
                        if (!customGroups.Exists(g => g.CustomGroupId == groups[0].CustomGroupId))
                        {
                            customGroups.Add(groups[0]);
                        }
                    }

                });

                projectCustomFields.ForEach(c =>
                {
                    var fields = _customFieldService.Query(l => l.CustomFieldId == c.CustomFieldId).Select().ToList();

                    if (!fields.Any())
                    {
                        return;
                    }

                    var customField = fields[0];
                    var customLists = _customListService.Query(l => l.CustomListId == customField.CustomListId).Select().ToList();
                    var cGroups = _customGroupService.Query(l => l.CustomGroupId == customField.CustomGroupId).Select().ToList();
                        
                    if (cGroups.Any())
                    {
                        if (!customGroups.Exists(a => a.CustomGroupId == cGroups[0].CustomGroupId))
                        {
                            customGroups.Add(cGroups[0]);
                        }

                        if (customLists.Any())
                        {
                            var cListItem = customLists[0];

                            if (!customList.Exists(a => a.CustomListId == cListItem.CustomListId))
                            {
                                customList.Add(cListItem);
                                var list = _customListDataService.Query(d => d.CustomListId == cListItem.CustomListId).Select().ToList();
                                if (list.Any())
                                {
                                    list.ForEach(l =>
                                    {
                                        if (!customListData.Exists(a => a.CustomListDataId == l.CustomListDataId))
                                        {
                                            customListData.Add(l);
                                        }
                                    });
                                   
                                }
                            }
                        }
                            
                        customFields.Add(customField);
                    }

                });
                   
                projectLicense.CustomFields = customFields.ToArray();
                projectLicense.CustomGroups = customGroups.ToArray();
                projectLicense.CustomLists = customList.ToArray();
                projectLicense.CustomListData = customListData.ToArray();
                projectLicense.CustomFieldTypes = fieldTypes.ToArray();

                const string folderPath = "~/TempProjectSetUp";
                
                var filePath = Server.MapPath(folderPath + "/" + projectLicense.ProjectName.Replace(" ", "-") + ".json");

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(projectLicense));
                
                acResponse.Code = 5;
                acResponse.DownloadLink = GenericHelpers.MapPath(filePath);
                acResponse.Message = "Project License was successfully generated.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.StackTrace, e.Source, e.Message);
                acResponse.Code = -1;
                acResponse.Message = "The selected Project information could not be accessed. Please try again later";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }
        
        #region Helpers
      
        public bool DownloadContentFromFolder(string path)
        {
            try
            {
                Response.Clear();
                var filename = Path.GetFileName(path);
                HttpContext.Response.Buffer = true;
                HttpContext.Response.Charset = "";
                HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = GetMimeType(filename);
                HttpContext.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
                Response.WriteFile(path);
                Response.End();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            var extension = Path.GetExtension(fileName);
            if (extension != null)
            {
                var ext = extension.ToLower();
                var regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
                if (regKey != null && regKey.GetValue("Content Type") != null)
                    mimeType = regKey.GetValue("Content Type").ToString();
            }
            return mimeType;
        }

        #endregion
    }
}
