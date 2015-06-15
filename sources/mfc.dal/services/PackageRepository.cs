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
    }
}
