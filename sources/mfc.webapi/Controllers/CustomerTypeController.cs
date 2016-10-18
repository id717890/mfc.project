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

    public class CustomerTypeController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var customerTypeService = CompositionRoot.Resolve<ICustomerTypeService>();
            var output = customerTypeService.GetAllTypes().Select(x => new CustomerTypeInfo(x));

            return Request.CreateResponse(HttpStatusCode.OK, output);
        }

        // GET: api/customertype/:id
        public HttpResponseMessage Get(int id)
        {
            var customerTypeService = CompositionRoot.Resolve<ICustomerTypeService>();

            var output = customerTypeService.GetTypeById(id);
            return output != null ?
                Request.CreateResponse(HttpStatusCode.OK, new CustomerTypeInfo(output)) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST: api/customertype
        public HttpResponseMessage Post([FromBody]CustomerTypeInfo value)
        {
            var customerTypeService = CompositionRoot.Resolve<ICustomerTypeService>();
            var identifier = customerTypeService.Create(value.Caption);

            var response = Request.CreateResponse(HttpStatusCode.Created, new Uri(Request.RequestUri + "/" + identifier.ToString()), MediaTypeHeaderValue.Parse("application/json"));
            response.Headers.Location = new Uri(Request.RequestUri + identifier.ToString());

            return response;
        }

        // PUT: api/customertype/:id
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]CustomerTypeInfo value)
        {
            var customerTypeService = CompositionRoot.Resolve<ICustomerTypeService>();
            var customerType = customerTypeService.GetTypeById(id);

            if (customerType != null)
            {
                customerType.Caption = value.Caption;
                customerTypeService.Update(customerType);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // DELETE: api/customertype/:id
        public HttpResponseMessage Delete(int id)
        {
            var customerTypeService = CompositionRoot.Resolve<ICustomerTypeService>();
            var customerType = customerTypeService.GetTypeById(id);

            if (customerType != null)
            {
                customerTypeService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}