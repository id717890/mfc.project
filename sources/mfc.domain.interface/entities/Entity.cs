using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    public abstract class Entity {
        public virtual Int64 Id { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}
