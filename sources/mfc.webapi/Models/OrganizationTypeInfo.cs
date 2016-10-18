using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace mfc.webapi.Models
{
    [JsonObject("organization_type_model")]
    public class OrganizationTypeInfo
    {
        public OrganizationTypeInfo() { }

        public OrganizationTypeInfo(domain.entities.OrganizationType organizationType)
        {
            Id = organizationType.Id;
            Caption = organizationType.Caption;
        }

        [JsonProperty("id")]
        public Int64 Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        [JsonProperty("caption")]
        public string Caption { get; set; }

        public domain.entities.OrganizationType ConvertToOrganizationType()
        {
            return new domain.entities.OrganizationType
            {
                Id = this.Id,
                Caption = this.Caption
            };
        }
    }
}