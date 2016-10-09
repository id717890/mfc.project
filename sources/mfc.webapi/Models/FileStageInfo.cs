using System.ComponentModel.DataAnnotations;
using mfc.domain.entities;
using Newtonsoft.Json;

namespace mfc.webapi.Models
{
    [JsonObject("file_stage_info")]
    public class FileStageInfo
    {
        public FileStageInfo() { }

        public FileStageInfo(FileStage fileStage)
        {
            Code = fileStage.Code;
            Caption = fileStage.Caption;
            Status = new FileStatusInfo(fileStage.Status);
            Order = fileStage.Order;
        }

        [Display(Name = "Идентификатор")]
        [JsonProperty("code")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Название")]
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [Required]
        [Display(Name = "Статус")]
        [JsonProperty("status")]
        public FileStatusInfo Status { get; set; }

        [Required]
        [Display(Name = "Порядок")]
        [JsonProperty("order")]
        public int Order { get; set; }

        public FileStage ConvertToFileStatus()
        {
            return new FileStage
            {
                Code = this.Code,
                Caption = this.Caption,
                Status = this.Status.ConvertToFileStatus(),
                Order = this.Order
            };
        }
    }
}

