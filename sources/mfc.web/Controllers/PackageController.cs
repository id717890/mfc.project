using mfc.domain;
using mfc.domain.entities;
using mfc.domain.services;
using mfc.infrastructure.security;
using mfc.infrastructure.services;
using mfc.web.Abstracts;
using mfc.web.Helpers;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mfc.web.Controllers {
    public class PackageController : BaseController {
        #region Filter session save keys
        private const string PackageControllerIndexKey = "PackageControllerIndexKey";
        private const string ControllerKey = "Controller";
        private const string OrgKey = "Org";
        private const string DateBeginKey = "DateBegin";
        private const string DateEndKey = "DateEnd";
        #endregion

        /// <summary>
        /// Get list of packages
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="controllerId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public ActionResult Index(string beginDate = null, string endDate = null, Int64 controllerId = -1, Int64 orgId = -1) {
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
            var settings = Session[PackageControllerIndexKey] as IDictionary<string, object>;

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
                if (settings.ContainsKey(OrgKey)) {
                    orgId = (Int64)settings[OrgKey];
                }
            }

            return View(CreateModel(queryDateBegin, queryDateEnd, controllerId, orgId));
        }

        [HttpPost]
        public ActionResult Index(PackageListViewModel model) {
            var new_model = model;
            if (ModelState.IsValid) {
                //сохраняем настройки фильтра
                Session[PackageControllerIndexKey] = new Dictionary<string, object>();
                var settings = Session[PackageControllerIndexKey] as IDictionary<string, object>;
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

                if (!settings.ContainsKey(OrgKey)) {
                    settings.Add(OrgKey, model.SelectedOgvId);
                }
                else {
                    settings[OrgKey] = model.SelectedOgvId;
                }

                model = CreateModel(model.BeginDate, model.EndDate, model.SelectedControllerId, model.SelectedOgvId);
            }

            return View(model);
        }

        public ActionResult Edit(Int64 id) {
            var package_srv = CompositionRoot.Resolve<IPackageService>();

            var package = package_srv.GetPackageById(id);
            if (package == null){
                throw new Exception(string.Format("Не найден пакет с идентификатором {0}", id));
            }

            PackageModel model = PackageHelper.CreateModel(package);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(PackageModel model) {
            bool has_error = false;

            var file_srv = CompositionRoot.Resolve<IFileService>();
            var file_stage_srv = CompositionRoot.Resolve<IFileStageService>();
            var package_srv = CompositionRoot.Resolve<IPackageService>();
            
            if (ModelState.IsValid) {
                var package = PackageHelper.CreatePackage(model);
                                
                try {
                    file_srv.SetStage(model.Files.Select(x => x.Id), file_stage_srv.GetStageByStatus(model.Status).Code, string.Empty);
                    package_srv.Update(package);
                    package_srv.UpdatePackageFiles(package.Id, model.Files != null ? model.Files.Select(f => f.Id) : new List<Int64>());
                }
                catch (DomainException e) {
                    has_error = true;
                    ModelState.AddModelError("", e.Message);
                }
            }

            if (has_error) {
                PackageHelper.PrepareModel(model);

                return View(model);
            }

            return RedirectToAction("Index");

        }

        /// <summary>
        /// Get-query for deleting package
        /// </summary>
        /// 
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Int64 id) {
            var result = false;

            var package_srv = CompositionRoot.Resolve<IPackageService>();
            var package = package_srv.GetPackageById(id);

            if (package != null) {
                try {
                    package_srv.Delete(id);
                    result = true;
                }
                catch (Exception e) {
                    Logger.Error(e);
                }
            }

            return Content(result ? Boolean.TrueString : Boolean.FalseString);
        }

        [HttpGet]
        public ActionResult Report(Int64 id) {
            var report_srv = CompositionRoot.Resolve<IReportService>();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition",
                               string.Format("attachment; filename={0}.xls", "rep"));
            report_srv.MakeReestr(id, Response.OutputStream);

            Response.End();

            return RedirectToAction("Edit", new {id = id});
        }

        private PackageListViewModel CreateModel(DateTime beginDate, DateTime endDate, Int64 controllerId = -1, Int64 orgId = -1) {
            var user_srv = CompositionRoot.Resolve<IUserService>();
            var status_srv = CompositionRoot.Resolve<IFileStatusService>();
            var org_srv = CompositionRoot.Resolve<IOrganizationService>();
            var package_srv = CompositionRoot.Resolve<IPackageService>();

            var model = new PackageListViewModel();

            model.SelectedOgvId = orgId == -1 ? Organization.All.Id : orgId;
            model.SelectedControllerId = controllerId == -1 ? mfc.domain.entities.User.All.Id : controllerId;
            model.BeginDate = beginDate;
            model.EndDate = endDate;

            //заполняем список всеми контролерами
            foreach (var user in user_srv.GetControllers()) {
                model.Controllers.Add(user);
            }
            model.Controllers.Insert(0, mfc.domain.entities.User.All);

            //заполняем список всеми организациями
            model.Organizations.Add(Organization.All);

            foreach (var item in org_srv.GetAllOrganizations()) {
                model.Organizations.Add(item);
            }

            foreach (var package in package_srv.GetPackages(model.BeginDate, model.EndDate, model.SelectedControllerId, model.SelectedOgvId)) {
                model.Packages.Add(package);
            }

            return model;
        }

        private void PrepareForCreate() {
            var usr_srv = CompositionRoot.Resolve<IUserService>();
            ViewBag.Controllers = new List<User>(usr_srv.GetControllers());
            ViewBag.Experts = new List<User>(usr_srv.GetExperts()); ;
        }
    }
}