using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class CustomerTypeModel {
        public Int64 Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Caption { get; set; }
    }
}