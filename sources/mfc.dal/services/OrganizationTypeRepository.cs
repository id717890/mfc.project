using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public class OrganizationTypeRepository : Repository<OrganizationType>, IOrganizationTypeRepository {
        public OrganizationTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork) {

        }
    }
}
