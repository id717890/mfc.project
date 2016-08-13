using NHibernate.Linq;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mfc.domain.models;
using Ninject;
using mfc.domain.services;
using mfc.infrastructure.services;
using NHibernate.Cache;
using NHibernate.Proxy;

namespace mfc.dal.services {
    public class FileRepository : Repository<File>, IFileRepository {
        [Inject]
        public ILogger Logger { get; set; }

        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        public FileRepository(IUnitOfWorkProvider unitOfWorkProvider, IFileStageService fileStageService) : base(unitOfWorkProvider) {
            var end_status = fileStageService.GetStatusForStage(FileStages.Sended);

            if (end_status != null) {
                _end_file_status_id = end_status.Id;
            }
        }

        public File GetByActionId(long actionId) {
            var files = Session.Query<File>().Where(f => f.Action.Id == actionId).ToList();
            return files.Count > 0 ? files[0] : null;
        }

        public IEnumerable<File> GetFiles(DateTime beginDate, DateTime endDate, Int64 controllerId, Int64 expertId, Int64 statusId, Int64 orgId, Int64 serviceId) {
            var queryParams = new Dictionary<string, Int64>();
            var queryString = @"
                    from File f                         
                        join fetch f.Action            
                        join fetch f.Action.Service            
                        left join fetch f.Expert
                        left join fetch f.Controller
                        left join fetch f.Ogv
                        left join fetch f.CurrentStatus
                    where f.IsDeleted = false
                        and f.Date between :date1 and :date2 ";

            if (controllerId != User.All.Id) {
                queryString += " and  (f.Controller = null or f.Controller.Id = :controllerId) ";
                queryParams.Add("controllerId", controllerId);
            }

            if (expertId != User.All.Id) {
                queryString += " and f.Expert.Id = :expertId ";
                queryParams.Add("expertId", expertId);
            }

            if (orgId != Organization.All.Id) {
                queryString += " and f.Ogv.Id = :orgId";
                queryParams.Add("orgId", orgId);
            }

            if (serviceId != Service.All.Id) {
                queryString += " and f.Action.Service.Id = :serviceId";
                queryParams.Add("serviceId", serviceId);
            }

            if (statusId != FileStatus.All.Id) {
                queryString += " and f.CurrentStatus.Id = :statusId";
                queryParams.Add("statusId", statusId);
            }
            else {
                //Исключаем из списка дела со статусом этапа "Отправлено в ОГВ"
                queryString += " and (f.CurrentStatus.Id != :statusId or f.CurrentStatus = null) ";
                queryParams.Add("statusId", _end_file_status_id);
            }

            queryString += " order by f.Date desc, f.Id desc";

            Logger.Debug($"FileRepository.GetFiles {queryString}");

            var query = Session.
             CreateQuery(queryString)
                .SetParameter("date1", beginDate)
                .SetParameter("date2", endDate);

            foreach (var param in queryParams) {
                query.SetParameter(param.Key, param.Value);
            }

            Logger.Debug("FileRepository.GetFiles before create result list");

            var files = query.SetReadOnly(true).List<File>();

            Logger.Debug("FileRepository.GetFiles after create result list");

            return files;
        }

        public IEnumerable<FileRecord> GetFileRecords(DateTime beginDate, DateTime endDate, Int64 controllerId, Int64 expertId, Int64 statusId, Int64 orgId, Int64 serviceId) {
            var files = new List<FileRecord>();

            SqlConnection connection = SqlProvider.CreateConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try {
                cmd = connection.CreateCommand();

                cmd.CommandText = @"
            select f.id, a.id as action_id, f.caption, f.dt, s.caption as service, u1.name expert, u2.name controller, o.caption organization, st.caption status
                from Files f
                join Actions a on a.id = f.action_id
                join Services s on s.id = a.service_id
                left join Users u1 on u1.id = f.expert_id
                left join Users u2 on u1.id = f.controller_id
                left join Organizations o on o.id = f.org_id
                left join Statuses st on st.id = f.status_id 
            where f.dt between @date1 and @date2 
                and f.is_deleted = 0 ";
                var queryParams = new Dictionary<string, Int64>();

                if (controllerId != User.All.Id) {
                    cmd.CommandText += " and  (f.controller_id is null or f.controller_d = @controllerId) ";
                    queryParams.Add("controllerId", controllerId);
                }

                if (expertId != User.All.Id) {
                    cmd.CommandText += " and f.expert_id = @expertId ";
                    queryParams.Add("expertId", expertId);
                }

                if (orgId != Organization.All.Id) {
                    cmd.CommandText += " and f.org_id = @orgId";
                    queryParams.Add("orgId", orgId);
                }

                if (serviceId != Service.All.Id) {
                    cmd.CommandText += " and a.service_id = @serviceId";
                    queryParams.Add("serviceId", serviceId);
                }

                if (statusId != FileStatus.All.Id) {
                    cmd.CommandText += " and f.status_id = @statusId";
                    queryParams.Add("statusId", statusId);
                }
                else {
                    //Исключаем из списка дела со статусом этапа "Отправлено в ОГВ"
                    cmd.CommandText += " and (f.status_id != @statusId or f.status_id is null) ";
                    queryParams.Add("statusId", _end_file_status_id);
                }

                cmd.CommandText += " order by f.dt desc, f.Id desc";

                Logger.Debug($"FileRepository.GetFiles {cmd.CommandText}");

                cmd.Parameters.Add("date1", beginDate);
                cmd.Parameters.Add("date2", endDate);

                foreach (var param in queryParams) {
                    cmd.Parameters.Add(param.Key, param.Value);
                }

                Logger.Debug("FileRepository.GetFiles before create result list");

                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    files.Add(new FileRecord {
                        Id = Convert.ToInt64(reader["id"]),
                        Caption = reader["caption"] != DBNull.Value ? Convert.ToString(reader["caption"]) : string.Empty,
                        ActionId = Convert.ToInt64(reader["action_id"]),
                        Service = Convert.ToString(reader["service"]),
                        Date = Convert.ToDateTime(reader["dt"]),
                        Organization =
                            reader["organization"] != DBNull.Value
                                ? Convert.ToString(reader["organization"])
                                : String.Empty,
                        Expert = reader["expert"] != DBNull.Value ? Convert.ToString(reader["expert"]) : string.Empty,
                        Controller =
                            reader["controller"] != DBNull.Value ? Convert.ToString(reader["controller"]) : string.Empty,
                        Status = reader["status"] != DBNull.Value ? Convert.ToString(reader["status"]) : string.Empty
                    });
                }

                Logger.Debug("FileRepository.GetFiles after create result list");
            }
            catch (Exception e) {
                Logger.Error(e);
                throw;
            }
            finally {
                if (reader != null) {
                    reader.Close();
                    reader.Dispose();
                }

                if (cmd != null) {
                    cmd.Dispose();
                }
                connection.Close();
            }

            return files;
        }

        private Int64 _end_file_status_id = -1;
    }
}
