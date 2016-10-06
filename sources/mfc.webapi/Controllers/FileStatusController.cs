using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace mfc.webapi.Controllers
{
    using Models;
    using domain.services;
    using System;
    using System.Net.Http.Headers;
    using System.Net.Http.Formatting;

    public class FileStatusController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var fileStatusService = CompositionRoot.Resolve<IFileStatusService>();
            var output = fileStatusService.GetAllStatuses().Select(x => new FileStatusInfo(x));

            return Request.CreateResponse(HttpStatusCode.OK, output);
        }

        // GET: api/filestatus/5
        public HttpResponseMessage Get(int id)
        {
            var fileStatusService = CompositionRoot.Resolve<IFileStatusService>();

            var output = fileStatusService.GetStatusById(id);
            return output != null ? 
                Request.CreateResponse(HttpStatusCode.OK, new FileStatusInfo(output)) : 
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST: api/filestatus
        public HttpResponseMessage Post([FromBody]FileStatusInfo value)
        {
            var fileStatusService = CompositionRoot.Resolve<IFileStatusService>();
            var identifier = fileStatusService.Create(value.Caption);

            var response = Request.CreateResponse(HttpStatusCode.Created, new Uri(Request.RequestUri + "/" + identifier.ToString()), MediaTypeHeaderValue.Parse("application/json"));
            response.Headers.Location = new Uri(Request.RequestUri + identifier.ToString());
            
            return response;
        }

        // PUT: api/filestatus/:id
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]FileStatusInfo value)
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

        // DELETE: api/filestatus/:id
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
