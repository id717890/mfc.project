using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    /// <summary>
    /// Элемент регистра статустов дел
    /// </summary>
    public class FileStatusInfo {
        public Int64 FileId { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public FileStatus Status { get; set; }
    }
}
