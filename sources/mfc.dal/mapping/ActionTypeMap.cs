using FluentNHibernate.Mapping;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.mapping {
    public class ActionTypeMap : ClassMap<ActionType>{
        public ActionTypeMap() {
            Table("ActionTypes");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Caption);
            Map(x => x.NeedMakeFile, "make_file");
            Map(x => x.IsDeleted, "is_deleted");
        }
    }
}
