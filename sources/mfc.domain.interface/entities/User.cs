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
    }
}
