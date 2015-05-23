using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public interface IFileStageRepository {
        IEnumerable<FileStage> GetAll();
        void Update(FileStage stage);
    }
}
