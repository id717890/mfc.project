using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class OrganizationModel {
        public Int64 Id { get; set; }

        [Required]
        [Display(Name = "ОГВ")]
        [StringLength(50, ErrorMessage = "{0} должно быть длиной не более {1} символов.")]
        public string Caption { get; set; }

        [Required]
        [Display(Name = "Полное наименование")]
        [StringLength(512, ErrorMessage = "{0} должно быть длиной не более {1} символов.")]
        public string FullCaption { get; set; }

        [Required]
        public Int64 TypeId { get; set; }

        [Display(Name = "Тип")]
        public String TypeCaption { get; set; }
    }
}