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
    public class ServiceService : IServiceService {
        private readonly object sync_obj = new object();

        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public IIdentifierService IdService { get; set; }

        [Inject]
        public IOrganizationService OrgService { get; set; }

        private Dictionary<Int64, Service> _cache = new Dictionary<long, Service>();
        private bool _is_cache_valid = false;

        public IEnumerable<Service> GetAllServices() {
            PrepareCache();

            return _cache.Values.OrderBy(m=>m.Caption).OrderBy(m => m.Organization.Caption);
        }

        public IEnumerable<Service> GetOrganizationServices(Int64 orgId) {
            PrepareCache();

            return 
                _cache.Values
                    .Where(item => item.Organization.Id == orgId)
                    .OrderBy(m => m.Caption);
        }

        public Service GetServiceById(long id) {
            PrepareCache();

            Service service = null;

            if (_cache.ContainsKey(id)) {
                service = _cache[id];
            }

            return service;
        }

        public long Create(string caption, Int64 organizationId) {
            Debug.Assert(!string.IsNullOrEmpty(caption));

            Int64 result_id = 0;
            Int64 new_id = IdService.GetId();

            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();

            try {
                cmd.CommandText = @"insert into Services (id, caption, org_id) values (@id, @caption, @org_id)";
                cmd.Parameters.Add(new SqlParameter("id", new_id));
                cmd.Parameters.Add(new SqlParameter("caption", caption));
                cmd.Parameters.Add(new SqlParameter("org_id", organizationId));

                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                cmd.Dispose();
                conn.Close();
                _is_cache_valid = false;
            }

            return result_id;
        }

        public void Update(Service service) {
            var conn = SqlProvider.CreateConnection(); 
            var cmd = conn.CreateCommand();

            try {
                cmd.CommandText = "update Services set caption = @caption, org_id = @org_id where id = @id";
                cmd.Parameters.Add(new SqlParameter("id", service.Id));
                cmd.Parameters.Add(new SqlParameter("caption", service.Caption));
                cmd.Parameters.Add(new SqlParameter("org_id", DBNull.Value));

                if (service.Organization != null) {
                    cmd.Parameters["org_id"].Value = service.Organization.Id;
                }

                int count = cmd.ExecuteNonQuery();

                if (count == 0) {
                    throw new DomainException("Не удалось обновить запись в таблице Services");
                }
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                cmd.Dispose();
                conn.Close();
                _is_cache_valid = false;
            }
        }

        public void Delete(Int64 id) {
            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();

            try {
                cmd.CommandText = "update Services set is_deleted = 1 where id = @id";
                cmd.Parameters.Add(new SqlParameter("id", id));

                int count = cmd.ExecuteNonQuery();

                if (count == 0) {
                    throw new DomainException("Не удалось обновить запись в таблице Services");
                }
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                cmd.Dispose();
                conn.Close();
                _is_cache_valid = false;
            }
        }

        #region Helpers

        private void PrepareCache() {
            if (_is_cache_valid) {
                return;
            }

            lock (sync_obj) {
                _cache.Clear();

                foreach (var type in GetServicesInternal()) {
                    _cache.Add(type.Id, type);
                }
                _is_cache_valid = true;
            }
        }

        private IEnumerable<Service> GetServicesInternal() {
            List<Service> services = new List<Service>();
            
            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();
            SqlDataReader reader = null;

            try {
                cmd.CommandText = @"
                    select id, caption, org_id 
                        from Services 
                    where is_deleted = 0
                    order by caption";
                reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    services.Add(CreateService(reader));
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
                conn.Close();
            }

            return services;
        }

        private Service CreateService(SqlDataReader reader) {
            return new Service {
                Id = Convert.ToInt64(reader["id"]),
                Caption = Convert.ToString(reader["caption"]),
                Organization = OrgService.GetOrganizationById(Convert.ToInt64(reader["org_id"]))
            };
        }

        #endregion
    }
}
