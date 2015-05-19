using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class FileModel {
        public Int64 Id { get; set; }

        public Int64 ActionId { get; set; }

        [Required]
        [Display(Name = "Номер дела")]
        public string Caption { get; set; }

        [Display(Name = "Эксперт")]
        public String Expert { get; set; }

        public Int64 ExpertId { get; set; }

        [Display(Name = "Контролер")]
        [Required]
        public Int64 ControllerId { get; set; }

        [Display(Name = "Услуга")]
        public string Service { get; set; }

        [Display(Name = "ОГВ")]
        public string Organization { get; set; }

        public Int64 OrganizationId { get; set; }

        [Required]
        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}