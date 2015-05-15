using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public class FileRepository : Repository<File>, IFileRepository {
        public FileRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
