using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class UserModelProfile : Profile
    {
        public UserModelProfile()
        {
            CreateMap<User, UserInfo>().ReverseMap();
        }
    }
}