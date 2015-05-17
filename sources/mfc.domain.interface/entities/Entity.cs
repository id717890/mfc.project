using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    public abstract class Entity {
        public virtual Int64 Id { get; set; }
        public virtual bool IsDeleted { get; set; }

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }

            var obj_cast = obj as Entity;

            if (obj_cast != null) {
                return obj_cast.Id.Equals(Id);
            }

            return ReferenceEquals(obj, this);
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}
