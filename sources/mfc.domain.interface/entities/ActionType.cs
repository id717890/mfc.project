using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    /// <summary>
    /// Элемент справочника "Виды деятельности". Предназначен для выбора вида деятельности при оказании услуги.
    /// </summary>
    public class ActionType : Entity {
        public virtual string Caption { get; set; }
        /// <summary>
        /// Признак необходимости создавать дела для всех приемов данного типа
        /// </summary>
        public virtual bool NeedMakeFile { get; set; }
    }
}
