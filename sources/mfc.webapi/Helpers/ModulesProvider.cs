using mfc.domain.entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace mfc.webapi.Helpers
{
    public class ModulesProvider : IModulesProvider
    {
        private readonly string _config;
        public ModulesProvider(IModulesConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            if (string.IsNullOrWhiteSpace(configuration.Configuration))
            {
                throw new ArgumentException("Пустая конфигурация", nameof(configuration));
            }

            _config = configuration.Configuration;
            
        }
        public IEnumerable<Module> GetRoot()
        {
            return JsonConvert.DeserializeObject<IEnumerable<Module>>(_config);
        }

        public IEnumerable<Module> GetAll()
        {
            var allModules = new List<Module>();

            var modules = JsonConvert.DeserializeObject<IEnumerable<Module>>(_config);

            var stack = new Stack<Module>();
            foreach (var module in modules)
            {
                stack.Push(module);
            }

            while (stack.Count > 0)
            {
                var module = stack.Pop();
                allModules.Add(module);

                foreach (var child in module.Modules)
                {
                    stack.Push(child);
                }
            }

            return allModules;
        }
    }
}