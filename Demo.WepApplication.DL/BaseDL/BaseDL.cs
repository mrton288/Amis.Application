using Dapper;
using Demo.WebApplication.Common.Entities;
using Demo.WebApplication.Common.Entities.DTO;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WepApplication.DL.BaseDL
{
    public class BaseDL<T> : IBaseDL<T>
    {


        #region Method
        /// <summary>
        /// Implement hàm mở kết nối từ interface
        /// </summary>
        /// <returns></returns>
        /// Author: NVDUC (23/3/2023)
        public IDbConnection GetOpenConnection()
        {
            // Khởi tạo kết nối với DB
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);
            mySqlConnection.Open();
            return mySqlConnection;
        }

        /// <summary>
        /// Trả về danh sách bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Author: NVDUC (23/3/2023)
        public IEnumerable<dynamic> GetAllRecord()
        {
            // Chuẩn bị tên stored
            string storedProcedureName = $"Proc_{typeof(T).Name}_GetAll";

            // Chuẩn bị tham số đầu vào
            // Khởi tạo kết nối với DB
            var dbConnection = GetOpenConnection();

            // Thực hiện câu lệnh sql
            var records = dbConnection.Query(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
            dbConnection.Close();
            return records;
        }

        /// <summary>
        /// Lấy ra thông tin bản ghi
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Thông tin của bản ghi đó</returns>
        /// Author: NVDUC (23/3/2023)
        public T GetRecordById(Guid recordId)
        {
            // Chuẩn bị tên stored
            string storedProcedureName = $"Proc_{typeof(T).Name}_GetById";

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"@v_{typeof(T).Name}Id", recordId);

            // Khởi tạo kết nối tới database
            var dbConnection = GetOpenConnection();

            // Thực hiện câu lệnh sql
            var record = dbConnection.QueryFirstOrDefault<T>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            dbConnection.Close();
            return record;
        }

        /// <summary>
        /// Thực hiện thêm mới bản ghi
        /// </summary>
        /// <param name="newRecord"></param>
        /// <returns>Id của bản ghi được thêm mới</returns>
        /// Author: NVDUC (23/3/2023)
        public int InsertRecord(T newRecord)
        {
            // Validate dữ liệu 
            // Chuẩn bị tên stored
            string storedProcedureName = $"Proc_{typeof(T).Name}_Add";

            // Chuẩn bị tham số đầu vào
            var properties = typeof(T).GetProperties();
            var parameters = new DynamicParameters();
            foreach (var property in properties)
            {
                parameters.Add($"@v_{property.Name}", property.GetValue(newRecord));
            }

            // Khởi tạo kết nối với DB
            var dbConnection = GetOpenConnection();
            // Thực hiện câu lệnh sql
            var numberRecord = dbConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            dbConnection.Close();
            return numberRecord;
        }

        /// <summary>
        /// Cập nhật thông tin bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="newRecord"></param>
        /// <returns>Trả về số bản ghi bị ảnh hưởng</returns>
        /// Author: NVDUC (23/3/2023)
        /// 
        public int UpdateRecordById(Guid recordId, T newRecord)
        {
            // Chuẩn bị tên stored
            string storedProcedureName = $"Proc_{typeof(T).Name}_UpdateById";

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                parameters.Add($"@v_{property.Name}", property.GetValue(newRecord));

            }
            // Khởi tạo kết nối với DB
            var dbConnection = GetOpenConnection();

            // Thực hiện câu lệnh sql
            var numberRecord = dbConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            dbConnection.Close();

            return numberRecord;
        }

        /// <summary>
        /// Xoá bản ghi theo Id
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Mã trạng thái thành công hay thất bại</returns>
        /// Author: NVDUC (23/3/2023)
        public int DeleteRecordById(Guid recordId)
        {
            //Chuẩn bị tên stored
            string storedProcedureName = $"Proc_{typeof(T).Name}_DeleteById";

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"@v_{typeof(T).Name}Id", recordId);

            // Khởi tạo kết nối với DB
            var dbConnection = GetOpenConnection();
            var result = dbConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            dbConnection.Close();
            return result;
        }

        /// <summary>
        /// Kiểm tra trùng mã nhân viên theo mã nhân viên
        /// </summary>
        /// <returns>Trả về true - trùng mã, false - không trùng</returns>
        /// Author: NVDUC (26/3/2023)
        public virtual bool CheckDuplicateCode(string recordCode)
        {
            return false;
        }
        #endregion

        /* /// <summary>
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
         public IEnumerable<T> Query(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
         {
             return cnn.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
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
         public T QueryFirstOrDefault(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
         {
             return cnn.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
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
         }*/
    }
}
