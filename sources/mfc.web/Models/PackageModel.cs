using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class PackageModel {
        #region Fields
        private List<Organization> _orgs = new List<Organization>();
        #endregion

        [Required]
        public Int64 Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        [Required()]
        public DateTime Date { get; set; }

        [Display(Name = "ОГВ")]
        [Required()]
        public Int64 OrganizationId { get; set; }

        [Required]
        public Int64 ControllerId { get; set; }

        public String Organization { get; set; }

        public FileModelItem[] Files { get; set; }

        public List<Organization> Organizations { get { return _orgs; } }
    }
}