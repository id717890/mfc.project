using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mfc.domain.entities;

namespace mfc.domain.services {
    public class FileService : IFileService {
        public IEnumerable<File> GetFiles(User user, DateTime date) {
            List<File> files = new List<File>();

            return files;
        }

        public File GetFileById(long Id) {
            throw new NotImplementedException();
        }

        public long Add(ServiceAction action) {
            throw new NotImplementedException();
        }

        public void Update(File file) {
            throw new NotImplementedException();
        }

        public void Delete(long id) {
            throw new NotImplementedException();
        }
    }
}
