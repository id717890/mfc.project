using mfc.dal.services;
using mfc.domain.services;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal {
    public class CompositeModule : NinjectModule {
        public override void Load() {
            Kernel.Bind<IOrganizationTypeRepository>().To<OrganizationTypeRepository>().InSingletonScope();
            Kernel.Bind<IActionTypeRepository>().To<ActionTypeRepository>().InSingletonScope();
            Kernel.Bind<IOrganizationRepository>().To<OrganizationRepository>().InSingletonScope();
            Kernel.Bind<IFileRepository>().To<FileRepository>();
            Kernel.Bind<IServiceRepository>().To<ServiceRepository>();
            Kernel.Bind<IUnitOfWorkProvider>().ToConstant(new UnitOfWorkProvider(Kernel)).InSingletonScope();
        }
    }
}
