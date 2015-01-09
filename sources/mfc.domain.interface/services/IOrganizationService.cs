using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.services {
    public interface IOrganizationService {
        #region Work with OrganizationType

        IEnumerable<OrganizationType> GetAllTypes();
        Int64 CreateType(String caption);
        void DeleteType(Int64 typeId);
        void UpdateType(OrganizationType type);
        OrganizationType GetTypeById(Int64 id);

        #endregion

        #region work with organization
        IEnumerable<Organization> GetAllOrganizations();
        Int64 CreateOrganization(string caption, string fullCaption, Int64 typeId);
        void DeleteOrganization(Int64 organizationId);
        void UpdateOgranization(Organization organization);
        Organization GetOrganizationById(Int64 id);
        #endregion
    }
}
