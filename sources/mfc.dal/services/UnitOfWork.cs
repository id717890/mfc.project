using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using mfc.dal.mapping;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    public class UnitOfWork : IUnitOfWork {
        private static readonly ISessionFactory _sessionFactory;
        private ITransaction _transaction;

        public ISession Session { get; private set; }

        static UnitOfWork() {
            // Initialise singleton instance of ISessionFactory, static constructors are only executed once during the
            // application lifetime - the first time the UnitOfWork class is used

            //setup the normal map configuration
            Configuration normalConfig = new Configuration().Configure();

            //setup the fluent map configuration
            _sessionFactory = Fluently.Configure(normalConfig)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<FileMap>())
                .BuildSessionFactory();
        }

        public UnitOfWork() {
            Session = _sessionFactory.OpenSession();
        }

        public void BeginTransaction() {
            _transaction = Session.BeginTransaction();
        }

        public void Commit() {
            try {
                _transaction.Commit();
            }
            catch {
                _transaction.Rollback();
                throw;
            }
        }

        public void Rollback() {
            _transaction.Rollback();
        }
    }
}
