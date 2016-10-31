using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using mfc.domain.services;
using mfc.webapi.Models;

namespace mfc.webapi.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            var users = _userService.GetAllUsers().Select(user => new AccountModel
            {
                Id = user.Id,
                UserName = user.Account,
                Description = user.Name,
                IsAdmin = user.IsAdmin,
                IsExpert = user.IsExpert,
                IsController = user.IsController
            }).ToList();
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
                return Request.CreateResponse(HttpStatusCode.OK, new AccountModel(user));
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
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            _userService.Update(value.ConvertToUser());
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
