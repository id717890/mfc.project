using System.Collections.Generic;
using System.Web.Mvc;

using mfc.domain.services;
using mfc.infrastructure.security;

namespace mfc.webapi.Controllers {
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : BaseController {
        public ActionResult Index() {
            return View();
        }

        public ActionResult List() {
            return View();
        }
    }
}