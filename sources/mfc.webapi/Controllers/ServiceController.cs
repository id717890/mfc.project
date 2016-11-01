using mfc.domain.services;
using System;
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

        public ServiceController(IServiceService customerTypeService, IOrganizationService organizationService)
        {
            _servicesService = customerTypeService;
            _organizationService = organizationService;
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            var services = _servicesService.GetAllServices().Select(service => new Models.ServiceInfo(service)).ToArray();
            if (services == null || services.Length == 0)
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
            var services = _servicesService.GetOrganizationServices(organization).Select(x=>new Models.ServiceInfo(x)).ToArray();
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
            return Request.CreateResponse(HttpStatusCode.OK, new Models.ServiceInfo(service));
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Models.ServiceInfo value)
        {
            var id = _servicesService.Create(value.Caption, value.OrganistaionId);

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