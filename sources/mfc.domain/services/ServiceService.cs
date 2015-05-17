﻿using mfc.dal.services;
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
        public IIdentifierService IdService { get; set; }

        [Inject]
        public IOrganizationService OrgService { get; set; }

        [Inject]
        public IUnitOfWorkProvider UnitOfWorkProvider { get; set; }

        [Inject]
        public IServiceRepository Repository { get; set; }

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

            var service = new Service {
                Id = IdService.GetId(),
                Caption = caption,
                Organization = OrgService.GetOrganizationById(organizationId),
                IsDeleted = false
            };

            var work_of_unit = UnitOfWorkProvider.GetUnitOfWork();

            work_of_unit.BeginTransaction();
            Repository.Create(service);
            work_of_unit.Commit();

            _is_cache_valid = false;
            
            return 0;
        }

        public void Update(Service service) {
            var work_of_unit = UnitOfWorkProvider.GetUnitOfWork();

            work_of_unit.BeginTransaction();
            Repository.Update(service);
            work_of_unit.Commit();

            _is_cache_valid = false;
        }

        public void Delete(Int64 id) {
            var work_of_unit = UnitOfWorkProvider.GetUnitOfWork();

            work_of_unit.BeginTransaction();
            Repository.Delete(id);
            work_of_unit.Commit();

            _is_cache_valid = false;
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
            return Repository.GetAll();
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
