using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mfc.domain.models;

namespace mfc.domain.services {
    /// <summary>
    /// Интерфейс для работы с делами
    /// </summary>
    public interface IFileService {
        IEnumerable<FileRecord> GetFiles(DateTime beginDate, DateTime endDate, Int64 controllerId, Int64 expertId, Int64 statusId, Int64 orgId, Int64 serviceId);
        File GetFileById(Int64 Id);
        File GetFileByActionId(Int64 actionId);
        Int64 Add(ServiceAction action);
        void Update(File file);
        void Delete(Int64 id);

        void SendForControl(Int64 fileId, string comments);
        IEnumerable<Int64> AcceptForControl(IEnumerable<Int64> file_ids);
        void Return(Int64 fileId, string comments);
        void Checked(Int64 fileId, string comments);
    }
}
