using FluentNHibernate.Mapping;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.mapping {
    public class FileMap : ClassMap<File>{
        public FileMap() {
            Table("Files");
            Id(x => x.Id);
            Map(x => x.Date, "dt");
            Map(x => x.Caption, "Caption");
            Map(x => x.IsDeleted, "is_deleted");
            References(x => x.Action, "action_id");
            References(x => x.Expert, "expert_id");
            References(x => x.Controller, "controller_id");
            References(x => x.Ogv, "org_id");
            References(x => x.Service, "service_id");
            References(x => x.CurrentStatus, "status_id");

        }
    }
}
