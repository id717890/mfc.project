using mfc.infrastructure.security;
using mfc.infrastructure.services;
using mfc.web.Abstracts;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mfc.web.Controllers {
    public class ReportController : BaseController {
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

            return View("Make", model);
        }

        [HttpPost]
        public ActionResult Make(DateTime dateBegin, DateTime dateEnd, Int32 report) {
            var report_srv = CompositionRoot.Resolve<IReportService>();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition",
                               string.Format("attachment; filename={0}.xls", "rep"));
            if (report == 1) {
                report_srv.MakeReportSum(dateBegin, dateEnd, Response.OutputStream);
            }
            else if (report == 2) {
                report_srv.MakeReportOper(dateBegin, dateEnd, Response.OutputStream);
            }

            Response.End();

            return View(new ReportModel {
                DateBegin = dateBegin, DateEnd = dateEnd
            });
        }
    }
}
