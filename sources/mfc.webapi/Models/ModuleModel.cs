using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace mfc.webapi.Models
{
    public class ModuleModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [Required]
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [Required]
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("parent")]
        public ModuleModel Parent { get; set; }
    }
}