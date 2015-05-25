using NHibernate.Linq;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public class FileRepository : Repository<File>, IFileRepository {
        public FileRepository(IUnitOfWorkProvider unitOfWorkProvider) : base(unitOfWorkProvider) { }


        public File GetByActionId(long actionId) {
            var files = Session.Query<File>().Where(f => f.Action.Id == actionId).ToList();
            return files.Count > 0 ? files[0] : null;
        }
    }
}
