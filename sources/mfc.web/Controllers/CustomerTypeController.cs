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
using Ninject;

namespace mfc.web.Controllers {
    [Authorize(Roles = Roles.Admin)]
    public class CustomerTypeController : BaseController {

        [Inject]
        public ICustomerTypeService CustomerTypeService { get; set; }
        //
        // GET: /Directory/
        public ActionResult List() {
            List<CustomerTypeModel> items = new List<CustomerTypeModel>();

            foreach (var item in CustomerTypeService.GetAllTypes()) {
                items.Add(CustomerTypeModelConverter.ToModel(item));
            }

            return View(items);
        }

        public ActionResult Create() {
            return View("Edit", new CustomerTypeModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerTypeModel model) {
            bool has_error = false;
            if (ModelState.IsValid) {
                try {
                    CustomerTypeService.Create(model.Caption);
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
            var type = CustomerTypeService.GetTypeById(id);

            if (type == null) {
                ModelState.AddModelError("", "Вид деятельности не найден");
                return RedirectToAction("List");
            }

            var model = CustomerTypeModelConverter.ToModel(type);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(Int64 id, FormCollection collection) {
            bool has_error = false;

            try {
                CustomerTypeService.Delete(id);
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
            var type = CustomerTypeService.GetTypeById(id);

            if (type == null) {
                ModelState.AddModelError("", "Вид деятельности не найден");
                return RedirectToAction("List");
            }

            var model = CustomerTypeModelConverter.ToModel(type);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CustomerTypeModel model) {
            if (ModelState.IsValid) {
                bool has_error = false;

                if (!has_error) {
                    try {
                        var type = CustomerTypeModelConverter.FromModel(model);
                        CustomerTypeService.Update(type);
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
