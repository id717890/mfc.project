using mfc.infrastructure.security;
using mfc.infrastructure.services;
using mfc.web.Abstracts;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mfc.domain.services;
using Ninject;

namespace mfc.web.Controllers {
    public class ReportController : BaseController {
        [Inject]
        public ICustomerTypeService CustomerTypeService { get; set; }

        public ActionResult Index() {
            return Make();
        }

        public ActionResult Make() {
            var date = DateTime.Today;
            var dateBegin = new DateTime(date.Year, date.Month, 1);

            ReportModel model = new ReportModel {
                DateBegin = dateBegin,
                DateEnd = dateBegin.AddMonths(1).AddDays(-1)
            };

            PrepareResponse();

            return View("Make", model);
        }

        [HttpPost]
        public ActionResult Make(DateTime dateBegin, DateTime dateEnd, Int32 report, Int64 customerTypeId) {
            var report_srv = CompositionRoot.Resolve<IReportService>();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition",
                               string.Format("attachment; filename={0}.xls", "rep"));
            if (report == 1) {
                report_srv.MakeReportSum(dateBegin, dateEnd, customerTypeId, Response.OutputStream);
            }
            else if (report == 2) {
                report_srv.MakeReportOper(dateBegin, dateEnd, customerTypeId, Response.OutputStream);
            }

            Response.End();

            PrepareResponse();

            return View(new ReportModel {
                DateBegin = dateBegin, DateEnd = dateEnd
            });
        }

        private void PrepareResponse() {
            ViewBag.CustomerTypes = CustomerTypeService.GetAllTypes();
        }
    }
}
