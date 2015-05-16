using NHibernate.Linq;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository {
        public OrganizationRepository(IUnitOfWorkProvider provider) : base(provider) { }

        public override IEnumerable<Organization> GetAll() {
            return Session.Query<Organization>().Fetch(x=>x.Type).Where(x=> !x.IsDeleted).ToList();
        }
    }
}
