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

        [Inject]
        public IFileService FileService { get; set; }

        public ServiceAction GetActionById(long Id) {
            return Repository.GetById(Id);
        }


        public IEnumerable<ServiceAction> GetActions(User user, DateTime date) {
            return Repository.GetActions(user.Id, date).OrderByDescending(x=>x.Date).ThenByDescending(x=>x.Id);
        }

        public IEnumerable<ServiceAction> GetActions(DateTime dateBegin, DateTime dateEnd) {
            return Repository.GetActions(dateBegin, dateEnd).OrderByDescending(x=>x.Date).ThenByDescending(x=>x.Id);
        }

        public IEnumerable<ServiceAction> GetActions(User user, DateTime dateBegin, DateTime dateEnd) {
            return Repository.GetActions(user.Id, dateBegin, dateEnd).OrderByDescending(x => x.Date).ThenByDescending(x => x.Id);
        }

        public long Add(DateTime date, Int64 serviceId, string customer, Int64 typeId, Int64 userId, Int64 serviceChildId, string comments) {
            var action = new ServiceAction {
                Date = date,
                Service = ServiceService.GetServiceById(serviceId),
                ServiceChild = ServiceService.GetServiceById(serviceChildId),
                Customer = customer,
                Type = TypeService.GetTypeById(typeId),
                User = UserService.GetUserById(userId),
                Comments = comments
            };

            if (date == DateTime.Today) {
                action.Date = DateTime.Now;
            }
            else {
                action.Date = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            }

            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();
            
            unit_of_work.BeginTransaction();
            
            Repository.Create(action);
            //Если тип услуги предполагает создание дела, то создаем дело
            if (action.Type.NeedMakeFile) {
                FileService.Add(action);
            }
            
            unit_of_work.Commit();

            return 0;
        }

        public void Update(ServiceAction action) {
            var file = FileService.GetFileByActionId(action.Id);

            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            unit_of_work.BeginTransaction();
            Repository.Update(action);
            
            //Если в результате смены типа приема требуется создание дела, тогда создаем
            if (action.Type.NeedMakeFile) {
                FileService.Add(action);
            }
            //Если услуга не требует дела, то удаляем существующее
            if (!action.Type.NeedMakeFile && file != null) {
                FileService.Delete(file.Id);
            }
            
            unit_of_work.Commit();
        }

        public void Delete(long id) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();

            var file = FileService.GetFileByActionId(id);

            unit_of_work.BeginTransaction();
            Repository.Delete(id);
            if (file != null) {
                FileService.Delete(file.Id);
            }
            unit_of_work.Commit();
        }
    }
}
