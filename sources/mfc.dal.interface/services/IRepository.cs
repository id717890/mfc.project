using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public interface IRepository<TEntity> where TEntity : Entity {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(Int64 id);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(Int64 id);
        void Delete(TEntity entity);
    }
}
