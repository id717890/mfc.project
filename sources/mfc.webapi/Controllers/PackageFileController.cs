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
    [RoutePrefix("api/package-files")]
    public class PackageFileController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IPackageService _packageService;

        public PackageFileController(IMapper mapper, IPackageService packageService)
        {
            _mapper = mapper;
            _packageService = packageService;
        }

        // GET: api/package-files/202020
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var output = _packageService.GetPackageFiles(id);
                if (output != null && output.Any())
                {
                    var i = _mapper.Map<IEnumerable<FileModel>>(output);
                    return Request.CreateResponse(HttpStatusCode.OK, i);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
