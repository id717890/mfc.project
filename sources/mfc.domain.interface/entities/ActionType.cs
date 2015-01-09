using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    /// <summary>
    /// Элемент справочника "Виды деятельности". Предназначен для выбора вида деятельности при оказании услуги.
    /// </summary>
    public class ActionType {
        public Int64 Id { get; set; }
        public string Caption { get; set; }
    }
}
