using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace mfc.webapi.Models
{
    using mfc.domain.models;

    [JsonObject("file_info")]
    public class FileInfo
    {
        public FileInfo() { }

        public FileInfo(domain.entities.File file)
        {
            Id = file.Id;
            Date = file.Date;
            Action = new ActionInfo(file.Action);
            Caption = file.Caption;
            Controller = new UserInfo(file.Controller);
            Expert = new UserInfo(file.Expert);
            Status = new FileStatusInfo(file.CurrentStatus);
            Organization = new OrganizationInfo(file.Ogv);
        }

        [Display(Name = "Идентификатор")]
        [JsonProperty("id")]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [Required]
        [Display(Name = "Дата")]
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Прием")]
        [JsonProperty("action")]
        public ActionInfo Action { get; set; }

        [Required]
        [Display(Name = "Контролер")]
        [JsonProperty("controller")]
        public UserInfo Controller { get; set; }

        [Required]
        [Display(Name = "Эксперт")]
        [JsonProperty("expert")]
        public UserInfo Expert { get; set; }

        [Required]
        [Display(Name = "Статус")]
        [JsonProperty("status")]
        public FileStatusInfo Status { get; set; }

        [Required]
        [Display(Name = "Организация")]
        [JsonProperty("organization")]
        public OrganizationInfo Organization { get; set; }

        public domain.entities.File ConvertToFile()
        {
            return new domain.entities.File
            {
                Id = this.Id,
                Caption = this.Caption,
                Date = this.Date,
                Action = this.Action.ConvertToAction(),
                Controller = this.Controller.ConvertToUser(),
                Expert = this.Expert.ConvertToUser(),
                CurrentStatus = this.Status.ConvertToFileStatus(),
                Ogv = this.Organization.ConvertToOrganization()
            };
        }
    }
}