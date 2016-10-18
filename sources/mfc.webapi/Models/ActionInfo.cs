using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace mfc.webapi.Models
{
    [JsonObject("action_info")]
    public class ActionInfo
    {
        public ActionInfo() { }

        public ActionInfo(domain.entities.ServiceAction action)
        {
            Id = action.Id;
            Date = action.Date;
            Customer = action.Customer;
            CustomerType = new CustomerTypeInfo(action.CustomerType);

            Service = new ServiceInfo(action.Service);
            ServiceChild = new ServiceInfo(action.ServiceChild);

            ActionType = new ActionTypeInfo(action.Type);
            User = new UserInfo(action.User);
        }

        [Display(Name = "Идентификатор")]
        [JsonProperty("id")]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Дата")]
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Заявитель")]
        [JsonProperty("customer")]
        public string Customer { get; set; }

        [Required]
        [Display(Name = "Тип заявителя")]
        [JsonProperty("customer_type")]
        public CustomerTypeInfo CustomerType { get; set; }

        [Required]
        [Display(Name = "Услуга")]
        [JsonProperty("service")]
        public ServiceInfo Service { get; set; }

        [Required]
        [Display(Name = "Дочерняя услуга")]
        [JsonProperty("service_child")]
        public ServiceInfo ServiceChild { get; set; }

        [Required]
        [Display(Name = "Вид деятельности")]
        [JsonProperty("action_type")]
        public ActionTypeInfo ActionType { get; set; }

        [Required]
        [Display(Name = "Эксперт")]
        [JsonProperty("expert")]
        public UserInfo User { get; set; }

        [Required]
        [Display(Name = "Вид деятельности")]
        [JsonProperty("action_type")]
        public string Comments { get; set; }

        [Required]
        [Display(Name = "Вид деятельности")]
        [JsonProperty("is_non_resident")]
        public bool IsNonresident { get; set; }

        [Required]
        [Display(Name = "Бесплатный выезд")]
        [JsonProperty("is_free_visit")]
        public bool FreeVisit { get; set; }

        public domain.entities.ServiceAction ConvertToAction()
        {
            return new domain.entities.ServiceAction
            {
                Id = this.Id,
                Customer = this.Customer,

            };
        }
    }
}
