using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class OrganizationTypeModelProfile : Profile
    {
        public OrganizationTypeModelProfile()
        {
            CreateMap<OrganizationType, OrganizationTypeModel>().ReverseMap();
        }
    }
}