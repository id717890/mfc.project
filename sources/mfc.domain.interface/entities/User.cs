using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    /// <summary>
    /// Класс с данными пользователя
    /// </summary>
    public class User {
        public Int64 Id { get; set; }
        public String Account { get; set; }
        public String Name { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsExpert { get; set; }
        public bool IsController { get; set; }

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
