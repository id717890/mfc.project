using mfc.domain.entities;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mfc.web.Helpers {
    public static class ActionTypeModelConverter {
        public static ActionTypeModel ToModel(ActionType type) {
            return new ActionTypeModel {
                Id = type.Id,
                Caption = type.Caption,
                NeedMakeFile = type.NeedMakeFile
            };
        }

        public static ActionType FromModel(ActionTypeModel model) {
            return new ActionType {
                Id = model.Id,
                Caption = model.Caption,
                NeedMakeFile = model.NeedMakeFile
            };
        }
    }
}