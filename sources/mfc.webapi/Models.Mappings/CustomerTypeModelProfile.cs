using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class CustomerTypeModelProfile : Profile
    {
        public CustomerTypeModelProfile()
        {
            CreateMap<CustomerType, CustomerTypeInfo>().ReverseMap();
        }
    }
}