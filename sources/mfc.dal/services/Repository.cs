using mfc.domain.entities;
using mfc.infrastructure.services;
using NHibernate;
using NHibernate.Linq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity {
        private IUnitOfWorkProvider _unitOfWorkProvider;
        [Inject]
        public IIdentifierService IdService { get; set; }

        public Repository(IUnitOfWorkProvider unitOfWorkProvider) {
            _unitOfWorkProvider = unitOfWorkProvider;
        }

        protected ISession Session { get { return ((UnitOfWork)_unitOfWorkProvider.GetUnitOfWork()).Session; } }

        public IEnumerable<TEntity> GetAll() {
            return Session.Query<TEntity>().Where(x=>!x.IsDeleted).ToList<TEntity>();
        }

        public TEntity GetById(Int64 id) {
            return Session.Get<TEntity>(id);
        }

        public void Create(TEntity entity) {
            entity.Id = IdService.GetId();
            Session.Save(entity);
        }

        public void Update(TEntity entity) {
            Session.Update(entity);
        }

        public void Delete(Int64 id) {
            var entity = Session.Load<TEntity>(id);
            entity.IsDeleted = true;
            Session.Update(entity);
        }
    }
}
