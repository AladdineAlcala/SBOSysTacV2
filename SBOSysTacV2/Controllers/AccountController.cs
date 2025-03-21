﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Controllers
{

    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ApplicationUser.ApplicationDbContext context=new ApplicationUser.ApplicationDbContext();
        
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
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

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: true);


            switch (result)
            {
                case SignInStatus.Success:

                    var isauthenticated = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;

                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
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

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return Json(new {success = true}, JsonRequestBehavior.AllowGet);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return PartialView("NewUser_PartialView", model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
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
       // [AllowAnonymous]
        //public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        //{
        //    var userId = await SignInManager.GetVerifiedUserIdAsync();
        //    if (userId == null)
        //    {
        //        return View("Error");
        //    }
        //    var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
        //    var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
        //    return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //}

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
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
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
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [Authorize(Roles = "superadmin")]
        public ActionResult UsersIndex()
        {

            return View();
        }


        public ActionResult GetAllUsers()
        {
            UsersViewModel users=new UsersViewModel();

            List<UsersViewModel> listofUsers=new List<UsersViewModel>();

            try
            {
                listofUsers = users.listofUsers().ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return Json(new {data= listofUsers}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult NewUser_PartialView()
        {


            return PartialView("NewUser_PartialView");
        }

        [HttpGet]
        public ActionResult UserperRole(string id)
        {
            UserinRoleViewModel user=new UserinRoleViewModel();

            var userRole = UserManager.GetRoles(id);
            string roleName = userRole.Count() == 0 ? "user" : userRole[0];

            var roleList = RoleManager.Roles.ToList();

            var userinrole = user.GetUsersinRole().FirstOrDefault(u => u.userId == id);
            userinrole.selectListRoles = roleList.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.Name,
                Selected = s.Name == roleName ? true : false
            });
            return PartialView("UserRoles_PartialView", userinrole);
        }


      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveRole_to_User(UserinRoleViewModel userinRole)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("");
            }
            bool success = false;
            var currentUser = UserManager.FindById(userinRole.userId);
            var rolename = RoleManager.Roles.First(r => r.Id == userinRole.selecteduserRole).Name;
            //if (currentUser.Roles.Any(x => x.RoleId == userinRole.selecteduserRole))
            //{

            //}
            //else
            //{
               
                if (rolename != null)
                {
                    var roleresult = UserManager.AddToRole(currentUser.Id, rolename);

                    if (roleresult.Succeeded)
                    {
                        success = true;
                    }
                }
               

            //}
            return Json(new {success=success }, JsonRequestBehavior.AllowGet);
        }

        //POST:/Remove User
        [HttpPost,ActionName("DeleteUser")]
        public async Task<ActionResult> RemoveUser(string id)
        {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var user = await UserManager.FindByIdAsync(id);
                var logins = user.Logins;
                var rolesforUser = await UserManager.GetRolesAsync(id);

                using (var transaction = context.Database.BeginTransaction())
                {
                    //IdentityResult result = IdentityResult.Success;

                    foreach (var login in logins)
                    {
                        await UserManager.RemoveLoginAsync(login.UserId,
                            new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                    }

                    if (rolesforUser.Any())
                    {
                        foreach (var item in rolesforUser.ToList())
                        {
                            var result = await UserManager.RemoveFromRoleAsync(user.Id, item);
                        }
                    }

                    await UserManager.DeleteAsync(user);
                    transaction.Commit();
                }
                //return RedirectToAction("UsersIndex");
                return Json(new {success = true}, JsonRequestBehavior.AllowGet);
         
        }

        //[HttpGet]
        [ChildActionOnly]
        public ActionResult getUserLogInIdentity()
        {
            var userId = User.Identity.GetUserId();
            UsersViewModel u = new UsersViewModel();

            var userslog = (from v in u.listofUsers().Where(x => x.userId == userId)
                select new UsersViewModel()
                {
                    userId = v.userId,
                    username = v.username,
                    roles = v.roles,
                    has_superadminRights = v.has_superadminRights
                }).FirstOrDefault();

            return PartialView("_logInUserPartial", userslog);

        }

        //POST:/Account/RemoveRoleinUser
        //[ActionName("DeleteRole_per_User")]
        public ActionResult RemoveUsersRole(string user,int roleId)
        {
            bool success = false;
            if (UserManager.IsInRole(user, "admin"))
            {
                string _roleid = Convert.ToString(roleId).Trim();

                var rolename = RoleManager.FindById(_roleid);

                var result = UserManager.RemoveFromRole(user, rolename.Name);

                success = true;
            }


            return Json(new {success=success }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetProfileInformation(string userId)
        {
 
            var context=new ApplicationUser.ApplicationDbContext();

           var userprofile = context.Users.FirstOrDefault(u => u.Id == userId);

            UserProfileViewModel userProfile = new UserProfileViewModel();
            //ViewBag.userRoles = string.Join(",", UserManager.GetRoles(userId));
            if (userprofile != null)
            {
                userProfile = new UserProfileViewModel()
                {
                    uId = userprofile.Id,
                    username = userprofile.UserName,
                    emailAdd = userprofile.Email,
                    fullname = Utilities.Getfullname_nonreverse(userprofile.Lastname,userprofile.Firstname,userprofile.Middle),
                    roles = string.Join(",", UserManager.GetRoles(userId))

                };
            }

          

            return View(userProfile);
        }

        [HttpGet,ChildActionOnly]
        public ActionResult ChangePassPartialView(string user_id)
        {
            var userUpdatePass = new UserUpdatePassViewModel();

            userUpdatePass.userId = user_id;

            return PartialView("_changepassPartialView", userUpdatePass);
        }



        [HttpPost]
        public ActionResult ChangePassword(UserUpdatePassViewModel _userProfileModel)
        {
            if (ModelState.IsValid)
            {
                var userId = _userProfileModel.userId;

                if (UserManager.CheckPassword(UserManager.FindById(userId), _userProfileModel.oldPass))
                {
                    if (_userProfileModel.newPass == _userProfileModel.confirmnewPass)
                    {
                        UserManager.RemovePassword(userId);
                        UserManager.AddPassword(userId, _userProfileModel.newPass);

                        var _url = Url.Action("ChangePassPartialView", "Account", new {user_id = userId});

                        return Json(new {success = true, url = _url}, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                     
                        return Json(new { success = false, errmsg = "invalid password confirmation" },
                            JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {

                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);


                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }


   

        //
        // GET: /Account/ExternalLoginFailure
        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

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

                if (_roleManager != null)
                {
                    _roleManager.Dispose();
                    _roleManager = null;
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