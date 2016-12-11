using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class ModuleModelProfile : Profile
    {
        public ModuleModelProfile()
        {
            /*
            CreateMap<Module, ModuleModel>()
                .ForMember(x => x.Parent, s => s.MapFrom(x => x.Parent));

            CreateMap<ModuleModel, Module>()
                .ForMember(x => x.Parent, s => s.MapFrom(x => x.Parent));
                */
            CreateMap<Module, ModuleModel>().ReverseMap();
        }
    }
}