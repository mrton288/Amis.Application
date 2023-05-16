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
    public class EmployeeDL : BaseDL<Employee>, IPayDetailDL
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
                };
                mySqlConnection.Close();
                return result;
            }
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

        /// <summary>
        /// Thực hiện tạo ra câu query cho Employee
        /// </summary>
        /// <returns></returns>
        /// Author: NVDUC (29/04/2023)
        public override object BuildQueryCustom()
        {
            string whereClause = "where employee.employee_code ilike ('%' || @search  || '%') " +
                "or employee.full_name ilike ('%' || @search  || '%') " +
                "or employee.position_name ilike ('%' || @search  || '%') " +
                "or employee.phone_number ilike ('%' || @search  || '%') ";
            return new
            {
                selectOption = "*",
                joinOption = "",
                orderBy = "employee.employee_code",
                search = whereClause,
            };
        }
    }
    #endregion

}
