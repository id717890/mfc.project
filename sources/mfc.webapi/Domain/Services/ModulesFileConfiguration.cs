using mfc.webapi.Helpers;
using System.IO;
using System.Web;

namespace mfc.domain.services
{
    public class ModulesFileConfiguration : IModulesConfiguration
    {
        public string Configuration
        {
            get
            {
                return File.ReadAllText(HttpContext.Current.Server.MapPath("~/modules.json"));
            }
        }
    }
}