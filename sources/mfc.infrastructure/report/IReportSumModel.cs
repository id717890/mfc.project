using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.infrastructure.report {
    public interface IReportSumModel {
        void Refresh(DateTime dateBegin, DateTime dateEnd, CustomerType customerType);
        IEnumerable<OrganizationType> GetTypes();
        IEnumerable<ReportSumRow> GetRows(OrganizationType type);
    }
}
