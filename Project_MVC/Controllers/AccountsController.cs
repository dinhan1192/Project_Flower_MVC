using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Project_MVC.App_Start;
using Project_MVC.Models;
using Project_MVC.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult Index()
        {
            return View();
        }

        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }
        //private RoleManager<AppRole> roleManager;
        private ApplicationUserManager _userManager;
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

        private ApplicationSignInManager _signInManager;
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

        //public AccountsController()
        //{
        //    //var roleStore = new RoleStore<AppRole>(dbContext);
        //    //roleManager = new RoleManager<AppRole>(roleStore);
        //    var userStore = new UserStore<AppUser>(dbContext);
        //    userManager = new UserManager<AppUser>(userStore);
        //}
        public AccountsController()
        {
        }

        private void Validate(AppUser appUser)
        {
            if (string.IsNullOrEmpty(appUser.FirstName))
            {
                ModelState.AddModelError("FirstName", "Phần tên không thể để trống");
            }

            if (string.IsNullOrEmpty(appUser.LastName))
            {
                ModelState.AddModelError("LastName", "Phần họ không thể để trống");
            }

            if (string.IsNullOrEmpty(appUser.UserName))
            {
                ModelState.AddModelError("UserName", "Tên tài khoản không thể để trống");
            }

            if (string.IsNullOrEmpty(appUser.Email))
            {
                ModelState.AddModelError("Email", "Email không thể để trống");
            }

            if (string.IsNullOrEmpty(appUser.Password))
            {
                ModelState.AddModelError("Password", "Mật khẩu không thể để trống");
            }

            if (string.IsNullOrEmpty(appUser.ConfirmPassword))
            {
                ModelState.AddModelError("ConfirmPassword", "Xác nhận mật khẩu không thể để trống");
            }
        }

        //public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        //{
        //    UserManager = userManager;
        //    SignInManager = signInManager;
        //}

        //public ApplicationSignInManager SignInManager
        //{
        //    get
        //    {
        //        return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        //    }
        //    private set
        //    {
        //        _signInManager = value;
        //    }
        //}

        [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
        //[Authorize]
        public ActionResult AddUserToRole(string accountId, string roleName)
        {
            UserManager.AddToRole(accountId, roleName);
            return Redirect("/Products/Index");
        }

        // GET: Accounts
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(AppUser user)
        {
            Validate(user);
            
            if (ModelState.IsValid)
            {
                var account = new AppUser()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    BirthDay = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                ViewBag.UserName = user.UserName;

                IdentityResult result = await UserManager.CreateAsync(account, user.Password);
                Debug.WriteLine("@@@" + result.Succeeded);
                if (result.Succeeded)
                {
                    //Debug.WriteLine("@@@" + account.Id);
                    UserManager.AddToRole(account.Id, "User");
                    //var authenticationManager = System.Web.HttpContext.Current
                    //    .GetOwinContext().Authentication;
                    //var userIdentity = UserManager.CreateIdentity(
                    //    account, DefaultAuthenticationTypes.ApplicationCookie);
                    //authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(account.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Accounts", new { userId = account.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(account.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(account.Id);
                    //await UserManager.SendEmailAsync(account.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"http://google.com.vn\">here</a>");

                    return View("PageWaitWhileRedirected");
                }
                AddErrors(result);
            }
            return View(user);
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(AppUser appUser, string returnUrl)
        {
            var user = UserManager.Find(appUser.UserName, appUser.Password);

            if (user != null && user.EmailConfirmed == true)
            {
                var authenticationManager = System.Web.HttpContext.Current
                    .GetOwinContext().Authentication;
                var userIdentity = UserManager.CreateIdentity(
                    user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                var lstRoleName = UserManager.GetRoles(user.Id);
                string roleName = null;
                roleName = roleName ?? (lstRoleName.Contains(Constant.Admin) ? Constant.Admin : null);
                roleName = roleName ?? (lstRoleName.Contains(Constant.Employee) ? Constant.Employee : null);

                switch (roleName)
                {
                    case Constant.Admin:
                        return Redirect("/Products/Index");
                    case Constant.Employee:
                        return Redirect("/Products/Index");
                    default:
                        return Redirect("/Home/Index");
                }

                //if (UserManager.IsInRole(user.Id, Constant.Admin) || UserManager.IsInRole(user.Id, Constant.Employee))
                //{
                //    return Redirect("/Products/Index");
                //} else
                //{
                //    return Redirect("/Products/IndexCustomer");
                //}
            }
            else if (user != null && user.EmailConfirmed == false)
            {
                ModelState.AddModelError("UserName", "Email chưa confirm");
            }
            else
            {
                ModelState.AddModelError("UserName", "UserName hoặc Password nhập sai");
            }

            //if (user.EmailConfirmed == false)
            //{
            //    ModelState.AddModelError("UserName", "Email chưa confirm");
            //}

            return View(appUser);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Accounts", new { ReturnUrl = returnUrl }));
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

            //var user = await UserManager.FindAsync(loginInfo.Login);
            //if (user == null)
            //{
            //    // Register the user. The user must be added to two tables using the built-in methods.
            //    //var email = loginInfo.DefaultUserName.ToLower().Replace("mydomain\\", "") + "@mydomain.org";  // The email was null on loginInfo, so using the DefaultUserName to build one
            //    var email = loginInfo.Email;
            //    var appUser = new AppUser { UserName = email, Email = email };
            //    var identityResult = await UserManager.CreateAsync(appUser);  // Adds user to AspNetUsers table
            //    if (identityResult.Succeeded)
            //    {
            //        identityResult = await UserManager.AddLoginAsync(appUser.Id, loginInfo.Login);    // Adds user to AspNetUserLogins
            //        if (!identityResult.Succeeded)
            //        {
            //            //await SignInManager.SignInAsync(appUser, isPersistent: false, rememberBrowser: false);
            //            return RedirectToLocal(returnUrl);
            //        }
            //    }
            //    else
            //    {

            //    }
            //}

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
                //case SignInStatus.Failure:
                //    return View("ExternalLoginFailure");
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
                var user = new AppUser { UserName = model.Email, Email = model.Email, Gender = AppUser.GenderStatus.Other };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "User");
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
                    return View("ExternalLoginFailure");
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }


        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                var authenticationManager = System.Web.HttpContext.Current
                    .GetOwinContext().Authentication;
                var account = DbContext.Users.Find(userId);
                var userIdentity = UserManager.CreateIdentity(
                    account, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                return View("ConfirmEmail");
            }
            return View("Error");
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(AppUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.UserName);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Accounts", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Accounts");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string userId, string code)
        {
            var user = DbContext.Users.Find(userId);
            user.CodeToResetPassword = code;
            return code == null ? View("Error") : View(user);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(AppUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Accounts");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.CodeToResetPassword, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Accounts");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            var authenticationManager = System.Web.HttpContext.Current
                .GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Login", "Accounts");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        #region Helpers

        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("IndexCustomer", "Products");
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