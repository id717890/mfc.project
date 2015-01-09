using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    public class Organization {
        public Int64 Id { get; set; }
        public string Caption { get; set; }
        public string FullCaption { get; set; }
        public OrganizationType Type { get; set; }
    }
}
