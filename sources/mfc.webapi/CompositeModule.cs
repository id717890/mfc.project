using AutoMapper;
using mfc.domain.services;
using mfc.webapi.App_Start;
using mfc.webapi.Helpers;
using Ninject.Modules;

namespace mfc.webapi
{
    public class CompositeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMapper>().ToMethod(x => { return AutoMapperConfig.Mapper(); });
            Kernel.Bind<IModuleService>().To<ModuleService>().InSingletonScope();
            Kernel.Bind<IModulesProvider>().To<ModulesProvider>().InSingletonScope();
            Kernel.Bind<IModulesConfiguration>().To<ModulesFileConfiguration>().InSingletonScope();
        }
    }
}