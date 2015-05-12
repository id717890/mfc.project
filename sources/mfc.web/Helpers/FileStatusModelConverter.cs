using mfc.domain.entities;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mfc.web.Helpers {
    public static class FileStatusModelConverter {
        public static FileStatusModel ToModel(FileStatus status) {
            return new FileStatusModel {
                Id = status.Id,
                Caption = status.Caption
            };
        }

        public static FileStatus FromModel(FileStatusModel model) {
            return new FileStatus {
                Id = model.Id,
                Caption = model.Caption
            };
        }
    }
}