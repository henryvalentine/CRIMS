using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Crims.Core.Utils;
using Crims.Data;
using Crims.Data.Contracts;
using Crims.Data.Migrations;
using Crims.Data.Models;
using Crims.UI.Web.Enroll.Helpers;
using Crims.UI.Web.Enroll.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Repository.Pattern.UnitOfWork;

namespace Crims.UI.Web.Enroll.Controllers
{
    public class HomeController : Controller
    {
        private IProjectService _projectService;
        private IAppSettingService _appSettingService;
        private IApprovalService _approvalService;
        private IBaseDataService _baseDataService;
        private IFingerprintImageService _fingerprintImageService;
        private IPhotographService _photographService;
        private ISignatureService _signatureService;
        private ISyncJobHistoryService _syncJobHistory;
        private IUnitOfWorkAsync _unitOfWork;

        public HomeController(ISignatureService signatureService, IPhotographService photographService, IFingerprintImageService fingerprintImageService, ISyncJobHistoryService syncJobHistory, IApprovalService approvalService, IProjectService projectService, IBaseDataService baseDataService, IUnitOfWorkAsync unitOfWork, IAppSettingService appSettingService)
        {
            _projectService = projectService;
            _appSettingService = appSettingService;
            _syncJobHistory = syncJobHistory;
            _approvalService = approvalService;
            _baseDataService = baseDataService;
            _signatureService = signatureService;
            _photographService = photographService;
            _fingerprintImageService = fingerprintImageService;
            _unitOfWork = unitOfWork;
        }
        
        public HomeController()
        {

        }
        public ActionResult NewLicense()
        {
            return View();
        }
      
        public ActionResult Index()
        {
            var folderPath = "~/UserRecords";
            var appSettings = _appSettingService.Queryable().ToList();
            if (!appSettings.Any())
            {
                var settingsPath = Server.MapPath(folderPath);
                var newAppSeting = new AppSetting { Id = EntityIdGenerator.GenerateEntityId(), BiometricTemplatePath = folderPath };
                if (!Directory.Exists(settingsPath))
                {
                    Directory.CreateDirectory(settingsPath);
                    var dInfo = new DirectoryInfo(settingsPath);
                    var dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    dInfo.SetAccessControl(dSecurity);
                }

                _appSettingService.Insert(newAppSeting);
                _unitOfWork.SaveChanges();
            }

            var projects = _projectService.Queryable().ToList();

            if (!projects.Any())
            {
                return RedirectToAction("SiteActivation", "SiteActivator");
            }

            var sessionProject = GetProjectInSession();
            if (sessionProject == null)
            {
                return RedirectToAction("ProjectOptions", "Home");
            }

            if (string.IsNullOrEmpty(sessionProject.ProjectCode))
            {
                return RedirectToAction("ProjectOptions", "Home");
            }
            var project = projects.Where(p => p.ProjectCode == sessionProject.ProjectCode).ToList()[0];
            if (string.IsNullOrEmpty(project?.ProjectCode))
            {
                return RedirectToAction("ProjectOptions", "Home");
            }

            if (project.LicenseExpiryDate < DateTime.Today)
            {
                Session["siteStatus"] = "expired";
                return RedirectToAction("SiteActivation", "SiteActivator", new RouteValueDictionary(new KeyValuePair<string, string>("ProjectExpired", "true")));
            }
            
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            
            return View();
        }

