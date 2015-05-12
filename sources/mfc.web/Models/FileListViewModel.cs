using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class FileListViewModel {
        #region Fields
        
        private readonly List<User> _users = new List<User>();
        private readonly List<File> _files = new List<File>();
        private readonly List<FileStatus> _statuses = new List<FileStatus>();
        private readonly List<Organization> _ogvs = new List<Organization>();
        
        #endregion

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public Int64 SelectedUserId { get; set; }
        public Int64 SelectedStatusId { get; set; }
        public Int64 SelectedOgvId { get; set; }

        public IList<User> Controllers {
            get {
                return _users;
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