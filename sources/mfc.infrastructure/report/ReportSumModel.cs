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
    public class ReportSumModel : IReportSumModel {
        #region Fields
        private Dictionary<OrganizationType, Dictionary<Service, ReportSumRow>> _data = new Dictionary<OrganizationType, Dictionary<Service, ReportSumRow>>();
        #endregion
        
        [Inject]
        public IOrganizationService OrgService { get; set; }

        [Inject]
        public IActionService ActionService { get; set; }

        [Inject]
        public IServiceService SrvService { get; set; }

        [Inject]
        public IActionTypeService ActionTypes { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        public void Refresh(DateTime dateBegin, DateTime dateEnd) {
            _data.Clear();

            IEnumerable<ServiceAction> actions;

            var user = UserService.GetCurrentUser();

            if (user.IsAdmin) {
                actions = ActionService.GetActions(dateBegin, dateEnd);
            }
            else {
                actions = ActionService.GetActions(user, dateBegin, dateEnd);
            }

            foreach (var action in actions) {
                if (action.Service == null || action.Type == null || action.Service.Organization == null || action.User == null || action.Service.Organization.Type == null) {
                    continue;
                }

                Dictionary<Service, ReportSumRow> services = null;

                if (!_data.ContainsKey(action.Service.Organization.Type)) {
                    services = new Dictionary<Service, ReportSumRow>();
                    _data.Add(action.Service.Organization.Type, services);
                }
                else {
                    services = _data[action.Service.Organization.Type];
                }

                ReportSumRow row = null;

                if (!services.ContainsKey(action.Service)) {
                    row = new ReportSumRow {
                        Service = action.Service
                    };

                    services.Add(action.Service, row);
                }
                else {
                    row = services[action.Service];
                }

                var caption = action.Type.Caption.ToLower().Replace("ё", "e");

                if (caption.Contains("прием")) {
                    row.Priem += 1;
                }
                else if (caption.Contains("выдача")) {
                    row.Vidacha += 1;
                }
                else if (caption.Contains("консул")) {
                    row.Consult += 1;
                }
                else {
                    throw new Exception(string.Format("Работа {0} не может быть обработана, поскольку ее тип отчетом не поддерживается", action.Type.Caption));
                }
            }

        }

        public IEnumerable<OrganizationType> GetTypes() {
            return _data.Keys.OrderBy(t => t.Caption);
        }


        public IEnumerable<ReportSumRow> GetRows(OrganizationType type) {
            return _data[type].Values.OrderBy(r => r.Service.Organization.FullCaption).ThenBy(r=>r.Service.Caption);
        }
    }
}
