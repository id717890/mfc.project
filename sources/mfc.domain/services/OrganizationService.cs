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
        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public IIdentifierService IdService { get; set; }

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

            Int64 result_id = 0;
            Int64 new_id = IdService.GetId();

            var cmd = SqlProvider.CreateConnection().CreateCommand();

            try {
                cmd.CommandText = @"insert into OrganizationTypes (id, caption) values (@id, @caption)";
                cmd.Parameters.Add(new SqlParameter("id", new_id));
                cmd.Parameters.Add(new SqlParameter("caption", caption));

                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                cmd.Dispose();
                _is_type_cache_valid = false;
            }

            return result_id;
        }

        public void DeleteType(long typeId) {
            var cmd = SqlProvider.CreateConnection().CreateCommand();

            try {
                cmd.CommandText = "update OrganizationTypes set is_deleted = 1 where id = @id";
                cmd.Parameters.Add(new SqlParameter("id", typeId));

                int count = cmd.ExecuteNonQuery();

                if (count == 0) {
                    throw new DomainException("Не удалось обновить запись в таблице OrganizationTypes");
                }
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                cmd.Dispose();
                _is_type_cache_valid = false;
            }
        }

        public void UpdateType(OrganizationType type) {
            var cmd = SqlProvider.CreateConnection().CreateCommand();

            try {
                cmd.CommandText = "update OrganizationTypes set caption = @caption where id = @id";
                cmd.Parameters.Add(new SqlParameter("id", type.Id));
                cmd.Parameters.Add(new SqlParameter("caption", type.Caption));
                
                int count = cmd.ExecuteNonQuery();
                
                if (count == 0) {
                    throw new DomainException("Не удалось обновить запись в таблице OrganizationTypes");
                }
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                cmd.Dispose();
                _is_type_cache_valid = false;
            }
        }


        #endregion

        #region Organization

        public IEnumerable<Organization> GetAllOrganizations() {
            PrepareOrgCache();
            
            return _cache_orgs.Values.OrderBy(m => m.Caption);
        }

        public long CreateOrganization(string caption, string fullCaption, Int64 typeId) {
            Debug.Assert(!string.IsNullOrEmpty(caption));

            Int64 result_id = 0;
            Int64 new_id = IdService.GetId();

            var cmd = SqlProvider.CreateConnection().CreateCommand();

            try {
                cmd.CommandText = @"insert into Organizations (id, caption, full_caption, type_id) values (@id, @caption, @full_caption, @type_id)";
                cmd.Parameters.Add(new SqlParameter("id", new_id));
                cmd.Parameters.Add(new SqlParameter("caption", caption));
                cmd.Parameters.Add(new SqlParameter("full_caption", fullCaption));
                cmd.Parameters.Add(new SqlParameter("type_id", typeId));

                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                cmd.Dispose();
                _is_org_cache_valid = false;
            }

            return result_id;
        }

        public void DeleteOrganization(long organizationId) {
            var cmd = SqlProvider.CreateConnection().CreateCommand();

            try {
                cmd.CommandText = "update Organizations set is_deleted = 1 where id = @id";
                cmd.Parameters.Add(new SqlParameter("id", organizationId));

                int count = cmd.ExecuteNonQuery();

                if (count == 0) {
                    throw new DomainException("Не удалось обновить запись в таблице Organizations");
                }
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                cmd.Dispose();
                _is_org_cache_valid = false; ;
            }
        }

        public void UpdateOgranization(Organization organization) {
            var cmd = SqlProvider.CreateConnection().CreateCommand();

            try {
                cmd.CommandText = "update Organizations set caption = @caption, full_caption = @full_caption, type_id = @type_id where id = @id";
                cmd.Parameters.Add(new SqlParameter("id", organization.Id));
                cmd.Parameters.Add(new SqlParameter("caption", organization.Caption));
                cmd.Parameters.Add(new SqlParameter("full_caption", organization.FullCaption));
                cmd.Parameters.Add(new SqlParameter("type_id", organization.Type.Id));

                int count = cmd.ExecuteNonQuery();

                if (count == 0) {
                    throw new DomainException("Не удалось обновить запись в таблице Organizations");
                }
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                cmd.Dispose();
                _is_org_cache_valid = false;
            }
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

        private OrganizationType CreateType(SqlDataReader reader) {
            return new OrganizationType {
                Id = Convert.ToInt64(reader["id"]),
                Caption = Convert.ToString(reader["caption"])
            };
        }

        private Organization CreateOrganization(SqlDataReader reader) {
            var org = new Organization {
                Id = Convert.ToInt64(reader["id"]),
                Caption = Convert.ToString(reader["caption"]),
                FullCaption = reader["full_caption"] != DBNull.Value ? Convert.ToString(reader["full_caption"]) : string.Empty
            };

            Int64 type_id = reader["type_id"] != DBNull.Value ? Convert.ToInt64(reader["type_id"]) : -1;

            if (type_id > 0 && _cache_types.ContainsKey(type_id)) {
                org.Type = _cache_types[type_id];
            }

            return org;
        }

        private void PrepareTypeCache() {
            if (_is_type_cache_valid) {
                return;
            }

            _cache_types.Clear();
            foreach (var type in GetAllTypesInternal()) {
                _cache_types.Add(type.Id, type);
            }

            _is_type_cache_valid = true;
        }

        private void PrepareOrgCache() {
            if (_is_org_cache_valid) {
                return;
            }

            _cache_orgs.Clear();
            foreach (var org in GetAllOrganizationsInternal()) {
                _cache_orgs.Add(org.Id, org);
            }

            _is_org_cache_valid = true;
        }

        private IEnumerable<OrganizationType> GetAllTypesInternal() {
            List<OrganizationType> types = new List<OrganizationType>();

            var cmd = SqlProvider.CreateConnection().CreateCommand();
            SqlDataReader reader = null;

            try {
                cmd.CommandText = @"
                    select id, caption 
                        from OrganizationTypes 
                    where is_deleted = 0
                    order by caption";
                reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    types.Add(CreateType(reader));
                }
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                if (reader != null) {
                    reader.Close();
                }
                cmd.Dispose();
            }

            return types;
        }

        public IEnumerable<Organization> GetAllOrganizationsInternal() {
            PrepareTypeCache();

            List<Organization> orgs = new List<Organization>();

            var cmd = SqlProvider.CreateConnection().CreateCommand();
            SqlDataReader reader = null;

            try {
                cmd.CommandText = @"
                    select id, caption, full_caption, type_id 
                        from Organizations
                    where is_deleted = 0
                    order by caption";
                reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    orgs.Add(CreateOrganization(reader));
                }
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                if (reader != null) {
                    reader.Close();
                }
                cmd.Dispose();
            }

            return orgs;
        }

        #endregion
    }
}
