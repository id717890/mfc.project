using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace mfc.webapi.Models
{
    [JsonObject("organization_info")]
    public class OrganizationInfo
    {
        public OrganizationInfo() { }

        public OrganizationInfo(domain.entities.Organization organization)
        {
            Id = organization.Id;
            Caption = organization.Caption;
            FullCaption = organization.FullCaption;
            OrganizationType = new OrganizationTypeInfo(organization.Type);
        }

        [Display(Name = "Идентификатор")]
        [JsonProperty("id")]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Краткое название")]
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [Required]
        [Display(Name = "Полное название")]
        [JsonProperty("full_caption")]
        public string FullCaption { get; set; }

        [Required]
        [Display(Name = "Тип организации")]
        [JsonProperty("organization_type")]
        public OrganizationTypeInfo OrganizationType { get; set; }

        public domain.entities.Organization ConvertToOrganization()
        {
            return new domain.entities.Organization
            {
                Id = this.Id,
                Caption = this.Caption,
                Type = this.OrganizationType.ConvertToOrganizationType()
            };
        }
    }
}