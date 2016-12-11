using System;
using System.Collections.Generic;
using System.Security.Principal;
using mfc.domain.entities;
using mfc.domain.services;
using Ninject;
using mfc.dal.services;
using System.Linq;

namespace mfc.infrastructure.services
{
    public class ActionPermissionService : IActionPermissionService
    {
        private readonly IModulePermissionRepository _modulePermissionRepository;
        private readonly IModuleService _moduleService;

        private readonly Dictionary<string, IEnumerable<Module>> _userModulesCache = new Dictionary<string, IEnumerable<Module>>();

        private readonly object sync_obj = new object();

        public ActionPermissionService(IModuleService moduleService, IModulePermissionRepository modulePermissionRepository)
        {
            if (moduleService == null)
            {
                throw new ArgumentNullException(nameof(moduleService));
            }
            if (modulePermissionRepository == null)
            {
                throw new ArgumentNullException(nameof(modulePermissionRepository));
            }

            _moduleService = moduleService;
            _modulePermissionRepository = modulePermissionRepository;

            Logger = new FakeLogger();
        }

        [Inject]
        public ILogger Logger { get; set; }

        public IEnumerable<Module> GetUserModules(IPrincipal user)
        {
            IEnumerable<Module> modules = new List<Module>();

            if (!(user == null || user.Identity == null || string.IsNullOrWhiteSpace(user.Identity.Name)))
            {
                Logger.Debug($"ActionPermissionService.GetUserModules({user.Identity.Name})");
                PrepareCache(user.Identity.Name);
                modules = _userModulesCache[user.Identity.Name];
            }
            else
            {
                Logger.Debug(string.Format("ActionPermissionService.GetUserModules: подан некорретный пользователь {0}", user != null ? (user.Identity != null ? "user.Identity.Name=" + user.Identity.Name : "user.Identity=null") : "user=null"));
            }

            return modules;
        }

        private void PrepareCache(string userName)
        {
            Logger.Debug($"ActionPermissionService.PrepareCache({userName})");
            if (!_userModulesCache.ContainsKey(userName))
            {
                Logger.Debug($"ActionPermissionService.PrepareCache({userName}): кэш не подготовлен");
                lock (sync_obj)
                {
                    if (!_userModulesCache.ContainsKey(userName))
                    {
                        _userModulesCache.Add(userName, CreateModules(userName));
                    }
                }
            }
        }

        //todo: написать тест
        private IEnumerable<Module> CreateModules(string userName)
        {
            var userModules = new Dictionary<string, Module>();
            var allModules = _moduleService.GetAll().ToDictionary(x => x.Code);
            var modulesData = _modulePermissionRepository.GetUserModulesAndOperations(userName);

            //1. Создаем все пользовательские модули с операциями, но без дочерних модулей
            foreach (var moduleData in modulesData)
            {
                if (allModules.ContainsKey(moduleData.Key))
                {
                    var userModule = allModules[moduleData.Key].Clone();
                    var operations = new List<Operation>();

                    foreach (var operation in userModule.Operations)
                    {
                        if (moduleData.Value.Contains(operation.Code))
                        {
                            Logger.Debug($"ActionPermissionService.CreateModules({userName}): модуль {userModule.Code} добавлена операция {operation.Code}");
                            operations.Add(operation);
                        }
                    }

                    userModule.Operations = operations;
                    userModules.Add(userModule.Code, userModule);

                    Logger.Debug($"ActionPermissionService.CreateModules({userName}): добавлен модуль {userModule.Code}");
                }
            }
            //2. Заполняем дочерние модули
            foreach (var module in userModules.Values)
            {
                var modules = new List<Module>();
                foreach (var child in module.Modules)
                {
                    if (modulesData.ContainsKey(child.Code))
                    {
                        modules.Add(child);
                        Logger.Debug($"ActionPermissionService.CreateModules({userName}): модуль {module.Code} добавлен дочерний модуль {child.Code}");
                    }
                }

                module.Modules = modules;
            }

            return userModules.Values;
        }
    }
}
