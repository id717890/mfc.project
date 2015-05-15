﻿using mfc.dal.services;
using mfc.domain.services;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal {
    public class CompositeModule : NinjectModule {
        public override void Load() {
            Kernel.Bind<IOrganizationTypeRepository>().To<OrganizationTypeRepository>();
            Kernel.Bind<IFileRepository>().To<FileRepository>();
        }
    }
}
