using mfc.domain.entities;
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
        private int _last_column = 5;
        private int _first_row = 3;

        #endregion

        [Inject]
        public IReportModel ReportModel { get; set; }

        public void MakeReport(DateTime dateBegin, DateTime dateEnd, Stream stream) {
            IWorkbook book = Factory.GetWorkbook();

            var sheet = book.Worksheets[0];

            PrepareTemplate(sheet, dateBegin, dateEnd);
            ReportModel.Refresh(dateBegin, dateEnd);

            Int32 row_index = _first_row;

            List<Int32> sum_rows = new List<int>();

            foreach (var type in ReportModel.GetTypes()) {
                RenderType(type, sheet, ref row_index);
                sum_rows.Add(row_index - 1);
            }

            string formula = "";
            foreach (int index in sum_rows) {
                formula += string.Format("R[{0}]C+", index - row_index);
            }

            if (!string.IsNullOrEmpty(formula)) {
                sheet.Cells[row_index, 2].FormulaR1C1 = string.Format("=SUM({0})", formula.TrimEnd('+'));
            }
            
            var range =sheet.Cells[row_index, 0]; 
            range.Value = "ИТОГО";
            range.Font.Italic = true;

            range = sheet.Cells[row_index, 0, row_index, _last_column];
            range.Font.Size = 16;
            

            sheet.Cells[_first_row - 2, 0, row_index, _last_column].Borders.LineStyle = LineStyle.Continuous;

            sheet.PageSetup.Orientation = PageOrientation.Landscape;
            sheet.PageSetup.FitToPagesTall = 999;
            

            book.SaveToStream(stream, FileFormat.Excel8);
        }

        private void RenderType(OrganizationType type, IWorksheet sheet, ref Int32 row_index) {
            Int32 first_row = row_index;

            sheet.Cells[row_index, 0].Value = type.Caption.ToUpper();

            var range = sheet.Cells[row_index, 0, row_index, 2];
            range.Merge();
            range.Font.Color = Color.Red;
            range.Font.Italic = true;

            row_index++;

            foreach (var org in ReportModel.GetOrganizations(type)) {
                RenderOrganization(org, sheet, ref row_index);
            }

            if (row_index == first_row) {
                ++row_index;
            }

            var formula = string.Format("=SUM(R[{0}]C:R[-1]C)", first_row - row_index);

            sheet.Cells[row_index, 2].FormulaR1C1 = formula;
            sheet.Cells[row_index, 3].FormulaR1C1 = formula;
            sheet.Cells[row_index, 4].FormulaR1C1 = formula;
            sheet.Cells[row_index, 5].FormulaR1C1 = formula;

            range = sheet.Cells[row_index, 0, row_index, _last_column];
            range.Font.Color = Color.Red;
            range.Font.Size = 14;


            sheet.Cells[row_index, 0].Value = string.Format("ИТОГО: {0}", type.Caption.ToUpper());
            range = sheet.Cells[row_index, 0, row_index, 1];
            range.Merge();
            range.Font.Italic = true;
            range.Font.Size = 11;

            row_index++;
        }

        private void RenderOrganization(Organization org, IWorksheet sheet, ref Int32 row_index) {
            foreach (var row in ReportModel.GetRows(org)) {
                sheet.Cells[row_index, 0].Value = org.Caption;
                RenderRow(row, sheet, row_index);
                row_index++;
            }
        }

        private void RenderRow(ReportRow row, IWorksheet sheet, Int32 row_index) {
            var range = sheet.Cells[row_index, 1];

            range.Value = row.Service;
            range.WrapText = true;

            sheet.Cells[row_index, 2].Value = row.All;
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

        private void PrepareTemplate(IWorksheet sheet, DateTime dateBegin, DateTime dateEnd) {
            sheet.Cells[0, 0, 0, 5].Merge();


            //Заголовок
            var range = sheet.Cells[0, 0];
            range.Value = string.Format("МАУ МФЦ с {0:dd.MM.yyyy} по {1:dd.MM.yyyy}", dateBegin, dateEnd);
            range.HorizontalAlignment = HAlign.Center;
            range.Font.Size = 16;
            range.Font.Bold = true;

            //Ширины колонок
            sheet.Cells[0, 0].ColumnWidth = 14;
            sheet.Cells[0, 1].ColumnWidth = 90;
            sheet.Cells[0, 2].ColumnWidth = 20;
            sheet.Cells[0, 3].ColumnWidth = 14;
            sheet.Cells[0, 4].ColumnWidth = 14;
            sheet.Cells[0, 5].ColumnWidth = 20;

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
    }
}
