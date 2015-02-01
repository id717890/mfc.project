using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.infrastructure.services {
    public interface IReportService {
        void MakeReportSum(DateTime dateBegin, DateTime dateEnd, Stream stream);
        void MakeReportOper(DateTime dateBegin, DateTime dateEnd, Stream stream);
    }
}
