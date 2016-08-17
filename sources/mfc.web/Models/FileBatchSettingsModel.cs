using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mfc.web.Models {
    public class FileBatchSettingsModel {
        public FileModelItem[] Files { get; set; }

        public FileStatusModel[] Statuses { get; set; }

        [Display(Name = "Статус")]
        public long Status { get; set; }
    }
}