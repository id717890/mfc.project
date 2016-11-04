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
            CreateMap<Service, ServiceInfo>()
               .ForMember(x => x.OrganizationId, s => s.MapFrom(x => x.Organization.Id))
               .ForMember(x => x.Organization, s => s.MapFrom(x => x.Organization.Caption));
        }

        private void CreateMapModelToEntity()
        {
            CreateMap<ServiceInfo, Service>()
               .ForMember(x => x.Organization, s => s.MapFrom(x => new OrganizationInfo() { Id = x.Id, Caption = x.Organization }));
        }
    }
}