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

        [Inject]
        public IFileStatusService FileStatusService { get; set; }

        [Inject]
        public IFileStageService FileStageService { get; set; }

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
            var sended_status = FileStageService.GetStatusForStage(FileStages.Sended);

            if (sended_status == null) {
                throw new ArgumentException(string.Format("Не определен статус для дел, отправленных в ОГВ"));
            }

            var files_old = new List<Int64>(GetPackageFiles(package_id).Select(m=>m.Id));
            var return_status = new List<Int64>();
            var set_status = new List<Int64>();

            //1. Для новых файлов
            foreach (var id in file_ids) {
                if (!files_old.Contains(id)) {
                    set_status.Add(id);
                }
            }

            //2. Для удаленных файлов
            foreach (var id in files_old) {
                if (!file_ids.Contains(id)) {
                    return_status.Add(id);
                }
            }

            PackageRepository.UpdateFiles(package_id, file_ids);

            //Для всех новых файлов устанавливаем статус "Отправлено в ОГВ"
            foreach (var id in set_status) {
                FileStatusService.SetStatus(id, sended_status.Id, DateTime.Now, string.Empty);
                
                var file = FileService.GetFileById(id);

                if (file != null) {
                    file.CurrentStatus = sended_status;
                    FileService.Update(file);
                }
            }

            //Для удаленных дел возвращаем последний статус
            ReturnStatusForFiles(return_status);
        }


        public void Update(Package package) {
            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();
            unit_of_work.BeginTransaction();

            PackageRepository.Update(package);

            unit_of_work.Commit();
        }


        public void Delete(long id) {
            var files_old = new List<Int64>(GetPackageFiles(id).Select(m => m.Id));

            var unit_of_work = UnitOfWorkProvider.GetUnitOfWork();
            unit_of_work.BeginTransaction();

            PackageRepository.Delete(id);

            unit_of_work.Commit();

            ReturnStatusForFiles(files_old);
        }

        private void ReturnStatusForFiles(IEnumerable<Int64> file_ids) {
            var sended_status = FileStageService.GetStatusForStage(FileStages.Sended);

            if (sended_status == null) {
                throw new ArgumentException(string.Format("Не определен статус для дел, отправленных в ОГВ"));
            }

            foreach (var id in file_ids) {
                //1. Удалеяем статус отправлено из истории
                //2. Возвращаем последний (по времени) статус

                //1.
                FileStatusService.DeleteStatus(id, sended_status.Id);

                //2.
                var file = FileService.GetFileById(id);
                var status = FileStatusService.GetLastStatuses(id);

                if (file != null && status != null) {
                    file.CurrentStatus = status.Status;
                    FileService.Update(file);
                }
            }
        }
    }
}
