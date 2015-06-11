using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class PackageListViewModel {
        #region Fields

        private readonly List<User> _controllers = new List<User>();
        private readonly List<Package> _packages = new List<Package>();
        private readonly List<Organization> _ogvs = new List<Organization>();
        
        #endregion

        [Display(Name = "Контролер")]
        public Int64 SelectedControllerId { get; set; }
        
        [Display(Name = "ОГВ")]
        public Int64 SelectedOgvId { get; set; }
        
        [Display(Name="От")]
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }
        
        [Display(Name = "До")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public IList<User> Controllers {
            get {
                return _controllers;
            }
        }

        public IList<Organization> Organizations {
            get {
                return _ogvs;
            }
        }

        public IList<Package> Packages {
            get {
                return _packages;
            }
        }
    }
}