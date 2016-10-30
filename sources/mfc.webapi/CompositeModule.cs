using AutoMapper;
using mfc.webapi.App_Start;
using Ninject.Modules;

namespace mfc.webapi
{
    public class CompositeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMapper>().ToMethod(x => { return AutoMapperConfig.Mapper(); });
        }
    }
}