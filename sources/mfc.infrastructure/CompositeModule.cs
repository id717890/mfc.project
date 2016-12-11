using mfc.infrastructure.report;
using mfc.infrastructure.services;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.infrastructure {
    public class CompositeModule : NinjectModule {
        public override void Load() {
            Kernel.Bind<ILogger>().ToConstant(new Logger(NLog.LogManager.GetCurrentClassLogger()));
            Kernel.Bind<ISqlProvider>().To<SqlProvider>().InSingletonScope();
            Kernel.Bind<IIdentifierService>().To<IdentifierService>().InSingletonScope();
            Kernel.Bind<IMembershipService>().To<MembershipService>().InSingletonScope();
            Kernel.Bind<IActionPermissionService>().To<ActionPermissionService>().InSingletonScope();
            Kernel.Bind<IReportService>().To<ReportService>();
            Kernel.Bind<IReportSumModel>().To<ReportSumModel>();
        }
    }
}
