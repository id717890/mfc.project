using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;

using System;
using System.Linq;
using System.Net;

namespace mfc.webapi.Controllers
{
    using domain.services;
    using Models;

    public class OrganizationController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var organizationService = CompositionRoot.Resolve<IOrganizationService>();
            var output = organizationService.GetAllOrganizations().Select(x => new OrganizationInfo(x));

            return Request.CreateResponse(HttpStatusCode.OK, output);
        }

        // GET: api/organization/:id
        public HttpResponseMessage Get(int id)
        {
            var organizationService = CompositionRoot.Resolve<IOrganizationService>();

            var output = organizationService.GetOrganizationById(id);
            return output != null ?
                Request.CreateResponse(HttpStatusCode.OK, new OrganizationInfo(output)) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST: api/organization
        public HttpResponseMessage Post([FromBody]OrganizationInfo value)
        {
            var organizationService = CompositionRoot.Resolve<IOrganizationService>();
            var identifier = organizationService.CreateType(value.Caption);

            var response = Request.CreateResponse(HttpStatusCode.Created, new Uri(Request.RequestUri + "/" + identifier.ToString()), MediaTypeHeaderValue.Parse("application/json"));
            response.Headers.Location = new Uri(Request.RequestUri + identifier.ToString());

            return response;
        }

        // PUT: api/organization/:id
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]OrganizationInfo value)
        {
            var organizationService = CompositionRoot.Resolve<IOrganizationService>();
            var organization = organizationService.GetOrganizationById(id);

            if (organization != null)
            {
                organization.Caption = value.Caption;
                organization.FullCaption = value.FullCaption;
                organization.Type = value.OrganizationType.ConvertToOrganizationType();
                organizationService.UpdateOgranization(organization);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // DELETE: api/organization/:id
        public HttpResponseMessage Delete(int id)
        {
            var organizationService = CompositionRoot.Resolve<IOrganizationService>();
            var organization = organizationService.GetOrganizationById(id);

            if (organization != null)
            {
                organizationService.DeleteOrganization(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}