using Dapper;
using Demo.WebApplication.API.Entities;
using MySqlConnector;
using System.Data;

namespace Demo.WebApplication.API.Database
{
    public class DepartmentRepository : IDepartmentRepository
    {
        #region Field

        private readonly string _conectionString = "Server=localhost;Port=3306;Database=misa.web202301_mf1562_nvduc;Uid=root;Pwd=123456;";
        #endregion

        #region Method
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
        public IEnumerable<Department> Query(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return cnn.Query<Department>(sql, param, transaction, buffered, commandTimeout, commandType);

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
        public Department QueryFirstOrDefault(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return cnn.QueryFirstOrDefault<Department>(sql, param, transaction, commandTimeout, commandType);
        }
        #endregion
    }   
}
