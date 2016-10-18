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

    public class FileController : ApiController
    {
        public HttpResponseMessage Get([FromUri]DateTime beginDate, [FromUri]DateTime endDate, [FromUri]int controller, [FromUri]int expert, [FromUri]int status, [FromUri]int organization, [FromUri]int service)
        {
            var fileService = CompositionRoot.Resolve<IFileService>();
            var output = fileService.GetFiles(beginDate, endDate, controller, expert, status, organization, service).Select(x => new FileInfo { } );

            return Request.CreateResponse(HttpStatusCode.OK, output);
        }

        // GET: api/file/:id
        public HttpResponseMessage Get(int id)
        {
            var fileService = CompositionRoot.Resolve<IFileService>();

            var output = fileService.GetFileById(id);
            return output != null ?
                Request.CreateResponse(HttpStatusCode.OK, new FileInfo(output)) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST: api/file
        public HttpResponseMessage Post([FromBody]FileInfo value)
        {
            var fileService = CompositionRoot.Resolve<IFileService>();
            //var identifier = fileService.Add()

            //var response = Request.CreateResponse(HttpStatusCode.Created, new Uri(Request.RequestUri + "/" + identifier.ToString()), MediaTypeHeaderValue.Parse("application/json"));
            //response.Headers.Location = new Uri(Request.RequestUri + identifier.ToString());

            //return response;
            return null;
        }

        // PUT: api/file/:id
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]FileInfo value)
        {
            var fileService = CompositionRoot.Resolve<IFileService>();
            var fileInfo = fileService.GetFileById(id);

            if (fileInfo != null)
            {
                fileInfo.Caption = value.Caption;
                fileService.Update(fileInfo);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // DELETE: api/file/:id
        public HttpResponseMessage Delete(int id)
        {
            var fileService = CompositionRoot.Resolve<IFileService>();
            var fileInfo = fileService.GetFileById(id);

            if (fileInfo != null)
            {
                fileService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}