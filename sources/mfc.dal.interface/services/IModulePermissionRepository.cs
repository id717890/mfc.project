using System.Collections.Generic;

namespace mfc.dal.services
{
    public interface IModulePermissionRepository
    {
        Dictionary<string, List<string>> GetUserModulesAndOperations(string userName);
    }
}
