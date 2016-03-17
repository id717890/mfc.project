using mfc.domain.entities;
using mfc.domain.services;
using mfc.infrastructure.report;
using Ninject;
using SpreadsheetGear;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace mfc.infrastructure.services {
    public class ReportService : IReportService {
        #region Fields
        

        #endregion

        [Inject]
        public IReportSumModel ReportSumModel { get; set; }

        [Inject]
        public IActionService ActionService { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public IPackageService PackageService { get; set; }

        [Inject]
        public ICustomerTypeService CustomerTypeService { get; set; }

        public void MakeReportSum(DateTime dateBegin, DateTime dateEnd, Int64 customerTypeId, Stream stream) {
            int last_column = 5;
            int first_row = 3;

            IWorkbook book = Factory.GetWorkbook();

            var sheet = book.Worksheets[0];
            var customerType = CustomerTypeService.GetTypeById(customerTypeId);

            if (customerType == CustomerType.Empty) {
                customerType = null;
            }

            PrepareTemplateSum(sheet, dateBegin, dateEnd, customerType);
            ReportSumModel.Refresh(dateBegin, dateEnd, customerType);

            Int32 row_index = first_row;

            List<Int32> sum_rows = new List<int>();

            foreach (var type in ReportSumModel.GetTypes()) {
                RenderType(type, sheet, ref row_index);
                sum_rows.Add(row_index - 1);
            }

            string formula = "";
            foreach (int index in sum_rows) {
                formula += string.Format("R[{0}]C+", index - row_index);
            }

            if (!string.IsNullOrEmpty(formula)) {
                formula = string.Format("=SUM({0})", formula.TrimEnd('+'));
                sheet.Cells[row_index, 2].FormulaR1C1 = formula;
                sheet.Cells[row_index, 3].FormulaR1C1 = formula;
                sheet.Cells[row_index, 4].FormulaR1C1 = formula;
                sheet.Cells[row_index, 5].FormulaR1C1 = formula;
            }
            
            var range =sheet.Cells[row_index, 0]; 
            range.Value = "ИТОГО";
            range.Font.Italic = true;

            range = sheet.Cells[row_index, 0, row_index, last_column];
            range.Font.Size = 16;
            

            sheet.Cells[first_row - 2, 0, row_index, last_column].Borders.LineStyle = LineStyle.Continuous;

            sheet.PageSetup.Orientation = PageOrientation.Landscape;
            sheet.PageSetup.FitToPagesTall = 999;
            

            book.SaveToStream(stream, FileFormat.Excel8);
        }

        public void MakeReportOper(DateTime dateBegin, DateTime dateEnd, Int64 customerTypeId, Stream stream) {
            int last_column = 8;
            int first_row = 2;

            IWorkbook book = Factory.GetWorkbook();

            var sheet = book.Worksheets[0];

            var customerType = CustomerTypeService.GetTypeById(customerTypeId);

            if (customerType == CustomerType.Empty) {
                customerType = null;
            }

            PrepareTemplateOper(sheet, dateBegin, dateEnd, customerType);
            ReportSumModel.Refresh(dateBegin, dateEnd, customerType);

            Int32 row_index = first_row;
            Int32 num = 0;

            IEnumerable<ServiceAction> actions = null;

            var user = UserService.GetCurrentUser();

            if (user.IsAdmin) {
                actions = ActionService.GetActions(dateBegin, dateEnd, customerType);
            }
            else {
                actions = ActionService.GetActions(user, dateBegin, dateEnd, customerType);
            }

            foreach (var action in actions) {
                if (action.Service == null || action.Service.Organization == null || action.User == null || action.Type == null) {
                    continue;
                }

                sheet.Cells[row_index, 0].Value = ++num;
                sheet.Cells[row_index, 1].Value = action.Date.ToString("dd.MM.yyyy");
                sheet.Cells[row_index, 2].Value = action.Customer;
                sheet.Cells[row_index, 3].Value = action.IsNonresident ? "Да" : "Нет";
                sheet.Cells[row_index, 4].Value = action.FreeVisit ? "Да" : "Нет";
                sheet.Cells[row_index, 5].Value = action.Service.Caption;
                sheet.Cells[row_index, 6].Value = action.Service.Organization.Caption;
                sheet.Cells[row_index, 7].Value = action.Type.Caption;
                sheet.Cells[row_index, 8].Value = action.User.Name;

                row_index++;
            }

            sheet.Cells[first_row - 2, 0, row_index, last_column].Borders.LineStyle = LineStyle.Continuous;

            sheet.PageSetup.Orientation = PageOrientation.Landscape;
            sheet.PageSetup.FitToPagesTall = 999;


            book.SaveToStream(stream, FileFormat.Excel8);
        }

        public void MakeReestr(long packageId, Stream stream) {
            var package = PackageService.GetPackageById(packageId);

            if (package == null) {
                throw new Exception(string.Format("Пакет с кодом {0} не найден", packageId));
            }
            
            int last_column = 3;
            int first_row = 2;

            IWorkbook book = Factory.GetWorkbook();

            var sheet = book.Worksheets[0];

            PrepareTemplateReestr(sheet, package.Organization != null ? package.Organization.FullCaption : "Не определена", package.Date);

            Int32 row_index = first_row;
            Int32 num = 0;

            foreach (var file in PackageService.GetPackageFiles(packageId)) {
                var action = file.Action;

                sheet.Cells[row_index, 0].Value = ++num;
                sheet.Cells[row_index, 1].Value = file.Date.ToString("dd.MM.yyyy");
                sheet.Cells[row_index, 2].Value = file.Caption;
                sheet.Cells[row_index, 3].Value =  action.Service != null ? action.Service.Caption : "не определена";

                row_index++;
            }

            sheet.Cells[first_row - 2, 0, row_index, last_column].Borders.LineStyle = LineStyle.Continuous;

            sheet.PageSetup.Orientation = PageOrientation.Portrait;
            sheet.PageSetup.FitToPagesTall = 999;


            book.SaveToStream(stream, FileFormat.Excel8);
        }


        private void RenderType(OrganizationType type, IWorksheet sheet, ref Int32 row_index) {
            Int32 first_row = row_index;
            Int32 last_column = 5;

            sheet.Cells[row_index, 0].Value = type.Caption.ToUpper();

            var range = sheet.Cells[row_index, 0, row_index, 2];
            range.Merge();
            range.Font.Color = Color.Red;
            range.Font.Italic = true;

            row_index++;

            foreach (var row in ReportSumModel.GetRows(type)) {
                RenderRow(row, sheet, row_index);
                row_index++;
            }

            if (row_index == first_row) {
                ++row_index;
            }

            var formula = string.Format("=SUM(R[{0}]C:R[-1]C)", first_row - row_index);

            sheet.Cells[row_index, 2].FormulaR1C1 = formula;
            sheet.Cells[row_index, 3].FormulaR1C1 = formula;
            sheet.Cells[row_index, 4].FormulaR1C1 = formula;
            sheet.Cells[row_index, 5].FormulaR1C1 = formula;

            range = sheet.Cells[row_index, 0, row_index, last_column];
            range.Font.Color = Color.Red;
            range.Font.Size = 14;


            sheet.Cells[row_index, 0].Value = string.Format("ИТОГО: {0}", type.Caption.ToUpper());
            range = sheet.Cells[row_index, 0, row_index, 1];
            range.Merge();
            range.Font.Italic = true;
            range.Font.Size = 11;

            row_index++;
        }

        private void RenderRow(ReportSumRow row, IWorksheet sheet, Int32 row_index) {
            var range = sheet.Cells[row_index, 0];

            range.Value = row.Service.Organization.FullCaption;
            range.WrapText = true;

            range = sheet.Cells[row_index, 1];
            range.Value = row.Service.Caption;
            range.WrapText = true;

            sheet.Cells[row_index, 2].FormulaR1C1 = "=RC[1] + RC[2] + RC[3]";
            if (row.Priem > 0) {
                sheet.Cells[row_index, 3].Value = row.Priem;
            }
            if (row.Vidacha > 0) {
                sheet.Cells[row_index, 4].Value = row.Vidacha;
            }
            if (row.Consult > 0) {
                sheet.Cells[row_index, 5].Value = row.Consult;
            }
        }

        private void PrepareTemplateSum(IWorksheet sheet, DateTime dateBegin, DateTime dateEnd, CustomerType customerType) {
            sheet.Cells[0, 0, 0, 5].Merge();


            //Заголовок
            var range = sheet.Cells[0, 0];
            range.Value = string.Format("МАУ МФЦ с {0:dd.MM.yyyy} по {1:dd.MM.yyyy}", dateBegin, dateEnd);

            if (customerType != null) {
                range.Value = range.Value.ToString() + string.Format(" ({0})", customerType.Caption);
            }
            range.HorizontalAlignment = HAlign.Center;
            range.Font.Size = 16;
            range.Font.Bold = true;

            //Ширины колонок
            sheet.Cells[0, 0].ColumnWidth = 50;
            sheet.Cells[0, 1].ColumnWidth = 90;
            sheet.Cells[0, 2].ColumnWidth = 15;
            sheet.Cells[0, 3].ColumnWidth = 14;
            sheet.Cells[0, 4].ColumnWidth = 14;
            sheet.Cells[0, 5].ColumnWidth = 14;

            //Заловок таблицы
            sheet.Cells[2, 0].Value = "ОГВ";
            sheet.Cells[1, 0, 2, 0].Merge();
            sheet.Cells[2, 1].Value = "Услуга";
            sheet.Cells[1, 1, 2, 1].Merge();
            sheet.Cells[2, 2].Value = "ВСЕГО";
            sheet.Cells[1, 2, 2, 2].Merge();

            range = sheet.Cells[1, 0, 2, 2];
            range.Font.Size = 16;

            sheet.Cells[2, 3].Value = "прием документов";
            sheet.Cells[2, 4].Value = "выдача документов";
            sheet.Cells[2, 5].Value = "консультирование";

            range = sheet.Cells[2, 0, 2, 2];
            range.Font.Size = 12;

            range = sheet.Cells[1, 3];
            range.Value = "в том числе:";
            range.Font.Italic = true;
            range.Font.Size = 14;

            sheet.Cells[1, 3, 1, 5].Merge();
            sheet.Cells[1, 3, 2, 5].Interior.Color = System.Drawing.Color.Yellow;

            range = sheet.Cells[1, 0, 2, 5];
            range.HorizontalAlignment = HAlign.Center;
            range.VerticalAlignment = VAlign.Bottom;
            range.Font.Bold = true;
            range.WrapText = true;

        }

        private void PrepareTemplateOper(IWorksheet sheet, DateTime dateBegin, DateTime dateEnd, CustomerType customerType) {
            sheet.Cells[0, 0, 0, 8].Merge();


            //Заголовок
            var range = sheet.Cells[0, 0];
            range.Value = string.Format("МАУ МФЦ с {0:dd.MM.yyyy} по {1:dd.MM.yyyy}", dateBegin, dateEnd);

            if (customerType != null) {
                range.Value = range.Value.ToString() + string.Format(" ({0})", customerType.Caption);
            }

            range.HorizontalAlignment = HAlign.Center;
            range.Font.Size = 16;
            range.Font.Bold = true;

            //Ширины колонок
            sheet.Cells[0, 0].ColumnWidth = 5;
            sheet.Cells[0, 1].ColumnWidth = 14;
            sheet.Cells[0, 2].ColumnWidth = 14;
            sheet.Cells[0, 3].ColumnWidth = 14;
            sheet.Cells[0, 4].ColumnWidth = 14;
            sheet.Cells[0, 5].ColumnWidth = 90;
            sheet.Cells[0, 6].ColumnWidth = 14;
            sheet.Cells[0, 7].ColumnWidth = 20;
            sheet.Cells[0, 8].ColumnWidth = 20;

            //Заловок таблицы
            sheet.Cells[1, 0].Value = "№";
            sheet.Cells[1, 1].Value = "Дата";
            sheet.Cells[1, 2].Value = "Заявитель";
            sheet.Cells[1, 3].Value = "Иногородний";
            sheet.Cells[1, 4].Value = string.Format("Бесплатный{0}выезд", Environment.NewLine);
            sheet.Cells[1, 5].Value = "Услуга";
            sheet.Cells[1, 6].Value = "ОГВ";
            sheet.Cells[1, 7].Value = "Тип услуги";
            sheet.Cells[1, 8].Value = "Эксперт";

            range = sheet.Range[1, 0, 1, 8];
            range.HorizontalAlignment = HAlign.Center;
            range.VerticalAlignment = VAlign.Center;
            range.Font.Bold = true;

        }

        private void PrepareTemplateReestr(IWorksheet sheet, string caption, DateTime date) {
            sheet.Cells[0, 0, 0, 3].Merge();


            //Заголовок
            var range = sheet.Cells[0, 0];
            range.Value = string.Format("Реестр дел '{0}' {1:dd.MM.yyyy}", caption, date);
            range.HorizontalAlignment = HAlign.Center;
            range.Font.Size = 16;
            range.Font.Bold = true;

            //Ширины колонок
            sheet.Cells[0, 0].ColumnWidth = 5;
            sheet.Cells[0, 1].ColumnWidth = 14;
            sheet.Cells[0, 2].ColumnWidth = 50;
            sheet.Cells[0, 3].ColumnWidth = 50;

            //Заловок таблицы
            sheet.Cells[1, 0].Value = "№";
            sheet.Cells[1, 1].Value = "Дата";
            sheet.Cells[1, 2].Value = "Номер";
            sheet.Cells[1, 3].Value = "Услуга";

            range = sheet.Range[1, 0, 1, 3];
            range.HorizontalAlignment = HAlign.Center;
            range.VerticalAlignment = VAlign.Center;
            range.Font.Bold = true;

        }
    }
}
