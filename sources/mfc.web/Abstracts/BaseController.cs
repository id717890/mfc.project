using mfc.infrastructure.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mfc.web.Abstracts {
    public abstract class BaseController : Controller {
        protected ILogger Logger { get { return CompositionRoot.Resolve<ILogger>(); } }

        protected override void OnException(ExceptionContext filterContext) {
            Logger.Error(filterContext.Exception);
        }
    }
}