using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace mfc.webapi.Models
{

    [JsonObject("service_info")]
    public class ServiceInfo
    {
        public ServiceInfo() { }

        public ServiceInfo(domain.entities.Service service)
        {
            Id = service.Id;
            Caption = service.Caption;
            OrganistaionId = service.Organization.Id;
        }

        [Display(Name = "Идентификатор")]
        [JsonProperty("id")]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [Required]
        [Display(Name = "Организация")]
        [JsonProperty("organization")]
        public Int64 OrganistaionId { get; set; }

        public domain.entities.Service ConvertToService()
        {
            return new domain.entities.Service
            {
                Id = this.Id,
                Caption = this.Caption,
                //Organization = this.OrganistaionId.ConvertToOrganization()
                //todo: convert to organization
            };
        }
    }
}