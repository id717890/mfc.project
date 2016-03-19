using NHibernate.Linq;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public class ServiceRepository : Repository<Service>, IServiceRepository {
        public ServiceRepository(IUnitOfWorkProvider provider) : base(provider) { }

        public override IEnumerable<Service> GetAll() {
            return Session.Query<Service>().Fetch(x=>x.Organization).Where(x=> !x.IsDeleted).ToList();
        }

        public IEnumerable<Service> GetChildServices(long parentId) {
            return Session.Query<Service>().Fetch(x => x.Organization).Where(x => !x.IsDeleted && x.Parent.Id == parentId).ToList();
        }

        public IEnumerable<Service> Search(string term) {
            return Session.Query<Service>().Fetch(x => x.Organization).Where(x => !x.IsDeleted && x.Caption.ToLower().Contains(term)).Take(20).ToList();
        }

        public override void Delete(Int64 id) {
            var entity = Session.Load<Service>(id);
            entity.IsDeleted = true;
            Session.Update(entity);

            foreach (var child in GetChildServices(id)) {
                child.IsDeleted = true;
                Session.Update(child);
            }
            
        }
    }
}
