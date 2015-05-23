using mfc.domain.entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public class FileStageRepository : IFileStageRepository {
        private IUnitOfWorkProvider _unitOfWorkProvider;

        public FileStageRepository(IUnitOfWorkProvider unitOfWorkProvider) {
            _unitOfWorkProvider = unitOfWorkProvider;
        }

        protected ISession Session { get { return ((UnitOfWork)_unitOfWorkProvider.GetUnitOfWork()).Session; } }

        public virtual IEnumerable<FileStage> GetAll() {
            return Session.Query<FileStage>().ToList<FileStage>();
        }

        public void Update(FileStage entity) {
            Session.Update(entity);
        }
    }
}
