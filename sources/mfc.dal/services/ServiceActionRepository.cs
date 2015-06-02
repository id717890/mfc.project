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
                    where sa.IsDeleted = false
                    order by sa.Date desc, sa.Id desc")
               .List<ServiceAction>();
        }

        public IEnumerable<ServiceAction> GetActions(long user_id, DateTime date) {
            var date1 = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            var date2 = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

            return Session
                .CreateQuery(@"
                    from ServiceAction sa 
                        join fetch sa.Type 
                        join fetch sa.Service 
                        join fetch sa.User 
                    where sa.IsDeleted = false
                        and sa.Date between :date1 and :date2
                        and sa.User.Id = :user_id
                    order by sa.Date desc, sa.Id desc")
                .SetParameter("date1", date1)
                .SetParameter("date2", date2)
                .SetParameter("user_id", user_id)
                .List<ServiceAction>();
        }

        public IEnumerable<ServiceAction> GetActions(DateTime dateBegin, DateTime dateEnd) {
            var date1 = new DateTime(dateBegin.Year, dateBegin.Month, dateBegin.Day, 0, 0, 0);
            var date2 = new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day, 23, 59, 59);

            return Session
                .CreateQuery(@"
                    from ServiceAction sa 
                        join fetch sa.Type 
                        join fetch sa.Service 
                        join fetch sa.User 
                    where sa.IsDeleted = false
                        and sa.Date between :date1 and :date2
                    order by sa.Date desc, sa.Id desc")
                .SetParameter("date1", date1)
                .SetParameter("date2", date2)
                .List<ServiceAction>();
        }

        public IEnumerable<ServiceAction> GetActions(Int64 user_id, DateTime dateBegin, DateTime dateEnd) {
            var date1 = new DateTime(dateBegin.Year, dateBegin.Month, dateBegin.Day, 0, 0, 0);
            var date2 = new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day, 23, 59, 59);

            return Session
                .CreateQuery(@"
                    from ServiceAction sa 
                        join fetch sa.Type 
                        join fetch sa.Service 
                        join fetch sa.User 
                    where sa.IsDeleted = false
                        and sa.Date between :date1 and :date2
                        and sa.User.Id = :user_id
                    order by sa.Date desc, sa.Id desc")
                .SetParameter("date1", date1)
                .SetParameter("date2", date2)
                .SetParameter("user_id", user_id)
                .List<ServiceAction>();
        }
    }
}
