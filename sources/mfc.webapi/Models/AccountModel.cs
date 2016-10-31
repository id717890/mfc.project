using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.webapi.Models {
    public class AccountModel {
        public AccountModel() { }

        public AccountModel(mfc.domain.entities.User user)
        {
            Id = user.Id;
            UserName = user.Account;
            Description = user.Name;
            IsAdmin = user.IsAdmin;
            IsController = user.IsController;
            IsExpert = user.IsExpert;
        }
        
        /// <summary>
        /// Идентификатор
        /// </summary>
        [JsonProperty("id")]
        public Int64 Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [JsonProperty("user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// Ф.И.О.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Признак администратора
        /// </summary>
        [JsonProperty("is_admin")]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Признак эксперта
        /// </summary>
        [JsonProperty("is_expert")]
        public bool IsExpert { get; set; }

        /// <summary>
        /// Признак контролера
        /// </summary>
        [JsonProperty("is_controller")]
        public bool IsController { get; set; }

        public mfc.domain.entities.User ConvertToUser()
        {
            return new mfc.domain.entities.User
            {
                Id = this.Id,
                Account = this.UserName,
                Name = this.Description,
                IsAdmin = this.IsAdmin,
                IsExpert = this.IsExpert,
                IsController = this.IsController
            };
        }
    }
}   