        public ActionResult GetDashboardStats()
        {
            try
            {
                var sessionProject = GetProjectInSession();
                if (sessionProject == null)
                {
                    return Json(new DashboardModel(), JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(sessionProject.ProjectCode))
                {
                    return Json(new DashboardModel(), JsonRequestBehavior.AllowGet);
                }

                var userId = User.Identity.GetUserId();
                var registeredUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Where(e => e.Id == userId).Include("UserInfo").ToList();
                if (!registeredUsers.Any())
                {
                    return Json(new DashboardModel(), JsonRequestBehavior.AllowGet);
                }

                var enrolledBy = registeredUsers[0].UserInfo.Id;
                var dashboardModel = new DashboardModel();
                var approved = (int)EnumManager.ApprovalStatus.Approved;
                var list = new List<BaseData>();

                var photosEnrolled = 0;
                var fingerprintsEnrolled = 0;
                var signaturesEnrolled = 0;
                var noPhotos = 0;
                var noFingerprints = 0;
                using (var db = new CrimsDbContext())
                {
                    if (User.IsInRole("Admin") || User.IsInRole("Site_Administrator") || User.IsInRole("Super_Admin"))
                    {
                       
                        list = db.BaseDatas.Where(t => t.ProjectCode == sessionProject.ProjectCode).Include("Approvals").Include("FingerprintImages")
                            .Include("Photographs").Include("Signatures").Include("SyncJobHistories")
                            .OrderByDescending(d => d.Firstname).Skip(0).Take(50).ToList();
                    }
                    else
                    {
                        if (User.IsInRole("Enrollment_Officer"))
                        {
                            dashboardModel.EnrollmentOfficer = enrolledBy;
                            list = db.BaseDatas.Where(t => t.ProjectCode == sessionProject.ProjectCode && (t.CreatedBy == enrolledBy || t.LastUpdatedby == enrolledBy))
                                .Include("Approvals").Include("FingerprintImages")
                                .Include("Photographs").Include("Signatures").Include("SyncJobHistories")
                                .OrderByDescending(d => d.EnrollmentDate).Skip(0).Take(50).ToList();
                        }
                        else
                        {
                            return Json(new DashboardModel(), JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (list.Any())
                    {
                        dashboardModel.DataRetrieved = true;
                        list.ForEach(d =>
                        {
                            if (d.Photographs.Any())
                            {
                                photosEnrolled += 1;
                            }
                            else
                            {
                                noPhotos += 1;
                            }
                            if (d.FingerprintImages.Any())
                            {
                                fingerprintsEnrolled += 1;
                            }
                            else
                            {
                                noFingerprints += 1;
                            }
                            if (d.Signatures.Any())
                            {
                                signaturesEnrolled += 1;
                            }
                            if (d.SyncJobHistories.Any())
                            {
                                dashboardModel.Synchronized += 1;
                            }
                        });

                        dashboardModel.TotalApprovals = list.Count(t => t.ApprovalStatus == approved);
                        dashboardModel.TotalCapture = list.Count;
                        dashboardModel.Synchronized = db.SyncJobHistory.Count();
                        dashboardModel.PhotoEnrolled = photosEnrolled;
                        dashboardModel.ProjectCode = sessionProject.ProjectCode;
                        dashboardModel.Signature = signaturesEnrolled;
                        dashboardModel.FingerprintsEnrolled = fingerprintsEnrolled;
                        dashboardModel.NoPhotos = noPhotos;
                        dashboardModel.NoFingerprint = noFingerprints;
                    }
                   
                    return Json(dashboardModel, JsonRequestBehavior.AllowGet);
                }
                   
            }
            catch (Exception e)
            {
                return Json(new DashboardModel(), JsonRequestBehavior.AllowGet);
            }
        }
       
        public ActionResult GetMoreDashboardStats(int page, int numOfItemsToFetch, string enrollmentOfficer, string projectCode)
        {
            try
            {
                var dashboardModel = new DashboardModel();
                var list = new List<BaseData>();

                var photosEnrolled = 0;
                var fingerprintsEnrolled = 0;
                var approved = (int)EnumManager.ApprovalStatus.Approved;
                var signaturesEnrolled = 0;
                var noPhotos = 0;
                var noFingerprints = 0;
                using (var db = new CrimsDbContext())
                {
                    if (string.IsNullOrEmpty(enrollmentOfficer) || enrollmentOfficer == "null")
                    {
                        list = db.BaseDatas.Where(t => t.ProjectCode == projectCode)
                            .Include("Approvals").Include("FingerprintImages")
                            .Include("Photographs").Include("Signatures").Include("SyncJobHistories")
                            .OrderByDescending(d => d.EnrollmentDate).Skip(page * numOfItemsToFetch).Take(numOfItemsToFetch).ToList();
                    }
                    else if (!string.IsNullOrEmpty(enrollmentOfficer))
                    {
                        list = db.BaseDatas.Where(t => t.ProjectCode == projectCode && (t.CreatedBy == enrollmentOfficer || t.LastUpdatedby == enrollmentOfficer))
                            .Include("Approvals").Include("FingerprintImages")
                            .Include("Photographs").Include("Signatures").Include("SyncJobHistories")
                            .OrderByDescending(d => d.EnrollmentDate).Skip(page * numOfItemsToFetch).Take(numOfItemsToFetch).ToList();
                    }

                    else { return Json(new DashboardModel(), JsonRequestBehavior.AllowGet); }

                    if (list.Any())
                    {
                        dashboardModel.DataRetrieved = true;
                        list.ForEach(d =>
                        {
                            if (d.Photographs.Any())
                            {
                                photosEnrolled += 1;
                            }
                            else
                            {
                                noPhotos += 1;
                            }
                            if (d.FingerprintImages.Any())
                            {
                                fingerprintsEnrolled += 1;
                            }
                            else
                            {
                                noFingerprints += 1;
                            }
                            if (d.Signatures.Any())
                            {
                                signaturesEnrolled += 1;
                            }
                            if (d.SyncJobHistories.Any())
                            {
                                dashboardModel.Synchronized += 1;
                            }
                        });
                        dashboardModel.TotalApprovals = list.Count(t => t.ApprovalStatus == approved);
                        dashboardModel.TotalCapture = list.Count;
                        dashboardModel.PhotoEnrolled = photosEnrolled;
                        dashboardModel.Signature = signaturesEnrolled;
                        dashboardModel.FingerprintsEnrolled = fingerprintsEnrolled;
                        dashboardModel.NoPhotos = noPhotos;
                        dashboardModel.NoFingerprint = noFingerprints;
                    }
                    return Json(dashboardModel, JsonRequestBehavior.AllowGet);
                }
               
            }
            catch (Exception e)
            {
                return Json(new DashboardModel(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AppSetting()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
       
        public Project GetProjectInSession()
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Login", "Account");
            }
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
        public ActionResult ProjectOptions()
        {
            return View();
        }
        public ActionResult GetProjects()
        {
            var projects = _projectService.Queryable().ToList();
            return Json(projects, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public ActionResult SetCurrentSessionGetProject(Project project)
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Login", "Account");
            }
            var acr = new ActivityResponse
            {
                Code = -1,
                Message = "Current Project could not be Set. Please try again later."
            };
            if (string.IsNullOrEmpty(project.ProjectCode) || string.IsNullOrEmpty(project.ProjectName))
            {
                return Json(acr, JsonRequestBehavior.AllowGet);
            }
            Session["_currentProject"] = project;
            acr.Code = 5;
            acr.Message = "Current session project was successfully set";
            return Json(acr, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult GetCurrentSessionProject()
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Login", "Account");
            }
            var currentSessionModel = new CurrentSessionModel {Project = new Project(), LoginName = string.Empty};
            if (Session["_loginName"] == null)
            {
                return Json(currentSessionModel, JsonRequestBehavior.AllowGet);
            }
            var loginName = Session["_loginName"] as string;
            if (string.IsNullOrEmpty(loginName))
            {
                return Json(currentSessionModel, JsonRequestBehavior.AllowGet);
            }
            if (Session["_currentProject"] == null)
            {
                return Json(currentSessionModel, JsonRequestBehavior.AllowGet);
            }
            var currentProject = Session["_currentProject"] as Project;
            if (string.IsNullOrEmpty(currentProject?.ProjectCode))
            {
                return Json(currentSessionModel, JsonRequestBehavior.AllowGet);
            }
            currentSessionModel.Project = currentProject;
            currentSessionModel.LoginName = loginName;
            return Json(currentSessionModel, JsonRequestBehavior.AllowGet);
        }
    
        public ActionResult GetLoginName()
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Login", "Account");
            }
            try
            {
                if (Session["_loginName"] == null)
                {
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
                var loginName = Session["_loginName"] as string;
                if (string.IsNullOrEmpty(loginName))
                {
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
                return Json(loginName, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetAppSetting()
        {
            var appSettings = _appSettingService.Queryable().ToList();
            if (!appSettings.Any())
            {
                //var mainPath = @"C:\Crims\UserRecords";
                var folderPath = "~/UserRecords";

                var mainPath = Server.MapPath(folderPath);

                var newAppSeting = new AppSetting { Id = EntityIdGenerator.GenerateEntityId(), BiometricTemplatePath = folderPath };
                if (!Directory.Exists(mainPath))
                {
                    Directory.CreateDirectory(mainPath);
                    var dInfo = new DirectoryInfo(mainPath);
                    var dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    dInfo.SetAccessControl(dSecurity);
                }

                _appSettingService.Insert(newAppSeting);
                _unitOfWork.SaveChanges();
                
                return Json(new AppSettingModel {Id = newAppSeting .Id, BiometricTemplatePath = newAppSeting.BiometricTemplatePath, SynchronisationTimeStr = ""}, JsonRequestBehavior.AllowGet);
            }

            var appSetting = new AppSettingModel
            {
                Id = appSettings[0].Id,
                BiometricTemplatePath = appSettings[0].BiometricTemplatePath,
                SynchronisationTime = appSettings[0].SynchronisationTime,
                SynchronisationTimeStr = appSettings[0].SynchronisationTime.ToString("hh:mm"),
                SynchFrequency = appSettings[0].SynchFrequency,
            };
            return Json(appSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddAppSetting(AppSetting appSetting)
        {
            var acResponse = new ActivityResponse();
            
            if (appSetting.SynchronisationTime.TimeOfDay.Hours < 1)
            {
                acResponse.Code = -1;
                acResponse.Message = "Please provide Synchronization time.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            if (appSetting.SynchFrequency < 1)
            {
                acResponse.Code = -1;
                acResponse.Message = "Please provide Synchronization Frequency.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(appSetting.Id))
            {
                appSetting.Id = EntityIdGenerator.GenerateEntityId();
                _appSettingService.Insert(appSetting);
                _unitOfWork.SaveChanges();
            }
            else
            {
                _appSettingService.Update(appSetting);
                _unitOfWork.SaveChanges();
            }
            
            acResponse.Code = 5;
            acResponse.EnrollmentId = appSetting.Id;
            acResponse.Message = "Process completed successfully";
            return Json(acResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NoPhoto()
        {
            var currentProject = GetProjectInSession();
            if (currentProject == null)
            {
                return RedirectToAction("ProjectOptions", "Home");
            }
            var projects = _projectService.Query(p =>p.ProjectCode == currentProject.ProjectCode).Select().ToList();

            if (!projects.Any())
            {
                return RedirectToAction("SiteActivation", "SiteActivator");
            }

            var project = projects[0];

            if (project.LicenseExpiryDate < DateTime.Today)
            {
                Session["siteStatus"] = "expired";
                return RedirectToAction("SiteActivation", "SiteActivator", new RouteValueDictionary(new KeyValuePair<string, string>("ProjectExpired", "true")));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.Identity.GetUserId();
            var registeredUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Where(e => e.Id == userId).Include("UserInfo").ToList();
            if (!registeredUsers.Any())
            {
                return View(new List<BaseData>());
            }

            var enrolledBy = registeredUsers[0].UserInfo.Id;
            var list = new List<BaseData>();

            var appSettings = _appSettingService.Queryable().ToList();
            if (!appSettings.Any())
            {
                return View(new List<BaseData>());
            }
            var mainPath = appSettings[0].BiometricTemplatePath;

            var ext = new List<string> { ".jpg" };

            var capturedDirs = Directory.GetDirectories(Server.MapPath(mainPath)).ToList();

            if (!capturedDirs.Any())
            {return View(new List<BaseData>());
            }
            
            var viewList = new List<BaseData>();

            if (User.IsInRole("Admin") || User.IsInRole("Site_Admin"))
            {
                list = _baseDataService.Query().Select().ToList();
            }
            else
            {
                if (!User.IsInRole("Enrollee"))
                {
                    list = _baseDataService.Query(t => t.CreatedBy == enrolledBy || t.LastUpdatedby == enrolledBy).Select().ToList();
                }
                else
                {
                    return Redirect("Enrollment");
                }
            }

            list.ForEach(d =>
            {
                var enrolleeFolder = capturedDirs.Find(e => e.Contains(d.EnrollmentId));
                if (enrolleeFolder != null)
                {
                    var enrollePath = Server.MapPath(mainPath + "/" + d.EnrollmentId);

                    var myFiles = Directory.GetFiles(enrollePath, "*.*", SearchOption.AllDirectories).Where(s => ext.Contains(Path.GetExtension(s))).ToList();
                    if (myFiles.Any())
                    {
                        var photos = myFiles.Where(f => f.Contains("photo_image")).ElementAt(0).ToList();
                        if (!photos.Any())
                        {
                            viewList.Add(d);
                        }
                    }
                    else
                    {
                        viewList.Add(d);
                    }
                }
                else
                {

                    viewList.Add(d);
                }

            });
            
            return View(viewList);

        }

        public ActionResult NoFingerPrint()
        {
            var currentProject = GetProjectInSession();
            if (currentProject == null)
            {
                return RedirectToAction("ProjectOptions", "Home");
            }
            var projects = _projectService.Query(p => p.ProjectCode == currentProject.ProjectCode).Select().ToList();

            if (!projects.Any())
            {
                return RedirectToAction("SiteActivation", "SiteActivator");
            }

            var project = projects[0];

            if (project.LicenseExpiryDate < DateTime.Today)
            {
                Session["siteStatus"] = "expired";
                return RedirectToAction("SiteActivation", "SiteActivator", new RouteValueDictionary(new KeyValuePair<string, string>("ProjectExpired", "true")));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.Identity.GetUserId();
            var registeredUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Where(e => e.Id == userId).Include("UserInfo").ToList();
            if (!registeredUsers.Any())
            {
                return View(new List<BaseData>());
            }

            var enrolledBy = registeredUsers[0].UserInfo.Id;
            var list = new List<BaseData>();

            var appSettings = _appSettingService.Queryable().ToList();
            if (!appSettings.Any())
            {
                return View(new List<BaseData>());
            }
            var mainPath = appSettings[0].BiometricTemplatePath;

            var ext = new List<string> { ".jpg" };

            var capturedDirs = Directory.GetDirectories(Server.MapPath(mainPath)).ToList();

            if (!capturedDirs.Any())
            {
                return View(new List<BaseData>());
            }
            
            var viewList = new List<BaseData>();

            if (User.IsInRole("Admin") || User.IsInRole("Site_Admin"))
            {
                list = _baseDataService.Query().Select().ToList();
            }
            else
            {
                if (!User.IsInRole("Enrollee"))
                {
                    list = _baseDataService.Query(t => t.CreatedBy == enrolledBy || t.LastUpdatedby == enrolledBy).Select().ToList();
                }
                else
                {
                    return Redirect("Enrollment");
                }
            }

            list.ForEach(d =>
            {
                var enrolleeFolder = capturedDirs.Find(e => e.Contains(d.EnrollmentId));
                if (enrolleeFolder != null)
                {
                    var enrollePath = Server.MapPath(mainPath + "/" + d.EnrollmentId);

                    var myFiles = Directory.GetFiles(enrollePath, "*.*", SearchOption.AllDirectories).Where(s => ext.Contains(Path.GetExtension(s))).ToList();
                    if (myFiles.Any())
                    {
                        var fingerPrints = myFiles.Where(f => Path.GetFileName(f).StartsWith("LF") || Path.GetFileName(f).StartsWith("RF")).ToList();

                        if (!fingerPrints.Any())
                        {
                            viewList.Add(d);
                        }
                    }
                    else
                    {
                        viewList.Add(d);
                    }
                }
                else
                {

                    viewList.Add(d);
                }

            });

            return View(viewList);

        }
    }
}
