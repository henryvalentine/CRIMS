using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Host.SystemWeb;
using Crims.API.Models;
using Crims.Data.Models;
using Crims.UI.Web;
using Crims.UI.Web.Models;
using Microsoft.Owin;
using Newtonsoft.Json;
using ApplicationUser = Crims.API.Models.ApplicationUser;
using ErrorLogger = Crims.UI.Web.Helpers.ErrorLogger;
using MySql.Data.MySqlClient;

namespace Crims.API.Controllers
{
    
    [System.Web.Http.RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

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
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
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
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [System.Web.Http.Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [System.Web.Http.Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [System.Web.Http.Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// <summary>
        /// Authentication via Api.
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("ApiLogin")]
        public UserModel ApiLogin()
        {
            var userModel = new UserModel();
            try
            {
                var credentials = Request.Content.ReadAsFormDataAsync().Result;
                var passwords = credentials.GetValues("password"); 
                if (passwords == null)
                {
                    userModel.Code = -1;
                    userModel.Message = "Please provide your Password";
                    return userModel;
                    
                }
                var password = passwords[0];

                var emails = credentials.GetValues("email"); 
                if (emails == null)
                {
                    userModel.Code = -1;
                    userModel.Message = "Please provide your Email";
                    return userModel;
                }

                var email = emails[0];
              
                if (!email.Contains("@"))
                {
                    userModel.Code = -1;
                    userModel.Message = "Please provide a valid Email";
                    return userModel;
                }

                if (!email.Contains("."))
                {
                    userModel.Code = -1;
                    userModel.Message = "Please provide a valid Email";
                    return userModel;
                }

                if (string.IsNullOrEmpty(password))
                {
                    userModel.Code = -1;
                    userModel.Message = "Please provide your Password";
                    return userModel;
                }

                var appUsers = new List<UserModel>();
                var connStr = ConfigurationManager.ConnectionStrings["crimsDbEntities"].ConnectionString; ;
                var conn = new MySqlConnection(connStr);
                conn.Open();

                var sql =
                    $"SELECT t.*, o.* FROM aspnetusers t JOIN userprofiles o ON o.Id = t.UserInfo_Id WHERE t.Email = '{email}'";

                var cmd = new MySqlCommand(sql, conn);
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        if (!rdr.HasRows) continue;
                        var dateRegistered = Convert.ToDateTime(rdr["DateCreated"].ToString());

                        var newUser = new UserModel
                        {
                            UserId = rdr["Id"].ToString(),
                            FullName = rdr["FullName"].ToString(),
                            Email = rdr["Email"].ToString(),
                            PhoneNumber = rdr["PhoneNumber"].ToString(),
                            Sex = rdr["Sex"].ToString(),
                            //Role = UserManager.GetRoles(rdr["Id"].ToString()).ToList()[0].Replace("_", " "),
                            ProfileId = rdr["UserInfo_Id"].ToString(),
                            DateCreated = dateRegistered,
                            Status = rdr["Status"].ToString(),
                            Hash = rdr["PasswordHash"].ToString()
                        };

                        appUsers.Add(newUser);
                    }
                }

                if (!appUsers.Any())
                {
                    userModel.Code = -1;
                    userModel.Message = "The email provided is wrong";
                    return userModel;
                }

                var user = appUsers[0];

                var hasher = new PasswordHasher();
                var passwordVerificationResult = hasher.VerifyHashedPassword(user.Hash, password);

                if (passwordVerificationResult == PasswordVerificationResult.Failed)
                {
                    userModel.Code = -1;
                    userModel.Message = "Wrong Password";
                    return userModel;
                }

                userModel = user;
                userModel.Code = 5;
                userModel.Message = "Login was successful";
                return userModel;
            }

            catch (Exception ex)
            {
                userModel.Code = -1;
                userModel.Message = "Login attempt failed. Please try again or contact our support";
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return userModel;
            }
        }
        
        [System.Web.Http.HttpGet]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("UserRecords")]
        public List<UserProfile> UserRecords()
        {
            try
            {
                //var users = UserManager.Users.Include(t => t.Roles).Include(t => t.UserInfo).ToList();

                var appUsers = new List<UserProfile>();
                var connStr = ConfigurationManager.ConnectionStrings["crimsDbEntities"].ConnectionString; ;
                var conn = new MySqlConnection(connStr);
                conn.Open();

                var sql = $"SELECT t.*, o.*, r.* FROM aspnetusers t JOIN userprofiles o ON o.Id = t.UserInfo_Id JOIN aspnetuserroles r ON t.Id = r.UserId";
                
                var cmd = new MySqlCommand(sql, conn);
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        if (!rdr.HasRows) continue;
                        var dateRegistered = Convert.ToDateTime(rdr["DateCreated"].ToString());
                        var lockoutEndDate = rdr["LockoutEndDateUtc"].ToString();
                        var lockoutEndDateUtc = !string.IsNullOrEmpty(lockoutEndDate) ? Convert.ToDateTime(lockoutEndDate) : new DateTime();
                        var accessFailedCount = rdr["AccessFailedCount"].ToString();
                        var lockOutEnabled = rdr["LockoutEnabled"].ToString();
                        var twoFactorEnabled = rdr["TwoFactorEnabled"].ToString();
                        var phoneNumberConfirmed = rdr["PhoneNumberConfirmed"].ToString();
                        var emailConfirmed = rdr["EmailConfirmed"].ToString();

                        var userid = rdr["Id"].ToString();
                        var newUser = new UserProfile
                        {
                            Id = rdr["UserInfo_Id"].ToString(),
                            FullName = rdr["FullName"].ToString(),
                            PhoneNumber = rdr["PhoneNumber"].ToString(),
                            Sex = rdr["Sex"].ToString(),
                            DateCreated = dateRegistered,
                            Status = rdr["Status"].ToString(),
                            AspNetUser = new AspNetUser
                            {
                                Id = userid,
                                PasswordHash = rdr["PasswordHash"].ToString(),
                                Email = rdr["Email"].ToString(),
                                UserInfo_Id = rdr["UserInfo_Id"].ToString(),
                                LockoutEndDateUtc = lockoutEndDateUtc,
                                SecurityStamp = rdr["SecurityStamp"].ToString(),
                                PhoneNumber = rdr["PhoneNumber"].ToString(),
                                EmailConfirmed = Convert.ToBoolean(emailConfirmed),
                                PhoneNumberConfirmed = Convert.ToBoolean(phoneNumberConfirmed),
                                TwoFactorEnabled = Convert.ToBoolean(twoFactorEnabled),
                                LockoutEnabled = Convert.ToBoolean(lockOutEnabled),
                                AccessFailedCount = string.IsNullOrEmpty(accessFailedCount) && accessFailedCount != "0" ? Convert.ToInt32(accessFailedCount) : 0,
                                UserName = rdr["UserName"].ToString(),
                                AspNetUserRole = new AspNetUserRole
                                {
                                    RoleId = rdr["RoleId"].ToString(),
                                    UserId = userid
                                },
                            }
                        };

                        appUsers.Add(newUser);
                    }
                }
                
