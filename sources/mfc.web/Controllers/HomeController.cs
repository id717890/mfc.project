using mfc.infrastructure.security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mfc.web.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult Help() {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [Authorize(Roles = Roles.Admin)]
        public ActionResult Directories() {
            return View();
        }
    }
}
