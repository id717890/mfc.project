using FluentNHibernate.Mapping;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.mapping {
    public class OrganizationTypeMapping : ClassMap<OrganizationType>{
        public OrganizationTypeMapping() {
            Table("OrganizationTypes");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Caption);
            Map(x => x.IsDeleted, "is_deleted");
        }
    }
}
