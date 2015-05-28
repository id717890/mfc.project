﻿using mfc.domain.entities;
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
        IEnumerable<File> GetFiles(Int64 controllerId, Int64 expertId, Int64 statusId, Int64 orgId);
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