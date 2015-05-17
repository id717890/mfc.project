using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mfc.domain.entities;
using Ninject;
using mfc.infrastructure.services;
using System.Data.SqlClient;
using mfc.dal.services;

namespace mfc.domain.services {
    public class ActionService : IActionService {
        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public IServiceService ServiceService { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public IActionTypeService TypeService { get; set; }

        [Inject]
        public IServiceActionRepository Repository { get; set; }

        [Inject]
        public IUnitOfWorkProvider UnitOfWorkProvider { get; set; }

        public ServiceAction GetActionById(long Id) {
            return Repository.GetById(Id);
        }


        public IEnumerable<ServiceAction> GetActions(User user, DateTime date) {
            return Repository.GetActions(user.Id, date).OrderBy(x=>x.Date);
        }

        public IEnumerable<ServiceAction> GetActions(DateTime dateBegin, DateTime dateEnd) {
            return Repository.GetActions(dateBegin, dateEnd).OrderBy(x=>x.Date);
        }

        public long Add(DateTime date, Int64 serviceId, string customer, Int64 typeId, Int64 userId, string comments) {
            var action = new ServiceAction {
                Date = date,
                Service = ServiceService.GetServiceById(serviceId),
                Customer = customer,
                Type = TypeService.GetTypeById(typeId),
                User = UserService.GetUserById(userId),
                Comments = comments
            };

            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();
            
            unit_of_work.BeginTransaction();
            Repository.Create(action);
            unit_of_work.Commit();

            return 0;
        }

        public void Update(ServiceAction action) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            Repository.Update(action);
            unit_of_work.Commit();
        }

        public void Delete(long id) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            Repository.Delete(id);
            unit_of_work.Commit();
        }
    }
}
