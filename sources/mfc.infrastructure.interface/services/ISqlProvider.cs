﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.infrastructure.services {
    /// <summary>
    /// Интерфейс описывает функционал для работы с подключение к СУБД Sql-Server
    /// </summary>
    public interface ISqlProvider {
        /// <summary>
        /// Создает новое подключение
        /// </summary>
        /// <returns></returns>
        SqlConnection CreateConnection();
        /// <summary>
        /// Закрывает ранее открытое подключение
        /// </summary>
        void CloseConnection();
        /// <summary>
        /// Инициализирует класс данными, необходимыми для подключения к северу СУДБ
        /// </summary>
        /// <param name="connectionString">Строка подлючения к серверу</param>
        void Init(string connectionString);
    }
}
