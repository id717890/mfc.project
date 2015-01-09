using mfc.domain;
using mfc.domain.services;
using mfc.infrastructure.security;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mfc.web.Controllers {
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller {
        public ActionResult Index() {
            var user_service = CompositionRoot.Resolve<IUserService>();
            var users = new List<AccountInfo>();

            foreach (var user in user_service.GetAllUsers()) {
                users.Add(new AccountInfo {
                    Id = user.Id,
                    UserName = user.Account,
                    Description = user.Name
                });
            }

            return View(users);
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccountInfo account) {
            bool has_error = false;
            if (ModelState.IsValid) {
                var user_service = CompositionRoot.Resolve<IUserService>();

                if (user_service.IsUserExists(account.UserName)) {
                    ModelState.AddModelError("", "Пользователь с таким именем уже существует");
                    has_error = true;
                }

                if (!has_error) {
                    try {
                        user_service.AddNew(account.UserName, account.Description, account.IsAdmin);
                    }
                    catch (DomainException e) {
                        ModelState.AddModelError("", e);
                        has_error = true;
                    }
                }

                if (!has_error) {
                    return RedirectToAction("Index", "Admin");
                }
            }

            return View();
        }

        public ActionResult Edit(Int64 id) {
            var user_service = CompositionRoot.Resolve<IUserService>();
            var user = user_service.GetUserById(id);

            if (user == null) {
                ModelState.AddModelError("", "Пользователь не найден");
                return RedirectToAction("Index", "Admin");
            }

            var account_info = UserToModelConveter.ModelFromUser(user);

            return View(account_info);
        }

        [HttpPost]
        public ActionResult Edit(AccountInfo account) {
            if (ModelState.IsValid) {
                bool has_error = false;

                var user_service = CompositionRoot.Resolve<IUserService>();

                var user = user_service.GetUser(account.UserName);

                if (user != null && user.Id != account.Id) {
                    ModelState.AddModelError("", "Пользователь с таким именем уже существует");
                    has_error = true;
                }

                if (!has_error) {
                    try {
                        user = UserToModelConveter.UserFromModel(account);
                        user_service.Update(user);
                    }
                    catch (DomainException e) {
                        ModelState.AddModelError("", e);
                        has_error = true;
                    }
                }

                if (!has_error) {
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();
        }

        public ActionResult Delete(Int64 id) {
            var user_service = CompositionRoot.Resolve<IUserService>();
            var user = user_service.GetUserById(id);

            if (user == null) {
                ModelState.AddModelError("", "Пользователь не найден");
                return RedirectToAction("Index", "Admin");
            }

            var account_info = UserToModelConveter.ModelFromUser(user);

            return View(account_info);
        }

        [HttpPost]
        public ActionResult Delete(AccountInfo account) {
            if (ModelState.IsValid) {
                bool has_error = false;

                var user_service = CompositionRoot.Resolve<IUserService>();
                try {
                    user_service.Delete(account.Id);
                }
                catch (DomainException e) {
                    ModelState.AddModelError("", e);
                    has_error = true;
                }

                if (!has_error) {
                    return RedirectToAction("Index", "Admin");
                }
            }

            return View();
        }

        public ActionResult SetPassword(Int64 id) {
            var password = new PasswordModel {
                UserId = id
            };

            return View(password);
        }

        [HttpPost]
        public ActionResult SetPassword(PasswordModel password) {
            if (ModelState.IsValid) {
                bool has_error = false;

                var user_service = CompositionRoot.Resolve<IUserService>();
                try {
                    user_service.SetPassword(password.UserId, password.NewPassword);
                }
                catch (DomainException e) {
                    ModelState.AddModelError("", e);
                    has_error = true;
                }

                if (!has_error) {
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();
        }

        private static class UserToModelConveter {
            public static mfc.domain.entities.User UserFromModel(AccountInfo account) {
                return new mfc.domain.entities.User {
                    Id = account.Id,
                    Account = account.UserName,
                    Name = account.Description,
                    IsAdmin = account.IsAdmin
                };
            }

            public static AccountInfo ModelFromUser(mfc.domain.entities.User user) {
                return new AccountInfo {
                    Id = user.Id,
                    UserName = user.Account,
                    Description = user.Name,
                    IsAdmin = user.IsAdmin
                };
            }
        }
    }
}