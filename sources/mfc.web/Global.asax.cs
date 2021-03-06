﻿using mfc.infrastructure.services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace mfc.web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //Настройка параметров подключения к серверу СУБД
            string conntion_string_name = ConfigurationManager.AppSettings["ConnectionStringName"];
            string connection_string = ConfigurationManager.ConnectionStrings[conntion_string_name].ConnectionString;
            CompositionRoot.Resolve<ISqlProvider>().Init(connection_string);

            log4net.Config.XmlConfigurator.Configure(); 
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e) {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || authCookie.Value == "")
                return;

            FormsAuthenticationTicket authTicket;
            try {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch {
                return;
            }

            // retrieve roles from UserData
            string[] roles = authTicket.UserData.Split(';');

            if (Context.User != null) {
                Context.User = new GenericPrincipal(Context.User.Identity, roles);
            }
        }
    }
}