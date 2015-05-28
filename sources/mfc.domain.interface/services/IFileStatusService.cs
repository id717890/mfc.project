using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mfc.domain.entities;

namespace mfc.domain.services {
    public interface IFileStatusService {
        IEnumerable<FileStatus> GetAllStatuses();
        FileStatus GetStatusById(Int64 id);
        Int64 Create(string caption);
        void Update(FileStatus status);
        void Delete(Int64 statusId);

        IEnumerable<FileStatusInfo> GetFileStatuses(Int64 fileId);
        void SetStatus(Int64 fileId, Int64 statusId, DateTime date);
    }
}
