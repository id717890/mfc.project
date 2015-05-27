using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class FileStageModel {
        public string Code { get; set; }
        public string Caption { get; set; }

        [Required]
        public Int64 StatusId { get; set; }
    }
}