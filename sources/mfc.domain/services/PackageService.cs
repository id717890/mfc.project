using mfc.dal.services;
using mfc.domain.entities;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.services {
    public class PackageService : IPackageService {
        [Inject]
        public IPackageRepository PackageRepository { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public IOrganizationService OrganizationService { get; set; }

        [Inject]
        public IUnitOfWorkProvider UnitOfWorkProvider { get; set; }

        [Inject]
        public IFileService FileService { get; set; }

        public IEnumerable<Package> GetPackages(DateTime dateBegin, DateTime dateEnd, Int64 controllerId, Int64 orgId) {
            var organization = OrganizationService.GetOrganizationById(orgId);
            var controller = UserService.GetUserById(controllerId);
            
            return PackageRepository.GetPackages(dateBegin, dateEnd, controller, organization);
        }

        public long CreatePackage(User controller, DateTime date, Organization organization, IEnumerable<long> files) {
            Package package = new Package();
            package.Date = date;
            package.Controller = controller;
            package.Organization = organization;

            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();
            unit_of_work.BeginTransaction();
            PackageRepository.Create(package);

            unit_of_work.Commit();

            UpdatePackageFiles(package.Id, files);

            return package.Id;
        }

        public Package GetPackageById(long packageId) {
            return PackageRepository.GetById(packageId);
        }

        public IEnumerable<File> GetPackageFiles(long packageId) {
            List<File> files = new List<File>();

            foreach (var id in PackageRepository.GetPackageFileIds(packageId)) {
                files.Add(FileService.GetFileById(id));
            }

            return files;
        }

        public void UpdatePackageFiles(Int64 package_id, IEnumerable<Int64> file_ids) {
            PackageRepository.UpdateFiles(package_id, file_ids);
        }


        public void Update(Package package) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();
            unit_of_work.BeginTransaction();

            PackageRepository.Update(package);

            unit_of_work.Commit();
        }


        public void Delete(long id) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();
            unit_of_work.BeginTransaction();

            PackageRepository.Delete(id);

            unit_of_work.Commit();
        }
    }
}
