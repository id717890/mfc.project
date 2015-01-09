using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain {
    public class DomainException : Exception {
        public DomainException(string message) : base(message) { }
        public DomainException(Exception innerException) : base("При выполнении операции в доменной области возникло необрабатываемое исключение", innerException) { }
    }
}
