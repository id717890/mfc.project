using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class AcceptForControlModel {
        List<File> _accepted_files = new List<File>();
        List<File> _rejected_files = new List<File>();
        public AcceptForControlModel() {

        }
        public List<File> AcceptedFiles { get { return _accepted_files; } }
        public List<File> RejectedFiles { get { return _rejected_files; } }
    }
}