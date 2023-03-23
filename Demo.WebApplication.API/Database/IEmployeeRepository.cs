using Dapper;
using Demo.WebApplication.API.Entities;
using System.Data;

namespace Demo.WebApplication.API.Database
{
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Thay thế cho việc khởi tạo new MySqlConnection
        /// </summary>
        /// <returns></returns>
        /// Author: NVDUC (20/3/2023)
        public IDbConnection GetOpenConnection();

        /// <summary>
        /// Thay thế cho việc khởi tạo Employee
        /// </summary>
        /// <returns></returns>
        /// Author: NVDUC (20/3/2023)
        public Employee QueryFirstOrDefault(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Thực thi câu lệnh sql và thay thế cho việc khởi tạo Employee
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        /// Author: NVDUC (20/3/2023)
        public IEnumerable<Employee> Query(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Thực thi câu lệnh sql và thay thế cho việc khởi tạo Employee
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        /// Author: NVDUC (20/3/2023)
        public int Execute(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Thực thi câu lệnh sql, trả về các đầu dữ liệu
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        /// Author: NVDUC (20/3/2023)
        public SqlMapper.GridReader QueryMultiple(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    }
}
