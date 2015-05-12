using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    public class Organization {
        public static Organization All = new Organization { Id = -1, Caption = "Все" };


        public Int64 Id { get; set; }
        public string Caption { get; set; }
        public string FullCaption { get; set; }
        public OrganizationType Type { get; set; }

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }
            if (obj.GetType() == typeof(Organization)) {
                return ((Organization)obj).Id.Equals(Id);
            }

            return ReferenceEquals(obj, this);
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}
