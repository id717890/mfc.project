using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities = mfc.domain.entities;

namespace mfc.webapi.Models
{
    [JsonObject("this_customer_type")]
    public class CustomerType
    {
        public CustomerType() { }

        public CustomerType(Entities.CustomerType customerType)
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