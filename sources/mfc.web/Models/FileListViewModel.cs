using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class FileListViewModel {
        #region Fields

        private readonly List<User> _controllers = new List<User>();
        private readonly List<User> _experts = new List<User>();
        private readonly List<File> _files = new List<File>();
        private readonly List<FileStatus> _statuses = new List<FileStatus>();
        private readonly List<Organization> _ogvs = new List<Organization>();
        
        #endregion

        public Int64 SelectedControllerId { get; set; }
        public Int64 SelectedStatusId { get; set; }
        public Int64 SelectedOgvId { get; set; }
        public Int64 SelectedExpertId { get; set; }

        public IList<User> Controllers {
            get {
                return _controllers;
            }
        }

        public IList<User> Experts {
            get {
                return _experts;
            }
        }

        public IList<FileStatus> Statuses {
            get {
                return _statuses;
            }
        }

        public IList<Organization> Organizations {
            get {
                return _ogvs;
            }
        }

        public IList<File> Files {
            get {
                return _files;
            }
        }
    }
}