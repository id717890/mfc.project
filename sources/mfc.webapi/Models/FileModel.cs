using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace mfc.webapi.Models
{
    using mfc.domain.models;

    public class FileModel
    {
        public FileModel() { }

        public FileModel(domain.entities.File file)
        {
            Id = file.Id;
            Date = file.Date;
            Action = new ActionModel(file.Action);
            Caption = file.Caption;
            Controller = new AccountModel(file.Controller);
            Expert = new AccountModel(file.Expert);
            Status = new FileStatusModel(file.CurrentStatus);
            Organization = new OrganizationInfo(file.Ogv);
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        [JsonProperty("caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Прием
        /// </summary>
        [JsonProperty("action")]
        public ActionModel Action { get; set; }

        /// <summary>
        /// Контролер
        /// </summary>
        [JsonProperty("controller")]
        public AccountModel Controller { get; set; }

        /// <summary>
        /// Эксперт
        /// </summary>
        [JsonProperty("expert")]
        public AccountModel Expert { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        [JsonProperty("status")]
        public FileStatusModel Status { get; set; }

        /// <summary>
        /// Организация
        /// </summary>
        [JsonProperty("organization")]
        public OrganizationInfo Organization { get; set; }

//        public domain.entities.File ConvertToFile()
//        {
//            return new domain.entities.File
//            {
//                Id = this.Id,
//                Caption = this.Caption,
//                Date = this.Date,
//                Action = this.Action.ConvertToAction(),
//                Controller = this.Controller.ConvertToUser(),
//                Expert = this.Expert.ConvertToUser(),
//                CurrentStatus = this.Status.ConvertToFileStatus(),
//                Ogv = this.Organization.ConvertToOrganization()
//            };
//        }
    }
}