using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class FileStatusModelProfile : Profile
    {
        public FileStatusModelProfile()
        {
            CreateMap<FileStatus, FileStatusModel>().ReverseMap();
        }
    }
}