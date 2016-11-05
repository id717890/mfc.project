﻿using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using mfc.domain.services;
using mfc.webapi.Models;

namespace mfc.webapi.Controllers
{
    [RoutePrefix("api/acceptions")]
    public class AcceptionController : ApiController
    {
        private readonly IActionService _actionService;
        private readonly IUserService _userService;

        public AcceptionController(IActionService actionService, IUserService userService)
        {
            _actionService = actionService;
            _userService = userService;
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            var queryDateBegin = DateTime.Today;
            var queryDateEnd = DateTime.Today.AddDays(1);
            var acceptions = _actionService.GetActions(queryDateBegin, queryDateEnd, null)
                .Select(x => new AcceptionModel(x)).ToList();
            var response = Request.CreateResponse(HttpStatusCode.OK, acceptions);
            response.Headers.Add("Total-rows", acceptions.Count().ToString());
            return response;
        }

        // GET: api/acceptions?pageIndex=1&pageSize=50
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(int pageIndex, int pageSize)
        {
            var queryDateBegin = DateTime.Today;
            var queryDateEnd = DateTime.Today.AddDays(1);
            var acceptions = _actionService.GetActions(queryDateBegin, queryDateEnd, null)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new AcceptionModel(x)).ToList();
            var response = Request.CreateResponse(HttpStatusCode.OK, acceptions);
            response.Headers.Add("Total-rows", acceptions.Count().ToString());
            return response;
        }

        // GET: api/acceptions?dateBegin='01.01.2016'&dateEnd='01.10.2016'&pageIndex=1&pageSize=50&userId=1000
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(string dateBegin, string dateEnd, int pageIndex, int pageSize, long userId)
        {
            var queryDateBegin = DateTime.Today;

            if (!string.IsNullOrEmpty(dateBegin))
            {
                DateTime.TryParse(dateBegin, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out queryDateBegin);
            }

            var queryDateEnd = DateTime.Today.AddDays(1);

            if (!string.IsNullOrEmpty(dateEnd))
            {
                DateTime.TryParse(dateEnd, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out queryDateEnd);
            }

            var user = _userService.GetUserById(userId);

            if (user == null)
            {
                var acceptions = _actionService.GetActions(queryDateBegin, queryDateEnd, null).ToList();
                var totalRows = acceptions.Count();
                var paging = acceptions
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new AcceptionModel(x)).ToList();
                var response = Request.CreateResponse(HttpStatusCode.OK, paging);
                response.Headers.Add("Total-rows", totalRows.ToString());
                return response;
            }
            else
            {
                var acceptions = _actionService.GetActions(user, queryDateBegin, queryDateEnd).ToList();
                var totalRows = acceptions.Count();
                var paging = acceptions
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new AcceptionModel(x)).ToList();
                var response = Request.CreateResponse(HttpStatusCode.OK, paging);
                response.Headers.Add("Total-rows", totalRows.ToString());
                return response;
            }
        }

        // GET: api/acceptions/5
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            var acception = _actionService.GetActionById(id);
            if (acception != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new AcceptionModel(acception));
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST: api/acceptions
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]AcceptionModel value)
        {
//            return Request.CreateResponse(HttpStatusCode.OK);
            var id = _actionService.Add(
                value.Date,
                1,
                value.Customer,
                value.ActionType.Id,
                value.CustomerType.Id,
                value.User.Id,
                1, value.IsNonresident, value.FreeVisit, value.Comments
                );
            var msg = Request.CreateResponse(HttpStatusCode.Created);
            msg.Headers.Location = new Uri(Request.RequestUri + "/" + id.ToString());
            return msg;
        }

        // DELETE: api/acceptions/5
        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var acception = _actionService.GetActionById(id);
            if (acception != null)
            {
                _actionService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
