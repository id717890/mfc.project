using AutoMapper;
using mfc.domain.entities;
using mfc.webapi.Models;
using mfc.webapi.Models.Mappings;
using System;
using System.Linq;
using System.Reflection;

namespace mfc.webapi.App_Start
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
            var profiles = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x != typeof(Profile) && typeof(Profile).IsAssignableFrom(x));
            foreach (Type profile in profiles)
            {
                cfg.AddProfile((Profile)CompositionRoot.Resolve(profile));
            }
        }

        public static IMapper Mapper()
        {
            return _mapper;
        }
    }
}