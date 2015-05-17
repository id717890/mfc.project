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
                Caption = caption,
            };

            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            Repository.Create(status);
            unit_of_work.Commit();

            _is_cache_valid = false;

            return 0;
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
