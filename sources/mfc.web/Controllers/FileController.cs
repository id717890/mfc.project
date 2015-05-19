using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mfc.domain;
using mfc.domain.entities;
using mfc.domain.services;
using mfc.infrastructure.security;
using mfc.web.Models;
using mfc.web.Helpers;

namespace mfc.web.Controllers {
    public class FileController : Controller {
        //
        // GET: /File/

        public ActionResult Index(string date = null, Int64 user_id = -1, Int64 status_id = -1, Int64 org_id = -1) {
            var user_srv = CompositionRoot.Resolve<IUserService>();
            var status_srv = CompositionRoot.Resolve<IFileStatusService>();
            var org_srv = CompositionRoot.Resolve<IOrganizationService>();

            User user = null;
            FileStatus status = null;

            bool is_admin = User.IsInRole(Roles.Admin);

            //Если текущий пользователь не администратор, то тогда
            //устанавливаем его текущим
            if (!is_admin && user_id == -1) {
                user = user_srv.GetUser(User.Identity.Name);
            }
            else {
                user = user_srv.GetUserById(user_id);
            }

            status = status_srv.GetStatusById(status_id);
            var org = org_srv.GetOrganizationById(org_id);

            var queryDate = DateTime.Today;

            if (!string.IsNullOrEmpty(date)) {
                DateTime.TryParse(date, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out queryDate);
            }

            var model = CreateFileListModel(queryDate, user, status, org);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(DateTime date, Int64 selectedUserId = -1, Int64 selectedStatusId = -1, Int64 selectedOrgId = -1) {
            var user_srv = CompositionRoot.Resolve<IUserService>();
            var status_srv = CompositionRoot.Resolve<IFileStatusService>();
            var org_srv = CompositionRoot.Resolve<IOrganizationService>();

            var user = user_srv.GetUserById(selectedUserId);
            var status = status_srv.GetStatusById(selectedStatusId);
            var org = org_srv.GetOrganizationById(selectedOrgId);

            var model = CreateFileListModel(date, user, status, org);

            return View(model);
        }

        public ActionResult Edit(Int64 id) {
            var file_srv = CompositionRoot.Resolve<IFileService>();
            bool has_error = false;
            FileModel model = null;

            try {
                var file = file_srv.GetFileById(id);
                if (file == null) {
                    ModelState.AddModelError("", "Дело не найдено");
                    has_error = true;
                }
                else {
                    model = FileModelConverter.ToModel(file);

                    if (file.Controller != null) {
                        model.ControllerId = file.Controller.Id;
                    }
                }
            }
            catch (DomainException e) {
                ModelState.AddModelError("", e);
                has_error = true;
            }

            if (has_error) {
                return RedirectToAction("Index", new { date = model.Date.ToString("dd.MM.yyyy") });
            }

            PrepareForCreate();

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(FileModel model) {
            bool has_error = false;
            if (ModelState.IsValid) {
                var file_srv = CompositionRoot.Resolve<IFileService>();
                var user_srv = CompositionRoot.Resolve<IUserService>();

                var file = file_srv.GetFileById(model.Id);

                if (file != null) {
                    try {
                        file.Controller = user_srv.GetUserById(model.ControllerId);
                        file.Caption = model.Caption;

                        file_srv.Update(file);
                    }
                    catch (DomainException e) {
                        ModelState.AddModelError("", e);
                        has_error = true;
                    }

                    if (!has_error) {
                        return RedirectToAction("Index", new { date = model.Date.ToString("dd.MM.yyyy") });
                    }
                }
            }

            PrepareForCreate();

            return View(model);
        }


        #region Helpers
        
        private FileListViewModel CreateFileListModel(DateTime date, User selectedUser, FileStatus status, Organization org) {
            var user_srv = CompositionRoot.Resolve<IUserService>();
            var status_srv = CompositionRoot.Resolve<IFileStatusService>();


            FileListViewModel model = new FileListViewModel();
            model.Date = date;

            //Для администратора заполняем список всеми экспертами
            //Для эксперта - только им.
            if (User.IsInRole(Roles.Admin)) {
                foreach (var user in user_srv.GetControllers()) {
                    model.Controllers.Add(user);
                }
            }
            else {
                model.Controllers.Add(selectedUser);
            }

            model.Statuses.Add(FileStatus.All);

            foreach (var item in status_srv.GetAllStatuses()) {
                model.Statuses.Add(item);
            }

            var file_srv = CompositionRoot.Resolve<IFileService>();
            var orgs = new List<Organization>();

            //Пользователь, для данными которого заполняется список оказанных услуг
            //Если он не задан, то берем первого в списке
            User queryUser = selectedUser;

            if (queryUser == null && model.Controllers.Count > 0) {
                queryUser = model.Controllers[0];
            }

            if (queryUser != null) {
                model.SelectedUserId = queryUser.Id;

                foreach (var file in file_srv.GetFiles(queryUser, date)) {
                    model.Files.Add(file);

                    if (!orgs.Contains(file.Ogv)) {
                        orgs.Add(file.Ogv);
                    }
                }
            }

            model.Organizations.Add(Organization.All);

            foreach (var item in orgs.OrderBy(m => m.Caption)){
                model.Organizations.Add(item);
            }

            return model;
        }

        private void PrepareForCreate() {
            var usr_srv = CompositionRoot.Resolve<IUserService>();

            var users = new List<User>();

            if (User.IsInRole(Roles.Admin)) {
                users.AddRange(usr_srv.GetControllers());
            }
            else {
                users.Add(usr_srv.GetUser(User.Identity.Name));
            }

            ViewBag.Controllers = users;
        }

        #endregion
    }
}
