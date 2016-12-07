using mfc.domain.entities;
using System.Collections.Generic;
using System.Security.Principal;

namespace mfc.infrastructure.services
{
    public interface IActionPermissionService
    {
        IEnumerable<Module> GetUserModules(IPrincipal user);
    }
}
