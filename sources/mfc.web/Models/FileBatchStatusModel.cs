using System.Collections.Generic;

namespace mfc.web.Models {
    public class FileBatchSettingsModel {
        #region Fields

        private List<long> _files;
        private List<FileStatusModel> _statuses;

        #endregion

        public List<long> Files {
            get {
                if (_files == null)
                    _files = new List<long>();

                return _files;
            }

            set {
                _files = value;
            }
        }

        public List<FileStatusModel> Statuses {
            get {
                if (_statuses == null)
                    _statuses = new List<FileStatusModel>();

                return _statuses;
            }

            set{
                _statuses = value;
            }
        }

        public long Status { get; set; }
    }
}