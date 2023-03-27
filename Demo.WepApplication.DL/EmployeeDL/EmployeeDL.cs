using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using Demo.WepApplication.DL.BaseDL;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var dbConnection = GetOpenConnection();

            // Tương tác với database
            string getMaxCode = "SELECT MAX(e.EmployeeCode) FROM Employee e;";
            string result = dbConnection.QueryFirstOrDefault<string>(getMaxCode, commandType: System.Data.CommandType.Text);
            // Mã nhân viên cuối
            int numberEmployeeCode = Convert.ToInt32(result.Substring(2)) + 1;

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

            result = result.Substring(0, 2) + employeeCodeFormat + numberEmployeeCode;
            if (result == null)
            {
                return "NV00001";
            }
            dbConnection.Close();
            return result;
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
            var dbConnection = GetOpenConnection();

            // Thưc hiện câu lệnh sql
            var multipleResults = dbConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            // Lấy danh sách nhân viên
            var employeeList = multipleResults.Read<Employee>().ToList();
            totalRecord = multipleResults.Read<int>().FirstOrDefault();
            totalPage = multipleResults.Read<int>().FirstOrDefault();


            var result = new PagingResult<Employee>
            {
                Data = employeeList,
                TotalRecord = totalRecord,
                TotalPage = totalPage,
                CurrentPage = pageNumber,
                CurrentPageRecords = pageSize,
            };
            return result;

        }

        /// <summary>
        /// Xoá nhiều nhân viên theo danh sách Id
        /// </summary>
        /// <param name="listEmployeeId"></param>
        /// <returns>Số lượng Id trong danh sách</returns>
        /// Author: NVDUC (25/3/2023)

        public int DeleteMultiple(Guid[] listEmployeeId)
        {
            // Chuẩn bị store
            string deleteMultiple = "DELETE FROM employee WHERE EmployeeId IN @listEmployeeId";
            // Chuẩn bị các tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("@listEmployeeId", listEmployeeId);

            // Khởi tạo kết nối tới database
            var dbConnection = GetOpenConnection();

            int numberRecord = dbConnection.Execute(deleteMultiple, parameters, commandType: System.Data.CommandType.Text);
            dbConnection.Close();
            return numberRecord;
        }

        /// <summary>
        /// Kiểm tra trùng mã nhân viên theo mã nhân viên
        /// </summary>
        /// <returns>Trả về true - trùng mã, false - không trùng</returns>
        /// Author: NVDUC (26/3/2023)
        public override bool CheckDuplicateCode(string employeeCode)
        {
            // Chuẩn bị store
            string checkDuplicateCode = "SELECT e.EmployeeCode FROM Employee e WHERE e.EmployeeCode = @employeeCode";
            // Chuẩn bị các tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("@employeeCode", employeeCode);

            // Khởi tạo kết nối tới database
            var dbConnection = GetOpenConnection();
            var result = dbConnection.QueryFirstOrDefault<string>(checkDuplicateCode, parameters, commandType: System.Data.CommandType.Text);
            if (result != null) {
                return true;
            }
            return false;
        }
        #endregion

    }
}
