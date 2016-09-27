using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using mfc.domain;
using mfc.domain.services;
using mfc.infrastructure.services;
using mfc.webapi.Models;
using Ninject;

namespace mfc.webapi.Controllers
{
    public class PasswordController : ApiController
    {
        // PUT: api/password/5
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody] string value)
        {
            try
            {
                var userService = CompositionRoot.Resolve<IUserService>();

                /*
                    Здесь нужно делать проверку через User.Identity.Name, 
                    но для этого нужно реализовать авторизацию на уровне API.

                    А пока делаем проверку через вытаскивание идентификационных данных о текущем пользователе из cookie,
                    т.к. при авторизации клиента данные об учетке пишутся именно туда.
                */

                //Вытаскиваем текущего пользователя из cookie
                // Retrieves the cookie that contains your custom 
                HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie == null) return Request.CreateResponse(HttpStatusCode.Unauthorized,"Не выполнена авторизация");
                // Decrypts the FormsAuthenticationTicket that is held in the cookie's .Value property.
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                // The "authTicket" variable now contains your original, custom FormsAuthenticationTicket,
                // complete with User-specific custom data.  You can then check that the FormsAuthenticationTicket's
                // .Name property is for the correct user, and perform the relevant functions with the ticket.
                // Here, we simply write the user-specific data to the Http Response stream.
                var currentUser = userService.GetUser(authTicket.Name);
                if (currentUser == null) return Request.CreateResponse(HttpStatusCode.NotFound, "Текущий пользователь не обнаружен в базе данных");
                
                //Если текущий пользователь Admin он меняет пароль у любого другого пользователя
                if (currentUser.IsAdmin)
                {
                    userService.SetPassword(id, value);
                    return Request.CreateResponse(HttpStatusCode.OK,@"Пароль пользователя успешно изменен");
                }

                //Если текущий пользователь НЕ Admin он может меняет пароль только у себя
                var userForUpdate = userService.GetUserById(id);
                if (currentUser.Id == userForUpdate.Id)
                {
                    userService.SetPassword(id, value);
                    return Request.CreateResponse(HttpStatusCode.OK, "Пароль успешно изменен");
                }

                //При попытке смены пароля НЕ админом у другого пользователя, выдавать ошибку 403
                return Request.CreateResponse(HttpStatusCode.Forbidden, "Доступ запрещен!\\nИзменять пароли других пользователей могут только администраторы");
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ошибка смены пароля пользователя");
            }
        }
    }
}
