using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    public class Organization : Entity {
        public static Organization All = new Organization { Id = -1, Caption = "Все" };

        public virtual string Caption { get; set; }
        public virtual string FullCaption { get; set; }
        public virtual OrganizationType Type { get; set; }

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
