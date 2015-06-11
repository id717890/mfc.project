using mfc.domain.services;
using mfc.web.Abstracts;
using mfc.web.Helpers;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mfc.web.Controllers {
    public class FileListController : BaseController {
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ControlList")]
        public ActionResult ControlList(List<FileModelItem> model) {
            var file_srv = CompositionRoot.Resolve<IFileService>();
            List<Int64> checked_file_ids = new List<long>();

             if (!ModelState.IsValid || model == null) {
                 return RedirectToAction("Index", "File");
             }

             foreach (var item in model) {
                 if (item.IsChecked) {
                     checked_file_ids.Add(item.Id);
                 }
             }

            if (checked_file_ids.Count == 0) {
                return RedirectToAction("Index", "File");
            }

            var accepted_ids = new List<Int64>(file_srv.AcceptForControl(checked_file_ids));

            var accept_model = new AcceptForControlModel();
            foreach (var id in accepted_ids) {
                accept_model.AcceptedFiles.Add(file_srv.GetFileById(id));
            }

            foreach (var id in checked_file_ids) {
                if (!accepted_ids.Contains(id)) {
                    accept_model.RejectedFiles.Add(file_srv.GetFileById(id));
                }
            }

            return View(accept_model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CreatePackage")]
        public ActionResult CreatePackage(List<FileModelItem> model) {
            var file_srv = CompositionRoot.Resolve<IFileService>();
            List<Int64> checked_file_ids = new List<long>();

            if (!ModelState.IsValid || model == null) {
                return RedirectToAction("Index", "File");
            }

            foreach (var item in model) {
                if (item.IsChecked) {
                    checked_file_ids.Add(item.Id);
                }
            }

            if (checked_file_ids.Count == 0) {
                return RedirectToAction("Index", "File");
            }

            var accepted_ids = new List<Int64>(file_srv.AcceptForControl(checked_file_ids));

            var accept_model = new AcceptForControlModel();
            foreach (var id in accepted_ids) {
                accept_model.AcceptedFiles.Add(file_srv.GetFileById(id));
            }

            foreach (var id in checked_file_ids) {
                if (!accepted_ids.Contains(id)) {
                    accept_model.RejectedFiles.Add(file_srv.GetFileById(id));
                }
            }

            return View(accept_model);
        }
    }
}