using mfc.domain.entities;
using mfc.domain.services;
using mfc.infrastructure.security;
using mfc.web.Abstracts;
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

            if (User.IsInRole(Roles.Admin)) {
                //Для администратора заполняем список всеми экспертами и контролерами
                foreach (var user in user_srv.GetControllers()) {
                    model.Controllers.Add(user);
                }
                model.Controllers.Insert(0, mfc.domain.entities.User.All);
            }
            else if (User.IsInRole(Roles.Controller)) {
                //если только контролер, то полный список экспертов и один контролер
                var user = user_srv.GetCurrentUser();

                model.Controllers.Add(user_srv.GetCurrentUser());

                model.SelectedControllerId = user.Id;
            }

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