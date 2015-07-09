using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    /// <summary>
    /// Класс данных, содержащих информацию об оказанной услуге
    /// </summary>
    public class ServiceAction : Entity {
        public virtual DateTime Date { get; set; }
        public virtual string Customer { get; set; }
        public virtual Service Service { get; set; }
        public virtual Service ServiceChild { get; set; }
        public virtual ActionType Type { get; set; }
        public virtual User User { get; set; }
        public virtual string Comments { get; set; }
        public virtual bool IsNonresident { get; set; }
        /// <summary>
        /// бесплатный выезд специалиста
        /// </summary>
        public virtual bool FreeVisit { get; set; }
    }
}
