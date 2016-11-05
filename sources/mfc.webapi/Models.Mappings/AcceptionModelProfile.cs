using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class AcceptionModelProfile: Profile
    {
        public AcceptionModelProfile()
        {
            CreateMap<ServiceAction, AcceptionModel>()
                .ForMember(x => x.User, s => s.MapFrom(x => x.User))
                .ForMember(x => x.ActionType, s => s.MapFrom(x => x.Type))
                .ForMember(x => x.Comments, s => s.MapFrom(x => x.Comments))
                .ForMember(x => x.Customer, s => s.MapFrom(x => x.Customer))
                .ForMember(x => x.CustomerType, s => s.MapFrom(x => x.CustomerType))
                .ForMember(x => x.Date, s => s.MapFrom(x => x.Date))
                .ForMember(x => x.FreeVisit, s => s.MapFrom(x => x.FreeVisit))
                .ForMember(x => x.Id, s => s.MapFrom(x => x.Id))
                .ForMember(x => x.IsNonresident, s => s.MapFrom(x => x.IsNonresident))
                .ForMember(x => x.Service, s => s.MapFrom(x => x.Service))
                .ForMember(x => x.ServiceChild, s => s.MapFrom(x => x.ServiceChild))
                ;
        }

    }
}