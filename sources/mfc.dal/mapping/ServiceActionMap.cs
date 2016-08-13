
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
            ReadOnly();
            Cache.ReadOnly();
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Date, "dt");
            Map(x => x.Comments);
            Map(x => x.IsDeleted, "is_deleted");
            Map(x => x.Customer);
            Map(x => x.IsNonresident, "is_nonresident");
            Map(x => x.FreeVisit, "free_visit");
            References(x => x.Type, "type_id");
            References(x => x.Service, "service_id");
            References(x => x.CustomerType, "customer_type_id").Nullable().NotFound.Ignore();
            References(x => x.ServiceChild, "service_child_id");
            References(x => x.User, "user_id");
        }
    }
}
