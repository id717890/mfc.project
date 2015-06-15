using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public interface IPackageRepository : IRepository<Package> {
        /// <summary>
        /// Позволяет получить все пакеты, созданные для отправки в ОГВ <code>organizationId</code> контролером <code>controllerId</code> и имеют дату создания в диапазоне от <code>dateBegin</code> до <code>dateEnd</code>
        /// </summary>
        IEnumerable<Package> GetPackages(DateTime dateBegin, DateTime dateEnd, User controller = null, Organization organization = null);

        void UpdateFiles(Int64 package_id, IEnumerable<Int64> file_ids);
        IEnumerable<Int64> GetPackageFileIds(Int64 packageId);
    }
}
