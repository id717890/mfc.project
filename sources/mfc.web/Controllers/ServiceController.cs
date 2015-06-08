using mfc.domain;
using mfc.domain.entities;
using mfc.domain.services;
using mfc.web.Abstracts;
using mfc.web.Helpers;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mfc.web.Controllers {
    public class ServiceController : BaseController {
        [Authorize(Roles = mfc.infrastructure.security.Roles.Admin)]
        public ActionResult List(Int64 id = 0) {
            var srv = CompositionRoot.Resolve<IServiceService>();
            var org_srv = CompositionRoot.Resolve<IOrganizationService>();

            ServiceListViewModel model = new ServiceListViewModel();

            model.Organizations = new List<Organization>(org_srv.GetAllOrganizations());
            model.Organizations.Insert(0, new Organization { Id = -1, Caption = "<Все>" });
            
            if (id > 0) {
                model.OrganizationId = id;
                model.Services = new List<Service>(srv.GetOrganizationServices(id));
            }
            else {
                model.OrganizationId = model.Organizations[0].Id;
                model.Services = new List<Service>(srv.GetAllServices());
            }

            return View(model);
        }
        
        public ActionResult ListData(int id) {
            var srv = CompositionRoot.Resolve<IServiceService>();

            List<ServiceModel> items = new List<ServiceModel>();

            foreach (var entity in srv.GetOrganizationServices(id)) {
                items.Add(ModelConverter.ToModel(entity));
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChildListData(int id) {
            var srv = CompositionRoot.Resolve<IServiceService>();

            List<ServiceModel> items = new List<ServiceModel>();

            foreach (var entity in srv.GetChildServices(id)) {
                items.Add(ModelConverter.ToModel(entity));
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Organization/Create
        [Authorize(Roles = mfc.infrastructure.security.Roles.Admin)]
        public ActionResult Create() {
            var srv = CompositionRoot.Resolve<IServiceService>();
            var org_srv = CompositionRoot.Resolve<IOrganizationService>();

            var orgs = new List<OrganizationModel>();

            foreach (var type in org_srv.GetAllOrganizations()) {
                orgs.Add(OrganizationModelConverter.ToModel(type));
            }

            ViewBag.Organizations = orgs;

            return View("Edit", new ServiceModel());
        }

        //
        // POST: /Organization/Create

        [HttpPost]
        [Authorize(Roles = mfc.infrastructure.security.Roles.Admin)]
        public ActionResult Create(ServiceModel model) {
            var srv = CompositionRoot.Resolve<IServiceService>();

            if (ModelState.IsValid) {
                bool has_error = false;

                try {
                    var org = ModelConverter.FromModel(model);
                    srv.Create(model.Caption, model.OrgId);
                }
                catch (DomainException e) {
                    ModelState.AddModelError("", e);
                    has_error = true;
                }

                if (!has_error) {
                    return RedirectToAction("List");
                }
            }

            var org_srv = CompositionRoot.Resolve<IOrganizationService>();

            var orgs = new List<OrganizationModel>();

            foreach (var type in org_srv.GetAllOrganizations()) {
                orgs.Add(OrganizationModelConverter.ToModel(type));
            }

            ViewBag.Organizations = orgs;

            return View("Edit");
        }

        //
        // GET: /Organization/Edit/5
        [Authorize(Roles = mfc.infrastructure.security.Roles.Admin)]
        public ActionResult Edit(int id) {
            var srv = CompositionRoot.Resolve<IServiceService>();
            var service = srv.GetServiceById(id);

            if (service == null) {
                ModelState.AddModelError("", string.Format("Услуга с кодом {0} не найдена в базе данных", id));
                return View();
            }

            var org_srv = CompositionRoot.Resolve<IOrganizationService>();

            var orgs = new List<OrganizationModel>();

            foreach (var type in org_srv.GetAllOrganizations()) {
                orgs.Add(OrganizationModelConverter.ToModel(type));
            }

            ViewBag.Organizations = orgs;

            return View(ModelConverter.ToModel(service));
        }

        //
        // POST: /Organization/Edit/5

        [HttpPost]
        [Authorize(Roles = mfc.infrastructure.security.Roles.Admin)]
        public ActionResult Edit(ServiceModel model) {
            if (ModelState.IsValid) {
                bool has_error = false;

                var srv = CompositionRoot.Resolve<IServiceService>();

                try {
                    var service = ModelConverter.FromModel(model);
                    srv.Update(service);
                }
                catch (DomainException e) {
                    ModelState.AddModelError("", e);
                    has_error = true;
                }

                if (!has_error) {
                    return RedirectToAction("List");
                }
            }
            return View();
        }

        //
        // GET: /Organization/Delete/5
        [Authorize(Roles = mfc.infrastructure.security.Roles.Admin)]
        public ActionResult Delete(int id) {
            var srv = CompositionRoot.Resolve<IServiceService>();

            var service = srv.GetServiceById(id);

            if (service == null) {
                ModelState.AddModelError("", "Услуга не найдена");
                return RedirectToAction("List");
            }

            var model = ModelConverter.ToModel(service);

            return View(model);
        }

        //
        // POST: /Organization/Delete/5

        [HttpPost]
        [Authorize(Roles = mfc.infrastructure.security.Roles.Admin)]
        public ActionResult Delete(Int64 id, FormCollection collection) {
            bool has_error = false;

            var srv = CompositionRoot.Resolve<IServiceService>();
            try {
                srv.Delete(id);
            }
            catch (DomainException e) {
                ModelState.AddModelError("", e);
                has_error = true;
            }

            if (!has_error) {
                return RedirectToAction("List");
            }
            else {
                return View();
            }
        }

        #region Helpers

        public static class ModelConverter {
            public static ServiceModel ToModel(Service entity) {
                return new ServiceModel {
                    Id = entity.Id,
                    Caption = entity.Caption,
                    OrgId = entity.Organization != null ? entity.Organization.Id : -1,
                    OrgCaption = entity.Organization != null ? entity.Organization.Caption : string.Empty,
                    ParentId = entity.Parent != null ? entity.Parent.Id : 0,
                    ParentCaption = entity.Parent != null ? entity.Parent.Caption : string.Empty
                };
            }

            public static Service FromModel(ServiceModel model) {
                var org_srv = CompositionRoot.Resolve<IOrganizationService>();
                var srv = CompositionRoot.Resolve<IServiceService>();
                var org = org_srv.GetOrganizationById(model.OrgId);
                var parent = srv.GetServiceById(model.ParentId);

                return new Service {
                    Id = model.Id,
                    Caption = model.Caption,
                    Organization = org,
                    Parent = parent
                };
            }
        }

        #endregion
    }
}
