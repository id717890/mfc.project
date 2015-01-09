using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mfc.domain.entities;
using Ninject;
using mfc.infrastructure.services;
using System.Data.SqlClient;

namespace mfc.domain.services {
    public class ActionService : IActionService {
        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public IIdentifierService IdService { get; set; }

        [Inject]
        public IServiceService ServiceService { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public IActionTypeService TypeService { get; set; }

        public ServiceAction GetActionById(long Id) {
            ServiceAction item = null;

            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();
            SqlDataReader reader = null;

            try {
                cmd.CommandText = @"
                    select id, dt, customer, service_id, type_id, user_id
                        from Actions
                    where is_deleted = 0
                        and id = @id
                    order by id";

                cmd.Parameters.Add(new SqlParameter("id", Id));

                reader = cmd.ExecuteReader();

                if (reader.Read()) {
                    item = CreateAction(reader);
                }
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                if (reader != null) {
                    reader.Close();
                }
                cmd.Dispose();
                conn.Close();
            }

            return item;
        }


        public IEnumerable<ServiceAction> GetActions(User user, DateTime date) {
            List<ServiceAction> items = new List<ServiceAction>();

            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();
            SqlDataReader reader = null;
            
            try{
                cmd.CommandText = @"
                    select id, dt, customer, service_id, type_id, user_id
                        from Actions
                    where is_deleted = 0
                        and user_id = @user_id
                        and dt = @dt
                    order by id";

                cmd.Parameters.Add(new SqlParameter("user_id", user.Id));
                cmd.Parameters.Add(new SqlParameter("dt", date));
                
                reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    items.Add(CreateAction(reader));
                }
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                if (reader != null) {
                    reader.Close();
                }
                cmd.Dispose();
                conn.Close();
            }

            return items;
        }

        public IEnumerable<ServiceAction> GetActions(User user, DateTime dateBegin, DateTime dateEnd) {
            throw new NotImplementedException();
        }

        public IEnumerable<ServiceAction> GetActions(DateTime dateBegin, DateTime dateEnd) {
            throw new NotImplementedException();
        }

        public long Add(DateTime date, Int64 serviceId, string customer, Int64 typeId, Int64 userId) {
            Int64 result_id = -1;

            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();

            try {
                var new_id = IdService.GetId();

                cmd.CommandText = @"
                    insert into Actions 
                            (id, dt, customer, service_id, type_id, user_id) 
                    values  (@id, @dt, @customer, @service_id, @type_id, @user_id)";

                cmd.Parameters.Add(new SqlParameter("id", new_id));
                cmd.Parameters.Add(new SqlParameter("dt", date));
                cmd.Parameters.Add(new SqlParameter("customer", customer));
                cmd.Parameters.Add(new SqlParameter("service_id", serviceId));
                cmd.Parameters.Add(new SqlParameter("type_id", typeId));
                cmd.Parameters.Add(new SqlParameter("user_id", userId));

                cmd.ExecuteNonQuery();

                result_id = new_id;
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                cmd.Dispose();
                conn.Close();
            }

            return result_id;
        }

        public void Update(ServiceAction action) {
            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();

            try {

                cmd.CommandText = @"
                    update Actions set 
                        dt = @dt,
                        customer = @customer,
                        service_id = @service_id,
                        type_id = @type_id,
                        user_id = @user_id
                     where id = @id";

                cmd.Parameters.Add(new SqlParameter("id", action.Id));
                cmd.Parameters.Add(new SqlParameter("dt", action.Date));
                cmd.Parameters.Add(new SqlParameter("customer", action.Customer));
                cmd.Parameters.Add(new SqlParameter("service_id", action.Service.Id));
                cmd.Parameters.Add(new SqlParameter("type_id", action.Type.Id));
                cmd.Parameters.Add(new SqlParameter("user_id", action.User.Id));

                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                cmd.Dispose();
                conn.Close();
            }
        }

        public void Delete(long id) {
            var conn = SqlProvider.CreateConnection();
            var cmd = conn.CreateCommand();

            try {

                cmd.CommandText = @"
                    update Actions set is_deleted = 1
                        where id = @id";

                cmd.Parameters.Add(new SqlParameter("id", id));
                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                throw new DomainException(e);
            }
            finally {
                cmd.Dispose();
                conn.Close();
            }
        }

        #region Helpers

        private ServiceAction CreateAction(SqlDataReader reader) {
            var action = new ServiceAction {
                Id = Convert.ToInt64(reader["id"]),
                User = UserService.GetUserById(Convert.ToInt64(reader["user_id"])),
                Type = TypeService.GetTypeById(Convert.ToInt64(reader["type_id"])),
                Service = ServiceService.GetServiceById(Convert.ToInt64(reader["service_id"])),
                Customer = Convert.ToString(reader["customer"]),
                Date = Convert.ToDateTime(reader["dt"])
            };

            return action;
        }

        #endregion
    }
}
