using AutoMapper;
using mfc.webapi.Models.Mappings;
using System;
using System.Linq;
using System.Reflection;

namespace mfc.webapi.tests.Automapper.Tests
{
    public static class AutoMapperConfig
    {
        private static MapperConfiguration _config;
        private static IMapper _mapper;
        public static void Configure()
        {
            _config = new MapperConfiguration(cfg =>
            {
                ApplicationAssemblies(cfg);
            });
            _mapper = _config.CreateMapper();
        }

        private static void ApplicationAssemblies(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfile(new OrganizationTypeModelProfile());
            cfg.AddProfile(new UserModelProfile());
            cfg.AddProfile(new OrganizationModelProfile());
            cfg.AddProfile(new ServiceModelProfile());
        }

        public static IMapper Mapper()
        {
            return _mapper;
        }
    }
}
