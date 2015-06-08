using mfc.domain;
using mfc.domain.entities;
using mfc.domain.services;
using mfc.infrastructure.security;
using mfc.web.Abstracts;
using mfc.web.Helpers;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mfc.web.Controllers {
    [Authorize(Roles = Roles.Admin)]
    public class OrganizationTypeController : BaseController {
        //
        // GET: /Directory/
        public ActionResult List() {
            var organization_service = CompositionRoot.Resolve<IOrganizationService>();
            List<OrganizationTypeModel> items = new List<OrganizationTypeModel>();

            foreach (var item in organization_service.GetAllTypes()) {
                items.Add(OrganizationTypeModelConverter.ToModel(item));
            }

            return View(items);
        }

        public ActionResult Create() {
            return View("Edit", new OrganizationTypeModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrganizationTypeModel model) {
            bool has_error = false;
            if (ModelState.IsValid) {
                var organization_service = CompositionRoot.Resolve<IOrganizationService>();
                try {
                    organization_service.CreateType(model.Caption);
                }
                catch (DomainException e) {
                    ModelState.AddModelError("", e);
                    has_error = true;
                }
            }

            if (!has_error) {
                return RedirectToAction("List");
            }

            return View("Edit");
        }

        public ActionResult Delete(Int64 id) {
            var organization_service = CompositionRoot.Resolve<IOrganizationService>();

            var type = organization_service.GetTypeById(id);

            if (type == null) {
                ModelState.AddModelError("", "Тип ОГВ не найден");
                return RedirectToAction("List");
            }

            var model = OrganizationTypeModelConverter.ToModel(type);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(OrganizationTypeModel model) {
            if (ModelState.IsValid) {
                bool has_error = false;

                var organization_service = CompositionRoot.Resolve<IOrganizationService>();
                try {
                    organization_service.DeleteType(model.Id);
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

        public ActionResult Edit(Int64 id) {
            var organization_service = CompositionRoot.Resolve<IOrganizationService>();
            var type = organization_service.GetTypeById(id);

            if (type == null) {
                ModelState.AddModelError("", "Тип ОГВ не найден");
                return RedirectToAction("List");
            }

            var model = OrganizationTypeModelConverter.ToModel(type);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(OrganizationTypeModel model) {
            if (ModelState.IsValid) {
                bool has_error = false;

                var organization_service = CompositionRoot.Resolve<IOrganizationService>();

                if (!has_error) {
                    try {
                        var type = OrganizationTypeModelConverter.FromModel(model);
                        organization_service.UpdateType(type);
                    }
                    catch (DomainException e) {
                        ModelState.AddModelError("", e);
                        has_error = true;
                    }
                }

                if (!has_error) {
                    return RedirectToAction("List");
                }
            }
            return View();
        }
    }
}
