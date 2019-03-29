using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Crims.API.Helpers;
using Crims.API.Models;
using Crims.Data.Contracts;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Crims.Data.Models;
using Repository.Pattern.UnitOfWork;
using WebGrease.Css.Extensions;

namespace Crims.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/ApiHub")]
    public class ApiHubController : ApiController
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
       
        public ApiHubController(IProjectService projectService)
        {
            _projectService = projectService;
            //_photographService = photographService;
            //_fingerprintTemplateService = fingerprintTemplateService;
            //_fingerprintImageService = fingerprintImageService;
            //_signatureService = signatureService;
            //_customDataService = customDataService;
            //_baseDataService = baseDataService;
            //_unitOfWork = unitOfWork;
        }
        //, IBaseDataService baseDataService, IPhotographService photographService, ICustomDataService customDataService, IFingerprintImageService fingerprintImageService, IFingerprintTemplateService fingerprintTemplateService, ISignatureService signatureService, IUnitOfWorkAsync unitOfWork
        public ApiHubController()
        {
        }
        /// <summary>
        /// Saves Enrollee Data.
        /// </summary>
        /// <returns>ActivityResponse</returns>
        [HttpPost]
        [Route("EnrolleeData")]
        public async Task<ActivityResponse> EnrolleeData()
        {
            var gVal = new ActivityResponse();
            try
            {
                var enrollmentRecord = await Request.Content.ReadAsAsync<EnrollmentRecord>();
                if (enrollmentRecord == null)
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected payload is empty.";
                    return gVal;
                }
                
                if (string.IsNullOrEmpty(enrollmentRecord.ProjectCode))
                {
                    gVal.Code = -1;
                    gVal.Message = "Project Code is empty.";
                    return gVal;
                }
                var projects = _projectService.Query(o => o.ProjectCode == enrollmentRecord.ProjectCode).Select().ToList();
                if (!projects.Any())
                {
                    gVal.Code = -1;
                    gVal.Message = "This Project does not exist.";
                    return gVal;
                }

                var project = projects[0];
                if (project.LicenseExpiryDate >= DateTime.Now)
                {
                    gVal.Code = -1;
                    gVal.Message = "This Project License has expired. Please contact our support.";
                    return gVal;
                }

                if (string.IsNullOrEmpty(enrollmentRecord.EnrollmentId))
                {
                    gVal.Code = -1;
                    gVal.Message = "Enrollment Id is empty.";
                    return gVal;
                }

                var baseData = enrollmentRecord.BaseData;

                if (!string.IsNullOrEmpty(baseData.Firstname) && !string.IsNullOrEmpty(baseData.Surname))
                {
                    //insert/update
                    var baseDatas = _baseDataService.Query(p => p.EnrollmentId == enrollmentRecord.EnrollmentId).Select().ToList();
                    if (!baseDatas.Any())
                    {
                        _baseDataService.Insert(baseData);
                        _unitOfWork.SaveChanges();
                    }
                    else
                    {
                        var baseD = baseDatas[0];
                        baseD.DateLastUpdated = baseData.DateLastUpdated;
                        baseD.Surname = baseData.Surname;
                        baseD.Firstname = baseData.Firstname;
                        baseD.MiddleName = baseData.MiddleName;
                        baseD.Title = baseData.Title;
                        baseD.MobileNumber = baseData.MobileNumber;
                        baseD.Gender = baseData.Gender;
                        baseD.ProjectPrimaryCode = baseData.ProjectPrimaryCode;
                        baseD.DOB = baseData.DOB;
                        baseD.ValidIdNumber = baseData.ValidIdNumber;
                        baseD.LastUpdatedby = baseData.LastUpdatedby;
                        baseD.DateLastUpdated = baseData.DateLastUpdated;
                        _baseDataService.Update(baseD);
                        _unitOfWork.SaveChanges();
                    }
                }

                var customDatas = enrollmentRecord.CustomDatas;

                if (customDatas.Any())
                {
                    customDatas.ForEach(d =>
                    {
                        var customDataLists = _customDataService.Query(p => p.CustomDataId == d.CustomDataId).Select().ToList();
                        if (!customDataLists.Any())
                        {
                            _customDataService.Insert(d);
                            _unitOfWork.SaveChanges();
                        }
                        else
                        {
                            var customData = customDataLists[0];
                            customData.DateLastUpdated = d.DateLastUpdated;
                            customData.CustomFieldId = d.CustomFieldId;
                            customData.CrimsCustomData = d.CrimsCustomData;
                            customData.CustomListId = d.CustomListId;
                            customData.DateLastUpdated = d.DateLastUpdated;
                            _customDataService.Update(customData);
                            _unitOfWork.SaveChanges();
                        }
                    });
                }

                var fingerprintImages = enrollmentRecord.FingerprintImages;

                if (fingerprintImages.Any())
                {
                    fingerprintImages.ForEach(d =>
                    {
                        var fingerprintImageList = _fingerprintImageService.Query(p => p.EnrollmentId == enrollmentRecord.EnrollmentId).Select().ToList();
                        if (!fingerprintImageList.Any())
                        {
                            _fingerprintImageService.Insert(d);
                            _unitOfWork.SaveChanges();
                        }
                        else
                        {
                            var fingerprintImage = fingerprintImageList[0];
                            fingerprintImage.DateLastUpdated = d.DateLastUpdated;
                            fingerprintImage.FingerIndexId = d.FingerIndexId;
                            fingerprintImage.FingerPrintImage = d.FingerPrintImage;
                            _fingerprintImageService.Update(fingerprintImage);
                            _unitOfWork.SaveChanges();
                        }
                    });
                }

                var fingerprintTemplate = enrollmentRecord.FingerprintTemplate;

                if (fingerprintTemplate.Template != null && fingerprintTemplate.Template.Length > 0)
                {
                    var fingerprintTemplateList = _fingerprintTemplateService.Query(p => p.EnrollmentId == enrollmentRecord.EnrollmentId).Select().ToList();
                    if (!fingerprintTemplateList.Any())
                    {
                        _fingerprintTemplateService.Insert(fingerprintTemplate);
                        _unitOfWork.SaveChanges();
                    }
                    else
                    {
                        var fingerprintTemp = fingerprintTemplateList[0];
                        fingerprintTemp.DateLastUpdated = fingerprintTemplate.DateLastUpdated;
                        fingerprintTemp.Template = fingerprintTemplate.Template;
                        fingerprintTemp.UniquenessStatus = fingerprintTemplate.UniquenessStatus;
                        _fingerprintTemplateService.Update(fingerprintTemp);
                        _unitOfWork.SaveChanges();
                    }
                }

                var signature = enrollmentRecord.Signature;

                if (signature.SignatureImage != null && signature.SignatureImage.Length > 0)
                {
                    var signatureList = _signatureService.Query(p => p.EnrollmentId == enrollmentRecord.EnrollmentId).Select().ToList();
                    if (!signatureList.Any())
                    {
                        _signatureService.Insert(signature);
                        _unitOfWork.SaveChanges();
                    }
                    else
                    {
                        var signatureTemp = signatureList[0];
                        signatureTemp.DateLastUpdated = signature.DateLastUpdated;
                        signatureTemp.SignatureImage = signature.SignatureImage;
                        _signatureService.Update(signatureTemp);
                        _unitOfWork.SaveChanges();
                    }
                }
                
                var photograph = enrollmentRecord.Photograph;

                if (photograph.PhotographImage.Length > 0 && photograph.PhotographTemplate.Length > 0)
                {
                    //insert/update
                    var photographs = _photographService.Query(p => p.EnrollmentId == enrollmentRecord.EnrollmentId).Select().ToList();
                    if (!photographs.Any())
                    {
                        _photographService.Insert(photograph);
                        _unitOfWork.SaveChanges();
                    }
                    else
                    {
                        var photo = photographs[0];
                        photo.DateLastUpdated = photograph.DateLastUpdated;
                        photo.PhotographImage = photograph.PhotographImage;
                        photo.PhotographTemplate = photograph.PhotographTemplate;
                        _photographService.Update(photograph);
                        _unitOfWork.SaveChanges();
                    }
                }
                
                _photographService.InsertOrUpdateGraph(photograph);
                _unitOfWork.SaveChanges();
                gVal.Code = 5;
                gVal.Message = "Action was successfully completed.";
                return gVal;

            }
            catch (Exception ex)
            {
                gVal.Code = -1;
                gVal.Message = "Internal Server error: Requested process could not be processed. Please try again.";
                return gVal;
            }
        }

        /// <summary>
        /// Saves Enrollee Data.
        /// </summary>
        /// <returns>ActivityResponse</returns>
        [HttpPost]
        [Route("EnrolleeDatas")]
        public async Task<ActivityResponse> EnrolleeDatas()
        {
            var gVal = new ActivityResponse();
            try
            {
                var enrollmentRecords = await Request.Content.ReadAsAsync<List<EnrollmentRecord>>();
                if (enrollmentRecords == null || !enrollmentRecords.Any())
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected payload is empty.";
                    return gVal;
                }

                if (string.IsNullOrEmpty(enrollmentRecords[0].ProjectCode))
                {
                    gVal.Code = -1;
                    gVal.Message = "Project Code is empty.";
                    return gVal;
                }
                var projectCode = enrollmentRecords[0].ProjectCode;
                var projects = _projectService.Query(o => o.ProjectCode == projectCode).Select().ToList();
                if (!projects.Any())
                {
                    gVal.Code = -1;
                    gVal.Message = "This Project does not exist.";
                    return gVal;
                }

                var project = projects[0];
                if (project.LicenseExpiryDate >= DateTime.Now)
                {
                    gVal.Code = -1;
                    gVal.Message = "This Project License has expired. Please contact our support.";
                    return gVal;
                }

                enrollmentRecords.ForEach(e =>
                {
                    var enrollmentRecord = e;

                    if (string.IsNullOrEmpty(enrollmentRecord.EnrollmentId))
                    {
                        gVal.FailedCount += 1;
                        return;
                    }

                    var baseData = enrollmentRecord.BaseData;

                    if (!string.IsNullOrEmpty(baseData.Firstname) && !string.IsNullOrEmpty(baseData.Surname))
                    {
                        //insert/update
                        var baseDatas = _baseDataService.Query(p => p.EnrollmentId == enrollmentRecord.EnrollmentId).Select().ToList();
                        if (!baseDatas.Any())
                        {
                            _baseDataService.Insert(baseData);
                            _unitOfWork.SaveChanges();
                        }
                        else
                        {
                            var baseD = baseDatas[0];
                            baseD.DateLastUpdated = baseData.DateLastUpdated;
                            baseD.Surname = baseData.Surname;
                            baseD.Firstname = baseData.Firstname;
                            baseD.MiddleName = baseData.MiddleName;
                            baseD.Title = baseData.Title;
                            baseD.MobileNumber = baseData.MobileNumber;
                            baseD.Gender = baseData.Gender;
                            baseD.ProjectPrimaryCode = baseData.ProjectPrimaryCode;
                            baseD.DOB = baseData.DOB;
                            baseD.ValidIdNumber = baseData.ValidIdNumber;
                            baseD.LastUpdatedby = baseData.LastUpdatedby;
                            baseD.DateLastUpdated = baseData.DateLastUpdated;
                            _baseDataService.Update(baseD);
                            _unitOfWork.SaveChanges();
                        }
                    }

                    var customDatas = enrollmentRecord.CustomDatas;

                    if (customDatas.Any())
                    {
                        customDatas.ForEach(d =>
                        {
                            var customDataLists = _customDataService.Query(p => p.CustomDataId == d.CustomDataId).Select().ToList();
                            if (!customDataLists.Any())
                            {
                                _customDataService.Insert(d);
                                _unitOfWork.SaveChanges();
                            }
                            else
                            {
                                var customData = customDataLists[0];
                                customData.DateLastUpdated = d.DateLastUpdated;
                                customData.CustomFieldId = d.CustomFieldId;
                                customData.CrimsCustomData = d.CrimsCustomData;
                                customData.CustomListId = d.CustomListId;
                                customData.DateLastUpdated = d.DateLastUpdated;
                                _customDataService.Update(customData);
                                _unitOfWork.SaveChanges();
                            }
                        });
                    }

                    var fingerprintImages = enrollmentRecord.FingerprintImages;

                    if (fingerprintImages.Any())
                    {
                        fingerprintImages.ForEach(d =>
                        {
                            var fingerprintImageList = _fingerprintImageService.Query(p => p.EnrollmentId == enrollmentRecord.EnrollmentId).Select().ToList();
                            if (!fingerprintImageList.Any())
                            {
                                _fingerprintImageService.Insert(d);
                                _unitOfWork.SaveChanges();
                            }
                            else
                            {
                                var fingerprintImage = fingerprintImageList[0];
                                fingerprintImage.DateLastUpdated = d.DateLastUpdated;
                                fingerprintImage.FingerIndexId = d.FingerIndexId;
                                fingerprintImage.FingerPrintImage = d.FingerPrintImage;
                                _fingerprintImageService.Update(fingerprintImage);
                                _unitOfWork.SaveChanges();
                            }
                        });
                    }

                    var fingerprintTemplate = enrollmentRecord.FingerprintTemplate;

                    if (fingerprintTemplate.Template != null && fingerprintTemplate.Template.Length > 0)
                    {
                        var fingerprintTemplateList = _fingerprintTemplateService.Query(p => p.EnrollmentId == enrollmentRecord.EnrollmentId).Select().ToList();
                        if (!fingerprintTemplateList.Any())
                        {
                            _fingerprintTemplateService.Insert(fingerprintTemplate);
                            _unitOfWork.SaveChanges();
                        }
                        else
                        {
                            var fingerprintTemp = fingerprintTemplateList[0];
                            fingerprintTemp.DateLastUpdated = fingerprintTemplate.DateLastUpdated;
                            fingerprintTemp.Template = fingerprintTemplate.Template;
                            fingerprintTemp.UniquenessStatus = fingerprintTemplate.UniquenessStatus;
                            _fingerprintTemplateService.Update(fingerprintTemp);
                            _unitOfWork.SaveChanges();
                        }
                    }

                    var signature = enrollmentRecord.Signature;

                    if (signature.SignatureImage != null && signature.SignatureImage.Length > 0)
                    {
                        var signatureList = _signatureService.Query(p => p.EnrollmentId == enrollmentRecord.EnrollmentId).Select().ToList();
                        if (!signatureList.Any())
                        {
                            _signatureService.Insert(signature);
                            _unitOfWork.SaveChanges();
                        }
                        else
                        {
                            var signatureTemp = signatureList[0];
                            signatureTemp.DateLastUpdated = signature.DateLastUpdated;
                            signatureTemp.SignatureImage = signature.SignatureImage;
                            _signatureService.Update(signatureTemp);
                            _unitOfWork.SaveChanges();
                        }
                    }

                    var photograph = enrollmentRecord.Photograph;

                    if (photograph.PhotographImage.Length > 0 && photograph.PhotographTemplate.Length > 0)
                    {
                        //insert/update
                        var photographs = _photographService.Query(p => p.EnrollmentId == enrollmentRecord.EnrollmentId).Select().ToList();
                        if (!photographs.Any())
                        {
                            _photographService.Insert(photograph);
                            _unitOfWork.SaveChanges();
                        }
                        else
                        {
                            var photo = photographs[0];
                            photo.DateLastUpdated = photograph.DateLastUpdated;
                            photo.PhotographImage = photograph.PhotographImage;
                            photo.PhotographTemplate = photograph.PhotographTemplate;
                            _photographService.Update(photograph);
                            _unitOfWork.SaveChanges();
                        }
                    }

                    _photographService.InsertOrUpdateGraph(photograph);
                    _unitOfWork.SaveChanges();
                });
                
                gVal.Code = 5;
                gVal.Message = "Action was successfully completed.";
                return gVal;

            }
            catch (Exception ex)
            {
                gVal.Code = -1;
                gVal.Message = "Internal Server error: Requested process could not be processed. Please try again.";
                return gVal;
            }
        }

        /// <summary>
        /// Saves Enrollee Bio Data to Folder.
        /// </summary>
        /// <returns>ActivityResponse</returns>
        [HttpPost]
        [Route("EnrolleeRecordToDisk")]
        public async Task<ActivityResponse> EnrolleeRecordToDisk()
        {
            var gVal = new ActivityResponse();
            try
            {
                var enrollmentRecord = await Request.Content.ReadAsAsync<EnrolleeRecord>();
                if (enrollmentRecord == null)
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected payload is empty.";
                    return gVal;
                }

                if (string.IsNullOrEmpty(enrollmentRecord.ProjectCode))
                {
                    gVal.Code = -1;
                    gVal.Message = "Project Code is empty.";
                    return gVal;
                }
                var projects = _projectService.Query(o => o.ProjectCode == enrollmentRecord.ProjectCode).Select().ToList();
                if (!projects.Any())
                {
                    gVal.Code = -1;
                    gVal.Message = "This Project does not exist.";
                    return gVal;
                }

                var project = projects[0];
                if (project.LicenseExpiryDate >= DateTime.Now)
                {
                    gVal.Code = -1;
                    gVal.Message = "This Project License has expired. Please contact our support.";
                    return gVal;
                }

                if (string.IsNullOrEmpty(enrollmentRecord.EnrollmentId))
                {
                    gVal.Code = -1;
                    gVal.Message = "Enrollment Id is empty.";
                    return gVal;
                }

                var baseData = enrollmentRecord.BaseData;

                if (!string.IsNullOrEmpty(baseData.Firstname) && !string.IsNullOrEmpty(baseData.Surname))
                {
                    //insert/update
                    var baseDatas = _baseDataService.Query(p => p.EnrollmentId == enrollmentRecord.EnrollmentId).Select().ToList();
                    if (!baseDatas.Any())
                    {
                        _baseDataService.Insert(baseData);
                        _unitOfWork.SaveChanges();
                    }
                    else
                    {
                        var baseD = baseDatas[0];
                        baseD.DateLastUpdated = baseData.DateLastUpdated;
                        baseD.Surname = baseData.Surname;
                        baseD.Firstname = baseData.Firstname;
                        baseD.MiddleName = baseData.MiddleName;
                        baseD.Title = baseData.Title;
                        baseD.MobileNumber = baseData.MobileNumber;
                        baseD.Gender = baseData.Gender;
                        baseD.ProjectPrimaryCode = baseData.ProjectPrimaryCode;
                        baseD.DOB = baseData.DOB;
                        baseD.ValidIdNumber = baseData.ValidIdNumber;
                        baseD.LastUpdatedby = baseData.LastUpdatedby;
                        baseD.DateLastUpdated = baseData.DateLastUpdated;
                        _baseDataService.Update(baseD);
                        _unitOfWork.SaveChanges();
                    }
                }

                var customDatas = enrollmentRecord.CustomDatas;

                if (customDatas.Any())
                {
                    customDatas.ForEach(d =>
                    {
                        var customDataLists = _customDataService.Query(p => p.CustomDataId == d.CustomDataId).Select().ToList();
                        if (!customDataLists.Any())
                        {
                            _customDataService.Insert(d);
                            _unitOfWork.SaveChanges();
                        }
                        else
                        {
                            var customData = customDataLists[0];
                            customData.DateLastUpdated = d.DateLastUpdated;
                            customData.CustomFieldId = d.CustomFieldId;
                            customData.CrimsCustomData = d.CrimsCustomData;
                            customData.CustomListId = d.CustomListId;
                            customData.DateLastUpdated = d.DateLastUpdated;
                            _customDataService.Update(customData);
                            _unitOfWork.SaveChanges();
                        }
                    });
                }
                
                gVal.Code = 5;
                gVal.Message = "Action was successfully completed.";
                return gVal;

            }
            catch (Exception ex)
            {
                gVal.Code = -1;
                gVal.Message = "Internal Server error: Requested process could not be processed. Please try again.";
                return gVal;
            }
        }

        [Route("Approvals")]
        public List<Approval> Approvals(int numberOfRecords, int pageNumber, string userId)
        {
            try
            {
                if (numberOfRecords < 1)
                {
                    return new List<Approval>();
                }

                var approvals = _approvalService.Query().Select().OrderBy(s => s.DateProcessed).Skip(pageNumber * numberOfRecords).Take(numberOfRecords).ToList();
                if (approvals.Any())
                {
                    return new List<Approval>();
                }
                
                return approvals;

            }
            catch (Exception ex)
            {
                return new List<Approval>();
            }
        }
        

        [Route("EnrolleeDataForms")]
        public Task<IEnumerable<FileDesc>> EnrolleeDataForms(string projectCode)
        {
            if (string.IsNullOrEmpty(projectCode))
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            var folderPath = HostingEnvironment.MapPath("~");

            if (string.IsNullOrEmpty(folderPath))
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            var mainPath = folderPath.Replace("crimsApi", "crimsWeb") + @"\UserRecords\" + projectCode;
            if (!Directory.Exists(mainPath))
            {
                Directory.CreateDirectory(mainPath);
                var dInfo = new DirectoryInfo(mainPath);
                var dSecurity = dInfo.GetAccessControl();
                dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                dInfo.SetAccessControl(dSecurity);
            }

            var rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, string.Empty);
        
            if (Request.Content.IsMimeMultipartContent())
            {
                var streamProvider = new CustomMultipartFormDataStreamProvider(mainPath);

                var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                    {
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    }

                    var fileInfo = streamProvider.FileData.Select(i =>
                    {
                        var enrolleeId = i.Headers.ContentDisposition.Name;
                        var info = new FileInfo(i.LocalFileName);
                        if (!string.IsNullOrEmpty(enrolleeId))
                        {
                            var filePath = mainPath + @"\" + enrolleeId;
                            if (!Directory.Exists(filePath))
                            {
                                Directory.CreateDirectory(filePath);
                                var dInfo = new DirectoryInfo(filePath);
                                var dSecurity = dInfo.GetAccessControl();
                                dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                                dInfo.SetAccessControl(dSecurity);
                            }

                            var fileId = enrolleeId.Replace("-", "") + "_dataForm";
                            var myFiles = Directory.GetFiles(filePath).Where(s =>
                            {
                                var fileName = Path.GetFileName(s);
                                return fileName != null && fileName.Contains(fileId);
                            }).ToList();

                            if (myFiles.Any())
                            {
                                File.Delete(myFiles[0]);
                            }
                            
                            var sourceFile = mainPath + @"\" + info.Name;
                            var destinationFile = filePath + @"\" + info.Name;
                            new FileInfo(sourceFile).MoveTo(destinationFile);
                            info = new FileInfo(destinationFile);
                            //File.Move(sourceFile, filePath);
                        }
                       
                        return new FileDesc(info.Name, rootUrl + "/" + enrolleeId + "/" + info.Name, info.Length / 1024);
                    });
                    return fileInfo;
                });

                return task;
            }
           
             throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
        }

        [Route("EnrolleeDataForm")]
        public Task<FileDesc> EnrolleeDataForm(string enrollmentId, string projectCode)
        {
            if (string.IsNullOrEmpty(projectCode) || string.IsNullOrEmpty(enrollmentId))
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            var folderPath = HostingEnvironment.MapPath("~");

            if (string.IsNullOrEmpty(folderPath))
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            var mainPath = folderPath.Replace("Crims.API", "crimsWeb") + @"\UserRecordsZ\" + projectCode + @"\" + enrollmentId;
            if (!Directory.Exists(mainPath))
            {
                Directory.CreateDirectory(mainPath);
                var dInfo = new DirectoryInfo(mainPath);
                var dSecurity = dInfo.GetAccessControl();
                dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                dInfo.SetAccessControl(dSecurity);
            }

            var myFiles = Directory.GetFiles(mainPath).Where(s =>
            {
                var fileName = Path.GetFileName(s);
                return fileName != null && fileName.Contains(enrollmentId.Replace("-", "") + "_dataForm");
            }).ToList();

            if (myFiles.Any())
            {
                File.Delete(myFiles[0]);
            }

            var rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, string.Empty);

            if (Request.Content.IsMimeMultipartContent())
            {
                var streamProvider = new CustomMultipartFormDataStreamProvider(mainPath);

                var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith<FileDesc>(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                    {
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    }

                    var fileInfo = streamProvider.FileData[0];
                    var info = new FileInfo(fileInfo.LocalFileName);
                    var fileDesc = new FileDesc(info.Name, rootUrl + "/" + enrollmentId + "/" + info.Name, info.Length / 1024);
                    return fileDesc;
                });

                return task;
            }

            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
        }
        /// <summary>
        /// Saves the photo.
        /// </summary>
        /// <returns>ActivityResponse</returns>
        [HttpPost]
        [Route("SavePhoto")]
        public async Task<ActivityResponse> SavePhoto()
        {
            var gVal = new ActivityResponse();
            try
            {
                var content = await Request.Content.ReadAsFormDataAsync();

                if (content == null)
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected payload is empty.";
                    return gVal;
                }

                var payload = content["payload"];
                if (payload == null)
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected payload is empty.";
                    return gVal;
                }
               
                var payloadBase64Str = Convert.FromBase64String(payload);
                var payloadAsciiStr = Encoding.ASCII.GetString(payloadBase64Str);
                var photograph = JsonConvert.DeserializeObject<Photograph>(payloadAsciiStr);
                if (string.IsNullOrEmpty(photograph?.EnrollmentId))
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected payload is empty.";
                    return gVal;
                }
                if (photograph.PhotographImage.Length < 1 || photograph.PhotographTemplate.Length < 1)
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected Photograph Template or Image is empty.";
                    return gVal;
                }
               
                var photoImage = (HttpPostedFileBase)new HttpFile(photograph.PhotographImage);
                if (photoImage.ContentLength < 1)
                {
                    gVal.Code = -1;
                    gVal.Message = "Internal server error: Photograph Image could not be processed.";
                    return gVal;
                }

                var folderPath = "~/Media/Photos";

                var mainPath = HostingEnvironment.MapPath(folderPath);

                if (!Directory.Exists(mainPath))
                {
                    if (mainPath != null)
                    {
                        Directory.CreateDirectory(mainPath);
                        var dInfo = new DirectoryInfo(mainPath);
                        var dSecurity = dInfo.GetAccessControl();
                        dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                        dInfo.SetAccessControl(dSecurity);
                    }
                    else
                    {
                        gVal.Code = -1;
                        gVal.Message = "Internal Server error: The requested operation could not be completed due to an internal server error.";
                        return gVal;
                    }
                }
                
                var fileName = photoImage.FileName;

                var path = Path.Combine(mainPath, fileName);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                photoImage.SaveAs(path);
                photograph.PhotographImagePath = path;
                _photographService.InsertOrUpdateGraph(photograph);
                _unitOfWork.SaveChanges();
                gVal.Code = 5;
                gVal.Message = "Photograph was successfully processed.";
                return gVal;

            }
            catch (Exception ex)
            {
                gVal.Code = -1;
                gVal.Message = "Internal Server error: Photograph could not be processed. Please try again.";
                return gVal;
            }
        }

        /// <summary>
        /// Saves the fingerprint image.
        /// </summary>
        /// <returns>ActivityResponse</returns>
        [HttpPost]
        [Route("SaveFingerprintImage")]
        public async Task<ActivityResponse> SaveFingerprintImage()
        {
            var gVal = new ActivityResponse();
            try
            {
                var content = await Request.Content.ReadAsFormDataAsync();

                if (content == null)
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected payload is empty.";
                    return gVal;
                }

                var payload = content["payload"];
                if (payload == null)
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected payload is empty.";
                    return gVal;
                }

                var payloadBase64Str = Convert.FromBase64String(payload);
                var payloadAsciiStr = Encoding.ASCII.GetString(payloadBase64Str);
                var fingerprint = JsonConvert.DeserializeObject<FingerprintImage>(payloadAsciiStr);
                if (string.IsNullOrEmpty(fingerprint?.EnrollmentId))
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected payload is empty.";
                    return gVal;
                }
                if (fingerprint.FingerPrintImage.Length < 1 || fingerprint.FingerPrintImage.Length < 1)
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected Fingerprint Image is empty.";
                    return gVal;
                }

                var fingerprintImage = (HttpPostedFileBase)new HttpFile(fingerprint.FingerPrintImage);
                if (fingerprintImage.ContentLength < 1)
                {
                    gVal.Code = -1;
                    gVal.Message = "Internal server error: Fingerprint Image could not be processed.";
                    return gVal;
                }

                var folderPath = "~/Media/FingerPrintImage/" + fingerprint.EnrollmentId;

                var mainPath = HostingEnvironment.MapPath(folderPath);

                if (!Directory.Exists(mainPath))
                {
                    if (mainPath != null)
                    {
                        Directory.CreateDirectory(mainPath);
                        var dInfo = new DirectoryInfo(mainPath);
                        var dSecurity = dInfo.GetAccessControl();
                        dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                        dInfo.SetAccessControl(dSecurity);
                    }
                    else
                    {
                        gVal.Code = -1;
                        gVal.Message = "Internal Server error: The requested operation could not be completed due to an internal server error.";
                        return gVal;
                    }
                }

                var fileName = fingerprintImage.FileName;

                var path = Path.Combine(mainPath, fileName);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                fingerprintImage.SaveAs(path);
                fingerprint.FilePath = path;
                _fingerprintImageService.InsertOrUpdateGraph(fingerprint);
                _unitOfWork.SaveChanges();
                gVal.Code = 5;
                gVal.Message = "Fingerprint was successfully processed.";
                return gVal;

            }
            catch (Exception ex)
            {
                gVal.Code = -1;
                gVal.Message = "Internal Server error: Fingerprint could not be processed. Please try again.";
                return gVal;
            }
        }

        /// <summary>
        /// Saves the finger print templates.
        /// </summary>
        /// <returns>ActivityResponse</returns>
        [HttpPost]
        [Route("SaveFingerPrintTemplates")]
        public async Task<ActivityResponse> SaveFingerPrintTemplates()
        {
            var gVal = new ActivityResponse();
            try
            {
                var content = await Request.Content.ReadAsFormDataAsync();

                if (content == null)
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected payload is empty.";
                    return gVal;
                }
                var col = new NameValueCollection
                {
                    {"ddd", "dd"},
                    {"", "" }
                };
                
                var payload = content["payload"];
                if (payload == null)
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected payload is empty.";
                    return gVal;
                }

                var payloadBase64Str = Convert.FromBase64String(payload);
                var payloadAsciiStr = Encoding.ASCII.GetString(payloadBase64Str);
                var fingerprintTemplate = JsonConvert.DeserializeObject<FingerprintTemplate>(payloadAsciiStr);
                if (string.IsNullOrEmpty(fingerprintTemplate?.EnrollmentId))
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected payload is empty.";
                    return gVal;
                }
                if (fingerprintTemplate.Template.Length < 1)
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected Fingerprint Template is empty.";
                    return gVal;
                }

                var fingerprintImage = (HttpPostedFileBase)new HttpFile(fingerprintTemplate.Template);
                if (fingerprintImage.ContentLength < 1)
                {
                    gVal.Code = -1;
                    gVal.Message = "Internal server error: Fingerprint Template could not be processed.";
                    return gVal;
                }

                var folderPath = "~/Media/FingerPrintTemplate/" + fingerprintTemplate.EnrollmentId;

                var mainPath = HostingEnvironment.MapPath(folderPath);

                if (!Directory.Exists(mainPath))
                {
                    if (mainPath != null)
                    {
                        Directory.CreateDirectory(mainPath);
                        var dInfo = new DirectoryInfo(mainPath);
                        var dSecurity = dInfo.GetAccessControl();
                        dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                        dInfo.SetAccessControl(dSecurity);
                    }
                    else
                    {
                        gVal.Code = -1;
                        gVal.Message = "Internal Server error: The requested operation could not be completed due to an internal server error.";
                        return gVal;
                    }
                }

                var fileName = fingerprintImage.FileName;

                var path = Path.Combine(mainPath, fileName);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                fingerprintImage.SaveAs(path);
                fingerprintTemplate.FilePath = path;
                _fingerprintTemplateService.InsertOrUpdateGraph(fingerprintTemplate);
                _unitOfWork.SaveChanges();
                gVal.Code = 5;
                gVal.Message = "Fingerprint was successfully processed.";
                return gVal;

            }
            catch (Exception ex)
            {
                gVal.Code = -1;
                gVal.Message = "Internal Server error: Fingerprint could not be processed. Please try again.";
                return gVal;
            }
        }

        /// <summary>
        /// Saves the signature.
        /// </summary>
        /// <returns>ActivityResponse</returns>
        [HttpPost]
        [Route("SaveSignature")]
        public async Task<ActivityResponse> SaveSignature()
        {
            var gVal = new ActivityResponse();
            try
            {
                var content = await Request.Content.ReadAsFormDataAsync();

                if (content == null)
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected payload is empty.";
                    return gVal;
                }

                var payload = content["payload"];
                if (payload == null)
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected payload is empty.";
                    return gVal;
                }

                var payloadBase64Str = Convert.FromBase64String(payload);
                var payloadAsciiStr = Encoding.ASCII.GetString(payloadBase64Str);
                var signature = JsonConvert.DeserializeObject<Signature>(payloadAsciiStr);
                if (string.IsNullOrEmpty(signature?.EnrollmentId))
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected payload is empty.";
                    return gVal;
                }
                if (signature.SignatureImage.Length < 1)
                {
                    gVal.Code = -1;
                    gVal.Message = "Invalid Operation: The expected Signature Image is empty.";
                    return gVal;
                }

                var fingerprintImage = (HttpPostedFileBase)new HttpFile(signature.SignatureImage);
                if (fingerprintImage.ContentLength < 1)
                {
                    gVal.Code = -1;
                    gVal.Message = "Internal server error: Signature Image could not be processed.";
                    return gVal;
                }

                var folderPath = "~/Media/SignatureImage";

                var mainPath = HostingEnvironment.MapPath(folderPath);

                if (!Directory.Exists(mainPath))
                {
                    if (mainPath != null)
                    {
                        Directory.CreateDirectory(mainPath);
                        var dInfo = new DirectoryInfo(mainPath);
                        var dSecurity = dInfo.GetAccessControl();
                        dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                        dInfo.SetAccessControl(dSecurity);
                    }
                    else
                    {
                        gVal.Code = -1;
                        gVal.Message = "Internal Server error: The requested operation could not be completed due to an internal server error.";
                        return gVal;
                    }
                }

                var fileName = fingerprintImage.FileName;

                var path = Path.Combine(mainPath, fileName);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                fingerprintImage.SaveAs(path);
                signature.FilePath = path;
                _signatureService.InsertOrUpdateGraph(signature);
                _unitOfWork.SaveChanges();
                gVal.Code = 5;
                gVal.Message = "Signature was successfully processed.";
                return gVal;

            }
            catch (Exception ex)
            {
                gVal.Code = -1;
                gVal.Message = "Internal Server error: Fingerprint Template could not be processed. Please try again.";
                return gVal;
            }
        }

        /// <summary>
        /// Retrieves a Signature by EnrollmentId.
        /// </summary>
        /// <param name="eid">The eid.</param>
        /// <returns>ActivityResponse</returns>
        [Route("Signature")]
        public ApiResponse Signature(string eid)
        {
            var response = new ApiResponse();
            try
            {
                if (string.IsNullOrEmpty(eid))
                {
                    response.Code = -1;
                    response.Message = "Invalid Query: The expected parametr 'eid' is empty.";
                    response.Response = new Signature();
                    return response;
                }
                
                var signatures = _signatureService.Query(s => s.EnrollmentId == eid).Select().ToList();
                if (signatures.Any())
                {
                    response.Code = -1;
                    response.Message = "No record found.";
                    response.Response = new Signature();
                    return response;
                }

                var signatureStr = JsonConvert.SerializeObject(signatures[0]);
                var payloadAsciiStr = Encoding.ASCII.GetBytes(signatureStr);
                var payloadBase64Str = Convert.ToBase64String(payloadAsciiStr);

                response.Code = 5;
                response.Response = payloadBase64Str;
                response.Message = "Query was successfully completed.";
                return response;

            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = "Process failed due to an internal server error. Please try again later.";
                return response;
            }
        }
        
        /// <summary>
        /// Retrieves Signatureses by the specified number of records.
        /// </summary>
        /// <param name="numberOfRecords">The number of records to retrieve.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ActivityResponse</returns>
        [Route("Signatures")]
        public ApiResponse Signatures(int numberOfRecords, int pageNumber)
        {
            var response = new ApiResponse();
            try
            {
                if (numberOfRecords < 1)
                {
                    response.Code = -1;
                    response.Message = "Invalid Query: The mandatory parametr 'numberOfRecords' is not provided.";
                    response.Response = new List<Signature>();
                    return response;
                }

                var signatures = _signatureService.Query().Select().OrderBy(s => s.TableId).Skip(pageNumber * numberOfRecords).Take(numberOfRecords).ToList();
                if (signatures.Any())
                {
                    response.Code = -1;
                    response.Message = "No record found.";
                    response.Response = new List<Signature>();
                    return response;
                }
                var signatureStr = JsonConvert.SerializeObject(signatures);
                var payloadAsciiStr = Encoding.ASCII.GetBytes(signatureStr);
                var payloadBase64Str = Convert.ToBase64String(payloadAsciiStr);

                response.Code = 5;
                response.Response = payloadBase64Str;
                response.Message = "Query was successfully completed.";
                return response;

            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = "Process failed due to an internal server error. Please try again later.";
                return response;
            }
        }

        [Route("FingerPrintImage")]
        public ApiResponse FingerPrintImage(string eid)
        {
            var response = new ApiResponse();
            try
            {
                if (string.IsNullOrEmpty(eid))
                {
                    response.Code = -1;
                    response.Message = "Invalid Query: The expected parametr 'eid' is empty.";
                    response.Response = new FingerprintImage();
                    return response;
                }

                var fingerprintImages = _fingerprintImageService.Query(s => s.EnrollmentId == eid).Select().ToList();
                if (fingerprintImages.Any())
                {
                    response.Code = -1;
                    response.Message = "No record found.";
                    response.Response = new FingerprintImage();
                    return response;
                }
                var fingerprintImageStr = JsonConvert.SerializeObject(fingerprintImages[0]);
                var payloadAsciiStr = Encoding.ASCII.GetBytes(fingerprintImageStr);
                var payloadBase64Str = Convert.ToBase64String(payloadAsciiStr);

                response.Code = 5;
                response.Response = payloadBase64Str;
                response.Message = "Query was successfully completed.";
                return response;

            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = "Process failed due to an internal server error. Please try again later.";
                return response;
            }
        }

        [Route("FingerPrintImages")]
        public ApiResponse FingerPrintImages(int numberOfRecords, int pageNumber)
        {
            var response = new ApiResponse();
            try
            {
                if (numberOfRecords < 1)
                {
                    response.Code = -1;
                    response.Message = "Invalid Query: The mandatory parametr 'numberOfRecords' is not provided.";
                    response.Response = new List<FingerprintImage>();
                    return response;
                }

                var fingerprintImages = _fingerprintImageService.Query().Select().OrderBy(s => s.TableId).Skip(pageNumber * numberOfRecords).Take(numberOfRecords).ToList();
                if (fingerprintImages.Any())
                {
                    response.Code = -1;
                    response.Message = "No record found.";
                    response.Response = new List<FingerprintImage>();
                    return response;
                }

                var fingerprintImageStr = JsonConvert.SerializeObject(fingerprintImages);
                var payloadAsciiStr = Encoding.ASCII.GetBytes(fingerprintImageStr);
                var payloadBase64Str = Convert.ToBase64String(payloadAsciiStr);

                response.Code = 5;
                response.Response = payloadBase64Str;
                response.Message = "Query was successfully completed.";
                return response;

            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = "Process failed due to an internal server error. Please try again later.";
                return response;
            }
        }

        [Route("Photograph")]
        public ApiResponse Photograph(string eid)
        {
            var response = new ApiResponse();
            try
            {
                if (string.IsNullOrEmpty(eid))
                {
                    response.Code = -1;
                    response.Message = "Invalid Query: The expected parametr 'eid' is empty.";
                    response.Response = new Photograph();
                    return response;
                }

                var photographs = _photographService.Query(s => s.EnrollmentId == eid).Select().ToList();
                if (photographs.Any())
                {
                    response.Code = -1;
                    response.Message = "No record found.";
                    response.Response = new Photograph();
                    return response;
                }

                var photographStr = JsonConvert.SerializeObject(photographs[0]);
                var payloadAsciiStr = Encoding.ASCII.GetBytes(photographStr);
                var payloadBase64Str = Convert.ToBase64String(payloadAsciiStr);

                response.Code = 5;
                response.Response = payloadBase64Str;
                response.Message = "Query was successfully completed.";
                return response;

            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = "Process failed due to an internal server error. Please try again later.";
                return response;
            }
        }

        [Route("Photographs")]
        public ApiResponse Photographs(int numberOfRecords, int pageNumber)
        {
            var response = new ApiResponse();
            try
            {
                if (numberOfRecords < 1)
                {
                    response.Code = -1;
                    response.Message = "Invalid Query: The mandatory parametr 'numberOfRecords' is not provided.";
                    response.Response = new List<Photograph>();
                    return response;
                }

                var photographs = _photographService.Query().Select().OrderBy(s => s.TableId).Skip(pageNumber * numberOfRecords).Take(numberOfRecords).ToList();
                if (photographs.Any())
                {
                    response.Code = -1;
                    response.Message = "No record found.";
                    response.Response = new List<Photograph>();
                    return response;
                }

                var photographStr = JsonConvert.SerializeObject(photographs);
                var payloadAsciiStr = Encoding.ASCII.GetBytes(photographStr);
                var payloadBase64Str = Convert.ToBase64String(payloadAsciiStr);

                response.Code = 5;
                response.Response = payloadBase64Str;
                response.Response = photographs;
                response.Message = "Query was successfully completed.";
                return response;

            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = "Process failed due to an internal server error. Please try again later.";
                return response;
            }
        }
        
    }
}
