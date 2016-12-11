using mfc.dal.services;
using mfc.domain.entities;
using mfc.infrastructure.services;
using mfc.webapi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mfc.domain.services
{
    public class ModuleService : IModuleService
    {
        private readonly object sync_obj = new object();

        private Dictionary<string, Module> _cacheByCode = new Dictionary<string, Module>();
        private List<Module> _root = new List<Module>();

        private bool _is_cache_valid = false;

        private readonly IModulesProvider _provider;
        private readonly IModulePermissionRepository _modulePermissionRepository;

        public ModuleService(IModulesProvider provider, IModulePermissionRepository modulePermissionRepository)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            if (modulePermissionRepository == null)
            {
                throw new ArgumentNullException(nameof(modulePermissionRepository));
            }

            _provider = provider;
            _modulePermissionRepository = modulePermissionRepository;
        }

        public IEnumerable<Module> GetRoot()
        {
            PrepareCache();
            return _root;
        }

        public IEnumerable<Module> GetAll()
        {
            PrepareCache();
            return _cacheByCode.Values;
        }

        public Module GetByCode(string code)
        {
            PrepareCache();

            if (_cacheByCode.ContainsKey(code))
            {
                return _cacheByCode[code];
            }

            return null;
        }

        private void PrepareCache()
        {
            if (_is_cache_valid)
            {
                return;
            }

            lock (sync_obj)
            {
                if (_is_cache_valid)
                {
                    return;
                }
                _cacheByCode.Clear();

                _root = _provider.GetRoot().ToList();
                foreach (var module in _provider.GetAll())
                {
                    if (!_cacheByCode.ContainsKey(module.Code))
                    {
                        _cacheByCode.Add(module.Code, module);
                    }
                }

                _is_cache_valid = true;
            }
        }
    }
}
