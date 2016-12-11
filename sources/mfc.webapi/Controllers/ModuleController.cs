using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Linq;

namespace mfc.webapi.Controllers
{
    using Models;
    using domain.services;
    using AutoMapper;
    using System.Collections.Generic;
    [RoutePrefix("api/modules")]
    public class ModuleController : ApiController
    {
        private IModuleService _moduleService;
        private IMapper _mapper;
        public ModuleController(IModuleService moduleService, IMapper mapper)
        {
            if (moduleService == null)
            {
                throw new ArgumentNullException(nameof(moduleService));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            _moduleService = moduleService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            var output = _mapper.Map<IEnumerable<ModuleModel>>(_moduleService.GetRoot());

            return output != null ?
                Request.CreateResponse(HttpStatusCode.OK, output) :
            Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [Route("{code}")]
        public HttpResponseMessage Get(string code)
        {
            var output = _moduleService.GetByCode(code);

            return output != null ?
                Request.CreateResponse(HttpStatusCode.OK, _mapper.Map<ModuleModel>(output)) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
