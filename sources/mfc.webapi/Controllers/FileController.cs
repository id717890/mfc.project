using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using AutoMapper;

namespace mfc.webapi.Controllers
{
    using domain.services;
    using Models;

    [RoutePrefix("api/files")]
    public class FileController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public FileController(IMapper mapper, IFileService fileService)
        {
            _mapper = mapper;
            _fileService = fileService;
        }

        // GET: api/files?pageIndex=1&pageSize=50
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(int pageIndex, int pageSize, string beginDate = "-1", string endDate = "-1", int status = -1, int organization = -1, int service = -1, int expert = -1, int controller = -1)
        {
            DateTime queryDateBegin = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime queryDateEnd = queryDateBegin.AddMonths(1).AddSeconds(-1);

            if ((!string.IsNullOrEmpty(beginDate)) && (beginDate != "-1"))
            {
                DateTime.TryParse(beginDate, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out queryDateBegin);
            }

            if ((!string.IsNullOrEmpty(endDate)) && (endDate != "-1"))
            {
                DateTime.TryParse(endDate, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out queryDateEnd);
            }
            var output = _fileService.GetFiles(queryDateBegin, queryDateEnd, status, organization, service, expert, controller, pageIndex, pageSize);
            var files = _mapper.Map<IEnumerable<FileModel>>(output.Value);
            var response = Request.CreateResponse(HttpStatusCode.OK, files);
            response.Headers.Add("Total-rows", output.Key.ToString());
            return response;
        }





        public HttpResponseMessage Get([FromUri]DateTime beginDate, [FromUri]DateTime endDate, [FromUri]int controller, [FromUri]int expert, [FromUri]int status, [FromUri]int organization, [FromUri]int service)
        {
            var fileService = CompositionRoot.Resolve<IFileService>();
            var output = fileService.GetFiles(beginDate, endDate, controller, expert, status, organization, service).Select(x => new FileModel { });

            return Request.CreateResponse(HttpStatusCode.OK, output);
        }

        // GET: api/file/:id
        public HttpResponseMessage Get(int id)
        {
            var fileService = CompositionRoot.Resolve<IFileService>();

            var output = fileService.GetFileById(id);
            return output != null ?
                Request.CreateResponse(HttpStatusCode.OK, new FileModel(output)) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST: api/file
        public HttpResponseMessage Post([FromBody]FileModel value)
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
        public HttpResponseMessage Put(int id, [FromBody]FileModel value)
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