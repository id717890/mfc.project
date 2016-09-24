using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Web.Http;
using mfc.domain;
using mfc.domain.services;
using mfc.webapi.Models;

namespace mfc.webapi.Controllers
{
    public class PasswordController : ApiController
    {
        // PUT: api/password/5
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody] string value)
        {
            var user_service = CompositionRoot.Resolve<IUserService>();
//            return Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                user_service.SetPassword(id, value);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, "Ошибка");
            }
        }
    }
}
