﻿using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public interface IServiceRepository : IRepository<Service> {
        IEnumerable<Service> GetChildServices(Int64 parentId);
        IEnumerable<Service> Search(string term);
    }
}
