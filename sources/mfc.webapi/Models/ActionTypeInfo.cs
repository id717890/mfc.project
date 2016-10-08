using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace mfc.webapi.Models
{
    public class ActionTypeInfo
    {
        public ActionTypeInfo() { }

        public ActionTypeInfo(domain.entities.ActionType fileStatus)
        {
            Id = fileStatus.Id;
            Caption = fileStatus.Caption;
            NeedMakeFile = fileStatus.NeedMakeFile;
        }

        [Display(Name = "Идентификатор")]
        [JsonProperty("id")]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [Required]
        [Display(Name = "Создавать дела")]
        [JsonProperty("need_make_file")]
        public bool NeedMakeFile { get; set; }

        public domain.entities.ActionType ConvertToFileStatus()
        {
            return new domain.entities.ActionType
            {
                Id = this.Id,
                Caption = this.Caption,
                NeedMakeFile = this.NeedMakeFile
            };
        }
    }
}