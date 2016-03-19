using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class ActionListViewModel {
        #region Fields
        private readonly List<User> _users = new List<User>();
        private readonly List<ServiceAction> _actions = new List<ServiceAction>();
        #endregion

        [DataType(DataType.Date)]
        [Display(Name = "С")]
        public DateTime DateBegin { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "До")]
        public DateTime DateEnd { get; set; }

        public Int64 SelectedUserId { get; set; }

        public IList<User> Users {
            get {
                return _users;
            }
        }

        public IList<ServiceAction> Actions {
            get {
                return _actions;
            }
        }
    }
}