using NHibernate.Linq;
using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using mfc.domain.services;
using mfc.infrastructure.services;
using System.Data.SqlClient;
using NHibernate.Criterion;

namespace mfc.dal.services {
    public class PackageRepository : Repository<Package>, IPackageRepository {
        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        public PackageRepository(IUnitOfWorkProvider unitOfWorkProvider) : base(unitOfWorkProvider) { }

        public IEnumerable<Package> GetPackages(DateTime dateBegin, DateTime dateEnd, User controller = null, Organization organization = null) {
            var query = Session.Query<Package>();
            if (controller != null) {
                query = query.Where(f => f.Controller.Id == controller.Id);
            }

            if (organization != null) {
                query = query.Where(f => f.Organization.Id == organization.Id);
            }


            return query.Where(f => !f.IsDeleted && f.Date >= dateBegin && f.Date <= dateEnd).OrderByDescending(m => m.Date).ThenByDescending(m => m.Id).ToList();
        }

        public IEnumerable<Int64> GetPackageFileIds(Int64 packageId) {
            List<Int64> ids = new List<long>();

            var session = SqlProvider.CreateConnection();

            try {
                var cmd = session.CreateCommand();
                cmd.CommandText = "select file_id from PackageItems where package_id = @id";
                cmd.Parameters.Add(new SqlParameter("id", packageId));

                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    ids.Add(Convert.ToInt64(reader["file_id"]));
                }
            }
            catch (Exception e) {
                throw e;
            }
            finally {
                session.Close();
            }

            return ids;
        }

        public void UpdateFiles(long package_id, IEnumerable<long> file_ids) {
            var session = SqlProvider.CreateConnection();
            SqlTransaction tx = null;

            try {
                var cmd = session.CreateCommand();
                cmd.CommandText = "delete from PackageItems where package_id = @id";
                cmd.Parameters.Add(new SqlParameter("id", package_id));

                cmd.ExecuteNonQuery();

                cmd.CommandText = "insert into PackageItems (package_id, file_id) values (@id, @file_id)";
                cmd.Parameters.Add(new SqlParameter("file_id", 0));

                foreach (var id in file_ids) {
                    cmd.Parameters["file_id"].Value = id;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e) {
                tx.Rollback();
                throw e;
            }
            finally {
                if (tx != null) {
                    tx.Commit();
                }
                session.Close();
            }
        }

        //Рефакторинг
        public long TotalRows { get; set; }

        public IEnumerable<Package> GetPackages(DateTime dateBegin, DateTime dateEnd, long organization, long controller, int pageIndex,
            int pageSize)
        {
            var date1 = new DateTime(dateBegin.Year, dateBegin.Month, dateBegin.Day, 0, 0, 0);
            var date2 = new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day, 23, 59, 59);
            var criteria = Session.CreateCriteria<Package>()
                .Add(Restrictions.Between("Date", date1, date2))
                .Add(Restrictions.Eq("IsDeleted", false));

            var countOfFiles = Session.CreateCriteria<Package>()
                .Add(Restrictions.Between("Date", date1, date2))
                .Add(Restrictions.Eq("IsDeleted", false));

            if (organization != -1)
            {
                criteria.CreateCriteria("Organization").Add(Restrictions.Eq("Id", organization));
                countOfFiles.CreateCriteria("Organization").Add(Restrictions.Eq("Id", organization));
            }

            if (controller != -1)
            {
                criteria.CreateCriteria("Controller").Add(Restrictions.Eq("Id", controller));
                countOfFiles.CreateCriteria("Controller").Add(Restrictions.Eq("Id", controller));
            }

            criteria
                .AddOrder(Order.Desc("Date"))
                .AddOrder(Order.Desc("Id"))
                .SetMaxResults(pageSize)
                .SetFirstResult((pageIndex - 1) * pageSize)
                .Future<Package>();

            countOfFiles.SetProjection(Projections.Count(Projections.Id()));
            TotalRows = countOfFiles.FutureValue<int>().Value;

            return criteria.List<Package>();
        }

    }
}
