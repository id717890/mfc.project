using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mfc.domain.entities;
using mfc.infrastructure.services;
using Ninject;

namespace mfc.domain.services {
    public class FileStatusService : IFileStatusService {
        private readonly object sync_obj = new object();

        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public IIdentifierService IdService { get; set; }

        private Dictionary<Int64, FileStatus> _cache = new Dictionary<long, FileStatus>();
        private bool _is_cache_valid = false;

        public IEnumerable<FileStatus> GetAllStatuses() {
            PrepareCache();

            return _cache.Values.OrderBy(m => m.Id);
        }

        public FileStatus GetStatusById(long id) {
            PrepareCache();

            FileStatus status = null;

            if (_cache.ContainsKey(id)) {
                status = _cache[id];
            }

            return status;
        }

        public long Create(string caption) {
            Debug.Assert(!string.IsNullOrEmpty(caption));

            Int64 result_id = 0;
            Int64 new_id = IdService.GetId();

            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();

            try {
                cmd.CommandText = @"insert into Statuses (id, caption) values (@id, @caption)";
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

        public void Update(FileStatus status) {
            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();

            try {
                cmd.CommandText = "update Statuses set caption = @caption where id = @id";
                cmd.Parameters.Add(new SqlParameter("id", status.Id));
                cmd.Parameters.Add(new SqlParameter("caption", status.Caption));

                int count = cmd.ExecuteNonQuery();

                if (count == 0) {
                    throw new DomainException("Не удалось обновить запись в таблице Statuses");
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
                cmd.CommandText = "update Statuses set is_deleted = 1 where id = @id";
                cmd.Parameters.Add(new SqlParameter("id", typeId));

                int count = cmd.ExecuteNonQuery();

                if (count == 0) {
                    throw new DomainException("Не удалось обновить запись в таблице Statuses");
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

                foreach (var type in GetTypesInternal()) {
                    if (!_cache.ContainsKey(type.Id)) {
                        _cache.Add(type.Id, type);
                    }
                }

                _is_cache_valid = true;
            }
        }

        private IEnumerable<FileStatus> GetTypesInternal() {
            List<FileStatus> statuses = new List<FileStatus>();

            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();
            SqlDataReader reader = null;

            try {
                cmd.CommandText = @"
                    select id, caption 
                        from Statuses 
                    where is_deleted = 0
                    order by id";
                reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    statuses.Add(CreateStatus(reader));
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

            return statuses;
        }

        private FileStatus CreateStatus(SqlDataReader reader) {
            return new FileStatus {
                Id = Convert.ToInt64(reader["id"]),
                Caption = Convert.ToString(reader["caption"])
            };
        }

        #endregion

        #region FileStatusInfo
        
        public IEnumerable<FileStatusInfo> GetFileStatuses(long fileId) {
            throw new NotImplementedException();
        }

        public void SetStatus(long fileId, long statusId, DateTime date, long userId) {
            throw new NotImplementedException();
        }

        public void UpdateStatus(FileStatusInfo status) {
            throw new NotImplementedException();
        }

        public void DeleteStatus(long statusId) {
            throw new NotImplementedException();
        }
        #endregion
    }
}
