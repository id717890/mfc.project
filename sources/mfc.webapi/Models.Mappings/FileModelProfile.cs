using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class FileModelProfile : Profile
    {
        public FileModelProfile()
        {
            CreateMapEntityToModel();
            CreateMapModelToEntity();
        }

        private void CreateMapEntityToModel()
        {
            CreateMap<File, FileModel>()
                .ForMember(x => x.Status, s => s.MapFrom(x => x.CurrentStatus))
                .ForMember(x => x.Organization, s => s.MapFrom(x => x.Ogv));
        }

        private void CreateMapModelToEntity()
        {
            CreateMap<FileModel, File>()
                .ForMember(x => x.CurrentStatus, s => s.MapFrom(x => x.Status))
                .ForMember(x => x.Ogv, s => s.MapFrom(x => x.Organization));
        }
    }
}