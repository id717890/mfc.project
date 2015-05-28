using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class FileReturnModel {
        public Int64 FileId { get; set; }

        [Required]
        [Display(Name = "Эксперт")]
        public Int64 ExpertId { get; set; }

        public string Expert { get; set; }

        [Display(Name = "Комментарий")]
        public string Comments { get; set; }
    }
}