using NHibernate.Linq;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using mfc.domain.services;

namespace mfc.dal.services {
    public class FileRepository : Repository<File>, IFileRepository {
        private Int64 _end_file_status_id = -1;

        public FileRepository(IUnitOfWorkProvider unitOfWorkProvider, IFileStageService fileStageService) : base(unitOfWorkProvider) {
            var end_status = fileStageService.GetStatusForStage(FileStages.Sended);
            
            if (end_status != null) {
                _end_file_status_id = end_status.Id;
            }
        }

        public File GetByActionId(long actionId) {
            var files = Session.Query<File>().Where(f => f.Action.Id == actionId).ToList();
            return files.Count > 0 ? files[0] : null;
        }

        public IEnumerable<File> GetFiles(DateTime beginDate, DateTime endDate, Int64 controllerId, Int64 expertId, Int64 statusId, Int64 orgId) {
            var query = Session.Query<File>();
            if (controllerId != User.All.Id) {
                query = query.Where(f => f.Controller.Id == controllerId || f.Controller == null);
            }

            if (expertId != User.All.Id) {
                query = query.Where(f => f.Expert.Id == expertId);
            }

            if (orgId != Organization.All.Id) {
                query = query.Where(f => f.Ogv.Id == orgId);
            }

            if (statusId != FileStatus.All.Id) {
                query = query.Where(f => f.CurrentStatus.Id == statusId);
            }
            else {
                //Исключаем из списка дела со статусом этапа "Отправлено в ОГВ"
                query = query.Where(f => f.CurrentStatus.Id != _end_file_status_id || f.CurrentStatus == null);
            }

            return query.Where(f=>!f.IsDeleted && f.Date >= beginDate && f.Date <= endDate).OrderByDescending(m=>m.Date).ThenByDescending(m=>m.Id).ToList();
        }
    }
}
