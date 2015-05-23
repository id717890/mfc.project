using mfc.dal.services;
using mfc.domain.entities;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.services {
    public class FileStageService : IFileStageService {
        private readonly object sync_obj = new object();
        private readonly object sync_obj_update = new object();
        private Dictionary<string, FileStage> _cache = new Dictionary<string, FileStage>();
        private bool _is_cache_valid = false;

        [Inject]
        public IUnitOfWorkProvider UnitOfWorkProvider { get; set; }

        [Inject]
        public IFileStageRepository Repository { get; set; }

        public IEnumerable<FileStage> GetAllStages() {
            PrepareCache();
            return _cache.Values.OrderBy(x => x.Order);
        }

        public void UpdateStage(FileStage stage) {
            lock (sync_obj_update) {
                var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

                unit_of_work.BeginTransaction();
                Repository.Update(stage);
                unit_of_work.Commit();

                _is_cache_valid = false;
            }
        }

        public FileStatus GetStatusForStage(string code) {
            PrepareCache();
            if (!_cache.ContainsKey(code)) {
                throw new DomainException(string.Format("Для кода {0} стадия дела неопределена"));
            }
            return _cache[code].Status;
        }

        public void PrepareCache() {
            if (_is_cache_valid) {
                return;
            }

            lock (sync_obj) {
                _cache.Clear();
                foreach (var stage in Repository.GetAll()) {
                    _cache.Add(stage.Code, stage);
                }

                _is_cache_valid = true;
            }
        }
    }
}
