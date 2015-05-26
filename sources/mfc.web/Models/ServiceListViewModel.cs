using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class ServiceListViewModel {
        #region Fields
        #endregion
        [Display(Name = "ОГВ")]
        public Int64 OrganizationId { get; set; }
        public List<Organization> Organizations { get; set; }
        public List<Service> Services { get; set; }
        public Service Parent { get; set; }
    }
}