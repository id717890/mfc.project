using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace mfc.webapi.Models
{
    /// <summary>
    /// Модель для изменения статуса дела
    /// </summary>
    public class ChangeStatusModel
    {
        /// <summary>
        /// Новый статус
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Коментарий пользователя
        /// </summary>
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}