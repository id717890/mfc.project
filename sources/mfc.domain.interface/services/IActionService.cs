using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.services {
    /// <summary>
    /// Интерфейс для работы с оказанными услугами
    /// </summary>
    public interface IActionService {
        IEnumerable<ServiceAction> GetActions(User user, DateTime date);
        IEnumerable<ServiceAction> GetActions(User user, DateTime dateBegin, DateTime dateEnd);
        IEnumerable<ServiceAction> GetActions(DateTime dateBegin, DateTime dateEnd, CustomerType customerType);
        IEnumerable<ServiceAction> GetActions(User user, DateTime dateBegin, DateTime dateEnd, CustomerType customerType);
        ServiceAction GetActionById(Int64 Id);
        Int64 Add(DateTime date, Int64 serviceId, string curtomer, Int64 typeId, Int64 customerTypeId, Int64 userId, Int64 serviceChildId, bool is_nonresident, bool free_visit, string comments);
        void Update(ServiceAction action);
        void Delete(Int64 id);
    }
}
