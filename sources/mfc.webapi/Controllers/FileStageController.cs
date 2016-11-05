using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using mfc.domain.entities;
using mfc.domain.services;
using mfc.webapi.Models;

namespace mfc.webapi.Controllers
{
    [RoutePrefix("api/file-stages")]
    public class FileStageController : ApiController
    {
        private readonly IFileStageService _fileStageService;
        private readonly IMapper _mapper;

        public FileStageController(IFileStageService fileStageService, IMapper mapper)
        {
            _fileStageService = fileStageService;
            _mapper = mapper;
        }

        // GET: api/file-stages
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            var fileStageList = _mapper.Map<IEnumerable<FileStageModel>>(_fileStageService.GetAllStages());
            return Request.CreateResponse(HttpStatusCode.OK, fileStageList);
        }

        // GET: api/file-stages/1
        [HttpGet]
        [Route("{code}")]
        public HttpResponseMessage Get(string code)
        {
            var output = _fileStageService.GetStage(code);
            return output != null ?
                Request.CreateResponse(HttpStatusCode.OK, _mapper.Map<FileStageModel>(output)) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // PUT: api/file-stages/:code
        [HttpPut]
        [Route("{code}")]
        public HttpResponseMessage Put(string code, [FromBody]FileStageModel value)
        {
            var fileStage = _fileStageService.GetStage(code);
            if (fileStage != null)
            {
                fileStage.Status = _mapper.Map<FileStatus>(value.Status);
                _fileStageService.UpdateStage(fileStage);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST: api/file-stages
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]FileStageModel value)
        {
            return Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
        }

        // DELETE: api/file-stages/:code
        [HttpDelete]
        [Route("")]
        public HttpResponseMessage Delete(string code)
        {
            return Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
        }
    }
}