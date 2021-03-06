﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class ServiceModel {
        public Int64 Id { get; set; }

        [Required]
        [Display(Name = "Наименование")]
        [StringLength(1024, ErrorMessage = "{0} должно быть длиной не более {1} символов.")]
        public string Caption { get; set; }

        [Required]
        public Int64 OrgId { get; set; }

        [Display(Name = "ОГВ")]
        public String OrgCaption { get; set; }

        [Display(Name = "Услуга")]
        public string ParentCaption { get; set; }

        [Display(Name = "Услуга")]
        public Int64 ParentId { get; set; }
    }
}