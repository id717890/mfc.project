using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    public class FileStages {
        public const string NewFile = "new";
        public const string SendForControl = "control";
        public const string ReturnForFix = "fix";
        public const string Checked = "check";
        public const string Sended = "send";
    }
}
