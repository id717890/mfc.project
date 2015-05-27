using mfc.domain;
using mfc.domain.entities;
using mfc.domain.services;
using mfc.web.Helpers;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mfc.web.Controllers {
    public class OrganizationController : Controller {
        //
        // GET: /Organization/

        public ActionResult List() {
            var org_service = CompositionRoot.Resolve<IOrganizationService>();

            List<OrganizationModel> items = new List<OrganizationModel>();

            foreach (var entity in org_service.GetAllOrganizations()) {
                items.Add(OrganizationModelConverter.ToModel(entity));
            }

            return View(items);
        }

        public ActionResult ListData() {
            var org_service = CompositionRoot.Resolve<IOrganizationService>();

            List<OrganizationModel> items = new List<OrganizationModel>();

            foreach (var entity in org_service.GetAllOrganizations()) {
                items.Add(OrganizationModelConverter.ToModel(entity));
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Organization/Create

        public ActionResult Create() {
            var org_service = CompositionRoot.Resolve<IOrganizationService>();

            var types = new List<OrganizationTypeModel>();

            foreach (var type in org_service.GetAllTypes()) {
                types.Add(OrganizationTypeModelConverter.ToModel(type));
            }

            ViewBag.OrganizationTypes = types;

            return View("Edit", new OrganizationModel());
        }

        //
        // POST: /Organization/Create

        [HttpPost]
        public ActionResult Create(OrganizationModel model) {
            var org_service = CompositionRoot.Resolve<IOrganizationService>();

            if (ModelState.IsValid) {
                bool has_error = false;

                try {
                    var org = OrganizationModelConverter.FromModel(model);
                    org_service.CreateOrganization(model.Caption, model.FullCaption, model.TypeId);
                }
                catch (DomainException e) {
                    ModelState.AddModelError("", e);
                    has_error = true;
                }

                if (!has_error) {
                    return RedirectToAction("List");
                }
            }

            var types = new List<OrganizationTypeModel>();

            foreach (var type in org_service.GetAllTypes()) {
                types.Add(OrganizationTypeModelConverter.ToModel(type));
            }

            ViewBag.OrganizationTypes = types;

            return View("Edit", model);
        }

        //
        // GET: /Organization/Edit/5

        public ActionResult Edit(int id) {
            var org_service = CompositionRoot.Resolve<IOrganizationService>();
            var org = org_service.GetOrganizationById(id);

            if (org == null) {
                ModelState.AddModelError("", string.Format("ОГВ с кодом {0} не найдена в базе данных", id));
                return View();
            }

            var types = new List<OrganizationTypeModel>();

            foreach (var type in org_service.GetAllTypes()) {
                types.Add(OrganizationTypeModelConverter.ToModel(type));
            }

            ViewBag.OrganizationTypes = types;

            return View(OrganizationModelConverter.ToModel(org));
        }

        //
        // POST: /Organization/Edit/5

        [HttpPost]
        public ActionResult Edit(OrganizationModel model) {
            var organization_service = CompositionRoot.Resolve<IOrganizationService>();

            if (ModelState.IsValid) {
                bool has_error = false;

                try {
                    var org = OrganizationModelConverter.FromModel(model);
                    organization_service.UpdateOgranization(org);
                }
                catch (DomainException e) {
                    ModelState.AddModelError("", e);
                    has_error = true;
                }

                if (!has_error) {
                    return RedirectToAction("List");
                }
            }
            var types = new List<OrganizationTypeModel>();

            foreach (var type in organization_service.GetAllTypes()) {
                types.Add(OrganizationTypeModelConverter.ToModel(type));
            }

            ViewBag.OrganizationTypes = types;

            return View(model);
        }

        //
        // GET: /Organization/Delete/5

        public ActionResult Delete(int id) {
            var organization_service = CompositionRoot.Resolve<IOrganizationService>();

            var org = organization_service.GetOrganizationById(id);

            if (org == null) {
                ModelState.AddModelError("", "ОГВ не найдена");
                return RedirectToAction("List");
            }

            var model = OrganizationModelConverter.ToModel(org);

            return View(model);
        }

        //
        // POST: /Organization/Delete/5

        [HttpPost]
        public ActionResult Delete(Int64 id, FormCollection collection) {
            bool has_error = false;

            var organization_service = CompositionRoot.Resolve<IOrganizationService>();
            try {
                organization_service.DeleteOrganization(id);
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
    }
}
