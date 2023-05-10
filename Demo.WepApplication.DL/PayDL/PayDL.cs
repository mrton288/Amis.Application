using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Enums;
using Demo.WepApplication.DL.BaseDL;
using Demo.WepApplication.DL.EmployeeDL;
using MySqlConnector;
using Npgsql;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WepApplication.DL.PayDL
{
    public class PayDL : BaseDL<Pay>, IPayDL
    {
        /// <summary>
        /// Sinh ra số chứng từ mới
        /// </summary>
        /// <returns>Số chứng từ mới</returns>
        /// Author: NVDUC (04/05/2023)
        public string GetNewVoucherCode()
        {
            // Tương tác với database
            string queryString = "select * from func_pay_getmax_voucher_number()";
            // Khởi tạo kết nối tới database
            using (var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString))
            {
                postgreSQL.Open();

                string result = postgreSQL.QueryFirstOrDefault<string>(queryString, commandType: System.Data.CommandType.Text);
                // Số chứng từ cuối
                int numberEmployeeCode = Convert.ToInt32(result.Substring(2)) + 1;

                // Format lại số chứng từ cuối nếu không đủ 5 số
                int numberEmployeeCodeClone = numberEmployeeCode;

                // Đếm số lượng chữ số trong mã số đó
                int count = 0;
                while (numberEmployeeCodeClone > 0)
                {
                    numberEmployeeCodeClone /= 10;
                    count++;
                }
                count -= 5;

                // Số 0 thêm vào phía trước để format
                string employeeCodeFormat = "";
                // Số lượng chữ còn thiếu để đủ 5 số
                if (count < 0)
                {
                    count = Math.Abs(count);
                    while (count > 0)
                    {
                        employeeCodeFormat += "0";
                        count--;
                    }
                }

                result = result.Substring(0, 2) + employeeCodeFormat + numberEmployeeCode;
                if (result == null)
                {
                    return "PC00001";
                }
                postgreSQL.Close();
                return result;
            }
        }

        /// <summary>
        /// Thực hiện chức năng xuất excel theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="pays"></param>
        /// <returns>File excel chứa dữ liệu theo điều kiện tìm kiếm</returns>
        /// Author: NVDUC (04/05/2023)
        public async Task<MemoryStream> ExportExcelPay(List<Pay> pays)
        {
            await Task.Yield();
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                // Tạo 1 sheet excel
                var workSheet = package.Workbook.Worksheets.Add("DANH SÁCH CHI TIỀN");
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 15;

                // Dòng 3 là các tên các header
                var rowStart = 3;
                workSheet.Cells[rowStart, 1].Value = "STT";
                workSheet.Cells[rowStart, 2].Value = "Ngày hạch toán";
                workSheet.Cells[rowStart, 3].Value = "Ngày chứng từ";
                workSheet.Cells[rowStart, 4].Value = "Số chứng từ";
                workSheet.Cells[rowStart, 5].Value = "Diễn giải";
                workSheet.Cells[rowStart, 6].Value = "Số tiền";
                workSheet.Cells[rowStart, 7].Value = "Đối tượng";
                workSheet.Cells[rowStart, 8].Value = "Mã đối tượng";
                workSheet.Cells[rowStart, 9].Value = "Địa chỉ";

                // Lọc qua từng đối tượng tương ứng với từng dòng 
                var forLoopIndex = rowStart + 1;
                // Đánh số thứ tự cho cột số thứ tự
                var tableIndex = 1;
                foreach (var pay in pays)
                {
                    workSheet.Cells[forLoopIndex, 1].Value = tableIndex;

                    workSheet.Cells[forLoopIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[forLoopIndex, 2].Style.Numberformat.Format = "dd/mm/yyyy";
                    workSheet.Cells[forLoopIndex, 2].Value = pay.ref_date;

                    workSheet.Cells[forLoopIndex, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[forLoopIndex, 3].Style.Numberformat.Format = "dd/mm/yyyy";
                    workSheet.Cells[forLoopIndex, 3].Value = pay.voucher_date;

  
                    workSheet.Cells[forLoopIndex, 4].Value = pay.voucher_number;

                    workSheet.Cells[forLoopIndex, 5].Value = pay.description;
             
                    workSheet.Cells[forLoopIndex, 6].Value = pay.amount_money;
                    workSheet.Cells[forLoopIndex, 6].Style.Numberformat.Format = "#,##0";

                    workSheet.Cells[forLoopIndex, 7].Value = pay.object_name;

                    workSheet.Cells[forLoopIndex, 8].Value = pay.object_code;

                    workSheet.Cells[forLoopIndex, 9].Value = pay.address;

                    forLoopIndex++;
                    tableIndex++;
                    // Format cho bảng dữ liệu
                    for (var i = 1; i <= 9; i++)
                    {
                        workSheet.Column(i).Style.Font.Name = "Times New Roman";
                        workSheet.Column(i).Style.Font.Size = 11;
                        workSheet.Columns[1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        workSheet.Columns[1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        workSheet.Cells[3, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[tableIndex + 2, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[forLoopIndex, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }
                }

                // Set style cho Title
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Cells["A1:I1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["A1:I1"].Merge = true;
                workSheet.Cells["A1:I1"].Value = "DANH SÁCH CHI TIỀN";
                workSheet.Cells["A1:I1"].Style.Font.Size = 16;
                workSheet.Cells["A1:I1"].Style.Font.Name = "Arial";

                // Dòng thứ 2
                workSheet.Row(2).Height = 20;
                workSheet.Cells["A2:I2"].Merge = true;

                // Set style cho Header
                workSheet.Row(3).Height = 15;
                workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(3).Style.Font.Bold = true;
                workSheet.Cells["A3:I3"].Style.Font.Size = 10;
                workSheet.Cells["A3:I3"].Style.Fill.SetBackground(System.Drawing.Color.LightGray);
                workSheet.Cells["A3:I3"].Style.Font.Name = "Arial";

                // Tạo dòng tổng
                decimal totalAmount = (decimal)pays.Sum(pay => pay.amount_money);
                workSheet.Cells[forLoopIndex, 2].Value = "Tổng";
                workSheet.Cells[forLoopIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[forLoopIndex, 6].Value = totalAmount;
                workSheet.Cells[forLoopIndex, 6].Style.Numberformat.Format = "#,##0";
                workSheet.Cells[forLoopIndex, 1, forLoopIndex, 9].Style.Font.Bold = true;
                workSheet.Cells[$"A{forLoopIndex}:I{forLoopIndex}"].Style.Fill.SetBackground(System.Drawing.Color.LightGray);

                //style cho cột
                for (var i = 1; i <= 9; i++)
                {
                    workSheet.Column(i).AutoFit();
                    workSheet.Column(4).Width = 15;
                    workSheet.Column(5).Width = 30;
                    workSheet.Column(6).Width = 25;
                    workSheet.Column(7).Width = 25;
                    workSheet.Column(7).Width = 20;
                }

                package.Save();
            }
            stream.Position = 0;

            return stream;
        }

        /// <summary>
        /// Thực hiện lấy ra danh sách theo keyword tìm kiếm
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Danh sách</returns>
        /// Author: NVDUC (29/04/2023)
        /// <summary>
        /// Thực hiện lấy ra danh sách theo keyword tìm kiếm
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Danh sách</returns>
        /// Author: NVDUC (29/04/2023)
        public IEnumerable<Pay> GetAllByKey(string? search)
        {
            search ??= "";
            string queryString = $"select * from func_pay_getall_by_key('{search}')";

            using var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString);
            postgreSQL.Open();

            var record = postgreSQL.Query<Pay>(queryString, commandType: System.Data.CommandType.Text);
            postgreSQL.Close();
            return record.ToList();
        }

    }
}
