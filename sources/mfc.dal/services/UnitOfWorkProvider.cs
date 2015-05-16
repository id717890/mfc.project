using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public class UnitOfWorkProvider : IUnitOfWorkProvider {
        private IKernel _kernel;

        public UnitOfWorkProvider(IKernel kernel) {
            _kernel = kernel;
        }
        public IUnitOfWork GetUnitOfWork() {
            return _kernel.Get<IUnitOfWork>();
        }
    }
}
