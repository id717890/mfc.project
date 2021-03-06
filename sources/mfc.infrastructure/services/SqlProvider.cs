﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace mfc.infrastructure.services {
    public class SqlProvider : ISqlProvider {
        #region fields
        
        private bool _is_init = false;
        private string _connection_string;
        /*private SqlConnection _connection;*/

        #endregion

        public SqlConnection CreateConnection() {
            ThrowIfNotConfigured();

            var connection = new SqlConnection(_connection_string);
            connection.Open();

            return connection;
        }

        public void Init(string connectionString) {
            _connection_string = connectionString;
            _is_init = true;
        }

        private void ThrowIfNotConfigured() {
            if (!_is_init) {
                throw new Exception("Работа с Sql-сервером не настроена");
            }
        }
    }
}
