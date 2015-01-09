using mfc.domain.entities;
using mfc.domain.services;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mfc.web.Helpers {
    public static class OrganizationModelConverter {
        public static OrganizationModel ToModel(Organization entity) {
            return new OrganizationModel {
                Id = entity.Id,
                Caption = entity.Caption,
                FullCaption = entity.FullCaption,
                TypeId = entity.Type != null ? entity.Type.Id : -1,
                TypeCaption = entity.Type != null ? entity.Type.Caption : string.Empty
            };
        }

        public static Organization FromModel(OrganizationModel model) {
            var srv = CompositionRoot.Resolve<IOrganizationService>();
            var type = srv.GetTypeById(model.TypeId);

            return new Organization {
                Id = model.Id,
                Caption = model.Caption,
                FullCaption = model.FullCaption,
                Type = type
            };
        }
    }
}