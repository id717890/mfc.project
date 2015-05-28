	using System;
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

        public IEnumerable<File> GetFiles(Int64 controllerId, Int64 expertId, Int64 statusId, Int64 orgId) {
            return FileRepository.GetFiles(controllerId, expertId, statusId, orgId);
        }

        public File GetFileById(long Id) {
            return FileRepository.GetById(Id);
        }

        public long Add(ServiceAction action) {
            Debug.Assert(action.Type.NeedMakeFile);

            var status = FileStageService.GetStatusForStage(FileStages.NewFile);
            if (status == null) {
                throw new DomainException(string.Format("Не определен статус для новых дел"));
            }

            File file = new File {
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
            //Прямого удаления дела не предполагается, только из внешних транзакций
            FileRepository.Delete(id);
        }


        public File GetFileByActionId(long actionId) {
            return FileRepository.GetByActionId(actionId);
        }


        public void SendForControl(long fileId, long controllerId, string comments) {
            var file = GetFileById(fileId);
            if (file == null) {
                throw new ArgumentException(string.Format("Дело с идентификатором {0} не найдено", fileId));
            }
            
            var user = UserService.GetUserById(controllerId);
            if (user == null) {
                throw new ArgumentException(string.Format("Пользователь с идентификатором {0} не найден", controllerId));
            }

            var status = FileStageService.GetStatusForStage(FileStages.SendForControl);

            if (status == null) {
                throw new ArgumentException(string.Format("Не определен статус для дел, переданных на проверку"));
            }

            file.Controller = user;
            file.CurrentStatus = status;

            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();
            
            unit_of_work.BeginTransaction();
            FileRepository.Update(file);
            FileStatusService.SetStatus(fileId, status.Id, DateTime.Now, comments);
            unit_of_work.Commit();
        }

        public void Return(long fileId, long expertId, string comments) {
            var file = GetFileById(fileId);
            if (file == null) {
                throw new ArgumentException(string.Format("Дело с идентификатором {0} не найдено", fileId));
            }

            var user = UserService.GetUserById(expertId);
            if (user == null) {
                throw new ArgumentException(string.Format("Пользователь с идентификатором {0} не найден", expertId));
            }

            var status = FileStageService.GetStatusForStage(FileStages.ReturnForFix);

            if (status == null) {
                throw new ArgumentException(string.Format("Не определен статус для дел, переданных на проверку"));
            }

            file.Expert = user;
            file.CurrentStatus = status;

            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            FileRepository.Update(file);
            FileStatusService.SetStatus(fileId, status.Id, DateTime.Now, comments);
            unit_of_work.Commit();
        }


        public void Checked(long fileId, string comments) {
            var file = GetFileById(fileId);
            if (file == null) {
                throw new ArgumentException(string.Format("Дело с идентификатором {0} не найдено", fileId));
            }

            var status = FileStageService.GetStatusForStage(FileStages.Checked);

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
    }
}
