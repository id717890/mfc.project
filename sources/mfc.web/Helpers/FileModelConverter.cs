using mfc.domain.entities;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mfc.web.Helpers {
    public static class FileModelConverter {
        public static FileModel ToModel(File file) {
            return new FileModel {
                Id = file.Id,
                ActionId = file.Action.Id,
                Caption = file.Caption,
                Expert = file.Expert.Name,
                ExpertId = file.Expert.Id,
                Organization = file.Ogv.Caption,
                OrganizationId = file.Ogv.Id,
                Service = file.Action.Service.Caption,
                Date = file.Date,
                ControllerId = file.Controller != null ? file.Controller.Id : 0
            };
        }
    }
}