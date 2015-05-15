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
            Map(x => x.Caption, "Caption");
        }
    }
}
