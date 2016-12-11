using Newtonsoft.Json;

namespace mfc.webapi.Models
{
    [JsonObject("action_type_info")]
    public class ActionTypeModel
    {
        public ActionTypeModel() { }

        public ActionTypeModel(domain.entities.ActionType fileStatus)
        {
            Id = fileStatus.Id;
            Caption = fileStatus.Caption;
            NeedMakeFile = fileStatus.NeedMakeFile;
        }

        /// <summary>
        /// Идентификтор
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Краткое наименование
        /// </summary>
        [JsonProperty("caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Создавать дела
        /// </summary>
        [JsonProperty("need_make_file")]
        public bool NeedMakeFile { get; set; }

        //        public domain.entities.ActionType ConvertToFileStatus()
        //        {
        //            return new domain.entities.ActionType
        //            {
        //                Id = this.Id,
        //                Caption = this.Caption,
        //                NeedMakeFile = this.NeedMakeFile
        //            };
        //        }
    }
}