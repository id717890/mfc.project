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
using mfc.web.Helpers;
using mfc.web.Models;

namespace mfc.web.Controllers {
    public class ActionController : Controller {

        public ActionResult Index(string date = null, Int64 user_id = -1) {
            var user_srv = CompositionRoot.Resolve<IUserService>();
            User user = null;

            //Если текущий пользователь не администратор, то тогда
            //устанавливаем его текущим
            if (!User.IsInRole(Roles.Admin) && user_id == -1) {
                user = user_srv.GetUser(User.Identity.Name);
            }
            else if (user_id > 0) {
                user = user_srv.GetUserById(user_id);
            }

            var queryDate = DateTime.Today;

            if (!string.IsNullOrEmpty(date)) {
                DateTime.TryParse(date, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out queryDate);
            }

            var model = CreateActionListModel(queryDate, user);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(DateTime date, Int64 selectedUserId = -1) {
            var user_srv = CompositionRoot.Resolve<IUserService>();
            var user = user_srv.GetUserById(selectedUserId);

            var model = CreateActionListModel(date, user);

            return View(model);
        }

        public ActionResult Create(string date, Int64 user_id) {
            var model = new ServiceActionViewModel();

            model.ExpertId = user_id;
            DateTime createDate = DateTime.Today;

            if (!string.IsNullOrEmpty(date)) {
                DateTime.TryParse(date, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out createDate);
            }

            model.Date = createDate;
            PrepareForCreate();

            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Create(ServiceActionViewModel model) {
            if (ModelState.IsValid) {
                var action_srv = CompositionRoot.Resolve<IActionService>();
                bool has_error = false;

                Int64 id = -1;

                try {
                    id = action_srv.Add(model.Date, model.ServiceId, model.Customer, model.TypeId, model.ExpertId, model.Comments);
                }
                catch (Exception e) {
                    has_error = true;
                    ModelState.AddModelError("", string.Format("{0} \r\n {1}", e.Message, e.StackTrace));
                }

                if (!has_error) {
                    return RedirectToAction("Index", new { date = model.Date.ToString("dd.MM.yyyy"), user_id = model.ExpertId });
                }
            }

            PrepareForCreate();

            return View("Edit", model);
        }

        public ActionResult Delete(int id) {
            var action_srv = CompositionRoot.Resolve<IActionService>();
            bool has_error = false;
            ServiceActionViewModel model = null;

            try {
                var action = action_srv.GetActionById(id);
                if (action == null) {
                    ModelState.AddModelError("", "Работа не найдена");
                    has_error = true;
                }

                model = ServiceActionModelConverter.ToModel(action);
            }
            catch (DomainException e) {
                ModelState.AddModelError("", e);
                has_error = true;
            }

            if (has_error) {
                return RedirectToAction("Index", new { date = model.Date.ToString("dd.MM.yyyy"), user_id = model.ExpertId });
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(ServiceActionViewModel model) {
            bool has_error = false;

            var action_srv = CompositionRoot.Resolve<IActionService>();
            try {
                action_srv.Delete(model.Id);
            }
            catch (DomainException e) {
                ModelState.AddModelError("", e);
                has_error = true;
            }

            if (!has_error) {
                return RedirectToAction("Index", new { date = model.Date.ToString("dd.MM.yyyy"), user_id = model.ExpertId });
            }
            else {
                return View();
            }
        }

        public ActionResult Edit(Int64 id) {
            var action_srv = CompositionRoot.Resolve<IActionService>();
            bool has_error = false;
            ServiceActionViewModel model = null;

            try {
                var action = action_srv.GetActionById(id);
                if (action == null) {
                    ModelState.AddModelError("", "Работа не найдена");
                    has_error = true;
                }

                model = ServiceActionModelConverter.ToModel(action);
            }
            catch (DomainException e) {
                ModelState.AddModelError("", e);
                has_error = true;
            }

            if (has_error) {
                return RedirectToAction("Index", new { date = model.Date.ToString("dd.MM.yyyy"), user_id = model.ExpertId });
            }

            PrepareForCreate();

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ServiceActionViewModel model) {
            bool has_error = false;
            if (ModelState.IsValid) {
                var action_srv = CompositionRoot.Resolve<IActionService>();
                try {
                    var action = ServiceActionModelConverter.FromModel(model);
                    action_srv.Update(action);
                }
                catch (DomainException e) {
                    ModelState.AddModelError("", e);
                    has_error = true;
                }

                if (!has_error) {
                    return RedirectToAction("Index", new { date = model.Date.ToString("dd.MM.yyyy"), user_id = model.ExpertId });
                }
            }
            PrepareForCreate();
            return View(model);
        }

        #region Helpers

        private void PrepareForCreate() {
            var service_srv = CompositionRoot.Resolve<IServiceService>();
            var type_srv = CompositionRoot.Resolve<IActionTypeService>();
            var usr_srv = CompositionRoot.Resolve<IUserService>();

            ViewBag.Services = service_srv.GetAllServices();
            ViewBag.ActionTypes = type_srv.GetAllTypes();

            var experts = new List<User>();

            if (User.IsInRole(Roles.Admin)) {
                experts.AddRange(usr_srv.GetExperts());
            }
            else {
                experts.Add(usr_srv.GetUser(User.Identity.Name));
            }

            ViewBag.Experts = experts;
        }

        private ActionListViewModel CreateActionListModel(DateTime date, User selectedUser) {
            var user_srv = CompositionRoot.Resolve<IUserService>();

            ActionListViewModel model = new ActionListViewModel();
            model.Date = date;

            //Для администратора заполняем список всеми экспертами
            //Для эксперта - только им.
            if (User.IsInRole(Roles.Admin)) {
                foreach (var user in user_srv.GetExperts()) {
                    model.Users.Add(user);
                }
            }
            else {
                model.Users.Add(selectedUser);
            }

            var action_srv = CompositionRoot.Resolve<IActionService>();

            //Пользователь, для данными которого заполняется список оказанных услуг
            //Если он не задан, то берем первого в списке
            User queryUser = selectedUser;

            if (queryUser == null && model.Users.Count > 0) {
                queryUser = model.Users[0];
            }

            if (queryUser != null) {
                model.SelectedUserId = queryUser.Id;

                foreach (var action in action_srv.GetActions(queryUser, date)) {
                    model.Actions.Add(action);
                }
            }

            return model;
        }

        #endregion

    }
}
