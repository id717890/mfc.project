using mfc.domain;
using mfc.domain.entities;
using mfc.domain.services;
using mfc.infrastructure.security;
using mfc.web.Helpers;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mfc.web.Controllers {
    [Authorize(Roles = Roles.Admin)]
    public class ActionTypeController : Controller {
        //
        // GET: /Directory/
        public ActionResult List() {
            var srv = CompositionRoot.Resolve<IActionTypeService>();
            List<ActionTypeModel> items = new List<ActionTypeModel>();

            foreach (var item in srv.GetAllTypes()) {
                items.Add(ActionTypeModelConverter.ToModel(item));
            }

            return View(items);
        }

        public ActionResult Create() {
            return View("Edit", new ActionTypeModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActionTypeModel model) {
            bool has_error = false;
            if (ModelState.IsValid) {
                var srv = CompositionRoot.Resolve<IActionTypeService>();
                try {
                    srv.Create(model.Caption);
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
            var srv = CompositionRoot.Resolve<IActionTypeService>();

            var type = srv.GetTypeById(id);

            if (type == null) {
                ModelState.AddModelError("", "Вид деятельности не найден");
                return RedirectToAction("List");
            }

            var model = ActionTypeModelConverter.ToModel(type);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(Int64 id, FormCollection collection) {
            bool has_error = false;

            var srv = CompositionRoot.Resolve<IActionTypeService>();
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

        public ActionResult Edit(Int64 id) {
            var srv = CompositionRoot.Resolve<IActionTypeService>();
            var type = srv.GetTypeById(id);

            if (type == null) {
                ModelState.AddModelError("", "Вид деятельности не найден");
                return RedirectToAction("List");
            }

            var model = ActionTypeModelConverter.ToModel(type);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ActionTypeModel model) {
            if (ModelState.IsValid) {
                bool has_error = false;

                var srv = CompositionRoot.Resolve<IActionTypeService>();

                if (!has_error) {
                    try {
                        var type = ActionTypeModelConverter.FromModel(model);
                        srv.Update(type);
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
