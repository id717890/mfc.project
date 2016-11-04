using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class OrganizationModelProfile : Profile
    {
        public OrganizationModelProfile()
        {
            CreateMapEntityToModel();
            CreateMapModelToEntity();            
        }

        private void CreateMapEntityToModel()
        {
            CreateMap<Organization, OrganizationInfo>()
                .ForMember(x => x.OrganizationType, s => s.MapFrom(x => x.Type));
        }

        private void CreateMapModelToEntity()
        {
            CreateMap<OrganizationInfo, Organization>()
                .ForMember(x => x.Type, s => s.MapFrom(x => x.OrganizationType));
        }


    }
}