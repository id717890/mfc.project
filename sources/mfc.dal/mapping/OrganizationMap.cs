using FluentNHibernate.Mapping;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.mapping {
    public class OrganizationMap : ClassMap<Organization>{
        public OrganizationMap() {
            Table("Organizations");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Caption);
            Map(x => x.FullCaption, "full_caption");
            References(x => x.Type, "type_id");
            Map(x => x.IsDeleted, "is_deleted");
        }
    }
}
