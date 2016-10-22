using System.Net.Http;
using System.Web.Http;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System;

namespace mfc.web.Controllers
{
    using domain.services;
    using webapi;
    using webapi.Models;

    public class OrganizationTypeController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var organizationService = CompositionRoot.Resolve<IOrganizationService>();
            var output = organizationService.GetAllTypes().Select(x => new OrganizationTypeInfo(x));

            return Request.CreateResponse(HttpStatusCode.OK, output);
        }

        // GET: api/organizationtype/:id
        public HttpResponseMessage Get(int id)
        {
            var organizationService = CompositionRoot.Resolve<IOrganizationService>();

            var output = organizationService.GetTypeById(id);
            return output != null ?
                Request.CreateResponse(HttpStatusCode.OK, new OrganizationTypeInfo(output)) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST: api/organizationtype
        public HttpResponseMessage Post([FromBody]OrganizationTypeInfo value)
        {
            var organizationService = CompositionRoot.Resolve<IOrganizationService>();
            var identifier = organizationService.CreateType(value.Caption);

            var response = Request.CreateResponse(HttpStatusCode.Created, new Uri(Request.RequestUri + "/" + identifier.ToString()), MediaTypeHeaderValue.Parse("application/json"));
            response.Headers.Location = new Uri(Request.RequestUri + "/" + identifier.ToString());

            return response;
        }

        // PUT: api/organizationtype/:id
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]OrganizationTypeInfo value)
        {
            var organizationService = CompositionRoot.Resolve<IOrganizationService>();
            var organizationType = organizationService.GetTypeById(id);

            if (organizationType != null)
            {
                organizationType.Caption = value.Caption;
                organizationService.UpdateType(organizationType);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // DELETE: api/organizationtype/:id
        public HttpResponseMessage Delete(int id)
        {
            var organizationService = CompositionRoot.Resolve<IOrganizationService>();
            var organizationType = organizationService.GetTypeById(id);

            if (organizationType != null)
            {
                organizationService.DeleteType(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
