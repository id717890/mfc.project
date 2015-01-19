using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class ReportModel {
        [Required]
        [Display(Name="Дата начала")]
        [DataType(DataType.Date)]
        public DateTime DateBegin { get; set; }

        [Required]
        [Display(Name = "Дата окончания")]
        [DataType(DataType.Date)]
        public DateTime DateEnd { get; set; }
    }
}