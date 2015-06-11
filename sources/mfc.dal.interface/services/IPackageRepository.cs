using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public interface IPackageRepository : IRepository<Package> {
        /// <summary>
        /// Позволяет получить все пакеты, которые имеют дату создания в диапазоне от <code>dateBegin</code> до <code>dateEnd</code>
        /// </summary>
        IEnumerable<Package> GetPackages(DateTime dateBegin, DateTime dateEnd);

        /// <summary>
        /// Позволяет получить все пакеты, созданные контролером <code>controller</code> и имеют дату создания в диапазоне от <code>dateBegin</code> до <code>dateEnd</code>
        /// </summary>
        IEnumerable<Package> GetPackages(DateTime dateBegin, DateTime dateEnd, Int64 controllerId);
    }
}
