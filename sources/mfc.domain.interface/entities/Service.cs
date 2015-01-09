using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    /// <summary>
    /// Элемент справочника "Услуги"
    /// </summary>
    public class Service {
        public Int64 Id { get; set; }
        public string Caption { get; set; }
        public Organization Organization { get; set; }
    }
}
