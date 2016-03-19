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
        private readonly List<FileModelItem> _files = new List<FileModelItem>();
        private readonly List<FileStatus> _statuses = new List<FileStatus>();
        private readonly List<Organization> _ogvs = new List<Organization>();
        
        #endregion

        [Display(Name = "Контролер")]
        public Int64 SelectedControllerId { get; set; }
        
        [Display(Name = "Статус")]
        public Int64 SelectedStatusId { get; set; }
        
        [Display(Name = "ОГВ")]
        public Int64 SelectedOgvId { get; set; }
        
        [Display(Name = "Эксперт")]        
        public Int64 SelectedExpertId { get; set; }
        
        [Display(Name="От")]
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }
        
        [Display(Name = "До")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

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

        public IList<FileModelItem> Files {
            get {
                return _files;
            }
        }

        public Int32 Page { get; set; }
    }
}