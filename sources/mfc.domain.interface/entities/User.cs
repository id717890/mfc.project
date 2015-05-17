using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    /// <summary>
    /// Класс с данными пользователя
    /// </summary>
    public class User : Entity {
        public virtual String Account { get; set; }
        public virtual String Name { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual bool IsExpert { get; set; }
        public virtual bool IsController { get; set; }

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }
            else if (obj.GetType() != typeof(User)) {
                return false;
            }
            return ((User)obj).Id == Id;
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}
