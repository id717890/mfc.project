using mfc.domain.entities;
using System;
using System.Collections.Generic;
using mfc.domain.models;

namespace mfc.dal.services {
    public interface IFileRepository : IRepository<File> {
        File GetByActionId(Int64 actionId);
        IEnumerable<File> GetFiles(DateTime beginDate, DateTime endDate, Int64 controllerId, Int64 expertId, Int64 statusId, Int64 orgId, Int64 serviceId);
        IEnumerable<FileRecord> GetFileRecords(DateTime beginDate, DateTime endDate, Int64 controllerId, Int64 expertId, Int64 statusId, Int64 orgId, Int64 serviceId);
    }
}
