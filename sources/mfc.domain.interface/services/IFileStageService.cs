using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.services {
    public interface IFileStageService {
        IEnumerable<FileStage> GetAllStages();
        FileStage GetStage(string code);
        FileStage GetStageByStatus(long status);
        void UpdateStages(IEnumerable<FileStage> stages);
        FileStatus GetStatusForStage(string code);
    }
}
