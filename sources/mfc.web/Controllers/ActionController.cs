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
using mfc.web.Abstracts;
using Ninject;
using Service = System.Web.Services.Description.Service;

namespace mfc.web.Controllers {
    public class ActionController : BaseController {
        private const string ActionControllerFilerKey = "ActionControllerFilerKey";
        private const string DateBeginKey = "DateBeginKey";
        private const string DateEndKey = "DateEndKey";
        private const string UserKey = "UserKey";


        [Inject]
        public ICustomerTypeService CustomerTypeService { get; set; }

        [Inject]
        public IActionService ActionService { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public IServiceService ServiceService { get; set; }

        [Inject]
        public IActionTypeService ActionTypeService { get; set; }

        public ActionResult Index(string dateBegin = null, string dateEnd = null, Int64 user_id = -1, Int32 page = 1) {
            User user = null;

            //Если текущий пользователь не администратор, то тогда
            //устанавливаем его текущим
            if (!User.IsInRole(Roles.Admin) && user_id == -1) {
                user = UserService.GetUser(User.Identity.Name);
            }
            else if (user_id > 0) {
                user = UserService.GetUserById(user_id);
            }

            var queryDateBegin = DateTime.Today;

            if (!string.IsNullOrEmpty(dateBegin)) {
                DateTime.TryParse(dateBegin, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out queryDateBegin);
            }

            var queryDateEnd = DateTime.Today;

            if (!string.IsNullOrEmpty(dateEnd)) {
                DateTime.TryParse(dateEnd, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out queryDateEnd);
            }

            //Загружаем настройки
            var settings = Session[ActionControllerFilerKey] as IDictionary<string, object>;

            if (settings != null) {
                if (settings.ContainsKey(DateBeginKey)) {
                    queryDateBegin = (DateTime)settings[DateBeginKey];
                }

                if (settings.ContainsKey(DateEndKey)) {
                    queryDateEnd = (DateTime)settings[DateEndKey];
                }

                if (settings.ContainsKey(UserKey)) {
                    user = UserService.GetUserById((Int64)settings[UserKey]);
                }
            }

            if (user == null) {
                user = domain.entities.User.All;
            }

            var model = CreateActionListModel(queryDateBegin, queryDateEnd, user);
            if (page == 0) {
                page = 1;
            }

            model.Page = page;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(DateTime dateBegin, DateTime dateEnd, Int64 selectedUserId = -1, Int32 page = 1) {
            //сохраняем настройки фильтра
            Session[ActionControllerFilerKey] = new Dictionary<string, object>();
            var settings = Session[ActionControllerFilerKey] as IDictionary<string, object>;
            if (!settings.ContainsKey(DateBeginKey)) {
                settings.Add(DateBeginKey, dateBegin);
            }
            else {
                settings[DateBeginKey] = dateBegin;
            }

            if (!settings.ContainsKey(DateEndKey)) {
                settings.Add(DateEndKey, dateEnd);
            }
            else {
                settings[DateEndKey] = dateEnd;
            }

            if (!settings.ContainsKey(UserKey)) {
                settings.Add(UserKey, selectedUserId);
            }
            else {
                settings[UserKey] = selectedUserId;
            }

            var user = domain.entities.User.All;

            if (selectedUserId != domain.entities.User.All.Id) {
                user = UserService.GetUserById(selectedUserId);
            }
            
            var model = CreateActionListModel(dateBegin, dateEnd, user);
            if (page == 0) {
                page = 1;
            }

            model.Page = 1;

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
                    id = action_srv.Add(model.Date, model.ServiceId, model.Customer, model.TypeId, model.CustomerTypeId, model.ExpertId, model.ServiceChildId, model.IsNonresident, model.FreeVisit, model.Comments);
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

            try {
                ActionService.Delete(model.Id);
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
                model.IsItInDialog = false;
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

        public ActionResult EditPartial(Int64 id) {
            bool has_error = false;
            ServiceActionViewModel model = null;

            try {
                var action = ActionService.GetActionById(id);
                if (action == null) {
                    ModelState.AddModelError("", "Работа не найдена");
                    has_error = true;
                }

                model = ServiceActionModelConverter.ToModel(action);
                model.IsItInDialog = true;
            }
            catch (DomainException e) {
                ModelState.AddModelError("", e);
                has_error = true;
            }

            if (has_error) {
                return RedirectToAction("Index", new { date = model.Date.ToString("dd.MM.yyyy"), user_id = model.ExpertId });
            }

            PrepareForCreate();

            return PartialView("_Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(ServiceActionViewModel model) {
            bool has_error = false;
            string errorMessage = string.Empty;

            if (ModelState.IsValid) {
                try {
                    var action = ActionService.GetActionById(model.Id);

                    if (action == null)
                    {
                        throw new DomainException($"Прием с идентификатором {model.Id} не найден в базе данных");
                    }

                    if (action.ServiceChild != null && !action.Service.Equals(action.ServiceChild.Parent)) {
                        throw new DomainException("Подуслуга не связана с услугой");
                    }

                    ActionService.Update(action);
                }
                catch (DomainException e) {
                    ModelState.AddModelError("", e.Message);
                    errorMessage = e.Message;
                    has_error = true;
                }
                catch (Exception e) {
                    if (!model.IsItInDialog) {
                        throw e;
                    }
                    Logger.Error(e);
                    errorMessage = $"Ошибка при сохранении данныхъ {e.Message}";
                    has_error = true;
                }

                if (!has_error && !model.IsItInDialog) {
                    return RedirectToAction("Index", new { date = model.Date.ToString("dd.MM.yyyy"), user_id = model.ExpertId });
                }
            }


            PrepareForCreate();

            if (model.IsItInDialog) {
                if (!has_error) {
                    return Json(true);
                }
                return Json(new {message = errorMessage});
            }

            return View(model);
        }

        public ActionResult GetById(Int64 id) {
            var action = ActionService.GetActionById(id);
            if (action == null) {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            
            return Json(ServiceActionModelConverter.ToModel(action), JsonRequestBehavior.AllowGet);
        }

        #region Helpers

        private void PrepareForCreate() {
            ViewBag.Services = ServiceService.GetAllServices();
            ViewBag.ActionTypes = ActionTypeService.GetAllTypes();
            ViewBag.CustomerTypes = CustomerTypeService.GetAllTypes();

            var experts = new List<User>();

            if (User.IsInRole(Roles.Admin)) {
                experts.AddRange(UserService.GetExperts());
            }
            else {
                experts.Add(UserService.GetUser(User.Identity.Name));
            }

            ViewBag.Experts = experts;
        }

        private ActionListViewModel CreateActionListModel(DateTime dateBegin, DateTime dateEnd, User selectedUser) {

            ActionListViewModel model = new ActionListViewModel();
            model.DateBegin = dateBegin;
            model.DateEnd = dateEnd;

            //Для администратора заполняем список всеми экспертами
            //Для эксперта - только им.
            if (User.IsInRole(Roles.Admin)) {
                foreach (var user in UserService.GetExperts()) {
                    model.Users.Add(user);
                }
                model.Users.Insert(0, mfc.domain.entities.User.All);
            }
            else {
                model.Users.Add(selectedUser);
            }
            
            //Пользователь, для данными которого заполняется список оказанных услуг
            //Если он не задан, то берем первого в списке
            if (selectedUser != null) {
                model.SelectedUserId = selectedUser.Id;

                foreach (var action in ActionService.GetActions(selectedUser, dateBegin, dateEnd)) {
                    model.Actions.Add(action);
                }
            }

            return model;
        }

        #endregion

    }
}
