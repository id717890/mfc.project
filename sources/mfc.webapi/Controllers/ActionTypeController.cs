using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace mfc.webapi.Controllers
{
    using Models;
    using domain.services;
    using System.Linq;

    public class ActionTypeController : ApiController
    {
        // GET: api/actiontype
        public HttpResponseMessage Get()
        {
            var actionTypeService = CompositionRoot.Resolve<IActionTypeService>();

            var output = actionTypeService.GetAllTypes();
            return output != null ?
                Request.CreateResponse(HttpStatusCode.OK, output.Select( x => new ActionTypeInfo(x))) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // GET: api/actiontype/:id
        public HttpResponseMessage Get(int id)
        {
            var actionTypeService = CompositionRoot.Resolve<IActionTypeService>();

            var output = actionTypeService.GetTypeById(id);
            return output != null ?
                Request.CreateResponse(HttpStatusCode.OK, new ActionTypeInfo(output)) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST: api/actiontype
        public HttpResponseMessage Post([FromBody]ActionTypeInfo value)
        {
            var actionTypeService = CompositionRoot.Resolve<IActionTypeService>();
            var identifier = actionTypeService.Create(value.Caption, value.NeedMakeFile);

            var response = Request.CreateResponse(HttpStatusCode.Created, new Uri(Request.RequestUri + "/" + identifier.ToString()), MediaTypeHeaderValue.Parse("application/json"));
            response.Headers.Location = new Uri(Request.RequestUri + identifier.ToString());

            return response;
        }

        // PUT: api/actiontype/:id
        [HttpPut]
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

        // DELETE: api/actiontype/:id
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
