using NHibernate.Linq;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public class ServiceActionRepository : Repository<ServiceAction>, IServiceActionRepository {
        public ServiceActionRepository(IUnitOfWorkProvider provider) : base(provider) { }

        public override IEnumerable<ServiceAction> GetAll() {
            return Session
               .CreateQuery(@"
                    from ServiceAction sa 
                        join fetch sa.Type 
                        join fetch sa.Service 
                        join fetch sa.User 
                    where sa.IsDeleted = false")
               .List<ServiceAction>();
        }

        public IEnumerable<ServiceAction> GetActions(long user_id, DateTime date) {
            return Session
                .CreateQuery(@"
                    from ServiceAction sa 
                        join fetch sa.Type 
                        join fetch sa.Service 
                        join fetch sa.User 
                    where sa.IsDeleted = false
                        and sa.Date =:date")
                .SetParameter("date", date)
                .List<ServiceAction>();
        }

        public IEnumerable<ServiceAction> GetActions(DateTime dateBegin, DateTime dateEnd) {
            return Session
                .CreateQuery(@"
                    from ServiceAction sa 
                        join fetch sa.Type 
                        join fetch sa.Service 
                        join fetch sa.User 
                    where sa.IsDeleted = false
                        and sa.Date between :dateBegin and :dateEnd")
                .SetParameter("dateBegin", dateBegin)
                .SetParameter("dateEnd", dateEnd)
                .List<ServiceAction>();
        }
    }
}
