using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class ServiceModelProfile : Profile
    {
        public ServiceModelProfile()
        {
            CreateMapEntityToModel();
            CreateMapModelToEntity();
        }

        private void CreateMapEntityToModel()
        {
            CreateMap<Service, ServiceModel>()
               .ForMember(x => x.Organization, s => s.MapFrom(x => x.Organization));
        }

        private void CreateMapModelToEntity()
        {
            CreateMap<ServiceModel, Service>()
               .ForMember(x => x.Organization, s => s.MapFrom(x => x.Organization));
        }
    }
}