using mfc.domain.entities;
using System.Collections.Generic;

namespace mfc.webapi.Helpers
{
    public interface IModulesProvider
    {
        IEnumerable<Module> GetRoot();
        IEnumerable<Module> GetAll();
    }
}