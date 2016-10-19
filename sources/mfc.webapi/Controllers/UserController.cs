using mfc.domain;
using mfc.domain.services;
using mfc.webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace mfc.webapi.Controllers
{
    public class UserController : ApiController
    {
        // GET: api/User

        public HttpResponseMessage Get()
        {
            var user_service = CompositionRoot.Resolve<IUserService>();
            var users = new List<AccountInfo>();

            foreach (var user in user_service.GetAllUsers())
            {
                users.Add(new AccountInfo
                {
                    Id = user.Id,
                    UserName = user.Account,
                    Description = user.Name,
                    IsAdmin = user.IsAdmin,
                    IsExpert = user.IsExpert,
                    IsController = user.IsController
                });
            }

            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        // GET: api/User/5
        public HttpResponseMessage Get(int id)
        {
            var user_service = CompositionRoot.Resolve<IUserService>();
            var user = user_service.GetUserById(id);

            if (user != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new AccountInfo(user));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        // POST: api/User
        public HttpResponseMessage Post([FromBody]AccountInfo value)
        {
            var user_service = CompositionRoot.Resolve<IUserService>();

            var find_user = user_service.GetUser(value.UserName);
            if (find_user != null)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, "Пользователь с таким именем уже существует");
            }
            else
            {
                var id = user_service.AddNew(value.UserName, value.Description, value.IsAdmin, value.IsExpert, value.IsController);
                var msg = Request.CreateResponse(HttpStatusCode.Created);
                msg.Headers.Location = new Uri(Request.RequestUri + "/" + id.ToString());
                return msg;
            }
        }

        // PUT: api/User/5
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]AccountInfo value)
        {
            var user_service = CompositionRoot.Resolve<IUserService>();
            var user = user_service.GetUserById(id);

            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                user_service.Update(value.ConvertToUser());
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        // DELETE: api/User/5
        public HttpResponseMessage Delete(int id)
        {
            var user_service = CompositionRoot.Resolve<IUserService>();
            var user = user_service.GetUserById(id);

            if (user != null)
            {
                user_service.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
