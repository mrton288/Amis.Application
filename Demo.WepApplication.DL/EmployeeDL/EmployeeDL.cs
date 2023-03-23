using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WepApplication.DL.EmployeeDL
{
    public class EmployeeDL : IEmployeeDL
    {

        #region Field

        private readonly string _conectionString = "Server=localhost;Port=3306;Database=misa.web202301_mf1562_nvduc;Uid=root;Pwd=123456;";

        #endregion

        #region Method
        /// <summary>
        /// Trả về danh sách nhân viên
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        /// Author: NVDUC (23/3/2023)
        public List<Employee> GetAllEmployee()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy ra thông tin nhân viên
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>Thông tin của nhân viên đó</returns>
        /// Author: NVDUC (23/3/2023)
        public Employee GetEmployeeById(Guid employeeId)
        {
            // Chuẩn bị tên stored
            string storedProcedureName = "Proc_employee_GetById";

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("@v_employeeId", employeeId);

            // Khởi tạo kết nối tới database
            var dbConnection = GetOpenConnection();

            // Thực hiện câu lệnh sql
            var employee = QueryFirstOrDefault(dbConnection, storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            return employee;
        }

        /// <summary>
        /// Thực hiện thêm mới nhân viên
        /// </summary>
        /// <param name="newEmployee"></param>
        /// <returns>Trạng thái của hành động thêm mới</returns>
        /// Author: NVDUC (23/3/2023)
        public int InsertEmployee(Employee newEmployee)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cập nhật thông tin nhân viên theo Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="newEmployee"></param>
        /// <returns>Trạng thái của hành động</returns>
        /// Author: NVDUC (23/3/2023)
        /// 
        public int UpdateEmployeeById(Guid employeeId, Employee newEmployee)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Xoá nhân viên theo Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>Mã trạng thái thành công hay thất bại</returns>
        /// Author: NVDUC (23/3/2023)
        public int DeleteEmployeeById(Guid employeeId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Thực hiện tìm kiếm phân trang danh sách nhân viên
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Các bản ghi trùng với điều kiện</returns>
        /// Author: NVDUC (23/3/2023)
        public PagingResult FilterEmployee(string? search, int pageNumber = 1, int pageSize = 50)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implement hàm mở kết nối từ interface
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IDbConnection GetOpenConnection()
        {
            // Khởi tạo kết nối với DB
            var mySqlConnection = new MySqlConnection(_conectionString);
            mySqlConnection.Open();
            return mySqlConnection;
        }

        /// <summary>
        /// Implement hàm thực hiện câu lệnh sql, kết nối với database từ interface 
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        /// Author: NVDUC (20/3/2023)
        public int Execute(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return cnn.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// Implement hàm thực hiện câu lệnh sql, kết nối với database từ interface 
        /// Query: Dùng cho lấy tất cả bản ghi
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        /// Author: NVDUC (20/3/2023)
        public IEnumerable<Employee> Query(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return cnn.Query<Employee>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        /// <summary>
        /// Implement hàm thực hiện câu lệnh sql, kết nối với database từ interface
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        /// Author: NVDUC (20/3/2023)
        public Employee QueryFirstOrDefault(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return cnn.QueryFirstOrDefault<Employee>(sql, param, transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// Implement hàm thực hiện câu lệnh sql, kết nối với database từ interface
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        /// Author: NVDUC (20/3/2023)
        public SqlMapper.GridReader QueryMultiple(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return cnn.QueryMultiple(sql, param, transaction, commandTimeout, commandType);
        } 
        #endregion

    }
}
