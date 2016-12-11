using mfc.dal.services;
using Ninject.Modules;

namespace mfc.dal {
    public class CompositeModule : NinjectModule {
        public override void Load() {
            Kernel.Bind<IOrganizationTypeRepository>().To<OrganizationTypeRepository>().InSingletonScope();
            Kernel.Bind<IActionTypeRepository>().To<ActionTypeRepository>().InSingletonScope();
            Kernel.Bind<IOrganizationRepository>().To<OrganizationRepository>().InSingletonScope();
            Kernel.Bind<IFileRepository>().To<FileRepository>();
            Kernel.Bind<IUserRepository>().To<UserRepository>();
            Kernel.Bind<IFileStageRepository>().To<FileStageRepository>();
            Kernel.Bind<IFileStatusRepository>().To<FileStatusRepository>();
            Kernel.Bind<IServiceActionRepository>().To<ServiceActionRepository>();
            Kernel.Bind<IServiceRepository>().To<ServiceRepository>();
            Kernel.Bind<IPackageRepository>().To<PackageRepository>();
            Kernel.Bind<IUnitOfWorkProvider>().ToConstant(new UnitOfWorkProvider(Kernel)).InSingletonScope();
            Kernel.Bind<ICustomerTypeRepository>().To<CustomerTypeRepository>().InSingletonScope();
            Kernel.Bind<IModulePermissionRepository>().To<ModulePermissionRepository>().InSingletonScope();
        }
    }
}
