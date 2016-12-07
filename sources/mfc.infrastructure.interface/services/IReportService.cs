using System;
using System.IO;

namespace mfc.infrastructure.services {
    public interface IReportService {
        void MakeReportSum(DateTime dateBegin, DateTime dateEnd, Int64 customerTypeId, Stream stream);
        void MakeReportOper(DateTime dateBegin, DateTime dateEnd, Int64 customerTypeId, Stream stream);
        void MakeReestr(Int64 packageId, Stream stream);
    }
}
