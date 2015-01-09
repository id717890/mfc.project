using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using mfc.web.Models;
using mfc.infrastructure.services;
using Ninject;
using mfc.domain.services;
using mfc.domain;
using System.Web.Security;

namespace mfc.web.Controllers {
    [Authorize]
    public class AccountController : Controller {
        //
        // GET: /Account/Login

        [Inject]
        public IIdentifierService IdService { get; set; }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl) {
            var membership_service = CompositionRoot.Resolve<IMembershipService>();
            var user_service = CompositionRoot.Resolve<IUserService>();

            if (ModelState.IsValid && membership_service.IsUserValid(model.UserName, model.Password)) {
                
                var user = user_service.GetUser(model.UserName);
                if (user != null) {
                    var authTicket = new FormsAuthenticationTicket(
                        1,                             // version
                        model.UserName,                      // user name
                        DateTime.Now,                  // created
                        DateTime.Now.AddMinutes(20),   // expires
                        true,                    // persistent?
                        user.IsAdmin ? mfc.infrastructure.security.Roles.Admin : mfc.infrastructure.security.Roles.Expert
                        );

                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    HttpContext.Response.Cookies.Add(authCookie);

                                        //FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToLocal(returnUrl);
                }
            }
            
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Введенные имя пользователя или пароль не верны");

            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff() {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }


        public ActionResult Manage() {
            bool has_error = false;

            var user_service = CompositionRoot.Resolve<IUserService>();
            var user = user_service.GetUser(User.Identity.Name);

            if (user == null) {
                ModelState.AddModelError("", "Пользователь не найден");
                has_error = true;
            }

            if (has_error) {
                return RedirectToAction("Index", "Home");
            }

            var password = new PasswordModel {
                UserId = user.Id
            };

            return View(password);
            //ViewBag.ReturnUrl = Url.Action("Manage");
            //return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(PasswordModel model) {
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (ModelState.IsValid) {
                bool has_error = false;

                var user_service = CompositionRoot.Resolve<IUserService>();
                try {
                    user_service.SetPassword(model.UserId, model.NewPassword);
                }
                catch (DomainException e) {
                    ModelState.AddModelError("", e);
                    has_error = true;
                }

                if (!has_error) {
                    ViewBag.StatusMessage = "Пароль успешно сменен";
                }
            }
            return View();
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus) {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus) {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
