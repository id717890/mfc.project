using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class ActionTypeModelProfile : Profile
    {
        public ActionTypeModelProfile()
        {
            CreateMap<ActionType, ActionTypeInfo>()
                .ForMember(x => x.Caption, s => s.MapFrom(x => x.Caption))
                .ForMember(x => x.NeedMakeFile, s => s.MapFrom(x => x.NeedMakeFile))
                .ReverseMap();
        }
    }
}