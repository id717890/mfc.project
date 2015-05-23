using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.services {
    public interface IFileStageService {
        IEnumerable<FileStage> GetAllStages();
        void UpdateStage(FileStage stage);
        FileStatus GetStatusForStage(string code);
    }
}
