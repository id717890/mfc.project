using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.webapi.Models {
    [JsonObject("account_info")]
    public class AccountInfo {
        public AccountInfo() { }

        public AccountInfo(mfc.domain.entities.User user)
        {
            Id = user.Id;
            UserName = user.Account;
            Description = user.Name;
            IsAdmin = user.IsAdmin;
            IsController = user.IsController;
            IsExpert = user.IsExpert;
        }
        
        [JsonProperty("id")]
        public Int64 Id { get; set; }

        [Required]
        [Display(Name = "Имя пользователя")]
        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Ф.И.О.")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("is_admin")]
        public bool IsAdmin { get; set; }
        [JsonProperty("is_expert")]
        public bool IsExpert { get; set; }
        [JsonProperty("is_controller")]
        public bool IsController { get; set; }

        public mfc.domain.entities.User ConvertToUser(AccountInfo account)
        {
            return new mfc.domain.entities.User
            {
                Id = account.Id,
                Account = account.UserName,
                Name = account.Description,
                IsAdmin = account.IsAdmin,
                IsExpert = account.IsExpert,
                IsController = account.IsController
            };
        }
    }
}   