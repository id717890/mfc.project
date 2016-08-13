using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.services {
    public interface IServiceService {
        IEnumerable<Service> GetAllServices();
        IEnumerable<Service> GetOrganizationServices(Int64 orgId);
        Service GetServiceById(Int64 Id);
        Int64 Create(string caption, Int64 organizationId);
        Int64 Create(string caption, Int64 organizationId, Int64 parentId);
        void Update(Service service);
        void Delete(Int64 id);

        IEnumerable<Service> GetChildServices(Int64 parentId);
        IEnumerable<Service> Search(string term);
    }
}
