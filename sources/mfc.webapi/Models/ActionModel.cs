using Newtonsoft.Json;
using System;

namespace mfc.webapi.Models
{
    public class ActionModel
    {
        public ActionModel() { }

        public ActionModel(domain.entities.ServiceAction action)
        {
            Id = action.Id;
            Date = action.Date;
            Customer = action.Customer;
            CustomerType = action.CustomerType != null ? new CustomerTypeInfo(action.CustomerType) : new CustomerTypeInfo(domain.entities.CustomerType.Empty);

            Service = action.Service != null ? new ServiceInfo(action.Service) : null;
            ServiceChild = action.ServiceChild != null ? new ServiceInfo(action.ServiceChild) : null;

            ActionType = new ActionTypeInfo(action.Type);
            User = action.User != null ? new AccountModel(action.User) : null;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Заявитель
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// Тип заявителя
        /// </summary>
        [JsonProperty("customer_type")]
        public CustomerTypeInfo CustomerType { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        [JsonProperty("service")]
        public ServiceInfo Service { get; set; }

        /// <summary>
        /// Дочерняя услуга
        /// </summary>
        [JsonProperty("service_child")]
        public ServiceInfo ServiceChild { get; set; }

        /// <summary>
        /// Вид деятельности
        /// </summary>
        [JsonProperty("action_type")]
        public ActionTypeInfo ActionType { get; set; }

        /// <summary>
        /// Эксперт
        /// </summary>
        [JsonProperty("expert")]
        public AccountModel User { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [JsonProperty("comments")]
        public string Comments { get; set; }

        /// <summary>
        /// Иногородний
        /// </summary>
        [JsonProperty("is_non_resident")]
        public bool IsNonresident { get; set; }

        /// <summary>
        /// Бесплатный выезд
        /// </summary>
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
