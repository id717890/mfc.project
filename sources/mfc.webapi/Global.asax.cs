using mfc.infrastructure.services;
using mfc.webapi.App_Start;
using System;
using System.Configuration;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Mvc;

namespace mfc.webapi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            //фэйковый пользователь
            //todo: Реализация авторизации пользователя по access-токену
            Context.User = new GenericPrincipal(new GenericIdentity("admin"), new string[] { });
            if (Context.User == null)
            {
                
            }
        }
    }
}
