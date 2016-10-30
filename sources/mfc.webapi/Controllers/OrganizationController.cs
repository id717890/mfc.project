﻿using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;

using System;
using System.Linq;
using System.Net;

namespace mfc.webapi.Controllers
{
    using domain.services;
    using Models;

    [RoutePrefix("api/organizations")]
    public class OrganizationController : ApiController
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            var output = _organizationService.GetAllOrganizations().Select(x => new OrganizationInfo(x));

            return Request.CreateResponse(HttpStatusCode.OK, output);
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            var output = _organizationService.GetOrganizationById(id);
            return output != null ?
                Request.CreateResponse(HttpStatusCode.OK, new OrganizationInfo(output)) :
                Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]OrganizationInfo value)
        {
            //todo: обработка ошибок и правильный возврат кода HTTP.
            var identifier = _organizationService.CreateOrganization(value.Caption, value.FullCaption, value.OrganizationType.Id);

            var locationUri = new Uri($"{Request.RequestUri}/{identifier}");
            var response = Request.CreateResponse(HttpStatusCode.Created, locationUri, MediaTypeHeaderValue.Parse("application/json"));
            response.Headers.Location = locationUri;

            return response;
        }

        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Put(int id, [FromBody]OrganizationInfo value)
        {
            //todo: обработка ошибок и возврат кода HTTP в соответствии с соглашением
            var organization = _organizationService.GetOrganizationById(id);

            if (organization != null)
            {
                organization.Caption = value.Caption;
                organization.FullCaption = value.FullCaption;
                organization.Type = value.OrganizationType.ConvertToOrganizationType();
                _organizationService.UpdateOgranization(organization);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            //todo: обработка ошибок и возврат кода HTTP в соответствии с соглашением
            var organization = _organizationService.GetOrganizationById(id);

            if (organization != null)
            {
                _organizationService.DeleteOrganization(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}