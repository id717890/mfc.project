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
    public class ActionTypeService : IActionTypeService {
        private readonly object sync_obj = new object();

        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public IActionTypeRepository Repository { get; set; }

        [Inject]
        public IUnitOfWorkProvider UnitOfWorkProvider { get; set; }

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

        public long Create(string caption, bool needMakeFile) {
            Debug.Assert(!string.IsNullOrEmpty(caption));

            var type = new ActionType {
                Caption = caption,
                NeedMakeFile = needMakeFile
            };

            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            Repository.Create(type);
            unit_of_work.Commit();

            _is_cache_valid = false;

            return type.Id;
        }

        public void Update(ActionType type) {
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

        private IEnumerable<ActionType> GetTypesInternal() {
            return Repository.GetAll();
        }

        #endregion
    }
}
