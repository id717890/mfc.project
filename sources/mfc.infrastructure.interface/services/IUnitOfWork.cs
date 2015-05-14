using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.infrastructure.services {
    public interface IUnitOfWork {
        void BeginTransaction();
        void Commit();
    }
}
