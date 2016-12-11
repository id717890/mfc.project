using Newtonsoft.Json;

namespace mfc.webapi.Models
{
    [JsonObject("organization_info")]
    public class OrganizationModel
    {
        public OrganizationModel() { }

        public OrganizationModel(domain.entities.Organization organization)
        {
            Id = organization.Id;
            Caption = organization.Caption;
            FullCaption = organization.FullCaption;
            OrganizationType = new OrganizationTypeModel(organization.Type);
        }

        /// <summary>
        /// Идентификтор
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Краткое название
        /// </summary>
        [JsonProperty("caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Полное название
        /// </summary>
        [JsonProperty("full_caption")]
        public string FullCaption { get; set; }

        /// <summary>
        /// Тип организации
        /// </summary>
        [JsonProperty("organization_type")]
        public OrganizationTypeModel OrganizationType { get; set; }

        //        public domain.entities.Organization ConvertToOrganization()
        //        {
        //            return new domain.entities.Organization
        //            {
        //                Id = this.Id,
        //                Caption = this.Caption,
        //                Type = this.OrganizationType.ConvertToOrganizationType()
        //            };
        //        }
    }
}