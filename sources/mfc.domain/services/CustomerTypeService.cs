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
using NHibernate.Type;

namespace mfc.domain.services {
    public class CustomerTypeService : ICustomerTypeService {
        private readonly object sync_obj = new object();

        [Inject]
        public ICustomerTypeRepository Repository { get; set; }

        [Inject]
        public IUnitOfWorkProvider UnitOfWorkProvider { get; set; }

        private Dictionary<Int64, CustomerType> _cache = new Dictionary<long, CustomerType>();
        private bool _is_cache_valid = false;

        public IEnumerable<CustomerType> GetAllTypes() {
            PrepareCache();

            return _cache.Values.OrderBy(m => m.Id);
        }

        public CustomerType GetTypeById(long id) {
            PrepareCache();

            CustomerType type = null;

            if (_cache.ContainsKey(id)) {
                type = _cache[id];
            }

            return type;
        }

        public long Create(string caption) {
            Debug.Assert(!string.IsNullOrEmpty(caption));

            var type = new CustomerType {
                Caption = caption
            };

            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            Repository.Create(type);
            unit_of_work.Commit();

            _is_cache_valid = false;

            return 0;
        }

        public void Update(CustomerType type) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            Repository.Update(type);
            unit_of_work.Commit();

            _is_cache_valid = false;
        }

        public void Delete(Int64 typeId) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            Repository.Delete(typeId);
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

        private IEnumerable<CustomerType> GetTypesInternal() {
            return Repository.GetAll();
        }

        #endregion
    }
}
