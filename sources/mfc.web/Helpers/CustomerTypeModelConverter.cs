using mfc.domain.entities;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mfc.web.Helpers {
    public static class CustomerTypeModelConverter {
        public static CustomerTypeModel ToModel(CustomerType type) {
            return new CustomerTypeModel { 
                Id = type.Id,
                Caption = type.Caption
            };
        }

        public static CustomerType FromModel(CustomerTypeModel model) {
            return new CustomerType {
                Id = model.Id,
                Caption = model.Caption
            };
        }
    }
}