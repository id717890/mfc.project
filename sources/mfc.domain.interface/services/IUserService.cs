using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.services {
    public interface IUserService {
        bool IsUserExists(string account);
        User GetUser(string account);
        User GetUserById(Int64 id);
        void Update(User user);
        void Delete(Int64 userId);
        IEnumerable<User> GetAllUsers();
        void AddNew(string account, string name, bool is_admin);
        void SetPassword(Int64 userId, string password);
    }
}
