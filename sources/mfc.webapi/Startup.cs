using mfc.infrastructure.services;
using mfc.webapi.App_Start;
using Microsoft.Owin;
using Owin;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(mfc.webapi.Startup))]

namespace mfc.webapi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
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
            //ConfigureAuth(app);
        }
    }
}
