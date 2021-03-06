﻿    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mfc.domain.entities;
using Ninject;
using mfc.infrastructure.services;
using System.Data.SqlClient;
using mfc.dal.services;
using System.Diagnostics;
    using mfc.domain.models;

namespace mfc.domain.services {
    public class FileService : IFileService {
        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public IIdentifierService IdService { get; set; }

        [Inject]
        public IFileRepository FileRepository { get; set; }

        [Inject]
        public IUnitOfWorkProvider UnitOfWorkProvider { get; set; }

        [Inject]
        public IFileStageService FileStageService { get; set; }

        [Inject]
        public IFileStatusService FileStatusService { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        public IEnumerable<FileRecord> GetFiles(DateTime beginDate, DateTime endDate, Int64 controllerId, Int64 expertId, Int64 statusId, Int64 orgId, Int64 serviceId) {
            return FileRepository.GetFileRecords(beginDate, endDate, controllerId, expertId, statusId, orgId, serviceId).OrderByDescending(x=>x.Date).ThenByDescending(x=>x.Id);
        }

        public File GetFileById(long Id) {
            return FileRepository.GetById(Id);
        }

        public long Add(ServiceAction action) {
            Debug.Assert(action.Type.NeedMakeFile);

            //Возраждение удаленного дела
            File file = FileRepository.GetByActionId(action.Id);

            if (file != null && file.IsDeleted) {
                file.IsDeleted = false;
                FileRepository.Update(file);

                return file.Id;
            }

            //Создание нового дела
            var status = FileStageService.GetStatusForStage(FileStages.NewFile);
            if (status == null) {
                throw new DomainException(string.Format("Не определен статус для новых дел"));
            }

            file = new File {
                Caption = action.Customer,
                Date = action.Date,
                Expert = action.User,
                Ogv = action.Service.Organization,
                Action = action,
                CurrentStatus = status
            };
            
            //операторные скобки UnitOfWork.BeginTransaction() and UnitOfWork.Commit()
            //не используем, поскольку предполагается использование этого метода
            //в уже созданнных
            FileRepository.Create(file);
            FileStatusService.SetStatus(file.Id, status.Id, DateTime.Now, string.Empty);
            
            return file.Id;
        }

        public void Update(File file) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            FileRepository.Update(file);
            unit_of_work.Commit();
        }

        public void Delete(long id) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            FileRepository.Delete(id);
            unit_of_work.Commit();
        }


        public File GetFileByActionId(long actionId) {
            return FileRepository.GetByActionId(actionId);
        }


        public void SendForControl(long fileId, string comments) {
            var file = GetFileById(fileId);
            if (file == null) {
                throw new ArgumentException(string.Format("Дело с идентификатором {0} не найдено", fileId));
            }
            
            var status = FileStageService.GetStatusForStage(FileStages.SendForControl);

            if (status == null) {
                throw new ArgumentException(string.Format("Не определен статус для дел, переданных на проверку"));
            }

            file.CurrentStatus = status;

            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();
            
            unit_of_work.BeginTransaction();
            FileRepository.Update(file);
            FileStatusService.SetStatus(fileId, status.Id, DateTime.Now, comments);
            unit_of_work.Commit();
        }

        public void Return(long fileId, string comments) {
            var file = GetFileById(fileId);
            if (file == null) {
                throw new ArgumentException(string.Format("Дело с идентификатором {0} не найдено", fileId));
            }

            var status = FileStageService.GetStatusForStage(FileStages.ReturnForFix);

            if (status == null) {
                throw new ArgumentException(string.Format("Не определен статус для дел, переданных на проверку"));
            }

            file.CurrentStatus = status;

            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            FileRepository.Update(file);
            FileStatusService.SetStatus(fileId, status.Id, DateTime.Now, comments);
            unit_of_work.Commit();
        }


        public void Checked(long fileId, string comments) {
            SetStage(new[] { fileId }, FileStages.Checked, string.Empty);
        }

        public void SetStage(IEnumerable<long> file_ids, string stage, string comments) {
            foreach (var file_id in file_ids) {
                var file = GetFileById(file_id);
                if (file == null) {
                    throw new ArgumentException(string.Format("Дело с идентификатором {0} не найдено", file_id));
                }

                var status = FileStageService.GetStatusForStage(stage);
                if (status == null) {
                    throw new ArgumentException(string.Format("Не определен статус для дел, переданных на проверку"));
                }

                file.CurrentStatus = status;

                var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();
                unit_of_work.BeginTransaction();
                FileRepository.Update(file);
                FileStatusService.SetStatus(file_id, status.Id, DateTime.Now, comments);
                unit_of_work.Commit();
            }
        }

        public IEnumerable<long> AcceptForControl(IEnumerable<long> file_ids) {
            //1. Проверяем право текущего пользователя принимать дела
            //2. Проверяем наличие статуса для дел, переданных на контроль
            //3. Формируем список дел (из переданных), которые еще не заняты другими контролерами
            //4. В одной транзакции принимает дел на контроль

            //1.
            var user = UserService.GetCurrentUser();

            if (!user.IsController) {
                throw new DomainException("Текущий пользователь не имеет прав для приема дел");
            }

            //2.
            var status = FileStageService.GetStatusForStage(FileStages.SendForControl);

            if (status == null) {
                throw new ArgumentException(string.Format("Не определен статус для дел, переданных на проверку"));
            }

            //3. 
            List<File> free_files = new List<File>();
            List<Int64> free_file_ids = new List<Int64>();
            foreach (var id in file_ids) {
                var file = GetFileById(id);

                if (file.Controller == null) {
                    free_files.Add(file);
                    free_file_ids.Add(id);
                }
            }

            //4. 
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();
            
            unit_of_work.BeginTransaction();

            foreach (var file in free_files) {
                file.Controller = user;
                file.CurrentStatus = status;
                FileRepository.Update(file);
                FileStatusService.SetStatus(file.Id, status.Id, DateTime.Now, string.Empty);
            }
                        
            unit_of_work.Commit();

            return free_file_ids;
        }

        public KeyValuePair<long, IEnumerable<File>> GetFiles(DateTime beginDate, DateTime endDate, long fileStatus, long organization, long service, long expert, long controller, int pageIndex, int pageSize)
        {
            var files = FileRepository.GetFiles(beginDate, endDate, fileStatus, organization, service, expert, controller, pageIndex, pageSize).OrderByDescending(x => x.Date).ThenByDescending(x => x.Id);
            return new KeyValuePair<long, IEnumerable<File>>(FileRepository.TotalRows, files);
        }

    }
}
