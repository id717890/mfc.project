using Newtonsoft.Json;

namespace mfc.webapi.Models
{
    [JsonObject("customer_type")]
    public class CustomerTypeInfo
    {
        public CustomerTypeInfo() { }

        public CustomerTypeInfo(mfc.domain.entities.CustomerType customerType)
        {
            Id = customerType.Id;
            Caption = customerType.Caption;
        }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }
    }
}