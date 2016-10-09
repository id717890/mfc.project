using mfc.dal.services;
using mfc.domain.entities;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.services
{
    public class FileStageService : IFileStageService
    {
        private readonly object sync_obj = new object();
        private readonly object sync_obj_update = new object();

        private Dictionary<string, FileStage> _cacheCode = new Dictionary<string, FileStage>();
        private Dictionary<long, FileStage> _cacheStatus = new Dictionary<long, FileStage>();

        private bool _is_cache_valid = false;

        [Inject]
        public IUnitOfWorkProvider UnitOfWorkProvider { get; set; }

        [Inject]
        public IFileStageRepository Repository { get; set; }

        public IEnumerable<FileStage> GetAllStages()
        {
            PrepareCache();
            return _cacheCode.Values.OrderBy(x => x.Order);
        }

        public void UpdateStages(IEnumerable<FileStage> stages)
        {
            lock (sync_obj_update)
            {
                var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

                unit_of_work.BeginTransaction();
                foreach (var stage in stages)
                {
                    Repository.Update(stage);
                }
                unit_of_work.Commit();

                _is_cache_valid = false;
            }
        }

        public void UpdateStage(FileStage fileStage)
        {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            Repository.Update(fileStage);
            unit_of_work.Commit();

            _is_cache_valid = false;
        }

        public FileStatus GetStatusForStage(string code)
        {
            PrepareCache();
            if (!_cacheCode.ContainsKey(code))
            {
                throw new DomainException(string.Format("Для кода {0} стадия дела неопределена"));
            }
            return _cacheCode[code].Status;
        }

        public void PrepareCache()
        {
            if (_is_cache_valid)
            {
                return;
            }

            lock (sync_obj)
            {
                _cacheCode.Clear();
                _cacheStatus.Clear();
                foreach (var stage in Repository.GetAll())
                {
                    _cacheCode.Add(stage.Code, stage);
                    if (!_cacheStatus.ContainsKey(stage.Status.Id)) _cacheStatus.Add(stage.Status.Id, stage);
                }

                _is_cache_valid = true;
            }
        }


        public FileStage GetStage(string code)
        {
            PrepareCache();
            return _cacheCode[code];
        }

        public FileStage GetStageByStatus(long status)
        {
            PrepareCache();
            return _cacheStatus[status];
        }
    }
}
