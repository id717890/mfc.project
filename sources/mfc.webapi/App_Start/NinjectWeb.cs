[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(mfc.webapi.App_Start.NinjectWeb), "Start")]

namespace mfc.webapi.App_Start
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject.Web;
    using Ninject.Web.Common;
    public static class NinjectWeb 
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
        }
    }
}
