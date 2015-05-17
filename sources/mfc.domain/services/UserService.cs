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
using mfc.dal.services;

namespace mfc.domain.services {
    public class UserService : IUserService {
        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public IIdentifierService IdService { get; set; }

        [Inject]
        public IUserRepository Repository { get; set; }

        [Inject]
        public IUnitOfWorkProvider UnitOfWorkProvider { get; set; }

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

        public void AddNew(string account, string name, bool is_admin, bool is_expert, bool is_controller) {
            var user = new User {
                Id = IdService.GetId(),
                Account = account,
                Name = name,
                IsAdmin = is_admin,
                IsController = is_controller,
                IsExpert = is_expert,
                IsDeleted = false
            };

            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            Repository.Create(user);
            unit_of_work.Commit();

            _is_cached_valid = false;
            
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
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            Repository.Update(user);
            unit_of_work.Commit();

            _is_cached_valid = false;
        }

        public void Delete(Int64 userId) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            Repository.Delete(userId);
            unit_of_work.Commit();

            _is_cached_valid = false;
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

            return _name_cache.Values.Where(m => m.IsExpert).OrderBy(m=>m.Name);
        }

        public IEnumerable<User> GetControllers() {
            PrepareCache();

            return _name_cache.Values.Where(m => m.IsController).OrderBy(m => m.Name);
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
            return Repository.GetAll();
        }

        #endregion
    }
}
