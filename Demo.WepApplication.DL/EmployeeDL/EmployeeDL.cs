using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WebApplication.Common.Enums;
using Demo.WepApplication.DL.BaseDL;
using Microsoft.AspNetCore.Http;
using MySqlConnector;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;

namespace Demo.WepApplication.DL.EmployeeDL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {

        #region Method
        /// <summary>
        /// Sinh ra mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Author: NVDUC (24/3/2023)
        public string GetNewCode()
        {
            // Khởi tạo kết nối tới database
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                mySqlConnection.Open();

                // Tương tác với database
                string getMaxCode = "Proc_employee_GetMaxCode";

                string result = mySqlConnection.QueryFirstOrDefault<string>(getMaxCode, commandType: System.Data.CommandType.StoredProcedure);
                // Mã nhân viên cuối
                int numberEmployeeCode = Convert.ToInt32(result.Substring(3)) + 1;

                // Format lại mã nhân viên cuối nếu không đủ 5 số
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

                result = result.Substring(0, 3) + employeeCodeFormat + numberEmployeeCode;
                if (result == null)
                {
                    return "NV-00001";
                }
                mySqlConnection.Close();
                return result;
            }
        }

        /// <summary>
        /// Thực hiện tìm kiếm phân trang danh sách bản ghi
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Các bản ghi trùng với điều kiện</returns>
        /// Author: NVDUC (23/3/2023)
        public PagingResult<Employee> GetPaging(string? search, int? pageNumber, int? pageSize)
        {
            int totalRecord = 0;
            int totalPage = 0;
            // Chuẩn bị tên storeProcedure
            string storedProcedureName = "Proc_employee_GetPaging";

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("@v_pageNumber", pageNumber);
            parameters.Add("@v_pageSize", pageSize);
            parameters.Add("@v_search", search);

            // Lấy tổng số bản ghi
            parameters.Add("@v_totalRecord", totalRecord);
            // Lấy tổng số trang
            parameters.Add("@v_totalPage", totalPage);

            // Khởi tạo kết nối với DB
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                mySqlConnection.Open();

                // Thưc hiện câu lệnh sql
                var multipleResults = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Lấy danh sách nhân viên
                var employeeList = multipleResults.Read<Employee>().ToList();
                totalRecord = multipleResults.Read<int>().FirstOrDefault();
                totalPage = multipleResults.Read<int>().FirstOrDefault();

                var result = new PagingResult<Employee>
                {
                    ListRecord = employeeList,
                    TotalRecord = totalRecord,
                    TotalPage = totalPage,
                    CurrentPage = pageNumber,
                    CurrentPageRecords = pageSize,
                };
                mySqlConnection.Close();
                return result;
            }
        }

        /// <summary>
        /// Xoá nhiều nhân viên theo danh sách Id
        /// </summary>
        /// <param name="listEmployeeId"></param>
        /// <returns>Số lượng Id trong danh sách</returns>
        /// Author: NVDUC (25/3/2023)
        public int DeleteMultiple(Guid[] listEmployeeId)
        {
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Khởi tạo kết nối với DB
                mySqlConnection.Open();
                using (MySqlTransaction transaction = mySqlConnection.BeginTransaction())
                {
                    try
                    {
                        // Chuẩn bị store
                        string deleteMultiple = "DELETE FROM employee WHERE EmployeeId IN @listEmployeeId";
                        // Chuẩn bị các tham số đầu vào
                        var parameters = new DynamicParameters();
                        parameters.Add("@listEmployeeId", listEmployeeId);

                        int numberRecord = mySqlConnection.Execute(deleteMultiple, parameters, transaction, commandType: System.Data.CommandType.Text);

                        transaction.Commit();
                        mySqlConnection.Close();
                        return numberRecord;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }
        }

        /// <summary>
        /// Thực hiện chức năng xuất excel toàn bộ dữ liệu 
        /// </summary>
        /// <param name="employees">Danh sách nhân viên</param>
        /// <returns>File excel chứa toàn bộ dữ liệu</returns>
        /// Author: NVDUC (1/4/2023)
        public async Task<MemoryStream> ExportExcelEmployee(List<Employee> employees)
        {
            await Task.Yield();
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                // Tạo 1 sheet excel
                var workSheet = package.Workbook.Worksheets.Add("DANH SÁCH NHÂN VIÊN");
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 15;

                // Dòng 3 là các tên các header
                var rowStart = 3;
                workSheet.Cells[rowStart, 1].Value = "STT";
                workSheet.Cells[rowStart, 2].Value = "Mã nhân viên";
                workSheet.Cells[rowStart, 3].Value = "Tên nhân viên";
                workSheet.Cells[rowStart, 4].Value = "Giới tính";
                workSheet.Cells[rowStart, 5].Value = "Ngày sinh";
                workSheet.Cells[rowStart, 6].Value = "Số CMND";
                workSheet.Cells[rowStart, 7].Value = "Ngày cấp";
                workSheet.Cells[rowStart, 8].Value = "Nơi cấp";
                workSheet.Cells[rowStart, 9].Value = "Tên đơn vị";
                workSheet.Cells[rowStart, 10].Value = "Chức danh";
                workSheet.Cells[rowStart, 11].Value = "Địa chỉ";
                workSheet.Cells[rowStart, 12].Value = "Điện thoại di động";
                workSheet.Cells[rowStart, 13].Value = "Điện thoại cố định";
                workSheet.Cells[rowStart, 14].Value = "Email";
                workSheet.Cells[rowStart, 15].Value = "Tài khoản ngân hàng";
                workSheet.Cells[rowStart, 16].Value = "Tên ngân hàng";
                workSheet.Cells[rowStart, 17].Value = "Chi nhánh";

                // Lọc qua từng đối tượng tương ứng với từng dòng 
                var forLoopIndex = rowStart + 1;
                // Đánh số thứ tự cho cột số thứ tự
                var tableIndex = 1;
                foreach (var employee in employees)
                {
                    workSheet.Cells[forLoopIndex, 1].Value = tableIndex;
                    workSheet.Cells[forLoopIndex, 2].Value = employee.EmployeeCode;
                    workSheet.Cells[forLoopIndex, 3].Value = employee.FullName;
                    if (employee.Gender == Gender.MALE)
                    {
                        workSheet.Cells[forLoopIndex, 4].Value = "Nam";
                    }
                    else if (employee.Gender == Gender.FEMALE)
                    {
                        workSheet.Cells[forLoopIndex, 4].Value = "Nữ";
                    }
                    else
                    {
                        workSheet.Cells[forLoopIndex, 4].Value = "Khác";
                    }
                    workSheet.Cells[forLoopIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[forLoopIndex, 5].Style.Numberformat.Format = "dd/mm/yyyy";
                    workSheet.Cells[forLoopIndex, 5].Value = employee.DateOfBirth;

                    workSheet.Cells[forLoopIndex, 6].Value = employee.IdentityNumber;

                    workSheet.Cells[forLoopIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[forLoopIndex, 7].Style.Numberformat.Format = "dd/mm/yyyy";
                    workSheet.Cells[forLoopIndex, 7].Value = employee.IdentityDate;

                    workSheet.Cells[forLoopIndex, 8].Value = employee.IdentityPlace;
                    workSheet.Cells[forLoopIndex, 9].Value = employee.DepartmentName;
                    workSheet.Cells[forLoopIndex, 10].Value = employee.PositionName;
                    workSheet.Cells[forLoopIndex, 11].Value = employee.Address;
                    workSheet.Cells[forLoopIndex, 12].Value = employee.PhoneNumber;
                    workSheet.Cells[forLoopIndex, 13].Value = employee.Landline;
                    workSheet.Cells[forLoopIndex, 14].Value = employee.Email;
                    workSheet.Cells[forLoopIndex, 15].Value = employee.BankAccount;
                    workSheet.Cells[forLoopIndex, 16].Value = employee.BankName;
                    workSheet.Cells[forLoopIndex, 17].Value = employee.BankBranch;

                    forLoopIndex++;
                    tableIndex++;
                    // Format cho bảng dữ liệu
                    for (var i = 1; i <= 17; i++)
                    {
                        workSheet.Column(i).Style.Font.Name = "Times New Roman";
                        workSheet.Column(i).Style.Font.Size = 11;
                        workSheet.Columns[1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        workSheet.Columns[1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        workSheet.Cells[3, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[tableIndex + 2, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    }
                }

                // Set style cho Title
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Cells["A1:Q1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["A1:Q1"].Merge = true;
                workSheet.Cells["A1:Q1"].Value = "DANH SÁCH NHÂN VIÊN";
                workSheet.Cells["A1:Q1"].Style.Font.Size = 16;
                workSheet.Cells["A1:Q1"].Style.Font.Name = "Arial";

                // Dòng thứ 2
                workSheet.Row(2).Height = 20;
                workSheet.Cells["A2:Q2"].Merge = true;

                // Set style cho Header
                workSheet.Row(3).Height = 15;
                workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(3).Style.Font.Bold = true;
                workSheet.Cells["A3:Q3"].Style.Font.Size = 10;
                workSheet.Cells["A3:Q3"].Style.Fill.SetBackground(System.Drawing.Color.LightGray);
                workSheet.Cells["A3:Q3"].Style.Font.Name = "Arial";

                //style cho cột
                for (var i = 1; i <= 17; i++)
                {
                    workSheet.Column(i).AutoFit();
                    workSheet.Column(5).Width = 14;
                    workSheet.Column(6).Width = 16;
                    workSheet.Column(7).Width = 14;
                }

                package.Save();
            }
            stream.Position = 0;

            return stream;
        }

        /// <summary>
        /// Kiểm tra trùng mã nhân viên
        /// </summary>
        /// <param name="employeeCode">Mã nhân viên</param>
        /// <param name="employeeId">Id nhân viên</param>
        /// <returns>Trả về true - trùng mã, false - không trùng</returns>
        /// Author: NVDUC (26/3/2023)
        public bool CheckDuplicateCode(string employeeCode, Guid employeeId)
        {
            // Chuẩn bị store
            string checkDuplicateCode = $"SELECT EmployeeCode, EmployeeId FROM Employee  WHERE EmployeeCode = @EmployeeCode AND EmployeeId != @EmployeeId";
            // Chuẩn bị các tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"@EmployeeCode", employeeCode);
            parameters.Add($"@EmployeeId", employeeId);

            // Khởi tạo kết nối tới database
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                mySqlConnection.Open();
                var result = mySqlConnection.QueryFirstOrDefault<string>(checkDuplicateCode, parameters, commandType: System.Data.CommandType.Text);
                if (result != null)
                {
                    return true;
                }
                return false;
            }
        }
    }
    #endregion

}
