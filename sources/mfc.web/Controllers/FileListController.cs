using mfc.domain;
using mfc.domain.entities;
using mfc.domain.services;
using mfc.web.Abstracts;
using mfc.web.Helpers;
using mfc.web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mfc.web.Controllers {
    public class FileListController : BaseController {
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ControlList")]
        public ActionResult ControlList(FormCollection collection) {
            var file_srv = CompositionRoot.Resolve<IFileService>();
            List<Int64> checked_file_ids = CreateFilesList(collection);
            
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
        public ActionResult CreatePackage(FormCollection collection)
        {
            List<Int64> checked_file_ids = CreateFilesList(collection);

            if (checked_file_ids.Count == 0)
            {
                return RedirectToAction("Index", "File");
            }

            return View(PackageHelper.CreateModel(checked_file_ids));
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "BatchSettings")]
        public ActionResult BatchSettings(FormCollection collection) {
            List<long> checked_file_ids = CreateFilesList(collection);

            if (checked_file_ids.Count == 0) {
                return RedirectToAction("Index", "File");
            }

            return View(new FileBatchSettingsModel { Status = Convert.ToInt64(collection["status"]), Files = checked_file_ids });
        }


        [HttpPost]
        public ActionResult SavePackage(PackageModel model) {
            if (ModelState.IsValid) {
                var package_srv = CompositionRoot.Resolve<IPackageService>();
                var user_srv = CompositionRoot.Resolve<IUserService>();
                var org_srv = CompositionRoot.Resolve<IOrganizationService>();

                var file_ids = new List<Int64>();

                foreach (var item in model.Files){
                    file_ids.Add(item.Id);
                }

                try {
                    var id = package_srv.CreatePackage(user_srv.GetCurrentUser(), model.Date, org_srv.GetOrganizationById(model.OrganizationId), file_ids, model.Comment);

                    return RedirectToAction("Edit", "Package", new { id = id });
                }
                catch (DomainException e) {
                    ModelState.AddModelError("", e.Message);
                }
            }

            PackageHelper.PrepareModel(model);

            return View("CreatePackage", model);
        }

        private List<Int64> CreateFilesList(FormCollection collection)
        {
            List<Int64> checked_file_ids = new List<long>();

            //проверим отработал ли код javascript, который добавляет полный список
            //отмеченных файлов
            if (collection["checked-files"] != null)
            {
                JObject json = JObject.Parse(collection["checked-files"]);
                foreach (var prop in json.Properties().Select(p => p.Name).ToList())
                {
                    if (json[prop].Value<bool>())
                    {
                        checked_file_ids.Add(long.Parse(prop.Replace("file", "")));
                    }
                }
            }
            else
            {
                foreach (var file in collection.AllKeys)
                {
                    if (file.StartsWith("file") && collection[file] == "on")
                    {
                        checked_file_ids.Add(long.Parse(file.Replace("file", "")));
                    }
                }
            }

            return checked_file_ids;
        }
    }
}