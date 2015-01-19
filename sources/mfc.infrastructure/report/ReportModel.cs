using mfc.domain.entities;
using mfc.domain.services;
using mfc.infrastructure.services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.infrastructure.report {
    public class ReportModel : IReportModel {
        #region Fields
        private Dictionary<OrganizationType, Dictionary<Organization, Dictionary<Service, ReportRow>>> _data = new Dictionary<OrganizationType, Dictionary<Organization, Dictionary<Service, ReportRow>>>();
        #endregion
        [Inject]
        public ISqlProvider SqlProvider { get; set; }

        [Inject]
        public IOrganizationService OrgService { get; set; }

        [Inject]
        public IActionService ActionService { get; set; }

        [Inject]
        public IServiceService SrvService { get; set; }

        [Inject]
        public IActionTypeService ActionTypes { get; set; }

        public void Refresh(DateTime dateBegin, DateTime dateEnd) {
            _data.Clear();

            var conn = SqlProvider.CreateConnection();
            SqlCommand cmd = null;

            try {
                cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    select a.type_id as action_id, s.id as srv_id, ot.id as type_id, o.id as org_id, count(*) as cnt from
                    Actions a
                    join Services s on s.id = a.service_id and s.is_deleted = 0
                    join Organizations o on o.id = s.org_id and o.is_deleted = 0
                    join OrganizationTypes ot on ot.id = o.type_id and ot.is_deleted = 0
                    where a.is_deleted = 0
                    and a.dt between @dt_begin and @dt_end
                    group by a.type_id, s.id, ot.id, o.id";
                cmd.Parameters.Add(new SqlParameter("dt_begin", dateBegin));
                cmd.Parameters.Add(new SqlParameter("dt_end", dateEnd));

                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    var type_id = Convert.ToInt64(reader["type_id"]);
                    
                    var type = OrgService.GetTypeById(type_id);

                    if (type == null) {
                        continue;
                    }

                    if (!_data.ContainsKey(type)) {
                        _data.Add(type, new Dictionary<Organization, Dictionary<Service, ReportRow>>());
                    }

                    var org_id = Convert.ToInt64(reader["org_id"]);
                    var org = OrgService.GetOrganizationById(org_id);

                    if (org == null) {
                        continue;
                    }

                    if (!_data[type].ContainsKey(org)) {
                        _data[type].Add(org, new Dictionary<Service, ReportRow>());
                    }

                    var srv_id = Convert.ToInt64(reader["srv_id"]);
                    var srv = SrvService.GetServiceById(srv_id);

                    if (srv == null) {
                        continue;
                    }

                    if (!_data[type][org].ContainsKey(srv)) {
                        _data[type][org].Add(srv, new ReportRow { Service = srv.Caption });
                    }

                    var act_id = Convert.ToInt64(reader["action_id"]);
                    var act = ActionTypes.GetTypeById(act_id);

                    if (act == null) {
                        continue;
                    }

                    var cnt = Convert.ToInt32(reader["cnt"]);

                    string act_caption = act.Caption.ToLower().Replace("ё", "e");
                    var row = _data[type][org][srv];

                    if (act_caption.Contains("прием")) {
                        row.Priem += cnt;
                    }
                    else if (act_caption.Contains("выдач")) {
                        row.Vidacha += cnt;
                    }
                    else if (act_caption.Contains("конс")) {
                        row.Consult += cnt;
                    }
                }
            }
            finally {
                conn.Close();
                if (cmd != null) {
                    cmd.Dispose();
                }
            }

        }

        public IEnumerable<OrganizationType> GetTypes() {
            return _data.Keys.OrderBy(t => t.Id);
        }

        public IEnumerable<Organization> GetOrganizations(OrganizationType type) {
            return _data[type].Keys.OrderBy(t => t.Caption);
        }

        public IEnumerable<ReportRow> GetRows(Organization org) {
            return _data[org.Type][org].Values.OrderBy(r => r.Service);
        }
    }
}
