using System;
using mfc.domain.entities;
using Newtonsoft.Json;

namespace mfc.webapi.Models
{
    [JsonObject("file_status_history")]
    public class FileStatusHistoryModel
    {
        public FileStatusHistoryModel() {}

        public FileStatusHistoryModel(FileStatusRegistry fileStatusHistory)
        {
            Date = fileStatusHistory.Date;
            Status = new FileStatusModel(fileStatusHistory.Status);
            User = new AccountModel(fileStatusHistory.User);
            Comments = fileStatusHistory.Comments;
        }

        //            /// <summary>
        //            /// Дело
        //            /// </summary>
        //            [JsonProperty("file")]
        //            public FileModel File { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Статус дела
        /// </summary>
        [JsonProperty("status")]
        public FileStatusModel Status { get; set; }

        /// <summary>
        /// Пользователь выполнивший действие
        /// </summary>
        [JsonProperty("user")]
        public AccountModel User { get; set; }

        /// <summary>
        /// Коментарий пользователя
        /// </summary>
        [JsonProperty("comments")]
        public string Comments { get; set; }
    }
}