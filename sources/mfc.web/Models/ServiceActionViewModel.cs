using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class ServiceActionViewModel {
        public Int64 Id { get; set; }

        [Display(Name = "ФИО заявителя/наименование ЮЛ")]
        public string Customer { get; set; }

        [Required]
        [Display(Name = "Эксперт")]
        public Int64 ExpertId { get; set; }

        [Display(Name = "Эксперт")]
        public String Expert { get; set; }

        [Required]
        [Display(Name = "Тип услуги")]
        public Int64 TypeId { get; set; }

        [Display(Name = "Тип услуги")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Категория заявителя/посетителя")]
        public Int64 CustomerTypeId { get; set; }

        [Display(Name = "Категория заявителя/посетителя")]
        public string CustomerType { get; set; }

        [Required]
        [Display(Name = "Услуга")]
        public Int64 ServiceId { get; set; }

        [Display(Name = "Услуга")]
        [Required]
        public string Service { get; set; }

        [Display(Name = "Подуслуга")]
        public Int64 ServiceChildId { get; set; }

        [Display(Name = "Подуслуга")]
        public string ServiceChild { get; set; }

        [Display(Name = "ОГВ")]
        public Int64 OrganizationId { get; set; }

        [Display(Name = "ОГВ")]
        public string Organization { get; set; }

        [Display(Name = "Комментарий")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        [Required]
        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Иногородний")]
        public bool IsNonresident { get; set; }

        [Display(Name = "Бесплатный выезд")]
        public bool FreeVisit { get; set; }
    }
}