using System;
using System.Collections;
using System.Collections.Generic;
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
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // GET: api/users
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            var users = _mapper.Map<IEnumerable<AccountModel>>(_userService.GetAllUsers());
            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        // GET: api/users/5
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _mapper.Map<AccountModel>(user));
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST: api/users
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]AccountModel value)
        {
            var find_user = _userService.GetUser(value.UserName);
            if (find_user != null)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, "Пользователь с таким именем уже существует");
            }
            var id = _userService.AddNew(value.UserName, value.Description, value.IsAdmin, value.IsExpert, value.IsController);
            var msg = Request.CreateResponse(HttpStatusCode.Created);
            msg.Headers.Location = new Uri(Request.RequestUri + "/" + id.ToString());
            return msg;
        }

        // PUT: api/users/5
        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Put(int id, [FromBody]AccountModel value)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return Request.CreateResponse(HttpStatusCode.NotFound);
            _userService.Update(_mapper.Map<User>(value));
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/users/5
        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                _userService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
