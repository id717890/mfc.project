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

        public IEnumerable<Package> GetPackages(DateTime dateBegin, DateTime dateEnd, User controller = null, Organization organization = null) {
            var query = Session.Query<Package>();
            if (controller != null) {
                query = query.Where(f => f.Controller.Id == controller.Id);
            }

            if (organization != null) {
                query = query.Where(f => f.Organization.Id == organization.Id);
            }


            return query.Where(f => !f.IsDeleted && f.Date >= dateBegin && f.Date <= dateEnd).OrderByDescending(m => m.Date).ThenByDescending(m => m.Id).ToList();
        }
    }
}
