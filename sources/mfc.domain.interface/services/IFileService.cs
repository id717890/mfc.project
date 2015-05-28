using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.services {
    /// <summary>
    /// Интерфейс для работы с делами
    /// </summary>
    public interface IFileService {
        IEnumerable<File> GetFiles(User user, DateTime date);
        File GetFileById(Int64 Id);
        File GetFileByActionId(Int64 actionId);
        Int64 Add(ServiceAction action);
        void Update(File file);
        void Delete(Int64 id);

        void SendForControl(Int64 fileId, Int64 controllerId, string comments);
        void Return(Int64 fileId, Int64 expertId, string comments);
        void Checked(Int64 fileId, string comments);
    }
}
