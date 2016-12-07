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
        protected void Application_Start()
        {
            AutoMapperConfig.Configure();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            //Настройка параметров подключения к серверу СУБД
            string conntion_string_name = ConfigurationManager.AppSettings["ConnectionStringName"];
            string connection_string = ConfigurationManager.ConnectionStrings[conntion_string_name].ConnectionString;
            CompositionRoot.Resolve<ISqlProvider>().Init(connection_string);

            log4net.Config.XmlConfigurator.Configure();
        }

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
