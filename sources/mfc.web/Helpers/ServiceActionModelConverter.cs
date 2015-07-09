using mfc.domain.entities;
using mfc.domain.services;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mfc.web.Helpers {
    public static class ServiceActionModelConverter {
        #region Fields
        private static IUserService _usr_service = CompositionRoot.Resolve<IUserService>();
        private static IActionTypeService _action_type_srv = CompositionRoot.Resolve<IActionTypeService>();
        private static IServiceService _service_srv = CompositionRoot.Resolve<IServiceService>();
        #endregion

        public static ServiceAction FromModel(ServiceActionViewModel model) {
            return new ServiceAction {
                Id = model.Id,
                User = _usr_service.GetUserById(model.ExpertId),
                Customer = model.Customer,
                Service = _service_srv.GetServiceById(model.ServiceId),
                Date = model.Date,
                Type = _action_type_srv.GetTypeById(model.TypeId),
                Comments = model.Comments,
                ServiceChild = _service_srv.GetServiceById(model.ServiceChildId),
                IsNonresident = model.IsNonresident,
                FreeVisit = model.FreeVisit
            };
        }

        public static ServiceActionViewModel ToModel(ServiceAction entity) {
            var item = new ServiceActionViewModel {
                Id = entity.Id,
                ExpertId = entity.User.Id,
                Expert = entity.User.Name,
                TypeId = entity.Type.Id,
                Type = entity.Type.Caption,
                Date = entity.Date,
                Customer = entity.Customer,
                Comments = entity.Comments,
                IsNonresident = entity.IsNonresident,
                FreeVisit = entity.FreeVisit
            };

            if (entity.Service != null) {
                item.ServiceId = entity.Service.Id;
                item.Service = entity.Service.Caption;
                item.OrganizationId = entity.Service.Organization.Id;
                item.Organization = entity.Service.Organization.Caption;
            }

            if (entity.ServiceChild != null) {
                item.ServiceChildId = entity.ServiceChild.Id;
                item.ServiceChild = entity.ServiceChild.Caption;
            }

            return item;
        }
    }
}