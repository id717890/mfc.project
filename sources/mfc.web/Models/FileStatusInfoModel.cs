using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class FileStatusInfoModel {
        public string Date { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
    }
}