using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace mfc.infrastructure.services {
    public class IdentifierService : IIdentifierService {
        [Inject]
        public ISqlProvider SqlProvider { get; set; }
        
        public long GetId() {
            Int64 id = 0;

            var cmd = SqlProvider.CreateConnection().CreateCommand();
            try{
                cmd.CommandText = @"select next value for mfc_seq";
                id = Convert.ToInt64(cmd.ExecuteScalar());
                
            }
            finally {
                cmd.Dispose();
            }

            return id;
        }
    }
}
