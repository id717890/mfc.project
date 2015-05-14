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

namespace mfc.domain.services {
    public class FileService : IFileService {
        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public IIdentifierService IdService { get; set; }

        [Inject]
        public IFileRepository FileRepository { get; set; }

        public IEnumerable<File> GetFiles(User user, DateTime date) {
            return FileRepository.GetAll();
        }

        public File GetFileById(long Id) {
            return FileRepository.GetById(Id);
        }

        public long Add(ServiceAction action) {
            File file = new File {
                Caption = action.Customer
            };

            FileRepository.Create(file);

            return file.Id;
        }

        public void Update(File file) {
            throw new NotImplementedException();
        }

        public void Delete(long id) {
            throw new NotImplementedException();
        }
    }
}
