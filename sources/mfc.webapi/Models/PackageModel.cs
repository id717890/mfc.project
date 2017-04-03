using System;
using System.Collections.Generic;
using mfc.domain.entities;
using Newtonsoft.Json;

namespace mfc.webapi.Models
{
    public class PackageModel
    {

        public PackageModel() { }

        public PackageModel(Package package)
        {
            Id = package.Id;
            Date = package.Date;
            Organization = new OrganizationModel(package.Organization);
            Controller = new AccountModel(package.Controller);
            Comment = package.Comment;
        }

        /// <summary>
        /// Идентификтор
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Организация
        /// </summary>
        [JsonProperty("organization")]
        public OrganizationModel Organization { get; set; }

        /// <summary>
        /// Контролёр
        /// </summary>
        [JsonProperty("controller")]
        public AccountModel Controller { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [JsonProperty("comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Список дел
        /// </summary>
        [JsonProperty("files")]
        public IEnumerable<FileModel> Files { get; set; } 
    }
}