using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    /// <summary>
    /// Дело
    /// </summary>
    public class File : Entity {
        /// <summary>
        /// Дата создания дела - дата-время приема документов
        /// </summary>
        public virtual DateTime Date { get; set; }
        /// <summary>
        /// Ссылка на действие по приему документов
        /// </summary>
        public virtual ServiceAction Action { get; set; }
        /// <summary>
        /// Номер дела
        /// </summary>
        public virtual string Caption { get; set; }
        /// <summary>
        /// Идентификатор контроллера по делу
        /// </summary>
        public virtual User Controller { get; set; }
        /// <summary>
        /// Идентификатор эксперта, принявшего документы
        /// </summary>
        public virtual User Expert { get; set; }
        /// <summary>
        /// Действиющий (последний) статус дела
        /// </summary>
        public virtual FileStatus CurrentStatus { get; set; }
        /// <summary>
        /// Идентификатор ОГВ, куда должно быть направлено дело
        /// </summary>
        public virtual Organization Ogv { get; set; }
    }
}
