using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class AccountModelProfile : Profile
    {
        public AccountModelProfile()
        {
            CreateMapEntityToModel();
            CreateMapModelToEntity();
        }

        private void CreateMapEntityToModel()
        {
            CreateMap<User, AccountModel>()
                .ForMember(x => x.UserName, s => s.MapFrom(x => x.Account))
                .ForMember(x => x.Description, s => s.MapFrom(x => x.Name));
        }

        private void CreateMapModelToEntity()
        {
            CreateMap<AccountModel, User>()
                .ForMember(x => x.Account, s => s.MapFrom(x => x.UserName))
                .ForMember(x => x.Name, s => s.MapFrom(x => x.Description));
        }
    }
}