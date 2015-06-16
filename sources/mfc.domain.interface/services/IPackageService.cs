using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.services {
    public interface IPackageService {
        /// <summary>
        /// Позволяет получить все пакеты, созданные контролером <code>controller</code> и имеют дату создания в диапазоне от <code>dateBegin</code> до <code>dateEnd</code>
        /// </summary>
        IEnumerable<Package> GetPackages(DateTime dateBegin, DateTime dateEnd, Int64 controllerId, Int64 organizationId);

        /// <summary>
        /// Создает пакет файлов
        /// </summary>
        Int64 CreatePackage(User controller, DateTime date, Organization organization, IEnumerable<Int64> files);

        /// <summary>
        /// Возвращает пакет по его идентификатору
        /// </summary>
        Package GetPackageById(Int64 packageId);
        
        /// <summary>
        /// Формирует список файлов пакета
        /// </summary>
        IEnumerable<File> GetPackageFiles(Int64 packageId);

        void Update(Package package);

        void Delete(Int64 id);
    }
}
