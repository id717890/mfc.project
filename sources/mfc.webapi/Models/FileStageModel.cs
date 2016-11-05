using mfc.domain.entities;
using Newtonsoft.Json;

namespace mfc.webapi.Models
{
    public class FileStageModel
    {
        public FileStageModel() { }

        public FileStageModel(FileStage fileStage)
        {
            Code = fileStage.Code;
            Caption = fileStage.Caption;
            Status = new FileStatusModel(fileStage.Status);
            Order = fileStage.Order;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        [JsonProperty("caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        [JsonProperty("status")]
        public FileStatusModel Status { get; set; }

        /// <summary>
        /// Порядок
        /// </summary>
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

