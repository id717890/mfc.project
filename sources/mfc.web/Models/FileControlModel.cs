using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class FileControlModel {
        public Int64 FileId { get; set; }

        [Required]
        [Display(Name = "Контролер")]
        public Int64 ControllerId { get; set; }

        public string Controller { get; set; }

        [Display(Name = "Комментарий")]
        public string Comments { get; set; }
    }
}