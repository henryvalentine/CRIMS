using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using Crims.Data.Contracts;
using Crims.Data.Models;
using Crims.UI.Web.Enroll.Helpers;
using Crims.UI.Web.Enroll.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Repository.Pattern.UnitOfWork;
using WebGrease.Css.Extensions;

namespace Crims.UI.Web.Enroll.Controllers
{
    public class EnrollmentController : Controller
    {
        private IApprovalService _approvalService;
        private ICustomDataService _customDataService;
        private ICustomListService _customListService;
        private IAppSettingService _appSettingService;
        private ICustomFieldService _customFieldService;
        private ICustomListDataService _customListDataService;
        private ICustomGroupService _customGroupService;
        private ICustomFieldTypeService _customFieldTypeService;
        private IPhotographService _photographService;
        private IFingerprintImageService _fingerprintImageService;
        private IFingerprintTemplateService _fingerprintTemplateService;
        private IFingerprintReasonService _fingerprintReasonService;
        private ISignatureService _signatureService;
        private IBaseDataService _baseDataService;
        private IUnitOfWorkAsync _unitOfWork;
        private IProjectService _projectService;
        public EnrollmentController()
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Login", "Account");
            }
        }

        public EnrollmentController(IFingerprintReasonService fingerprintReasonService, IPhotographService photographService, IProjectService projectService, IBaseDataService baseDataService, IUnitOfWorkAsync unitOfWork, IAppSettingService appSettingService, ISignatureService signatureService, IFingerprintImageService fingerprintImageService, IFingerprintTemplateService fingerprintTemplateService)
        {
            _projectService = projectService;
            _photographService = photographService;
            _projectService = projectService;
            _fingerprintImageService = fingerprintImageService;
            _fingerprintReasonService = fingerprintReasonService;
            _fingerprintTemplateService = fingerprintTemplateService;
            _signatureService = signatureService;
            _baseDataService = baseDataService;
            _baseDataService = baseDataService;
            _appSettingService = appSettingService;
            _unitOfWork = unitOfWork;
        }
        
        public ActionResult Index()
        {
          return View();
        }
        
        private BiometricModel GetBioDataFromFolder(string enrollmentId, string mainPath)
        {
            try
            {
                var folderPath = mainPath + "/" + enrollmentId;

                var enrollePath = Server.MapPath(folderPath);

                var ext = new List<string> { ".jpg" };
                
                if (!Directory.Exists(enrollePath))
                {
                    return new BiometricModel();
                }

                var myFiles = Directory.GetFiles(enrollePath, "*.*", SearchOption.AllDirectories).Where(s => ext.Contains(Path.GetExtension(s))).ToList();
                if (!myFiles.Any())
                {
                    return new BiometricModel();
                }
              
                var bioModel = new BiometricModel
                {
                    PhotoPath = GenericHelpers.MapPath(myFiles.Where(f => f.Contains("photo_image")).ElementAt(0)).Replace("~", ""),
                    LeftLittle = GenericHelpers.MapPath(myFiles.Where(f => f.Contains("LFLittle")).ElementAt(0)).Replace("~", ""),
                    LeftRing = GenericHelpers.MapPath(myFiles.Where(f => f.Contains("LFRing")).ElementAt(0)).Replace("~", ""),
                    LeftMiddle = GenericHelpers.MapPath(myFiles.Where(f => f.Contains("LFMiddle")).ElementAt(0)).Replace("~", ""),
                    LeftIndex = GenericHelpers.MapPath(myFiles.Where(f => f.Contains("LFIndex")).ElementAt(0)).Replace("~", ""),
                    LeftThumb = GenericHelpers.MapPath(myFiles.Where(f => f.Contains("LFThumb")).ElementAt(0)).Replace("~", ""),
                    RightThumb = GenericHelpers.MapPath(myFiles.Where(f => f.Contains("RFThumb")).ElementAt(0)).Replace("~", ""),
                    RightIndex = GenericHelpers.MapPath(myFiles.Where(f => f.Contains("RFIndex")).ElementAt(0)).Replace("~", ""),
                    RightMiddle = GenericHelpers.MapPath(myFiles.Where(f => f.Contains("RFMiddle")).ElementAt(0)).Replace("~", ""),
                    RightRing = GenericHelpers.MapPath(myFiles.Where(f => f.Contains("RFRing")).ElementAt(0)).Replace("~", ""),
                    RightLittle = GenericHelpers.MapPath(myFiles.Where(f => f.Contains("RFLittle")).ElementAt(0)).Replace("~", ""),
                    Signature = GenericHelpers.MapPath(myFiles.Where(f => f.Contains("sign_image")).ElementAt(0)).Replace("~", "")
                };

                return bioModel;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return new BiometricModel();
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
        public ActionResult GetBiometricData(string enrollmentId)
        {
            var bioModel = new BiometricDbModel { FingerReasons = new List<FingerReason>() };
            try
            {
                var photographs = _photographService.Query(x => x.EnrollmentId == enrollmentId).Select().ToList();
                var fingerprintImages = _fingerprintImageService.Query(x => x.EnrollmentId == enrollmentId).Select().ToList();
                var signatures = _signatureService.Query(x => x.EnrollmentId == enrollmentId).Select().ToList();
                
                if (photographs.Any())
                {
                    bioModel.Photo = $"data:image/jpg;base64,{Convert.ToBase64String(photographs[0].PhotographImage)}";
                }

                if (signatures.Any())
                {
                    bioModel.Signature = $"data:image/jpg;base64,{Convert.ToBase64String(signatures[0].SignatureImage)}";
                }
                else
                {
                    bioModel.Signature = "";
                }
                //bioModel.LeftLittle = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.LFLittle) ? $"data:image/jpg;base64,{Convert.ToBase64String(fingerprintImages.Find(f => f.FingerIndexId == (int)EnumManager.FingerDescription.LFLittle).FingerPrintImage)}" : "";
                //bioModel.LeftRing = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.LFRing) ? $"data:image/jpg;base64,{Convert.ToBase64String(fingerprintImages.Find(f => f.FingerIndexId == (int)EnumManager.FingerDescription.LFRing).FingerPrintImage)}" : "";
                //bioModel.LeftMiddle = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.LFMiddle) ? $"data:image/jpg;base64,{Convert.ToBase64String(fingerprintImages.Find(f => f.FingerIndexId == (int)EnumManager.FingerDescription.LFMiddle).FingerPrintImage)}" : "";
                //bioModel.LeftIndex = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.LFIndex) ? $"data:image/jpg;base64,{Convert.ToBase64String(fingerprintImages.Find(f => f.FingerIndexId == (int)EnumManager.FingerDescription.LFIndex).FingerPrintImage)}" : "";
                //bioModel.LeftThumb = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.LFThumb) ? $"data:image/jpg;base64,{Convert.ToBase64String(fingerprintImages.Find(f => f.FingerIndexId == (int)EnumManager.FingerDescription.LFThumb).FingerPrintImage)}" : "";
                //bioModel.RightThumb = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.RFThumb) ? $"data:image/jpg;base64,{Convert.ToBase64String(fingerprintImages.Find(f => f.FingerIndexId == (int)EnumManager.FingerDescription.RFThumb).FingerPrintImage)}" : "";
                //bioModel.RightIndex = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.RFIndex) ? $"data:image/jpg;base64,{Convert.ToBase64String(fingerprintImages.Find(f => f.FingerIndexId == (int)EnumManager.FingerDescription.RFIndex).FingerPrintImage)}" : "";
                //bioModel.RightMiddle = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.RFMiddle) ? $"data:image/jpg;base64,{Convert.ToBase64String(fingerprintImages.Find(f => f.FingerIndexId == (int)EnumManager.FingerDescription.RFMiddle).FingerPrintImage)}" : "";
                //bioModel.RightRing = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.RFRing) ? $"data:image/jpg;base64,{Convert.ToBase64String(fingerprintImages.Find(f => f.FingerIndexId == (int)EnumManager.FingerDescription.RFRing).FingerPrintImage)}" : "";
                //bioModel.RightLittle = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.RFLittle) ? $"data:image/jpg;base64,{Convert.ToBase64String(fingerprintImages.Find(f => f.FingerIndexId == (int)EnumManager.FingerDescription.RFLittle).FingerPrintImage)}" : "";

                bioModel.LeftLittle = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.LFLittle) ? "/Images/LFLittle.jpg" : "";
                bioModel.LeftRing = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.LFRing) ? "/Images/LFRing.jpg" : "";
                bioModel.LeftMiddle = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.LFMiddle) ? "/Images/LFMiddle.jpg" : "";
                bioModel.LeftIndex = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.LFIndex) ? "/Images/LFIndex.jpg" : "";
                bioModel.LeftThumb = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.LFThumb) ? "/Images/LFThumb.jpg" : "";
                bioModel.RightThumb = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.RFThumb) ? "/Images/RFThumb.jpg" : "";
                bioModel.RightIndex = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.RFIndex) ? "/Images/RFIndex.jpg" : "";
                bioModel.RightMiddle = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.RFMiddle) ? "/Images/RFMiddle.jpg" : "";
                bioModel.RightRing = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.RFRing) ? "/Images/RFRing.jpg" : "";
                bioModel.RightLittle = fingerprintImages.Any(f => f.FingerIndexId == (int)EnumManager.FingerDescription.RFLittle) ? "/Images/RFLittle.jpg" : "";
                
                var fingerprintReasons = _fingerprintReasonService.Query(f => f.EnrollmentId == enrollmentId).Select().ToList();
                if (fingerprintReasons.Any())
                {
                    fingerprintReasons.ForEach(t =>
                    {
                        var finger = Enum.GetName(typeof(EnumManager.FingerDescription), t.FingerIndex);
                        bioModel.FingerReasons.Add(new FingerReason {FingerIndex = t.FingerIndex, Name = finger, Reason = t.FingerReason});
                    });
                }
                return Json(bioModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return Json(bioModel, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetFileResult(byte[] binry)
        {
            return new FileContentResult(binry, "image/jpg"); ;
        }
    }
}
