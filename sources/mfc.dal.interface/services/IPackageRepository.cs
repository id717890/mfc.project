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

        /*Рефакторинг*/
        /// <summary>
        /// Общее количество строк полученное при выполнении последнего запроса к сущности
        /// </summary>
        Int64 TotalRows { get; set; }

        /// <summary>
        /// Позволяет получить страницу <code>pageIndex</code> со списком пакетов, созданных контролером <code>controller</code> и имеют дату создания в диапазоне от <code>beginDate</code> до <code>endDate</code>
        /// </summary>
        IEnumerable<Package> GetPackages(DateTime dateBegin, DateTime dateEnd, Int64 organization, Int64 controller, Int32 pageIndex, Int32 pageSize);
    }
}
