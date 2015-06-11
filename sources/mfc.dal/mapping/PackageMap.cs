using FluentNHibernate.Mapping;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.mapping {
    public class PackageMap : ClassMap<Package>{
        public PackageMap() {
            Table("Packages");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Date, "dt");
            Map(x => x.IsDeleted, "is_deleted");
            References(x => x.Organization, "org_id");
            References(x => x.Controller, "user_id");
        }
    }
}
