using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using AutoMapper;
using mfc.domain.entities;
using Newtonsoft.Json;

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

        // GET: api/files/5
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(long id)
        {
            var file = _fileService.GetFileById(id);
            return file != null ? Request.CreateResponse(HttpStatusCode.OK, _mapper.Map<FileModel>(file)) : Request.CreateResponse(HttpStatusCode.NotFound);
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

        // POST: api/files/2/
        [HttpPost]
        [Route("{id}")]
        public HttpResponseMessage Post(int id, [FromBody]ChangeStatusModel value)
        {
            var file = _fileService.GetFileById(id);
            if (file == null) return Request.CreateResponse(HttpStatusCode.NotFound);

            switch (value.Status)
            {
                case FileStages.SendForControl:
                    _fileService.SendForControl(id, value.Comment);
                    break;
                case FileStages.ReturnForFix:
                    _fileService.Return(id, value.Comment);
                    break;
                case FileStages.Checked:
                    _fileService.Checked(id, string.Empty);
                    break;
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT: api/files/5
        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Put(int id, [FromBody]FileModel value)
        {
            var file = _fileService.GetFileById(id);
            if (file == null) return Request.CreateResponse(HttpStatusCode.NotFound);
            //            _fileService.Update(_mapper.Map<File>(value));  // Так не работает, Hibernate выдает ошибку "a different object with the same identifier value was already associated with the session"
            file.Caption = value.Caption;
            _fileService.Update(file);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT: api/files/controll
        [HttpPut]
        [Route("controll")]
        public HttpResponseMessage Put([FromBody]IEnumerable<FileModel> value)
        {
            var checkedFileIds = value.Select(x=>x.Id).Distinct();
            var acceptedIds = new List<Int64>(_fileService.AcceptForControl(checkedFileIds));
            var acceptModel = acceptedIds.Select(id => _mapper.Map<FileModel>(_fileService.GetFileById(id))).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, acceptModel);
        }

        // DELETE: api/files/:id
        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var file = _fileService.GetFileById(id);
            if (file == null) return Request.CreateResponse(HttpStatusCode.NotFound);
            _fileService.Delete(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}