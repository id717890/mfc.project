using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class FileStageModelProfile : Profile
    {
        public FileStageModelProfile()
        {
            CreateMap<FileStage, FileStageModel>().ReverseMap();
        }

    }
}