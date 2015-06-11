using mfc.dal.services;
using mfc.domain.entities;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.services {
    public class PackageService : IPackageService {
        [Inject]
        public IPackageRepository PackageRepository { get; set; }

        public IEnumerable<Package> GetPackages(DateTime dateBegin, DateTime dateEnd) {
            throw new NotImplementedException();
        }

        public IEnumerable<Package> GetPackages(DateTime dateBegin, DateTime dateEnd, entities.User controller) {
            throw new NotImplementedException();
        }

        public long CreatePackage(User controller, DateTime date, Organization organization, IEnumerable<long> files) {
            throw new NotImplementedException();
        }

        public Package GetPackageById(long packageId) {
            throw new NotImplementedException();
        }

        public IEnumerable<File> GetPackageFiles(long packageId) {
            throw new NotImplementedException();
        }
    }
}
