using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace mfc.web.Controllers
{
    using AutoMapper;
    using domain.services;
    using System.Collections.Generic;
    using webapi;
    using webapi.Models;

    public class OrganizationTypeController : ApiController
    {
        private IMapper _mapper;
        private IOrganizationService organizationService;

        public OrganizationTypeController()
        {
            _mapper = CompositionRoot.Resolve<IMapper>();
            organizationService = CompositionRoot.Resolve<IOrganizationService>();
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _mapper.Map<IEnumerable<OrganizationTypeModel>>(organizationService.GetAllTypes()));
        }

        // GET: api/organizationtype/:id
        public HttpResponseMessage Get(int id)
        {
            var output = organizationService.GetTypeById(id);
            return output != null ?
                Request.CreateResponse(HttpStatusCode.OK, _mapper.Map<OrganizationTypeModel>(output)) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST: api/organizationtype
        public HttpResponseMessage Post([FromBody]OrganizationTypeModel value)
        {
            var identifier = organizationService.CreateType(value.Caption);

            var response = Request.CreateResponse(HttpStatusCode.Created, new Uri(Request.RequestUri + "/" + identifier.ToString()), MediaTypeHeaderValue.Parse("application/json"));
            response.Headers.Location = new Uri(Request.RequestUri + "/" + identifier.ToString());

            return response;
        }

        // PUT: api/organizationtype/:id
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]OrganizationTypeModel value)
        {
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
