using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using mfc.domain.services;
using mfc.webapi.Models;

namespace mfc.webapi.Controllers
{
    [RoutePrefix("api/file-history")]
    public class FileStatusHistoryController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IFileStatusService _fileHistoryService;

        public FileStatusHistoryController(IMapper mapper, IFileStatusService fileStatusService)
        {
            _mapper = mapper;
            _fileHistoryService = fileStatusService;
        }

        // GET: api/file-history/5
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(long id)
        {
            var output = _fileHistoryService.GetFileStatuses(id);
            var historyFile = _mapper.Map<IEnumerable<FileStatusHistoryModel>>(output);
            var response = Request.CreateResponse(HttpStatusCode.OK, historyFile);
            response.Headers.Add("Total-rows", historyFile.Count().ToString());
            return response;
        }
    }
}
