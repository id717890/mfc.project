using mfc.domain.services;
using mfc.webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace mfc.webapi.Controllers
{
    public class OrganizationTypeApiController : ApiController
    {
        // GET: api/OrganizationType
        public HttpResponseMessage Get()
        {
            var organization_service = CompositionRoot.Resolve<IOrganizationService>();
            List<OrganizationTypeModel> organizationTypes = new List<OrganizationTypeModel>();

            foreach (var type in organization_service.GetAllTypes())
            {
                organizationTypes.Add( new  OrganizationTypeModel(type));
            }

            return Request.CreateResponse(HttpStatusCode.OK, organizationTypes);
        }

        // GET: api/OrganizationType/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/OrganizationType
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/OrganizationType/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/OrganizationType/5
        public void Delete(int id)
        {
        }
    }
}
