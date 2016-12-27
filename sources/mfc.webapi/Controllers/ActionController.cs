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
    [RoutePrefix("api/actions")]
    public class ActionController : ApiController
    {
        private readonly IActionService _actionService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ActionController(IActionService actionService, IUserService userService, IMapper mapper)
        {
            _actionService = actionService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            var queryDateBegin = DateTime.Today;
            var queryDateEnd = DateTime.Today.AddDays(1);
            var actions = _mapper.Map<IEnumerable<ActionModel>>(_actionService.GetActions(queryDateBegin, queryDateEnd, null)).ToList();
            var response = Request.CreateResponse(HttpStatusCode.OK, actions);
            response.Headers.Add("Total-rows", actions.Count().ToString());

            return response;
        }

        // GET: api/actions?pageIndex=1&pageSize=50
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(int pageIndex, int pageSize)
        {
            var queryDateBegin = DateTime.Today;
            var queryDateEnd = DateTime.Today.AddDays(1);
            var output = _actionService.GetActions(queryDateBegin, queryDateEnd, pageIndex, pageSize);
            var actions = _mapper.Map<IEnumerable<ActionModel>>(output.Value);
            var response = Request.CreateResponse(HttpStatusCode.OK, actions);
            response.Headers.Add("Total-rows", output.Key.ToString());
            return response;
        }

        // GET: api/actions?dateBegin='01.01.2016'&dateEnd='01.10.2016'&pageIndex=1&pageSize=50&userId=1000
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
                var output = _actionService.GetActions(queryDateBegin, queryDateEnd, pageIndex, pageSize);
                var actions = _mapper.Map<IEnumerable<ActionModel>>(output.Value).ToList();
                var totalRows = output.Key;
                var response = Request.CreateResponse(HttpStatusCode.OK, actions);
                response.Headers.Add("Total-rows", totalRows.ToString());
                return response;
            }
            else
            {
                var output = _actionService.GetActions(user, queryDateBegin, queryDateEnd, pageIndex, pageSize);
                var actions = _mapper.Map<IEnumerable<ActionModel>>(output.Value).ToList();
                var totalRows = output.Key;
                var response = Request.CreateResponse(HttpStatusCode.OK, actions);
                response.Headers.Add("Total-rows", totalRows.ToString());
                return response;
            }
        }

        // GET: api/actions/5
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            var actions = _actionService.GetActionById(id);
            return actions != null ? Request.CreateResponse(HttpStatusCode.OK, _mapper.Map<ActionModel>(actions)) : Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST: api/actions
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]ActionModel value)
        {
            var id = _actionService.Add(
                value.Date,
                value.Service.Id,
                value.Customer,
                value.ActionType.Id,
                value.CustomerType.Id,
                value.User.Id,
                value.ServiceChild?.Id ?? -1,
                value.IsNonresident,
                value.FreeVisit,
                value.Comments
                );
            var msg = Request.CreateResponse(HttpStatusCode.Created);
            msg.Headers.Location = new Uri(Request.RequestUri + "/" + id.ToString());
            return msg;
        }

        // POST: api/actions/copy
        [HttpPost]
        [Route("copy")]
        public HttpResponseMessage Post([FromBody] IEnumerable<ActionModel> values)
        {
            if (values == null || !values.Any()) return Request.CreateResponse(HttpStatusCode.NoContent);
            var copyList = values.Select(value => _mapper.Map<ActionModel>(_actionService.GetActionById(_actionService.Add(value.Date, value.Service.Id, value.Customer, value.ActionType.Id, value.CustomerType.Id, value.User.Id, value.ServiceChild?.Id ??-1, value.IsNonresident, value.FreeVisit, value.Comments)))).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, copyList);
        }

        // PUT: api/actions/5
        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Put(int id, [FromBody]ActionModel value)
        {
            var actions = _actionService.GetActionById(id);
            if (actions == null) return Request.CreateResponse(HttpStatusCode.NotFound);

            //            _actionService.Update(_mapper.Map<ServiceAction>(value));  // Так не работает, Hibernate выдает ошибку "a different object with the same identifier value was already associated with the session"

            /* А так работает */
            actions.Date = value.Date;
            actions.Customer = value.Customer;
            actions.CustomerType = _mapper.Map<CustomerType>(value.CustomerType);
            actions.Service = _mapper.Map<Service>(value.Service);
            actions.ServiceChild = _mapper.Map<Service>(value.ServiceChild);
            actions.Type = _mapper.Map<ActionType>(value.ActionType);
            actions.User = _mapper.Map<User>(value.User);
            actions.Comments = value.Comments;
            actions.FreeVisit = value.FreeVisit;
            actions.IsNonresident = value.IsNonresident;
            _actionService.Update(actions);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/actions/5
        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var actions = _actionService.GetActionById(id);
            if (actions != null)
            {
                _actionService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
