using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.infrastructure.services {
    public interface IReportService {
        void MakeReport(DateTime dateBegin, DateTime dateEnd, Stream stream);
    }
}
