using mfc.domain.entities;
using mfc.domain.services;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mfc.web.Helpers
{
    public static class ServiceActionModelConverter
    {
        #region Fields
        private static IUserService UserService = CompositionRoot.Resolve<IUserService>();
        private static IActionTypeService ActionTypeService = CompositionRoot.Resolve<IActionTypeService>();
        private static IServiceService ServiceService = CompositionRoot.Resolve<IServiceService>();
        private static ICustomerTypeService CustomerService = CompositionRoot.Resolve<ICustomerTypeService>();
        #endregion

        public static void FromModel(ServiceActionViewModel model, ServiceAction action)
        {
            /*action.Id = model.Id;*/
            action.User = UserService.GetUserById(model.ExpertId);
            action.Customer = model.Customer;
            action.Service = ServiceService.GetServiceById(model.ServiceId);
            action.Date = model.Date;
            action.Type = ActionTypeService.GetTypeById(model.TypeId);
            action.Comments = model.Comments;
            action.ServiceChild = ServiceService.GetServiceById(model.ServiceChildId);
            action.IsNonresident = model.IsNonresident;
            action.FreeVisit = model.FreeVisit;
            action.CustomerType = model.CustomerTypeId == CustomerType.Empty.Id
                ? null
                : CustomerService.GetTypeById(model.CustomerTypeId);
        }

        public static ServiceActionViewModel ToModel(ServiceAction entity)
        {
            var item = new ServiceActionViewModel
            {
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

            if (entity.Service != null)
            {
                item.ServiceId = entity.Service.Id;
                item.Service = entity.Service.Caption;
                item.OrganizationId = entity.Service.Organization.Id;
                item.Organization = entity.Service.Organization.Caption;
            }

            if (entity.ServiceChild != null)
            {
                item.ServiceChildId = entity.ServiceChild.Id;
                item.ServiceChild = entity.ServiceChild.Caption;
            }

            if (entity.CustomerType != null)
            {
                item.CustomerTypeId = entity.CustomerType.Id;
                item.CustomerType = entity.CustomerType.Caption;
            }

            return item;
        }
    }
}