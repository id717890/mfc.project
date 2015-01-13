﻿using mfc.domain.entities;
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
                Comments = model.Comments
            };
        }

        public static ServiceActionViewModel ToModel(ServiceAction entity) {
            return new ServiceActionViewModel {
                Id = entity.Id,
                ExpertId = entity.User.Id,
                Expert = entity.User.Name,
                ServiceId = entity.Service.Id,
                Service = entity.Service.Caption,
                TypeId = entity.Type.Id,
                Type = entity.Type.Caption,
                Date = entity.Date,
                Customer = entity.Customer,
                OrganizationId = entity.Service.Organization.Id,
                Organization = entity.Service.Organization.Caption,
                Comments = entity.Comments
            };
        }
    }
}