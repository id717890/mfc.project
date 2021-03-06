﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mfc.domain.entities;
using mfc.infrastructure.services;
using Ninject;
using mfc.dal.services;

namespace mfc.domain.services {
    public class FileStatusService : IFileStatusService {
        private readonly object sync_obj = new object();

        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public IIdentifierService IdService { get; set; }

        [Inject]
        public IUnitOfWorkProvider UnitOfWorkProvider { get; set; }

        [Inject]
        public IFileStatusRepository Repository { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

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

            var status = new FileStatus {
                Caption = caption
            };

            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            Repository.Create(status);
            unit_of_work.Commit();

            _is_cache_valid = false;

            return status.Id;
        }

        public void Update(FileStatus status) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            Repository.Update(status);
            unit_of_work.Commit();

            _is_cache_valid = false;
        }

        public void Delete(Int64 statusId) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            Repository.Delete(statusId);
            unit_of_work.Commit();

            _is_cache_valid = false;
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
            return Repository.GetAll();
        }

        #endregion

        #region FileStatusInfo
        
        public IEnumerable<FileStatusRegistry> GetFileStatuses(long fileId) {
            var statuses = new List<FileStatusRegistry>();

            var conn = SqlProvider.CreateConnection();
            SqlCommand cmd = null;


            try {
                cmd = conn.CreateCommand();
                cmd.CommandText = @"
                      select FileStatus.file_id, FileStatus.dt, FileStatus.status_id, FileStatus.user_id, FileStatus.comments 
	                        from FileStatus
	                        join Files on Files.id = FileStatus.file_id
	                        where Files.is_deleted = 0
                        and FileStatus.file_id = @file_id
                        order by FileStatus.dt desc";
                cmd.Parameters.Add(new SqlParameter("file_id", fileId));
                
                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    statuses.Add(new FileStatusRegistry {
                        FileId= fileId,
                        Date = Convert.ToDateTime(reader["dt"]),
                        User = UserService.GetUserById(Convert.ToInt64(reader["user_id"])),
                        Status = GetStatusById(Convert.ToInt64(reader["status_id"])),
                        Comments = Convert.ToString(reader["comments"])
                    });
                }
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                conn.Dispose();
                conn.Close();
                if (cmd != null) {
                    cmd.Dispose();
                }
            }

            return statuses;
        }

        public void SetStatus(long fileId, long statusId, DateTime date, string comments) {
            var conn = SqlProvider.CreateConnection();
            SqlCommand cmd = null;

            /*Т.к. авторизация еще не реализована берем админа = 1*/
            var user = UserService.GetUserById(1);
//            var user = UserService.GetCurrentUser();
//            if (user == null) {
//                throw new DomainException("Для текущего контекста не определен пользователь. Метод IUserService.GetCurrentUser вернул пустое значение");
//            }

            try {
                cmd = conn.CreateCommand();
                cmd.CommandText = @"
                      insert into FileStatus (file_id, dt, status_id, user_id, comments)
                        values (@file_id, @dt, @status_id, @user_id, @comments)";
                cmd.Parameters.Add(new SqlParameter("file_id", fileId));
                cmd.Parameters.Add(new SqlParameter("dt", date));
                cmd.Parameters.Add(new SqlParameter("user_id", user.Id));
                cmd.Parameters.Add(new SqlParameter("status_id", statusId));
                cmd.Parameters.Add(new SqlParameter("comments", string.IsNullOrEmpty(comments) ? string.Empty : comments));

                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                throw e;
            }
            finally {
                conn.Dispose();
                conn.Close();
                if (cmd != null) {
                    cmd.Dispose();
                }
            }
        }

        public void DeleteStatus(Int64 fileId, Int64 statusId) {
            var conn = SqlProvider.CreateConnection();
            SqlCommand cmd = null;

            try {
                cmd = conn.CreateCommand();
                cmd.CommandText = @"
                      delete from FileStatus 
                        where file_id = @file_id and status_id = @status_id";
                cmd.Parameters.Add(new SqlParameter("file_id", fileId));
                cmd.Parameters.Add(new SqlParameter("status_id", statusId));

                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                throw e;
            }
            finally {
                conn.Dispose();
                conn.Close();
                if (cmd != null) {
                    cmd.Dispose();
                }
            }
        }

        public FileStatusRegistry GetLastStatuses(Int64 fileId) {
            FileStatusRegistry status = null;

            var conn = SqlProvider.CreateConnection();
            SqlCommand cmd = null;


            try {
                cmd = conn.CreateCommand();
                cmd.CommandText = @"
                      select FileStatus.file_id, FileStatus.dt, FileStatus.status_id, FileStatus.user_id, FileStatus.comments 
	                        from FileStatus
	                        join Files on Files.id = FileStatus.file_id
	                        where Files.is_deleted = 0
                        and FileStatus.file_id = @file_id
                        order by FileStatus.dt desc";
                cmd.Parameters.Add(new SqlParameter("file_id", fileId));

                var reader = cmd.ExecuteReader();
                if (reader.Read()) {
                    status = new FileStatusRegistry {
                        FileId = fileId,
                        Date = Convert.ToDateTime(reader["dt"]),
                        User = UserService.GetUserById(Convert.ToInt64(reader["user_id"])),
                        Status = GetStatusById(Convert.ToInt64(reader["status_id"])),
                        Comments = Convert.ToString(reader["comments"])
                    };
                }
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                conn.Dispose();
                conn.Close();
                if (cmd != null) {
                    cmd.Dispose();
                }
            }

            return status;
        }

        #endregion
    }
}
