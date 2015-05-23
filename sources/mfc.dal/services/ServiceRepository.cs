﻿using NHibernate.Linq;
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
    }
}