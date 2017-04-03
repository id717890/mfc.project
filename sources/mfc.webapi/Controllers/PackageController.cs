using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using mfc.domain.entities;
using mfc.domain.services;
using mfc.webapi.Models;

namespace mfc.webapi.Controllers
{
    [RoutePrefix("api/packages")]
    public class PackageController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IPackageService _packageService;
        private readonly IFileService _fileService;
        private readonly IFileStageService _fileStageService;
        private readonly IUserService _userService;


        public PackageController(
            IMapper mapper
            , IPackageService packageService
            , IFileService fileService
            , IFileStageService fileStageService
            , IUserService userService)
        {
            _mapper = mapper;
            _packageService = packageService;
            _fileService = fileService;
            _fileStageService = fileStageService;
            _userService = userService;
        }

        // GET: api/users/5
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            var package = _packageService.GetPackageById(id);
            return package != null ? Request.CreateResponse(HttpStatusCode.OK, _mapper.Map<PackageModel>(package)) : Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(int pageIndex, int pageSize, string beginDate = "-1", string endDate = "-1", int organization = -1, int controller = -1)
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
            var output = _packageService.GetPackages(queryDateBegin, queryDateEnd, organization, controller, pageIndex, pageSize);
            var packages = _mapper.Map<IEnumerable<PackageModel>>(output.Value);
            var response = Request.CreateResponse(HttpStatusCode.OK, packages);
            response.Headers.Add("Total-rows", output.Key.ToString());
            return response;
        }

        // PUT: api/packages/5
        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Put(int id, [FromBody]PackageModel value)
        {
            try
            {
                var package = _mapper.Map<Package>(value);
                var packageItems = _packageService.GetPackageFiles(id).ToList();
                if (!packageItems.Any()) throw new Exception("В данном пакете отсутствуют дела, выявить статус невозможно");
                _fileService.SetStage(packageItems.Select(x => x.Id), _fileStageService.GetStageByStatus(packageItems.Last().CurrentStatus.Id).Code, String.Empty);
                _packageService.Update(package);
                _packageService.UpdatePackageFiles(package.Id, packageItems.Select(x => x.Id));
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        // POST: api/package
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]PackageModel value)
        {
            var fileIds = value.Files.Select(item => item.Id).ToList();
            try
            {
                var id = _packageService.CreatePackage(_userService.GetCurrentUser(), value.Date, _mapper.Map<Organization>(value.Organization), fileIds, value.Comment);
                var msg = Request.CreateResponse(HttpStatusCode.Created);
                msg.Headers.Location = new Uri(Request.RequestUri + "/" + id.ToString());
                return msg;
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        // DELETE: api/packages/5
        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var package = _packageService.GetPackageById(id);
                if (package == null) return Request.CreateResponse(HttpStatusCode.NotFound);
                _packageService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
