using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;
using mfc.infrastructure.services;
using System.Linq;
using Ninject;

namespace mfc.webapi.Controllers
{
    [RoutePrefix("api/action-permissions")]
    public class ActionPermissionController : ApiController
    {
        private IActionPermissionService _actionPermissionService;

        [Inject]
        public ILogger Logger { get; set; }

        public ActionPermissionController(IActionPermissionService actionPermissionService)
        {
            if (actionPermissionService == null)
            {
                throw new ArgumentNullException(nameof(actionPermissionService));
            }
            _actionPermissionService = actionPermissionService;
            Logger = new FakeLogger();
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            return Get(string.Empty);
        }

        [HttpGet]
        [Route("{module}")]
        public HttpResponseMessage Get(string module)
        {
            Logger.Debug($"ActionPermissionController.Get({module})");
            try
            {
                var actions = _actionPermissionService.GetUserModules(User).Select(m => m.Code).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, actions);
            }
            catch (Exception e)
            {

                Logger.Error($"ActionPermissionController.Get(\"{module}\")", e);
            }

            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
