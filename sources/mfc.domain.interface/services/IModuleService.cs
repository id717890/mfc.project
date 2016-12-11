using mfc.domain.entities;
using System;
using System.Collections.Generic;

namespace mfc.domain.services {
    public interface IModuleService {
        IEnumerable<Module> GetRoot();
        IEnumerable<Module> GetAll();
        Module GetByCode(string code);
    }
}
