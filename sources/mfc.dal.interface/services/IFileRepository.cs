using mfc.domain.entities;
using System;
using System.Collections.Generic;
using mfc.domain.models;

namespace mfc.dal.services {
    public interface IFileRepository : IRepository<File> {
        File GetByActionId(Int64 actionId);
        IEnumerable<File> GetFiles(DateTime beginDate, DateTime endDate, Int64 controllerId, Int64 expertId, Int64 statusId, Int64 orgId, Int64 serviceId);
        IEnumerable<FileRecord> GetFileRecords(DateTime beginDate, DateTime endDate, Int64 controllerId, Int64 expertId, Int64 statusId, Int64 orgId, Int64 serviceId);

        /*Рефакторинг*/
        Int64 TotalRows { get; set; }
        IEnumerable<File> GetFiles(DateTime dateBegin, DateTime dateEnd, Int64 fileStatus, Int64 organization, Int64 service, Int64 expert, Int64 controller, Int32 pageIndex, Int32 pageSize);
    }
}
