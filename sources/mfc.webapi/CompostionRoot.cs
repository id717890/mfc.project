using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mfc.web {
    public class CompositionRoot {
        private static IKernel _ninjectKernel;

        private static void ThrowIfNotInit() {
            if (_ninjectKernel == null) {
                throw new Exception("CompositeRoot не инициализирован");
            }
        }

        public static void Init(IKernel kernel) {
            _ninjectKernel = kernel;
        }

        public static void Wire(NinjectModule module) {
            ThrowIfNotInit();
            _ninjectKernel.Load(module);
        }

        public static T Resolve<T>() {
            ThrowIfNotInit();
            return _ninjectKernel.Get<T>();
        }

        public static object Resolve(Type type) {
            ThrowIfNotInit();
            return _ninjectKernel.Get(type);
        }
    }
}