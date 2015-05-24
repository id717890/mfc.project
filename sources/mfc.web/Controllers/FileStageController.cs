using mfc.dal.services;
using mfc.domain.entities;
using mfc.domain.services;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mfc.web.Controllers {
    public class FileStageController : Controller {
        // GET: FileStage
        public ActionResult Index() {
            var file_stage_srv = CompositionRoot.Resolve<IFileStageService>();
            var file_status_srv = CompositionRoot.Resolve<IFileStatusService>();

            var model = new FileStagesModel();
            foreach (var stage in file_stage_srv.GetAllStages()) {
                model.Stages.Add(new FileStageModel {
                    Code = stage.Code,
                    Caption = stage.Caption,
                    StatusId = stage.Status != null ? stage.Status.Id : -1
                });
            }
            model.Statuses.AddRange(file_status_srv.GetAllStatuses());

            
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FileStagesModel model) {
            var file_status_srv = CompositionRoot.Resolve<IFileStatusService>();
            var file_stage_srv = CompositionRoot.Resolve<IFileStageService>();

            var stages = new List<FileStage>();

            foreach (var stage_model in model.Stages) {
                var stage = file_stage_srv.GetStage(stage_model.Code);
                stage.Status = file_status_srv.GetStatusById(stage_model.StatusId);
                stages.Add(stage);
            }

            file_stage_srv.UpdateStages(stages);


            model.Statuses.AddRange(file_status_srv.GetAllStatuses());
            
            return View(model);
        }
    }
}