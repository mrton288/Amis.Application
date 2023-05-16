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
using System.Reflection;
using Demo.WebApplication.Common.Entities.DTO;
using System.Data;
using System.Drawing.Printing;
using Demo.WepApplication.DL.AccountDL;

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
        public async Task<MemoryStream> ExportExcelPay(string? search)
        {
            try
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
                    IEnumerable<dynamic> pays = GetAllByKey(search);
                    decimal totalAmount = 0;
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

                        if (pay.total_amount != null)
                        {
                            workSheet.Cells[forLoopIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            workSheet.Cells[forLoopIndex, 6].Value = pay.total_amount.ToString("#,##0").Replace(".", ",");
                        }

                        workSheet.Cells[forLoopIndex, 7].Value = pay.object_name;

                        workSheet.Cells[forLoopIndex, 8].Value = pay.supplier_code;

                        workSheet.Cells[forLoopIndex, 9].Value = pay.address;

                        forLoopIndex++;
                        tableIndex++;
                        if (pay.total_amount != null)
                        {
                            totalAmount += pay.total_amount;
                        }
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

                    //Tạo dòng tổng
                    workSheet.Cells[forLoopIndex, 2].Value = "Tổng";
                    workSheet.Cells[forLoopIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[forLoopIndex, 6].Value = totalAmount.ToString("#,##0").Replace(".", ",");
                    workSheet.Cells[forLoopIndex, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
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
            catch (Exception)
            {
                throw;
            }
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
        public IEnumerable<dynamic> GetAllByKey(string? search)
        {
            search ??= "";
            string selectOption = "p.pay_id, p.ref_date, p.voucher_date, p.voucher_number, p.description, p.receiver, p.quantity, " +
                "p.employee_id, p.object_id , s.supplier_code, COALESCE(p.object_name, s.supplier_name) AS object_name, COALESCE(p.address, s.address) AS address , COALESCE(p.receiver, s.supplier_name) AS receiver, " +
                "(SELECT SUM(amount_money) FROM paydetail pd WHERE pd.pay_id = p.pay_id) AS total_amount";
            string whereClause = "where p.description ilike ('%' || @search  || '%') " +
                "or cast(p.ref_date as text) ilike ('%' || @search || '%') " +
                "or cast(p.voucher_date as text) ilike ('%' || @search || '%') " +
                "or p.voucher_number ilike ('%' || @search  || '%') " +
                "or p.description ilike ('%' || @search  || '%')" +
                "or p.object_name ilike ('%' || @search  || '%') " +
                "or s.supplier_code ilike ('%' || @search  || '%') " +
                "or p.address ilike ('%' || @search  || '%')";
            string queryString = $"select {selectOption}  from pay p left join supplier s on p.object_id = s.supplier_id left join employee e on e.employee_id = p.employee_id {whereClause} order by p.voucher_number desc;";
            using (var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString))
            {
                postgreSQL.Open();
                // Thực hiện truy vấn
                var result = postgreSQL.Query<dynamic>(queryString, new { search }, commandType: CommandType.Text);
                postgreSQL.Close();
                return result;
            }
        }

        /// <summary>
        /// Thực hiện xoá bản ghi ở Table Pay đồng thời 
        /// lấy ra và xoá bản ghi ở table Paydetail
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// Author: NVDUC (11/05/2023)
        public int DeleteFullMultiple(Guid[]? ids)
        {
            int masterEffected = 0;
            int detailEffected = 0;
            using (var postgreSQL = new NpgsqlConnection(DatabaseContext.ConnectionString))
            {
                postgreSQL.Open();
                using (var transaction = postgreSQL.BeginTransaction())
                {
                    try
                    {
                        string sql = $"DELETE FROM paydetail WHERE pay_id = ANY(:ids);";
                        using (var command = new NpgsqlCommand(sql, postgreSQL))
                        {
                            command.Transaction = transaction;
                            command.Parameters.AddWithValue("ids", ids);
                            detailEffected = command.ExecuteNonQuery();
                        }

                        string sqlPay = $"DELETE FROM pay WHERE pay_id = ANY(:ids);";
                        using (var command = new NpgsqlCommand(sqlPay, postgreSQL))
                        {
                            command.Transaction = transaction;
                            command.Parameters.AddWithValue("ids", ids);
                            masterEffected = command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            return masterEffected + detailEffected;
        }

        /// <summary>
        /// Thực hiện xoá bản ghi ở Table Pay đồng thời 
        /// lấy ra và xoá bản ghi ở table Paydetail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Author: NVDUC (11/05/2023)
        public int DeleteMultiple(Guid id)
        {
            string queryDetail = $"Delete from paydetail where paydetail.pay_id = '{id}';";
            string queryMaster = $"Delete from pay where pay.pay_id = '{id}';";
            int masterEffected = 0;
            int detailEffected = 0;

            // Kết nối tới db
            using var postgresql = new NpgsqlConnection(DatabaseContext.ConnectionString);

            postgresql.Open();
            // Thực hiện query
            masterEffected = postgresql.Execute(queryDetail, commandType: System.Data.CommandType.Text);
            detailEffected = postgresql.Execute(queryMaster, commandType: System.Data.CommandType.Text);

            postgresql.Close();
            return masterEffected + detailEffected;
        }

        /// <summary>
        /// Check trùng số phiếu chi
        /// </summary>
        /// <param name="voucherNumber">Số phiếu chi</param>
        /// <param name="payId">Id pay</param>
        /// <returns>Trả về true - trùng mã, false - không trùng</returns>
        /// Author: NVDUC (29/4/2023)
        public bool CheckDuplicateVoucherNumber(string voucherNumber, Guid payId)
        {
            // Chuẩn bị store
            string stringFunction = $"select voucher_number, pay_id from pay WHERE voucher_number = @VoucherNumber AND pay_id != @PayId";
            // Chuẩn bị các tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"@VoucherNumber", voucherNumber);
            parameters.Add($"@PayId", payId);

            // Khởi tạo kết nối tới database
            using var postgreSql = new NpgsqlConnection(DatabaseContext.ConnectionString);
            postgreSql.Open();
            var result = postgreSql.QueryFirstOrDefault<string>(stringFunction, parameters, commandType: System.Data.CommandType.Text);
            if (result != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Thực hiện tạo ra câu query cho Pay
        /// </summary>
        /// <returns></returns>
        /// Author: NVDUC (29/04/2023)
        public override object BuildQueryCustom()
        {
            string select = "pay.pay_id, pay.ref_date, pay.voucher_date, pay.voucher_number, pay.description, pay.quantity, " +
                "employee.employee_id, pay.object_id , supplier.supplier_code, COALESCE(pay.object_name, supplier.supplier_name) AS object_name, COALESCE(pay.address, supplier.address) AS address, COALESCE(pay.receiver, supplier.supplier_name) AS receiver, " +
                "(SELECT SUM(amount_money) FROM paydetail pd WHERE pd.pay_id = pay.pay_id) AS total_amount";
            string join = "left join supplier on object_id = supplier_id left join employee on employee.employee_id = pay.employee_id";
            string whereClause = "where pay.description ilike ('%' || @search  || '%') " +
                "or cast(pay.ref_date as text) ilike ('%' || @search || '%') " +
                "or cast(pay.voucher_date as text) ilike ('%' || @search || '%') " +
                "or pay.voucher_number ilike ('%' || @search  || '%') " +
                "or pay.description ilike ('%' || @search  || '%')" +
                "or pay.object_name ilike ('%' || @search  || '%') " +
                "or supplier.supplier_code ilike ('%' || @search  || '%') " +
                "or pay.address ilike ('%' || @search  || '%')";
            return new
            {
                selectOption = select,
                joinOption = join,
                orderBy = "pay.voucher_number",
                search = whereClause,
            };
        }
    }
}
