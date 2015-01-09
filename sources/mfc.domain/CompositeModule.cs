﻿using mfc.domain.services;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain {
    public class CompositeModule : NinjectModule {
        public override void Load() {
            Kernel.Bind<IUserService>().To<UserService>().InSingletonScope();
            Kernel.Bind<IOrganizationService>().To<OrganizationService>().InSingletonScope();
            Kernel.Bind<IActionTypeService>().To<ActionTypeService>().InSingletonScope();
            Kernel.Bind<IServiceService>().To<ServiceService>().InSingletonScope();
        }
    }
}
