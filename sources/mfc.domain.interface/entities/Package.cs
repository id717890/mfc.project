using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    /// <summary>
    /// Пакет для отправки документов в ОГВ
    /// </summary>
    public class Package : Entity {
        public virtual Organization Organization { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual User Controller { get; set; }
    }
}
