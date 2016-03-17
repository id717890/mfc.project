using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class FileReturnModel {
        public Int64 FileId { get; set; }

        [Display(Name = "Комментарий")]
        public string Comments { get; set; }
    }
}