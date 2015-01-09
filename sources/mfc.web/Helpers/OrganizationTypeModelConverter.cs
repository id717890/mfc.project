using mfc.domain.entities;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mfc.web.Helpers {
    public static class OrganizationTypeModelConverter {
        public static OrganizationTypeModel ToModel(OrganizationType type) {
            return new OrganizationTypeModel {
                Id = type.Id,
                Caption = type.Caption
            };
        }

        public static OrganizationType FromModel(OrganizationTypeModel model) {
            return new OrganizationType {
                Id = model.Id,
                Caption = model.Caption
            };
        }
    }
}