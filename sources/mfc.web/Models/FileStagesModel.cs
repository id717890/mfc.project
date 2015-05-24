using mfc.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mfc.web.Models {
    public class FileStagesModel {
        private readonly List<FileStatus> _statuses = new List<FileStatus>();
        private readonly List<FileStageModel> _stages = new List<FileStageModel>();

        public List<FileStageModel> Stages { get { return _stages; } }
        public List<FileStatus> Statuses { get { return _statuses; } }
    }
}