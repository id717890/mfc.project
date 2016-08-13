using mfc.domain.entities;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mfc.domain.models;

namespace mfc.web.Helpers {
    public static class FileModelConverter {
        public static FileModel ToModel(File file) {
            return new FileModel {
                Id = file.Id,
                ActionId = file.Action.Id,
                Caption = file.Caption,
                Expert = file.Expert.Name,
                Organization = file.Ogv.Caption,
                Service = file.Action.Service.Caption,
                Date = file.Date,
                Controller = file.Controller != null ? file.Controller.Name : string.Empty,
                Status = file.CurrentStatus != null ? file.CurrentStatus.Caption : string.Empty
            };
        }

        public static FileModelItem ToModelItem(File file) {
            return new FileModelItem {
                Id = file.Id,
                Date = file.Date,
                Caption = file.Caption,
                Service = file.Action.Service.Caption,
                Expert = file.Expert.Name,
                Controller = file.Controller != null ? file.Controller.Name : string.Empty,
                Organization = file.Ogv.Caption,
                Status = file.CurrentStatus != null ? file.CurrentStatus.Caption : string.Empty,
                ActionId = file.Action.Id
            };
        }

        public static FileModelItem RecordToModelItem(FileRecord file) {
            return new FileModelItem {
                Id = file.Id,
                Date = file.Date,
                Caption = file.Caption,
                Service = file.Service,
                Expert = file.Expert,
                Controller = file.Controller,
                Organization = file.Organization,
                Status = file.Status,
                ActionId = file.ActionId
            };
        }
    }
}