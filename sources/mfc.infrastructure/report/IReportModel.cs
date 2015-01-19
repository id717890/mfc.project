using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.infrastructure.report {
    public interface IReportModel {
        void Refresh(DateTime dateBegin, DateTime dateEnd);
        IEnumerable<OrganizationType> GetTypes();
        IEnumerable<Organization> GetOrganizations(OrganizationType type);
        IEnumerable<ReportRow> GetRows(Organization org);
    }
}
