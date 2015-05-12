using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class AccountInfo {
        public Int64 Id { get; set; }

        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Ф.И.О.")]
        public string Description { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsExpert { get; set; }
        public bool IsController { get; set; }
    }
}   