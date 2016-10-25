using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Linq;

namespace mfc.webapi.Controllers
{
    using Models;
    using domain.services;

    [RoutePrefix("api/action-types")]
    public class ActionTypeController : ApiController
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            var actionTypeService = CompositionRoot.Resolve<IActionTypeService>();

            var output = actionTypeService.GetAllTypes();
            return output != null ?
                Request.CreateResponse(HttpStatusCode.OK, output.Select( x => new ActionTypeInfo(x))) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            var actionTypeService = CompositionRoot.Resolve<IActionTypeService>();

            var output = actionTypeService.GetTypeById(id);
            return output != null ?
                Request.CreateResponse(HttpStatusCode.OK, new ActionTypeInfo(output)) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]ActionTypeInfo value)
        {
            var actionTypeService = CompositionRoot.Resolve<IActionTypeService>();
            var identifier = actionTypeService.Create(value.Caption, value.NeedMakeFile);

            var response = Request.CreateResponse(HttpStatusCode.Created, new Uri(Request.RequestUri + "/" + identifier.ToString()), MediaTypeHeaderValue.Parse("application/json"));
            response.Headers.Location = new Uri(Request.RequestUri + identifier.ToString());

            return response;
        }

        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Put(int id, [FromBody]ActionTypeInfo value)
        {
            var actionTypeService = CompositionRoot.Resolve<IActionTypeService>();
            var actionType = actionTypeService.GetTypeById(id);

            if (actionType != null)
            {
                actionType.Caption = value.Caption;
                actionType.NeedMakeFile = value.NeedMakeFile;
                actionTypeService.Update(actionType);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var actionTypeService = CompositionRoot.Resolve<IActionTypeService>();
            var actionType = actionTypeService.GetTypeById(id);

            if (actionType != null)
            {
                actionTypeService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
