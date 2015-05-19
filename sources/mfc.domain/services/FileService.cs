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

        public IEnumerable<File> GetFiles(User user, DateTime date) {
            return FileRepository.GetAll();
        }

        public File GetFileById(long Id) {
            return FileRepository.GetById(Id);
        }

        public long Add(ServiceAction action) {
            Debug.Assert(action.Type.NeedMakeFile);

            File file = new File {
                Caption = action.Customer,
                Date = action.Date,
                Expert = action.User,
                Ogv = action.Service.Organization,
                Action = action
            };
            
            //операторные скобки UnitOfWork.BeginTransaction() and UnitOfWork.Commit()
            //не используем, поскольку предполагается использование этого метода
            //в уже созданнных
            FileRepository.Create(file);
            
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
    }
}
