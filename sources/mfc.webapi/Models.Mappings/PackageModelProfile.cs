using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.Models.Mappings
{
    public class PackageModelProfile: Profile
    {
        public PackageModelProfile()
        {
            CreateMap<Package, PackageModel>().ReverseMap();
        }
    }
}