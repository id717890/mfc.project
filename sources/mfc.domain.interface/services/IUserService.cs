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
        IEnumerable<User> GetExperts();
        IEnumerable<User> GetControllers();
        long AddNew(string account, string name, bool is_admin, bool is_expert, bool is_controller);
        void SetPassword(Int64 userId, string password);
        User GetCurrentUser();
    }
}
