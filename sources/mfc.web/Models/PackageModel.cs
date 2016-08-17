using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class PackageModel {
        #region Fields
        private List<Organization> _orgs = new List<Organization>();
        private List<FileStatus> _statuses = new List<FileStatus>();
        #endregion

        [Required]
        public Int64 Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        [Required()]
        public DateTime Date { get; set; }

        [Display(Name = "ОГВ")]
        [Required()]
        public Int64 OrganizationId { get; set; }

        [Required]
        public Int64 ControllerId { get; set; }

        [Display(Name = "Описание")]
        public string Comment { get; set; }

        public String Organization { get; set; }

        public FileModelItem[] Files { get; set; }

        public List<Organization> Organizations { get { return _orgs; } }

        [Display(Name = "Статус")]
        public long Status { get; set; }

        public List<FileStatus> Statuses { get { return _statuses; } }
    }
}