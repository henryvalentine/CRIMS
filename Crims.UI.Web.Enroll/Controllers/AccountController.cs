using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Crims.UI.Web.Enroll.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Crims.UI.Web.Enroll.Models;
using Crims.UI.Web.Enroll.Results;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Crims.UI.Web.Enroll.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ChamsMailService.WebServiceSoapClient _mailService = new ChamsMailService.WebServiceSoapClient();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult UserView()
        {
            return View();
        }

        // The Authorize Action is the end point which gets called when you access any
        // protected Web API. If the user is not logged in then they will be redirected to 
        // the Login page. After a successful login you can call a Web API.
        [HttpGet]
        public ActionResult Authorize()
        {
            var claims = new ClaimsPrincipal(User).Claims.ToArray();
            var identity = new ClaimsIdentity(claims, "Bearer");
            AuthenticationManager.SignIn(identity);
            return new EmptyResult();
        }
        
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Setup()
        {
            return View();
        }
        
        public ActionResult Users()
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Login", "Account");
            }

            if (!User.IsInRole("Admin") && !User.IsInRole("Site_Administrator"))
            {
                RedirectToAction("Login", "Account");
            }
            
            return View();
        }

        public ActionResult GetUserSessionName()
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

        [HttpGet]
        public ActionResult GetUsers(JQueryDataTableParamModel param)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    RedirectToAction("Login", "Account");
                }
                if (!User.IsInRole("Admin") && !User.IsInRole("Site_Administrator"))
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

                var appUsers = new List<AppUser>();
                var userList = string.IsNullOrEmpty(param.sSearch) ? UserManager.Users.OrderBy(f => f.UserInfo.FullName).Skip(param.iDisplayStart).Take(param.iDisplayLength).Include("UserInfo").ToList()
                    : UserManager.Users.Where(s => s.Email == param.sSearch || s.PhoneNumber == param.sSearch || s.UserInfo.FullName.ToLower().Contains(param.sSearch.ToLower())).OrderBy(f => f.UserInfo.FullName).Skip(param.iDisplayStart).Take(param.iDisplayLength).Include("UserInfo").ToList();
                if (!userList.Any())
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
                var countG = string.IsNullOrEmpty(param.sSearch) ? UserManager.Users.Count(): UserManager.Users.Count(s => s.Email == param.sSearch || s.PhoneNumber == param.sSearch || s.UserInfo.FullName.ToLower().Contains(param.sSearch.ToLower()));
                userList.ForEach(s =>
                {
                    appUsers.Add(new AppUser
                    {
                        Id = s.Id,
                        FullName = s.UserInfo.FullName,
                        Email = s.Email,
                        PhoneNumber = s.PhoneNumber,
                        Sex = s.UserInfo.Sex,
                        Role = UserManager.GetRoles(s.Id).ToList()[0].Replace("_", " "),
                        ProfileId = s.UserInfo.Id,
                        DateCreated = s.UserInfo.DateCreated,
                        DateCreatedStr = s.UserInfo.DateCreated.ToString("dd/MM/yyyy"),
                        Status = s.UserInfo.Status,
                    });
                });
               
                if (!appUsers.Any())
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
                
                IEnumerable<AppUser> dataList = appUsers;
                var result = from c in dataList
                             select new[] {c.Id, c.FullName, c.Email, c.PhoneNumber, c.Sex, c.Role, c.DateCreatedStr};
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
        
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var userInfo = new UserModel();
            try
            {
                if (string.IsNullOrEmpty(model.Email))
                {
                    userInfo.Code = -1;
                    userInfo.Message = "Please provide your Email";
                    return Json(userInfo, JsonRequestBehavior.AllowGet);
                }

                if (!model.Email.Contains("@"))
                {
                    userInfo.Code = -1;
                    userInfo.Message = "Please provide a valid Email";
                    return Json(userInfo, JsonRequestBehavior.AllowGet);
                }

                if (!model.Email.Contains("."))
                {
                    userInfo.Code = -1;
                    userInfo.Message = "Please provide a valid Email";
                    return Json(userInfo, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(model.Password))
                {
                    userInfo.Code = -1;
                    userInfo.Message = "Please provide your Password";
                    return Json(userInfo, JsonRequestBehavior.AllowGet);
                }

                var user = await UserManager.FindAsync(model.Email, model.Password);
                if (user == null)
                {
                    userInfo.Code = -1;
                    userInfo.Message = "Invalid Credentials";
                    return Json(userInfo, JsonRequestBehavior.AllowGet);
                }

                if (!user.EmailConfirmed)
                {
                    userInfo.Code = -1;
                    userInfo.Message = "Please go to your email to activate your account before proceeding";
                    return Json(userInfo, JsonRequestBehavior.AllowGet);
                }
                
                var connStr = ConfigurationManager.ConnectionStrings["crimsDbEntities"].ConnectionString; ;
                var conn = new MySqlConnection(connStr);
                conn.Open();

                var sql = $"SELECT t.*, o.*, r.*, rr.* FROM aspnetusers t JOIN userprofiles o ON o.Id = t.UserInfo_Id JOIN aspnetuserroles r ON t.Id = r.UserId JOIN aspnetroles rr ON r.RoleId = rr.Id WHERE t.Id = '{user.Id}'";
                
                var cmd = new MySqlCommand(sql, conn);
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        if (!rdr.HasRows) continue;
                        var dateRegistered = Convert.ToDateTime(rdr["DateCreated"].ToString());
                        //var lockoutEndDate = rdr["LockoutEndDateUtc"].ToString();
                        //var lockoutEndDateUtc = !string.IsNullOrEmpty(lockoutEndDate) ? Convert.ToDateTime(lockoutEndDate) : new DateTime();
                        //var accessFailedCount = rdr["AccessFailedCount"].ToString();
                        //var lockOutEnabled = rdr["LockoutEnabled"].ToString();
                        //var twoFactorEnabled = rdr["TwoFactorEnabled"].ToString();
                        //var phoneNumberConfirmed = rdr["PhoneNumberConfirmed"].ToString();
                        //var emailConfirmed = rdr["EmailConfirmed"].ToString();

                        userInfo = new UserModel
                        {
                            ProfileId = rdr["UserInfo_Id"].ToString(),
                            FullName = rdr["FullName"].ToString(),
                            PhoneNumber = rdr["PhoneNumber"].ToString(),
                            Sex = rdr["Sex"].ToString(),
                            DateCreated = dateRegistered,
                            Status = rdr["Status"].ToString(),
                            Hash = rdr["PasswordHash"].ToString(),
                            Email = rdr["Email"].ToString(),
                            Role = rdr["Name"].ToString().Replace("_", " "),
                            UserId = rdr["Id"].ToString(),
                        };
                    }
                    ;
                }
                if (string.IsNullOrEmpty(userInfo?.ProfileId))
                    {
                        userInfo.Code = -1;
                        userInfo.Message = "Your user details could not be retrieved. Please contact our support team";
                        return Json(userInfo, JsonRequestBehavior.AllowGet);
                    }

                 var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
                switch (result)
                {
                    case SignInStatus.Success:
                        userInfo.Code = 5;
                        userInfo.Message = "Login was successful";
                        Session["_loginName"] = @"Hello " + userInfo.FullName.Split(' ')[0] + @"!";
                        return Json(userInfo, JsonRequestBehavior.AllowGet);
                    case SignInStatus.LockedOut:
                        userInfo.Code = -1;
                        userInfo.Message = "You account was temporarily locked due to maximum number of password attempots reached. Please wait to be unlocked after a while.";
                        return Json(userInfo, JsonRequestBehavior.AllowGet);
                    case SignInStatus.RequiresVerification:
                        userInfo.Code = -1;
                        userInfo.Message = "Your account requires verification";
                        return Json(userInfo, JsonRequestBehavior.AllowGet);
                    case SignInStatus.Failure:
                    default:
                        userInfo.Code = -1;
                        userInfo.Message = "Invalid login attempt.";
                        return Json(userInfo, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                userInfo.Code = -1;
                userInfo.Message = ex.Message;
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return Json(userInfo, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AppLogin(string email, string password)
        {
            var acResponse = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide your Email";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (!email.Contains("@"))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide a valid Email";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (!email.Contains("."))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide a valid Email";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(password))
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please provide your Password";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                var user = await UserManager.FindAsync(email, password);
                if (user == null)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Invalid Credentials";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                if (!user.EmailConfirmed)
                {
                    acResponse.Code = -1;
                    acResponse.Message = "Please go to your email to activate your account before proceeding";
                    return Json(acResponse, JsonRequestBehavior.AllowGet);
                }

                var result = await SignInManager.PasswordSignInAsync(email, password, false, true);
                switch (result)
                {
                    case SignInStatus.Success:
                        acResponse.Code = 5;
                        acResponse.Message = "Login was successful";
                        var userRoles = UserManager.GetRoles(user.Id);
                        acResponse.UserRole = userRoles[0];
                        return Json(acResponse, JsonRequestBehavior.AllowGet);
                    case SignInStatus.LockedOut:
                        acResponse.Code = -1;
                        acResponse.Message = "You account was temporarily locked due to maximum number of password attempots reached. Please wait to be unlocked after a while.";
                        return Json(acResponse, JsonRequestBehavior.AllowGet);
                    case SignInStatus.RequiresVerification:
                        acResponse.Code = -1;
                        acResponse.Message = "Your account requires verification";
                        return Json(acResponse, JsonRequestBehavior.AllowGet);
                    case SignInStatus.Failure:
                    default:
                        acResponse.Code = -1;
                        acResponse.Message = "Invalid login attempt.";
                        return Json(acResponse, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                acResponse.Code = -1;
                acResponse.Message = ex.Message;
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return Json(acResponse, JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult GetRoleLists()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new List<IdentityRole>(), JsonRequestBehavior.AllowGet);
                }

                if (!User.IsInRole("Super_Admin") && !User.IsInRole("Admin"))
                {
                    return Json(new List<IdentityRole>(), JsonRequestBehavior.AllowGet);
                }
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                var userRoles = roleManager.Roles.ToList();
                if (userRoles.Any())
                {
                    var admins = userRoles.Where(r => r.Name.ToLower() == "admin").ToList();
                    if (admins.Any())
                    {
                        userRoles.Remove(admins[0]);
                    }
                }
                return Json(userRoles, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<IdentityRole>(), JsonRequestBehavior.AllowGet);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UserSignUp(AppUser user)
        {
            var gVal = new ActivityResponse();

            try
            {
                if (string.IsNullOrEmpty(user.FullName))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Full Name";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.PhoneNumber))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Phone Number";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Sex))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please select Gender";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }
                
                if (string.IsNullOrEmpty(user.Email))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide your Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (!user.Email.Contains("@"))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide a valid Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (!user.Email.Contains("."))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide a valid Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Password))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Password";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.ConfirmPassword))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please confirm Password";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (user.Password != user.ConfirmPassword)
                {
                    gVal.Code = -1;
                    gVal.Message = "The Passwords do not match";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                var duplicateEmailsAndPhoneNumbers = UserManager.Users.Where(u => u.Email == user.Email || u.PhoneNumber == user.PhoneNumber).ToList();

                if (duplicateEmailsAndPhoneNumbers.Any())
                {
                    var duplicate = duplicateEmailsAndPhoneNumbers[0];
                    if (duplicate.PhoneNumber == user.PhoneNumber && duplicate.Id != user.Id)
                    {
                        gVal.Message = "Anothr user with this Phone Number already exists.";
                        gVal.Code = -1;
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }

                    if (duplicate.Email == user.Email && duplicate.Id != user.Id)
                    {
                        gVal.Message = "Anothr user with this Email already exists.";
                        gVal.Code = -1;
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }
                }
                
                var appUser = new ApplicationUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    PhoneNumber = user.PhoneNumber,
                    UserInfo = new ApplicationDbContext.UserProfile
                    {
                        Id = Guid.NewGuid().ToString(),
                        FullName = user.FullName,
                        Sex = user.Sex,
                        PhoneNumber = user.PhoneNumber,
                        DateCreated = DateTime.Now,
                        Status = "Active"
                    }
                };

                var result = await UserManager.CreateAsync(appUser, user.Password);
                if (!result.Succeeded)
                {
                    gVal.Message = result.Errors.ToList()[0];
                    gVal.Code = -1;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                UserManager.AddToRole(appUser.Id, "Enrollee");

                var verifier = UserManager.FindByEmail(user.Email);
                if (verifier == null)
                {
                    gVal.Message = "Your Account was successfully set up but an activation email could not be sent to you. Please request for an activation email from the support team.";
                    gVal.Code = -1;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                user.SecurityStamp = verifier.SecurityStamp;
                var status = SendSignupMail(user);
                if (!status)
                {
                    gVal.Message = "Your Account was successfully set up but an activation email could not be sent to you. Please request for an activation email from the support team.";
                    gVal.Code = -1;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                gVal.Message = "Your account was successfully set up ";
                gVal.Code = 5;
                return Json(gVal, JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception ex)
            {
                gVal.Message = "An unknown error was encountered. Please try again later or contact administrator.";
                gVal.Code = -1;
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> OfficerSetup(AppUser user)
        {
            var gVal = new ActivityResponse();

            try
            {
                if (string.IsNullOrEmpty(user.FullName))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Full Name";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.PhoneNumber))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Phone Number";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Sex))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please select Gender";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Email))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide your Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (!user.Email.Contains("@"))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide a valid Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (!user.Email.Contains("."))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide a valid Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Password))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Password";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.ConfirmPassword))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please confirm Password";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (user.Password != user.ConfirmPassword)
                {
                    gVal.Code = -1;
                    gVal.Message = "The Passwords do not match";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                var duplicateEmailsAndPhoneNumbers = UserManager.Users.Where(u => u.Email == user.Email || u.PhoneNumber == user.PhoneNumber).ToList();

                if (duplicateEmailsAndPhoneNumbers.Any())
                {
                    var duplicate = duplicateEmailsAndPhoneNumbers[0];
                    if (duplicate.PhoneNumber == user.PhoneNumber && duplicate.Id != user.Id)
                    {
                        gVal.Message = "Anothr user with this Phone Number already exists.";
                        gVal.Code = -1;
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }

                    if (duplicate.Email == user.Email && duplicate.Id != user.Id)
                    {
                        gVal.Message = "Anothr user with this Email already exists.";
                        gVal.Code = -1;
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }
                }

                var appUser = new ApplicationUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PhoneNumber = user.PhoneNumber,
                    UserInfo = new ApplicationDbContext.UserProfile
                    {
                        Id = Guid.NewGuid().ToString(),
                        FullName = user.FullName,
                        Sex = user.Sex,
                        PhoneNumber = user.PhoneNumber,
                        DateCreated = DateTime.Now,
                        Status = "Active"
                    }
                };

                var result = await UserManager.CreateAsync(appUser, user.Password);
                if (!result.Succeeded)
                {
                    gVal.Message = result.Errors.ToList()[0];
                    gVal.Code = -1;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                UserManager.AddToRole(appUser.Id, "Enrollment_Officer");
                gVal.Message = "Officer account was successfully set up ";
                gVal.Code = 5;
                return Json(gVal, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                gVal.Message = "An unknown error was encountered. Please try again later or contact administrator.";
                gVal.Code = -1;
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(AppUser user)
        {
            var gVal = new ActivityResponse();

            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    RedirectToAction("Login", "Account");
                }

                if (!User.IsInRole("Admin"))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please sign in";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }
                
                if (string.IsNullOrEmpty(user.FullName))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Full Name";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.PhoneNumber))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Phone Number";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Sex))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please select Gender";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Role))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please select a Role";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Email))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide your Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (!user.Email.Contains("@"))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide a valid Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (!user.Email.Contains("."))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide a valid Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Password))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Password";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.ConfirmPassword))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please confirm Password";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (user.Password != user.ConfirmPassword)
                {
                    gVal.Code = -1;
                    gVal.Message = "The Passwords do not match";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                var duplicateEmailsAndPhoneNumbers = UserManager.Users.Where(u => u.Email == user.Email || u.PhoneNumber == user.PhoneNumber).ToList();

                if (duplicateEmailsAndPhoneNumbers.Any())
                {
                    var duplicate = duplicateEmailsAndPhoneNumbers[0];
                    if (duplicate.PhoneNumber == user.PhoneNumber && duplicate.Id != user.Id)
                    {
                        gVal.Message = "Anothr user with this Phone Number already exists.";
                        gVal.Code = -1;
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }

                    if (duplicate.Email == user.Email && duplicate.Id != user.Id)
                    {
                        gVal.Message = "Anothr user with this Email already exists.";
                        gVal.Code = -1;
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }
                }

                if (string.IsNullOrEmpty(user.Role))
                {
                    gVal.Message = "Please select a Role for the user.";
                    gVal.Code = -1;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                var appUser = new ApplicationUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PhoneNumber = user.PhoneNumber,
                    UserInfo = new ApplicationDbContext.UserProfile
                    {
                        Id = Guid.NewGuid().ToString(),
                        FullName = user.FullName,
                        Sex = user.Sex,
                        PhoneNumber = user.PhoneNumber,
                        DateCreated = DateTime.Now,
                        Status = "Active"
                    }
                };

                var result = await UserManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(appUser.Id, user.Role);
                    
                    gVal.Message = "Your account was successfully set up ";
                    gVal.Code = 5;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                gVal.Message = result.Errors.ToList()[0];
                gVal.Code = -1;
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                gVal.Message = "An unknown error was encountered. Please try again later or contact administrator.";
                gVal.Code = -1;
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> EditUser(AppUser user)
        {
            var gVal = new ActivityResponse();

            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    RedirectToAction("Login", "Account");
                }
                if (!User.IsInRole("Admin"))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please sign in";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(user.FullName))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Full Name";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.PhoneNumber))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Phone Number";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Sex))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please select Gender";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Role))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please select a Role";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Email))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide your Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (!user.Email.Contains("@"))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide a valid Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (!user.Email.Contains("."))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide a valid Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                var updatePassword = false;
                if (!string.IsNullOrEmpty(user.Password))
                {
                    if (string.IsNullOrEmpty(user.ConfirmPassword))
                    {
                        gVal.Code = -1;
                        gVal.Message = "Please confirm Password";
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }

                    if (user.Password != user.ConfirmPassword)
                    {
                        gVal.Code = -1;
                        gVal.Message = "The Passwords do not match";
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }

                    updatePassword = true;
                }
                
                var duplicateEmailsAndPhoneNumbers = UserManager.Users.Where(u => u.Email == user.Email || u.PhoneNumber == user.PhoneNumber).ToList();

                if (duplicateEmailsAndPhoneNumbers.Any())
                {
                    var duplicate = duplicateEmailsAndPhoneNumbers[0];
                    if (duplicate.PhoneNumber == user.PhoneNumber && duplicate.Id != user.Id)
                    {
                        gVal.Message = "Anothr user with this Phone Number already exists.";
                        gVal.Code = -1;
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }

                    if (duplicate.Email == user.Email && duplicate.Id != user.Id)
                    {
                        gVal.Message = "Anothr user with this Email already exists.";
                        gVal.Code = -1;
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }
                }

                var users = UserManager.Users.Where(u => u.Id == user.Id).Include("UserInfo").ToList();

                if (!users.Any())
                {
                    gVal.Message = "An unknown error was encountered. Please try again or contact support";
                    gVal.Code = -1;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Role))
                {
                    gVal.Message = "Please select a Role for the user.";
                    gVal.Code = -1;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                var userInfo = users[0];
                userInfo.UserName = user.Email;
                userInfo.Email = user.Email;
                userInfo.PhoneNumber = user.PhoneNumber;

                userInfo.UserInfo.FullName = user.FullName;
                userInfo.UserInfo.Sex = user.Sex;
                userInfo.UserInfo.PhoneNumber = user.PhoneNumber;

                var result = await UserManager.UpdateAsync(userInfo);
                if (result.Succeeded)
                {
                    var userRoles = UserManager.GetRoles(userInfo.Id).ToList();

                    if (userRoles.All(r => r != user.Role))
                    {
                        UserManager.AddToRole(user.Id, user.Role);

                        userRoles.ForEach(f =>
                        {
                            if (f != user.Role)
                            {
                                UserManager.RemoveFromRole(userInfo.Id, f);
                            }
                        });
                    }

                    if (updatePassword)
                    {
                        var passwordHash = UserManager.PasswordHasher.HashPassword(user.Password);
                        var verify = UserManager.PasswordHasher.VerifyHashedPassword(passwordHash, user.Password);
                        if (verify != PasswordVerificationResult.Success)
                        {
                            gVal.Message = "User profile was successfully updated but the password could not be updated. Please try again later";
                            gVal.Code = -1;
                            return Json(gVal, JsonRequestBehavior.AllowGet);
                        }

                        userInfo.PasswordHash = passwordHash;
                        var passwordUpdateResult = await UserManager.UpdateAsync(userInfo);
                        if (!passwordUpdateResult.Succeeded)
                        {
                            gVal.Message = "User profile was successfully updated but the password could not be updated. Please try again later";
                            gVal.Code = -1;
                            return Json(gVal, JsonRequestBehavior.AllowGet);
                        }
                    }

                    gVal.Message = "Account was successfully updated ";
                    gVal.Code = 5;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                gVal.Message = result.Errors.ToList()[0];
                gVal.Code = -1;
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                gVal.Message = "An unknown error was encountered. Please try again later or contact administrator.";
                gVal.Code = -1;
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditMyProfile(AppUser user)
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Login", "Account");
            }
            var gVal = new ActivityResponse();

            try
            {
                if (string.IsNullOrEmpty(user.FullName))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Full Name";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.PhoneNumber))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Phone Number";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Sex))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please select Gender";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }
                
                if (string.IsNullOrEmpty(user.Email))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide your Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (!user.Email.Contains("@"))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide a valid Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (!user.Email.Contains("."))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide a valid Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                var updatePassword = false;
                if (!string.IsNullOrEmpty(user.Password))
                {
                    if (string.IsNullOrEmpty(user.ConfirmPassword))
                    {
                        gVal.Code = -1;
                        gVal.Message = "Please confirm Password";
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }

                    if (user.Password != user.ConfirmPassword)
                    {
                        gVal.Code = -1;
                        gVal.Message = "The Passwords do not match";
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }

                    updatePassword = true;
                }

                var duplicateEmailsAndPhoneNumbers = UserManager.Users.Where(u => u.Email == user.Email || u.PhoneNumber == user.PhoneNumber).ToList();

                if (duplicateEmailsAndPhoneNumbers.Any())
                {
                    var duplicate = duplicateEmailsAndPhoneNumbers[0];
                    if (duplicate.PhoneNumber == user.PhoneNumber && duplicate.Id != user.Id)
                    {
                        gVal.Message = "Anothr user with this Phone Number already exists.";
                        gVal.Code = -1;
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }

                    if (duplicate.Email == user.Email && duplicate.Id != user.Id)
                    {
                        gVal.Message = "Anothr user with this Email already exists.";
                        gVal.Code = -1;
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }
                }

                var users = UserManager.Users.Where(u => u.Id == user.Id).Include("UserInfo").ToList();

                if (!users.Any())
                {
                    gVal.Message = "An unknown error was encountered. Please try again or contact support";
                    gVal.Code = -1;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Role))
                {
                    gVal.Message = "Please select a Role for the user.";
                    gVal.Code = -1;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                var userInfo = users[0];
                //userInfo.UserName = user.Email;
                //userInfo.Email = user.Email;
                userInfo.PhoneNumber = user.PhoneNumber;

                userInfo.UserInfo.FullName = user.FullName;
                userInfo.UserInfo.Sex = user.Sex;
                userInfo.UserInfo.PhoneNumber = user.PhoneNumber;

                var result = await UserManager.UpdateAsync(userInfo);
                if (result.Succeeded)
                {
                    if (updatePassword)
                    {
                        var passwordHash = UserManager.PasswordHasher.HashPassword(user.Password);
                        var verify = UserManager.PasswordHasher.VerifyHashedPassword(passwordHash, user.Password);
                        if (verify != PasswordVerificationResult.Success)
                        {
                            gVal.Message = "User profile was successfully updated but the password could not be updated. Please try again later";
                            gVal.Code = -1;
                            return Json(gVal, JsonRequestBehavior.AllowGet);
                        }

                        userInfo.PasswordHash = passwordHash;
                        var passwordUpdateResult = await UserManager.UpdateAsync(userInfo);
                        if (!passwordUpdateResult.Succeeded)
                        {
                            gVal.Message = "User profile was successfully updated but the password could not be updated. Please try again later";
                            gVal.Code = -1;
                            return Json(gVal, JsonRequestBehavior.AllowGet);
                        }
                    }

                    gVal.Message = "Your Profile was successfully updated ";
                    gVal.Code = 5;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                gVal.Message = result.Errors.ToList()[0];
                gVal.Code = -1;
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                gVal.Message = "An unknown error was encountered. Please try again later or contact administrator.";
                gVal.Code = -1;
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult GetUser(string userId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Login", "Account");
            }
            try
            {
                var users = UserManager.Users.Where(u => u.Id == userId).Include("UserInfo").ToList();

                if (!users.Any())
                {
                    return Json(new AppUser(), JsonRequestBehavior.AllowGet);
                }

                var user = users[0];
                
                var userRoles = UserManager.GetRoles(user.Id);
                if (!userRoles.Any())
                {
                    return Json(new AppUser(), JsonRequestBehavior.AllowGet);
                }

                var appUser = new AppUser
                {
                    Id = user.Id,
                    FullName = user.UserInfo.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Sex = user.UserInfo.Sex,
                    ProfileId = user.UserInfo.Id,
                    Status = user.UserInfo.Sex,
                    Role = userRoles[0]
                };
                
                return Json(appUser, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return Json(new AppUser(), JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult MyProfile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Id = User.Identity.GetUserId();
            return View(new AppUser());
        }
        
        public ActionResult UserProfile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Id = User.Identity.GetUserId();
            return View(new AppUser());
        }

        [HttpPost]
        [AllowAnonymous]
        ////[ValidateAntiForgeryToken]
        public async Task<ActionResult> AdminSignUp(AppUser user)
        {
            var gVal = new ActivityResponse();

            try
            {
                if (string.IsNullOrEmpty(user.FullName))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Full Name";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.PhoneNumber))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Phone Number";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Sex))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please select Gender";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }
                
                if (string.IsNullOrEmpty(user.Email))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide your Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (!user.Email.Contains("@"))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide a valid Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (!user.Email.Contains("."))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide a valid Email";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.Password))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide Password";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(user.ConfirmPassword))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please confirm Password";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (user.Password != user.ConfirmPassword)
                {
                    gVal.Code = -1;
                    gVal.Message = "The Passwords do not match";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                var emailDuplicates = UserManager.FindByEmail(user.Email);

                if (emailDuplicates != null)
                {
                    gVal.Message = "This Email already exists on our servers";
                    gVal.Code = -1;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                var appUser = new ApplicationUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PhoneNumber = user.PhoneNumber,
                    UserInfo = new ApplicationDbContext.UserProfile
                    {
                        Id = Guid.NewGuid().ToString(),
                        FullName = user.FullName,
                        Sex = user.Sex,
                        PhoneNumber = user.PhoneNumber,
                        DateCreated = DateTime.Now,
                        Status = "Active"
                    }
                };

                var result = await UserManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(appUser.Id, "Admin");
                    
                    gVal.Message = "Your account was successfully set up. Please log in to continue.";
                    gVal.Code = 5;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                gVal.Message = result.Errors.ToList()[0];
                gVal.Code = -1;
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                gVal.Message = "An unknown error was encountered. Please try again later or contact support.";
                gVal.Code = -1;
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }
        }

        private bool SendSignupMail(AppUser model)
        {
            try
            {
                if (Request.Url != null)
                {
                    var appName = System.Configuration.ConfigurationManager.AppSettings["AplicationName"];
                    if (string.IsNullOrEmpty(appName))
                    {
                        return false;
                    }
                   
                    var xBody = "Thanks for signing up on " + appName + ". " +
                              "<br/>To activate your account, click the activate button or copy & paste the link below into your web browser <br/><br/>emailLink";

                    xBody = xBody.Replace("emailLink", "<a style=\"color:#099d77; cursor:pointer\" title=\"Activate Account\" href=" + Url.Action("ConfirmEmail", "Account", new { email = model.Email, code = model.SecurityStamp }, Request.Url.Scheme) + "> Click to Activate your account</a>");

                    var res =  _mailService.sendmail1(model.Email, "Welcome to " + appName, xBody);
                  
                    if (res)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.InnerException?.Message ?? ex.Message);
                return false;
            }
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmail(string email, string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(email))
                {
                    ViewBag.Message = "An unknown error was encountered. <br/>Please contact the Administrator.";
                    ViewBag.Code = -1;
                    return View();
                }

                var users = UserManager.Users.Where(u => u.Email == email && u.SecurityStamp == code).ToList();

                if (!users.Any())
                {
                    ViewBag.Message = "Your user account could not be verified. <br/>Please contact the Administrator.";
                    ViewBag.Code = -1;
                    return View();
                }

                var user = users[0];
                ViewBag.Code = 5;
                user.EmailConfirmed = true;
                UserManager.Update(user);

                ViewBag.Message = "Your Email was successfully confirmed and your account has been activated.";
                return View();
            }
            catch (Exception)
            {
                ViewBag.Message = "Your account could not be verified. <br/>Please try again or contact the Administrator";
                ViewBag.Code = -1;
                return View();
            }

        }
        
        public ActionResult RequestPasswordReset(string email)
        {
            var gVal = new ActivityResponse();
            try
            {
                var user = UserManager.FindByEmail(email);
                if (user == null)
                {
                    gVal.Code = -1;
                    gVal.Message = "This Email could not be found.";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (Request.Url != null)
                {
                    var xBody = "We received a request to reset the password for your Account." +
                    "<br/>If this Request was initiated by you, click the link below (or copy and paste the URL into your browser) to reset your password: <br/> " +
                    "{Link to reset password} <br/>" +
                    "If you don't want to reset your password, kindly ignore this message. Your password will not be reset.";

                    xBody = xBody.Replace("{Link to reset password}", "<a style=\"color:green; cursor:pointer\" title=\"Reset Password\" href=" + Url.Action("ConfirmPasswordReset", "Account", new { remail = email, code = UserManager.GeneratePasswordResetTokenAsync(user.Id).Result }, Request.Url.Scheme) + ">Confirm Password request</a>").Replace("\n", "<br/>");
                    
                    var appName = System.Configuration.ConfigurationManager.AppSettings["AplicationName"];

                    if (string.IsNullOrEmpty(appName))
                    {
                        gVal.Code = -1;
                        gVal.Message = "Your request could not be processed. Please try again.";
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }

                    var res = _mailService.sendmail1(email, "Password Reset Required Validation", xBody);

                    if (res)
                    {
                        gVal.Code = 5;
                        gVal.Message = "A password reset link has been sent to your email.";
                        return Json(gVal, JsonRequestBehavior.AllowGet);
                    }
                    gVal.Code = -1;
                    gVal.Message = "Your request could not be processed. Please try again.";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                gVal.Code = -1;
                gVal.Message = "Your request could not be processed. Please try again.";
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                gVal.Code = -1;
                gVal.Message = "Your request could not be processed. Please try again.";
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult ConfirmPasswordReset(string remail, string code)
        {
            var gVal = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(remail) || string.IsNullOrEmpty(code))
                {
                    ViewBag.Code = -2;
                    ViewBag.Message = "Your user account could not be verified. Please contact our support.";
                    return View();
                }

                var user = UserManager.FindByEmail(remail);
                if (user == null)
                {
                    ViewBag.Code = -2;
                    ViewBag.Message = "Your email could not be found. Please contact our support";
                    return View();
                }
                ViewBag.Email = user.Email;
                ViewBag.passCode = code;
                ViewBag.Code = 5;
                return View();
            }
            catch (Exception)
            {
                gVal.Code = -2;
                gVal.Message = "Your user account could not be verified. Please contact our support.";
                return View();
            }

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var gVal = new ActivityResponse();
            try
            {
                if (string.IsNullOrEmpty(model.Email))
                {
                    gVal.Code = -1;
                    gVal.Message = "An error was encountered. Pleae try again later or contact our support team.";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(model.Code))
                {
                    gVal.Code = -1;
                    gVal.Message = "An error was encountered. Pleae try again later or contact our support team.";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(model.Password))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please provide a new Password";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(model.ConfirmPassword))
                {
                    gVal.Code = -1;
                    gVal.Message = "Please Confirm your new Password.";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                if (model.Password != model.ConfirmPassword)
                {
                    gVal.Code = -1;
                    gVal.Message = "The Passwords do not match.";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }
               
                var users = UserManager.Users.Where(u => u.Email == model.Email).ToList();

                if (!users.Any())
                {
                    ViewBag.Message = "Your user account information could not be retreived. <br/>Please contact the support.";
                    gVal.Code = -1;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }
                
                var status = UserManager.ResetPasswordAsync(users[0].Id, model.Code, model.Password).Result;
                if (status != IdentityResult.Success)
                {
                    ViewBag.Message = "Your password could not be reset. Please try again or contact our support team.";
                    gVal.Code = -1;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                var updateUser = UserManager.Users.Where(u => u.Email == model.Email).ToList();
                if (!updateUser.Any())
                {
                    ViewBag.Message = "Your account password has been changed successfully but the changes could not be persisted on the remote server";
                    gVal.Code = -1;
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }
                var appName = ConfigurationManager.AppSettings["AplicationName"];

                if (string.IsNullOrEmpty(appName))
                {
                    gVal.Code = -1;
                    gVal.Message = "Your request could not be processed. Please try again.";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }

                var siteServerConnection = ConfigurationManager.AppSettings["siteServerConnection"];

                if (!string.IsNullOrEmpty(siteServerConnection))
                {
                    //sync password change
                    var table = new DataTable();
                    new MySqlDataAdapter($"SELECT Email from aspnetUsers WHERE ID='{model.Email.Trim()}'", siteServerConnection).Fill(table);

                    if (!(table.Rows.Count > 0))
                    {
                        using (var cn = new MySqlConnection(siteServerConnection))
                        {
                            var aspNetCmd = new MySqlCommand
                            {
                                Connection = cn,
                                CommandText = $"UPDATE aspnetUsers SET PasswordHash='{updateUser[0].PasswordHash}' WHERE Email = '{model.Email.Trim()}'"
                            };
                            aspNetCmd.ExecuteNonQuery();
                        }
                    }
                }

                var xBody = "Your account password has been changed successfully.<br/>" +
                                   "Don’t hesitate to contact us if you think your account has been compromised";
                var res = _mailService.sendmail1(model.Email, "Password Reset Successful", xBody);

                if (res)
                {
                    gVal.Code = 5;
                    gVal.Message = "Your account password has been changed successfully.";
                    return Json(gVal, JsonRequestBehavior.AllowGet);
                }
                gVal.Code = -1;
                gVal.Message = "Your request could not be processed. Please try again.";
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                gVal.Code = -1;
                gVal.Message = "Process failed. Please try again.";
                return Json(gVal, JsonRequestBehavior.AllowGet);
            }

        }
      
        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email};
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return Json(5, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, identity);
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
