using NHibernate.Linq;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using mfc.domain.services;

namespace mfc.dal.services {
    public class PackageRepository : Repository<Package>, IPackageRepository {
        public PackageRepository(IUnitOfWorkProvider unitOfWorkProvider) : base(unitOfWorkProvider) { }

        public IEnumerable<Package> GetPackages(DateTime dateBegin, DateTime dateEnd) {
            throw new NotImplementedException();
        }

        public IEnumerable<Package> GetPackages(DateTime dateBegin, DateTime dateEnd, long controllerId) {
            throw new NotImplementedException();
        }
    }
}
