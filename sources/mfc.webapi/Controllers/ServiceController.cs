using AutoMapper;
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
    [RoutePrefix("api/services")]
    public class ServiceController : ApiController
    {
        private readonly IServiceService _servicesService;
        private readonly IOrganizationService _organizationService;
        private IMapper _mapper;

        public ServiceController(IServiceService customerTypeService, IOrganizationService organizationService)
        {
            _mapper = CompositionRoot.Resolve<IMapper>();
            _servicesService = customerTypeService;
            _organizationService = organizationService;
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            var services = _mapper.Map<IEnumerable<ServiceInfo>>(_servicesService.GetAllServices());
            if (services == null || !services.Any())
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, services);
        }

        // GET: api/services?organization=12
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(long organization)
        {
            var services = _mapper.Map<IEnumerable<ServiceInfo>>(_servicesService.GetOrganizationServices(organization));
            return services == null ? Request.CreateResponse(HttpStatusCode.NotFound) : Request.CreateResponse(HttpStatusCode.OK, services);
        }

        // GET: api/services?organization=12&parent=204
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(int organization, int parent)
        {
            var services = _mapper.Map<IEnumerable<ServiceInfo>>(_servicesService.GetOrganizationServices(organization,parent));
            return services == null ? Request.CreateResponse(HttpStatusCode.NotFound) : Request.CreateResponse(HttpStatusCode.OK, services);
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            var service = _servicesService.GetServiceById(id);
            if (service == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _mapper.Map<ServiceInfo>(service));
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Models.ServiceInfo value)
        {
            var id = _servicesService.Create(value.Caption, value.OrganizationId);

            var response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri($"{Request.RequestUri}/{id}");

            return response;
        }

        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Put(int id, [FromBody]Models.CustomerTypeInfo value)
        {
            var service = _servicesService.GetServiceById(id);

            if (service != null)
            {
                service.Caption = value.Caption;
                _servicesService.Update(service);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var service = _servicesService.GetServiceById(id);

            if (service != null)
            {
                _servicesService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}