using mfc.domain.entities;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace mfc.webapi.Models
{
    [JsonObject("organization_type_model")]
    public class OrganizationTypeModel
    {
        [JsonProperty("id")]
        public Int64 Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        [JsonProperty("caption")]
        public string Caption { get; set; }

        public OrganizationTypeModel()
        {

        }

        public OrganizationTypeModel(OrganizationType organizationType)
        {
            Id = organizationType.Id;
            Caption = organizationType.Caption;
        }
    }
}