using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class FIleStatusHistoryModelProfile : Profile
    {
        public FIleStatusHistoryModelProfile()
        {
            CreateMap<FileStatusRegistry, FileStatusHistoryModel>().ReverseMap();
        }
    }
}