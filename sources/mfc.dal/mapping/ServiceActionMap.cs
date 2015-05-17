
using FluentNHibernate.Mapping;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.mapping {
    public class ServiceActionMap : ClassMap<ServiceAction> {
        public ServiceActionMap() {
            Table("Actions");
            Id(x => x.Id);
            Map(x => x.Date, "dt");
            Map(x => x.Comments);
            Map(x => x.IsDeleted, "is_deleted");
            Map(x => x.Customer);
            References(x => x.Type, "type_id").LazyLoad(Laziness.NoProxy);
            References(x => x.Service, "service_id").LazyLoad(Laziness.NoProxy);
            References(x => x.User, "user_id").LazyLoad(Laziness.NoProxy);
        }
    }
}
