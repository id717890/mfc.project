using mfc.dal.services;
using mfc.domain.entities;
using mfc.infrastructure.services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.services {
    public class OrganizationService : IOrganizationService {
        private readonly object sync_obj1 = new object();
        private readonly object sync_obj2 = new object();

        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public IOrganizationTypeRepository TypeRepo { get; set; }

        [Inject]
        public IUnitOfWorkProvider UnitOfWorkProvider { get; set; }

        [Inject]
        public IOrganizationRepository OrgRepo { get; set; }

        private Dictionary<Int64, OrganizationType> _cache_types = new Dictionary<long, OrganizationType>();
        private Dictionary<Int64, Organization> _cache_orgs = new Dictionary<long, Organization>();
        private bool _is_type_cache_valid = false;
        private bool _is_org_cache_valid = false;

        #region Organization Type

        public IEnumerable<OrganizationType> GetAllTypes() {
            PrepareTypeCache();
            return _cache_types.Values.OrderBy(x => x.Caption);
        }
        

        public OrganizationType GetTypeById(Int64 id) {
            OrganizationType type = null;

            PrepareTypeCache();

            if (_cache_types.ContainsKey(id)) {
                type = _cache_types[id];
            }

            return type;
        }

        public long CreateType(string caption) {
            Debug.Assert(!string.IsNullOrEmpty(caption));

            OrganizationType type = new OrganizationType { Caption = caption };
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();
            
            unit_of_work.BeginTransaction(); 
            TypeRepo.Create(type);
            unit_of_work.Commit();

            _is_type_cache_valid = false;

            return 0;
        }

        public void DeleteType(long typeId) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();
            
            unit_of_work.BeginTransaction();
            TypeRepo.Delete(typeId);
            unit_of_work.Commit();

            _is_type_cache_valid = false;
        }

        public void UpdateType(OrganizationType type) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            TypeRepo.Update(type);
            unit_of_work.Commit();

            _is_type_cache_valid = false;
        }


        #endregion

        #region Organization

        public IEnumerable<Organization> GetAllOrganizations() {
            PrepareOrgCache();
            
            return _cache_orgs.Values.OrderBy(m => m.Caption);
        }

        public long CreateOrganization(string caption, string fullCaption, Int64 typeId) {
            Debug.Assert(!string.IsNullOrEmpty(caption));

            var org = new Organization {
                Caption = caption,
                FullCaption = fullCaption,
                Type = _cache_types[typeId]
            };

            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            OrgRepo.Create(org);
            unit_of_work.Commit();

            _is_org_cache_valid = false;

            return 0;
        }

        public void DeleteOrganization(long organizationId) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            OrgRepo.Delete(organizationId);
            unit_of_work.Commit();

            _is_org_cache_valid = false;
        }

        public void UpdateOgranization(Organization organization) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            OrgRepo.Update(organization);
            unit_of_work.Commit();

            _is_org_cache_valid = false;
        }

        public Organization GetOrganizationById(long id) {
            PrepareOrgCache();

            Organization org = null;

            if (_cache_orgs.ContainsKey(id)) {
                org = _cache_orgs[id];
            }

            return org;
        }

        #endregion


        #region Helper Methods

        private void PrepareTypeCache() {
            if (_is_type_cache_valid) {
                return;
            }

            lock (sync_obj2) {
                _cache_types.Clear();
                foreach (var type in GetAllTypesInternal()) {
                    _cache_types.Add(type.Id, type);
                }

                _is_type_cache_valid = true;
            }
        }

        private void PrepareOrgCache() {
            if (_is_org_cache_valid) {
                return;
            }
            lock (sync_obj1) {
                _cache_orgs.Clear();
                foreach (var org in GetAllOrganizationsInternal()) {
                    _cache_orgs.Add(org.Id, org);
                }

                _is_org_cache_valid = true;
            }
        }

        private IEnumerable<OrganizationType> GetAllTypesInternal() {
            return TypeRepo.GetAll();
        }

        public IEnumerable<Organization> GetAllOrganizationsInternal() {
            return OrgRepo.GetAll().ToList();
        }

        #endregion
    }
}
