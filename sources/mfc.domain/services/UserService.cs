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
using System.Diagnostics;

namespace mfc.domain.services {
    public class UserService : IUserService {
        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public IIdentifierService IdService { get; set; }

        #region Cache
        private bool _is_cached_valid = false;
        private Dictionary<Int64, User> _id_cache = new Dictionary<long, User>();
        private Dictionary<String, User> _name_cache = new Dictionary<string, User>();
        #endregion

        public User GetUser(string account) {
            Debug.Assert(!string.IsNullOrEmpty(account));

            PrepareCache();

            User user = null;

            var key = account.ToLower();

            if (_name_cache.ContainsKey(key)) {
                user = _name_cache[key];
            }

            return user;
        }


        public IEnumerable<User> GetAllUsers() {
            PrepareCache();

            return _name_cache.Values.OrderBy(item=>item.Account.ToLower());
        }

        public void AddNew(string account, string name, bool is_admin) {
            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();
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
                conn.Close();
                _is_cached_valid = false;
            }
        }

        public bool IsUserExists(string account) {
            return GetUser(account) != null;
        }

        public User GetUserById(long id) {
            PrepareCache();

            User user = null;

            if (_id_cache.ContainsKey(id)) {
                user = _id_cache[id];
            }

            return user;
        }


        public void Update(User user) {
            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();

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
                conn.Close();
                _is_cached_valid = false;
            }
        }

        public void Delete(Int64 userId) {
            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();

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
                conn.Close();
                _is_cached_valid = false;
            }
        }


        public void SetPassword(long userId, string password) {
            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();

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
                conn.Close();
            }
        }

        public IEnumerable<User> GetExperts() {
            PrepareCache();

            return _name_cache.Values.Where(m => !m.IsAdmin).OrderBy(m=>m.Name);
        }


        #region Helpers

        private void PrepareCache() {
            if (_is_cached_valid) {
                return;
            }

            _id_cache.Clear();
            _name_cache.Clear();

            foreach (var user in GetAllUsersInternal()) {
                _id_cache.Add(user.Id, user);
                _name_cache.Add(user.Account.ToLower(), user);
            }

            _is_cached_valid = true;
        }

        private IEnumerable<User> GetAllUsersInternal() {
            List<User> users = new List<User>();

            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();
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
                conn.Close();
            }

            return users;
        }

        private User CreateUser(SqlDataReader reader) {
            return new User {
                Id = Convert.ToInt64(reader["id"]),
                Account = Convert.ToString(reader["account"]),
                Name = reader["name"] == DBNull.Value ? string.Empty : Convert.ToString(reader["name"]),
                IsAdmin = Convert.ToBoolean(reader["is_admin"])
            };
        }

        #endregion
    }
}
