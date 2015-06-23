using mfc.domain.entities;
using mfc.domain.services;
using mfc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mfc.web.Helpers {
    public static class PackageHelper {
        public static PackageModel CreateModel(IEnumerable<Int64> fileIds) {
            var file_srv = CompositionRoot.Resolve<IFileService>();
            

            PackageModel model = new PackageModel();
            model.Date = DateTime.Today;

            var files = new List<FileModelItem>();

            foreach (var id in fileIds) {
                var file = file_srv.GetFileById(id);
                if (file != null) {
                    //Если ОГВ еще не установлено, то устанавливаем ОГВ первого в списке дел
                    if (model.OrganizationId <=0 ) {
                        model.Organization = file.Ogv.Caption;
                        model.OrganizationId = file.Ogv.Id;
                    }
                    files.Add(FileModelConverter.ToModelItem(file));
                }
            }

            model.Files = files.ToArray();

            PrepareModel(model);

            return model;
        }

        public static PackageModel CreateModel(Package package) {
            var file_srv = CompositionRoot.Resolve<IFileService>();
            var package_srv = CompositionRoot.Resolve<IPackageService>();

            PackageModel model = new PackageModel();
            model.Date = package.Date;
            model.Id = package.Id;
            model.Organization = package.Organization.Caption;
            model.OrganizationId = package.Organization.Id;
            model.ControllerId = package.Controller.Id;
            model.Comment = package.Comment;

            var files = new List<FileModelItem>();

            foreach (var file in package_srv.GetPackageFiles(package.Id)) {
                files.Add(FileModelConverter.ToModelItem(file));
            }

            model.Files = files.ToArray();

            PrepareModel(model);

            return model;
        }

        public static Package CreatePackage(PackageModel model) {
            var org_srv = CompositionRoot.Resolve<IOrganizationService>();
            var user_srv = CompositionRoot.Resolve<IUserService>();

            Package package = new Package {
                Id = model.Id,
                Date = model.Date,
                Organization = org_srv.GetOrganizationById(model.OrganizationId),
                Controller = user_srv.GetUserById(model.ControllerId),
                Comment = model.Comment
            };

            return package;
        }
        public static void PrepareModel(PackageModel model) {
            var org_srv = CompositionRoot.Resolve<IOrganizationService>();
            model.Organizations.AddRange(org_srv.GetAllOrganizations());
        }
    }
}