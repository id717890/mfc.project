using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace mfc.webapi.Models
{
    [JsonObject("user_info")]
    public class UserInfo
    {
        public UserInfo() { }

        public UserInfo(domain.entities.User user)
        {
            Id = user.Id;
            Account = user.Account;
            Name = user.Name;
            IsAdmin = user.IsAdmin;
            IsExpert = user.IsExpert;
            IsController = user.IsController;
        }

        [Display(Name = "Идентификатор")]
        [JsonProperty("id")]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Аккаунт")]
        [JsonProperty("account")]
        public string Account { get; set; }

        [Required]
        [Display(Name = "ФИО")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Администратор")]
        [JsonProperty("is_admin")]
        public bool IsAdmin { get; set; }

        [Required]
        [Display(Name = "Эксперт")]
        [JsonProperty("is_expert")]
        public bool IsExpert { get; set; }

        [Required]
        [Display(Name = "Котроллер")]
        [JsonProperty("is_controller")]
        public bool IsController { get; set; }

        public domain.entities.User ConvertToUser()
        {
            return new domain.entities.User
            {
                Id = this.Id,
                Account = this.Account,
                Name = this.Name,
                IsAdmin = this.IsAdmin,
                IsExpert = this.IsExpert,
                IsController = this.IsController
            };
        }
    }
}