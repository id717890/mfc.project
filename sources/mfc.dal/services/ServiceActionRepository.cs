using NHibernate.Linq;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Visitors;
using mfc.domain.services;
using mfc.infrastructure.services;
using Ninject;

namespace mfc.dal.services {
    public class ServiceActionRepository : Repository<ServiceAction>, IServiceActionRepository {
        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public ILogger Logger { get; set; }

        [Inject]
        public IServiceService ServiceService { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public IActionTypeService ActionTypeService { get; set; }

        [Inject]
        public ICustomerTypeService CustomerTypeService { get; set; }

        public ServiceActionRepository(IUnitOfWorkProvider provider) : base(provider) { }

        public IEnumerable<ServiceAction> GetActions(DateTime dateBegin, DateTime dateEnd) {
            return GetActions(-1, dateBegin, dateEnd);
        }

        public IEnumerable<ServiceAction> GetActions(Int64 user_id, DateTime dateBegin, DateTime dateEnd) {
            var date1 = new DateTime(dateBegin.Year, dateBegin.Month, dateBegin.Day, 0, 0, 0);
            var date2 = new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day, 23, 59, 59);

            var actions = new List<ServiceAction>();

            SqlConnection connection = SqlProvider.CreateConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try {
                cmd = connection.CreateCommand();

                cmd.CommandText = @"
                    select a.id, a.dt, a.customer, a.service_id, a.service_child_id, a.type_id, a.user_id, s.org_id, a.customer_type_id, a.free_visit, a.is_nonresident, a.comments  
                        from Actions a
                        join Services s on s.id = a.service_id
                    where a.dt between @date1 and @date2
                        and a.is_deleted = 0 ";


                cmd.Parameters.AddWithValue("date1", date1);
                cmd.Parameters.AddWithValue("date2", date2);

                if (user_id >= 0) {
                    cmd.CommandText += " and a.user_id = @user_id";
                    cmd.Parameters.AddWithValue("user_id", user_id);
                }

                cmd.CommandText += " order by a.dt desc, a.id desc";


                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    actions.Add(new ServiceAction {
                        Id = Convert.ToInt64(reader["id"]),
                        Service = ServiceService.GetServiceById(Convert.ToInt64(reader["service_id"])),
                        ServiceChild = reader["service_child_id"] != DBNull.Value ? ServiceService.GetServiceById(Convert.ToInt64(reader["service_child_id"])) : null,
                        User = UserService.GetUserById(Convert.ToInt64(reader["user_id"])),
                        Date = Convert.ToDateTime(reader["dt"]),
                        Type = ActionTypeService.GetTypeById(Convert.ToInt64(reader["type_id"])),
                        Customer = reader["customer"] != DBNull.Value ? Convert.ToString(reader["customer"]) : string.Empty,
                        CustomerType = reader["customer_type_id"] != DBNull.Value ? CustomerTypeService.GetTypeById(Convert.ToInt64(reader["customer_type_id"])) : CustomerType.Empty,
                        FreeVisit = reader["free_visit"] !=DBNull.Value && Convert.ToBoolean(reader["free_visit"]),
                        IsNonresident = reader["is_nonresident"] !=DBNull.Value && Convert.ToBoolean(reader["is_nonresident"]),
                        Comments = reader["comments"] != DBNull.Value ? Convert.ToString(reader["comments"]) : string.Empty
                    });
                }
            }
            catch (Exception e) {
                Logger.Error(e);
                throw;
            }
            finally {
                if (reader != null) {
                    reader.Close();
                    reader.Dispose();
                }

                if (cmd != null) {
                    cmd.Dispose();
                }
                connection.Close();
            }

            return actions;
        }
    }
}
