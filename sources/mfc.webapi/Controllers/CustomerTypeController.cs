using mfc.domain.services;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace mfc.webapi.Controllers
{
    [RoutePrefix("api/customer-types")]
    public class CustomerTypeController : ApiController
    {
        private readonly ICustomerTypeService _customerTypeService;

        public CustomerTypeController(ICustomerTypeService customerTypeService)
        {
            _customerTypeService = customerTypeService;
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var types = _customerTypeService.GetAllTypes().Select(type => new Models.CustomerType(type)).ToArray();
            if (types == null || types.Length == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, types);
        }

        [Route("{id}")]
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var type = _customerTypeService.GetTypeById(id);
            if (type == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, new Models.CustomerType(type));
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]Models.CustomerType value)
        {
            var id = _customerTypeService.Create(value.Caption);

            var response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri + id.ToString());

            return response;
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]Models.CustomerType value)
        {
            var type = _customerTypeService.GetTypeById(id);

            if (type != null)
            {
                type.Caption = value.Caption;
                _customerTypeService.Update(type);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            var type = _customerTypeService.GetTypeById(id);

            if (type != null)
            {
                _customerTypeService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
