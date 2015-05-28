﻿using System;
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
        public ActionResult Index(Int64 controllerId = -1, Int64 expertId = -1, Int64 statusId = -1, Int64 orgId = -1) {
            return View(CreateModel(controllerId, expertId, statusId, orgId));
        }

        [HttpPost]
        public ActionResult Index(FileListViewModel model) {
            var new_model = model;
            if (ModelState.IsValid) {
                model = CreateModel(model.SelectedControllerId, model.SelectedExpertId, model.SelectedStatusId, model.SelectedOgvId);
            }

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

        public ActionResult Control(Int64 fileId) {
            var file_srv = CompositionRoot.Resolve<IFileService>();
            bool has_error = false;
            FileControlModel model = new FileControlModel();

            model.FileId = fileId;

            PrepareForCreate();

            return View(model);
        }

        [HttpPost]
        public ActionResult Control(FileControlModel model) {
            bool has_error = false;
            if (ModelState.IsValid) {
                var file_srv = CompositionRoot.Resolve<IFileService>();

                try {
                    file_srv.SendForControl(model.FileId, model.ControllerId, model.Comments);
                }
                catch (DomainException e) {
                    ModelState.AddModelError("", e);
                    has_error = true;
                }

                if (!has_error) {
                    return RedirectToAction("Edit", new { id = model.FileId });
                }
            }

            PrepareForCreate();

            return View(model);
        }

        public ActionResult Return(Int64 fileId) {
            var file_srv = CompositionRoot.Resolve<IFileService>();
            bool has_error = false;
            FileReturnModel model = new FileReturnModel();

            model.FileId = fileId;

            PrepareForCreate();

            return View(model);
        }

        [HttpPost]
        public ActionResult Return(FileReturnModel model) {
            bool has_error = false;
            if (ModelState.IsValid) {
                var file_srv = CompositionRoot.Resolve<IFileService>();

                try {
                    file_srv.Return(model.FileId, model.ExpertId, model.Comments);
                }
                catch (DomainException e) {
                    ModelState.AddModelError("", e);
                    has_error = true;
                }

                if (!has_error) {
                    return RedirectToAction("Edit", new { id = model.FileId });
                }
            }

            PrepareForCreate();

            return View(model);
        }

        public ActionResult Check(Int64 fileId) {
            var file_srv = CompositionRoot.Resolve<IFileService>();
            bool has_error = false;
            FileModel model = null;

            try {
                var file = file_srv.GetFileById(fileId);
                if (file == null) {
                    ModelState.AddModelError("", "Дело не найдено");
                    has_error = true;
                }
                else {
                    model = FileModelConverter.ToModel(file);
                }
            }
            catch (DomainException e) {
                ModelState.AddModelError("", e);
                has_error = true;
            }

            if (has_error) {
                return RedirectToAction("Edit", new { id = fileId });
            }

            PrepareForCreate();

            return View(model);
        }

        [HttpPost]
        public ActionResult Check(FileModel model) {
            bool has_error = false;
            if (ModelState.IsValid) {
                var file_srv = CompositionRoot.Resolve<IFileService>();
                var user_srv = CompositionRoot.Resolve<IUserService>();

                var file = file_srv.GetFileById(model.Id);

                if (file != null) {
                    try {
                        file_srv.Checked(model.Id, string.Empty);
                    }
                    catch (DomainException e) {
                        ModelState.AddModelError("", e);
                        has_error = true;
                    }

                    if (!has_error) {
                        return RedirectToAction("Edit", new { id = model.Id});
                    }
                }
            }

            PrepareForCreate();

            return View(model);
        }

        public ActionResult FileStatuses(Int64 id) {
            var srv = CompositionRoot.Resolve<IFileStatusService>();
            
            var items = new List<FileStatusInfoModel>();
            foreach (var item in srv.GetFileStatuses(id)) {
                items.Add(new FileStatusInfoModel {
                    Date = item.Date.ToString("dd.MM.yyyy HH:mm:ss"),
                    User = item.User.Name,
                    Status = item.Status.Caption,
                    Comments = item.Comments
                });
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }


        #region Helpers

        private FileListViewModel CreateModel(Int64 controllerId = -1, Int64 expertId = -1, Int64 statusId = -1, Int64 orgId = -1) {
            var user_srv = CompositionRoot.Resolve<IUserService>();
            var status_srv = CompositionRoot.Resolve<IFileStatusService>();
            var org_srv = CompositionRoot.Resolve<IOrganizationService>();
            var file_srv = CompositionRoot.Resolve<IFileService>();

            FileListViewModel model = new FileListViewModel();

            model.SelectedOgvId = orgId == -1 ? Organization.All.Id : orgId;
            model.SelectedControllerId = controllerId == -1 ? mfc.domain.entities.User.All.Id : controllerId;
            model.SelectedExpertId = expertId == -1 ? mfc.domain.entities.User.All.Id : expertId;
            model.SelectedStatusId = statusId == -1 ? FileStatus.All.Id : statusId;

            if (User.IsInRole(Roles.Admin)) {
                //Для администратора заполняем список всеми экспертами и контролерами
                foreach (var user in user_srv.GetControllers()) {
                    model.Controllers.Add(user);
                }
                model.Controllers.Insert(0, mfc.domain.entities.User.All);

                foreach (var user in user_srv.GetExperts()) {
                    model.Experts.Add(user);
                }
                model.Experts.Insert(0, mfc.domain.entities.User.All);
            }
            else if (User.IsInRole(Roles.Controller) && User.IsInRole(Roles.Expert)) {
                //Если пользователь и контролер и эксперт одновремено, то вносим в список только его
                var user = user_srv.GetCurrentUser();

                model.Controllers.Add(user);
                model.Experts.Add(user);

                model.SelectedExpertId = model.SelectedControllerId = user.Id;
            }
            else if (User.IsInRole(Roles.Controller)) {
                //если только контролер, то полный список экспертов и один контролер
                var user = user_srv.GetCurrentUser();

                model.Controllers.Add(user_srv.GetCurrentUser());

                model.Experts.Add(mfc.domain.entities.User.All);
                foreach (var expert in user_srv.GetExperts()) {
                    model.Experts.Add(expert);
                }

                model.SelectedControllerId = user.Id;
            }
            else if (User.IsInRole(Roles.Expert)) {
                //если только эксперт, то полный список экспертов и один контролер
                var user = user_srv.GetCurrentUser();

                model.Experts.Add(user_srv.GetCurrentUser());

                model.Controllers.Add(mfc.domain.entities.User.All);
                foreach (var controller in user_srv.GetControllers()) {
                    model.Controllers.Add(controller);
                }

                model.SelectedExpertId = user.Id;
            }

            model.Statuses.Add(FileStatus.All);

            foreach (var item in status_srv.GetAllStatuses()) {
                model.Statuses.Add(item);
            }

            model.Organizations.Add(Organization.All);

            foreach (var item in org_srv.GetAllOrganizations()) {
                model.Organizations.Add(item);
            }

            


            foreach (var file in file_srv.GetFiles(model.SelectedControllerId, model.SelectedExpertId, model.SelectedStatusId, model.SelectedOgvId)) {
                model.Files.Add(file);
            }

            return model;
        }

        private void PrepareForCreate() {
            var usr_srv = CompositionRoot.Resolve<IUserService>();
            ViewBag.Controllers = new List<User>(usr_srv.GetControllers());
            ViewBag.Experts = new List<User>(usr_srv.GetExperts()); ;
        }

        #endregion
    }
}
