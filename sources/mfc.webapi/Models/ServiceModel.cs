using Newtonsoft.Json;

namespace mfc.webapi.Models
{
    [JsonObject("service_info")]
    public class ServiceModel
    {
        public ServiceModel() { }

        public ServiceModel(domain.entities.Service service)
        {
            Id = service.Id;
            Caption = service.Caption;
            Organization = new OrganizationModel(service.Organization);
        }

        /// <summary>
        /// Идентификтор
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [JsonProperty("caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Организация
        /// </summary>
        [JsonProperty("organization")]
        public OrganizationModel Organization { get; set; }

        //        public domain.entities.Service ConvertToService()
        //        {
        //            return new domain.entities.Service
        //            {
        //                Id = this.Id,
        //                Caption = this.Caption,
        //                Organization = new domain.entities.Organization { Id = OrganizationId, Caption = Organization}
        //            };
        //        }
    }
}