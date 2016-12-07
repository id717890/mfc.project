using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public interface IServiceActionRepository : IRepository<ServiceAction>
    {
        IEnumerable<ServiceAction> GetActions(DateTime dateBegin, DateTime dateEnd);
        IEnumerable<ServiceAction> GetActions(Int64 user_id, DateTime dateBegin, DateTime dateEnd);

        Int64 TotalRows { get; set; }

        IEnumerable<ServiceAction> GetActions(DateTime dateBegin, DateTime dateEnd, Int32 pageIndex, Int32 pageSize);
        IEnumerable<ServiceAction> GetActions(Int64 userId, DateTime dateBegin, DateTime dateEnd, Int32 pageIndex, Int32 pageSize);
    }
}
