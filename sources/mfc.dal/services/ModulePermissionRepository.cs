using mfc.infrastructure.services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace mfc.dal.services
{
    public class ModulePermissionRepository : IModulePermissionRepository
    {
        private readonly ISqlProvider _sqlProvider;

        public ModulePermissionRepository(ISqlProvider sqlProvider)
        {
            if (sqlProvider == null)
            {
                throw new ArgumentNullException(nameof(sqlProvider));
            }
            _sqlProvider = sqlProvider;
        }

        public Dictionary<string, List<string>> GetUserModulesAndOperations(string userName)
        {
            string query = @"
select distinct rm.module, rmo.operation
from users u
join UserRoleAsn ura on ura.user_id = user_id
join RoleModules rm on rm.role_id = ura.role_id
left join RoleModuleOperations rmo on rmo.role_id = ura.role_id and rmo.module=rm.module
where Account=@user_name
";

            var modules = new Dictionary<string, List<string>>();

            var session = _sqlProvider.CreateConnection();
            try
            {

                var cmd = session.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add(new SqlParameter("user_name", userName));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    var moduleName = Convert.ToString(reader["module"]);
                    if (!modules.ContainsKey(moduleName))
                    {
                        modules.Add(moduleName, new List<string>());
                    }
                    if (reader["operation"] != DBNull.Value)
                    {
                        modules[moduleName].Add(Convert.ToString(reader["operation"]));
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                session.Close();
            }

            return modules;
        }
    }
}
