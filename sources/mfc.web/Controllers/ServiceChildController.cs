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
    public class ServiceChildController : BaseController {
        public ActionResult Index(Int64 parentId) {
            var srv = CompositionRoot.Resolve<IServiceService>();

            ServiceListViewModel model = new ServiceListViewModel();

            model.Services = new List<Service>(srv.GetChildServices(parentId));
            model.Parent = srv.GetServiceById(parentId);

            return View(model);
        }
        
        public ActionResult Create(Int64 parentId) {
            var srv = CompositionRoot.Resolve<IServiceService>();

            var model = new ServiceModel();
            var parent = srv.GetServiceById(parentId);

            if (parent != null){
                model.ParentId = parent.Id;
                model.ParentCaption = parent.Caption;
                model.OrgCaption = parent.Organization.Caption;
                model.OrgId = parent.Organization.Id;
            }
            else {
                RedirectToAction("Index", new { parentId = parentId });
            }


            return View("Edit", model);
        }

        //
        // POST: /Organization/Create

        [HttpPost]
        public ActionResult Create(ServiceModel model) {
            var srv = CompositionRoot.Resolve<IServiceService>();

            if (ModelState.IsValid) {
                bool has_error = false;

                try {
                    var org = mfc.web.Controllers.ServiceController.ModelConverter.FromModel(model);
                    srv.Create(model.Caption, model.OrgId, model.ParentId);
                }
                catch (DomainException e) {
                    ModelState.AddModelError("", e);
                    has_error = true;
                }

                if (!has_error) {
                    return RedirectToAction("Index", new { parentId = model.ParentId });
                }
            }

            return View("Edit", model);
        }
        
        public ActionResult Edit(int id) {
            var srv = CompositionRoot.Resolve<IServiceService>();
            var service = srv.GetServiceById(id);

            if (service == null) {
                ModelState.AddModelError("", string.Format("Услуга с кодом {0} не найдена в базе данных", id));
                return View();
            }

            return View(mfc.web.Controllers.ServiceController.ModelConverter.ToModel(service));
        }

        [HttpPost]
        public ActionResult Edit(ServiceModel model) {
            if (ModelState.IsValid) {
                bool has_error = false;

                var srv = CompositionRoot.Resolve<IServiceService>();

                try {
                    var service = mfc.web.Controllers.ServiceController.ModelConverter.FromModel(model);
                    srv.Update(service);
                }
                catch (DomainException e) {
                    ModelState.AddModelError("", e);
                    has_error = true;
                }

                if (!has_error) {
                    return RedirectToAction("Index", new { parentId = model.ParentId });
                }
            }
            return View();
        }

        //
        // GET: /Organization/Delete/5
        public ActionResult Delete(int id) {
            var srv = CompositionRoot.Resolve<IServiceService>();

            var service = srv.GetServiceById(id);

            if (service == null) {
                ModelState.AddModelError("", "Услуга не найдена");
                return RedirectToAction("List", "Service");
            }

            var model = mfc.web.Controllers.ServiceController.ModelConverter.ToModel(service);

            return View(model);
        }

        //
        // POST: /Organization/Delete/5

        [HttpPost]
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
                return RedirectToAction("Index", "ServiceChild", new { parentId = collection["ParentId"] });
            }
            else {
                return View();
            }
        }
    }
}