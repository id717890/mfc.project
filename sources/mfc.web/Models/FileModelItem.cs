using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class FileModelItem {
        public Int64 Id { get; set; }
        public DateTime Date { get; set; }
        public String Caption { get; set; }
        public string Service { get; set; }
        public string Organization { get; set; }
        public string Expert { get; set; }
        public string Controller { get; set; }
        public string Status { get; set; }
        public bool IsChecked { get; set; }
        public Int64 ActionId { get; set; }
    }
}