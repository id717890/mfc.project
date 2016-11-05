using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using mfc.domain.services;
using mfc.webapi.Models;

namespace mfc.webapi.Controllers
{
    [RoutePrefix("api/file-stages")]
    public class FileStageController : ApiController
    {
        // GET: api/filestage
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            var fileStageService = CompositionRoot.Resolve<IFileStageService>();
            var fileStageList = fileStageService.GetAllStages().Select(x => new FileStageModel(x));

            return Request.CreateResponse(HttpStatusCode.OK, fileStageList);
        }

        // GET: api/filestage/1
        [HttpGet]
        [Route("{code}")]
        public HttpResponseMessage Get(string code)
        {
            var fileStageService = CompositionRoot.Resolve<IFileStageService>();
            var output = fileStageService.GetStage(code);
            return output != null ?
                Request.CreateResponse(HttpStatusCode.OK, new FileStageModel(output)) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // PUT: api/filestage/:code
        [HttpPut]
        [Route("{code}")]
        public HttpResponseMessage Put(string code, [FromBody]FileStageModel value)
        {
            var fileStageService = CompositionRoot.Resolve<IFileStageService>();
            var fileStage = fileStageService.GetStage(code);

            if (fileStage != null)
            {
                fileStage.Status = value.Status.ConvertToFileStatus();
                fileStageService.UpdateStage(fileStage);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST: api/filestage
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]FileStageModel value)
        {
            return Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
        }

        // DELETE: api/filestage/:code
        [HttpDelete]
        [Route("")]
        public HttpResponseMessage Delete(string code)
        {
            return Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
        }
    }
}