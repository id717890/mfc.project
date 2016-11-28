using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class ActionModelProfile: Profile
    {
        public ActionModelProfile()
        {
            CreateMapEntityToModel();
            CreateMapModelToEntity();
        }

        private void CreateMapEntityToModel()
        {
            CreateMap<ServiceAction, ActionModel>()
                .ForMember(x => x.ActionType, s => s.MapFrom(x => x.Type));
        }

        private void CreateMapModelToEntity()
        {
            CreateMap<ActionModel, ServiceAction>()
               .ForMember(x => x.Type, s => s.MapFrom(x => x.ActionType));
        }

    }
}