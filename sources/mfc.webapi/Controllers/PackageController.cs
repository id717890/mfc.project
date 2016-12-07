using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using mfc.domain.services;
using mfc.webapi.Models;

namespace mfc.webapi.Controllers
{
    [RoutePrefix("api/packages")]
    public class PackageController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IPackageService _packageService;

        public PackageController(IMapper mapper, IPackageService packageService)
        {
            _mapper = mapper;
            _packageService = packageService;
        }
        
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(int pageIndex, int pageSize, string beginDate="-1", string endDate="-1", int organization=-1, int controller=-1)
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
    }
}