                if (!appUsers.Any())
                {
                    return new List<UserProfile>();
                }
                
                return appUsers;
            }

            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return new List<UserProfile>();
            }
        }
        // POST api/Account/SetPassword
        [System.Web.Http.Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [System.Web.Http.Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [System.Web.Http.Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        //[OverrideAuthentication]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        //[AllowAnonymous]
        //[Route("ExternalLogin", Name = "ExternalLogin")]
        //public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        //{
        //    if (error != null)
        //    {
        //        return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
        //    }

        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return new ChallengeResult(provider, this);
        //    }

        //    ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

        //    if (externalLogin == null)
        //    {
        //        return InternalServerError();
        //    }

        //    if (externalLogin.LoginProvider != provider)
        //    {
        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //        return new ChallengeResult(provider, this);
        //    }

        //    ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
        //        externalLogin.ProviderKey));

        //    bool hasRegistered = user != null;

        //    if (hasRegistered)
        //    {
        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
        //         ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
        //            OAuthDefaults.AuthenticationType);
        //        ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
        //            CookieAuthenticationDefaults.AuthenticationType);

        //        AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
        //        Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
        //    }
        //    else
        //    {
        //        IEnumerable<Claim> claims = externalLogin.GetClaims();
        //        ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
        //        Authentication.SignIn(identity);
        //    }

        //    return Ok();
        //}

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RegisterExternal
        [System.Web.Http.OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result); 
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
