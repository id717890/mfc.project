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
            Organistaion = service.Organization.Caption;
        }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("organization")]
        public Int64 OrganistaionId { get; set; }

        [JsonProperty("organization_caption")]
        public string Organistaion { get; set; }

        public domain.entities.Service ConvertToService()
        {
            return new domain.entities.Service
            {
                Id = this.Id,
                Caption = this.Caption,
                Organization = new domain.entities.Organization { Id = OrganistaionId, Caption = Organistaion }
            };
        }
    }
}