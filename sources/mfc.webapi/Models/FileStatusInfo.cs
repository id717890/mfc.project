using System;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace mfc.webapi.Models
{
    [JsonObject("file_status_info")]
    public class FileStatusInfo
    {
        public FileStatusInfo() { }

        public FileStatusInfo(domain.entities.FileStatus fileStatus)
        {
            Id = fileStatus.Id;
            Caption = fileStatus.Caption;
        }

        [Display(Name = "Идентификатор")]
        [JsonProperty("id")]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        [JsonProperty("caption")]
        public string Caption { get; set; }
        
        public domain.entities.FileStatus ConvertToFileStatus()
        {
            return new domain.entities.FileStatus
            {
                Id = this.Id,
                Caption = this.Caption
            };
        }
    }
}

