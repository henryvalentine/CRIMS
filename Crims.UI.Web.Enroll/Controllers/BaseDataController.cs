using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Crims.Data;
using Crims.Data.Contracts;
using Crims.Data.Models;
using Crims.UI.Web.Enroll.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using QRCoder;
using Repository.Pattern.UnitOfWork;
using Crims.UI.Web.Enroll.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using WebGrease.Css.Extensions;

namespace Crims.UI.Web.Enroll.Controllers
{
    public class BaseDataController : Controller
    {
        private ICustomDataService _customDataService;
        private ICustomListService _customListService;
        private ICustomFieldService _customFieldService;
        private ICustomListDataService _customListDataService;
        private ICustomGroupService _customGroupService;
        private ICustomFieldTypeService _customFieldTypeService;
        private IApprovalService _approvalService;
        private IBaseDataService _baseDataService;
        private IUnitOfWorkAsync _unitOfWork;
        private IProjectService _projectService;
        private IPhotographService _photographService;
        private ISignatureService _signatureService;
        private IFingerprintTemplateService _fingerprintTemplateService;
        private IFingerprintImageService _fingerprintImageService;
        private IFingerprintReasonService _fingerprintReasonService;
        public BaseDataController()
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Login", "Account");
            }
        }
       
        public BaseDataController(IProjectService projectService, IFingerprintReasonService fingerprintReasonService, IBaseDataService baseDataService, IPhotographService photographService, ICustomDataService customDataService, IFingerprintImageService fingerprintImageService, IFingerprintTemplateService fingerprintTemplateService, ISignatureService signatureService, IUnitOfWorkAsync unitOfWork)
        {
            _projectService = projectService;
            _fingerprintReasonService = fingerprintReasonService;
            _photographService = photographService;
            _fingerprintTemplateService = fingerprintTemplateService;
            _fingerprintImageService = fingerprintImageService;
            _signatureService = signatureService;
            _customDataService = customDataService;
            _baseDataService = baseDataService;
            _unitOfWork = unitOfWork;
        }
        
        public ActionResult GetView()
        {
            return View();
        }
        
        public ActionResult CustomDataView()
        {
            return View();
        }
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Rejections()
        {
            return View();
        }
        public ActionResult ApproveEnrollment()
        {
            return View();
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

        public ActionResult DataImport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportData5()
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
                        acResponse.Message = "The provided import File could not be correctly accessed. Please try again later";
                        return Json(acResponse, JsonRequestBehavior.AllowGet);
                    }

                    const string folderPath = "~/TempProject";

                    var mainPath = Server.MapPath(folderPath);

                    if (!Directory.Exists(mainPath))
                    {
                        Directory.CreateDirectory(mainPath);
                        var dInfo = new DirectoryInfo(mainPath);
                        var dSecurity = dInfo.GetAccessControl();
                        dSecurity.AddAccessRule(
                            new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                                FileSystemRights.FullControl,
                                InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                                PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
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

                    var enrollmentBackups = JsonConvert.DeserializeObject<List<EnrollmentBackup>>(System.IO.File.ReadAllText(path));
                    if (!enrollmentBackups.Any() || enrollmentBackups.Any(b => b.BaseData == null) || enrollmentBackups.Any(b => !b.FingerprintImages.Any()))
                    {
                        acResponse.Code = -1;
                        acResponse.Message = "The provided import File could not be correctly accessed. Please try again later";
                        return Json(acResponse, JsonRequestBehavior.AllowGet);
                    }
                    var result = ImportFromBackup(enrollmentBackups);
                    if (!result.Any())
                    {
                        acResponse.Code = -1;
                        acResponse.Message = "The provided import File could not be correctly accessed. Please try again later";
                        return Json(acResponse, JsonRequestBehavior.AllowGet);
                    }

                    acResponse.Code = 5;
                    acResponse.Message = result.Count + " item(s) were successfully imported";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                acResponse.Code = -1;
                acResponse.Message = "The provided import File could not be correctly accessed. Please try again later";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = e.Message;
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ImportData(List<EnrollmentBackup> imports)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (!imports.Any() || imports.Any(b => b.BaseData == null) || imports.Any(b => !b.FingerprintImages.Any()))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "The provided import File could not be correctly accessed. Please try again later";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                var result = ImportFromBackup(imports);
                if (!result.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "The provided import File could not be correctly accessed. Please try again later";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                acResponse.Code = 5;
                acResponse.Message = result.Count + " item(s) were successfully imported";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
               
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = e.Message;
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }
        
        private List<EnrollmentBackup> ImportFromBackup(List<EnrollmentBackup> imports)
        {
            var importedList = new List<EnrollmentBackup>();
            var connectionString = ConfigurationManager.ConnectionStrings["crimsDbEntities"].ConnectionString;
           
            imports.ForEach(b =>
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var table = new DataTable();
                    new MySqlDataAdapter($"SELECT EnrollmentId from basedatas WHERE EnrollmentId='{b.BaseData.EnrollmentId}'", conn).Fill(table);

                    int basedataProcessed;
                    var middleName = !string.IsNullOrEmpty(b.BaseData.MiddleName) ? b.BaseData.MiddleName.Replace("'", "''").ToUpper() : null;
                    using (var cmd = new MySqlCommand(null, conn) { CommandTimeout = 604800 })
                    {
                        if (!(table.Rows.Count > 0))
                        {
                            var query =
                                "INSERT INTO basedatas (EnrollmentId,ProjectCode,CreatedBy,LastUpdatedby,FormPath,ProjectSiteId,ApprovalStatus,DOB,EnrollmentDate,DateLastUpdated,Email,Firstname," +
                                "MiddleName,Gender,Title,ProjectPrimaryCode,CuntryCode,MobileNumber,ValidIdNumber,Surname)" +
                                " VALUES (@enrollmentid,@projectcode,@createdby,@lastupdatedby,@formpath," +
                                "@projectsiteid,@approvalstatus,@dob,@enrollmentdate,@datelastupdated,@email," +
                                "@firstname,@middleName,@gender,@title,@projectprimarycode,@cuntryCode,@mobilenumber,@validIdnumber,@surname)";
                            cmd.CommandText = query;
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add("@enrollmentid", MySqlDbType.String).Value = b.BaseData.EnrollmentId;
                            cmd.Parameters.Add("@projectcode", MySqlDbType.String).Value = b.BaseData.ProjectCode;
                            cmd.Parameters.Add("@createdby", MySqlDbType.String).Value = b.BaseData.CreatedBy;
                            cmd.Parameters.Add("@lastupdatedby", MySqlDbType.String).Value = b.BaseData.LastUpdatedby;
                            cmd.Parameters.Add("@formpath", MySqlDbType.String).Value = b.BaseData.FormPath;
                            cmd.Parameters.Add("@projectsiteid", MySqlDbType.String).Value = b.BaseData.ProjectSiteId;
                            cmd.Parameters.Add("@approvalstatus", MySqlDbType.String).Value = b.BaseData.ApprovalStatus;
                            cmd.Parameters.Add("@dob", MySqlDbType.String).Value = b.BaseData.DOB;
                            cmd.Parameters.Add("@enrollmentdate", MySqlDbType.DateTime).Value = b.BaseData.EnrollmentDate;
                            cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.BaseData.DateLastUpdated;
                            cmd.Parameters.Add("@email", MySqlDbType.String).Value = b.BaseData.Email;
                            cmd.Parameters.Add("@firstname", MySqlDbType.String).Value = b.BaseData.Firstname.ToUpper();
                            cmd.Parameters.Add("@middleName", MySqlDbType.String).Value = middleName;
                            cmd.Parameters.Add("@gender", MySqlDbType.String).Value = b.BaseData.Gender;
                            cmd.Parameters.Add("@title", MySqlDbType.String).Value = b.BaseData.Title;
                            cmd.Parameters.Add("@projectprimarycode", MySqlDbType.String).Value = b.BaseData.ProjectPrimaryCode;
                            cmd.Parameters.Add("@cuntryCode", MySqlDbType.String).Value = b.BaseData.CuntryCode;
                            cmd.Parameters.Add("@mobilenumber", MySqlDbType.String).Value = b.BaseData.MobileNumber;
                            cmd.Parameters.Add("@validIdnumber", MySqlDbType.String).Value = b.BaseData.ValidIdNumber;
                            cmd.Parameters.Add("@surname", MySqlDbType.String).Value = b.BaseData.Surname.ToUpper();
                            basedataProcessed = cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            var query = "UPDATE basedatas SET LastUpdatedby = lastupdatedby,ApprovalStatus = @approvalstatus,DOB=@dob," +
                                        "DateLastUpdated=@datelastupdated,Email=@email,Firstname=@firstname,MiddleName=@middleName" +
                                        ",Gender=@gender,Title=@title,CuntryCode=@cuntryCode,ProjectPrimaryCode=@projectprimarycode,ValidIdNumber=@validIdnumber" +
                                        ",Surname=@surname,MobileNumber=@mobilenumber WHERE EnrollmentId = '{b.BaseData.EnrollmentId}'";

                            cmd.CommandText = query;
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add("@lastupdatedby", MySqlDbType.String).Value = b.BaseData.LastUpdatedby;
                            cmd.Parameters.Add("@formpath", MySqlDbType.String).Value = b.BaseData.FormPath;
                            cmd.Parameters.Add("@projectsiteid", MySqlDbType.String).Value = b.BaseData.ProjectSiteId;
                            cmd.Parameters.Add("@approvalstatus", MySqlDbType.String).Value = b.BaseData.ApprovalStatus;
                            cmd.Parameters.Add("@dob", MySqlDbType.String).Value = b.BaseData.DOB;
                            cmd.Parameters.Add("@enrollmentdate", MySqlDbType.DateTime).Value = b.BaseData.EnrollmentDate;
                            cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.BaseData.DateLastUpdated;
                            cmd.Parameters.Add("@email", MySqlDbType.String).Value = b.BaseData.Email;
                            cmd.Parameters.Add("@firstname", MySqlDbType.String).Value = b.BaseData.Firstname.ToUpper();
                            cmd.Parameters.Add("@middleName", MySqlDbType.String).Value = middleName;
                            cmd.Parameters.Add("@gender", MySqlDbType.String).Value = b.BaseData.Gender;
                            cmd.Parameters.Add("@title", MySqlDbType.String).Value = b.BaseData.Title;
                            cmd.Parameters.Add("@projectprimarycode", MySqlDbType.String).Value = b.BaseData.ProjectPrimaryCode;
                            cmd.Parameters.Add("@cuntryCode", MySqlDbType.String).Value = b.BaseData.CuntryCode;
                            cmd.Parameters.Add("@mobilenumber", MySqlDbType.String).Value = b.BaseData.MobileNumber;
                            cmd.Parameters.Add("@validIdnumber", MySqlDbType.String).Value = b.BaseData.ValidIdNumber;
                            cmd.Parameters.Add("@surname", MySqlDbType.String).Value = b.BaseData.Surname.ToUpper();
                            basedataProcessed = cmd.ExecuteNonQuery();
                        }

                        if (basedataProcessed > 0)
                        {
                            if (b.CustomDatas.Any())
                            {
                                b.CustomDatas.ToList().ForEach(c =>
                                {
                                    var csTable = new DataTable();
                                    new MySqlDataAdapter($"SELECT CustomDataId FROM customdatas WHERE EnrollmentId = '{c.EnrollmentId}' AND CustomDataId='{c.CustomDataId}'", conn).Fill(csTable);
                                    if (!(csTable.Rows.Count > 0))
                                    {
                                        var query = "INSERT INTO customdatas (EnrollmentId,CustomDataId,ProjectSIteId,CrimsCustomData,CustomFieldId,CustomListId,ChildCrimsCustomData,DateLastUpdated)" +
                                                    " VALUES (@enrollmentid,@customdataid,@projectsiteid,@crimscustomdata,@customfieldid,@customlistid," +
                                                    "@childcrimscustomdata,@datelastupdated)";
                                        cmd.Parameters.Clear();
                                        cmd.CommandText = query;
                                        cmd.Parameters.Add("@enrollmentid", MySqlDbType.String).Value = c.EnrollmentId;
                                        cmd.Parameters.Add("@customdataid", MySqlDbType.String).Value = c.CustomDataId;
                                        cmd.Parameters.Add("@crimscustomdata", MySqlDbType.String).Value = c.CrimsCustomData;
                                        cmd.Parameters.Add("@projectsiteid", MySqlDbType.String).Value = c.ProjectSIteId;
                                        cmd.Parameters.Add("@customfieldid", MySqlDbType.String).Value = c.CustomFieldId;
                                        cmd.Parameters.Add("@customlistid", MySqlDbType.String).Value = c.CustomListId;
                                        cmd.Parameters.Add("@childcrimscustomdata", MySqlDbType.String).Value = c.ChildCrimsCustomData;
                                        cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = c.DateLastUpdated;
                                        basedataProcessed = cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        var query = "UPDATE customdatas SET CrimsCustomData = @crimscustomdata,CustomFieldId=@customfieldid," +
                                                    $"CustomListId=@customlistid,ChildCrimsCustomData=@childcrimscustomdata,DateLastUpdated=@datelastupdated WHERE CustomDataId = '{c.CustomDataId}'";

                                        cmd.CommandText = query;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.Add("@crimscustomdata", MySqlDbType.String).Value = c.CrimsCustomData;
                                        cmd.Parameters.Add("@customfieldid", MySqlDbType.String).Value = c.CustomFieldId;
                                        cmd.Parameters.Add("@customlistid", MySqlDbType.String).Value = c.CustomListId;
                                        cmd.Parameters.Add("@childcrimscustomdata", MySqlDbType.String).Value = c.ChildCrimsCustomData;
                                        cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = c.DateLastUpdated;
                                        basedataProcessed = cmd.ExecuteNonQuery();
                                    }

                                });
                            }

                            if (b.FingerprintImages != null && b.FingerprintImages.Any())
                            {
                                b.FingerprintImages.ToList().ForEach(c =>
                                {
                                    var fiTable = new DataTable();
                                    new MySqlDataAdapter($"SELECT EnrollmentId from fingerprintimages WHERE EnrollmentId='{c.EnrollmentId}' AND FingerIndexId='{c.FingerIndexId}'", conn).Fill(fiTable);
                                    if (!(fiTable.Rows.Count > 0))
                                    {
                                        var query = "INSERT INTO fingerprintimages (EnrollmentId,FilePath,FingerIndexId,DateLastUpdated,FingerPrintImage)" +
                                                    " VALUES (@enrollmentid,@filepath,@fingerindexid,@datelastupdated,@fingerprintimage)";
                                        cmd.CommandText = query;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.Add("@enrollmentid", MySqlDbType.String).Value = c.EnrollmentId;
                                        cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = c.FilePath;
                                        cmd.Parameters.Add("@fingerindexid", MySqlDbType.Int32).Value = c.FingerIndexId;
                                        cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = c.DateLastUpdated;
                                        cmd.Parameters.Add("@fingerprintimage", MySqlDbType.VarBinary).Value = c.FingerPrintImage;
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        var query = $"UPDATE fingerprintimages SET DateLastUpdated = @datelastupdated,FingerPrintImage=@fingerprintimage,FilePath=@filepath WHERE EnrollmentId = '{c.EnrollmentId}' AND FingerIndexId='{c.FingerIndexId}'";
                                        cmd.CommandText = query;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.Add("@EnrollmentId", MySqlDbType.String).Value = c.EnrollmentId;
                                        cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = c.FilePath;
                                        cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = c.DateLastUpdated;
                                        cmd.Parameters.Add("@fingerprintimage", MySqlDbType.VarBinary).Value = c.FingerPrintImage;
                                        cmd.ExecuteNonQuery();
                                    }
                                });
                            }

                            if (b.FingerprintReasons != null && b.FingerprintReasons.Any())
                            {
                                b.FingerprintReasons.ToList().ForEach(c =>
                                {
                                    var frTable = new DataTable();
                                    new MySqlDataAdapter($"SELECT EnrollmentId from fingerprintreasons WHERE EnrollmentId='{c.EnrollmentId}' AND FingerIndex='{c.FingerIndex}'", conn).Fill(frTable);
                                    if (!(frTable.Rows.Count > 0))
                                    {
                                        var query = "INSERT INTO fingerprintreasons (EnrollmentId,FingerIndex,DateLastUpdated,fingerReason)" +
                                                    $" VALUES ('{c.EnrollmentId}','{c.FingerIndex}','{c.DateLastUpdated:yyyy-MM-dd hh:mm:ss}','{c.FingerReason.Replace("'", "''")}')";
                                        cmd.CommandText = query;
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        var query = $"UPDATE fingerprintreasons SET DateLastUpdated = '{c.DateLastUpdated:yyyy-MM-dd hh:mm:ss}',fingerReason='{c.FingerReason}' WHERE EnrollmentId = '{c.EnrollmentId}' AND FingerIndex='{c.FingerIndex}'";
                                        cmd.CommandText = query;
                                        cmd.ExecuteNonQuery();
                                    }

                                });
                            }

                            if (!string.IsNullOrEmpty(b.FingerprintTemplate?.EnrollmentId))
                            {
                                var ftTable = new DataTable();
                                new MySqlDataAdapter($"SELECT EnrollmentId from fingerprinttemplates WHERE EnrollmentId='{b.FingerprintTemplate.EnrollmentId}'", conn).Fill(ftTable);
                                if (!(ftTable.Rows.Count > 0))
                                {
                                    var query = "INSERT INTO fingerprinttemplates (EnrollmentId,Template,FilePath,UniquenessStatus,DateLastUpdated)" +
                                                " VALUES (@enrollmentId,@template,@filepath,@uniquenessstatus,@datelastupdated)";

                                    cmd.CommandText = query;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@enrollmentId", MySqlDbType.String).Value = b.FingerprintTemplate.EnrollmentId;
                                    cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = b.FingerprintTemplate.FilePath;
                                    cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.FingerprintTemplate.DateLastUpdated;
                                    cmd.Parameters.Add("@uniquenessstatus", MySqlDbType.Int32).Value = b.FingerprintTemplate.UniquenessStatus;
                                    cmd.Parameters.Add("@template", MySqlDbType.Blob).Value = b.FingerprintTemplate.Template;
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    var query = $"UPDATE fingerprinttemplates SET DateLastUpdated = @datelastupdated,Template=@template,UniquenessStatus=@uniquenessstatus WHERE EnrollmentId = '{b.FingerprintTemplate.EnrollmentId}'";
                                    cmd.CommandText = query;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = b.FingerprintTemplate.FilePath;
                                    cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.FingerprintTemplate.DateLastUpdated;
                                    cmd.Parameters.Add("@uniquenessstatus", MySqlDbType.Int32).Value = b.FingerprintTemplate.UniquenessStatus;
                                    cmd.Parameters.Add("@template", MySqlDbType.Blob).Value = b.FingerprintTemplate.Template;
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            if (!string.IsNullOrEmpty(b.Photograph?.EnrollmentId))
                            {
                                var phTable = new DataTable();
                                new MySqlDataAdapter($"SELECT EnrollmentId from photographs WHERE EnrollmentId='{b.Photograph.EnrollmentId}'", conn).Fill(phTable);
                                if (!(phTable.Rows.Count > 0))
                                {
                                    var query = "INSERT INTO photographs (EnrollmentId,PhotographId,PhotographTemplate,PhotographImagePath,DateLastUpdated,PhotographImage)" +
                                                " VALUES (@enrollmentid,@photographid,@photographtemplate,@photographimagepath,@datelastupdated,@photographimage)";

                                    cmd.CommandText = query;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@enrollmentid", MySqlDbType.String).Value = b.Photograph.EnrollmentId;
                                    cmd.Parameters.Add("@photographimagepath", MySqlDbType.String).Value = b.Photograph.PhotographImagePath;
                                    cmd.Parameters.Add("@photographid", MySqlDbType.String).Value = b.Photograph.PhotographId;
                                    cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.Photograph.DateLastUpdated;
                                    cmd.Parameters.Add("@photographtemplate", MySqlDbType.Blob).Value = b.Photograph.PhotographTemplate;
                                    cmd.Parameters.Add("@photographimage", MySqlDbType.Blob).Value = b.Photograph.PhotographImage;
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    var query = $"UPDATE photographs SET DateLastUpdated = @datelastupdated,PhotographTemplate=@photographtemplate,PhotographImage=@photographimage WHERE EnrollmentId = '{b.Photograph.EnrollmentId}'";
                                    cmd.CommandText = query;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@photographimagepath", MySqlDbType.String).Value = b.Photograph.PhotographImagePath;
                                    cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.Photograph.DateLastUpdated;
                                    cmd.Parameters.Add("@photographtemplate", MySqlDbType.Blob).Value = b.Photograph.PhotographTemplate;
                                    cmd.Parameters.Add("@photographimage", MySqlDbType.Blob).Value = b.Photograph.PhotographImage;
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            if (!string.IsNullOrEmpty(b.Signature?.EnrollmentId))
                            {
                                var phTable = new DataTable();
                                new MySqlDataAdapter($"SELECT EnrollmentId from signatures WHERE EnrollmentId='{b.Signature.EnrollmentId}'", conn).Fill(phTable);
                                if (!(phTable.Rows.Count > 0))
                                {
                                    var query = "INSERT INTO signatures (EnrollmentId,SignatureImage,FilePath,DateLastUpdated)" +
                                                " VALUES (@enrollmentid,@signatureimage,@filepath,@datelastupdated)";
                                    cmd.CommandText = query;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@enrollmentid", MySqlDbType.String).Value = b.Signature.EnrollmentId;
                                    cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = b.Signature.FilePath;
                                    cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.Signature.DateLastUpdated;
                                    cmd.Parameters.Add("@signatureimage", MySqlDbType.Blob).Value = b.Signature.SignatureImage;
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    var query = $"UPDATE signatures SET DateLastUpdated = @datelastupdated,SignatureImage=@signatureimage,FilePath=@filepath WHERE EnrollmentId = '{b.Signature.EnrollmentId}'";
                                    cmd.CommandText = query;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@filepath", MySqlDbType.String).Value = b.Signature.FilePath;
                                    cmd.Parameters.Add("@datelastupdated", MySqlDbType.DateTime).Value = b.Signature.DateLastUpdated;
                                    cmd.Parameters.Add("@signatureimage", MySqlDbType.Blob).Value = b.Signature.SignatureImage;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        importedList.Add(b);
                    }
                }

            });
            return importedList;
           

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
        [HttpGet]
        public ActionResult GetEnrollments(JQueryDataTableParamModel param)
        {
            try
            {
                var currentProject = GetProjectInSession();
                if (currentProject == null)
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
                var userId = User.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new List<BaseDataViewModel>(), JsonRequestBehavior.AllowGet);
                }

                var registeredUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Where(e => e.Id == userId).Include("UserInfo").ToList();
                if (!registeredUsers.Any())
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

                var enrolledBy = registeredUsers[0].UserInfo.Id;
                var approved = (int)EnumManager.ApprovalStatus.Approved;
                IEnumerable<BaseDataViewModel> dataList;
                var dataListx = new List<BaseDataViewModel> ();
                var connStr = ConfigurationManager.ConnectionStrings["crimsDbEntities"].ConnectionString;
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    var skip = param.iDisplayStart > 0 ? " OFFSET " + param.iDisplayStart : string.Empty;
                    
                    int countG;
                    if (User.IsInRole("Admin") || User.IsInRole("Site_Administrator"))
                    {
                        var sql = string.IsNullOrEmpty(param.sSearch)
                            ? $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where t.ProjectCode ='{currentProject.ProjectCode}' and t.ApprovalStatus != '{approved}' order by t.EnrollmentDate desc limit " +
                              param.iDisplayLength + skip
                            : $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where t.ProjectCode ='{currentProject.ProjectCode}' and t.ApprovalStatus != '{approved}' and t.EnrollmentId = '{param.sSearch}' order by t.EnrollmentDate desc limit " +
                              param.iDisplayLength + skip;

                        countG = string.IsNullOrEmpty(param.sSearch)
                            ? _baseDataService.Query().Select().Count()
                            : _baseDataService.Query().Select().Count(t => t.EnrollmentId == param.sSearch);
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
                                    FormPath = rdr["FormPath"].ToString(),
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
                                dataListx.Add(newUser);
                            }
                        }
                    }
                    else
                    {
                        if (User.IsInRole("Enrollment_Officer"))
                        {
                            var sql = string.IsNullOrEmpty(param.sSearch)
                                ? $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where t.ProjectCode ='{currentProject.ProjectCode}' and t.ApprovalStatus != '{approved}' and (t.CreatedBy = '{enrolledBy}' or t.LastUpdatedby = '{enrolledBy}') order by t.EnrollmentDate desc limit " +
                                  param.iDisplayLength + skip
                                : $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where t.ProjectCode ='{currentProject.ProjectCode}' and t.ApprovalStatus != '{approved}' and t.EnrollmentId = '{param.sSearch}' and (t.CreatedBy = '{enrolledBy}' or t.LastUpdatedby = '{enrolledBy}') order by t.EnrollmentDate desc limit " +
                                  param.iDisplayLength + skip;

                            var cmd2 = new MySqlCommand(sql, conn);
                            countG = string.IsNullOrEmpty(param.sSearch)
                                ? _baseDataService
                                    .Query(b => b.CreatedBy == enrolledBy || b.LastUpdatedby == enrolledBy).Select()
                                    .Count()
                                : _baseDataService
                                    .Query(b => b.CreatedBy == enrolledBy || b.LastUpdatedby == enrolledBy).Select()
                                    .Count(f => f.EnrollmentId == param.sSearch);
                            using (var rdr = cmd2.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    if (!rdr.HasRows) continue;
                                    var dateRegistered = Convert.ToDateTime(rdr["EnrollmentDate"].ToString());
                                    var enrollmentDateStr = dateRegistered.ToString("dd/MM/yyyy");
                                    var newUser = new BaseDataViewModel
                                    {
                                        
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
                                    
                                    dataListx.Add(newUser);
                                }
                            }
                        }
                        else
                        {
                            return Redirect("Enrollment");
                        }
                    }

                    if (!dataListx.Any())
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

                    //var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                    //Func<BaseDataViewModel, string> orderingFunction = (c => sortColumnIndex == 1 ? c.Surname : c.EnrollmentDateStr);
                    //var sortDirection = Request["sSortDir_0"]; // asc or desc
                    //dataList = sortDirection == "asc" ? dataListx.OrderBy(orderingFunction) : dataListx.OrderByDescending(orderingFunction);

                    dataList = dataListx;
                    var result = from c in dataList
                        //select new[] { c.EnrollmentId, c.ProjectPrimaryCode, c.Surname, c.Firstname, c.BiometricStatus, c.EnrollmentOfficer, c.EnrollmentDateStr };
                        select new[]
                        {
                            c.EnrollmentId, c.ProjectPrimaryCode, c.Surname, c.Firstname, c.EnrollmentOfficer,
                            c.EnrollmentDateStr
                        };
                    return Json(new
                        {
                            param.sEcho,
                            iTotalRecords = countG,
                            iTotalDisplayRecords = countG,
                            aaData = result
                        },
                        JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
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
        [HttpGet]
        public ActionResult GetApprovedEnrollments(JQueryDataTableParamModel param)
        {
            try
            {
                var currentProject = GetProjectInSession();
                if (currentProject == null)
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
                var userId = User.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new List<BaseDataViewModel>(), JsonRequestBehavior.AllowGet);
                }

                var registeredUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Where(e => e.Id == userId).Include("UserInfo").ToList();
                if (!registeredUsers.Any())
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

                var enrolledBy = registeredUsers[0].UserInfo.Id;
                var approved = (int)EnumManager.ApprovalStatus.Approved;
                IEnumerable<BaseDataViewModel> dataList;
                var dataListx = new List<BaseDataViewModel>();
                var connStr = ConfigurationManager.ConnectionStrings["crimsDbEntities"].ConnectionString;
                var conn = new MySqlConnection(connStr);
                var skip = param.iDisplayStart > 0 ? " OFFSET " + param.iDisplayStart : string.Empty;
                conn.Open();

                int countG;
                if (User.IsInRole("Admin") || User.IsInRole("Site_Administrator"))
                {
                    var sql = string.IsNullOrEmpty(param.sSearch) ? $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where t.ProjectCode ='{currentProject.ProjectCode}' and t.ApprovalStatus = '{approved}' order by t.EnrollmentDate desc limit " + param.iDisplayLength + skip : $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where t.ProjectCode ='{currentProject.ProjectCode}' and t.ApprovalStatus = '{approved}' and t.EnrollmentId = '{param.sSearch}' order by t.EnrollmentDate desc limit " + param.iDisplayLength + skip;
                    
                    countG = _baseDataService.Query().Select().Count();
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
                                FormPath = rdr["FormPath"].ToString(),
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
                            
                            dataListx.Add(newUser);
                        }
                    }
                }
                else
                {
                    if (User.IsInRole("Enrollment_Officer"))
                    {
                        var sql = string.IsNullOrEmpty(param.sSearch) ? $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where t.ProjectCode ='{currentProject.ProjectCode}' and t.ApprovalStatus = '{approved}' and (t.CreatedBy = '{enrolledBy}' or t.LastUpdatedby = '{enrolledBy}') order by t.EnrollmentDate desc limit " + param.iDisplayLength + skip : $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where t.ProjectCode ='{currentProject.ProjectCode}' and t.ApprovalStatus = '{approved}' and t.EnrollmentId = '{param.sSearch}' and (t.CreatedBy = '{enrolledBy}' or t.LastUpdatedby = '{enrolledBy}') order by t.EnrollmentDate desc limit " + param.iDisplayLength + skip;

                        var cmd2 = new MySqlCommand(sql, conn);
                        countG = _baseDataService.Query(b => b.CreatedBy == enrolledBy || b.LastUpdatedby == enrolledBy).Select().Count();
                        using (var rdr = cmd2.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (!rdr.HasRows) continue;
                                var dateRegistered = Convert.ToDateTime(rdr["EnrollmentDate"].ToString());
                                var enrollmentDateStr = dateRegistered.ToString("dd/MM/yyyy");
                                var newUser = new BaseDataViewModel
                                {
                                    
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
                                
                                dataListx.Add(newUser);
                            }
                        }
                    }
                    else
                    {
                        return Redirect("Enrollment");
                    }
                }

                if (!dataListx.Any())
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

                //var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                //Func<BaseDataViewModel, string> orderingFunction = (c => sortColumnIndex == 1 ? c.Surname : c.EnrollmentDateStr);
                //var sortDirection = Request["sSortDir_0"]; // asc or desc
                //dataList = sortDirection == "asc" ? dataListx.OrderBy(orderingFunction) : dataListx.OrderByDescending(orderingFunction);

                dataList = dataListx;
                var result = from c in dataList
                             select new[] { c.EnrollmentId, c.ProjectPrimaryCode, c.Surname, c.Firstname, c.EnrollmentOfficer, c.EnrollmentDateStr };
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
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
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
        [HttpGet]
        public ActionResult GetRejectedEnrollments(JQueryDataTableParamModel param)
        {
            try
            {
                var currentProject = GetProjectInSession();
                if (currentProject == null)
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
                var userId = User.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new List<BaseDataViewModel>(), JsonRequestBehavior.AllowGet);
                }

                var registeredUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Where(e => e.Id == userId).Include("UserInfo").ToList();
                if (!registeredUsers.Any())
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

                var enrolledBy = registeredUsers[0].UserInfo.Id;
                var rejected = (int)EnumManager.ApprovalStatus.Rejected;
                IEnumerable<BaseDataViewModel> dataList;
                var dataListx = new List<BaseDataViewModel>();
                var connStr = ConfigurationManager.ConnectionStrings["crimsDbEntities"].ConnectionString;
                var conn = new MySqlConnection(connStr);
                var skip = param.iDisplayStart > 0 ? " OFFSET " + param.iDisplayStart : string.Empty;
                conn.Open();

                int countG;
                if (User.IsInRole("Admin") || User.IsInRole("Site_Administrator"))
                {
                    var sql = string.IsNullOrEmpty(param.sSearch) ? $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where t.ProjectCode ='{currentProject.ProjectCode}' and t.ApprovalStatus = '{rejected}' order by t.EnrollmentDate desc limit " + param.iDisplayLength + skip : $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where t.ProjectCode ='{currentProject.ProjectCode}' and t.ApprovalStatus = '{rejected}' and t.EnrollmentId = '{param.sSearch}' order by t.EnrollmentDate desc limit " + param.iDisplayLength + skip;
                   
                    countG = _baseDataService.Query().Select().Count();
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
                                FormPath = rdr["FormPath"].ToString(),
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

                            dataListx.Add(newUser);
                        }
                    }
                }
                else
                {
                    if (User.IsInRole("Enrollment_Officer"))
                    {
                        var sql = string.IsNullOrEmpty(param.sSearch) ? $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where t.ProjectCode ='{currentProject.ProjectCode}' and t.ApprovalStatus = '{rejected}' and (t.CreatedBy = '{enrolledBy}' or t.LastUpdatedby = '{enrolledBy}') order by t.EnrollmentDate desc limit " + param.iDisplayLength + skip : $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where t.ProjectCode ='{currentProject.ProjectCode}' and t.ApprovalStatus = '{rejected}' and t.EnrollmentId = '{param.sSearch}' and (t.CreatedBy = '{enrolledBy}' or t.LastUpdatedby = '{enrolledBy}') order by t.EnrollmentDate desc limit " + param.iDisplayLength + skip;
                       
                        var cmd2 = new MySqlCommand(sql, conn);
                        countG = _baseDataService.Query(b => b.CreatedBy == enrolledBy || b.LastUpdatedby == enrolledBy).Select().Count();
                        using (var rdr = cmd2.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (!rdr.HasRows) continue;
                                var dateRegistered = Convert.ToDateTime(rdr["EnrollmentDate"].ToString());
                                var enrollmentDateStr = dateRegistered.ToString("dd/MM/yyyy");
                                var newUser = new BaseDataViewModel
                                {
                                    
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

                                dataListx.Add(newUser);
                            }
                        }
                    }
                    else
                    {
                        return Redirect("Enrollment");
                    }
                }

                if (!dataListx.Any())
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

                //var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                //Func<BaseDataViewModel, string> orderingFunction = (c => sortColumnIndex == 1 ? c.Surname : c.EnrollmentDateStr);
                //var sortDirection = Request["sSortDir_0"]; // asc or desc
                //dataList = sortDirection == "asc" ? dataListx.OrderBy(orderingFunction) : dataListx.OrderByDescending(orderingFunction);

                dataList = dataListx;

                var result = from c in dataList
                             select new[] { c.EnrollmentId, c.ProjectPrimaryCode, c.Surname, c.Firstname, c.EnrollmentOfficer, c.EnrollmentDateStr };
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
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
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

        public ActionResult GetEnrollmentsX(JQueryDataTableParamModel param)
        {
            try
            {
                var currentProject = GetProjectInSession();
                if (currentProject == null)
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
                var userId = User.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
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

                var registeredUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Where(e => e.Id == userId).Include("UserInfo").ToList();
                if (!registeredUsers.Any())
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
                
                var enrolledBy = registeredUsers[0].UserInfo.Id;
                var approved = (int)EnumManager.ApprovalStatus.Approved;
                var dataListx = new List<BaseDataViewModel>();
                var dataList = new List<BaseDataViewModel>();
                int countG;
                var connStr = ConfigurationManager.ConnectionStrings["crimsDbEntities"].ConnectionString; ;
                var conn = new MySqlConnection(connStr);
                conn.Open();


                if (User.IsInRole("Admin") || User.IsInRole("Site_Administrator"))
                {
                    var sql = $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where t.ProjectCode ='{currentProject.ProjectCode}' and t.ApprovalStatus != '{approved}'";
                    //var sql = $"select t.* from basedatas as t join userprofiles as o on o.Id = t.CreatedBy where t.ProjectCode ='{currentProject.ProjectCode}' and t.ApprovalStatus != '{approved}'";
                    countG = _baseDataService.Query().Select().Count();
                   
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
                                FormPath = rdr["FormPath"].ToString(),
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

                            dataList.Add(newUser);
                        }
                    }
                }
                else
                {
                    if (User.IsInRole("Enrollment_Officer"))
                    {
                        var sql2 = $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where t.ProjectCode ='{currentProject.ProjectCode}' and t.ApprovalStatus != '{approved}' and (t.CreatedBy = '{enrolledBy}' or t.LastUpdatedby = '{enrolledBy}')";
                        
                        countG = _baseDataService.Query(b => b.CreatedBy == enrolledBy || b.LastUpdatedby == enrolledBy).Select().Count();
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

                                dataList.Add(newUser);
                            }
                        }
                    }
                    else
                    {
                        return Redirect("Enrollment");
                    }
                }

                if (!dataList.Any())
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

                var folderPath = "~/UserRecords/";

                var ext = new List<string> { ".jpg" };

                dataList.ForEach(d =>
                {
                    var bioPath = Server.MapPath(folderPath + d.EnrollmentId);
                    if (!Directory.Exists(bioPath))
                    {
                        d.BiometricStatus = "None";
                        return;
                    }

                    var myFiles = Directory.GetFiles(bioPath, "*.*", SearchOption.AllDirectories).Where(s => ext.Contains(Path.GetExtension(s))).ToList();
                    if (!myFiles.Any())
                    {
                        d.BiometricStatus = "None";
                        return;
                    }

                    var photos = myFiles.Where(f => f.Contains("photo_image")).ElementAt(0).ToList();
                    if (!photos.Any())
                    {
                        d.BiometricStatus = "Incomplete";
                    }

                    var fingerPrints = myFiles.Where(f => Path.GetFileName(f).StartsWith("LF") || Path.GetFileName(f).StartsWith("RF")).ToList();
                    var count = fingerPrints.Count();

                    if (count < 1 || count < 10)
                    {
                        d.BiometricStatus = "Incomplete";
                    }
                    var signatures = myFiles.Where(f => Path.GetFileName(f).Contains("sign_image")).ToList();
                    if (!signatures.Any())
                    {
                        d.BiometricStatus = "Incomplete";
                    }

                });

                dataList = dataListx;
                var result = from c in dataList
                             select new[] { c.EnrollmentId, c.ProjectPrimaryCode, c.Surname, c.Firstname, c.BiometricStatus, c.EnrollmentOfficer, c.EnrollmentDateStr };
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
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
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

        [HttpPost]
        public ActionResult TriggerBio()
        {
            var acResponse = new ActivityResponse();
            try
            {
                acResponse.Code = 5;
                acResponse.Message = "Process was completed Successfully";
                //var runningProcessByName = Process.GetProcessesByName("Crims.UI.Win.Enroll");
                var runningProcessByName = Process.GetProcessesByName("ProcessStartTest");

                var p = new Process
                {
                    StartInfo =
                    {
                        FileName = @"C:\Users\abum\Documents\Visual Studio 2015\Projects\MVC\ProcessStartTest\ProcessStartTest\bin\Debug\ProcessStartTest.exe",
                        Arguments = "crimsadmin@chams.com f989a651-2617-481b-b900-985a2c825cce Admin"
                    }
                };


                //if (runningProcessByName.Length == 0)
                //{
                    p.Start();
                    //Process.Start(@"C:\inetpub\wwwroot\crimsweb\CrimsUIEnroll\Crims.UI.Win.Enroll.exe", "www.northwindtraders.com");
               //}

               // runningProcessByName[0].StartInfo.Arguments = "crimsadmin@chams.com f989a651-2617-481b-b900-985a2c825cce Admin";
               //SwitchToThisWindow(runningProcessByName[0].MainWindowHandle, true);

            return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EnrollBaseData(BaseData basedata)
        {
            var acResponse = new ActivityResponse();
            try
            {
                //if (string.IsNullOrEmpty(basedata.Email))
                //{
                //    acResponse.Code = -1;
                //    acResponse.Message = "Please provide Email";
                //    return Json(acResponse, JsonRequestBehavior.AllowGet);
                //}
                
                if (string.IsNullOrEmpty(basedata.MobileNumber))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Mobile Number";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(basedata.Surname))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Surname";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(basedata.Gender))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Gender";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(basedata.DOB))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Date Of Birthday";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                var duplicates = _baseDataService.Query(b => b.ProjectPrimaryCode.ToLower() == basedata.ProjectPrimaryCode.ToLower()).Select();
                if (duplicates.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "A user with : " + basedata.ProjectPrimaryCode + " already exists on the system";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                //var duplicateEmails = _baseDataService.Query(b => b.Email == basedata.Email).Select();
                //if (duplicateEmails.Any())
                //{
                //    acResponse.Code = -1;
                //    acResponse.Message = "A Record with : " + basedata.Email + " already exists on the system";
                //    return Json(acResponse, JsonRequestBehavior.AllowGet);
                //}

                var duplicatePhoneNumbers = _baseDataService.Query(b => b.MobileNumber == basedata.MobileNumber).Select();
                if (duplicatePhoneNumbers.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "A Record with : " + basedata.MobileNumber + " already exists on the system";
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

                basedata.EnrollmentId = Guid.NewGuid().ToString();
                basedata.CreatedBy = registeredUsers[0].UserInfo.Id;
                basedata.LastUpdatedby = registeredUsers[0].UserInfo.Id;
                basedata.EnrollmentDate = DateTime.Now;
                basedata.DateLastUpdated = DateTime.Now;
                basedata.ApprovalStatus = (int)EnumManager.ApprovalStatus.Pending;

                _baseDataService.Insert(basedata);
                _unitOfWork.SaveChanges();
                acResponse.Code = 5;
                acResponse.EnrollmentId = basedata.EnrollmentId;
                acResponse.Message = "Base Data was successfully Enrolled";
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
        public ActionResult PullEnrollmentFromRejection(string enrollmentId)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(enrollmentId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid action request";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                var userId = User.Identity.GetUserId();
                var registeredUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Where(e => e.Id == userId).Include("UserInfo").ToList();
                if (!registeredUsers.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "It seems your session has expired. Please logtry again or contact the support team.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                var originalEntities = _baseDataService.Query(b => b.EnrollmentId == enrollmentId).Select().ToList();
                if (!originalEntities.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An error was encountered. Process was terminated.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                var rejected = (int) EnumManager.ApprovalStatus.Rejected;
                var originalEntity = originalEntities[0];
                if (originalEntity.ApprovalStatus != rejected)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "This Enrollment was not rejected.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                originalEntity.DateLastUpdated = DateTime.Now;
                originalEntity.LastUpdatedby = registeredUsers[0].UserInfo.Id;
                originalEntity.ApprovalStatus = (int)EnumManager.ApprovalStatus.Pending;
                _baseDataService.Update(originalEntity);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Enrollment was successfully pulled out of rejected";
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
        public ActionResult CheckEnrollmentInForApproval(string enrollmentId)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(enrollmentId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid action request";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                var userId = User.Identity.GetUserId();
                var registeredUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Where(e => e.Id == userId).Include("UserInfo").ToList();
                if (!registeredUsers.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "It seems your session has expired. Please logtry again or contact the support team.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                
                var pending = (int)EnumManager.ApprovalStatus.Pending;
                var lockedForApproval = (int)EnumManager.ApprovalStatus.LockedForApproval;
                var userInfoId = registeredUsers[0].UserInfo.Id;

                var originalEntities = _baseDataService.Query(b => b.EnrollmentId == enrollmentId && b.ApprovalStatus == pending || (b.ApprovalStatus == lockedForApproval && b.LastUpdatedby == userInfoId)).Select().ToList();
                if (!originalEntities.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An error was encountered. Process was terminated.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
               
                var originalEntity = originalEntities[0];
                originalEntity.DateLastUpdated = DateTime.Now;
                originalEntity.LastUpdatedby = userInfoId;
                originalEntity.ApprovalStatus = lockedForApproval;
                _baseDataService.Update(originalEntity);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Enrollment was successfully Checked In for Approval";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult GetNextEnrollmentInForApproval()
        {
            try
            {
                var currentProject = GetProjectInSession();
                if (currentProject == null)
                {
                    return Json(new BaseDataViewModel(), JsonRequestBehavior.AllowGet);
                }
                var userId = User.Identity.GetUserId();
                var registeredUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Where(e => e.Id == userId).Include("UserInfo").ToList();
                if (!registeredUsers.Any())
                {
                    return Json(new BaseDataViewModel(), JsonRequestBehavior.AllowGet);
                }
                 
                var pending = (int)EnumManager.ApprovalStatus.Pending;
                var lockedForApproval = (int)EnumManager.ApprovalStatus.LockedForApproval;
                var userInfoId = registeredUsers[0].UserInfo.Id;
                
                var connStr = ConfigurationManager.ConnectionStrings["crimsDbEntities"].ConnectionString;
                var conn = new MySqlConnection(connStr);
                conn.Open();
                
                var sql = $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where (t.ProjectCode ='{currentProject.ProjectCode}' and ((select COUNT(f.EnrollmentId) from fingerprintimages f where f.EnrollmentId = t.EnrollmentId) > 0 and (select COUNT(p.EnrollmentId) from photographs p where p.EnrollmentId = t.EnrollmentId) > 0) " +
                          $"and t.ApprovalStatus = {pending} or (t.LastUpdatedby = '{userInfoId}' and t.ApprovalStatus = {lockedForApproval})) order by t.EnrollmentDate desc limit 1";

                var enrollment = new BaseDataViewModel();
                var cmd = new MySqlCommand(sql, conn);
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        if (!rdr.HasRows) continue;
                        var dateRegistered = Convert.ToDateTime(rdr["EnrollmentDate"].ToString());
                        var enrollmentDateStr = dateRegistered.ToString("dd/MM/yyyy");
                        enrollment = new BaseDataViewModel
                        {
                            
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
                            FormPath = rdr["FormPath"].ToString(),
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
                    }

                    if (string.IsNullOrEmpty(enrollment.EnrollmentId))
                    {
                        return Json(new BaseDataViewModel(), JsonRequestBehavior.AllowGet);
                    }
                    return Json(enrollment, JsonRequestBehavior.AllowGet);
                }
               
            }
            catch (Exception e)
            {
                return Json(new BaseDataViewModel(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetEnrollmentInForApproval(string enrollmentId)
        {
            try
            {
                if (string.IsNullOrEmpty(enrollmentId))
                {
                    return Json(new BaseDataViewModel(), JsonRequestBehavior.AllowGet);
                }
                var userId = User.Identity.GetUserId();
                var registeredUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Where(e => e.Id == userId).Include("UserInfo").ToList();
                if (!registeredUsers.Any())
                {
                    return Json(new BaseDataViewModel(), JsonRequestBehavior.AllowGet);
                }

                var pending = (int)EnumManager.ApprovalStatus.Pending;
                var lockedForApproval = (int)EnumManager.ApprovalStatus.LockedForApproval;
                var userInfoId = registeredUsers[0].UserInfo.Id;

                var connStr = ConfigurationManager.ConnectionStrings["crimsDbEntities"].ConnectionString;
                var conn = new MySqlConnection(connStr);
                conn.Open();
                
                var sql = $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where (t.EnrollmentId = '{enrollmentId}' and ((select COUNT(f.EnrollmentId) from fingerprintimages f where f.EnrollmentId = t.EnrollmentId) > 0  and (select COUNT(p.EnrollmentId) from photographs p where p.EnrollmentId = t.EnrollmentId) > 0)" +
                          $"and t.ApprovalStatus = {pending} or (t.LastUpdatedby = '{userInfoId}' and t.ApprovalStatus = {lockedForApproval})) order by t.EnrollmentDate desc limit 1";

                var enrollment = new BaseDataViewModel();
                var cmd = new MySqlCommand(sql, conn);
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        if (!rdr.HasRows) continue;
                        var dateRegistered = Convert.ToDateTime(rdr["EnrollmentDate"].ToString());
                        var enrollmentDateStr = dateRegistered.ToString("dd/MM/yyyy");
                        enrollment = new BaseDataViewModel
                        {
                            
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
                            FormPath = rdr["FormPath"].ToString(),
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
                    }

                    if (string.IsNullOrEmpty(enrollment.EnrollmentId))
                    {
                        return Json(new BaseDataViewModel(), JsonRequestBehavior.AllowGet);
                    }
                    return Json(enrollment, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new BaseDataViewModel(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetEnrollmentInfo(string enrollmentId)
        {
            try
            {
                var currentProject = GetProjectInSession();
                if (currentProject == null)
                {
                    return Json(new BaseDataViewModel(), JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(enrollmentId))
                {
                    return Json(new BaseDataViewModel(), JsonRequestBehavior.AllowGet);
                }
               
                var connStr = ConfigurationManager.ConnectionStrings["crimsDbEntities"].ConnectionString;
                var conn = new MySqlConnection(connStr);
                conn.Open();

                var sql = $"select t.*, o.* from basedatas t join userprofiles o on o.Id = t.CreatedBy where t.EnrollmentId = '{enrollmentId}' and t.EnrollmentId = '{enrollmentId}'";

                var enrollment = new BaseDataViewModel();
                var cmd = new MySqlCommand(sql, conn);
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        if (!rdr.HasRows) continue;
                        var dateRegistered = Convert.ToDateTime(rdr["EnrollmentDate"].ToString());
                        var enrollmentDateStr = dateRegistered.ToString("dd/MM/yyyy");
                        enrollment = new BaseDataViewModel
                        {
                            
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
                            FormPath = rdr["FormPath"].ToString(),
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
                    }

                    if (string.IsNullOrEmpty(enrollment.EnrollmentId))
                    {
                        return Json(new BaseDataViewModel(), JsonRequestBehavior.AllowGet);
                    }
                    return Json(enrollment, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new BaseDataViewModel(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateBaseData(BaseData basedata)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(basedata.EnrollmentId))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An Unknown error was encountered. Please try again later";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                //if (string.IsNullOrEmpty(basedata.Email))
                //{
                //    acResponse.Code = -1;
                //    acResponse.Message = "Please provide Email";
                //    return Json(acResponse, JsonRequestBehavior.AllowGet);
                //}

                if (string.IsNullOrEmpty(basedata.MobileNumber))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Mobile Number";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(basedata.Surname))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Surname";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(basedata.Gender))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Gender";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(basedata.DOB))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide Date Of Birthday";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                var duplicates = _baseDataService.Query(b => b.ProjectPrimaryCode.ToLower() == basedata.ProjectPrimaryCode.ToLower() && b.EnrollmentId != basedata.EnrollmentId).Select();
                if (duplicates.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "A user with : " + basedata.ProjectPrimaryCode + " already exists on the system";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                //var duplicateEmails = _baseDataService.Query(b => b.Email == basedata.Email && b.EnrollmentId != basedata.EnrollmentId).Select();
                //if (duplicateEmails.Any())
                //{
                //    acResponse.Code = -1;
                //    acResponse.Message = "A Record with : " + basedata.Email + " already exists on the system";
                //    return Json(acResponse, JsonRequestBehavior.AllowGet);
                //}

                var duplicatePhoneNumbers = _baseDataService.Query(b => b.MobileNumber == basedata.MobileNumber && b.EnrollmentId != basedata.EnrollmentId).Select();
                if (duplicatePhoneNumbers.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "A Record with : " + basedata.MobileNumber + " already exists on the system";
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

                var originalEntities = _baseDataService.Query(b => b.EnrollmentId == basedata.EnrollmentId).Select().ToList();
                if (!originalEntities.Any())
                {
                    acResponse.Code = -1;
                    acResponse.Message = "An error was encountered. Process was terminated.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
                var originalEntity = originalEntities[0];
                originalEntity.DateLastUpdated = DateTime.Now;
                originalEntity.LastUpdatedby = registeredUsers[0].UserInfo.Id;
                originalEntity.DOB = basedata.DOB;
                originalEntity.Surname = basedata.Surname;
                originalEntity.Firstname = basedata.Firstname;
                originalEntity.MiddleName = basedata.MiddleName;
                originalEntity.MobileNumber = basedata.MobileNumber;
                originalEntity.ProjectPrimaryCode = basedata.ProjectPrimaryCode;
                originalEntity.Title = basedata.Title;
                originalEntity.ValidIdNumber = basedata.ValidIdNumber;
                if (!string.IsNullOrEmpty(basedata.FormPath))
                {
                    originalEntity.FormPath = basedata.FormPath;
                }

                _baseDataService.Update(originalEntity);
                _unitOfWork.SaveChanges();

                acResponse.Code = 5;
                acResponse.Message = "Base Data was successfully Updated";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                acResponse.Code = -1;
                acResponse.Message = "An unknown error was encountered. Please try again.";
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpGet]
        public ActionResult SyncEnrollmentRecordFromDb(string enrollmentId)
        {
            try
            {
                var enrollmentRecord = new EnrollmentRecord();
                if (_baseDataService == null) return Json(-1, JsonRequestBehavior.AllowGet);
                var baseDatas = _baseDataService.Query(x => x.EnrollmentId == enrollmentId).Select().ToList();
                if (baseDatas.Any())
                {
                    var baseData = baseDatas[0];
                    enrollmentRecord.BaseData = baseData;
                    enrollmentRecord.ProjectCode = baseData.ProjectCode;
                    enrollmentRecord.EnrollmentId = baseData.EnrollmentId;

                    enrollmentRecord.CustomDatas = _customDataService.Query(x => x.EnrollmentId == baseData.EnrollmentId).Select().ToList();
                    var photographs = _photographService.Query(x => x.EnrollmentId == enrollmentId).Select().ToList();
                    if (photographs.Any())
                    {
                        enrollmentRecord.Photograph = photographs[0];
                    }
                    var signatures = _signatureService.Query(x => x.EnrollmentId == baseData.EnrollmentId).Select().ToList();
                    if (signatures.Any())
                    {
                        enrollmentRecord.Signature = signatures[0];
                    }
                    var fingerprintImages = _fingerprintImageService.Query(x => x.EnrollmentId == baseData.EnrollmentId).Select().ToList();
                    if (fingerprintImages.Any())
                    {
                        enrollmentRecord.FingerprintImages = fingerprintImages;
                    }
                    var fingerprintTemplates = _fingerprintTemplateService.Query(x => x.EnrollmentId == baseData.EnrollmentId).Select().ToList();
                    if (fingerprintTemplates.Any())
                    {
                        enrollmentRecord.FingerprintTemplate = fingerprintTemplates[0];
                    }

                    using (var client = new WebClient())
                    {
                        var json = JsonConvert.SerializeObject(enrollmentRecord);
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        var result = client.UploadString("http://localhost:9495/api/ApiHub/EnrolleeData", "POST", json);
                    }
                    return Json(5, JsonRequestBehavior.AllowGet);
                }

                return Json(-2, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }
        
        public async Task<ActionResult> SyncEnrollmentDataForms(string destinationUrl, string projectCode)
        {
            try
            {
                var folderPath = "~/UserRecords/" + projectCode;

                var mainPath = HttpContext.Server.MapPath(folderPath);
                
                var subfolders = Directory.GetDirectories(mainPath);
                if (!subfolders.Any())
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }
                
                var message = new HttpRequestMessage();
                var content = new MultipartFormDataContent();
                subfolders.ForEach(d =>
                {
                    var myFiles = Directory.GetFiles(d);
                    if (!myFiles.Any())
                    {
                        return;
                    }
                    var dirName = new DirectoryInfo(d).Name;
                    if (string.IsNullOrEmpty(dirName))
                    {
                        return;
                    }
                    var files = myFiles.Where(s =>
                    {
                        var fileName = Path.GetFileName(s);
                        return fileName != null && fileName.Contains(dirName.Replace("-", "") + "_dataForm");
                    }).ToList();

                    if (!files.Any())
                    {
                        return;
                    }
                    
                    var filestream = new FileStream(files[0], FileMode.Open);
                    var fName = Path.GetFileName(files[0]);
                    content.Add(new StreamContent(filestream), dirName, fName);
                });

                if (!content.Any())
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }
                
                message.Method = HttpMethod.Post;
                message.Content = content;
                message.RequestUri = new Uri(destinationUrl + "?projectCode=" + projectCode);
                var client = new HttpClient();
                string ty = null;
                await client.SendAsync(message).ContinueWith(task =>
                {
                    if (task.Result.IsSuccessStatusCode)
                    {
                        //do something with response
                        ty = task.Result.Content.ReadAsStringAsync().Result;
                        return Json(ty, JsonRequestBehavior.AllowGet);
                    }
                    return Json(-1, JsonRequestBehavior.AllowGet);
                });

                return Json(ty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> SyncEnrollmentDataForm(string destinationUrl, string projectCode, string enrolleeId)
        {
            try
            {
                var folderPath = "~/UserRecords/" + projectCode + "/" + enrolleeId;

                var mainPath = HttpContext.Server.MapPath(folderPath);
                
                var message = new HttpRequestMessage();
                var content = new MultipartFormDataContent();
                var myFiles = Directory.GetFiles(mainPath).Where(s =>
                {
                    var fileName = Path.GetFileName(s);
                    return fileName != null && fileName.Contains(enrolleeId.Replace("-", "") + "_dataForm");
                }).ToList();
                 
                if (!myFiles.Any())
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }

                var filestream = new FileStream(myFiles[0], FileMode.Open);
                var fName = Path.GetFileName(myFiles[0]);
                content.Add(new StreamContent(filestream), enrolleeId, fName);

                if (!content.Any())
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }

                message.Method = HttpMethod.Post;
                message.Content = content;
                message.RequestUri = new Uri(destinationUrl + "?projectCode=" + projectCode + "&enrolleeId=" + enrolleeId);
                var client = new HttpClient();
                Task<string> ty = null;
                await client.SendAsync(message).ContinueWith(task =>
                {
                    if (task.Result.IsSuccessStatusCode)
                    {
                        //do something with response
                        ty = task.Result.Content.ReadAsStringAsync();
                        return Json(ty, JsonRequestBehavior.AllowGet);
                    }
                    return Json(-1, JsonRequestBehavior.AllowGet);
                });

                return Json(ty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetBaseData(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Json(new BaseData(), JsonRequestBehavior.AllowGet);
                }

                var querries = _baseDataService.Query(b => b.EnrollmentId == id).Select().ToList();
                if (!querries.Any())
                {
                    return Json(new BaseData(), JsonRequestBehavior.AllowGet);
                }

                var query = querries[0];
                
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new BaseData(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBaseDataByEmail()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var registeredUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Where(e => e.Id == userId).Include("UserInfo").ToList();
                if (!registeredUsers.Any())
                {
                    return Json(new BaseData(), JsonRequestBehavior.AllowGet);
                }

                var user = registeredUsers[0];
                var querries = _baseDataService.Query(b => b.Email == user.Email).Select().ToList();

                if (!querries.Any())
                {
                    var names = user.UserInfo.FullName.Split(' ');
                    var baseData = new BaseData
                    {
                        Email = user.Email,
                        MobileNumber = user.PhoneNumber,
                        Gender = user.UserInfo.Sex,
                        Firstname = names[0],
                        Surname = names[1]
                    };

                    return Json(baseData, JsonRequestBehavior.AllowGet);
                }

                var query = querries[0];

                return Json(query, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new BaseData(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteBaseData(int baseDataTableId)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (baseDataTableId < 1)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid selection.";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                _baseDataService.Delete(baseDataTableId);
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

        public ActionResult GetProject()
        {
            try
            {
                var currentProject = GetProjectInSession();
                if (currentProject == null)
                {
                    return Json(new Project(), JsonRequestBehavior.AllowGet);
                }
                var userId = User.Identity.GetUserId();
                var userRoles = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().GetRoles(userId);
                if (!userRoles.Any())
                {
                    return Json(new Project(), JsonRequestBehavior.AllowGet);
                }
                var querries = _projectService.Query(p => p.ProjectCode == currentProject.ProjectCode).Select().ToList();
                if (!querries.Any())
                {
                    return Json(new Project(), JsonRequestBehavior.AllowGet);
                }

                var query = querries[0];

                var project = new ProjectViewModel
                {
                    TableId = query.TableId,
                    ProjectName = query.ProjectName,
                    ProjectDescription = query.ProjectDescription,
                    ProjectCode = query.ProjectCode,
                    DateCreated = query.DateCreated,
                    LicenceCode = query.LicenceCode,
                    UserRole = userRoles[0],
                    ActivationCode = query.ActivationCode,
                    OnlineMode = query.OnlineMode,
                    LicenseExpiryDate = query.LicenseExpiryDate
                };

                return Json(project, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new Project(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UploadFile(string enrollmentId, string projectCode)
        {
            var gVal = new ActivityResponse();
            try
            {
                if (Request.Files != null)
                {
                    var file = Request.Files[0];

                    if (file == null || file.ContentLength < 1)
                    {
                        gVal.Code = -1;
                        gVal.Message = "The provided Site License File could not be correctly accessed. Please provide all required fields and try again later";
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }

                    if (file.ContentLength > 2096000)
                    {
                        gVal.Code = -1;
                        gVal.Message = "The File size should not be larger than 2MB.";
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }

                    var basedataList = _baseDataService.Query(d => d.EnrollmentId == enrollmentId).Select().ToList();
                    if (!basedataList.Any())
                    {
                        gVal.Code = -1;
                        gVal.Message = "File processing failed. Please trya again later.";
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }

                    var folderPath = "~/UserRecords/" + projectCode + "/" + enrollmentId;

                    var mainPath = HostingEnvironment.MapPath(folderPath);
                    
                    if (!Directory.Exists(mainPath))
                    {
                        Directory.CreateDirectory(mainPath);
                        var dInfo = new DirectoryInfo(mainPath);
                        var dSecurity = dInfo.GetAccessControl();
                        dSecurity.AddAccessRule(
                            new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                                FileSystemRights.FullControl,
                                InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                                PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                        dInfo.SetAccessControl(dSecurity);
                    }
                    
                    //var fileName = file.FileName;

                    var path = Path.Combine(mainPath, enrollmentId.Replace("-", "") + "_dataForm" + Path.GetExtension(file.FileName));

                    if (System.IO.File.Exists(path))
                    {
                        DeleteFile(path);
                    }

                    file.SaveAs(path);
                    
                    var dataToUpdate = basedataList[0];
                    var virtualPath = GenericHelpers.MapPath(path);
                    dataToUpdate.FormPath = virtualPath;
                    
                    _baseDataService.Update(dataToUpdate);
                    _unitOfWork.SaveChanges();

                    gVal.Code = 5;
                    gVal.Message = "File was successfully processed";
                    gVal.FilePath = virtualPath;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                gVal.Code = -1;
                gVal.Message = "Invalid file!";
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                gVal.Code = -1;
                gVal.Message = "File information could not be processed";
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GenerateQrCode(string enrolmentId)
        {
            var gVal = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(enrolmentId))
                {
                    gVal.Code = -1;
                    gVal.Message = "You cannot print your tag at this time. Please ensure you have ";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(enrolmentId, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);
                var qrCodeImage = qrCode.GetGraphic(20);
                var arr = ImageToByte(qrCodeImage);

                var mainPath = Server.MapPath("~/TempQrCodeFile");

                if (!Directory.Exists(mainPath))
                {
                    Directory.CreateDirectory(mainPath);
                    var dInfo = new DirectoryInfo(mainPath);
                    var dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    dInfo.SetAccessControl(dSecurity);
                }

                var newPathv = Path.Combine(mainPath, enrolmentId + ".png");
                if (System.IO.File.Exists(newPathv))
                {
                    System.IO.File.Delete(newPathv);
                }
                var fileStream = new FileStream(newPathv, FileMode.Create, FileAccess.ReadWrite);
                fileStream.Write(arr, 0, arr.Length);
                fileStream.Close();
                return Json(GenericHelpers.MapPath(newPathv).Replace("~",""), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                gVal.Code = -1;
                gVal.Message = "Process failed. Please contact the support team for help.";
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }
        }

        public static byte[] ImageToByte(Image img)
        {
            var converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        private static string GenerateUniqueName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff" + "_" + Guid.NewGuid());
        }

        public ActionResult DeleteFilex(string path)
        {
            try
            {
                DeleteFile(path);
                return Json(5, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }
        private bool SaveToFolder(HttpPostedFileBase file, ref string path, string folderPath, string formerFilePath)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    var fileName = GenerateUniqueName() + fileExtension;
                    var newPathv = Path.Combine(folderPath, fileName);
                    file.SaveAs(newPathv);
                    if (!string.IsNullOrEmpty(formerFilePath))
                    {
                        if (!DeleteFile(formerFilePath))
                        {
                            DeleteFile(newPathv);
                            return false;
                        }
                    }
                    path = newPathv;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private bool DeleteFile(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    return false;
                }

                if (!filePath.StartsWith("~"))
                {
                    filePath = "~" + filePath;
                }

                System.IO.File.Delete(Server.MapPath(filePath));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DownloadContentFromFolder2(string path)
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
            catch (Exception ex)
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

        //______________________ WinForm Helper ___________________________

        [DllImport("User32.dll", SetLastError = true)]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
    }
}
