using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class AcceptionModelProfile: Profile
    {
        public AcceptionModelProfile()
        {
            CreateMapEntityToModel();
            CreateMapModelToEntity();
        }

        private void CreateMapEntityToModel()
        {
            CreateMap<ServiceAction, AcceptionModel>()
                .ForMember(x => x.ActionType, s => s.MapFrom(x => x.Type));
        }

        private void CreateMapModelToEntity()
        {
            CreateMap<AcceptionModel, ServiceAction>()
               .ForMember(x => x.Type, s => s.MapFrom(x => x.ActionType));
        }

    }
}