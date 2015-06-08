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
    public class FileStatusController : BaseController {
        //
        // GET: /Directory/
        public ActionResult List() {
            var srv = CompositionRoot.Resolve<IFileStatusService>();
            List<FileStatusModel> items = new List<FileStatusModel>();

            foreach (var item in srv.GetAllStatuses()) {
                items.Add(FileStatusModelConverter.ToModel(item));
            }

            return View(items);
        }

        public ActionResult Create() {
            return View("Edit", new FileStatusModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FileStatusModel model) {
            bool has_error = false;
            if (ModelState.IsValid) {
                var srv = CompositionRoot.Resolve<IFileStatusService>();
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
            var srv = CompositionRoot.Resolve<IFileStatusService>();

            var type = srv.GetStatusById(id);

            if (type == null) {
                ModelState.AddModelError("", "Статус не найден");
                return RedirectToAction("List");
            }

            var model = FileStatusModelConverter.ToModel(type);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(Int64 id, FormCollection collection) {
            bool has_error = false;

            var srv = CompositionRoot.Resolve<IFileStatusService>();
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
            var srv = CompositionRoot.Resolve<IFileStatusService>();
            var type = srv.GetStatusById(id);

            if (type == null) {
                ModelState.AddModelError("", "Статус не найден");
                return RedirectToAction("List");
            }

            var model = FileStatusModelConverter.ToModel(type);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(FileStatusModel model) {
            if (ModelState.IsValid) {
                bool has_error = false;

                var srv = CompositionRoot.Resolve<IFileStatusService>();

                if (!has_error) {
                    try {
                        var type = FileStatusModelConverter.FromModel(model);
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
