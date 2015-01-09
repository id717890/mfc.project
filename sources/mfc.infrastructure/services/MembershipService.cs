using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace mfc.infrastructure.services {
    public class MembershipService : IMembershipService {
        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        public bool IsUserValid(string account, string password) {
            bool is_valid = false;

            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();

            try {
                cmd.CommandText = @"
                    select id, account, name, password from Users where account = @account and password = @password";
                cmd.Parameters.Add(new SqlParameter("account", account.Trim()));
                cmd.Parameters.Add(new SqlParameter("password", password));

                var reader = cmd.ExecuteReader();

                if (reader.Read()) {
                    is_valid = true;
                }

                reader.Close();
            }
            finally {
                cmd.Dispose();
                conn.Close();
            }

            return is_valid;
        }
    }
}
