using Newtonsoft.Json;
using System;

namespace mfc.webapi.Models
{
    [JsonObject("organization_type_model")]
    public class OrganizationTypeModel
    {
        public OrganizationTypeModel() { }

        public OrganizationTypeModel(domain.entities.OrganizationType organizationType)
        {
            Id = organizationType.Id;
            Caption = organizationType.Caption;
        }

        /// <summary>
        /// Идентификтор
        /// </summary>
        [JsonProperty("id")]
        public Int64 Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [JsonProperty("caption")]
        public string Caption { get; set; }

        //        public domain.entities.OrganizationType ConvertToOrganizationType()
        //        {
        //            return new domain.entities.OrganizationType
        //            {
        //                Id = this.Id,
        //                Caption = this.Caption
        //            };
        //        }
    }
}