using mfc.infrastructure.security;
using mfc.webapi;
using System.Web.Mvc;

namespace mfc.web.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class OrganizationTypeController : BaseController {
        //
        // GET: /Directory/
        public ActionResult List() {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            return View();
        }

    }
}
