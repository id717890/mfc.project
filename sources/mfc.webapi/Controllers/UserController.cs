using mfc.domain.services;
using mfc.webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                    Description = user.Name
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
                return Request.CreateResponse(HttpStatusCode.Found, new AccountInfo(user));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        // POST: api/User
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
