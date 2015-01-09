using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using mfc.domain.entities;
using Ninject;
using mfc.infrastructure.services;

namespace mfc.domain.services {
    public class UserService : IUserService {
        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public IIdentifierService IdService { get; set; }

        public User GetUser(string account) {
            User user = null;

            var cmd = SqlProvider.CreateConnection().CreateCommand();
            SqlDataReader reader = null;

            try {
                cmd.CommandText = @"
                        select id, account, name, is_admin from Users 
                        where account = @account
                            and is_deleted = 0";
                cmd.Parameters.Add(new SqlParameter("account", account));

                reader = cmd.ExecuteReader();

                if (reader.Read()) {
                    user = CreateUser(reader);
                }
                
            }
            finally {
                if (reader != null) {
                    reader.Close();
                }
                cmd.Dispose();
            }

            return user;
        }


        public IEnumerable<User> GetAllUsers() {
            List<User> users = new List<User>();

            var cmd = SqlProvider.CreateConnection().CreateCommand();
            SqlDataReader reader = null;

            try {
                cmd.CommandText = @"
                        select id, account, name, is_admin from Users 
                        where is_deleted = 0
                        order by account";
                reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    users.Add(CreateUser(reader));
                }
                
            }
            finally {
                if (reader != null) {
                    reader.Close();
                }
                cmd.Dispose();
            }

            return users;
        }

        public void AddNew(string account, string name, bool is_admin) {
            var cmd = SqlProvider.CreateConnection().CreateCommand();
            SqlDataReader reader = null;

            try {
                cmd.CommandText = @"insert into Users (id, account, name, is_admin) values (@id, @account, @name, @is_admin)";
                cmd.Parameters.Add(new SqlParameter("id", IdService.GetId()));
                cmd.Parameters.Add(new SqlParameter("account", account));
                cmd.Parameters.Add(new SqlParameter("name", name));
                cmd.Parameters.Add(new SqlParameter("is_admin", is_admin));

                int row_count = cmd.ExecuteNonQuery();

                if (row_count == 0) {
                    throw new DomainException("Подсистеме работы с данными не удалось добавить строку в таблицу Users");
                }
            }
            finally {
                if (reader != null) {
                    reader.Close();
                }
                cmd.Dispose();
            }
        }

        public bool IsUserExists(string account) {
            bool is_user_exist = false;

            var cmd = SqlProvider.CreateConnection().CreateCommand();
            SqlDataReader reader = null;

            try {
                cmd.CommandText = @"
                        select id, account, name, is_admin from Users 
                        where account = @account";
                cmd.Parameters.Add(new SqlParameter("account", account));

                reader = cmd.ExecuteReader();

                if (reader.Read()) {
                    is_user_exist = true;
                }
            }
            finally {
                if (reader != null) {
                    reader.Close();
                }
                cmd.Dispose();
            }

            return is_user_exist;
        }

        private User CreateUser(SqlDataReader reader) {
            return new User {
                Id = Convert.ToInt64(reader["id"]),
                Account = Convert.ToString(reader["account"]),
                Name = reader["name"] == DBNull.Value ? string.Empty : Convert.ToString(reader["name"]),
                IsAdmin = Convert.ToBoolean(reader["is_admin"])
            };
        }


        public User GetUserById(long id) {
            User user = null;
            
            var cmd = SqlProvider.CreateConnection().CreateCommand();
            SqlDataReader reader = null;

            try {
                cmd.CommandText = @"
                        select id, account, name, is_admin from Users 
                        where id = @id";
                cmd.Parameters.Add(new SqlParameter("id", id));

                reader = cmd.ExecuteReader();

                if (reader.Read()) {
                    user = CreateUser(reader);
                }
            }
            finally {
                if (reader != null) {
                    reader.Close();
                }
                cmd.Dispose();
            }

            return user;
        }


        public void Update(User user) {
            var cmd = SqlProvider.CreateConnection().CreateCommand();

            try {
                cmd.CommandText = @"
                        update Users set account = @account, name = @name, is_admin = @is_admin
                        where id = @id";
                cmd.Parameters.Add(new SqlParameter("id", user.Id));
                cmd.Parameters.Add(new SqlParameter("account", user.Account));
                cmd.Parameters.Add(new SqlParameter("name", user.Name));
                cmd.Parameters.Add(new SqlParameter("is_admin", user.IsAdmin));

                int row_count = cmd.ExecuteNonQuery();

                if (row_count == 0) {
                    throw new DomainException("Подсистеме работы с данными не удалось обновить информацию в таблице Users");
                }
            }
            finally {
                cmd.Dispose();
            }
        }

        public void Delete(Int64 userId) {
            var cmd = SqlProvider.CreateConnection().CreateCommand();
            try {
                cmd.CommandText = @"
                        update Users set is_deleted = 1
                        where id = @id";
                cmd.Parameters.Add(new SqlParameter("id", userId));

                int row_count = cmd.ExecuteNonQuery();

                if (row_count == 0) {
                    throw new DomainException("Подсистеме работы с данными не удалось обновить информацию в таблице Users");
                }
            }
            finally {
                cmd.Dispose();
            }
        }


        public void SetPassword(long userId, string password) {
            var cmd = SqlProvider.CreateConnection().CreateCommand();
            try {
                cmd.CommandText = @"
                        update Users set password = @password
                        where id = @id";
                cmd.Parameters.Add(new SqlParameter("id", userId));
                cmd.Parameters.Add(new SqlParameter("password", password));

                int row_count = cmd.ExecuteNonQuery();

                if (row_count == 0) {
                    throw new DomainException("Подсистеме работы с данными не удалось обновить информацию в таблице Users");
                }
            }
            finally {
                cmd.Dispose();
            }
        }
    }
}
