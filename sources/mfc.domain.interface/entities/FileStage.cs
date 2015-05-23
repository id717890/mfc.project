using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    /// <summary>
    /// Этапы жизненного цикла дела
    /// </summary>
    public class FileStage {
        public virtual string Code { get; set; }
        public virtual string Caption { get; set; }
        public virtual FileStatus Status { get; set; }
        public virtual Int32 Order { get; set; }
    }
}
