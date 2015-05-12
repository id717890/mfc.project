using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    /// <summary>
    /// Дело
    /// </summary>
    public class File {
        /// <summary>
        /// Уникальный идентификато
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// Дата создания дела - дата-время приема документов
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Ссылка на действие по приему документов
        /// </summary>
        public Int64 ActionId { get; set; }
        /// <summary>
        /// Номер дела
        /// </summary>
        public string Caption { get; set; }
        /// <summary>
        /// Идентификатор контроллера по делу
        /// </summary>
        public User Controller { get; set; }
        /// <summary>
        /// Идентификатор эксперта, принявшего документы
        /// </summary>
        public User Excpert { get; set; }
        /// <summary>
        /// Действиющий (последний) статус дела
        /// </summary>
        public FileStatus CurrentStatus { get; set; }
        /// <summary>
        /// Идентификатор ОГВ, куда должно быть направлено дело
        /// </summary>
        public Organization Ogv { get; set; }
        /// <summary>
        /// Услуга
        /// </summary>
        public Service Service { get; set; }
    }
}
