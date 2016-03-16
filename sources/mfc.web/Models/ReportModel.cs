using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        [Required]
        [Display(Name = "Категория заявителя")]
        public Int64 CustomerTypeId { get; set; }

        public Int32 Report { get; set; }

        public SelectList Reports { get; private set; }

        static Dictionary<int, string> ReportDict = new Dictionary<int, string>{
            {1, "Сводный отчет"},
            {2, "Оперативный отчет"}
        };

        public ReportModel() {
            Reports = new SelectList(ReportDict, "Key", "Value");
            Report = 1;
        }
    }
}