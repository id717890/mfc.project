using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.services {
    public interface IServiceService {
        IEnumerable<Service> GetAllServices();
        Service GetServiceById(Int64 Id);
        Int64 Create(string caption, Int64 organizationId);
        void Update(Service service);
        void Delete(Int64 id);
    }
}
