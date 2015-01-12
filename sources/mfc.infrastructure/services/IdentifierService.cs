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

            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();

            try{
                cmd.CommandText = @"insert into globalids (null_value) values (null);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = @"select SCOPE_IDENTITY()";
                id = Convert.ToInt64(cmd.ExecuteScalar());
                
            }
            finally {
                cmd.Dispose();
                conn.Close();
            }

            return id;
        }
    }
}
