using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    /// <summary>
    /// Класс данных, содержащих информацию об оказанной услуге
    /// </summary>
    public class ServiceAction {
        public Int64 Id { get; set; }
        public DateTime Date { get; set; }
        public string Customer { get; set; }
        public Service Service { get; set; }
        public ActionType Type { get; set; }
        public User User { get; set; }
    }
}
