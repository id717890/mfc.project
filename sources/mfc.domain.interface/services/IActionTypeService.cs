using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.services {
    public interface IActionTypeService {
        IEnumerable<ActionType> GetAllTypes();
        ActionType GetTypeById(Int64 id);
        Int64 Create(string caption);
        void Update(ActionType actionType);
        void Delete(Int64 typeId);
    }
}
