using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    /// <summary>
    /// Элемент справочника "Статусы дел"
    /// </summary>
    public class FileStatus : Entity {
        public static FileStatus All = new FileStatus { Id = -1, Caption = "Все" };
        public virtual string Caption { get; set; }
    }
}
