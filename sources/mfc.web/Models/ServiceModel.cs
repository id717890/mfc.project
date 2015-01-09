using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class ServiceModel {
        public Int64 Id { get; set; }

        [Required]
        [Display(Name = "Наименование")]
        [StringLength(250, ErrorMessage = "{0} должно быть длиной не более {1} символов.")]
        public string Caption { get; set; }

        [Required]
        public Int64 OrgId { get; set; }

        [Display(Name = "ОГВ")]
        public String OrgCaption { get; set; }
    }
}