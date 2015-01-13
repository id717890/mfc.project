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
    public class ActionTypeService : IActionTypeService {
        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public IIdentifierService IdService { get; set; }

        private Dictionary<Int64, ActionType> _cache = new Dictionary<long, ActionType>();
        private bool _is_cache_valid = false;

        public IEnumerable<ActionType> GetAllTypes() {
            PrepareCache();

            return _cache.Values.OrderBy(m => m.Id);
        }

        public ActionType GetTypeById(long id) {
            PrepareCache();

            ActionType type = null;

            if (_cache.ContainsKey(id)) {
                type = _cache[id];
            }

            return type;
        }

        public long Create(string caption) {
            Debug.Assert(!string.IsNullOrEmpty(caption));

            Int64 result_id = 0;
            Int64 new_id = IdService.GetId();

            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();

            try {
                cmd.CommandText = @"insert into ActionTypes (id, caption) values (@id, @caption)";
                cmd.Parameters.Add(new SqlParameter("id", new_id));
                cmd.Parameters.Add(new SqlParameter("caption", caption));

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

        public void Update(ActionType type) {
            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();

            try {
                cmd.CommandText = "update ActionTypes set caption = @caption where id = @id";
                cmd.Parameters.Add(new SqlParameter("id", type.Id));
                cmd.Parameters.Add(new SqlParameter("caption", type.Caption));

                int count = cmd.ExecuteNonQuery();

                if (count == 0) {
                    throw new DomainException("Не удалось обновить запись в таблице ActionTypes");
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

        public void Delete(Int64 typeId) {
            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();

            try {
                cmd.CommandText = "update ActionTypes set is_deleted = 1 where id = @id";
                cmd.Parameters.Add(new SqlParameter("id", typeId));

                int count = cmd.ExecuteNonQuery();

                if (count == 0) {
                    throw new DomainException("Не удалось обновить запись в таблице ActionTypes");
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

            _cache.Clear();

            foreach (var type in GetTypesInternal()) {
                _cache.Add(type.Id, type);
            }
        }

        private IEnumerable<ActionType> GetTypesInternal() {
            List<ActionType> types = new List<ActionType>();

            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();
            SqlDataReader reader = null;

            try {
                cmd.CommandText = @"
                    select id, caption 
                        from ActionTypes 
                    where is_deleted = 0
                    order by id";
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
                conn.Close();
            }

            return types;
        }

        private ActionType CreateType(SqlDataReader reader) {
            return new ActionType {
                Id = Convert.ToInt64(reader["id"]),
                Caption = Convert.ToString(reader["caption"])
            };
        }

        #endregion
    }
}
