using FluentNHibernate.Mapping;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.mapping {
    public class FileStageMap : ClassMap<FileStage>{
        public FileStageMap() {
            Table("FileStages");
            Id(x => x.Code).GeneratedBy.Assigned();
            Map(x => x.Caption, "Caption");
            Map(x => x.Order, "order_id");
            References(x => x.Status, "status_id");
        }
    }
}
