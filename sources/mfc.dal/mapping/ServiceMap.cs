using FluentNHibernate.Mapping;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.mapping {
    public class ServiceMap : ClassMap<Service>{
        public ServiceMap() {
            Table("Services");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Caption);
            References(x => x.Organization, "org_id");
            References(x => x.Parent, "parent_id");
            Map(x => x.IsDeleted, "is_deleted");
        }
    }
}
