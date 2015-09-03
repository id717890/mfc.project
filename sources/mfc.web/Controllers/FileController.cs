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
using mfc.web.Abstracts;

namespace mfc.web.Controllers {
    public class FileController : BaseController {
        private const string FileControllerIndexKey = "FileControllerIndexKey";
        private const string ControllerKey = "Controller";
        private const string ExpertKey = "Expert";
        private const string OrgKey = "Org";
        private const string StatusKey = "Status";
        private const string DateBeginKey = "DateBegin";
        private const string DateEndKey = "DateEnd";
        //
        // GET: /File/
        public ActionResult Index(string beginDate = null, string endDate = null, Int64 controllerId = -1, Int64 expertId = -1, Int64 statusId = -1, Int64 orgId = -1) {
            DateTime queryDateBegin = DateTime.Today;
            queryDateBegin = new DateTime(queryDateBegin.Year, queryDateBegin.Month, 1);

            if (!string.IsNullOrEmpty(beginDate)) {
                DateTime.TryParse(beginDate, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out queryDateBegin);
            }

            DateTime queryDateEnd = queryDateBegin.AddMonths(1).AddSeconds(-1);

            if (!string.IsNullOrEmpty(endDate)) {
                DateTime.TryParse(endDate, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out queryDateEnd);
            }
            //загружаем настройки фильтра
            var settings = Session[FileControllerIndexKey] as IDictionary<string, object>;
                
            if (settings != null) {
                if (settings.ContainsKey(DateBeginKey)) {
                    queryDateBegin = (DateTime)settings[DateBeginKey];
                }

                if (settings.ContainsKey(DateEndKey)) {
                    queryDateEnd = (DateTime)settings[DateEndKey];
                }
                if (settings.ContainsKey(ControllerKey)) {
                    controllerId = (Int64)settings[ControllerKey];
                }
                if (settings.ContainsKey(ExpertKey)) {
                    expertId = (Int64)settings[ExpertKey];
                }
                if (settings.ContainsKey(OrgKey)) {
                    orgId = (Int64)settings[OrgKey];
                }
                if (settings.ContainsKey(StatusKey)) {
                    statusId = (Int64)settings[StatusKey];
                }
            }
            
            return View(CreateModel(queryDateBegin, queryDateEnd, controllerId, expertId, statusId, orgId));
        }

        [HttpPost]
        public ActionResult Index(FileListViewModel model) {
            var new_model = model;
            if (ModelState.IsValid) {
                //сохраняем настройки фильтра
                Session[FileControllerIndexKey] = new Dictionary<string, object>();
                var settings = Session[FileControllerIndexKey] as IDictionary<string, object>;
                if (!settings.ContainsKey(DateBeginKey)) {
                    settings.Add(DateBeginKey, model.BeginDate);
                }
                else {
                    settings[DateBeginKey] = model.BeginDate;
                }

                if (!settings.ContainsKey(DateEndKey)) {
                    settings.Add(DateEndKey, model.EndDate);
                }
                else {
                    settings[DateEndKey] = model.EndDate;
                }

                if (!settings.ContainsKey(ControllerKey)) {
                    settings.Add(ControllerKey, model.SelectedControllerId);
                }
                else {
                    settings[ControllerKey] = model.SelectedControllerId;
                }

                if (!settings.ContainsKey(ExpertKey)) {
                    settings.Add(ExpertKey, model.SelectedExpertId);
                }
                else {
                    settings[ExpertKey] = model.SelectedExpertId;
                }

                if (!settings.ContainsKey(OrgKey)) {
                    settings.Add(OrgKey, model.SelectedOgvId);
                }
                else {
                    settings[OrgKey] = model.SelectedOgvId;
                }

                if (!settings.ContainsKey(StatusKey)) {
                    settings.Add(StatusKey, model.SelectedStatusId);
                }
                else {
                    settings[StatusKey] = model.SelectedStatusId;
                }

                model = CreateModel(model.BeginDate, model.EndDate, model.SelectedControllerId, model.SelectedExpertId, model.SelectedStatusId, model.SelectedOgvId);
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
                    file_srv.SendForControl(model.FileId, model.Comments);
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
                    file_srv.Return(model.FileId, model.Comments);
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

        private FileListViewModel CreateModel(DateTime beginDate, DateTime endDate, Int64 controllerId = -1, Int64 expertId = -1, Int64 statusId = -1, Int64 orgId = -1) {
            var user_srv = CompositionRoot.Resolve<IUserService>();
            var status_srv = CompositionRoot.Resolve<IFileStatusService>();
            var org_srv = CompositionRoot.Resolve<IOrganizationService>();
            var file_srv = CompositionRoot.Resolve<IFileService>();

            FileListViewModel model = new FileListViewModel();

            model.SelectedOgvId = orgId == -1 ? Organization.All.Id : orgId;
            model.SelectedControllerId = controllerId == -1 ? mfc.domain.entities.User.All.Id : controllerId;
            model.SelectedExpertId = expertId == -1 ? mfc.domain.entities.User.All.Id : expertId;
            model.SelectedStatusId = statusId == -1 ? FileStatus.All.Id : statusId;
            model.BeginDate = beginDate;
            model.EndDate = endDate;

            if (User.IsInRole(Roles.Admin) || User.IsInRole(Roles.Controller)) {
                //Для администратора и контролера заполняем список всеми экспертами и контролерами
                foreach (var user in user_srv.GetControllers()) {
                    model.Controllers.Add(user);
                }
                model.Controllers.Insert(0, mfc.domain.entities.User.All);

                foreach (var user in user_srv.GetExperts()) {
                    model.Experts.Add(user);
                }
                model.Experts.Insert(0, mfc.domain.entities.User.All);
            }
            else if (User.IsInRole(Roles.Expert)) {
                //если только эксперт, то полный список контроллеров и один эксперт
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

            foreach (var file in file_srv.GetFiles(model.BeginDate, model.EndDate, model.SelectedControllerId, model.SelectedExpertId, model.SelectedStatusId, model.SelectedOgvId)) {
                model.Files.Add(FileModelConverter.ToModelItem(file));
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
