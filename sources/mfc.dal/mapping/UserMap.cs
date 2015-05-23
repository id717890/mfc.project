using FluentNHibernate.Mapping;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.mapping {
    public class UserMap : ClassMap<User>{
        public UserMap() {
            Table("users");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.IsDeleted, "is_deleted");
            Map(x => x.Name);
            Map(x => x.Account);
            Map(x => x.IsAdmin, "is_admin");
            Map(x => x.IsExpert, "is_expert");
            Map(x => x.IsController, "is_controller");
        }
    }
}
