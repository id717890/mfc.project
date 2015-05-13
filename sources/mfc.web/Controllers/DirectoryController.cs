using mfc.infrastructure.security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mfc.web.Controllers {
    [Authorize(Roles = Roles.Admin)]
    public class DirectoryController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}