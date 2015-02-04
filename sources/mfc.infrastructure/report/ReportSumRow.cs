using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.infrastructure.report {
    public class ReportSumRow {
        public Service Service { get; set; }
        public Int32 Priem { get; set; }
        public Int32 Vidacha { get; set; }
        public Int32 Consult { get; set; }

        public Int32 All {
            get {
                return Priem + Vidacha + Consult;
            }
        }
    }
}
