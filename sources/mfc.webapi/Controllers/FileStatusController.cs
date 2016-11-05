using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace mfc.webapi.Controllers
{
    using Models;
    using domain.services;

    [RoutePrefix("api/file-statuses")]
    public class FileStatusController : ApiController
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            var fileStatusService = CompositionRoot.Resolve<IFileStatusService>();
            var output = fileStatusService.GetAllStatuses().Select(x => new FileStatusModel(x));

            return Request.CreateResponse(HttpStatusCode.OK, output);
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            var fileStatusService = CompositionRoot.Resolve<IFileStatusService>();

            var output = fileStatusService.GetStatusById(id);
            return output != null ? 
                Request.CreateResponse(HttpStatusCode.OK, new FileStatusModel(output)) : 
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]FileStatusModel value)
        {
            var fileStatusService = CompositionRoot.Resolve<IFileStatusService>();
            var identifier = fileStatusService.Create(value.Caption);

            var location = new Uri(Request.RequestUri + "/" + identifier.ToString());
            var response = Request.CreateResponse(HttpStatusCode.Created, location, MediaTypeHeaderValue.Parse("application/json"));
            response.Headers.Location = location;

            return response;
        }

        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Put(int id, [FromBody]FileStatusModel value)
        {
            var fileStatusService = CompositionRoot.Resolve<IFileStatusService>();
            var fileStatus = fileStatusService.GetStatusById(id);
            
            if (fileStatus != null)
            {
                fileStatus.Caption = value.Caption;
                fileStatusService.Update(fileStatus);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var fileStatusService = CompositionRoot.Resolve<IFileStatusService>();
            var fileStatus = fileStatusService.GetStatusById(id);

            if (fileStatus != null)
            {
                fileStatusService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
