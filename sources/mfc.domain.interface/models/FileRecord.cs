using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.models {
    /// <summary>
    /// Структура данных с информацией о деле. Используеся в списочных запросах
    /// </summary>
    public class FileRecord {
        public Int64 Id { get; set; }
        public Int64 ActionId { get; set; }
        public string Caption { get; set; }
        public String Expert { get; set; }
        public string Controller { get; set; }
        public string Service { get; set; }
        public string Organization { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}
